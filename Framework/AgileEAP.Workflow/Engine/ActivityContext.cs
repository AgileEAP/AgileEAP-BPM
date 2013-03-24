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
    /// 活动上下文
    /// </summary>
    public class ActivityContext
    {
        #region Properties

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
        public ActivityContext(ProcessInst processInst, ActivityInst activityInst)
        {
            this.ProcessInst = processInst;
            this.ActivityInst = activityInst;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ActivityContext()
        { }
        #endregion
    }
}
