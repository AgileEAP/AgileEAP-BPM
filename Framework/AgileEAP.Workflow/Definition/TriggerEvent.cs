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
   
    public class TriggerEvent : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get
            {
                return Attibutes.GetSafeValue<string>("id");
            }
            set
            {
                Attibutes.SafeAdd("id", value);
            }
        }

        /// <summary>
        /// 触发时机,创建（create）,启动（start）,结束（terminate）,超时（overtime）,提醒（remind）
        /// </summary>
        public TriggerEventType TriggerEventType
        {
            get
            {
                return Properties.GetSafeValue<TriggerEventType>("triggerEventType", TriggerEventType.WorkItemCompleted);
            }
            set
            {
                Properties.SafeAdd("triggerEventType", value);
            }
        }

        /// <summary>
        /// 调用方式，synchronous同步，asynchronous异步
        /// </summary>
        public InvokePattern InvokePattern
        {
            get
            {
                return Properties.GetSafeValue<InvokePattern>("invokePattern", InvokePattern.Synchronous);
            }
            set
            {
                Properties.SafeAdd("invokePattern", value);
            }
        }

        /// <summary>
        /// 事件动作
        /// </summary>
        public string EventAction
        {
            get
            {
                return Properties.GetSafeValue<string>("eventAction");
            }
            set
            {
                Properties.SafeAdd("eventAction", value);
            }
        }


        #endregion

        #region Construtor
        public TriggerEvent()
        {
            ElementName = "triggerEvent";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public TriggerEvent(ConfigureElement parent, XElement xElem)
            : base(parent, "triggerEvent", xElem)
        {
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
                                Properties.Select(p => new XElement(p.Key, p.Value)));
        }

        #endregion
    }
}
