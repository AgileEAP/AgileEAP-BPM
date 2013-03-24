using System.Collections.Generic;

namespace AgileEAP.Core
{
    /// <summary>
    /// 表单字段
    /// </summary>

    public class FormField 
    {
        #region Properties 成员

        /// <summary>
        /// 序号
        /// </summary>

        public int SortOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 字段名
        /// </summary>

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string ExtendData
        {
            get;
            set;
        }

        /// <summary>
        /// 显示名
        /// </summary>

        public string Text
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
        /// 是否必需的
        /// </summary>

        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// 绑定控件
        /// </summary>

        public ControlType ControlType
        {
            get;
            set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>

        public DataType DataType
        {
            get;
            set;
        }

        /// <summary>
        /// 访问类型
        /// </summary>

        public AccessPattern AccessPattern
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public int Rows
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public int Cols
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>

        public string URL
        {
            get;
            set;
        }

        public string X
        {
            get;
            set;
        }
        public string Y
        {
            get;
            set;
        }
        public string Z
        {
            get;
            set;
        }
        public string Width
        {
            get;
            set;
        }
        public string Height
        {
            get;
            set;
        }

        public string CustomStyle
        {
            get;
            set;
        }

        /// <summary>
        /// 表单列
        /// </summary>

        public List<ListItem> ListItems
        {
            get;
            set;
        }
        public string Container
        {
            get;
            set;
        }
        #endregion
    }
}