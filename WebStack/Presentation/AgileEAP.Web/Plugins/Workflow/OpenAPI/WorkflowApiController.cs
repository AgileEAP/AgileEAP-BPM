using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Utility;

using AgileEAP.Core.Caching;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Plugin.Workflow.OpenAPI
{
    public class WorkflowApiController : System.Web.Http.ApiController
    {
        private readonly IWorkflowEngine workflowEngine = null;
        public WorkflowApiController(IWorkflowEngine workflowEngine)
        {
            this.workflowEngine = workflowEngine;
        }

        #region 工作流管理

        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="parentProcessDefID">父流程ID</param>
        /// <param name="parentActivityID">父活动ID</param>
        /// <returns></returns>
        public string CreateAProcess(string processDefID, string parentProcessDefID = null, string parentActivityID = null)
        {
            return workflowEngine.CreateAProcess(processDefID, parentProcessDefID, parentActivityID);
        }

        /// <summary>
        /// 开始启动一个流程
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        ///<param name="parameters">启动参数</param>
        public void StartAProcess(string processInstID, IDictionary<string, object> parameters = null)
        {
            workflowEngine.StartAProcess(processInstID, parameters);
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <returns></returns>
        public ProcessDefine GetProcessDefine(string processDefID)
        {
            return workflowEngine.GetProcessDefine(processDefID);
        }
        #endregion

        #region 流程实例管理
        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void SuspendProcessInst(string processInstID)
        {
            workflowEngine.SuspendProcessInst(processInstID);
        }

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void ResumeProcessInst(string processInstID)
        {
            workflowEngine.ResumeProcessInst(processInstID);
        }

        /// <summary>
        /// 终止流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void TerminateProcessInst(string processInstID)
        {
            workflowEngine.TerminateProcessInst(processInstID);
        }

        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        public void DeleteProcessInst(string processInstID)
        {
            workflowEngine.DeleteProcessInst(processInstID);
        }
        #endregion

        #region 活动实例管理
        /// <summary>
        /// 完成活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void CompleteActivityInst(string activityInstID)
        {
            workflowEngine.CompleteActivityInst(activityInstID);
        }

        /// <summary>
        /// 重启活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ResetActivityInst(string activityInstID)
        {
            workflowEngine.ResetActivityInst(activityInstID);
        }

        /// <summary>
        /// 挂起活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void SuspendActivityInst(string activityInstID)
        {
            workflowEngine.SuspendActivityInst(activityInstID);
        }

        /// <summary>
        /// 恢复活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ResumeActivityInst(string activityInstID)
        {
            workflowEngine.ResumeActivityInst(activityInstID);
        }

        /// <summary>
        /// 激活活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ActivateActivityInst(string activityInstID)
        {
            workflowEngine.ActivateActivityInst(activityInstID);
        }

        /// <summary>
        /// 终止活动实例
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void TerminateActivityInst(string activityInstID)
        {
            workflowEngine.TerminateActivityInst(activityInstID);
        }

        /// <summary>
        /// 将活动实例置为出错
        /// </summary>
        /// <param name="activityInstID">活动实例ID</param>
        public void ErrorActivityInst(string activityInstID)
        {
            workflowEngine.ErrorActivityInst(activityInstID);
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
            return workflowEngine.GetMyWorkItems(userID, parameters, sortCommand, pageInfo, includeAuto);
        }

        /// <summary>
        /// 完成一个工作项
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="parameters">参数</param>
        public void CompleteWorkItem(string workItemID, IDictionary<string, object> parameters = null, Func<Participantor> assignParticipant = null)
        {
            workflowEngine.CompleteWorkItem(workItemID, parameters, assignParticipant);
        }

        /// <summary>
        /// 删除工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        public void DeleteWorkItem(string workItemID)
        {
            workflowEngine.DeleteWorkItem(workItemID);
        }

        /// <summary>
        /// 领取工作项
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="workItemID">工作项ID</param>
        public void FetchWorkItem(IUser user, string workItemID)
        {
            workflowEngine.FetchWorkItem(user, workItemID);
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
            workflowEngine.RollbackWorkItem(user, workItemID);
        }


        /// <summary>
        /// 获取当前活动，可回退的活动列表
        /// </summary>
        /// <param name="processDefID">流程定义ID</param>
        /// <param name="activityID">当前活动ID</param>
        /// <returns></returns>
        public IList<Activity> GetRollbackActivityList(string processDefID, string activityID)
        {
            return workflowEngine.GetRollbackActivityList(processDefID, activityID);
        }

        #endregion

        #region 参与者接口
        /// <summary>
        /// 获取角色和组织参与者
        /// </summary>
        /// <returns></returns>
        public IList<Participantor> GetRoleAndOrgParticipantors()
        {
            return workflowEngine.GetRoleAndOrgParticipantors();
        }

        /// <summary>
        /// 获取某参与者类型下的所有参与者
        /// </summary>
        /// <param name="parentType">父参与者类型</param>
        /// <param name="parentID">父参与者ID</param>
        /// <returns></returns>
        public IList<Participantor> GetPersonParticipantors(ParticipantorType parentType, string parentID)
        {
            return workflowEngine.GetPersonParticipantors(parentType, parentID);
        }
        #endregion

    }
}
