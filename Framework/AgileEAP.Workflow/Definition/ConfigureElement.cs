using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.IO;
using System.Web;

using AgileEAP.Core.Extensions;
using AgileEAP.Core;
using Newtonsoft.Json;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// Xml元素基类
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
   
    public class ConfigureElement : IConfigureElement
    {
        protected ILogger log = LogManager.GetLogger(typeof(ConfigureElement));

        #region Properties 成员
        /// <summary>
        /// 元素名称
        
        public string ElementName
        {
            get;
            set;
        }

        /// <summary>
        /// 元素文本值
        /// </summary>
        public string ElementValue
        {
            get;
            set;
        }

        /// <summary>
        /// 关联XElement对象
        /// </summary>
        [JsonIgnore]
        protected XElement Current
        {
            get;
            set;
        }

        /// <summary>
        /// 父结点
        /// </summary>
        [JsonIgnore]
        protected ConfigureElement Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 特性列表
        /// </summary>
        [JsonIgnore]
        protected Dictionary<string, object> Attibutes
        {
            get;
            set;
        }

        /// <summary>
        ///属性列表
        /// </summary>
        [JsonIgnore]
        protected Dictionary<string, object> Properties
        {
            get;
            set;
        }

        //private static string xmlPath = "WorkflowDefine";
        ///// <summary>
        ///// xml配置文件
        ///// </summary>
        //public static string XmlPath
        //{
        //    get
        //    {
        //        if (!Path.IsPathRooted(xmlPath))
        //        {
        //            string executePath = AppDomain.CurrentDomain.BaseDirectory;

        //            if (!xmlPath.StartsWith(executePath))
        //                xmlPath = Path.Combine(executePath, xmlPath);
        //        }
        //        if (!Directory.Exists(xmlPath)) Directory.CreateDirectory(xmlPath);

        //        return xmlPath;
        //    }
        //    set
        //    {
        //        xmlPath = value;
        //    }
        //}
        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigureElement()
        {
            ElementName = "workflow";

            Attibutes = new Dictionary<string, object>();
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        public ConfigureElement(ConfigureElement parent, string nodeName)
            : this(parent, nodeName, string.Empty)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        /// <param name="xElem"></param>
        public ConfigureElement(ConfigureElement parent, string nodeName, XElement xElem)
            : this(parent, nodeName, string.Empty, xElem)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeValue"></param>
        public ConfigureElement(ConfigureElement parent, string nodeName, string nodeValue)
            : this(parent, nodeName, nodeValue, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeValue"></param>
        /// <param name="xElem"></param>
        public ConfigureElement(ConfigureElement parent, string nodeName, string nodeValue, XElement xElem)
        {
            Parent = parent;
            ElementName = nodeName;
            ElementValue = nodeValue;

            Attibutes = new Dictionary<string, object>();
            Properties = new Dictionary<string, object>();

            if (xElem == null) return;

            Current = (xElem.Name.LocalName == nodeName) ? xElem : xElem.Element(nodeName) ?? xElem.Descendants(nodeName).FirstOrDefault();
            Initilize(Current);
        }

        #endregion

        #region Methods
        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public virtual XElement ToXElement()
        {
            return new XElement(ElementName,
                                Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                Properties.Select(o => new XElement(o.Key, o.Value)),
                                ElementValue);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="configFile"></param>
        public virtual void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            ConfigureElement parent = Parent;
            try
            {
                if (parent == null)
                {
                    ToXElement().Save(fileName);
                    return;
                }

                if (this.GetType() == typeof(ProcessDefine))
                {
                    ToXElement().Save(fileName);
                    return;
                }

                while (parent != null && parent.Parent != null) parent = parent.Parent;

                parent.Save(fileName);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Save configure file {0} error!", fileName), ex);
            }
        }

        //public virtual void Save()
        //{
        //    string configFile = string.Empty;

        //    ConfigureElement parent = this;
        //    while (parent != null && parent.Parent != null) parent = parent.Parent;

        //    if (parent is ProcessDefine)
        //    {
        //        ProcessDefine wf = (ProcessDefine)parent;
        //        Save(System.IO.Path.Combine(XmlPath, string.Format("{0}{1}.xml", wf.Name, wf.Version)));

        //        return;
        //    }

        //    throw new Exception("获取不到默认路径！");
        //}

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initilize()
        { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xElem"></param>
        public virtual void Initilize(XElement xElem)
        {
            if (xElem == null) return;

            xElem.Attributes()
                 .ForEach(e => { if (!string.IsNullOrEmpty(e.Name.LocalName) && !string.IsNullOrEmpty(e.Value))Attibutes.SafeAdd(e.Name.LocalName, e.Value.Trim()); });

            xElem.Elements()
                 .Where(e => e.Elements().Count() == 0)
                 .ForEach(e => { if (!string.IsNullOrEmpty(e.Name.LocalName) && !string.IsNullOrEmpty(e.Value)) Properties.SafeAdd(e.Name.LocalName, e.Value.Trim()); });
        }

        /// <summary>
        /// 转换为xml字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToXml()
        {
            return ToXElement().ToSafeString();
        }

        /// <summary>
        /// 新建参数
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        public KeyValuePair<string, object> NewArgument(XElement xElem)
        {
            return new KeyValuePair<string, object>(xElem.Attribute("name").Value, xElem.Value);
        }

        /// <summary>
        /// 初始化属性值
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        public Dictionary<string, object> InitProperties(XElement xElem)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            if (xElem == null) return properties;
            var xProperties = xElem.Element("properties");
            if (xProperties != null)
            {
                var temp = xProperties.Elements("property").ToArray();

                foreach (var xProperty in temp)
                {
                    string key = xProperty.AttributeValue("name");
                    if (!string.IsNullOrEmpty(key))
                        properties.SafeAdd(key, xProperty.AttributeValue("value"));
                }
            }

            return properties;
        }
        #endregion
    }
}

