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
    [Remark("子流程")]
   
    public class SubflowActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 调用方式，synchronous同步，asynchronous异步
        /// </summary>
        public InvokePattern InvokePattern
        {
            get;
            set;
        }

        /// <summary>
        /// 子流程
        /// </summary>
        public string SubProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 是否多子流程
        /// </summary>
        public bool IsMultiSubProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 关联数据
        /// </summary>
        public string IterationRelevantData
        {
            get;
            set;
        }

        /// <summary>
        /// 关联变量名
        /// </summary>
        public string IterationVariableName
        {
            get;
            set;
        }


        /// <summary>
        /// 激动规则
        /// </summary>
        public ActivateRule ActivateRule
        {
            get;
            set;
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
        public SubflowActivity()
        {
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public SubflowActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            ActivateRule = new ActivateRule(this, xElem.Element("activateRule"));
            XElement subElem = xElem.Element("subflowActivity");

            if (subElem != null)
            {
                if (subElem.Element("subProcess") != null)
                    SubProcess = subElem.Element("subProcess").Value;

                if (subElem.Element("InvokePattern") != null)
                    InvokePattern = subElem.Element("InvokePattern").Value.Cast<InvokePattern>(InvokePattern.Synchronous);

                if (subElem.Element("isMultiSubProcess") != null)
                    IsMultiSubProcess = subElem.Element("isMultiSubProcess").Value.Cast<bool>(false);

                if (subElem.Element("iterationRelevantData") != null)
                    IterationRelevantData = subElem.Element("iterationRelevantData").Value;

                if (subElem.Element("iterationVariableName") != null)
                    IterationVariableName = subElem.Element("iterationVariableName").Value;

                if (subElem.Element("triggerEvents") != null && subElem.Element("triggerEvents").Elements("triggerEvent") != null)
                    TriggerEvents = subElem.Element("triggerEvents").Elements("triggerEvent")
                                   .Select(e => new TriggerEvent(this, e)).ToList();
            }

            RollBack = new CustomAction("rollBack", this, xElem.Element("rollBack"));
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            base.Initilize();

            ActivateRule = new ActivateRule();
            TriggerEvents = new List<TriggerEvent>();
            RollBack = new CustomAction("rollBack", this, null);
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
                                new XElement("subflowActivity",
                                    new XElement("subProcess", SubProcess),
                                    new XElement("invokePattern", InvokePattern),
                                    new XElement("isMultiSubProcess", IsMultiSubProcess),
                                    new XElement("iterationRelevantData", IterationRelevantData),
                                    new XElement("iterationVariableName", IterationVariableName),
                                    new XElement("triggerEvents", TriggerEvents != null ? TriggerEvents.Select(o => o.ToXElement()) : null),
                                    RollBack != null ? RollBack.ToXElement() : null));
        }
        #endregion
    }
}
