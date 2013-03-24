using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core.Extensions;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 折点
    /// </summary>
  
    public class BendPoint : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// X轴位置
        /// </summary>
        
        public double X
        {
            get
            {
                return Properties.GetSafeValue<double>("x");
            }
            set
            {
                Properties.SafeAdd("x", value);
            }
        }

        /// <summary>
        /// Y轴位置
        /// </summary>
        
        public double Y
        {
            get
            {
                return Properties.GetSafeValue<double>("y");
            }
            set
            {
                Properties.SafeAdd("y", value);
            }
        }


        /// <summary>
        /// Z轴位置
        /// </summary>
        
        public double Z
        {
            get
            {
                return Properties.GetSafeValue<double>("z");
            }
            set
            {
                Properties.SafeAdd("z", value);
            }
        }

        /// <summary>
        /// 源点
        /// </summary>
        
        public Point3D Source
        {
            get
            {
                return Properties.GetSafeValue<Point3D>("source");
            }
            set
            {
                Properties.SafeAdd("source", value);
            }
        }

        /// <summary>
        /// 目标点
        /// </summary>
        
        public Point3D Sink
        {
            get
            {
                return Properties.GetSafeValue<Point3D>("sink");
            }
            set
            {
                Properties.SafeAdd("sink", value);
            }
        }

        /// <summary>
        /// 是否已经有中折点
        /// </summary>
        
        public bool IsBend
        {
            get
            {
                return Properties.GetSafeValue<bool>("isBend");
            }
            set
            {
                Properties.SafeAdd("isBend", value);
            }
        }

        #endregion

        #region Construtor
        public BendPoint()
        {
            ElementName = "bendPoint";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public BendPoint(ConfigureElement parent, XElement xElem)
            : base(parent, "bendPoint", xElem)
        { }

        #endregion
    }
}
