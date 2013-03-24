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
    /// 表单字段
    /// </summary>

    public class FormField : ConfigureElement
    {
        #region Properties 成员

        /// <summary>
        /// 序号
        /// </summary>

        public int SortOrder
        {
            get
            {
                return Properties.GetSafeValue<int>("index");
            }
            set
            {
                Properties.SafeAdd("index", value);
            }
        }

        /// <summary>
        /// 字段名
        /// </summary>

        public string Name
        {
            get
            {
                return Properties.GetSafeValue<string>("name");
            }
            set
            {
                Properties.SafeAdd("name", value);
            }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string ExtendData
        {
            get
            {
                return Properties.GetSafeValue<string>("extendData");
            }
            set
            {
                Properties.SafeAdd("extendData", value);
            }
        }

        /// <summary>
        /// 显示名
        /// </summary>

        public string Text
        {
            get
            {
                return Properties.GetSafeValue<string>("text");
            }
            set
            {
                Properties.SafeAdd("text", value);
            }
        }

        /// <summary>
        /// 数据来源
        /// </summary>

        public string DataSource
        {
            get
            {
                return Properties.GetSafeValue<string>("dataSource");
            }
            set
            {
                Properties.SafeAdd("dataSource", value);
            }
        }

        /// <summary>
        /// 是否必需的
        /// </summary>

        public bool Required
        {
            get
            {
                return Properties.GetSafeValue<bool>("required", false);
            }
            set
            {
                Properties.SafeAdd("required", value);
            }
        }

        /// <summary>
        /// 绑定控件
        /// </summary>

        public ControlType ControlType
        {
            get
            {
                return Properties.GetSafeValue<ControlType>("controlType", ControlType.TextBox);
            }
            set
            {
                Properties.SafeAdd("controlType", value);
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>

        public DataType DataType
        {
            get
            {
                return Properties.GetSafeValue<DataType>("dataType", DataType.String);
            }
            set
            {
                Properties.SafeAdd("dataType", value);
            }
        }

        /// <summary>
        /// 访问类型
        /// </summary>

        public AccessPattern AccessPattern
        {
            get
            {
                return Properties.GetSafeValue<AccessPattern>("accessPattern", AccessPattern.ReadOnly);
            }
            set
            {
                Properties.SafeAdd("accessPattern", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public string DefaultValue
        {
            get
            {
                return Properties.GetSafeValue<string>("defaultValue");
            }
            set
            {
                Properties.SafeAdd("defaultValue", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public int Rows
        {
            get
            {
                return Properties.GetSafeValue<int>("rows", 1);
            }
            set
            {
                Properties.SafeAdd("rows", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public int Cols
        {
            get
            {
                return Properties.GetSafeValue<int>("cols", 1);
            }
            set
            {
                Properties.SafeAdd("cols", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public string URL
        {
            get
            {
                return Properties.GetSafeValue<string>("url");
            }
            set
            {
                Properties.SafeAdd("url", value);
            }
        }

        public string X
        {
            get
            {
                return Properties.GetSafeValue<string>("x");
            }
            set
            {
                Properties.SafeAdd("x", value);
            }
        }
        public string Y
        {
            get
            {
                return Properties.GetSafeValue<string>("y");
            }
            set
            {
                Properties.SafeAdd("y", value);
            }
        }
        public string Z
        {
            get
            {
                return Properties.GetSafeValue<string>("z");
            }
            set
            {
                Properties.SafeAdd("z", value);
            }
        }
        public int Width
        {
            get
            {
                return Properties.GetSafeValue<int>("width");
            }
            set
            {
                Properties.SafeAdd("width", value);
            }
        }
        public int Height
        {
            get
            {
                return Properties.GetSafeValue<int>("height");
            }
            set
            {
                Properties.SafeAdd("height", value);
            }
        }

        public string CustomStyle
        {
            get
            {
                return Properties.GetSafeValue<string>("customData");
            }
            set
            {
                Properties.SafeAdd("customData", value);
            }
        }

        /// <summary>
        /// 表单列
        /// </summary>

        public List<ListItem> ListItems
        {
            get;
            set;
        }
        public string Container
        {
              get
            {
                return Properties.GetSafeValue<string>("container");
            }
            set
            {
                Properties.SafeAdd("container", value);
            }
        }
        #endregion

        #region Construtor
        public FormField()
        {
            ElementName = "field";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public FormField(ConfigureElement parent, XElement xElem)
            : base(parent, "field", xElem)
        {
            if (xElem.Element("list") != null)
                ListItems = xElem.Element("list").Elements("listItem").Select(o => new ListItem(this, o)).ToList();
        }

        #endregion

        #region Methods
        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Attibutes.Select(o => new XAttribute(o.Key, o.Value)), Properties.Select(o => new XElement(o.Key, o.Value)),
                      ListItems != null ? new XElement("list", ListItems.Select(o => o.ToXElement())) : null);
        }

        #endregion
    }
}