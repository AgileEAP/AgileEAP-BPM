using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 多工作项
    /// </summary>
  
    public class MultiWorkItem : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 是否启动多工作项设置
        /// </summary>
        
        public bool IsMulWIValid
        {
            get
            {
                return Properties.GetSafeValue<bool>("isMulWIValid");
            }
            set
            {
                Properties.SafeAdd("isMulWIValid", value);
            }
        }

        /// <summary>
        /// 多工作项分配策略
        /// </summary>
        
        public WorkItemNumStrategy WorkItemNumStrategy
        {
            get
            {
                return Properties.GetSafeValue<WorkItemNumStrategy>("workItemNumStrategy", WorkItemNumStrategy.ParticipantNumber);
            }
            set
            {
                Properties.SafeAdd("workItemNumStrategy", value);
            }
        }

        /// <summary>
        /// 完成规则设定
        /// </summary>
        
        public FinishRule FinishRule
        {
            get
            {
                return Properties.GetSafeValue<FinishRule>("finishRule", FinishRule.FinishAll);
            }
            set
            {
                Properties.SafeAdd("finishRule", value);
            }
        }

        /// <summary>
        /// 完成个数
        /// </summary>
        
        public int FinishRquiredNum
        {
            get
            {
                return Properties.GetSafeValue<int>("finishRquiredNum");
            }
            set
            {
                Properties.SafeAdd("finishRquiredNum", value);
            }
        }

        /// <summary>
        /// 完成百分比
        /// </summary>
        
        public double FinishRequiredPercent
        {
            get
            {
                return Properties.GetSafeValue<double>("finishRequiredPercent");
            }
            set
            {
                Properties.SafeAdd("finishRequiredPercent", value);
            }
        }

        /// <summary>
        /// 未完成工作项是否自动终止
        /// </summary>
        
        public bool IsAutoCancel
        {
            get
            {
                return Properties.GetSafeValue<bool>("isAutoCancel");
            }
            set
            {
                Properties.SafeAdd("isAutoCancel", value);
            }
        }

        /// <summary>
        /// 是否顺序执行
        /// </summary>
        
        public bool IsSequentialExecute
        {
            get
            {
                return Properties.GetSafeValue<bool>("isSequentialExecute");
            }
            set
            {
                Properties.SafeAdd("isSequentialExecute", value);
            }
        }
        #endregion

        #region Construtor
        public MultiWorkItem()
        {
            ElementName = "multiWorkItem";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public MultiWorkItem(ConfigureElement parent, XElement xElem)
            : base(parent, "multiWorkItem", xElem)
        { }

        #endregion
    }
}
