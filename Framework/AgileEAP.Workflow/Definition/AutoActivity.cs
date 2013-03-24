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
    [Remark("自动活动")]
   
    public class AutoActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 激动规则
        /// </summary>
        
        public ActivateRule ActivateRule
        {
            get;
            set;
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        
        public string ExecuteAction
        {
            get
            {
                return Properties.GetSafeValue<string>("executeAction");
            }
            set
            {
                Properties.SafeAdd("executeAction", value);
            }
        }

        /// <summary>
        /// 回滚操作
        /// </summary>
        
        public CustomAction RollBack
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoActivity()
        {
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public AutoActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem == null) return;

            ActivateRule = new ActivateRule(this, xElem.Element("activateRule"));
            XElement elem = xElem.Element("autoActivity");
            if (elem != null)
            {
                TimeLimit = new TimeLimit(this, elem.Element("timeLimit"));
                TriggerEvents = elem.Element("triggerEvents").Elements("triggerEvent").Select(o => new TriggerEvent(this, o)).ToList();
                RollBack = new CustomAction("rollBack", this, elem.Element("rollBack"));
            }
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            base.Initilize();

            ActivateRule = new ActivateRule();
            TimeLimit = new TimeLimit();
            TriggerEvents = new List<TriggerEvent>();
            RollBack = new CustomAction();
        }

        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                               Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                               Properties.Select(p => new XElement(p.Key, p.Value)),
                              ActivateRule != null ? ActivateRule.ToXElement() : null,
                               new XElement("parameters", Parameters != null ? Parameters.Select(p => p.ToXElement()) : null),
                               Style.ToXElement(),
                               new XElement("autoActivity",
                                   TimeLimit != null ? TimeLimit.ToXElement() : null,
                                   new XElement("triggerEvents", TriggerEvents != null ? TriggerEvents.Select(o => o.ToXElement()) : null),
                                   RollBack != null ? RollBack.ToXElement() : null));
        }

        #endregion
    }
}
