using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 工作项上下文
    /// </summary>
    public class WorkItemContext
    {
        #region Properties

        public ProcessDefine ProcessDefine
        {
            get;
            set;
        }

        public Activity Activity
        {
            get
            {
                return ProcessDefine.Activities.FirstOrDefault(o => o.ID == ActivityInst.ActivityDefID);
            }
        }

        /// <summary>
        /// 流程实例
        /// </summary>
        public ProcessInst ProcessInst
        {
            get;
            set;
        }

        /// <summary>
        /// 活动实例
        /// </summary>
        public ActivityInst ActivityInst
        {
            get;
            set;
        }

        /// <summary>
        /// 当前工作项
        /// </summary>
        public WorkItem WorkItem
        {
            get;
            set;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get;
            set;
        }
        #endregion

        #region Contructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="engineContext">流程引擎上下文</param>
        /// <param name="processInst">流程实例</param>
        /// <param name="activityInst">活动实例</param>
        /// <param name="workItem">工作项</param>
        public WorkItemContext(ProcessInst processInst, ActivityInst activityInst, WorkItem workItem)
        {
            this.ProcessInst = processInst;
            this.ActivityInst = activityInst;
            this.WorkItem = workItem;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkItemContext()
        { }
        #endregion
    }
}
