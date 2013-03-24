using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Security.Permissions;


namespace AgileEAP.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandItem
    {
        /// <summary>
        /// 操作按钮名称
        /// </summary>
        public string CommandName
        {
            get;
            set;
        }
        /// <summary>
        /// 参数
        /// </summary>
        public string CommandArgument
        {
            get;
            set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string ImageUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 触发方式0：通过jvavscript弹出
        /// 1：转向
        /// 2：ajax实现
        /// </summary>
        public short Runat
        {
            get;
            set;
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Text
        {
            get;
            set;
        }
        /// <summary>
        /// 暂无用
        /// </summary>
        public string OnClientClick
        {
            get;
            set;
        }

        private bool doValid = false;
        /// <summary>
        /// 是否要验证true:需要验证；false：不需要验证
        /// </summary>
        public bool DoValid
        {
            get
            {
                return doValid;
            }
            set
            {
                doValid = value;
            }
        }
    }
}
