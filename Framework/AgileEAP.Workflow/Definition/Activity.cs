using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 工作流活动基类
    /// </summary>
    //[JsonObject(MemberSerialization.OptOut)]
    //[JsonConverter(typeof(ActivityConvert))]
    public class Activity : ConfigureElement
    {
        #region Properties 成员
        /// <summary>
        /// 活动ID
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
        /// 活动名称
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
        /// 活动类型
        /// </summary>
        
        public ActivityType ActivityType
        {
            get
            {
                return Properties.GetSafeValue<ActivityType>("activityType", ActivityType.ManualActivity);
            }
            set
            {
                Properties.SafeAdd("activityType", value);
            }
        }

        /// <summary>
        /// 分支模式
        /// </summary>
        
        public SplitType SplitType
        {
            get
            {
                return Properties.GetSafeValue<SplitType>("splitType", SplitType.XOR);
            }
            set
            {
                Properties.SafeAdd("splitType", value);
            }
        }
        /// <summary>
        /// 聚合模式
        /// </summary>
        
        public JoinType JoinType
        {
            get
            {
                return Properties.GetSafeValue<JoinType>("joinType", JoinType.AND);
            }
            set
            {
                Properties.SafeAdd("joinType", value);
            }
        }

        /// <summary>
        /// 是否分享事物
        /// </summary>
        
        public bool IsSplitTransaction
        {
            get
            {
                return Properties.GetSafeValue<bool>("isSplitTransaction");
            }
            set
            {
                Properties.SafeAdd("isSplitTransaction", value);
            }
        }

        /// <summary>
        /// 优先级别
        /// </summary>
        
        public PriorityType Priority
        {
            get
            {
                return Properties.GetSafeValue<PriorityType>("priority", PriorityType.Middle); ;
            }
            set
            {
                Properties.SafeAdd("priority", value);
            }

        }

        /// <summary>
        /// 说明
        /// </summary>
        
        public string Description
        {
            get
            {
                return Properties.GetSafeValue<string>("description"); ;
            }
            set
            {
                Properties.SafeAdd("description", value);
            }
        }

        /// <summary>
        /// 显示样式
        /// </summary>
        
        public Style Style
        {
            get;
            set;
        }

        /// <summary>
        /// 超时限制
        /// </summary>
        
        public TimeLimit TimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        
        public List<TriggerEvent> TriggerEvents
        {
            get;
            set;
        }

        /// <summary>
        /// 活动参数
        /// </summary>
        
        public List<Parameter> Parameters
        {
            get;
            set;
        }
        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public Activity()
        {
            ElementName = "activity";

            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Activity(ConfigureElement parent, XElement xElem)
            : base(parent, "activity", xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            var xParameters = xElem.Element("parameters");
            if (xParameters != null)
                Parameters = xParameters.Elements("parameter").Select(e => new Parameter(this, e)).ToList();
            Style = new Style(this, xElem.Element("style"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                                Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                Properties.Select(o => new XElement(o.Key, o.Value)),
                                new XElement("parameters", Parameters != null ? Parameters.Select(p => p.ToXElement()) : null),
                                Style.ToXElement()
                                );
        }

        public override int GetHashCode()
        {
            return string.Format("{0}.{1}", Name, ID).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != this.GetType()) return false;

            Activity tmp = (Activity)obj;

            return ID == tmp.ID && Name == tmp.Name;
        }

        public override void Initilize()
        {
            Style = new Style();
            Parameters = new List<Parameter>();
        }
        #endregion

        #region Events

        #endregion

        #region Execute Methods

        public virtual void Execute(object state)
        {
            throw new Exception("no impl");
        }

        public virtual void Execute(Action task)
        {
            if (task != null)
            {
                task();
            }
        }
        #endregion
    }

    public class ActivityConvert : JsonCreationConverter<Activity>
    {
        protected override Activity Create(Type objectType, JObject jObject)
        {
            JProperty jProperty = jObject.Property("ActivityType");
            if (jProperty == null)
                return new ManualActivity();

            ActivityType activityType = jProperty.Value.Cast<ActivityType>(ActivityType.ManualActivity);
            var activityName = activityType.ToString();
            if (!activityName.StartsWith("AgileEAP.Workflow.Definition"))
                activityName = string.Format("AgileEAP.Workflow.Definition.{0}", activityName);

            Activity activity = null;
            try
            {
                Type type = Type.GetType(activityName);
                activity = Activator.CreateInstance(type) as Activity;
                activity.ActivityType = activityType;
            }
            catch (Exception ex)
            {
                activity = new ManualActivity();
                GlobalLogger.Error<ActivityConvert>(ex);
            }

            return activity;
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //throw new NotImplementedException();
        }
    }
}
