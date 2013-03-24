//************************************************************
//��Ŀ:WebControls���������������� 
//��Ȩ:Copyright(c) 2008,̷�λ� ��������������������������
//ģ����:MenuExplorer
//˵��:����һ���˵���
//�汾��1.0
//�޸���ʷ��
//			V1.0
//˵������ӵ�������
//��ע�����
//	2008-1-25 by ̷�λ�
//
//          V2.0
//˵����ʵ��ˢ�º󱣴�˵��ĵ�ǰ״̬
//��ע��δ���
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
        #region �Զ������

        private List<MenuItemGroup> menuItemGroups = new List<MenuItemGroup>();
        private Image footerImage = new Image();
        private Image headerImage = new Image();
        #endregion

        #region �Զ�������
        /// <summary>
        /// �˵�����
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

        private string title = "�˵�";
        /// <summary>
        /// �����˵�����
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("�˵�")]
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
        /// ��������ߵ�Ȩ��
        /// </summary>
        private string userRole = "all";
        [Bindable(true),
        Category("Action"),
        DefaultValue("all"),
        Description("��������ߵ�Ȩ��")]
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

        #region �˵���ʽ
        /// <summary>
        /// �������˵�ͷ��ͼƬ
        /// </summary>
        private string menuExplorerHeaderImageUrl = "";
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("�������˵�ͷ��ͼƬUrl")]
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
        /// ���ò˵���ı���ͼ
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("���ò˵���ı���ͼUrl")]
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
        /// ���ò˵����ͼ��
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("���ò˵����ͼ��Url")]
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
        /// ���ò˵��ײ���ͼ��
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("���ò˵��ײ���ͼ��Url")]
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
        /// ���ò˵��Ƿ���ʾ
        /// </summary>
        [Bindable(false)]
        [Category("MenuExplorerStyle")]
        [DefaultValue(false)]
        [Description("���ò˵��Ƿ���ʾFooter")]
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

        #region �Զ��巽��
        /// <summary>
        /// ע��ؼ���Դ
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
        /// ���������˵�ͷ
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
        ///  ���������˵���
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
        /// ���������˵�
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
        /// �Ƿ���֤ͨ��
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

        #region ���ط���
        /// <summary>
        /// ���캯��
        /// </summary>
        public MenuExplorer()
        {
        }

        /// <summary>
        /// ���ֿ�ʼ���
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
        /// ���ֽ������
        /// </summary>
        /// <param name="output"></param>
        public override void RenderEndTag(HtmlTextWriter output)
        {
            output.RenderEndTag();
        }

        /// <summary>
        /// �����ӿؼ�
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
        /// ��������
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //���ֲ˵�ͷ
            CreateMenuExplorerHeader(output);

            //���ֲ˵���
            CreateMenuExplorerMenuGroups(output);

            //���ֲ˵�β
            CreateMenuExplorerFooter(output);
        }

        #endregion
    }
}
