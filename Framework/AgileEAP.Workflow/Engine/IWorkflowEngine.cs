using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Enums;
using AgileEAP.Core;
using AgileEAP.Core.Authentication;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 工作流引擎接口
    /// </summary>
    [ServiceContract]
    public interface IWorkflowEngine
    {
        /// <summary>
        /// 流程数据接口
        /// </summary>
        IWorkflowPersistence Persistence
        {
            get;
        }

        /// <summary>
        /// 清除流程缓存
        /// </summary>
        [OperationContract]
        void ClearProcessCache();

        #region 流程管理

        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        string CreateAProcess(string processDefID);

        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="parentProcessDefID">父流程ID</param>
        /// <param name="parentActivityID">父活动ID</param>
        /// <returns></returns>
        [OperationContract]
        string CreateAProcess(string processDefID, string parentProcessDefID, string parentActivityID);

        /// <summary>
        /// 开始启动一个流程
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <param name="parameters">参数</param>
        [OperationContract]
        void StartAProcess(string processInstID, IDictionary<string, object> parameters);

        /// <summary>
        /// 开始启动一个流程
        /// </summary>
        /// <param name="processInstID">启动</param>
        void StartAProcess(string processInstID);

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        [OperationContract]
        ProcessDefine GetProcessDefine(string processDefID);
        #endregion

        #region 流程实例管理
        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        [OperationContract]
        void SuspendProcessInst(string processInstID);

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        [OperationContract]
        void ResumeProcessInst(string processInstID);

        /// <summary>
        /// 终止流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        [OperationContract]
        void TerminateProcessInst(string processInstID);

        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        [OperationContract]
        void DeleteProcessInst(string processInstID);
        #endregion

        #region 活动实例管理
        /// <summary>
        /// 完成活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void CompleteActivityInst(string activityInstID);

        /// <summary>
        /// 重启活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void ResetActivityInst(string activityInstID);

        /// <summary>
        /// 挂起活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void SuspendActivityInst(string activityInstID);

        /// <summary>
        /// 恢复活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void ResumeActivityInst(string activityInstID);

        /// <summary>
        /// 激活活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void ActivateActivityInst(string activityInstID);

        /// <summary>
        /// 终止活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void TerminateActivityInst(string activityInstID);

        /// <summary>
        /// 将活动实例置为出错
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        [OperationContract]
        void ErrorActivityInst(string activityInstID);
        #endregion

        #region 工作项管理
        /// <summary>
        /// 获取待办工作项列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="parameters">参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        [OperationContract]
        IPageOfList<WorkItem> GetMyWorkItems(string userID, IDictionary<string, object> parameters, string sortCommand, PageInfo pageInfo, bool includeAuto = false);

        /// <summary>
        /// 获取当前流程实例待执行工作项及参与者字典
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        [OperationContract]

        IDictionary<WorkItem, IList<Operator>> GetActiveWorkItems(string processInstID);

        /// <summary>
        /// 完成一个工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="parameters">参数</param>
        [OperationContract]
        void CompleteWorkItem(string workItemID, IDictionary<string, object> parameters = null, Func<Participantor> assignParticipant = null);

        /// <summary>
        /// 委托代办工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="agentID">代理用户</param>
        /// <param name="workItemID">工作项ID</param>
        [OperationContract]
        void DelegateWorkItem(IUser user, string agentID, string workItemID);

        /// <summary>
        /// 删除工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        [OperationContract]
        void DeleteWorkItem(string workItemID);

        /// <summary>
        /// 代办工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        [OperationContract]
        void AgentWorkItem(IUser user, string workItemID);

        /// <summary>
        /// 领取工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        [OperationContract]
        void FetchWorkItem(IUser user, string workItemID);
        #endregion

        #region 跳转接口

        /// <summary>
        /// 退回工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        void RollbackWorkItem(IUser user, string workItemID);


        /// <summary>
        /// 获取当前活动，可回退的活动列表
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">当前活动ID</param>
        /// <returns></returns>
        IList<Activity> GetRollbackActivityList(string processDefID, string activityID);

        /// <summary>
        /// 退回工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">回退的源活动实例</param>
        /// <param name="parameters">回退到的目标活动</param>
        void RollbackActivity(IUser user, ActivityInst srcActInst, Activity destActivity);

        /// <summary>
        /// 跳到某个工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        void SkipToWorkItem(IUser user, string workItemID);

        /// <summary>
        /// 获取可跳过的活动列表
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="srcActInst">源活动实例</param>
        IList<Activity> GetSkipActivityList(IUser user, Activity srcAct);

        /// <summary>
        /// 跳到某个活动
        /// <param name="user">当前用户</param>
        /// <param name="srcActInst">源活动实例</param>
        /// <param name="destActivity">目标活动</param>
        void SkipToActivity(IUser user, ActivityInst srcActInst, Activity destActivity);

        #endregion

        #region 参与者接口
        /// <summary>
        /// 获取角色和组织参与者
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Participantor> GetRoleAndOrgParticipantors();

        /// <summary>
        /// 获取某参与者类型下的所有参与者
        /// </summary>
        /// <param name="parentType">父参与者类型</param>
        /// <param name="parentID">父参与者ID</param>
        /// <returns></returns>
        [OperationContract]
        IList<Participantor> GetPersonParticipantors(ParticipantorType parentType, string parentID);
        #endregion
    }
}
