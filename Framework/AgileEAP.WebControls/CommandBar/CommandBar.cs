#region Description
/*================================================================================
 *  Copyright (c) trh.  All rights reserved.
 * ===============================================================================
 * Solution: AgileEAP.WebControls
 * Module:  CommandBar
 * Descrption:Create a CommandBar for page commands
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
    ///命令按钮栏 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("CommandExecute"),
    ToolboxData("<{0}:CommandBar runat=server></{0}:CommandBar>")]
    public class CommandBar : WebControl, IPostBackEventHandler
    {
        private List<CommandItem> items = new List<CommandItem>();
        private static readonly object commandExecute = new object();

        /// <summary>
        /// 命令项列表
        /// </summary>
        public List<CommandItem> Items
        {
            get
            { return items; }
            set
            { items = value; }
        }

        /// <summary>
        /// 命令执行委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommandExecuteEventHandler(object sender, CommandExecuteEventArgs e);

        /// <summary>
        /// 命令事件
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
            //呈现开始标记
            RenderBeginTag(writer);

            //呈现内容
            RenderContents(writer);

            //呈现结束标记
            RenderEndTag(writer);
        }

        /// <summary>
        /// 注册控件资源
        /// </summary>
        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "CommandBar.Resources.CommandBar.js"))
            {
                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.CommandBar.Resources.CommandBar.js");
                clientManager.RegisterClientScriptInclude(type, "CommandBar.Resources.CommandBar.js", url);
            }
        }

        /// <summary>
        /// 呈现开始标记
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "commandbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        }

        /// <summary>
        /// 呈现内容
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "commanditem_first");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.RenderEndTag();

            foreach (CommandItem item in items)
            {
                string argument = string.Concat(item.CommandName, "$", item.CommandArgument);
                string deleteComfirm = "";
                if (item.CommandName == "Delete")
                {
                    deleteComfirm = "if(!confirm('是否确定删除记录？')){ return false;}";
                }

                string script = string.Empty;
                if (item.Runat == 1)
                {
                    script = string.Format("if(formValidatorIsValid({0})){1}{2};", item.DoValid.ToString().ToLower(), "{" + deleteComfirm, string.Format("commandExecute(this,'{0}','{1}',true);", item.CommandName, item.CommandArgument) + "}");
                }
                else if (item.Runat == 2)
                {
                    script = string.Format("if(formValidatorIsValid({0})){1}{2};", item.DoValid.ToString().ToLower(), "{" + deleteComfirm, Page.ClientScript.GetPostBackEventReference(this, argument) + "}");
                }
                else if (item.Runat == 3)
                {
                    script = Page.ClientScript.GetPostBackEventReference(this, argument);
                    script = string.Format("if(formValidatorIsValid({0})){1}{2};", item.DoValid.ToString().ToLower(), "{" + deleteComfirm, string.Format("commandExecute(this,'{0}','{1}',false);", item.CommandName, item.CommandArgument) + "}");
                }

                string style = string.Concat("cursor: pointer;", !string.IsNullOrEmpty(item.ImageUrl) ? string.Format("background-image: url('{0}'); background-repeat: no-repeat;background-position: center center; ", item.ImageUrl) : string.Empty);

                writer.AddAttribute("style", style);
                writer.AddAttribute("onclick", script);
                writer.AddAttribute("onmouseover", "CommandItemOver(this)");
                writer.AddAttribute("onmouseout", "CommandItemOut(this)");
                writer.AddAttribute("class", "commanditem");
                writer.AddAttribute("id", Guid.NewGuid().ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Style, style);
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }

            writer.AddAttribute("class", "commanditem_last");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.RenderEndTag();
        }

        /// <summary>
        /// 呈现结束标记
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        #region IPostBackEventHandler 成员

        /// <summary>
        /// 触发回发事件
        /// </summary>
        /// <param name="argument"></param>
        public void RaisePostBackEvent(string argument)
        {
            string[] args = argument.Split('$');
            CommandExecuteEventArgs e = new CommandExecuteEventArgs();
            try
            {
                e.CommandName = args[0];
                e.CommandArgument = args[1];
            }
            catch { }

            CommandExecuteEventHandler handler = (CommandExecuteEventHandler)base.Events[commandExecute];
            if (handler != null)
                handler(this, e);

            //if (e.CommandName.Equals("Save", StringComparison.OrdinalIgnoreCase) && Page != null)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "AfterSave", "afterSave(true)", true);
            //}
        }

        #endregion
    }

    /// <summary>
    /// 命令执行参数
    /// </summary>
    public class CommandExecuteEventArgs
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName
        {
            get;
            set;
        }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string CommandArgument
        {
            set;
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandExecuteEventArgs()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="commandArgument">命令参数</param>
        public CommandExecuteEventArgs(string commandName, string commandArgument)
        {
            CommandName = commandName;
            CommandArgument = commandArgument;
        }

    }
}
