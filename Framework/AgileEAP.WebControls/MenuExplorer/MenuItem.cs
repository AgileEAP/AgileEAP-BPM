using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Security.Permissions;

namespace EAFrame.WebControls
{
    [AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Text")]
    [ToolboxData("<{0}:MenuItem runat=server></{0}:MenuItem>")]
    public class MenuItem : WebControl
    {

        #region �Զ������

        private System.Web.UI.WebControls.Image menuItemImage = new System.Web.UI.WebControls.Image();
        private HyperLink link = new HyperLink();

        #endregion


        #region  �Զ�������

        private string commandName = "Action";
        /// <summary>
        /// �����ѯ�ַ����Ĳ�������
        /// </summary>
        [Bindable(true),
        DefaultValue("Action"),
        Description("�����ѯ�ַ����Ĳ�������"),
        Category("Action")]
        public string CommandName
        {
            get
            {
                object obj = ViewState["CommandName"];
                return obj == null ? commandName : (string)obj;
            }
            set { ViewState["CommandName"] = value; }
        }

        private string commandArgument = "Query";
        /// <summary>
        /// �����ѯ�ַ�����ֵ
        /// </summary>
        [Bindable(true),
        DefaultValue("Action"),
        Description("�����ѯ�ַ�����ֵ"),
        Category("Action")]
        public string CommandArgument
        {
            get
            {
                object obj = ViewState["CommandArgument"];
                return obj == null ? commandArgument : (string)obj;
            }
            set { ViewState["CommandArgument"] = value; }
        }

        /// <summary>
        /// �߶�
        /// </summary>
        private Unit height;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("50")]
        public override Unit Height
        {
            get
            {
                //  EnsureChildControls();
                return height;
            }
            set
            {
                height = value;
            }
        }

        private Unit width;
        /// <summary>
        /// ���
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("18")]
        public override Unit Width
        {
            get
            {
                //  EnsureChildControls();
                return width;
            }
            set
            {
                width = value;
            }
        }

        private string menuItemImageUrl = string.Empty;
        /// <summary>
        /// �˵���ͼ��
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("default.gif")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        public string MenuItemImageUrl
        {
            get
            {
                EnsureChildControls();
                return menuItemImageUrl;
            }
            set
            {
                menuItemImageUrl = value;
            }
        }

        /// <summary>
        /// Ŀ����
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("_self")]
        public string Target
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["Target"];
                return obj == null ? "_self" : (string)obj;
            }
            set
            {
                ViewState["Target"] = value;
            }
        }

        private string linkUrl = "Default.aspx";
        /// <summary>
        /// �˵�������
        /// </summary>
        [Bindable(true)]
        [Category("Action")]
        [DefaultValue("default.aspx")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        public string LinkUrl
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["LinkUrl"];
                return obj == null ? linkUrl : (string)obj;
            }
            set
            {
                ViewState["LinkUrl"] = value;
            }
        }

        private string text = "�˵���";
        /// <summary>
        /// �˵���
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["Text"];
                return obj == null ? text : (string)obj;
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        private string viewRole = "all";
        /// <summary>
        /// ������ʸò˵�����Ҫ�����Ȩ��
        /// </summary>
        [Bindable(true),
        Category("Action"),
        DefaultValue("all"),
        Description("������ʸò˵�����Ҫ�����Ȩ��")]
        public string ViewRole
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["ViewRole"];
                return obj == null ? viewRole : (string)obj;
            }
            set
            {
                ViewState["ViewRole"] = value;
            }
        }
        #endregion


        #region �Զ��巽��

        /// <summary>
        /// ���캯��
        /// </summary>
        public MenuItem()
        {

        }

        /// <summary>
        /// ע��ؼ���Դ
        /// </summary>
        private void InitWebR()
        {
            MenuExplorer e = this.Parent.Parent as MenuExplorer;
            if (e != null)
                menuItemImageUrl = e.MenuItemImageUrl;
            else
                menuItemImageUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuItem.jpg");
        }

        #endregion

        #region ���ط���
        /// <summary>
        /// �����ӿؼ�
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            menuItemImage.ImageUrl = menuItemImageUrl;
            menuItemImage.Height = height;
            menuItemImage.Width = width;
            this.Controls.Add(menuItemImage);


            link.Text = Text;
            link.Target = Target;
            link.NavigateUrl = LinkUrl;// +"?" + CommandName + "=" + CommandArgument;
            Controls.Add(link);

            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// ���ֿ�ʼ���
        /// </summary>
        /// <param name="output"></param>
        public override void RenderBeginTag(HtmlTextWriter output)
        {
            menuItemImage.ImageUrl = MenuItemImageUrl;
            output.AddAttribute(HtmlTextWriterAttribute.Width, "164", false);
            output.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            if (this.Parent != null)
                output.AddAttribute("onclick", "activeMenuGroup('" + this.Parent.UniqueID + "','" + this.LinkUrl + "',this)", false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_item");
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
        }

        /// <summary>
        /// ���ֽ������
        /// </summary>
        /// <param name="output"></param>
        public override void RenderEndTag(HtmlTextWriter output)
        {
            output.RenderEndTag();
        }

        /// <summary>
        /// ���ֿؼ�����
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Width, "30%", false);
            output.AddAttribute(HtmlTextWriterAttribute.Align, "right", false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_item_left");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            //menuItemImage.RenderControl(output);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_item_left_image");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Align, "left", false);
            output.AddAttribute(HtmlTextWriterAttribute.Width, "70%", false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_item_right");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            link.RenderControl(output);
            output.RenderEndTag();
        }

        #endregion
    }
}
