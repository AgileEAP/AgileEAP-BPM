using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
using AgileEAP.Core.Data;

namespace AgileEAP.Workflow.Engine
{
    public class WorkItemRuntime
    {
        #region Variables
        private IEngineService engineService;
        #endregion

        #region Properties
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

        public IUser User
        {
            get;
            set;
        }

        public WorkItemContext Context
        {
            get;
            set;
        }

        #endregion

        public WorkItemRuntime(IEngineService services, WorkItemContext context)
        {
            this.engineService = services;
            Context = context;
        }

        public void Run(object state)
        {
            try
            {
                EventPublisher.Publish<WorkItemExecutingEvent>(new WorkItemExecutingEvent()
                {
                    UUID = string.Format("{0}-{1}-{2}-WorkItemExecutingEvent", Context.ProcessDefine.ID, Context.ProcessDefine.Version, Context.Activity.ID),
                    Context = Context
                });

                CompleteWorkItem(WFUtil.User);

                EventPublisher.Publish<WorkItemExecutedEvent>(new WorkItemExecutedEvent()
                {
                    UUID = string.Format("{0}-{1}-{2}-WorkItemExecutedEvent", Context.ProcessDefine.ID, Context.ProcessDefine.Version, Context.Activity.ID),
                    Context = Context
                });
            }
            catch (Exception ex)
            {
                TraceLog traceLog = new TraceLog()
                {
                    ActionType = (short)Operation.Complete,
                    ActivityID = Context.ActivityInst.ActivityDefID,
                    ActivityInstID = Context.ActivityInst.ID,
                    ClientIP = WebUtil.GetClientIP(),
                    ID = IdGenerator.NewComb().ToSafeString(),
                    Operator = WFUtil.User.ID,
                    ProcessID = Context.ProcessInst.ProcessDefID,
                    ProcessInstID = Context.ProcessInst.ID,
                    WorkItemID = Context.WorkItem.ID,
                    Message = string.Format("完成工作项{0}出错,WorkItemID={1}", Context.WorkItem.Name, Context.WorkItem.ID),
                    CreateTime = DateTime.Now
                };
                Persistence.Repository.SaveOrUpdate(traceLog);
                AbortWorkItem();
                WFUtil.HandleException(new WFException(GetType().FullName, string.Format("完成工作项{0}出错,WorkItemID={1}", Context.WorkItem.Name, Context.WorkItem.ID), ex));

                EventPublisher.Publish<WorkItemErrorEvent>(new WorkItemErrorEvent()
                {
                    UUID = string.Format("{0}-{1}-{2}-WorkItemErrorEvent", Context.ProcessDefine.ID, Context.ProcessDefine.Version, Context.Activity.ID),
                    Context = Context
                });
            }
        }

        /// <summary>
        /// 获取待执行活动
        /// </summary>
        /// <param name="processInstID">流程实例ID</param>
        /// <returns></returns>
        public IList<ActivityInst> GetToRunActivityInsts(string processInstID)
        {
            return Persistence.GetActivityInsts(processInstID).Where(a => a.CurrentState == (short)ActivityInstStatus.NoStart
                    || a.CurrentState == (short)ActivityInstStatus.WaitActivate).ToList();
        }

        /// <summary>
        /// 完成工作项
        /// </summary>
        public void CompleteWorkItem(IUser user)
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(WorkItem)))
            {
                Context.WorkItem.CurrentState = (short)WorkItemStatus.Compeleted;
                Context.WorkItem.Executor = WFUtil.User.ID;
                Context.WorkItem.ExecutorName = WFUtil.User.Name;
                Context.WorkItem.ExecuteTime = DateTime.Now;
                Persistence.Repository.SaveOrUpdate(Context.WorkItem);

                TraceLog traceLog = new TraceLog()
                {
                    ActionType = (short)Operation.Complete,
                    ActivityID = Context.ActivityInst.ActivityDefID,
                    ActivityInstID = Context.ActivityInst.ID,
                    ClientIP = WebUtil.GetClientIP(),
                    ID = IdGenerator.NewComb().ToSafeString(),
                    Operator = WFUtil.User.ID,
                    ProcessID = Context.ProcessInst.ProcessDefID,
                    ProcessInstID = Context.ProcessInst.ID,
                    WorkItemID = Context.WorkItem.ID,
                    Message = string.Format("完成工作项{0}", Context.WorkItem.Name),
                    CreateTime = DateTime.Now
                };
                Persistence.Repository.SaveOrUpdate(traceLog);

                trans.Commit();
            }
        }

        public void AbortWorkItem()
        {
            Context.WorkItem.CurrentState = (short)WorkItemStatus.Error;
            Persistence.Repository.SaveOrUpdate(Context.WorkItem);
        }
    }
}
