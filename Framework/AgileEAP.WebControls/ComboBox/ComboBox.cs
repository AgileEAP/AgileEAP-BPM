#region Description
/*================================================================================
 *  Copyright (c) Hiway.  All rights reserved.
 * ===============================================================================
 * Solution: WebControls
 * Module:   
 * Descrption:自定义ComboBox控件
 * CreateDate: 2009-10-13
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace AgileEAP.WebControls
{
    /// <summary>
    /// 可编辑下拉控件
    /// </summary>
    [ParseChildren(true, "Items")]
    [PersistChildren(false)]
    [DefaultProperty("ID")]
    [ToolboxData("<{0}:ComboBox runat=\"server\"></{0}:ComboBox>")]
    [ValidationPropertyAttribute("Value")]
    public class ComboBox : DropDownList, INamingContainer
    {
        private readonly ListItemCollection m_Items = new ListItemCollection();
        private readonly TextBox m_Textbox = new TextBox();
        private string m_Attribute;

        /// <summary>
        /// 控件类型
        /// </summary>
        public enum DropDownType
        {
            /// <summary>
            /// 下拉控件
            /// </summary>
            DropDownList,


            /// <summary>
            /// 可写下拉控件
            /// </summary>
            DropDown

        }

        /// <summary>
        /// 是否在得到焦点的时候显示选择项文本
        /// </summary>
        [DefaultValue(false)]
        public bool ShowSelectTip
        {
            get
            {
                object o = ViewState["ShowSelectTip"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                ViewState["ShowSelectTip"] = value;
            }
        }
        /// <summary>
        /// ComboBox的Text值
        /// </summary>
        [Browsable(true)]
        [Category("行为")]
        [Description("ComboBox的初始文本值")]
        [DefaultValue("")]
        public string Value
        {
            get
            {
                if (DropDownStyle == DropDownType.DropDown)
                {
                    if (string.IsNullOrEmpty(m_Textbox.Text))
                    {
                        if (SelectedItem != null)
                        {
                            return SelectedValue;
                        }
                    }
                    return m_Textbox.Text;
                }
                else
                {
                    return SelectedValue;
                }
            }
            set
            {
                if (DropDownStyle == DropDownType.DropDown)
                {
                    SelectedValue = value;
                    m_Textbox.Text = value;
                    if (SelectedValue != value)
                    {
                        this.Items.Add(value);
                        SelectedValue = value;
                    }
                }
                else
                {
                    SelectedValue = value;
                }
            }
        }

        /// <summary>
        /// ComboBox的Items内容
        /// </summary>
        [Browsable(true)]
        [Category("杂项")]
        [Description("ComboBox的内容项")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false)]
        public override ListItemCollection Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        [Browsable(true)]
        [Category("属性")]
        [Description("自定义属性")]
        [DefaultValue("")]
        public string Attribute
        {
            get { return m_Attribute; }
            set { m_Attribute = value; }
        }


        /// <summary>
        /// 参数
        /// </summary>
        [Browsable(true)]
        [Category("参数")]
        [Description("ComboBox样式：DropDown 可手动输入 DropDownList 不能手动输入 即为DropDownList控件")]
        [DefaultValue(DropDownType.DropDownList)]
        public DropDownType DropDownStyle
        {
            get
            {
                if (ViewState["DropDownStyle"] == null)
                {
                    return DropDownType.DropDownList;
                }
                return (DropDownType)ViewState["DropDownStyle"];
            }
            set
            {
                ViewState["DropDownStyle"] = value;
            }
        }


        /// <summary>
        /// 文本框的数据回传
        /// </summary>
        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            m_Textbox.Text = postCollection[postDataKey + "_Text"];
            return base.LoadPostData(postDataKey, postCollection);
        }

        /// <summary>
        /// 输出
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                base.Render(writer);
                return;
            }

            if (DropDownStyle == DropDownType.DropDown)
            {
                var dropWidth = Convert.ToInt32(base.Width.Value);
                if (dropWidth == 0)
                {
                    dropWidth = 160;
                    base.Width = 160;
                }
                var textWidth = dropWidth - 22;

                var dropHeight = 22;
                var textHeight = 18;

                if (!DesignMode)
                {
                    var client = HttpContext.Current.Request.Browser;
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.Write("<div style=\"position:absolute;\">");
                    if (client.Browser == "IE")
                    {
                        textHeight = 18;
                        writer.Write(
                            "<iframe frameborder=\"0\" scrolling=\"no\" style=\"z-index:-10;margin-top:2px;margin-left:1px;");
                        writer.Write("width:" + textWidth + "px;height:" + textHeight +
                                     "px;position:absolute;\"></iframe>");
                    }
                    else if (client.Browser == "Firefox")
                    {
                        dropHeight = 20;
                        m_Textbox.Style.Add("z-index", "10");
                    }

                    m_Textbox.Style.Clear();
                    m_Textbox.Width = textWidth;
                    m_Textbox.Height = textHeight - 1;
                    m_Textbox.Style.Add(HtmlTextWriterStyle.FontSize, "12px");
                    m_Textbox.Style.Add(HtmlTextWriterStyle.FontFamily, "宋体");
                    m_Textbox.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                    m_Textbox.Style.Add("margin-top", "1px");
                    m_Textbox.Style.Add("margin-left", "2px");
                    m_Textbox.BorderWidth = 0;
                    m_Textbox.ID = base.UniqueID + "_Text";
                    //if (!string.IsNullOrEmpty(m_Attribute))
                    //{
                    //    m_Textbox.Attributes.Add("onchange", m_Attribute);
                    //}
                    string changeDefaultSelectValue = string.Format(
                                                        "ontextchange(this,document.getElementById('{0}'))",
                                                        base.ClientID);
                    m_Textbox.Attributes.Add("onchange", m_Attribute + changeDefaultSelectValue);
                    m_Textbox.RenderControl(writer);

                    writer.Write("</div>");

                    const string commstr = "(document.getElementById('{0}'),document.getElementById('{1}'))";
                    var ondropchange = string.Format("ondropchange" + commstr, m_Textbox.ClientID,
                                                        base.ClientID);

                    Style.Clear();
                    Attributes.Add("onchange", m_Attribute + ondropchange);
                    base.Height = dropHeight;

                    Style.Add(HtmlTextWriterStyle.FontSize, "12px");
                    Style.Add(HtmlTextWriterStyle.FontFamily, "Times New Roman");
                    Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdownlist");
                    base.Render(writer);

                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
            }
            else
            {
                if (ShowSelectTip)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdownlist");
                    Attributes.Add("onmouseover", m_Attribute + string.Format("showTip(document.getElementById('{0}'))", this.ID + "div"));
                    Attributes.Add("onmouseout", m_Attribute + string.Format("hideTip(document.getElementById('{0}'))", this.ID + "div"));
                    Attributes.Add("onchange", m_Attribute + string.Format("ondivchange(this,document.getElementById('{0}'))", this.ID + "div"));
                    //writer.AddAttribute("onmouseover", m_Attribute + string.Format("showTip(document.getElementById('{0}'))", this.ID + "div"));
                    //writer.AddAttribute("onmouseout", m_Attribute + string.Format("hideTip(document.getElementById('{0}'))", this.ID + "div"));
                    //writer.AddAttribute("onchange", m_Attribute + string.Format("ondivchange(this,document.getElementById('{0}'))", this.ID + "div"));
                    base.Render(writer);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "buttonshadow");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "1001");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "Cornsilk");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Left, "5px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Top, "22px");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "div");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write(this.SelectedItem.Text);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdownlist");
                    base.Render(writer);
                }

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

            if (DropDownStyle == DropDownType.DropDown || ShowSelectTip)
            {
                string url = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.ComboBox.Resources.ComboBox.js");
                if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "ComboBox.Resources.ComboBox.js"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "ComboBox.Resources.ComboBox.js", url);
                }
            }
        }
    }
}