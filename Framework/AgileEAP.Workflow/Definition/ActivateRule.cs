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
    /// 激活规则
    /// </summary>
   
    public class ActivateRule : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 激活类型
        /// </summary>
        
        public ActivateRuleType ActivateRuleType
        {
            get
            {
                return Properties.GetSafeValue<ActivateRuleType>("activateRuleType", Enums.ActivateRuleType.DirectRunning);
            }
            set
            {
                Properties.SafeAdd("activateRuleType", value);
            }
        }

        /// <summary>
        /// 响应类型
        /// </summary>
        
        public ActionPattern ActionPattern
        {
            get
            {
                return Properties.GetSafeValue<ActionPattern>("actionPattern", Enums.ActionPattern.Method);
            }
            set
            {
                Properties.SafeAdd("actionPattern", value);
            }
        }

        /// <summary>
        /// 事物失败
        /// </summary>
        
        public SuppressJoinFailure SuppressJoinFailure
        {
            get
            {
                return Properties.GetSafeValue<SuppressJoinFailure>("suppressJoinFailure", SuppressJoinFailure.Suppress);
            }
            set
            {
                Properties.SafeAdd("suppressJoinFailure", value);
            }
        }

        /// <summary>
        /// 应用Uri
        /// </summary>
        
        public string ApplicationUri
        {
            get
            {
                return Properties.GetSafeValue<string>("applicationUri");
            }
            set
            {
                Properties.SafeAdd("applicationUri", value);
            }
        }

        /// <summary>
        ///事物类型
        /// </summary>
        
        public TransactionType TransactionType
        {
            get
            {
                return Properties.GetSafeValue<TransactionType>("transactionType", TransactionType.Join);
            }
            set
            {
                Properties.SafeAdd("transactionType", value);
            }
        }

        /// <summary>
        /// 异常处理策略
        /// </summary>
        
        public ExceptionStrategy ExceptionStrategy
        {
            get
            {
                return Properties.GetSafeValue<ExceptionStrategy>("exceptionStrategy", ExceptionStrategy.Rollback);
            }
            set
            {
                Properties.SafeAdd("exceptionStrategy", value);
            }
        }

        /// <summary>
        /// 调用模式
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
        /// 规则逻辑
        /// </summary>
        
        public string ActivateRuleApp
        {
            get
            {
                return Properties.GetSafeValue<string>("activateRuleApp");
            }
            set
            {
                Properties.SafeAdd("activateRuleApp", value);
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        
        public List<Parameter> Parameters
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        public ActivateRule()
        {
            ElementName = "activateRule";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ActivateRule(ConfigureElement parent, XElement xElem)
            : base(parent, "activateRule", xElem)
        {
            if (xElem == null)
            {
                Parameters = new List<Parameter>();
                return;
            }

            ActivateRuleType = xElem.ChildElementValue("activateRuleType").Cast<ActivateRuleType>(ActivateRuleType.DirectRunning);
            XElement elem = xElem.Element("customAction");

            if (elem == null) return;

            elem.Elements().Where(e => e.Elements().Count() == 0)
                    .ForEach(e => Properties.SafeAdd(e.Name.LocalName, e.Value));
            Parameters = elem.Element("parameters").Elements().Select(e => new Parameter(this, e)).ToList();
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
                               new XElement("activateRuleType", ActivateRuleType),
                               new XElement("customAction",
                                           new XElement("actionPattern", ActionPattern),
                                           new XElement("suppressJoinFailure", SuppressJoinFailure),
                                           new XElement("applicationUri", ApplicationUri),
                                           new XElement("transactionType", TransactionType),
                                           new XElement("exceptionStrategy", ExceptionStrategy),
                                           new XElement("invokePattern", InvokePattern),
                                           new XElement("activateRuleApp", ActivateRuleApp),
                                           new XElement("parameters",
                                           Parameters != null ? Parameters.Select(p => p.ToXElement()) : null))
                              );
        }
        #endregion
    }
}
