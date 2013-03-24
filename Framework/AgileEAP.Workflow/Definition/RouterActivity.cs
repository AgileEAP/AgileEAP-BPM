using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;


namespace AgileEAP.Workflow.Definition
{
    [Remark("路由活动")]
   
    public class RouterActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 是否允许代理
        /// </summary>
        
        public bool AllowAgent
        {
            get
            {
                return Properties.GetSafeValue<bool>("allowAgent");
            }
            set
            {
                Properties.SafeAdd("allowAgent", value);
            }
        }

        /// <summary>
        /// 扩展属性，自定义URL
        /// </summary>
        
        public ActionURL CustomURL
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public RouterActivity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public RouterActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            XElement elem = xElem.Element("routerActivity");

            if (elem != null)
            {
                CustomURL = new ActionURL("customURL", this, elem.Element("customURL"));
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
                               Properties.Select(p => new XElement(p.Key, p.Value)),
                               new XElement("parameters", Parameters != null ? Parameters.Select(p => p.ToXElement()) : null),
                               Style.ToXElement(),
                               new XElement("routerActivity",
                                   CustomURL != null ? CustomURL.ToXElement("customURL") : null,
                                   TimeLimit != null ? TimeLimit.ToXElement() : null,
                                   new XElement("triggerEvents", TriggerEvents != null ? TriggerEvents.Select(o => o.ToXElement()) : null)));
        }
        #endregion
    }
}
