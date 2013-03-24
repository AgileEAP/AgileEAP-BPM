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
    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("MenuItems"),
    ParseChildren(true, "MenuItems"),
    ToolboxData("<{0}:MenuItemGroup runat=server></{0}:MenuItemGroup>")]
    public class MenuItemGroup : CompositeControl
    {

        #region     自定义成员变量属性
        /// <summary>
        /// 菜单组标题
        /// </summary>
        private string title = "菜单组";
        [Bindable(true),
        Category("Appearance"),
        DefaultValue("")]
        public string GroupTitle
        {
            get
            {
                // EnsureChildControls();
                object obj = ViewState["GroupTitle"];
                return obj == null ? title : (string)obj;
            }
            set { ViewState["GroupTitle"] = value; }
        }

        /// <summary>
        /// 设置菜单组是否展开
        /// </summary>
        [Bindable(true),
        Category("Appearance"),
        DefaultValue(false),
        Description("设置菜单组是否展开")]
        public bool Expanded
        {
            get
            {
                EnsureChildControls();
                object obj = ViewState["Expanded"];
                return obj == null ? false : (bool)obj;
            }
            set
            {
                ViewState["Expanded"] = value;
            }
        }

        private string menuGroupImageUrl = string.Empty;
        /// <summary>
        /// 菜单组图标
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor)),
        DefaultValue("")]
        public string MenuGroupImageUrl
        {
            get
            {
                EnsureChildControls();
                return menuGroupImageUrl;
            }
            set { menuGroupImageUrl = value; }
        }

        private Unit _width = new Unit(160);
        /// <summary>
        /// 宽度
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        DefaultValue("")]
        public override Unit Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }


        private List<MenuItem> menuItems = new List<MenuItem>();
        /// <summary>
        /// 菜单项列表
        /// </summary>
        [Category("Behavior"),
        Description("The menuitem collection"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(MenuItemEditor), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public List<MenuItem> MenuItems
        {
            get
            {
                // EnsureChildControls();
                if (menuItems == null)
                {
                    menuItems = new List<MenuItem>();
                }
                return menuItems;
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

        /// <summary>
        ///当前程序路径
        /// </summary>
        public string AppPath
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        #endregion

        #region  自定义方法

        public MenuItemGroup()
        {
        }

        #endregion

        #region      重载方法
        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            foreach (MenuItem menuItem in menuItems)
            {
                Controls.Add(menuItem);
            }
            this.ChildControlsCreated = true;
        }


        /// <summary>
        /// 呈现控件内容
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            string menuItemGroupName = "MenuGroup_" + this.UniqueID;

            output.AddAttribute(HtmlTextWriterAttribute.Onclick, "showMenuGroup('" + menuItemGroupName + "')", false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_group_header");
            output.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("MenuGroup_Header_{0}", this.UniqueID), false);
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write(GroupTitle);
            output.RenderEndTag();
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Id, menuItemGroupName, false);
            output.AddAttribute(HtmlTextWriterAttribute.Style, "display:" + (Expanded == false ? "none" : "block"), false);
            output.AddAttribute(HtmlTextWriterAttribute.Class, "navmenu_group");
            output.RenderBeginTag(HtmlTextWriterTag.Table);

            MenuExplorer parentMenuExplorer = this.Parent as MenuExplorer;
            foreach (MenuItem menuItem in menuItems)
            {
                if (parentMenuExplorer != null)
                    if (parentMenuExplorer.IsAuthenticated(parentMenuExplorer.UserRole, menuItem.ViewRole))
                    {
                        menuItem.MenuItemImageUrl = parentMenuExplorer.MenuItemImageUrl;
                        menuItem.RenderControl(output);
                    }
            }
            output.RenderEndTag();
        }
        #endregion

    }

}
