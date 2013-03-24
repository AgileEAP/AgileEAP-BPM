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
    /// 提醒信息
    /// </summary>
   
    public class RemindInfo : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 提醒类型
        /// </summary>
        
        public RemindType RemindType
        {
            get
            {
                return Properties.GetSafeValue<RemindType>("remindType", RemindType.Email);
            }
            set
            {
                Properties.SafeAdd("remindType", value);
            }
        }

        /// <summary>
        /// 提醒策略
        /// </summary>
        
        public RemindStrategy RemindStrategy
        {
            get
            {
                return Properties.GetSafeValue<RemindStrategy>("remindStrategy", RemindStrategy.RemindLimtTime);
            }
            set
            {
                Properties.SafeAdd("remindStrategy", value);
            }
        }


        /// <summary>
        /// 天
        /// </summary>
        
        public int RemindLimtTimeDay
        {
            get
            {
                return Properties.GetSafeValue<int>("remindLimtTimeDay");
            }
            set
            {
                Properties.SafeAdd("remindLimtTimeDay", value);
            }
        }

        /// <summary>
        /// 小时
        /// </summary>
        
        public int RemindLimtTimeHour
        {
            get
            {
                return Properties.GetSafeValue<int>("remindLimtTimeHour");
            }
            set
            {
                Properties.SafeAdd("remindLimtTimeHour", value);
            }
        }

        /// <summary>
        /// 分钟
        /// </summary>
        
        public int RemindLimtTimeMinute
        {
            get
            {
                return Properties.GetSafeValue<int>("remindLimtTimeMinute");
            }
            set
            {
                Properties.SafeAdd("remindLimtTimeMinute", value);
            }
        }

        /// <summary>
        /// 提醒相关数据
        /// </summary>
        
        public string RemindRelevantData
        {
            get
            {
                return Properties.GetSafeValue<string>("remindRelevantData");
            }
            set
            {
                Properties.SafeAdd("remindRelevantData", value);
            }
        }

        /// <summary>
        /// 是否发送提醒信息
        /// </summary>
        
        public bool IsSendMessageForRemind
        {
            get
            {
                return Properties.GetSafeValue<bool>("isSendMessageForRemind");
            }
            set
            {
                Properties.SafeAdd("isSendMessageForRemind", value);
            }
        }

        #endregion

        #region Construtor
        public RemindInfo()
        {
            ElementName = "remindInfo";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public RemindInfo(ConfigureElement parent, XElement xElem)
            : base(parent, "remindInfo", xElem)
        { }

        #endregion
    }
}
