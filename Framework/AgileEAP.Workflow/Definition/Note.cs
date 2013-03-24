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
    /// <summary>
    /// 注释
    /// </summary>
  
    public class Note : ConfigureElement
    {
        #region Properties 成员

        /// <summary>
        /// 标题
        /// </summary>
        
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        
        public string Name
        {
            get
            {
                return Attibutes.GetSafeValue<string>("name");
            }
            set
            {
                Attibutes.SafeAdd("name", value);
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public Note()
        {
            ElementName = "note";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Note(ConfigureElement parent, XElement xElem)
            : base(parent, "note", xElem)
        {
            Title = xElem.ChildElementValue("title");
            Content = xElem.ChildElementValue("content");
        }

        #endregion

        #region Methods
        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                new XElement("title", Title),
                                new XElement("content", Content));
        }
        #endregion
    }
}
