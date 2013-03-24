#region Description
/*================================================================================
 *  Copyright (c) Hiway.  All rights reserved.
 * ===============================================================================
 * Solution: Trenhui.WebControls
 * Module:  CommandBar
 * Descrption:Create a ChooseBox for page
 * CreateDate: 2009-7-15
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:增加弹出窗口 增加Value值返回 处理控件样式
 * Remark:
 * Update Time:2009-10-09
 * Author: chenxiaoxi
 * 
 * ===============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.Security.Permissions;

namespace AgileEAP.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [
    AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Text"),
    ToolboxData("<{0}:ChooseBox runat=server></{0}:ChooseBox>")]
    public class ChooseBox : TextBox
    {
        private string dataValue = string.Empty;
        /// <summary>
        /// 控件值
        /// </summary>
        [Browsable(true)]
        [Category("控件值")]
        [Description("自定义控件值")]
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(dataValue))
                    dataValue = Page.Request.Form[this.ID + "data"];
                return dataValue;
            }
            set
            {
                dataValue = value;
            }
        }

        private string tagValue = string.Empty;
        /// <summary>
        /// 控件扩展值
        /// </summary>
        [Browsable(true)]
        [Category("控件扩展值")]
        [Description("自定义控件扩展值")]
        public string Tag
        {
            get
            {
                if (string.IsNullOrEmpty(tagValue))
                    tagValue = Page.Request.Form[this.ID + "tag"];
                return tagValue;
            }
            set
            {
                tagValue = value;
            }
        }


        /// <summary>
        /// 打开页面路径
        /// </summary>
        [Browsable(true)]
        [Category("打开页面路径")]
        [Description("自定义打开页面路径")]
        [DefaultValue("")]
        public string OpenUrl
        {
            get
            {
                return (ViewState["OpenUrl"] as string) ?? string.Empty;
            }
            set
            {
                ViewState["OpenUrl"] = value;
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        [Browsable(true)]
        [Category("参数")]
        [Description("自定义参数")]
        public string QueryString
        {
            get
            {
                return (ViewState["QueryString"] as string) ?? string.Empty;
            }
            set
            {
                ViewState["QueryString"] = value;
            }
        }

        /// <summary>
        /// 是否包含单击事件，如果为False 则不加载相应方法事件 外部可自定义
        /// </summary>
        [Browsable(true)]
        [Category("是否包含单击事件")]
        [Description("自定义是否包含单击事件")]
        public bool InbuiltClick
        {
            get
            {
                object o = ViewState["InbuiltClick"];
                return (o == null) ? true : (bool)o;
            }
            set
            {
                ViewState["InbuiltClick"] = value;
            }

        }

        /// <summary>
        /// 弹出窗体高度
        /// </summary>
        [Browsable(true)]
        [Category("弹出窗体高度")]
        [Description("自定义弹出窗体高度")]
        public string DialogHeight
        {
            get
            {
                return (ViewState["DialogHeight"] as string) ?? "350";
            }
            set
            {
                ViewState["DialogHeight"] = value;
            }
        }

        /// <summary>
        /// 弹出窗体宽度
        /// </summary>
        [Browsable(true)]
        [Category("弹出窗体宽度")]
        [Description("自定义弹出窗体宽度")]
        public string DialogWidth
        {
            get
            {
                return (ViewState["DialogWidth"] as string) ?? "600";
            }
            set
            {
                ViewState["DialogWidth"] = value;
            }
        }

        /// <summary>
        /// 弹出窗体标头
        /// </summary>
        [Browsable(true)]
        [Category("弹出窗体标头")]
        [Description("自定义弹出窗体标头")]
        public string DialogTitle
        {
            get
            {
                return (ViewState["DialogTitle"] as string) ?? "选择对话框";
            }
            set
            {
                ViewState["DialogTitle"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            if (InbuiltClick)
            {
                string url = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.ChooseBox.Resources.ChooseBox.js");
                if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "ChooseBox.Resources.ChooseBox.js"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "ChooseBox.Resources.ChooseBox.js", url);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                base.Render(writer);
                return;
            }

            string pic = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.ChooseBox.Resources.chooseBox.png");
            string backgrounp = "background:#fff url(" + pic + ") no-repeat right;";
            string style = "cursor:pointer;" + backgrounp + "background-color:#fff";
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style);
            if (InbuiltClick)
            {
                this.Attributes.Add("onclick", "openChooseBoxDialog('" + this.ClientID + "','" + this.ID + "','" + DialogTitle + "', '" + OpenUrl + "', '" + DialogWidth + "','" + DialogHeight + "')");
                Attributes.Add("tag", "choosebox");
                this.ReadOnly = true;
            }

            base.Render(writer);
            writer.Write(string.Format("<input name=\"{0}data\" type=\"hidden\" tag=\"choosebox\" id=\"{0}data\" value='{1}' />", this.ID, Value));
            writer.Write(string.Format("<input name=\"{0}tag\" type=\"hidden\" tag=\"choosebox\" id=\"{0}tag\" value='{1}' />", this.ID, Tag));
        }
    }
}
