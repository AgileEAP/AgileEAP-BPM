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
    /// 活动参与者
    /// </summary>
   
    public class Participant : ConfigureElement
    {
        #region Properties
        /// <summary>
        /// 参与者方式:rblOrgization组织机构、角色或用户,rblWFStarter流程启动者
        /// </summary>
        
        public ParticipantType ParticipantType
        {
            get
            {
                return Properties.GetSafeValue<ParticipantType>("participantType", ParticipantType.Participantor);
            }
            set
            {
                Properties.SafeAdd("participantType", value);
            }
        }

        /// <summary>
        /// 是否允许前驱活动根据如上参与者列表指派该活动的参与者
        /// </summary>
        
        public bool AllowAppointParticipants
        {
            get
            {
                return Properties.GetSafeValue<bool>("allowAppointParticipants");
            }
            set
            {
                Properties.SafeAdd("allowAppointParticipants", value);
            }
        }

        /// <summary>
        /// 参与者
        /// </summary>
        
        public List<Participantor> Participantors
        {
            get;
            set;
        }

        
        public string ParticipantValue
        {
            get
            {
                return Properties.GetSafeValue<string>("participantValue");
            }
            set
            {
                Properties.SafeAdd("participantValue", value);
            }
        }

        /// <summary>
        /// 从活动中获取
        /// </summary>
        
        public string SpecialActivityID
        {
            get
            {
                return Properties.GetSafeValue<string>("specialActivityID");
            }
            set
            {
                Properties.SafeAdd("specialActivityID", value);
            }
        }

        /// <summary>
        /// 从相关数据获取
        /// </summary>
        
        public string SpecialPath
        {
            get
            {
                return Properties.GetSafeValue<string>("specialPath");
            }
            set
            {
                Properties.SafeAdd("specialPath", value);
            }
        }

        #endregion

        #region Construtor
        public Participant()
        {
            ElementName = "participant";
            Initilize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Participant(ConfigureElement parent, XElement xElem)
            : base(parent, "participant", xElem)
        {
            if (xElem == null)
            {
                Initilize();
                return;
            }

            Participantors = xElem.Element("participantors").Elements().Select(e => new Participantor(this, e)).ToList();
        }

        #endregion

        #region Methods

        public override void Initilize()
        {
            Participantors = new List<Participantor>();
        }
        /// <summary>
        ///把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            return new XElement(ElementName,
                               Properties.Select(p => new XElement(p.Key, p.Value)),
                               new XElement("participantors",
                                   Participantors != null ? Participantors.Select(p => p.ToXElement()) : null)
                              );
        }
        #endregion
    }
}
