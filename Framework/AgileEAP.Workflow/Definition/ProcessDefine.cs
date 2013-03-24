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
    /// 工作流定义类
    /// </summary>

    public class ProcessDefine : ConfigureElement
    {
        #region Basic

        /// <summary>
        /// 版本
        /// </summary>

        public string Version
        {
            get
            {
                return Attibutes.GetSafeValue<string>("version");
            }
            set
            {
                Attibutes.SafeAdd("version", value);
            }
        }

        /// <summary>
        /// 作者
        /// </summary>

        public string Author
        {
            get
            {
                return Attibutes.GetSafeValue<string>("author");
            }
            set
            {
                Attibutes.SafeAdd("author", value);
            }
        }

        /// <summary>
        /// 流程ID
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
        /// 工作流名称
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
        /// 是否分离事物
        /// </summary>

        public string IsSplitTransaction
        {
            get
            {
                return Properties.GetSafeValue<string>("isSplitTransaction");
            }
            set
            {
                Properties.SafeAdd("isSplitTransaction", value);
            }
        }

        /// <summary>
        /// 时间限制
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
        /// 流程启动类型
        /// </summary>

        public string StarterType
        {
            get;
            set;
        }

        /// <summary>
        /// 流程启动者
        /// </summary>

        public List<Participantor> Initiators
        {
            get;
            set;
        }

        public string StartURL
        {
            get
            {
                return Properties.GetSafeValue<string>("startURL", "/workflow/eform");
            }
            set
            {
                Properties.SafeAdd("startURL", value);
            }
        }
        #endregion

        #region Implementation 成员

        /// <summary>
        /// 工作流连接列表
        /// </summary>
        public List<Transition> Transitions
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动列表
        /// </summary>
        public List<Activity> Activities
        {
            get;
            set;
        }

        /// <summary>
        /// 开始活动
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public StartActivity StartActivity
        {
            get
            {
                if (Activities == null) return null;

                return Activities.FirstOrDefault(o => o.ActivityType == ActivityType.StartActivity) as StartActivity;
            }
        }

        /// <summary>
        /// 结束活动
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public EndActivity EndActivity
        {
            get
            {
                if (Activities == null) return null;

                return Activities.FirstOrDefault(o => o.ActivityType == ActivityType.EndActivity) as EndActivity;
            }
        }

        /// <summary>
        /// 业务变量
        /// </summary>
        public List<BizVariable> BizVariables
        {
            get;
            set;
        }

        /// <summary>
        /// 注释
        /// </summary>
        public List<Note> Notes
        {
            get;
            set;
        }

        #endregion

        #region Construtor
        public ProcessDefine()
        {
            ElementName = "workflowDefine";
            Properties = new Dictionary<string, object>();
            Activities = new List<Activity>();
            Attibutes = new Dictionary<string, object>();
            Initiators = new List<Participantor>();
            TriggerEvents = new List<TriggerEvent>();
            Transitions = new List<Transition>();
            Notes = new List<Note>();
            TimeLimit = new TimeLimit();
            BizVariables = new List<BizVariable>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        public ProcessDefine(string xml)
            : this(xml, false)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// /// <param name="xmlFile">xml文件路径</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ProcessDefine(string xml, bool isXmlFile)
            : base(null, "workflowDefine")
        {
            try
            {
                Current = isXmlFile ? XElement.Load(xml) : XElement.Parse(xml);
            }
            catch (Exception ex)
            {
                log.Error("初始化工作流定义出错", ex);
            }

            Initilize(Current);
        }

        #endregion

        #region Methods
        /// <summary>
        /// 初始化对象            
        /// </summary>
        /// <param name="xElem"></param>
        public override void Initilize(XElement xElem)
        {
            Attibutes = new Dictionary<string, object>();
            Properties = new Dictionary<string, object>();

            XElement basic = xElem.Element("basic");
            xElem.Attributes()
                  .ForEach(e => { if (!string.IsNullOrEmpty(e.Name.LocalName) && !string.IsNullOrEmpty(e.Value))Attibutes.SafeAdd(e.Name.LocalName, e.Value.Trim()); });

            basic.Elements()
                 .Where(e => e.Elements().Count() == 0)
                 .ForEach(e => { if (!string.IsNullOrEmpty(e.Name.LocalName) && !string.IsNullOrEmpty(e.Value)) Properties.SafeAdd(e.Name.LocalName, e.Value.Trim()); });

            TimeLimit = new TimeLimit(this, basic.Element("timeLimit"));
            var xInitiator = basic.Element("initiator");
            StarterType = xInitiator.Element("starterType").Value;
            Initiators = xInitiator.Elements("participantor").Select(o => new Participantor(this, o)).ToList();
            TriggerEvents = basic.Element("triggerEvents").Elements("triggerEvent").Select(o => new TriggerEvent(this, o)).ToList();
            Activities = xElem.Element("activities").Elements("activity").Select(e => ObejectFactory.CreateActivity(this, e)).ToList();
            Transitions = xElem.Element("transitions").Elements("transition").Select(e => new Transition(this, e)).ToList();

            XElement xResource = xElem.Element("resource");
            BizVariables = xResource.Element("bizVariables").Elements("bizVariable").Select(e => new BizVariable(this, e)).ToList();
            Notes = xResource.Element("notes").Elements("note").Select(e => new Note(this, e)).ToList();
        }

        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName, Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                                new XElement("basic",
                                Properties.Select(p => new XElement(p.Key, p.Value)),
                                TimeLimit.ToXElement(),
                                 new XElement("triggerEvents", TriggerEvents != null ? TriggerEvents.Select(o => o.ToXElement()) : null),
                                new XElement("initiator",
                                    new XElement("starterType", StarterType),
                                    Initiators != null ? Initiators.Select(o => o.ToXElement()) : null)
                                ),
                                new XElement("transitions", Transitions != null ? Transitions.Select(o => o.ToXElement()) : null),
                                new XElement("activities", Activities != null ? Activities.Select(o => o.ToXElement()) : null),
                                new XElement("resource",
                                    new XElement("bizVariables", BizVariables != null ? BizVariables.Select(o => o.ToXElement()) : null),
                                    new XElement("notes", Notes != null ? Notes.Select(o => o.ToXElement()) : null)));
        }

        ///// <summary>
        ///// 添加或修改活动
        ///// </summary>
        ///// <param name="activity"></param>
        //public void SafeAddActivity(Activity activity)
        //{
        //    if (this.Activities == null)
        //    {
        //        this.Activities = new List<Activity>();
        //    }
        //    Activity old = Activities.FirstOrDefault(a => a.ID == activity.ID);
        //    if (old != null)
        //    {
        //        this.Activities.Remove(old);
        //    }
        //    //if (activity.Parent != this)
        //    //{
        //    //    activity.Parent = this;
        //    //}

        //    this.Activities.Add(activity);
        //    this.Transitions.ForEach(a =>
        //        {
        //            if (a.SrcActivity.ID == activity.ID)
        //            {
        //                a.SrcActivity = activity;
        //            }
        //            if (a.DestActivity.ID == activity.ID)
        //            {
        //                a.DestActivity = activity;
        //            }
        //        });
        //}

        /// <summary>
        /// 添加或修改活动
        /// </summary>
        /// <param name="oldActivityID"></param>
        /// <param name="activity"></param>
        public void SafeAddActivity(string oldActivityID, Activity activity)
        {
            if (this.Activities == null)
            {
                this.Activities = new List<Activity>();
            }

            ProcessDefine parent = activity.GetPropertyValue<ProcessDefine>("Parent");
            if (parent == null)
            {
                activity.SetPropertyValue("Parent", this);
            }

            this.Transitions.ForEach(a =>
            {
                if (a.SrcActivity == oldActivityID)
                {
                    a.SrcActivity = activity.ID;
                }
                if (a.DestActivity == oldActivityID)
                {
                    a.DestActivity = activity.ID;
                }
            });

            int index = -1;
            for (int i = 0; i < Activities.Count; i++)
            {
                if (Activities[i].ID == oldActivityID)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                activity.Style = Activities[index].Style;
                Activities.RemoveAt(index);
                Activities.Insert(index, activity);
            }
            else
            {
                Activities.Add(activity);
            }
        }
        #endregion
    }
}
