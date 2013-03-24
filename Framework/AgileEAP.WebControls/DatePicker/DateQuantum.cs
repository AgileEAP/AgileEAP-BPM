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
    ToolboxData("<{0}:DateQuantum runat=server></{0}:DateQuantum>")]
    public class DateQuantum : CompositeControl
    {
        private DatePicker dpstart = new DatePicker();
        private DatePicker dpend = new DatePicker();

        /// <summary>
        /// DateQuantum的Text值
        /// </summary>
        [Browsable(true)]
        [Category("行为")]
        [Description("DateQuantum的初始文本值")]
        [DefaultValue("")]
        public string Value
        {
            get
            {
                string value = string.Empty;
                int index = 0;
                index = (string.IsNullOrEmpty(this.dpstart.Text.Trim()) ? 0 : 1) + (string.IsNullOrEmpty(this.dpend.Text.Trim()) ? 0 : 2);
                switch (index)
                {
                    case 0:
                        value = string.Empty;
                        break;
                    case 1:
                        value = this.dpstart.Text.Trim();
                        break;
                    case 2:
                        value = Convert.ToDateTime(this.dpend.Text.Trim()).AddDays(1).ToShortDateString();
                        break;
                    case 3:
                        value = this.dpstart.Text + " And " + Convert.ToDateTime(this.dpend.Text.Trim()).AddDays(1).ToShortDateString();
                        break;
                }
                return value;
            }
            set
            {
                this.dpstart.Text = value.Substring(0, value.IndexOf("And")).Trim();
                this.dpend.Text = value.Substring(value.IndexOf("And")).Trim();
            }
        }

        /// <summary>
        /// DateQuantum的Text值
        /// </summary>
        [Browsable(true)]
        [Category("行为")]
        [Description("DateQuantum的初始文本值")]
        [DefaultValue("")]
        public string Signs
        {
            get
            {
                string value = string.Empty;
                int index = 0;
                index = (string.IsNullOrEmpty(this.dpstart.Text.Trim()) ? 0 : 1) + (string.IsNullOrEmpty(this.dpend.Text.Trim()) ? 0 : 2);
                switch (index)
                {
                    case 0:
                        value = string.Empty;
                        break;
                    case 1:
                        value = ">=";
                        break;
                    case 2:
                        value = "<=";
                        break;
                    case 3:
                        value = "Between";
                        break;
                }
                return value;
            }
        }


        /// <summary>
        /// 排列方式
        /// </summary>
        [Browsable(true)]
        [Category("排列方式")]
        [Description("自定义排列方式")]
        [DefaultValue(RepeatDirection.Horizontal)]
        public RepeatDirection RepeatDirection
        {
            get
            {
                if (ViewState["RepeatDirection"] == null)
                {
                    return RepeatDirection.Horizontal;
                }
                return (RepeatDirection)ViewState["RepeatDirection"];
            }
            set
            {
                ViewState["RepeatDirection"] = value;
            }
        }

        /// <summary>
        /// 标签名字前缀
        /// </summary>
        [Browsable(true)]
        [Category("标签名字前缀")]
        [Description("自定义标签名字前缀")]
        [DefaultValue("")]
        public string LableText
        {
            get
            {
                return (ViewState["LableText"] as string) ?? string.Empty;
            }
            set
            {
                ViewState["LableText"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.Controls.Add(dpstart);
            this.Controls.Add(dpend);
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
            AddAttributesToRender(writer);

            if (RepeatDirection == RepeatDirection.Vertical)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rowlable");
                writer.RenderBeginTag(HtmlTextWriterTag.Dt);
                writer.Write(LableText + "开始时间");
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rowinput");
                writer.RenderBeginTag(HtmlTextWriterTag.Dd);
                dpstart.Attributes.Add("onchange", string.Format("checkupvalue(document.getElementById('{0}'),document.getElementById('{1}'),'{2}')", dpstart.ClientID, dpend.ClientID, "start"));
                dpstart.RenderControl(writer);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rowlable");
                writer.RenderBeginTag(HtmlTextWriterTag.Dt);
                writer.Write(LableText + "结束时间");
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rowinput");
                writer.RenderBeginTag(HtmlTextWriterTag.Dd);
                dpend.Attributes.Add("onchange", string.Format("checkupvalue(document.getElementById('{0}'),document.getElementById('{1}'),'{2}')", dpstart.ClientID, dpend.ClientID, "end"));
                dpend.RenderControl(writer);
                writer.RenderEndTag();
            }
            else
            {
                writer.Write(LableText + "从");
                dpstart.Attributes.Add("onchange", string.Format("checkupvalue(document.getElementById('{0}'),document.getElementById('{1}'),'{2}')", dpstart.ClientID, dpend.ClientID, "start"));
                dpstart.RenderControl(writer);
                writer.Write("到");
                dpend.Attributes.Add("onchange", string.Format("checkupvalue(document.getElementById('{0}'),document.getElementById('{1}'),'{2}')", dpstart.ClientID, dpend.ClientID, "end"));
                dpend.RenderControl(writer);
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

            string url = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.DatePicker.Resources.DateQuantum.js");
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "DatePicker.Resources.DateQuantum.js"))
            {
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "DatePicker.Resources.DateQuantum.js", url);
            }

        }

    }
}
