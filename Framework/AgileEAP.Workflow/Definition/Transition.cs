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
    /// 活动迁移定义类
    /// </summary>
   
    public class Transition : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ID
        {
            get
            {
                return Properties.GetSafeValue<string>("id", Guid.NewGuid().ToString());
            }
            set
            {
                Properties.SafeAdd("id", value);
            }
        }



        /// <summary>
        ///连接显示名称
        /// </summary>
        public string Name
        {
            get
            {
                return Properties.GetSafeValue<string>("name", string.Empty);
            }
            set
            {
                Properties.SafeAdd("name", value);
            }
        }

        /// <summary>
        ///显示皮肤
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
        /// 是否是默认连接
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return Properties.GetSafeValue<bool>("isDefault");
            }
            set
            {
                Properties.SafeAdd("isDefault", value);
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
        /// 连接开始结点位置
        /// </summary>
        public string SourceOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// 源点位置
        /// </summary>
        public Point3D SourcePoint
        {
            get;
            set;
        }

        /// <summary>
        /// 连接结束结点位置
        /// </summary>
        public string SinkOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// 目标点位置
        /// </summary>
        public Point3D SinkPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 连接开始结点
        /// </summary>
        public string SrcActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 连接结束结点ID
        /// </summary>
        public string DestActivity
        {
            get;
            set;
        }

        /// <summary>
        /// 表达式值
        /// </summary>
        public string Expression
        {
            get
            {
                return Properties.GetSafeValue<string>("expression");
            }
            set
            {
                Properties.SafeAdd("expression", value);
            }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return Properties.GetSafeValue<PriorityType>("priority", PriorityType.Middle);
            }
            set
            {
                Properties.SafeAdd("priority", value);
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return Properties.GetSafeValue<string>("description");
            }
            set
            {
                Properties.SafeAdd("description", value);
            }
        }

        /// <summary>
        /// 连接折点
        /// </summary>
        public List<BendPoint> BendPoints
        {
            get;
            set;
        }
        #endregion

        #region Construtor
        public Transition()
        {
            ElementName = "transition";
            Properties = new Dictionary<string, object>();
            BendPoints = new List<BendPoint>();
            SourcePoint = new Point3D();
            SinkPoint = new Point3D();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Transition(ConfigureElement parent, XElement xElem)
            : base(parent, "transition", xElem)
        {
            XElement sourceElem = xElem.Element("source");
            XElement sinkElem = xElem.Element("sink");

            SourceOrientation = sourceElem.ChildElementValue("orientation");
            SinkOrientation = sinkElem.ChildElementValue("orientation");
            SourcePoint = new Point3D() { X = sourceElem.ChildElementValue("x").ToDouble(), Y = sourceElem.ChildElementValue("y").ToDouble() };
            SinkPoint = new Point3D() { X = sinkElem.ChildElementValue("x").ToDouble(), Y = sinkElem.ChildElementValue("y").ToDouble() };
            BendPoints = xElem.Element("bendPoints").Elements("bendPoint").Select(e => new BendPoint(this, e)).ToList();

            ProcessDefine wd = parent as ProcessDefine;
            SrcActivity = sourceElem.ChildElementValue("activityID");
            DestActivity = sinkElem.ChildElementValue("activityID");
        }

        #endregion

        #region Methods
        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Properties.Select(o => new XElement(o.Key, o.Value)),
                                new XElement("source", new XElement("orientation", SourceOrientation),
                                                       new XElement("x", SourcePoint.X),
                                                       new XElement("y", SourcePoint.Y),
                                                       new XElement("activityID", SrcActivity)),
                                new XElement("sink", new XElement("orientation", SinkOrientation),
                                                       new XElement("x", SinkPoint.X),
                                                       new XElement("y", SinkPoint.Y),
                                                       new XElement("activityID",  DestActivity)),
                                new XElement("bendPoints", BendPoints != null ? BendPoints.Select(b => b.ToXElement()) : null)
                                );
        }

        #endregion

        public override int GetHashCode()
        {
            return string.Format("{0}.{1}", ID, Name).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != this.GetType()) return false;

            Transition tmp = (Transition)obj;

            return ID == tmp.ID;
        }
    }

    public struct Point3D
    {
        public double X
        { get; set; }

        public double Y
        { get; set; }

        public int Z
        { get; set; }
    }
}
