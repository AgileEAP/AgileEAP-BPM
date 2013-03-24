#region Description
/*================================================================================
 *  Copyright (c) trh.  All rights reserved.
 * ===============================================================================
 * Solution: AgileEAP.WebControls
 * Module:  CommandBar
 * Descrption:Create a Combox
 * CreateDate: 2009-7-15
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:
 * Remark:
 * Update Time:
 * Author:
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
    ///组合框 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    ToolboxData("<{0}:Combox runat=server></{0}:Combox>")]
    public class Combox : WebControl
    {
        private string comboxIcon = string.Empty;
        /// <summary>
        /// 组合框图标
        /// </summary>
        public string ComboxIcon
        {
            get
            {
                return comboxIcon;
            }
            set
            {
                comboxIcon = value;
            }
        }

        /// <summary>
        /// Change事件响应方法
        /// </summary>
        public string OnChanged
        {
            get;
            set;
        }

        private List<ComboxItem> items = new List<ComboxItem>();
        /// <summary>
        /// 组合项
        /// </summary>
        public List<ComboxItem> Items
        {
            get
            { return items; }
            set
            { items = value; }
        }

        private string selectedValue = string.Empty;
        /// <summary>
        /// 选中项
        /// </summary>
        public string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = Page.Request.Form[this.ID + "Data"];
                }
                if (string.IsNullOrEmpty(selectedValue) && Items != null && Items.Count > 0)
                {
                    selectedValue = Items[0].Value;
                    selectedText = items[0].Text;
                }

                return selectedValue;
            }
            set
            {
                foreach (ComboxItem item in Items)
                {
                    if (item.Value == value)
                    {
                        selectedText = item.Text;
                        selectedValue = value;
                    }
                }
            }
        }

        private string selectedText = string.Empty;
        /// <summary>
        /// 选中项文本
        /// </summary>
        public string SelectedText
        {
            get
            {
                if (string.IsNullOrEmpty(selectedText))
                {
                    selectedText = Page.Request.Form[this.ClientID];
                }
                return selectedText;
            }
            set
            {
                selectedText = value;
            }
        }

        private bool isSingle = true;
        /// <summary>
        /// 是否单选
        /// </summary>
        public bool IsSingle
        {
            get
            {
                return isSingle;
            }
            set
            {
                isSingle = value;
            }
        }

        /// <summary>
        /// 注册控件资源
        /// </summary>
        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "Combox.Resources.Combox.js"))
            {
                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.Combox.Resources.Combox.js");
                clientManager.RegisterClientScriptInclude(type, "Combox.Resources.Combox.js", url);
            }
        }

        /// <summary>
        /// 预呈现事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //初始化Web资源
            InitWebResource();
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            comboxIcon = Page.ClientScript.GetWebResourceUrl(GetType(), "AgileEAP.WebControls.Combox.Resources.ComboxIcon.gif");

            if (isSingle)
            {
                RenderSingle(writer);
            }
            else
            {
                RenderMultiple(writer);
            }
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public void RenderSingle(HtmlTextWriter writer)
        {
            StringBuilder html = new StringBuilder();

            string backIcon = string.Format("background:#fff url({0}) no-repeat right;", comboxIcon);
            html.Append("<Div id=\"singleComboxContainer\">");
            if (Items != null && Items.Count > 0)
            {   //没有数据不创建激发事件
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" onclick=\"comboxExpand('combox{0}');\" class=\"textbox_combox\" style='{1}' value='{2}' />", this.ID, backIcon, SelectedText);
            }
            else
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" class=\"textbox_combox\" style='{1}' value='{2}' />", this.ID, backIcon, SelectedText);
            }
            html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" tag=\"combox\" id=\"{0}data\" value='{1}' />", this.ID, SelectedValue);

            string height = Items != null && Items.Count > 10 ? "height:150px;" : string.Empty;

            html.AppendFormat("<div id='combox{0}' class=\"cbList\" style=\"{1}\">", this.ID, height);
            html.Append("<div id=\"cbItems\">");
            html.AppendLine("<ul>");

            foreach (var item in Items)
                html.AppendFormat("<li><a href=\"#\" data='{0}' class=\"single_combox_item_a\" onclick=\"comboxChooseItem(this,'{2}');{4};return false;\" tag=\"{3}\" >{1}</a></li>", item.Value, item.Text, this.ID, item.Tag, string.IsNullOrEmpty(OnChanged) ? string.Empty : string.Format("{0}('{1}','{2}');", OnChanged, item.Value, item.Text));

            html.AppendLine("</ul>");
            html.AppendLine("</div>");

            html.AppendLine("</div>");
            html.AppendLine("</div>");

            writer.Write(html.ToString());
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public void RenderMultiple(HtmlTextWriter writer)
        {
            StringBuilder html = new StringBuilder();
            string backIcon = string.Format("background:#fff url({0}) no-repeat right;", comboxIcon);
            html.Append("<Div id=\"multipleComboxContainer\">");
            if (Items != null && items.Count > 0)
            {   //没有数据不创建激发事件
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" onclick=\"comboxExpand('combox{0}');\" class=\"textbox_combox\" style='{1}' value='{2}' />", this.ID, backIcon, SelectedText);
            }
            else
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" class=\"textbox_combox\" style='{1}' value='{2}' />", this.ID, backIcon, SelectedText);
            }
            html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" tag=\"combox\" id=\"{0}data\" value='{1}' />", this.ID, SelectedValue);
            //html.Append("</Div>");

            string height = Items != null && Items.Count > 10 ? "height:150px;" : string.Empty;

            html.AppendFormat("<div id='combox{0}' class=\"cbList\" style=\"{1}\">", this.ID, height);

            html.Append("<div style=\"height:20px;\">");
            html.AppendFormat("<a href=\"#\" onclick=\"comboxSelectAll('{0}');return false;\"/>全选</a>&nbsp;<a href=\"#\"  onclick=\"comboxDeSelect('{0}');return false;\"/>反选</a>&nbsp;<a href=\"#\" onclick=\"comboxGetSelected('{0}');return false;\"/>确定</a></div>", this.ID);

            html.Append("<div id=\"cbItems\">");
            html.Append("<ul>");

            foreach (var item in Items)
                html.AppendFormat("<li><a href=\"#\" onclick='comboxChooseItem(this);return false;'><input type=\"checkbox\" {2} value=\"{0}\" style=\"height:12px; border:0px solid red;\" checked />{1}</a></li>", item.Value, item.Text, item.Value.Equals(SelectedValue, StringComparison.OrdinalIgnoreCase) ? "selected" : string.Empty);

            html.Append("</ul>");
            html.Append("</div>");
            html.Append("</div>");

            writer.Write(html.ToString());
        }
    }
}
