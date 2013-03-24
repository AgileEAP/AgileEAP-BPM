using System.Collections.Generic;

namespace AgileEAP.Core
{
    /// <summary>
    /// 工作流表单
    /// </summary>

    public class Form 
    {
        #region Properties 成员
        /// <summary>
        /// 表单名称
        /// </summary>

        public string Name
        {
            get;
            set;
        }

        public string Script
        {
            get;
            set;
        }

        public string AutoGenScript
        {
            get;
            set;
        }

        public string Style
        {
            get;
            set;
        }

        /// <summary>
        /// 数据来源
        /// </summary>

        public string DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// 表单标题
        /// </summary>

        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 表单行
        /// </summary>

        public List<FormField> Fields
        {
            get;
            set;
        }
        #endregion
    }
}
