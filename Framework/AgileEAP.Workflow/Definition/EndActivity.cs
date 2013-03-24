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
    [Remark("结束活动")]
   
    public class EndActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 激活规则
        /// </summary>
        public ActivateRule ActivateRule
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public EndActivity()
        {
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public EndActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            ActivateRule = new ActivateRule(this, xElem.Element("activateRule"));
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            base.Initilize();

            ActivateRule = new ActivateRule();
        }
        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                                Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                Properties.Select(o => new XElement(o.Key, o.Value)),
                               ActivateRule != null ? ActivateRule.ToXElement() : null,
                                Style.ToXElement());
        }
        #endregion
    }
}
