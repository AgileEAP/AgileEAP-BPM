
namespace EAFrame.WebControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Collections.Specialized;
    using System.Data.SqlClient;
    using System.Data;
    using System.Security.Permissions;

    /// <summary>
    /// XLoadTree树控件
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:XLoadTree runat=server></{0}:XLoadTree>")]
    public class XLoadTree : Control
    {
        /// <summary>
        /// 根节点显示文本
        /// </summary>
        public string RootText
        {
            get
            {
                object o = ViewState["RootText"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["RootText"] = value;
            }
        }

        /// <summary>
        /// 根节点链接
        /// </summary>
        public string RootAction
        {
            get
            {
                object o = ViewState["RootAction"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["RootAction"] = value;
            }
        }

        /// <summary>
        /// 根节点折叠状态图片
        /// </summary>
        public string RootIcon
        {
            get
            {
                object o = ViewState["RootIcon"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["RootIcon"] = value;
            }
        }

        /// <summary>
        /// 根节点折叠状态图片
        /// </summary>
        public string RootTarget
        {
            get
            {
                object o = ViewState["RootTarget"];
                return (o == null) ? "main_right" : (string)o;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ViewState["RootTarget"] = "main_right";
                }
                else
                {
                    ViewState["RootTarget"] = value;
                }
            }
        }

        /// <summary>
        /// 根节点打开状态图片
        /// </summary>
        public string OpenRootIcon
        {
            get
            {
                object o = ViewState["OpenRootIcon"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["OpenRootIcon"] = value;
            }
        }


        /// <summary>
        /// 文件夹折叠状态图片
        /// </summary>
        public string FolderIcon
        {
            get
            {
                object o = ViewState["FolderIcon"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["FolderIcon"] = value;
            }
        }

        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlSrc
        {
            get
            {
                object o = ViewState["XmlSrc"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["XmlSrc"] = value;
            }
        }

        /// <summary>
        /// 文件夹打开状态图片
        /// </summary>
        public string OpenFolderIcon
        {
            get
            {
                object o = ViewState["OpenFolderIcon"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["OpenFolderIcon"] = value;
            }
        }

        /// <summary>
        /// 无子节点图片
        /// </summary>
        public string FileIcon
        {
            get
            {
                object o = ViewState["FileIcon"];
                return (o == null) ? string.Empty : (string)o;
            }
            set
            {
                ViewState["FileIcon"] = value;
            }
        }

        /// <summary>
        /// 是否增加复选按钮
        /// </summary>
        public bool CheckBox
        {
            get
            {
                object o = ViewState["CheckBox"];
                return (o == null) ? false : (bool)o;
            }
            set
            {
                ViewState["CheckBox"] = value;
            }
        }

        /// <summary>
        /// 是否增加复选按钮
        /// </summary>
        public bool IsApplyStyleSheetCss
        {
            get
            {
                object o = ViewState["IsApplyStyleSheetCss"];
                return (o == null) ? true : (bool)o;
            }
            set
            {
                ViewState["IsApplyStyleSheetCss"] = value;
            }
        }

        /// <summary>
        /// 处理 Control.Load 事件
        /// </summary>
        /// <param name="e">包含事件数据的 EventArgs 对象</param>       
        protected override void OnLoad(EventArgs e)
        {
            //注册JS
            Type t = this.GetType();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, "EAFrame.WebControls.RadTree.Resources.xtree.js"))
            {
                string url = Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xtree.js");
                Page.ClientScript.RegisterClientScriptInclude(t, "EAFrame.WebControls.RadTree.Resources.xtree.js", url);
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, "EAFrame.WebControls.RadTree.Resources.xmlextras.js"))
            {
                string url = Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xmlextras.js");
                Page.ClientScript.RegisterClientScriptInclude(t, "EAFrame.WebControls.RadTree.Resources.xmlextras.js", url);
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, "EAFrame.WebControls.RadTree.Resources.xloadtree.js"))
            {
                string url = Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xloadtree.js");
                Page.ClientScript.RegisterClientScriptInclude(t, "EAFrame.WebControls.RadTree.Resources.xloadtree.js", url);
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, "EAFrame.WebControls.RadTree.Resources.xmenu.js"))
            {
                string url = Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xmenu.js");
                Page.ClientScript.RegisterClientScriptInclude(t, "EAFrame.WebControls.RadTree.Resources.xmenu.js", url);
            }

            if (!IsApplyStyleSheetCss)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered(t, "EAFrame.WebControls.RadTree.Resources.css"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(t, "EAFrame.WebControls.RadTree.Resources.css", "");

                    //注册CSS
                    HtmlLink xmenuLink = new HtmlLink();
                    xmenuLink.Attributes.Add("type", "text/css");
                    xmenuLink.Attributes.Add("rel", "stylesheet");
                    xmenuLink.Attributes.Add("href", Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xmenu.css"));
                    Page.Header.Controls.Add(xmenuLink);
                    HtmlLink xtreeLink = new HtmlLink();
                    xtreeLink.Attributes.Add("type", "text/css");
                    xtreeLink.Attributes.Add("rel", "stylesheet");
                    xtreeLink.Attributes.Add("href", Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.xtree.css"));
                    Page.Header.Controls.Add(xtreeLink);
                }
            }
        }

        /// <summary>
        /// 使用HtmlTextWriter类呈现HTML代码
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        protected override void Render(HtmlTextWriter writer)
        {
            Type t = this.GetType();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("EAFrame.WebControls.RadTree.Resources.rightMenujs"))
            {
                //生成右键Div
                writer.Write("<div id=\"menudata\"></div>");
            }
            //生成树容器
            writer.Write("<div id=\"treedata\"> </div>");
            // 生成树控件JS
            writer.Write("<script type=\"text/javascript\">\n");
            string rootIcon, openRootIcon, folderIcon, openFolderIcon, fileIcon;
            rootIcon = string.IsNullOrEmpty(RootIcon) ? Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.closefolder.gif") : RootIcon;
            if (RootIcon == "WebSite")
            {
                rootIcon = Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.WebSite.gif");
            }
            openRootIcon = string.IsNullOrEmpty(OpenRootIcon) ? Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.closefolder.gif") : OpenRootIcon;
            folderIcon = string.IsNullOrEmpty(FolderIcon) ? Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.closefolder.gif") : FolderIcon;
            openFolderIcon = string.IsNullOrEmpty(OpenFolderIcon) ? Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.openfolder.gif") : OpenFolderIcon;
            fileIcon = string.IsNullOrEmpty(FileIcon) ? Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.closefolder.gif") : FileIcon;

            writer.Write("webFXTreeConfig.rootIcon		= \"" + rootIcon + "\";\n");
            writer.Write("webFXTreeConfig.openRootIcon	= \"" + openRootIcon + "\";\n");
            writer.Write("webFXTreeConfig.folderIcon	= \"" + folderIcon + "\";\n");
            writer.Write("webFXTreeConfig.openFolderIcon= \"" + openFolderIcon + "\";\n");
            writer.Write("webFXTreeConfig.fileIcon		= \"" + fileIcon + "\";\n");
            writer.Write("webFXTreeConfig.containerIcon	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.closefolder.gif") + "\";\n"); //关闭文件夹
            writer.Write("webFXTreeConfig.linkIcon	    = \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.outlink.gif") + "\";\n");　   //外部链接
            writer.Write("webFXTreeConfig.singleIcon	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.singlepage.gif") + "\";\n");  //单个页面
            //权限图片
            writer.Write("webFXTreeConfig.forbidclosefolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.forbidclosefolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.forbidopenfolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.forbidopenfolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.lockclosefolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.lockclosefolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.lockopenfolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.lockopenfolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.purviewclosefolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.purviewclosefolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.purviewopenfolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.purviewopenfolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.halfclosefolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.halfcolsefolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.halfopenfolder	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.halfopenfolder.gif") + "\";\n");
            writer.Write("webFXTreeConfig.Archiving	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.Archiving.gif") + "\";\n");
            writer.Write("webFXTreeConfig.openArchiving	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.openArchiving.gif") + "\";\n");

            //图线链接图片
            writer.Write("webFXTreeConfig.lMinusIcon	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.Lminus.png") + "\";\n");
            writer.Write("webFXTreeConfig.lPlusIcon		= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.Lplus.png") + "\";\n");
            writer.Write("webFXTreeConfig.tMinusIcon	= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.Tminus.png") + "\";\n");
            writer.Write("webFXTreeConfig.tPlusIcon		= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.Tplus.png") + "\";\n");
            writer.Write("webFXTreeConfig.iIcon			= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.I.png") + "\";\n");
            writer.Write("webFXTreeConfig.lIcon			= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.L.png") + "\";\n");
            writer.Write("webFXTreeConfig.tIcon			= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.T.png") + "\";\n");
            writer.Write("webFXTreeConfig.blankIcon		= \"" + Page.ClientScript.GetWebResourceUrl(t, "EAFrame.WebControls.RadTree.Resources.blank.png") + "\";\n");

            if (CheckBox)
            {
                writer.Write("webFXTreeConfig.checkbox = true ;");
            }
            writer.Write("var rti;\n");
            writer.Write("var RootText=\"" + RootText + "\";\n");
            writer.Write("var XmlSrc=\"" + XmlSrc + "\";\n");
            writer.Write("var RootAction=\"" + RootAction + "\";\n");
            writer.Write("var rootIcon=\"" + rootIcon + "\";\n");
            writer.Write("var RootTarget=\"" + RootTarget + "\";\n");
            writer.Write("var tree = new WebFXLoadTree(RootText,XmlSrc,RootAction,\"\",rootIcon,rootIcon,RootTarget);\n");
            //writer.Write("document.write(tree);\n");
            writer.Write("document.getElementById('treedata').innerHTML=tree;\n");

            writer.Write("if (webFXTreeConfig.expanIds != \"\") {\n");
            writer.Write("    var arrId = webFXTreeConfig.expanIds.split(\",\");\n");
            writer.Write("    for (i=0; i < arrId.length;i++){\n");
            writer.Write("        webFXTreeHandler.toggle(arrId[i]);\n");
            writer.Write("    }\n");
            writer.Write("}\n");
            writer.Write("</script>\n");
        }
    }
}
