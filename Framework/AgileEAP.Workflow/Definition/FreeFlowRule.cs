using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 自由流规则
    /// </summary>
 
    public class FreeFlowRule : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 该活动是否为自由活动
        /// </summary>
        
        public bool IsFreeActivity
        {
            get
            {
                return Properties.GetSafeValue<bool>("isFreeActivity");
            }
            set
            {
                Properties.SafeAdd("isFreeActivity", value);
            }
        }

        /// <summary>
        /// 自由范围设置策略
        /// </summary>
        
        public FreeRangeStrategy FreeRangeStrategy
        {
            get
            {
                return Properties.GetSafeValue<FreeRangeStrategy>("freeRangeStrategy", FreeRangeStrategy.FreeWithinNextActivites);
            }
            set
            {
                Properties.SafeAdd("freeRangeStrategy", value);
            }
        }

        /// <summary>
        /// 指定活动表范围内自由
        /// </summary>
        
        public List<Activity> FreeRangeActivities
        {
            get;
            set;
        }

        /// <summary>
        /// 流向的目标活动仅限于人式活动
        /// </summary>
        
        public bool IsOnlyLimitedManualActivity
        {
            get
            {
                return Properties.GetSafeValue<bool>("isOnlyLimitedManualActivity");
            }
            set
            {
                Properties.SafeAdd("isOnlyLimitedManualActivity", value);
            }
        }
        #endregion

        #region Construtor
        public FreeFlowRule()
        {
            ElementName = "freeFlowRule";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public FreeFlowRule(ConfigureElement parent, XElement xElem)
            : base(parent, "freeFlowRule", xElem)
        { }

        #endregion
    }
}
