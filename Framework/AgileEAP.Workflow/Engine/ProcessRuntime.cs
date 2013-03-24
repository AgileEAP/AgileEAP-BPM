using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AgileEAP.Core;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
using Jint;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 实现推动和更改具体某个Workflow状态，要调用ProcessRuntime
    /// </summary>
    public class ProcessRuntime
    {
        #region Variables
        private IEngineService engineService;
        private Func<Participantor> assignParticipant = null;
        private bool waiting = false;
        #endregion

        #region Properties
        private IWorkflowPersistence Persistence
        {
            get { return engineService.Persistence; }
        }
        private INotification Notification
        {
            get { return engineService.Notification; }
        }
        private IWorkAssign WorkAssign
        {
            get { return engineService.WorkAssign; }
        }

        public ProcessContext Context
        {
            get;
            set;
        }


        #endregion

        #region Constructor
        public ProcessRuntime(IEngineService services, ProcessContext context, Func<Participantor> assignParticipant)
        {
            engineService = services;
            Context = context;
            this.assignParticipant = assignParticipant;
        }
        #endregion

        public void Run()
        {
            waiting = true;
            while (waiting)
            {
                waiting = false;
                var runningActInsts = Persistence.Repository.Query<ActivityInst>().Where(ai => ai.ProcessInstID == Context.ProcessInst.ID && ai.CurrentState == (short)ActivityInstStatus.Running && ai.RollbackFlag == 0).ToArray();
                foreach (ActivityInst activityInst in runningActInsts)
                {
                    ActivityContext activityContext = new ActivityContext()
                    {
                        ProcessInst = Context.ProcessInst,
                        ActivityInst = activityInst,
                        Parameters = Context.Parameters,
                        ProcessDefine = Context.ProcessDefine
                    };
                    ActivityRuntime runtime = new ActivityRuntime(engineService, activityContext);

                    runtime.Run();

                    UpdateActivityInstAndRoute(activityInst);
                }
            }

            CheckAndUpdateProcessInstStatus();
        }

        private void UpdateActivityInstAndRoute(ActivityInst activityInst)
        {
            IList<WorkItem> workItems = Persistence.GetWorkItems(activityInst.ID);
            ActivityInstStatus newStatus = ActivityInstStatus.NoStart;

            bool allCompleted = true;
            foreach (WorkItem ti in workItems)
            {
                switch (ti.CurrentState.Cast<WorkItemStatus>(WorkItemStatus.WaitExecute))
                {
                    case WorkItemStatus.WaitExecute:
                        allCompleted = false;
                        break;
                    case WorkItemStatus.Suspended:
                        allCompleted = false;
                        break;
                    case WorkItemStatus.Executing:
                        allCompleted = false;
                        if (newStatus == ActivityInstStatus.NoStart)
                            newStatus = ActivityInstStatus.Running;
                        break;
                    case WorkItemStatus.Compeleted:
                        if (newStatus == ActivityInstStatus.NoStart || newStatus == ActivityInstStatus.Running)
                            newStatus = ActivityInstStatus.Running;
                        break;
                    case WorkItemStatus.Canceled:
                        break;
                    case WorkItemStatus.Terminated:
                        break;
                }
            }

            if (allCompleted)
                newStatus = ActivityInstStatus.Compeleted;

            if (activityInst.CurrentState != (short)newStatus)
            {
                activityInst.CurrentState = (short)newStatus;
                Persistence.UpdateActivityInstStatus(activityInst.ID, newStatus);
                waiting = true;
            }
            if (allCompleted)
            {
                newStatus = ActivityInstStatus.Compeleted;
                RouteActivityInst(activityInst);
            }
        }

        /// <summary>
        /// 检测活动是否可以激活
        /// </summary>
        /// <param name="processInstID"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public bool CanActivateActiviy(string processInstID, Activity activity)
        {
            IList<ActivityInst> actInsts = Persistence.GetActivityInsts(processInstID, activity.ID);
            if (actInsts != null && actInsts.Count > 0) return false;

            IList<Activity> activities = Persistence.GetInActivities(Context.ProcessInst.ProcessDefID, activity.ID);
            foreach (var act in activities)
            {
                if (activity.JoinType == JoinType.XOR)
                {
                    bool hasCompleted = false;
                    foreach (var actInst in Persistence.GetActivityInsts(processInstID, act.ID))
                    {
                        if (actInst.CurrentState == (short)ActivityInstStatus.Compeleted)
                        {
                            hasCompleted = true;
                            break;
                        }
                    }

                    if (hasCompleted)
                    {
                        using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(ProcessDef)))
                        {
                            foreach (var actInst in Persistence.GetActivityInsts(processInstID, act.ID))
                            {
                                if (actInst.CurrentState != (short)ActivityInstStatus.Compeleted && actInst.CurrentState != (short)ActivityInstStatus.Terminated)
                                {
                                    foreach (var workItem in Persistence.GetWorkItems(actInst.ID))
                                    {
                                        if (workItem.CurrentState != (short)WorkItemStatus.Compeleted && actInst.CurrentState != (short)ActivityInstStatus.Terminated)
                                        {
                                            workItem.CurrentState = (short)WorkItemStatus.Terminated;
                                            Persistence.Repository.SaveOrUpdate(workItem);
                                        }
                                    }

                                    actInst.CurrentState = (short)ActivityInstStatus.Terminated;
                                    Persistence.Repository.SaveOrUpdate(actInst);
                                }
                            }

                            trans.Commit();
                        }
                    }

                    return hasCompleted;
                }
                else
                {
                    foreach (var actInst in Persistence.GetActivityInsts(processInstID, act.ID))
                    {
                        if (actInst.CurrentState != (short)ActivityInstStatus.Compeleted && actInst.CurrentState != (short)ActivityInstStatus.Terminated)
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 获取待运行活动
        /// </summary>
        /// <param name="processDefID"></param>
        /// <param name="srcActInst"></param>
        /// <returns></returns>
        public IList<Activity> GetActivateActivities(string processDefID, ActivityInst srcActInst)
        {
            IList<Definition.Transition> transitions = Persistence.GetOutTransitions(processDefID, srcActInst.ActivityDefID);
            Activity srcActDef = Persistence.GetActivity(processDefID, srcActInst.ActivityDefID);

            IList<Activity> activateActivities = new List<Activity>();
            Activity defaultActivity = null;
            foreach (var transition in transitions)
            {
                //IDictionary<string, object> parameters = new Dictionary<string, object>();
                //parameters.SafeAdd("money", 1000);
                //parameters.SafeAdd("number", 4);
                //Context.Parameters = parameters;
                ////engine.SetParameter(":money", 1000);
                //transition.Expression = "return @money-100>0&&@number<10;";
                //new Regex(@"[^@@](?<p>@\w+)");对@@value,不表示变量;Regex(@"@\w*")以@开头的表示变量。
                string expression = transition.Expression;
                try
                {
                    bool expressionResult = string.IsNullOrWhiteSpace(expression);
                    if (!expressionResult)
                    {
                        var prefix = WFUtil.ExpressionVariablePrefix;
                        Regex regex = new Regex(string.Format(@"{0}\w*", prefix));
                        MatchCollection matches = regex.Matches(expression.Replace(string.Format("{0}{0}", prefix), "###"));
                        JintEngine engine = new JintEngine();

                        if (matches != null && Context.Parameters != null)
                        {
                            foreach (Match match in matches)
                            {
                                string variable = match.Value.TrimStart(prefix);
                                if (Context.Parameters.ContainsKey(variable))
                                    engine.SetParameter(variable, Context.Parameters[variable]);
                            }
                        }
                        expression = expression.Replace(string.Format("{0}{0}", prefix), "###").Replace(prefix.ToString(), "").Replace("###", prefix.ToString());
                        expressionResult = (bool)engine.Run(expression);
                    }

                    Activity destActivity = Persistence.GetActivity(processDefID, transition.DestActivity);
                    //记住默认活动
                    if (transition.IsDefault) defaultActivity = destActivity;

                    if (expressionResult && CanActivateActiviy(srcActInst.ProcessInstID, destActivity))
                    {
                        activateActivities.SafeAdd(destActivity);
                    }
                }
                catch (Exception ex)
                {
                    WFUtil.HandleException(new WFException(typeof(ProcessRuntime).FullName, string.Format("计算表达式{0}出错", expression), ex));
                    throw;
                }

            }


            if (activateActivities.Count == 0 && defaultActivity != null && CanActivateActiviy(srcActInst.ProcessInstID, defaultActivity))
            {
                activateActivities.Add(defaultActivity);
            }

            return activateActivities;
        }

        /// <summary>
        /// 产生迁移活动
        /// </summary>
        /// <param name="srcActInst"></param>
        public void RouteActivityInst(ActivityInst srcActInst)
        {
            Activity srcActivity = Context.ProcessDefine.Activities.FirstOrDefault(a => a.ID == srcActInst.ActivityDefID);
            IList<Activity> activateActivities = GetActivateActivities(Context.ProcessInst.ProcessDefID, srcActInst);
            string processInstID = Context.ProcessInst.ID;
            string processDefID = Context.ProcessInst.ProcessDefID;

            UnitOfWork.ExecuteWithTrans<WorkItem>(() =>
            {
                foreach (var destActivity in activateActivities)
                {
                    DateTime createTime = DateTime.Now;
                    ActivityInst destActInst = new ActivityInst()
                    {
                        ActivityDefID = destActivity.ID,
                        CurrentState = (short)(destActivity.ActivityType == ActivityType.EndActivity || destActivity.ActivityType == ActivityType.AutoActivity ? ActivityInstStatus.Compeleted : ActivityInstStatus.NoStart),
                        ID = IdGenerator.NewComb().ToString(),
                        CreateTime = createTime,
                        Description = destActivity.Description,
                        EndTime = WFUtil.MaxDate,
                        Name = destActivity.Name,
                        ProcessInstID = processInstID,
                        RollbackFlag = 0,
                        StartTime = createTime,
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
                        ProcessInstName = Context.ProcessInst.Name,
                        SrcActID = srcActivity.ID,
                        SrcActInstID = srcActInst.ID,
                        SrcActInstName = srcActInst.Name,
                        SrcActName = srcActivity.Name,
                        TransTime = createTime
                    };

                    TransControl transControl = new TransControl()
                    {
                        ID = IdGenerator.NewComb().ToString(),
                        DestActID = destActivity.ID,
                        DestActName = destActivity.Name,
                        ProcessInstID = processInstID,
                        SrcActID = srcActivity.ID,
                        SrcActName = srcActivity.Name,
                        TransTime = createTime,
                        TransWeight = 100
                    };

                    //srcActInst.CurrentState = (short)ActivityInstStatus.Compeleted;
                    if (destActivity is ManualActivity)
                    {
                        #region ManualActivityHandler

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
                            CreateTime = createTime,
                            Creator = WFUtil.User.ID,
                            CreatorName = WFUtil.User.Name,
                            CurrentState = (short)WorkItemStatus.WaitExecute,
                            Description = destActInst.Description,
                            EndTime = WFUtil.MaxDate,
                            Executor = string.Empty,
                            ExecutorName = string.Empty,
                            IsTimeOut = 0,
                            Name = destActInst.Name,
                            Participant = Persistence.GetParticipant(processDefID, destActivity.ID),
                            ProcessID = processDefID,
                            ProcessInstID = processInstID,
                            ProcessInstName = Context.ProcessInst.Name,
                            ProcessName = Context.ProcessDefine.Name,
                            RemindTime = WFUtil.MaxDate,
                            RootProcessInstID = string.Empty,
                            StartTime = createTime,
                            TimeOutTime = WFUtil.MaxDate,
                            Type = destActInst.Type,
                            //ExecuteTime = null,
                        };

                        IList<Participantor> participantors = assignParticipant == null ? ma.Participant.Participantors : new List<Participantor> { assignParticipant() };

                        foreach (var participantor in participantors)
                        {
                            Domain.Participant participant = new Domain.Participant()
                            {
                                ID = IdGenerator.NewComb().ToString(),
                                CreateTime = createTime,
                                DelegateType = (short)DelegateType.Sponsor,
                                Name = participantor.Name,
                                ParticipantID = participantor.ID,
                                ParticipantIndex = participantor.SortOrder,
                                ParticipantType = (short)participantor.ParticipantorType.Cast<ParticipantorType>(ParticipantorType.Person),
                                PartiInType = (short)PartiInType.Exe,
                                WorkItemID = wi.ID,
                                WorkItemState = (short)wi.CurrentState
                            };

                            Persistence.Repository.SaveOrUpdate(participant);
                        }

                        Persistence.Repository.SaveOrUpdate(wi);

                        #endregion
                    }
                    else if (destActivity is AutoActivity)
                    {
                        if (!HandlerAutoActivity(processDefID, srcActivity, destActivity, destActInst))
                            destActInst.CurrentState = (short)ActivityInstStatus.Suspended;
                    }

                    srcActInst.CurrentState = (short)ActivityInstStatus.Compeleted;
                    Persistence.Repository.SaveOrUpdate(srcActInst);
                    Persistence.Repository.SaveOrUpdate(destActInst);
                    Persistence.Repository.SaveOrUpdate(transition);
                    Persistence.Repository.SaveOrUpdate(transControl);
                }
            });
        }

        private bool HandlerAutoActivity(string processDefID, Activity srcActivity, Activity destActivity, ActivityInst destActInst)
        {
            DateTime createTime = DateTime.Now;
            AutoActivity autoActivity = destActivity as AutoActivity;
            ManualActivity manualActivity = srcActivity as ManualActivity;
            string actionURL = null;
            if (manualActivity != null && manualActivity.CustomURL != null)
                actionURL = manualActivity.CustomURL.SpecifyURL;

            WorkItem wi = new WorkItem()
            {
                ID = IdGenerator.NewComb().ToString(),
                ActionMask = string.Empty,
                ActivityInstID = destActInst.ID,
                ActivityInstName = destActInst.Name,
                BizState = (short)WorkItemBizStatus.Common,
                CreateTime = createTime,
                Creator = WFUtil.User.ID,
                CreatorName = WFUtil.User.Name,
                CurrentState = (short)WorkItemStatus.Executing,
                Description = destActInst.Description,
                EndTime = WFUtil.MaxDate,
                Executor = string.Empty,
                ExecutorName = string.Empty,
                IsTimeOut = 0,
                Name = destActInst.Name,
                Participant = "AgileEAP.Workflow",
                ProcessID = processDefID,
                ProcessInstID = destActInst.ProcessInstID,
                ProcessInstName = Context.ProcessInst.Name,
                ProcessName = Context.ProcessDefine.Name,
                RemindTime = WFUtil.MaxDate,
                RootProcessInstID = string.Empty,
                StartTime = createTime,
                TimeOutTime = WFUtil.MaxDate,
                Type = destActInst.Type,
                ActionURL = actionURL
            };

            bool isCompeleted = false;
            //如果自动处理完成并返回true,完成工作项，并路由到下一活动
            if (DefaultExecutor.Execute(string.Format("{0}-{1}-{2}", Context.ProcessDefine.ID, Context.ProcessDefine.Version, destActivity.ID), wi))
            {
                wi.CurrentState = (short)WorkItemStatus.Compeleted;
                Persistence.Repository.SaveOrUpdate(wi);
                RouteActivityInst(destActInst);
                isCompeleted = true;
            }
            else
            {
                wi.CurrentState = (short)WorkItemStatus.Suspended;
                Persistence.Repository.SaveOrUpdate(wi);
                isCompeleted = false;
            }

            return isCompeleted;
        }

        private void CheckAndUpdateProcessInstStatus()
        {
            //TODO: 还需要考虑其他条件和状态(Cancelled? Failed?)来判断Workflow的状态
            //例如Workflow是否Complete了，该如何判断？所有的Activity都完成了？最后的Activity完成了？

            ProcessInstStatus newStatus = ProcessInstStatus.NoStart;
            IList<ActivityInst> activityInsts = Persistence.GetActivityInsts(Context.ProcessInst.ID);
            bool allCompleted = true;
            foreach (ActivityInst ai in activityInsts)
            {
                switch (ai.CurrentState.Cast<ActivityInstStatus>(ActivityInstStatus.WaitActivate))
                {
                    case ActivityInstStatus.NoStart:
                        allCompleted = false;
                        break;
                    case ActivityInstStatus.Suspended:
                        allCompleted = false;
                        newStatus = ProcessInstStatus.Running;
                        break;
                    case ActivityInstStatus.Running:
                        allCompleted = false;
                        if (newStatus == ProcessInstStatus.NoStart)
                            newStatus = ProcessInstStatus.Running;
                        break;
                    //case ActivityInstStatus.In_Progress:
                    //    allCompleted = false;
                    //    if (newStatus == ProcessInstStatus.NotStarted || newStatus == ProcessInstStatus.In_Progress)
                    //        newStatus = ProcessInstStatus.In_Progress;
                    //    break;
                    case ActivityInstStatus.Compeleted:
                        if (newStatus == ProcessInstStatus.NoStart || newStatus == ProcessInstStatus.Running)
                            newStatus = ProcessInstStatus.Running;
                        break;
                    //case ActivityInstStatus.Cancelled:
                    //    break;
                    case ActivityInstStatus.Error:
                        break;
                }
            }
            if (allCompleted)
                newStatus = ProcessInstStatus.Completed;

            if (Context.ProcessInst.CurrentState != (short)newStatus)
            {
                Context.ProcessInst.CurrentState = (short)newStatus;
                Persistence.UpdateProcessInstStatus(Context.ProcessInst.ID, newStatus);
            }
        }
    }
}

