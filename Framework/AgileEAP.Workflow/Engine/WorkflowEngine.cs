using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 工作流引擎
    /// </summary>
    public class WorkflowEngine : IWorkflowEngine
    {
        #region Variables
        ILogger log = LogManager.GetLogger(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType);
        IEngineService engineService = null;
        private IDictionary<string, ProcessInst> runtimeProcessInsts = new Dictionary<string, ProcessInst>();
        private IRepository<string> repository = null;// new Repository<string>();
        #endregion

        #region Properties

        /// <summary>
        /// 运行中的流程实例
        /// </summary>
        public IDictionary<string, ProcessInst> RuntimeProcessInsts
        {
            get
            {
                return runtimeProcessInsts;
            }
            set
            {
                runtimeProcessInsts = value;
            }
        }
        /// <summary>
        /// 流程数据接口
        /// </summary>
        public IWorkflowPersistence Persistence
        {
            get { return engineService.Persistence; }
        }

        public INotification Notification
        {
            get { return engineService.Notification; }
        }
        public IWorkAssign WorkAssign
        {
            get { return engineService.WorkAssign; }
        }

        #endregion

        #region Constructor

        public WorkflowEngine()
        {
            engineService = new EngineService();
            repository = engineService.Persistence.Repository;
        }

        #endregion

        #region IWorkflowEngine

        #region 工作流管理

        /// <summary>
        /// 清除流程缓存
        /// </summary>
        public void ClearProcessCache()
        {
            CacheManager.Remove("all_ProcessDefine");
        }

        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public string CreateAProcess(string processDefID)
        {
            return CreateAProcess(processDefID, string.Empty, string.Empty);
        }

        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="parentProcessDefID">父流程ID</param>
        /// <param name="parentActivityID">父活动ID</param>
        /// <returns></returns>
        public string CreateAProcess(string processDefID, string parentProcessDefID, string parentActivityID)
        {
            ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);
            if (processDef == null)
            {
                WFUtil.HandleException(new MessageException()
                {
                    PromptMessage = string.Format("ID={0}的流程不存在", processDefID),
                    Source = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                });

                return string.Empty;
            }

            if (processDef.CurrentState == (short)ProcessStatus.Candidate)
            {
                WFUtil.HandleException(new MessageException()
                {
                    PromptMessage = string.Format("ID={0}的流程还未发布，不能创建流程", processDefID),
                    Source = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                });

                return string.Empty;
            }

            DateTime startTime = DateTime.Now.AddSeconds(-2);
            ProcessDefine define = GetProcessDefine(processDefID);

            ProcessInst processInst = new ProcessInst()
            {
                ID = IdGenerator.NewComb().ToString(),
                ProcessDefID = processDefID,
                CreateTime = DateTime.Now,
                Creator = WFUtil.User.ID,
                CurrentState = (short)ProcessInstStatus.NoStart,
                IsTimeOut = 0,
                Name = processDef.Text,
                Description = processDef.Description,
                ParentProcessID = parentProcessDefID,
                ParentActivityID = parentActivityID,
                StartTime = startTime,
                EndTime = WFUtil.MaxDate,
                FinalTime = WFUtil.MaxDate,
                LimitTime = WFUtil.MaxDate,
                ProcessDefName = processDef.Text,
                ProcessVersion = processDef.Version,
                RemindTime = WFUtil.MaxDate,
                TimeOutTime = WFUtil.MaxDate
            };

            Activity ad = Persistence.GetStartActivity(processDefID);
            ActivityInst activityInst = new ActivityInst()
            {
                ID = IdGenerator.NewComb().ToString(),
                ActivityDefID = ad.ID,
                CreateTime = DateTime.Now,
                CurrentState = (short)ActivityInstStatus.Compeleted,
                Description = ad.Description,
                EndTime = WFUtil.MaxDate,
                Name = string.Format("启动{0}", processDef.Text),
                ProcessInstID = processInst.ID,
                RollbackFlag = 0,
                StartTime = startTime,
                SubProcessInstID = string.Empty,
                Type = (short)ActivityType.StartActivity
            };

            UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
            {
                repository.SaveOrUpdate(processInst);
                repository.SaveOrUpdate(activityInst);

                if (processDef.IsActive != 1)
                {
                    processDef.IsActive = 1;
                    repository.SaveOrUpdate(processDef);
                }
            });

            return processInst.ID;
        }

        /// <summary>
        /// 开始启动一个流程
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        ///<param name="parameters">启动参数</param>
        public void StartAProcess(string processInstID, IDictionary<string, object> parameters)
        {
            ProcessInst processInst = GetToRunProcessInst(processInstID, WFUtil.User);
            ProcessDefine processDefine = Persistence.GetProcessDefine(processInst.ProcessDefID);
            ProcessContext processContext = new ProcessContext()
            {
                ProcessDefine = processDefine,
                ProcessInst = processInst,
                Parameters = parameters
            };

            ProcessRuntime runtime = new ProcessRuntime(engineService, processContext, null);
            runtime.Run();
        }

        /// <summary>
        /// 开始启动一个流程
        /// </summary>
        /// <param name="processInstID">启动</param>
        public void StartAProcess(string processInstID)
        {
            StartAProcess(processInstID, null);
        }

        /// <summary>
        /// 获取一个流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        private ProcessInst GetToRunProcessInst(string processInstID, IUser user)
        {
            ProcessInst processInst = Persistence.GetProcessInst(processInstID);
            ProcessDefine processDefine = GetProcessDefine(processInst.ProcessDefID);
            string processDefID = processInst.ProcessDefID;

            //如果流程还未启动，产生一个活动实例
            if (processInst.CurrentState == (short)ProcessInstStatus.NoStart)
            {
                Activity srcActivity = processDefine.StartActivity;
                ActivityInst srcActInst = Persistence.GetActivityInsts(processInstID, srcActivity.ID, ActivityInstStatus.Compeleted)[0];
                IList<Activity> destActivities = Persistence.GetOutActivities(processDefID, srcActivity.ID);

                UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
                {
                    foreach (var destActivity in destActivities)
                    {
                        DateTime startTime = DateTime.Now.AddSeconds(-1);
                        ActivityInst destActInst = new ActivityInst()
                        {
                            ActivityDefID = destActivity.ID,
                            CurrentState = (short)ActivityInstStatus.NoStart,
                            ID = IdGenerator.NewComb().ToString(),
                            CreateTime = startTime,
                            Description = destActivity.Description,
                            EndTime = WFUtil.MaxDate,
                            Name = destActivity.Name,
                            ProcessInstID = processInstID,
                            RollbackFlag = 0,
                            StartTime = startTime,
                            SubProcessInstID = string.Empty,
                            Type = (short)destActivity.ActivityType.Cast<ActivityType>(ActivityType.ManualActivity)
                        };

                        Domain.Transition transition = new Domain.Transition()
                        {
                            ID = IdGenerator.NewComb().ToString(),
                            DestActID = destActivity.ID,
                            DestActInstID = destActInst.ID,
                            DestActInstName = destActInst.Name,
                            DestActName = destActivity.Name,
                            ProcessInstID = processInstID,
                            ProcessInstName = processInst.Name,
                            SrcActID = srcActivity.ID,
                            SrcActInstID = srcActInst.ID,
                            SrcActInstName = srcActInst.Name,
                            SrcActName = srcActivity.Name,
                            TransTime = startTime
                        };

                        TransControl transControl = new TransControl()
                        {
                            ID = IdGenerator.NewComb().ToString(),
                            DestActID = destActivity.ID,
                            DestActName = destActivity.Name,
                            ProcessInstID = processInst.ID,
                            SrcActID = srcActivity.ID,
                            SrcActName = srcActivity.Name,
                            TransTime = startTime,
                            TransWeight = 100
                        };


                        if (destActivity is ManualActivity)
                        {
                            ManualActivity ma = destActivity as ManualActivity;
                            WorkItem wi = new WorkItem()
                            {
                                ID = IdGenerator.NewComb().ToString(),
                                ActionMask = string.Empty,
                                ActionURL = ma.CustomURL.SpecifyURL,
                                ActivityInstID = destActInst.ID,
                                ActivityInstName = destActInst.Name,
                                AllowAgent = (short)(ma.AllowAgent ? 1 : 0),
                                BizState = (short)WorkItemBizStatus.Common,
                                CreateTime = startTime,
                                Creator = user.ID,
                                CreatorName = user.Name,
                                CurrentState = (short)WorkItemStatus.WaitExecute,
                                Description = destActInst.Description,
                                EndTime = WFUtil.MaxDate,
                                Executor = string.Empty,
                                ExecutorName = string.Empty,
                                IsTimeOut = 0,
                                Name = destActInst.Name,
                                Participant = string.Empty,
                                ProcessID = processDefID,
                                ProcessInstID = processInst.ID,
                                ProcessInstName = processInst.Name,
                                ProcessName = processDefine.Name,
                                RemindTime = WFUtil.MaxDate,
                                RootProcessInstID = string.Empty,
                                StartTime = startTime,
                                TimeOutTime = WFUtil.MaxDate,
                                Type = destActInst.Type
                            };

                            //foreach (var participantor in ma.Participant.Participantors)
                            //{
                            //    Domain.Participant participant = new Domain.Participant()
                            //    {
                            //        ID = IdGenerator.NewComb().ToString(),
                            //        CreateTime = createTime,
                            //        DelegateType = (short)DelegateType.Sponsor,
                            //        Name = participantor.Name,
                            //        ParticipantID = participantor.ID,
                            //        ParticipantIndex = participantor.SortOrder,
                            //        ParticipantType = (short)participantor.ParticipantorType.Cast<ParticipantorType>(ParticipantorType.Person),
                            //        PartiInType = (short)PartiInType.Exe,
                            //        WorkItemID = wi.ID,
                            //        WorkItemState = (short)wi.CurrentState
                            //    };

                            //    repository.SaveOrUpdate(participant);
                            //}

                            Domain.Participant participant = new Domain.Participant()
                            {
                                ID = IdGenerator.NewComb().ToString(),
                                CreateTime = startTime,
                                DelegateType = (short)DelegateType.Sponsor,
                                Name = user.Name,
                                ParticipantID = user.ID,
                                ParticipantIndex = 1,
                                ParticipantType = (short)ParticipantorType.Person,
                                PartiInType = (short)PartiInType.Exe,
                                WorkItemID = wi.ID,
                                WorkItemState = (short)wi.CurrentState
                            };

                            repository.SaveOrUpdate(participant);

                            repository.SaveOrUpdate(wi);
                        }

                        srcActInst.CurrentState = (short)ActivityInstStatus.Compeleted;
                        repository.SaveOrUpdate(srcActInst);
                        repository.SaveOrUpdate(destActInst);
                        repository.SaveOrUpdate(transition);
                        repository.SaveOrUpdate(transControl);
                    }
                });
            }

            return processInst;
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public ProcessDefine GetProcessDefine(string processDefID)
        {
            return Persistence.GetProcessDefine(processDefID);
        }
        #endregion

        #region 流程实例管理
        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void SuspendProcessInst(string processInstID)
        {
            UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
            {
                var processInst = repository.GetDomain<ProcessInst>(processInstID);
                repository.ExecuteNonQuery<ActivityInst>(string.Format("Update WF_ActivityInst Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2} and RollbackFlag=0", (short)ActivityInstStatus.Suspended, processInstID, (short)ActivityInstStatus.Running));
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2}", (short)WorkItemStatus.Suspended, processInstID, (short)WorkItemStatus.WaitExecute));
                processInst.CurrentState = (short)ProcessInstStatus.Suspended;
                repository.SaveOrUpdate(processInst);
            });
        }

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void ResumeProcessInst(string processInstID)
        {
            UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
            {
                var processInst = repository.GetDomain<ProcessInst>(processInstID);
                repository.ExecuteNonQuery<ActivityInst>(string.Format("Update WF_ActivityInst Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2} and RollbackFlag=0", (short)ActivityInstStatus.Running, processInstID, (short)ActivityInstStatus.Suspended));
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2}", (short)WorkItemStatus.WaitExecute, processInstID, (short)WorkItemStatus.Suspended));
                processInst.CurrentState = (short)ProcessInstStatus.Running;
                repository.SaveOrUpdate(processInst);
            });
        }

        /// <summary>
        /// 终止流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void TerminateProcessInst(string processInstID)
        {
            UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
            {
                var processInst = repository.GetDomain<ProcessInst>(processInstID);
                repository.ExecuteNonQuery<ActivityInst>(string.Format("Update WF_ActivityInst Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2} and RollbackFlag=0", (short)ActivityInstStatus.Terminated, processInstID, (short)ActivityInstStatus.Running));
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ProcessInstID='{1}' and CurrentState={2}", (short)WorkItemStatus.Terminated, processInstID, (short)WorkItemStatus.WaitExecute));
                processInst.CurrentState = (short)ProcessInstStatus.Terminated;
                repository.SaveOrUpdate(processInst);
            });
        }

        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void DeleteProcessInst(string processInstID)
        {
            UnitOfWork.ExecuteWithTrans<ProcessInst>(() =>
            {
                //repository.ExecuteNonQuery<AgileEAP.Workflow.Domain.TransControl>(string.Format("Delete From WF_TransControl Where ProcessInstID={0}", processInstID));
                //repository.ExecuteNonQuery<AgileEAP.Workflow.Domain.Transition>(string.Format("Delete From WF_Transition Where ProcessInstID={0}", processInstID));
                //repository.ExecuteNonQuery<WorkItem>(string.Format("Delete From WF_WorkItem Where ProcessInstID={0}", processInstID));
                //repository.ExecuteNonQuery<ActivityInst>(string.Format("Delete From WF_ActivityInst Where ProcessInstID={0}", processInstID));

                IDictionary<string, object> parameters = ParameterBuilder.BuildParameters().SafeAdd("ProcessInstID", processInstID);
                repository.Delete<TransControl>(parameters);
                repository.Delete<Domain.Transition>(parameters);
                repository.Delete<WorkItem>(parameters);
                repository.Delete<ActivityInst>(parameters);

                repository.Delete<ProcessInst>(processInstID);
            });
        }
        #endregion

        #region 活动实例管理
        /// <summary>
        /// 完成活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void CompleteActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                ActivateActivityInst(activityInstID);
                foreach (var wi in repository.Query<WorkItem>().Where(wi => wi.ActivityInstID == activityInstID && (wi.CurrentState == (short)WorkItemStatus.WaitExecute || wi.CurrentState == (short)WorkItemStatus.Suspended)).ToArray())
                {
                    CompleteWorkItem(wi.ID, null);
                }
            });
        }

        /// <summary>
        /// 重启活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ResetActivityInst(string activityInstID)
        {
            ActivateActivityInst(activityInstID);
            //var actInst = repository.GetDomain<ActivityInst>(activityInstID);
            //UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            //{
            //    foreach (var wi in repository.Query<WorkItem>().Where(wi => wi.ActivityInstID == activityInstID).ToArray())
            //    {
            //        wi.CurrentState = (short)WorkItemStatus.WaitExecute;
            //        repository.SaveOrUpdate(wi);
            //    }
            //});
        }

        /// <summary>
        /// 挂起活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void SuspendActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                var activityInst = repository.GetDomain<ActivityInst>(activityInstID);
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ActivityInstID='{1}'", (short)WorkItemStatus.Suspended, activityInstID));
                activityInst.CurrentState = (short)ActivityInstStatus.Suspended;
                repository.SaveOrUpdate(activityInst);
            });
        }

        /// <summary>
        /// 恢复活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ResumeActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                var activityInst = repository.GetDomain<ActivityInst>(activityInstID);
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ActivityInstID='{1}'", (short)WorkItemStatus.WaitExecute, activityInstID));
                activityInst.CurrentState = (short)ActivityInstStatus.Running;
                repository.SaveOrUpdate(activityInst);
            });
        }

        /// <summary>
        /// 激活活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ActivateActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                var activityInst = repository.GetDomain<ActivityInst>(activityInstID);
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ActivityInstID='{1}'", (short)WorkItemStatus.WaitExecute, activityInstID));
                activityInst.CurrentState = (short)ActivityInstStatus.Running;
                repository.SaveOrUpdate(activityInst);
            });
        }

        /// <summary>
        /// 终止活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void TerminateActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                var activityInst = repository.GetDomain<ActivityInst>(activityInstID);
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ActivityInstID='{1}'", (short)WorkItemStatus.Terminated, activityInstID));
                activityInst.CurrentState = (short)ActivityInstStatus.Terminated;
                repository.SaveOrUpdate(activityInst);
            });
        }

        /// <summary>
        /// 将活动实例置为出错
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ErrorActivityInst(string activityInstID)
        {
            UnitOfWork.ExecuteWithTrans<ActivityInst>(() =>
            {
                var activityInst = repository.GetDomain<ActivityInst>(activityInstID);
                repository.ExecuteNonQuery<WorkItem>(string.Format("Update WF_WorkItem Set CurrentState={0} Where ActivityInstID='{1}'", (short)WorkItemStatus.Error, activityInstID));
                activityInst.CurrentState = (short)ActivityInstStatus.Error;
                repository.SaveOrUpdate(activityInst);
            });
        }
        #endregion

        #region 工作项管理
        /// <summary>
        /// 获取待办工作项列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="parameters">参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public IPageOfList<WorkItem> GetMyWorkItems(string userID, IDictionary<string, object> parameters, string sortCommand, PageInfo pageInfo, bool includeAuto)
        {
            return Persistence.GetMyWorkItems(userID, parameters, sortCommand, pageInfo, includeAuto);
        }

        /// <summary>
        /// 完成一个工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="parameters">参数</param>
        public void CompleteWorkItem(string workItemID, IDictionary<string, object> parameters, Func<Participantor> assignParticipant = null)
        {
            WorkItem wi = Persistence.GetWorkItem(workItemID);
            if (wi.CurrentState != (short)WorkItemStatus.WaitExecute && wi.CurrentState != (short)WorkItemStatus.Suspended)
            {
                WFUtil.HandleException(new MessageException()
                {
                    PromptMessage = string.Format("ID={0}的工作项{1}当前状态={2},不能被执行", wi.ID, wi.Name, wi.CurrentState.ToString().Cast<WorkItemStatus>(WorkItemStatus.Executing).GetRemark()),
                    Source = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName
                });

                return;
            }

            wi.CurrentState = (short)WorkItemStatus.Executing;

            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(ProcessDef)))
            {
                repository.SaveOrUpdate(wi);
                Persistence.UpdateActivityInstStatus(wi.ActivityInstID, ActivityInstStatus.Running);

                trans.Commit();
            }

            ProcessInst processInst = Persistence.GetProcessInst(wi.ProcessInstID);
            ProcessDefine processDefine = Persistence.GetProcessDefine(processInst.ProcessDefID);
            ProcessContext processContext = new ProcessContext()
            {
                ProcessDefine = processDefine,
                ProcessInst = processInst,
                Parameters = parameters
            };

            ProcessRuntime runtime = new ProcessRuntime(engineService, processContext, assignParticipant);
            runtime.Run();
        }


        /// <summary>
        /// 委托代办工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="agentID">代理用户</param>
        /// <param name="workItemID">工作项ID</param>
        public void DelegateWorkItem(IUser user, string agentID, string workItemID)
        {

        }

        /// <summary>
        /// 删除工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        public void DeleteWorkItem(string workItemID)
        {
            repository.Delete<WorkItem>(workItemID);
        }

        /// <summary>
        /// 代办工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        public void AgentWorkItem(IUser user, string workItemID)
        {
            //WorkItem wi = repository.GetDomain<WorkItem>(workItemID);

            //repository.GetDomain
        }
        /// <summary>
        /// 领取工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        [OperationContract]
        public void FetchWorkItem(IUser user, string workItemID)
        {
            WorkItem wi = repository.GetDomain<WorkItem>(workItemID);
            wi.CurrentState = (short)WorkItemStatus.Executing;

            repository.SaveOrUpdate(wi);
        }

        /// <summary>
        /// 获取当前流程实例待执行工作项及参与者字典
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public IDictionary<WorkItem, IList<Operator>> GetActiveWorkItems(string processInstID)
        {
            IDictionary<WorkItem, IList<Operator>> result = new Dictionary<WorkItem, IList<Operator>>();

            var activeWorkItems = repository.Query<WorkItem>().Where(o => o.ProcessInstID == processInstID && o.CurrentState == (short)WorkItemStatus.WaitExecute);
            foreach (var item in activeWorkItems)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("WorkItemID", item.ID);
                var participants = repository.ExecuteDataTable<WorkItem>(@"select * from AC_Operator where ID in (select ParticipantID from WF_Participant where WorkItemID=$WorkItemID and ParticipantType=1) 
union 
select * from AC_Operator where ID in (select ObjectID from OM_ObjectRole where RoleID in (select ParticipantID from WF_Participant where WorkItemID=$WorkItemID and ParticipantType=2))
union 
select * from AC_Operator where ID in (select EmployeeID from OM_EmployeeOrg where OrgID in( select ParticipantID from WF_Participant where WorkItemID=$WorkItemID and ParticipantType=3))
", parameters).ToList<Operator>();

                result.SafeAdd(item, participants);
            }
            return result;
        }
        #endregion

        #region 跳转接口
        /// <summary>
        /// 退回工作项
        /// 说明：将当前工作项退回到，最近一个可退回的活动。
        /// 最近可退回活动:最近可退回活动，是当前活动的主支上最近的一个共同祖先结点。
        /// 具体步骤：
        /// 1、寻找最近的一个可退回目标活动。
        /// 2、将流程回退到该目标活动。
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        public void RollbackWorkItem(IUser user, string workItemID)
        {
            WorkItem wi = Persistence.GetWorkItem(workItemID);
            if (wi.CurrentState != (short)WorkItemStatus.Executing)
            {
                ActivityInst actInst = Persistence.GetActivityInst(wi.ActivityInstID);
                //寻找最近的一个可退回活动。
                Activity destActivity = GetFirstRollbackActivity(wi.ProcessID, actInst.ActivityDefID);

                if (destActivity.ActivityType != ActivityType.StartActivity)
                {
                    log.Info(string.Format("回退工作项{0}-{1}到活动{2}-{3}", wi.ID, wi.Name, destActivity.ID, destActivity.Name));
                    //回退到目标活动
                    RollbackActivity(user, actInst, destActivity);
                }

                return;
            }

            WFUtil.HandleException(new MessageException()
            {
                PromptMessage = string.Format("ID={0}的工作项{1}当前状态={2},不能被执行", wi.ID, wi.Name, wi.CurrentState.ToString().Cast<WorkItemStatus>(WorkItemStatus.Executing).GetRemark()),
                Source = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName
            });
        }

        /// <summary>
        /// 获取活动第一个分支祖先活动
        /// </summary>
        /// <param name="processDefID"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Activity GetForefatherSplitActivity(string processDefID, Activity activity)
        {
            IList<Activity> inActivities = Persistence.GetInActivities(processDefID, activity.ID);

            if (inActivities.Count > 1 || (inActivities.Count == 1 && inActivities[0].ActivityType == ActivityType.StartActivity))
            {
                return activity;
            }

            return GetForefatherSplitActivity(processDefID, inActivities[0]);
        }

        /// <summary>
        /// 获取第一个共同祖先结点
        /// </summary>
        /// <param name="processDefine">流程定义</param>
        /// <param name="splitActivites">分支结点列表</param>
        /// <returns></returns>
        private Activity GetFirstRollbackActivity(string processDefID, string activityID)
        {
            IList<Activity> inActivities = Persistence.GetInActivities(processDefID, activityID);
            if (inActivities == null) return null;

            if (inActivities.Count == 1)
            {
                return inActivities[0];
            }

            //获取所有入口活动的第一个共同祖先结点
            return GetForefatherActivity(processDefID, inActivities);
        }

        /// <summary>
        /// 获取第一个共同祖先结点
        /// </summary>
        /// <param name="processDefine">流程定义</param>
        /// <param name="splitActivites">分支结点列表</param>
        /// <returns></returns>
        private Activity GetForefatherActivity(string processDefID, IList<Activity> splitActivites)
        {
            if (splitActivites != null && splitActivites.Count == 1)
                return splitActivites[0];

            int count = splitActivites.Count;
            Activity splitParent1 = GetForefatherSplitActivity(processDefID, splitActivites[count - 1]);
            Activity splitParent2 = GetForefatherSplitActivity(processDefID, splitActivites[count - 2]);
            Activity joinParent = null;
            if (splitParent1.ID == splitParent2.ID && splitActivites.Count == 2)
            {
                return splitParent1;
            }
            else if (splitParent1.ID != splitParent2.ID)
            {
                IList<Activity> children = new List<Activity>() { splitParent1, splitParent2 };
                joinParent = GetForefatherActivity(processDefID, children);
            }
            else
            {
                joinParent = splitParent1;
            }

            splitActivites.RemoveAt(count - 1);
            splitActivites.RemoveAt(count - 2);
            splitActivites.SafeAdd(joinParent);

            return GetForefatherActivity(processDefID, splitActivites);
        }

        /// <summary>
        /// 获取当前活动，可回退的活动列表
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">当前活动ID</param>
        /// <returns></returns>
        public IList<Activity> GetRollbackActivityList(string processDefID, string activityID)
        {
            IList<Activity> result = new List<Activity>();
            IList<Activity> inActivities = Persistence.GetInActivities(processDefID, activityID);
            if (inActivities == null) return null;

            if (inActivities.Count == 1)
            {
                if (inActivities[0].ActivityType != ActivityType.StartActivity)
                {
                    result.SafeAdd(inActivities);
                    result.SafeAdd(GetRollbackActivityList(processDefID, inActivities[0].ID));
                }
                return result;
            }

            //获取所有入口活动的第一个共同祖先结点
            Activity forefather = GetForefatherActivity(processDefID, inActivities);
            result.Add(forefather);

            result.SafeAdd(GetRollbackActivityList(processDefID, forefather.ID));

            return result;
        }

        /// <summary>
        /// 终点某活动的自身及所有后继活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="destActivity">目标活动</param>
        private void TerminateSelfAndOutActivity(string processInstID, Activity destActivity)
        {
            var destActInst = repository.Query<ActivityInst>().FirstOrDefault(ai => ai.ProcessInstID == processInstID && ai.RollbackFlag == 0 && ai.ActivityDefID == destActivity.ID);

            if (destActInst != null)
            {
                //寻找目标活动的所有出口活动实例
                var outActInsts = repository.FindAll<ActivityInst>(ParameterBuilder.BuildParameters().SafeAdd("ID", new Condition(string.Format("ID in(select DestActInstID from WF_Transition where SrcActInstID='{0}')", destActInst.ID)))
                                                                                                     .SafeAdd("RollbackFlag", 0));
                UnitOfWork.ExecuteWithTrans<ProcessDef>(() =>
                {
                    if (outActInsts != null)
                    {
                        foreach (var actInst in outActInsts)
                        {
                            //终止工作项
                            IList<WorkItem> workItems = Persistence.GetWorkItems(actInst.ID);
                            foreach (var wi in workItems)
                            {
                                wi.CurrentState = (short)WorkItemStatus.Terminated;
                                log.Info(string.Format("终止工作项{0}-{1}", wi.ID, wi.Name));
                                repository.SaveOrUpdate(wi);
                            }

                            //终止当前活动
                            actInst.CurrentState = (short)ActivityInstStatus.Terminated;
                            actInst.RollbackFlag = 1;
                            log.Info(string.Format("终止活动实例{0}-{1}", actInst.ID, actInst.Name));
                            repository.SaveOrUpdate(actInst);

                            //继续终止该活动的出口活动
                            ProcessInst processInst = repository.GetDomain<ProcessInst>(processInstID);
                            Activity newDestAct = Persistence.GetActivity(processInst.ProcessDefID, actInst.ActivityDefID);
                            TerminateSelfAndOutActivity(processInstID, newDestAct);
                        }
                    }

                    destActInst.RollbackFlag = 1;
                    destActInst.CurrentState = (short)ActivityInstStatus.Terminated;
                    log.Info(string.Format("终止活动实例{0}-{1}", destActInst.ID, destActInst.Name));
                    repository.SaveOrUpdate(destActInst);
                });
            }
        }

        /// <summary>
        /// 回退流程到某个活动
        /// 说明：将当前活动实例回退到，某个目标活动。
        /// 具体步骤：
        /// 1、寻找目标活动实例。
        /// 2、将目标活动及后续已完成的活动实例及工作项终止。
        /// 3、终止当前活动实例及工作项。（注：为记录当前执行人再终止一次）
        /// 4、重新启动可退回活动。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="srcActInst"></param>
        /// <param name="destActivity"></param>
        public void RollbackActivity(IUser user, ActivityInst srcActInst, Activity destActivity)
        {
            var destActInst = repository.Query<ActivityInst>().FirstOrDefault(ai => ai.ProcessInstID == srcActInst.ProcessInstID && ai.RollbackFlag == 0 && ai.ActivityDefID == destActivity.ID);

            if (destActInst != null)
            {
                DateTime createTime = DateTime.Now;
                UnitOfWork.ExecuteWithTrans<ProcessDef>(() =>
                {
                    //终止destActivity所有的后续已执行的的活动实例
                    TerminateSelfAndOutActivity(destActInst.ProcessInstID, destActivity);

                    //将当前活动实例的工作项终止（将终止人设为当前用户)
                    var workItems = Persistence.GetWorkItems(srcActInst.ID);
                    foreach (var wi in workItems)
                    {
                        wi.CurrentState = (short)WorkItemStatus.Terminated;
                        wi.ExecutorName = user.Name;
                        wi.Executor = user.ID;
                        wi.ExecuteTime = createTime;
                        log.Info(string.Format("终止工作项{0}-{1}", wi.ID, wi.Name));
                        repository.SaveOrUpdate(wi);

                    }
                    //将当前活动实例终止
                    srcActInst.CurrentState = (short)ActivityInstStatus.Terminated;
                    srcActInst.RollbackFlag = 1;
                    srcActInst.EndTime = createTime;
                    log.Info(string.Format("终止活动实例{0}-{1}", srcActInst.ID, srcActInst.Name));
                    repository.SaveOrUpdate(srcActInst);


                    //将目标活动实例的工作项重置为待执行
                    workItems = Persistence.GetWorkItems(destActInst.ID);
                    foreach (var wi in workItems)
                    {
                        wi.CurrentState = (short)WorkItemStatus.WaitExecute;
                        //wi.ExecutorName = user.Name;
                        //wi.Executor = user.ID;
                        // wi.ExecuteTime = null;// DateTime.MaxValue;
                        log.Info(string.Format("启动工作项{0}-{1}", wi.ID, wi.Name));
                        repository.SaveOrUpdate(wi);
                    }

                    //将目标活动实例状态置为未开始
                    destActInst.CurrentState = (short)ActivityInstStatus.NoStart;
                    destActInst.RollbackFlag = 0;
                    log.Info(string.Format("启动活动实例{0}-{1}", destActInst.ID, destActInst.Name));
                    repository.SaveOrUpdate(destActInst);

                    ProcessInst processInst = repository.GetDomain<ProcessInst>(srcActInst.ProcessInstID);
                    Domain.Transition transition = new Domain.Transition()
                    {
                        ID = IdGenerator.NewComb().ToString(),
                        DestActID = destActivity.ID,
                        DestActInstID = destActInst.ID,
                        DestActInstName = destActInst.Name,
                        DestActName = destActivity.Name,
                        ProcessInstID = srcActInst.ProcessInstID,
                        ProcessInstName = processInst.Name,
                        SrcActID = srcActInst.ActivityDefID,
                        SrcActInstID = srcActInst.ID,
                        SrcActInstName = srcActInst.Name,
                        SrcActName = srcActInst.Name,
                        TransTime = createTime
                    };
                    repository.SaveOrUpdate(transition);

                    TransControl transControl = new TransControl()
                    {
                        ID = IdGenerator.NewComb().ToString(),
                        DestActID = destActivity.ID,
                        DestActName = destActivity.Name,
                        ProcessInstID = srcActInst.ProcessInstID,
                        SrcActID = srcActInst.ActivityDefID,
                        SrcActName = srcActInst.Name,
                        TransTime = createTime,
                        TransWeight = 100
                    };
                    repository.SaveOrUpdate(transControl);
                });
            }
        }

        public void SkipToWorkItem(IUser user, string workItemID)
        {
            throw new NotImplementedException();
        }

        public IList<Activity> GetSkipActivityList(IUser user, Activity srcAct)
        {
            throw new NotImplementedException();
        }

        public void SkipToActivity(IUser user, ActivityInst srcActInst, Activity destActivity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 参与者接口
        /// <summary>
        /// 获取角色和组织参与者
        /// </summary>
        /// <returns></returns>
        public IList<Participantor> GetRoleAndOrgParticipantors()
        {
            string cacheKey = "All_RoleAndOrgParticipantors";
            IList<Participantor> result = CacheManager.GetData<IList<Participantor>>(cacheKey);
            if (result == null)
            {
                int index = 0;
                result = repository.All<Role>().Select(r =>
                   new Participantor()
                   {
                       ID = r.ID,
                       ParticipantorType = ParticipantorType.Role,
                       SortOrder = ++index,
                       Name = r.Name,
                       ParentID = r.AppID
                   }).ToList();

                index = 0;
                foreach (var o in repository.All<Organization>())
                {
                    result.Add(new Participantor()
                    {
                        ID = o.ID,
                        Name = o.Name,
                        ParticipantorType = ParticipantorType.Org,
                        SortOrder = ++index,
                        ParentID = o.ParentID
                    });
                }

                CacheManager.Add(cacheKey, result);
            }

            return result;
        }

        /// <summary>
        /// 获取某参与者类型下的所有参与者
        /// </summary>
        /// <param name="parentType">父参与者类型</param>
        /// <param name="parentID">父参与者ID</param>
        /// <returns></returns>
        public IList<Participantor> GetPersonParticipantors(ParticipantorType parentType, string parentID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.SafeAdd("ID", "none");
            if (parentType == ParticipantorType.Role)
            {
                parameters.SafeAdd("ID", new AgileEAP.Core.Data.Condition(string.Format("ID in (select b.ObjectID from OM_ObjectRole b where b.RoleID='{0}')", parentID)));
            }
            else if (parentType == ParticipantorType.Org)
            {
                parameters.SafeAdd("ID", new AgileEAP.Core.Data.Condition(string.Format("ID in (select b.EmployeeID from OM_EmployeeOrg b where b.OrgID='{0}')", parentID)));
            }

            int index = 0;
            return repository.FindAll<Employee>(parameters).Select(o => new Participantor()
            {
                ID = o.ID,
                Name = o.Name,
                ParticipantorType = ParticipantorType.Person,
                SortOrder = ++index,
                ParentID = parentID
            }).ToList();
        }
        #endregion

        #endregion
    }
}
