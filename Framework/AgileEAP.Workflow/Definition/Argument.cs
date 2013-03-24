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
    /// 参数
    /// </summary>
   
    public class Argument : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 参数名称
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
        /// 参数类型
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
        /// 参数值
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
        public Argument()
        {
            ElementName = "argument";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Argument(ConfigureElement parent, XElement xElem)
            : base(parent, "argument", xElem)
        { }

        #endregion

        #region Methods

        #endregion
    }
}
