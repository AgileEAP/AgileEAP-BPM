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
    [Remark("人工活动")]
    public class ManualActivity : Activity
    {
        #region Properties 成员

        /// <summary>
        /// 是否允许代理
        /// </summary>

        public bool AllowAgent
        {
            get
            {
                return Properties.GetSafeValue<bool>("allowAgent");
            }
            set
            {
                Properties.SafeAdd("allowAgent", value);
            }
        }

        /// <summary>
        /// 重启参与者
        /// </summary>

        public ResetParticipant ResetParticipant
        {
            get
            {
                return Properties.GetSafeValue<ResetParticipant>("resetParticipant", ResetParticipant.FirstParticipantor);
            }
            set
            {
                Properties.SafeAdd("resetParticipant", value);
            }
        }

        /// <summary>
        /// 激动规则
        /// </summary>

        public ActivateRule ActivateRule
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展属性，自定义URL
        /// </summary>

        public ActionURL CustomURL
        {
            get;
            set;
        }

        /// <summary>
        /// 重置URL
        /// </summary>

        public ActionURL ResetURL
        {
            get;
            set;
        }

        /// <summary>
        /// 参与者
        /// </summary>

        public Participant Participant
        {
            get;
            set;
        }

        /// <summary>
        /// 多工作项设置
        /// </summary>

        public MultiWorkItem MultiWorkItem
        {
            get;
            set;
        }

        /// <summary>
        /// 回滚操作
        /// </summary>

        public CustomAction RollBack
        {
            get;
            set;
        }

        /// <summary>
        /// 自由流规则
        /// </summary>

        public FreeFlowRule FreeFlowRule
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流表单
        /// </summary>

        public string eForm
        {
            get
            {
                string formID = Properties.GetSafeValue<string>("eForm");
                if (string.IsNullOrEmpty(formID))
                {
                    formID = IdGenerator.NewComb().ToSafeString();
                    Properties.SafeAdd("eForm", formID);
                }

                return formID;
            }
            set
            {
                Properties.SafeAdd("eForm", value);
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManualActivity()
        {
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ManualActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            ActivateRule = new ActivateRule(this, xElem.Element("activateRule"));
            XElement manualElem = xElem.Element("manualActivity");

            if (manualElem != null)
            {
                CustomURL = new ActionURL("customURL", this, manualElem.Element("customURL"));
                ResetURL = new ActionURL("resetURL", this, manualElem.Element("resetURL"));
                Participant = new Participant(this, manualElem.Element("participant"));

                TimeLimit = new TimeLimit(this, manualElem.Element("timeLimit"));
                MultiWorkItem = new MultiWorkItem(this, manualElem.Element("multiWorkItem"));

                var triggerEvents = manualElem.Element("triggerEvents");
                if (triggerEvents != null)
                {
                    var triggerEventlist = triggerEvents.Elements("triggerEvent");
                    if (triggerEventlist != null)
                        TriggerEvents = triggerEventlist.Select(o => new TriggerEvent(this, o)).ToList();
                }
                RollBack = new CustomAction("rollBack", this, manualElem.Element("rollBack"));
                FreeFlowRule = new FreeFlowRule(this, manualElem.Element("freeFlowRule"));
            }
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            base.Initilize();

            ActivateRule = new ActivateRule();
            CustomURL = new ActionURL();
            ResetURL = new ActionURL();
            Participant = new Participant();

            TimeLimit = new TimeLimit();
            MultiWorkItem = new MultiWorkItem();
            TriggerEvents = new List<TriggerEvent>();
            RollBack = new CustomAction();
            FreeFlowRule = new FreeFlowRule();
        }

        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                               Attibutes.Select(o => new XAttribute(o.Key, o.Value)),
                               Properties.Select(p => new XElement(p.Key, p.Value)),
                               ActivateRule != null ? ActivateRule.ToXElement() : null,
                               new XElement("parameters", Parameters != null ? Parameters.Select(p => p.ToXElement()) : null),
                               Style.ToXElement(),
                               new XElement("manualActivity",
                                   CustomURL != null ? CustomURL.ToXElement("customURL") : null,
                                   ResetURL != null ? ResetURL.ToXElement("resetURL") : null,
                                   Participant != null ? Participant.ToXElement() : null,
                                   TimeLimit != null ? TimeLimit.ToXElement() : null,
                                   MultiWorkItem != null ? MultiWorkItem.ToXElement() : null,
                                   new XElement("triggerEvents", TriggerEvents != null ? TriggerEvents.Select(o => o.ToXElement()) : null),
                                   RollBack != null ? RollBack.ToXElement() : null,
                                   FreeFlowRule != null ? FreeFlowRule.ToXElement() : null));
        }
        #endregion
    }
}
