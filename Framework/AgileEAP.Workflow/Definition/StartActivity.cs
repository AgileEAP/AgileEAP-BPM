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
    [Remark("开始活动")]

    public class StartActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 工作流表单
        /// </summary>

        public string eForm
        {
            get
            {
                return Properties.GetSafeValue<string>("eForm");
            }
            set
            {
                Properties.SafeAdd("eForm", value);
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public StartActivity()
        {
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public StartActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }
        }

        #endregion

        #region Methods

        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                                Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                Properties.Select(p => new XElement(p.Key, p.Value)),
                                new XElement("parameters", Parameters != null ? Parameters.Select(p => p.ToXElement()) : null),
                                Style.ToXElement()
                               );

        }

        #endregion
    }
}
