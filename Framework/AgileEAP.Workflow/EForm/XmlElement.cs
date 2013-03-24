using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using Newtonsoft.Json;

namespace AgileEAP.EForm
{
    public interface IXmlElement
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        string ElementName
        {
            get;
            set;
        }

        /// <summary>
        /// 元素值
        /// </summary>
        string ElementValue
        {
            get;
            set;
        }

        /// <summary>
        /// 当前结点元素
        /// </summary>
        XElement Current
        {
            get;
            set;
        }

        /// <summary>
        /// 当前结点父元素
        /// </summary>
        IXmlElement Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="xElem"></param>
        void Initilize(XElement xElem);

        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        XElement ToXElement();

        /// <summary>
        /// 保存为文件
        /// </summary>
        /// <param name="xmlFile"></param>
        void Save(string xmlFile);

        /// <summary>
        /// 转换为xml格式的字符串
        /// </summary>
        string ToXml();
    }

    public class XmlElement : IXmlElement
    {
        protected ILogger log = LogManager.GetLogger(typeof(XmlElement));

        #region Properties 成员
        /// <summary>
        /// 元素名称
        /// </summary>
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
        public XElement Current
        {
            get;
            set;
        }

        /// <summary>
        /// 父结点
        /// </summary>
        public IXmlElement Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 特性列表
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Attibutes
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public XmlElement()
        {
            ElementName = "xml";

            Attibutes = new Dictionary<string, object>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        public XmlElement(IXmlElement parent, string nodeName)
            : this(parent, nodeName, string.Empty)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        /// <param name="xElem"></param>
        public XmlElement(IXmlElement parent, string nodeName, XElement xElem)
            : this(parent, nodeName, string.Empty, xElem)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeValue"></param>
        public XmlElement(IXmlElement parent, string nodeName, string nodeValue)
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
        public XmlElement(IXmlElement parent, string nodeName, string nodeValue, XElement xElem)
        {
            Parent = parent;
            ElementName = nodeName;
            ElementValue = nodeValue;

            Attibutes = new Dictionary<string, object>();

            if (xElem == null)
            {
                log.Warn("field xElem is null");
                return;
            }

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
            return new XElement(ElementName, Attibutes.Where(o => o.Value != null && !string.IsNullOrEmpty(o.Value.ToSafeString())).Select(o => new XAttribute(o.Key, o.Value)),
                                ElementValue);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="configFile"></param>
        public virtual void Save(string configFile)
        {
            if (string.IsNullOrEmpty(configFile)) return;

            IXmlElement parent = Parent;
            try
            {
                if (parent == null)
                {
                    ToXElement().Save(configFile);
                    return;
                }

                Type type = this.GetType();
                if (type == typeof(FormView))
                {
                    ToXElement().Save(configFile);
                    return;
                }

                while (parent != null && parent.Parent != null) parent = parent.Parent;

                parent.Save(configFile);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Save configure file {0} error!", configFile), ex);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="xElem"></param>
        public virtual void Initilize(XElement xElem)
        {
            if (xElem == null) return;

            var attributes = (from attribute in xElem.Attributes() select attribute).ToArray();
            foreach (XAttribute attribute in attributes)
            {
                string key = attribute.Name.LocalName;
                if (!string.IsNullOrEmpty(key))
                    Attibutes.SafeAdd(attribute.Name.LocalName, attribute.Value);
            }
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

        #endregion
    }
}
