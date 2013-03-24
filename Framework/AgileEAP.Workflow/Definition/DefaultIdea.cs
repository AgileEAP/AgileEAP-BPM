using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 配置默认可选办理意见
    /// </summary>
    public class DefaultIdea : ConfigureElement
    {
        #region Properties 成员
        public List<FormRow> Ideas
        {
            get;
            set;
        }
        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultIdea()
        {
            ElementName = "defaultIdea";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public DefaultIdea(ConfigureElement parent, XElement xElem)
            : base(parent, "defaultIdea", xElem)
        {
            Ideas = xElem.Elements("row").Select(o => new FormRow(this, o)).ToList();
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
                       Ideas.Select(o => o.ToXElement()));
        }
        #endregion

    }

}
