using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Enums;
using AgileEAP.Core.Data;
using AgileEAP.Core;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 定义保存、获取流程定义和实例的接口
    /// </summary>
    public interface IWorkflowPersistence
    {
        #region Definition
        /// <summary>
        /// 获取工作流定义
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        ProcessDefine GetProcessDefine(string processDefID);

        /// <summary>
        /// 获取工作流某个活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动定义ID</param>
        /// <returns></returns>
        Activity GetActivity(string processDefID, string activityID);

        /// <summary>
        /// 获取流程所有定义活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        IList<Activity> GetActivities(string processDefID);

        /// <summary>
        /// 获取流程初始活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        Activity GetStartActivity(string processDefID);
        #endregion

        #region Transition
        /// <summary>
        /// 获取活动之间的迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="srcActivityID">源活动ID</param>
        /// <param name="destActivityID">目标活动ID</param>
        /// <returns></returns>
        Definition.Transition GetTransition(string processDefID, string srcActivityID, string destActivityID);

        /// <summary>
        /// 获取活动所有的入口迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        IList<Definition.Transition> GetInTransitions(string processDefID, string activityID);

        /// <summary>
        /// 获取活动所有的出口迁移
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        IList<Definition.Transition> GetOutTransitions(string processDefID, string activityID);

        /// <summary>
        /// 获取活动所有的入口活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        IList<Definition.Activity> GetInActivities(string processDefID, string activityID);

        /// <summary>
        /// 获取活动所有的出口活动
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">活动ID</param>
        /// <returns></returns>
        IList<Definition.Activity> GetOutActivities(string processDefID, string activityID);

        #endregion

        #region Instance

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        ProcessInst GetProcessInst(string processInstID);

        /// <summary>
        /// 获取某个活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <returns></returns>
        ActivityInst GetActivityInst(string activityInstID);

        /// <summary>
        /// 获取某个工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <returns></returns>
        WorkItem GetWorkItem(string workItemID);

        /// <summary>
        /// 获取某个流程实例的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        IList<ActivityInst> GetActivityInsts(string processInstID);

        /// <summary>
        /// 获取某个流程活动的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="activityDefID">活动定义ID</param>
        /// <returns></returns>
        IList<ActivityInst> GetActivityInsts(string processInstID, string activityDefID);

        /// <summary>
        /// 获取某个流程活动的所有活动实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="activityDefID">活动定义ID</param>
        /// <param name="actInstStatus">活动实例状态</param>
        /// <returns></returns>
        IList<ActivityInst> GetActivityInsts(string processInstID, string activityDefID, ActivityInstStatus actInstStatus);

        /// <summary>
        /// 获取某个活动实例的所有工作项
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <returns></returns>
        IList<WorkItem> GetWorkItems(string activityInstID);

        /// <summary>
        /// 获取待办工作项列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="parameters">参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        IPageOfList<WorkItem> GetMyWorkItems(string userID, IDictionary<string, object> parameters, string sortCommand, PageInfo pageInfo, bool includeAuto = false);

        /// <summary>
        /// 修改工作项状态
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="status">工作项状态</param>
        void UpdateWorkItemStatus(string workItemID, WorkItemStatus status);

        /// <summary>
        /// 修改活动状态
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        /// <param name="status">活动状态</param>
        void UpdateActivityInstStatus(string activityInstID, ActivityInstStatus status);

        /// <summary>
        ///修改流程状态 
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="status">流程状态</param>
        void UpdateProcessInstStatus(string processInstID, ProcessInstStatus status);

        /// <summary>
        /// 获取活动参与者
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityDefID">活动定义ID</param>
        /// <returns></returns>
        string GetParticipant(string processDefID, string activityDefID);

        #endregion

        #region Repository
        IRepository<string> Repository
        {
            get;
        }
        #endregion
    }
}
