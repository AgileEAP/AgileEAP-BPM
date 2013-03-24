using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 参与者信息
    /// </summary>
   
    public class Participantor : ConfigureElement
    {
        #region Properties 成员

        /// <summary>
        /// 编号
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
        /// 参与者名称
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
        /// 参与者类别:person,role,org三类
        /// </summary>
        
        public ParticipantorType ParticipantorType
        {
            get
            {
                return Properties.GetSafeValue<ParticipantorType>("participantorType", ParticipantorType.Role);
            }
            set
            {
                Properties.SafeAdd("participantorType", value);
            }
        }

        /// <summary>
        /// 父结点ID
        /// </summary>
        
        public string ParentID
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        
        public int SortOrder
        {
            get
            {
                return Properties.GetSafeValue<int>("sortOrder");
            }
            set
            {
                Properties.SafeAdd("sortOrder", value);
            }
        }

        #endregion

        #region Construtor
        public Participantor()
        {
            ElementName = "participantor";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public Participantor(ConfigureElement parent, XElement xElem)
            : base(parent, "participantor", xElem)
        { }

        #endregion
    }
}
