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

        #region 自定义变量

        private System.Web.UI.WebControls.Image menuItemImage = new System.Web.UI.WebControls.Image();
        private HyperLink link = new HyperLink();

        #endregion


        #region  自定义属性

        private string commandName = "Action";
        /// <summary>
        /// 定义查询字符串的参数名称
        /// </summary>
        [Bindable(true),
        DefaultValue("Action"),
        Description("定义查询字符串的参数名称"),
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
        /// 定义查询字符串的值
        /// </summary>
        [Bindable(true),
        DefaultValue("Action"),
        Description("定义查询字符串的值"),
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
        /// 高度
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
        /// 宽度
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
        /// 菜单项图标
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
        /// 目标框架
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
        /// 菜单项链接
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

        private string text = "菜单项";
        /// <summary>
        /// 菜单项
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
        /// 定义访问该菜单组所要的最低权限
        /// </summary>
        [Bindable(true),
        Category("Action"),
        DefaultValue("all"),
        Description("定义访问该菜单组所要的最低权限")]
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


        #region 自定义方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuItem()
        {

        }

        /// <summary>
        /// 注册控件资源
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

        #region 重载方法
        /// <summary>
        /// 创建子控件
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
        /// 呈现开始标记
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
        /// 呈现结束标记
        /// </summary>
        /// <param name="output"></param>
        public override void RenderEndTag(HtmlTextWriter output)
        {
            output.RenderEndTag();
        }

        /// <summary>
        /// 呈现控件内容
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
