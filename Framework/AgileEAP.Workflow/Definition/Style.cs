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
    /// 活动样式
    /// </summary>
  
    public class Style : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 活动皮肤
        /// </summary>
        
        public string Skin
        {
            get
            {
                return Properties.GetSafeValue<string>("skin");
            }
            set
            {
                Properties.SafeAdd("skin", value);
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        
        public double Height
        {
            get
            {
                return Properties.GetSafeValue<double>("height");
            }
            set
            {
                Properties.SafeAdd("height", value);
            }
        }


        /// <summary>
        /// 宽度
        /// </summary>
        
        public double Width
        {
            get
            {
                return Properties.GetSafeValue<double>("width");
            }
            set
            {
                Properties.SafeAdd("width", value);
            }
        }

        /// <summary>
        /// 左边位置
        /// </summary>
        
        public double Left
        {
            get
            {
                return Properties.GetSafeValue<double>("left");
            }
            set
            {
                Properties.SafeAdd("left", value);
            }
        }

        /// <summary>
        /// 顶端位置
        /// </summary>
        
        public double Top
        {
            get
            {
                return Properties.GetSafeValue<double>("top");
            }
            set
            {
                Properties.SafeAdd("top", value);
            }
        }

        /// <summary>
        /// Z轴
        /// </summary>
        
        public int ZIndex
        {
            get
            {
                return Properties.GetSafeValue<int>("zIndex");
            }
            set
            {
                Properties.SafeAdd("zIndex", value);
            }
        }

        /// <summary>
        /// 是否组合
        /// </summary>
        
        public bool IsGroup
        {
            get
            {
                return Properties.GetSafeValue<bool>("isGroup");
            }
            set
            {
                Properties.SafeAdd("isGroup", value);
            }
        }
        #endregion

        #region Construtor
        public Style()
        {
            ElementName = "style";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Style(ConfigureElement parent, XElement xElem)
            : base(parent, "style", xElem)
        { }

        #endregion
    }
}
