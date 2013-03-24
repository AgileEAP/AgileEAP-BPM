using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Workflow.Definition
{
    public class ListItem : ConfigureElement
    {
        #region Properties 成员

        /// <summary>
        /// 列名
        /// </summary>
        public string Text
        {
            get
            {
                return Attibutes.GetSafeValue<string>("text");
            }
            set
            {
                Attibutes.SafeAdd("text", value);
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get
            {
                return Attibutes.GetSafeValue<string>("value");
            }
            set
            {
                Attibutes.SafeAdd("value", value);
            }
        }

        #endregion

        #region Construtor
        public ListItem()
        {
            ElementName = "listItem";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ListItem(ConfigureElement parent, XElement xElem)
            : base(parent, "listItem", xElem)
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
            return new XElement(ElementName, Attibutes.Select(o => new XAttribute(o.Key, o.Value)));
        }

        #endregion
    }
}
