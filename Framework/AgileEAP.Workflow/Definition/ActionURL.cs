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
    /// 响应URL
    /// </summary>
   
    public class ActionURL : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 是否自定义URL
        /// </summary>
        
        public bool IsSpecifyURL
        {
            get
            {
                return Properties.GetSafeValue<bool>("isSpecifyURL", false);
            }
            set
            {
                Properties.SafeAdd("isSpecifyURL", value);
            }
        }

        /// <summary>
        /// URL类型,LogicalProcess(逻辑处理),WebURL(web展现),Other(其他)
        /// </summary>
        
        public URLType URLType
        {
            get
            {
                return Properties.GetSafeValue<URLType>("urlType", URLType.CustomURL);
            }
            set
            {
                Properties.SafeAdd("urlType", value);
            }
        }

        /// <summary>
        /// 调用URL
        /// </summary>
        
        public string SpecifyURL
        {
            get
            {
                return Properties.GetSafeValue<string>("specifyURL");
            }
            set
            {
                Properties.SafeAdd("specifyURL", value);
            }
        }

        /// <summary>
        /// 人工处理
        /// </summary>
        
        public CustomAction ManualProcess
        {
            get;
            set;
        }
        #endregion

        #region Construtor
        public ActionURL()
        {
            ElementName = "actionURL";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ActionURL(ConfigureElement parent, XElement xElem)
            : base(parent, "actionURL", xElem)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="elemName">元素名</param>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ActionURL(string elemName, ConfigureElement parent, XElement xElem)
            : base(parent, elemName, xElem)
        {
            if (xElem == null) return;

            ManualProcess = new CustomAction("manualProcess", this, xElem.Element("manualProcess"));
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
                       ManualProcess != null ? ManualProcess.ToXElement() : null
                       );
        }

        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <param name="elemName">特定的elemName</param>
        /// <returns></returns>
        public XElement ToXElement(string elemName)
        {
            return new XElement(elemName,
                       Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                       Properties.Select(p => new XElement(p.Key, p.Value)),
                       ManualProcess != null ? ManualProcess.ToXElement() : null
                       );
        }
        #endregion
    }
}
