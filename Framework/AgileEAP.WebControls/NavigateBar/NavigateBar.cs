#region Description
/*================================================================================
 *  Copyright (c) Hiway.  All rights reserved.
 * ===============================================================================
 * Solution: Trenhui.WebControls
 * Module:  NavigateBar
 * Descrption:Create a NavigateBar for page commands
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
    /// 
    /// </summary>
    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("CommandExecute"),
    ToolboxData("<{0}:NavigateBar runat=server></{0}:NavigateBar>")]
    public class NavigateBar : WebControl
    {
        private List<NavigateItem> items = new List<NavigateItem>();
        private static readonly object commandExecute = new object();

        /// <summary>
        /// 
        /// </summary>
        public string ActiveId
        {
            get;
            set;
        }

        private string action = "updateNavBar";
        /// <summary>
        /// 
        /// </summary>
        public string Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public short Runat
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<NavigateItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, CommandExecuteEventArgs e);
        /// <summary>
        /// 
        /// </summary>
        public event CommandExecuteEventHandler CommandExecute
        {
            add
            {
                base.Events.AddHandler(commandExecute, value);
            }
            remove
            {
                base.Events.RemoveHandler(commandExecute, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            InitWebResource();
        }

        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "NavigateBar.Resources.NavigateBar.js"))
            {
                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.NavigateBar.Resources.NavigateBar.js");
                clientManager.RegisterClientScriptInclude(type, "NavigateBar.Resources.NavigateBar.js", url);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (items == null)
            {
                writer.Write("Items is Empty!");
                return;
            }

            if (Page != null)
            {
                string cssUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.NavigateBar.Resources.NavigateBar.css");
                writer.Write(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssUrl));
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "menubar_container");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "mainmenuBar");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "navigateBar");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (var item in items)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, item.Id);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, ActiveId.Equals(item.Id) ? "active_navigate_item" : "navigate_item");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("{0}('{1}')", action, item.Id));
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
