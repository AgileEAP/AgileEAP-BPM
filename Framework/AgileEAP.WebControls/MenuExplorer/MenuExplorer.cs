//************************************************************
//项目:WebControls　　　　　　　　 
//版权:Copyright(c) 2008,谭任辉 　　　　　　　　　　　　　
//模块名:MenuExplorer
//说明:定义一个菜单组
//版本：1.0
//修改历史：
//			V1.0
//说明：添加导航功能
//备注：完成
//	2008-1-25 by 谭任辉
//
//          V2.0
//说明：实现刷新后保存菜单的当前状态
//备注：未完成
//	
//************************************************************
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

namespace EAFrame.WebControls
{
    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("MenuItemGroups"),
    ParseChildren(true, "MenuItemGroups"),
    ToolboxData("<{0}:MenuExplorer runat=server></{0}:MenuExplorer>")]
    public class MenuExplorer : CompositeControl
    {
        #region 自定义变量

        private List<MenuItemGroup> menuItemGroups = new List<MenuItemGroup>();
        private Image footerImage = new Image();
        private Image headerImage = new Image();
        #endregion

        #region 自定义属性
        /// <summary>
        /// 菜单项组
        /// </summary>
        [Category("Behavior"),
        Description("The menuitem collection"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(MenuItemGroupEditor), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public List<MenuItemGroup> MenuItemGroups
        {
            get
            {
                if (menuItemGroups == null)
                    menuItemGroups = new List<MenuItemGroup>();
                return menuItemGroups;
            }
        }

        private string title = "菜单";
        /// <summary>
        /// 导航菜单标题
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("菜单")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["Title"];
                return obj == null ? title : (string)obj;
            }

            set
            {
                ViewState["Title"] = value;
            }
        }

        /// <summary>
        /// 定义访问者的权限
        /// </summary>
        private string userRole = "all";
        [Bindable(true),
        Category("Action"),
        DefaultValue("all"),
        Description("定义访问者的权限")]
        public string UserRole
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["UserRole"];
                return obj == null ? userRole : (string)obj;
            }
            set
            {
                ViewState["UserRole"] = value;
            }
        }
        #endregion

        #region 菜单样式
        /// <summary>
        /// 设置主菜单头的图片
        /// </summary>
        private string menuExplorerHeaderImageUrl = "";
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("设置主菜单头的图片Url")]
        [DefaultValue("")]
        public string MenuExplorerHeaderImageUrl
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["MenuExplorerHeaderImageUrl"];
                return obj == null ? menuExplorerHeaderImageUrl : (string)obj;
            }
            set
            {
                ViewState["MenuExplorerHeaderImageUrl"] = value;
            }
        }

        private string menuItemGroupImageUrl = "";
        /// <summary>
        /// 设置菜单组的背景图
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("设置菜单组的背景图Url")]
        public string MenuItemGroupImageUrl
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["MenuItemGroupImageUrl"];
                return obj == null ? menuItemGroupImageUrl : (string)obj;
            }
            set
            {
                ViewState["MenuItemGroupImageUrl"] = value;
            }
        }

        private string menuItemImageUrl = "";
        /// <summary>
        /// 设置菜单项的图标
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("设置菜单项的图标Url")]
        public string MenuItemImageUrl
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["MenuItemImageUrl"];
                return obj == null ? menuItemImageUrl : (string)obj;
            }
            set
            {
                ViewState["MenuItemImageUrl"] = value;
            }
        }

        private string menuExplorerFooterImageUrl = "";
        /// <summary>
        /// 设置菜单底部的图标
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("设置菜单底部的图标Url")]
        public string MenuExplorerFooterImageUrl
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["MenuExplorerFooterImageUrl"];
                return obj == null ? menuExplorerFooterImageUrl : (string)obj; ;
            }
            set
            {
                ViewState["MenuExplorerFooterImageUrl"] = value;
            }
        }

        private bool showFooter = false;
        /// <summary>
        /// 设置菜单是否显示
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue(false)]
        [Description("设置菜单是否显示Footer")]
        public bool ShowFooter
        {
            get
            {
                //EnsureChildControls();
                object obj = ViewState["ShowFooter"];
                return obj == null ? showFooter : (bool)obj;
            }
            set
            {
                ViewState["ShowFooter"] = value;
            }
        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 注册控件资源
        /// </summary>
        private void InitWebSources()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            menuExplorerHeaderImageUrl = clientManager.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuExplorerHeader.jpg");
            menuExplorerFooterImageUrl = clientManager.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuExplorerFooter.jpg");
            menuItemGroupImageUrl = clientManager.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuItemGroup.gif");
            menuItemImageUrl = clientManager.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuItem.jpg");

            if (!clientManager.IsClientScriptIncludeRegistered(type, "MenuExplorer.Resources.MenuExplorer.js"))
            {
                string url = clientManager.GetWebResourceUrl(type, "EAFrame.WebControls.MenuExplorer.Resources.MenuExplorer.js");
                clientManager.RegisterClientScriptInclude(type, "MenuExplorer.Resources.MenuExplorer.js", url);
            }
        }

        /// <summary>
        /// 创建导航菜单头
        /// </summary>
        /// <param name="output"></param>
        private void CreateMenuExplorerHeader(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("background-position: center center;"), false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_header");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();
        }

        /// <summary>
        ///  创建导航菜单组
        /// </summary>
        /// <param name="output"></param>
        private void CreateMenuExplorerMenuGroups(HtmlTextWriter output)
        {
            foreach (MenuItemGroup menuItemGroup in menuItemGroups)
            {
                if (IsAuthenticated(UserRole, menuItemGroup.ViewRole))
                {
                    menuItemGroup.MenuGroupImageUrl = MenuItemGroupImageUrl;
                    menuItemGroup.RenderControl(output);
                }
            }
        }

        /// <summary>
        /// 创建导航菜单
        /// </summary>
        /// <param name="output"></param>
        private void CreateMenuExplorerFooter(HtmlTextWriter output)
        {
            if (ShowFooter)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_footer");
                output.RenderBeginTag(HtmlTextWriterTag.Div);
                footerImage.RenderControl(output);
                output.RenderEndTag();
            }
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="viewRole"></param>
        /// <returns></returns>
        public bool IsAuthenticated(string userRole, string viewRole)
        {
            if (viewRole == "all")
                return true;
            string[] userRoles = userRole.ToLower().Split(',');
            string[] viewRoles = viewRole.ToLower().Split(',');

            foreach (string UserRole in userRoles)
            {
                foreach (string ViewRole in viewRoles)
                {
                    if (UserRole.Trim() == ViewRole.Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 重载方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuExplorer()
        {
        }

        /// <summary>
        /// 呈现开始标记
        /// </summary>
        /// <param name="output"></param>
        public override void RenderBeginTag(HtmlTextWriter output)
        {
            if (Page != null)
            {
                string cssFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "EAFrame.WebControls.MenuExplorer.Resources.MenuExplorer.css");
                output.Write(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssFile));
            }

            output.AddAttribute(HtmlTextWriterAttribute.Id, "menuExplorerContainer", false);
            output.RenderBeginTag(HtmlTextWriterTag.Div);
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
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {

            Controls.Clear();
            InitWebSources();

            headerImage.ImageAlign = ImageAlign.AbsMiddle;
            headerImage.ImageUrl = menuExplorerHeaderImageUrl;
            Controls.Add(headerImage);

            footerImage.ImageAlign = ImageAlign.Middle;
            footerImage.Width = 164;
            footerImage.Height = 26;
            footerImage.ImageUrl = menuExplorerFooterImageUrl;
            Controls.Add(footerImage);

            if (menuItemGroups.Count == 0)
                menuItemGroups.Add(new MenuItemGroup());
            foreach (MenuItemGroup menuItemGroup in menuItemGroups)
            {
                Controls.Add(menuItemGroup);
            }
            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// 呈现内容
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //呈现菜单头
            CreateMenuExplorerHeader(output);

            //呈现菜单组
            CreateMenuExplorerMenuGroups(output);

            //呈现菜单尾
            CreateMenuExplorerFooter(output);
        }

        #endregion
    }
}
