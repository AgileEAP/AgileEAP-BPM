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
    /// 超时信息
    /// </summary>
   
    public class TimeLimitInfo : ConfigureElement
    {
        #region Properties

        /// <summary>
        /// 超时提醒策略
        /// </summary>
        public TimeLimitStrategy TimeLimitStrategy
        {
            get
            {
                return Properties.GetSafeValue<TimeLimitStrategy>("timeLimitStrategy", TimeLimitStrategy.LimitTime);
            }
            set
            {
                Properties.SafeAdd("timeLimitStrategy", value);
            }
        }


        /// <summary>
        /// 天
        /// </summary>
        public int LimitTimeDay
        {
            get
            {
                return Properties.GetSafeValue<int>("limitTimeDay");
            }
            set
            {
                Properties.SafeAdd("limitTimeDay", value);
            }
        }

        /// <summary>
        /// 小时
        /// </summary>
        public int LimitTimeHour
        {
            get
            {
                return Properties.GetSafeValue<int>("limitTimeHour");
            }
            set
            {
                Properties.SafeAdd("limitTimeHour", value);
            }
        }

        /// <summary>
        /// 分钟
        /// </summary>
        public int LimitTimeMinute
        {
            get
            {
                return Properties.GetSafeValue<int>("limitTimeMinute");
            }
            set
            {
                Properties.SafeAdd("limitTimeMinute", value);
            }
        }

        /// <summary>
        /// 相关数据
        /// </summary>
        public string RelevantData
        {
            get
            {
                return Properties.GetSafeValue<string>("relevantData");
            }
            set
            {
                Properties.SafeAdd("relevantData", value);
            }
        }

        /// <summary>
        /// 是否发送超时信息
        /// </summary>
        public bool IsSendMessageForOvertime
        {
            get
            {
                return Properties.GetSafeValue<bool>("isSendMessageForOvertime");
            }
            set
            {
                Properties.SafeAdd("isSendMessageForOvertime", value);
            }
        }

        #endregion

        #region Construtor
        public TimeLimitInfo()
        {
            ElementName = "timeLimitInfo";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public TimeLimitInfo(ConfigureElement parent, XElement xElem)
            : base(parent, "timeLimitInfo", xElem)
        { }

        #endregion
    }
}
