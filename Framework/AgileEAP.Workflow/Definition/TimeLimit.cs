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
    /// 超时设置
    /// </summary>
   
    public class TimeLimit : ConfigureElement
    {
        /// <summary>
        /// 是否启用超时设置
        /// </summary>
        public bool IsTimeLimitSet
        {
            get
            {
                return Properties.GetSafeValue<bool>("isTimeLimitSet");
            }
            set
            {
                Properties.SafeAdd("isTimeLimitSet", value);
            }
        }

        /// <summary>
        /// 工作日历
        /// </summary>
        public CalendarSet CalendarSet
        {
            get;
            set;
        }

        /// <summary>
        /// 超时信息
        /// </summary>
        public TimeLimitInfo TimeLimitInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 提醒信息
        /// </summary>
        public RemindInfo RemindInfo
        {
            get;
            set;
        }


        #region Construtor
        public TimeLimit()
        {
            ElementName = "timeLimit";
            Properties = new Dictionary<string, object>();
            CalendarSet = new CalendarSet();
            TimeLimitInfo = new TimeLimitInfo();
            RemindInfo = new RemindInfo();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public TimeLimit(ConfigureElement parent, XElement xElem)
            : base(parent, "timeLimit", xElem)
        {
            if (xElem != null)
            {
                CalendarSet = new CalendarSet(this, xElem.Element("calendar"));
                TimeLimitInfo = new TimeLimitInfo(this, xElem.Element("timeLimitInfo"));
                RemindInfo = new RemindInfo(this, xElem.Element("remindInfo"));
            }
        }

        #endregion

        #region Methods
        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                                Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                Properties.Select(o => new XElement(o.Key, o.Value)),
                                CalendarSet != null ? CalendarSet.ToXElement() : null,
                                TimeLimitInfo != null ? TimeLimitInfo.ToXElement() : null,
                                RemindInfo != null ? RemindInfo.ToXElement() : null);
        }

        #endregion
    }
}
