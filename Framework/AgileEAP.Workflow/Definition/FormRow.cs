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
    /// <summary>
    /// 表单行
    /// </summary>
   
    public class FormRow : ConfigureElement
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
        /// 表单列
        /// </summary>
        
        public List<FormColumn> Columns
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        public FormRow()
        {
            ElementName = "row";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public FormRow(ConfigureElement parent, XElement xElem)
            : base(parent, "row", xElem)
        {
            Columns = xElem.Elements("column").Select(o => new FormColumn(this, o)).ToList();
        }

        #endregion

        #region Methods
        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                       Columns.Select(o => o.ToXElement()));
        }

        #endregion
    }
}
