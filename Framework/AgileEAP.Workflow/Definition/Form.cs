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
    /// 工作流表单
    /// </summary>

    public class Form : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 表单名称
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

        public string Script
        {
            get
            {
                return Attibutes.GetSafeValue<string>("script");
            }
            set
            {
                Attibutes.SafeAdd("script", value);
            }
        }

        public string AutoGenScript
        {
            get
            {
                return Attibutes.GetSafeValue<string>("autoGenScript");
            }
            set
            {
                Attibutes.SafeAdd("autoGenScript", value);
            }
        }

        public string Style
        {
            get
            {
                return Attibutes.GetSafeValue<string>("style");
            }
            set
            {
                Attibutes.SafeAdd("style", value);
            }
        }

        /// <summary>
        /// 数据来源
        /// </summary>

        public string DataSource
        {
            get
            {
                return Attibutes.GetSafeValue<string>("dataSource");
            }
            set
            {
                Attibutes.SafeAdd("dataSource", value);
            }
        }

        /// <summary>
        /// 表单标题
        /// </summary>

        public string Title
        {
            get
            {
                return Attibutes.GetSafeValue<string>("title");
            }
            set
            {
                Attibutes.SafeAdd("title", value);
            }
        }

        /// <summary>
        /// 表单行
        /// </summary>

        public List<FormField> Fields
        {
            get;
            set;
            //get
            //{
            //    return Attibutes.GetSafeValue<List<FormField>>("fields");
            //}
            //set
            //{
            //    Attibutes.SafeAdd("fields", value);
            //}
        }
        #endregion

        #region Construtor
        public Form()
        {
            ElementName = "form";

            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Form(ConfigureElement parent, XElement xElem)
            : base(parent, "form", xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            if (xElem.Elements("field") != null)
                Fields = xElem.Elements("field").Select(o => new FormField(this, o)).ToList();
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            base.Initilize();

            Fields = new List<FormField>();
        }

        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Attibutes != null ? Attibutes.Select(o => o.Value != null ? new XAttribute(o.Key, o.Value) : null) : null,
                      Fields != null ? Fields.Select(o => o.ToXElement()) : null);
        }

        #endregion
    }
}
