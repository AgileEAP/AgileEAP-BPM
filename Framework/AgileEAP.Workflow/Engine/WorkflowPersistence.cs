using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using AgileEAP.Core;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
using AgileEAP.Core.Data;
using AgileEAP.Core.Service;

namespace AgileEAP.Workflow.Engine
{
    public class WorkflowPersistence : IWorkflowPersistence
    {
        #region Variables
        ILogger log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region IWorkflowPersistence

        #region Definition
        /// <summary>
        /// 获取工作流定义
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public ProcessDefine GetProcessDefine(string processDefID)
        {
            return CacheManager.Get<ProcessDefine>(processDefID, () =>
            {
                ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);
                return new ProcessDefine(processDef.Content, false);
            });
        }

        /// <summary>
        /// 获取工作流某个活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动定义ID</param>
        /// <returns></returns>
        public Activity GetActivity(string processDefID, string activityID)
        {
            return GetProcessDefine(processDefID).Activities.FirstOrDefault(a => string.Equals(a.ID, activityID, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取流程所有定义活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public IList<Activity> GetActivities(string processDefID)
        {
            return GetProcessDefine(processDefID).Activities;
        }

        /// <summary>
        /// 获取流程初始活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public Activity GetStartActivity(string processDefID)
        {
            return GetProcessDefine(processDefID).StartActivity;
        }

        /// <summary>
        /// 获取活动所有的入口活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public IList<Definition.Activity> GetInActivities(string processDefID, string activityID)
        {
            ProcessDefine processDefine = GetProcessDefine(processDefID);

            return processDefine.Activities.Where(a => processDefine.Transitions.Exists(t => t.DestActivity == activityID && t.SrcActivity == a.ID)).ToList();
        }

        /// <summary>
        /// 获取活动所有的出口活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public IList<Definition.Activity> GetOutActivities(string processDefID, string activityID)
        {
            ProcessDefine processDefine = GetProcessDefine(processDefID);

            return processDefine.Activities.Where(a => processDefine.Transitions.Exists(t => t.SrcActivity == activityID && t.DestActivity == a.ID)).ToList();
        }

        #endregion

        #region Transition
        /// <summary>
        /// 获取活动之间的迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="srcActivityID">源活动ID</param>
        /// <param name="destActivityID">目标活动ID</param>
        /// <returns></returns>
        public Definition.Transition GetTransition(string processDefID, string srcActivityID, string destActivityID)
        {
            return GetProcessDefine(processDefID).Transitions.FirstOrDefault(t => t.SrcActivity.EqualIgnoreCase(srcActivityID) && t.DestActivity.EqualIgnoreCase(destActivityID));
        }

        /// <summary>
        /// 获取活动所有的入口迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public IList<Definition.Transition> GetInTransitions(string processDefID, string activityID)
        {
            return GetProcessDefine(processDefID).Transitions.Where(t => t.DestActivity.EqualIgnoreCase(activityID)).ToList();
        }

        /// <summary>
        /// 获取活动所有的入口迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        public IList<Definition.Transition> GetOutTransitions(string processDefID, string activityID)
        {
            return GetProcessDefine(processDefID).Transitions.Where(t => t.SrcActivity.EqualIgnoreCase(activityID)).ToList();
        }

        #endregion

        #region Instance

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public ProcessInst GetProcessInst(string processInstID)
        {
            return repository.GetDomain<ProcessInst>(processInstID);
        }

        /// <summary>
        /// 获取某个活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <returns></returns>
        public ActivityInst GetActivityInst(string activityInstID)
        {
            return repository.GetDomain<ActivityInst>(activityInstID);
        }

        /// <summary>
        /// 获取某个流程活动的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public IList<ActivityInst> GetActivityInsts(string processInstID, string activityDefID)
        {
            return repository.Query<ActivityInst>().Where(ai => ai.ProcessInstID == processInstID && ai.ActivityDefID == activityDefID && ai.RollbackFlag == 0).ToList();
        }

        /// <summary>
        /// 获取某个流程活动的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public IList<ActivityInst> GetActivityInsts(string processInstID, string activityDefID, ActivityInstStatus actInstStatus)
        {
            return GetActivityInsts(processInstID, activityDefID).Where(a => a.CurrentState == (short)actInstStatus).ToList();
        }

        /// <summary>
        /// 获取某个工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <returns></returns>
        public WorkItem GetWorkItem(string workItemID)
        {
            return repository.GetDomain<WorkItem>(workItemID);
        }

        /// <summary>
        /// 获取某个流程实例的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public IList<ActivityInst> GetActivityInsts(string processInstID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.SafeAdd("ProcessInstID", processInstID);

            return repository.FindAll<ActivityInst>(parameters);
        }

        /// <summary>
        /// 获取某个活动实例的所有工作项
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <returns></returns>
        public IList<WorkItem> GetWorkItems(string activityInstID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.SafeAdd("ActivityInstID", activityInstID);

            return repository.FindAll<WorkItem>(parameters);
        }


        /// <summary>
        /// 获取待办工作项列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="parameters">参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public IPageOfList<WorkItem> GetMyWorkItems(string userID, IDictionary<string, object> parameters, string sortCommand, PageInfo pageInfo, bool includeAuto)
        {
            if (parameters == null) parameters = new Dictionary<string, object>();
            string includesql = includeAuto ? "or Type=5 " : string.Empty;

            DatabaseType databaseType = UnitOfWork.CurrentDatabaseType;
            if (databaseType == DatabaseType.MySQL)
            {

                System.Data.DataTable dt = repository.ExecuteDataTable<WorkItem>(string.Format(@"select a.* from WF_WorkItem a 
inner join  WF_Participant p on a.ID=p.WorkItemID
where ((p.ParticipantID='{0}' or
 		p.ParticipantID in(select  RoleID from OM_ObjectRole r where r.ObjectID='{0}')
 		or p.ParticipantID in (select OrgID from OM_EmployeeOrg o where o.EmployeeID='{0}')) {1}) and 1=1 ", userID, includesql), parameters, sortCommand ?? "order by CreateTime desc", pageInfo);


                return new PageOfList<WorkItem>()
                    {
                        ItemList = dt.ToList<WorkItem>(),
                        PageInfo = pageInfo
                    };
            }
            else
            {
                parameters.SafeAdd(userID, new Condition(string.Format(@"(ID in
                                (select WorkItemID from WF_Participant p where p.ParticipantID='{0}' or
                                 exists(select ObjectID from OM_ObjectRole r where r.ObjectID='{0}' and r.RoleID=p.ParticipantID) or
                                 exists(select ID from OM_EmployeeOrg o where o.EmployeeID='{0}' and o.OrgID=p.ParticipantID)) {1})", userID, includesql)));


                return repository.FindAll<WorkItem>(parameters, sortCommand ?? "order by CreateTime desc", pageInfo);
            }
        }

        /// <summary>
        /// 修改工作项状态
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="status">工作项状态</param>
        public void UpdateWorkItemStatus(string workItemID, WorkItemStatus status)
        {
            WorkItem workItem = repository.GetDomain<WorkItem>(workItemID);

            if (workItem == null)
            {
                log.Warn("UpdateWorkItemStatus error workitem is null");
                return;
            }

            workItem.CurrentState = (short)status;
            repository.Update(workItem);
        }

        /// <summary>
        /// 修改活动状态
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <param name="status">活动状态</param>
        public void UpdateActivityInstStatus(string activityInstID, ActivityInstStatus status)
        {
            ActivityInst activityInst = repository.GetDomain<ActivityInst>(activityInstID);

            if (activityInst == null)
            {
                log.Warn("UpdateActivityInstStatus error activityInst is null");
                return;
            }

            activityInst.CurrentState = (short)status;
            repository.Update(activityInst);
        }

        /// <summary>
        ///修改流程状态 
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="status">流程状态</param>
        public void UpdateProcessInstStatus(string processInstID, ProcessInstStatus status)
        {
            ProcessInst instance = repository.GetDomain<ProcessInst>(processInstID);

            if (instance == null)
            {
                log.Warn("UpdateActivityInstStatus error activityInst is null");
                return;
            }

            instance.CurrentState = (short)status;
            repository.Update(instance);
        }

        /// <summary>
        /// 获取活动参与者
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityDefID">活动定义ID</param>
        /// <returns></returns>
        public string GetParticipant(string processDefID, string activityDefID)
        {
            return string.Empty;
            //ManualActivity ma = GetActivity(processDefID, activityDefID) as ManualActivity;

            ////if (ma.Participant.ParticipantType.Cast<ParticipantType>(ParticipantType.Person) == ParticipantType.Person)
            ////{

            ////}
            //return string.Join(",", ma.Participant.Participantors.Select(p => p.ID).ToArray());
        }
        #endregion

        #region Repository
        private IRepository<string> repository = new Repository<string>();
        public IRepository<string> Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = new Repository<string>();
                }

                return repository;
            }
        }
        #endregion
        #endregion
    }
}
