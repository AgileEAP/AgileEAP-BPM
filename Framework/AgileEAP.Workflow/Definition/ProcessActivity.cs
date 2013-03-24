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
    /// 工作流处理活动
    /// </summary>
    [Remark("处理")]
   
    public class ProcessActivity : Activity
    {
        #region Properties 成员

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessActivity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ProcessActivity(ConfigureElement parent, XElement xElem)
            : base(parent, xElem)
        {
        }

        #endregion

        #region Methods

        #endregion
    }
}
