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
    /// 工作日历
    /// </summary>
   
    public class CalendarSet : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 初始类型
        /// </summary>
        
        public string Init
        {
            get
            {
                return Properties.GetSafeValue<string>("init");
            }
            set
            {
                Properties.SafeAdd("init", value);
            }
        }

        /// <summary>
        /// 日历类型
        /// </summary>
        
        public CalendarType Type
        {
            get
            {
                return Properties.GetSafeValue<CalendarType>("type", CalendarType.CDefault);
            }
            set
            {
                Properties.SafeAdd("type", value);
            }
        }


        /// <summary>
        /// 日历标记
        /// </summary>
        
        public string ID
        {
            get
            {
                return Properties.GetSafeValue<string>("id");
            }
            set
            {
                Properties.SafeAdd("id", value);
            }
        }

        /// <summary>
        /// 日历名称
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
        #endregion

        #region Construtor
        public CalendarSet()
        {
            ElementName = "calendar";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public CalendarSet(ConfigureElement parent, XElement xElem)
            : base(parent, "calendar", xElem)
        { }

        #endregion
    }
}
