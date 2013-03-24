#region Description
/*================================================================================
 *  Copyright (c) Hiway.  All rights reserved.
 * ===============================================================================
 * Solution: WebControls
 * Module:  DatePicker
 * Descrption:Create a DatePicker for page
 * CreateDate: 2009-10-12
 * Author: chenxiaoxi
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
    DefaultProperty("Text"),
    ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
    public class DatePicker : TextBox
    {
        /// <summary>
        /// 皮肤类型
        /// </summary>
        public enum SkinType
        {
            /// <summary>
            /// 
            /// </summary>
            Wdate,

            /// <summary>
            /// 
            /// </summary>
            Odate,
            /// <summary>
            /// 
            /// </summary>
            Text
        }

        /// <summary>
        /// 参数
        /// </summary>
        [Browsable(true)]
        [Category("参数")]
        [Description("样式：Wdate Odate Text")]
        [DefaultValue(SkinType.Wdate)]
        public SkinType Skin
        {
            get
            {
                if (ViewState["Skin"] == null)
                {
                    return SkinType.Wdate;
                }
                return (SkinType)ViewState["Skin"];
            }
            set
            {
                ViewState["Skin"] = value;
            }
        }

        ///// <summary>
        ///// ddd
        ///// </summary>
        ///// <param name="e"></param>
        ////protected override void OnLoad(EventArgs e)
        ////{
        ////    base.OnLoad(e);
        ////    string appPath = HttpContext.Current.Request.ApplicationPath;
        ////    appPath = (appPath.Length > 1 && !appPath.EndsWith("/")) ? appPath + "/" : "/";

        ////    string sitePath = string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, appPath);
        ////    string jsUrl = string.Format("{0}Scripts/DatePicker/WdatePicker.js", sitePath);
        ////    if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "WdatePicker.js"))
        ////    {
        ////        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "WdatePicker.js", jsUrl);
        ////    }

        ////}


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

            switch (Skin)
            {
                case SkinType.Wdate:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Wdate");
                    this.Attributes.Add("onclick", "var skin=getSkin();WdatePicker({skin:skin})");
                    base.Render(writer);
                    break;
                case SkinType.Odate:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "text");
                    base.Render(writer);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "odate_img");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "var skin=getSkin();WdatePicker({skin:skin,el:'" + this.ClientID + "'" + "})");
                    writer.RenderBeginTag(HtmlTextWriterTag.Em);
                    writer.RenderEndTag();
                    break;
                case SkinType.Text:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, ReadOnly ? "textreadonly" : "text");
                    if (!ReadOnly)
                    {
                        this.Attributes.Add("onclick", "var skin=getSkin();WdatePicker({skin:skin})");
                    }
                    base.Render(writer);
                    break;
                default:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Wdate");
                    this.Attributes.Add("onclick", "var skin=getSkin();WdatePicker({skin:skin})");
                    base.Render(writer);
                    break;
            }
        }
    }
}
