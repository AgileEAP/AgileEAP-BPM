using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 表单列
    /// </summary>
   
    public class FormColumn : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 序号
        /// </summary>
        
        public string Index
        {
            get
            {
                return Attibutes.GetSafeValue<string>("index");
            }
            set
            {
                Attibutes.SafeAdd("index", value);
            }
        }

        /// <summary>
        /// 列名
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

        /// <summary>
        /// 类型
        /// </summary>
        
        public string Type
        {
            get
            {
                return Attibutes.GetSafeValue<string>("type");
            }
            set
            {
                Attibutes.SafeAdd("type", value);
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
        public FormColumn()
        {
            ElementName = "column";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public FormColumn(ConfigureElement parent, XElement xElem)
            : base(parent, "column", xElem)
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