using System;
using System.Collections.Generic;
using System.Text;

namespace AgileEAP.WebControls
{
    /// <summary>
    /// 组合框项
    /// </summary>
    public class ComboxItem
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// 标记
        /// </summary>
        public string Tag
        {
            get;
            set;
        }
    }
}
