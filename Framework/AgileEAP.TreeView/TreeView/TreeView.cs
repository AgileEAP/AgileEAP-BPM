//************************************************************
//项目:WebControls　　　　　　　　 
//版权:Copyright(c) 2008,谭任辉 　　　　　　　　　　　　　
//模块名:TreeView
//说明:异步加载树
//版本：1.0
//修改历史：
//			V1.0
//说明：
//备注：完成
//	2011-8-2 by 谭任辉
//
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
using System.Security.Permissions;

namespace AgileEAP.WebControls
{
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Nodes"),
    ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>")]
    public class TreeView : CompositeControl
    {
        #region Style Properties

        private string treeView_bottom_line = string.Empty;
        public string TreeView_bottom_line
        {
            get
            {
                return treeView_bottom_line;
            }
            set
            {
                treeView_bottom_line = value;
            }
        }

        private string treeView_middle_line = string.Empty;
        public string TreeView_middle_line
        {
            get
            {
                return treeView_middle_line;
            }
            set
            {
                treeView_middle_line = value;
            }
        }

        private string treeView_root_line = string.Empty;
        public string TreeView_root_line
        {
            get
            {
                return treeView_root_line;
            }
            set
            {
                treeView_root_line = value;
            }
        }

        private string treeView_top_line = string.Empty;
        public string TreeView_top_line
        {
            get
            {
                return treeView_top_line;
            }
            set
            {
                treeView_top_line = value;
            }
        }

        private string treeView_vertical_line = string.Empty;
        public string TreeView_vertical_line
        {
            get
            {
                return treeView_vertical_line;
            }
            set
            {
                treeView_vertical_line = value;
            }
        }

        private string treeView_folder = string.Empty;
        private string parentNodeIcon = string.Empty;
        public string ParentNodeIcon
        {
            get
            {
                return string.IsNullOrEmpty(parentNodeIcon) ? treeView_folder : parentNodeIcon;
            }
            set
            {
                parentNodeIcon = value;
            }
        }

        private string treeView_folder_open = string.Empty;
        public string ParentNode_Open
        {
            get
            {
                return treeView_folder_open;
            }
            set
            {
                treeView_folder_open = value;
            }
        }

        private string treeView_minus = string.Empty;
        public string TreeView_minus
        {
            get
            {
                return treeView_minus;
            }
            set
            {
                treeView_minus = value;
            }
        }

        private string treeView_node = string.Empty;
        private string leafNodeIcon = string.Empty;
        public string LeafNodeIcon
        {
            get
            {
                return string.IsNullOrEmpty(leafNodeIcon) ? treeView_node : leafNodeIcon;
            }
            set
            {
                leafNodeIcon = value;
            }
        }

        private string treeView_plus = string.Empty;
        public string TreeView_plus
        {
            get
            {
                return treeView_plus;
            }
            set
            {
                treeView_plus = value;
            }
        }

        private string checkbox_checked = string.Empty;
        public string Checkbox_checked
        {
            get
            {
                return checkbox_checked;
            }
            set
            {
                checkbox_checked = value;
            }
        }
        private string checkbox_half_checked = string.Empty;
        public string Checkbox_half_checked
        {
            get
            {
                return checkbox_half_checked;
            }
            set
            {
                checkbox_half_checked = value;
            }
        }

        private string checkbox_unchecked = string.Empty;
        public string Checkbox_unchecked
        {
            get
            {
                return checkbox_unchecked;
            }
            set
            {
                checkbox_unchecked = value;
            }
        }

        private string context_menu_gradient = string.Empty;
        public string Context_menu_gradient
        {
            get
            {
                return context_menu_gradient;
            }
            set
            {
                context_menu_gradient = value;
            }
        }

        private string drag_drop_ind1 = string.Empty;
        public string Drag_drop_ind1
        {
            get
            {
                return drag_drop_ind1;
            }
            set
            {
                drag_drop_ind1 = value;
            }
        }

        private string drag_drop_ind2 = string.Empty;
        public string Drag_drop_ind2
        {
            get
            {
                return drag_drop_ind2;
            }
            set
            {
                drag_drop_ind2 = value;
            }
        }

        private string folder_close = string.Empty;
        public string Folder_close
        {
            get
            {
                return folder_close;
            }
            set
            {
                folder_close = value;
            }
        }

        private string folder_dots = string.Empty;
        public string Folder_dots
        {
            get
            {
                return folder_dots;
            }
            set
            {
                folder_dots = value;
            }
        }
        private string folder_folder = string.Empty;
        public string Folder_folder
        {
            get
            {
                return folder_folder;
            }
            set
            {
                folder_folder = value;
            }
        }

        private string folder_lastsub = string.Empty;
        public string Folder_lastsub
        {
            get
            {
                return folder_lastsub;
            }
            set
            {
                folder_lastsub = value;
            }
        }

        private string folder_open = string.Empty;
        public string Folder_open
        {
            get
            {
                return folder_open;
            }
            set
            {
                folder_open = value;
            }
        }

        private string folder_sub = string.Empty;
        public string Folder_sub
        {
            get
            {
                return folder_sub;
            }
            set
            {
                folder_sub = value;
            }
        }

        private string treeView_onLoad = string.Empty;
        public string TreeView_onload
        {
            get
            {
                return treeView_onLoad;
            }
            set
            {
                treeView_onLoad = value;
            }
        }

        private string treeView_drag_enable = string.Empty;
        public string TreeView_drag_enable
        {
            get
            {
                return treeView_drag_enable;
            }
            set
            {
                treeView_drag_enable = value;
            }
        }

        private string treeView_drag_disable = string.Empty;
        public string TreeView_drag_disable
        {
            get
            {
                return treeView_drag_disable;
            }
            set
            {
                treeView_drag_disable = value;
            }
        }

        private TreeViewNodeCollection nodes = new TreeViewNodeCollection();
        public TreeViewNodeCollection Nodes
        {
            get
            {
                return nodes;
            }
            set
            {
                nodes = value;
            }
        }

        private string containerCssClass = "treeView-container";
        public string ContainerCssClass
        {
            get
            {
                return containerCssClass;
            }
            set
            {
                containerCssClass = value;
            }
        }

        private string treeCssClass = "treeView-tree";
        public string TreeCssClass
        {
            get
            {
                return treeCssClass;
            }
            set
            {
                treeCssClass = value;
            }
        }

        private string parentNodeCssClass = "treeView-parentNode";
        public string ParentNodeCssClass
        {
            get
            {
                return parentNodeCssClass;
            }
            set
            {
                parentNodeCssClass = value;
            }
        }

        private string dragContainerCssClass = "treeView-drag-container";
        public string DragContainerCssClass
        {
            get
            {
                return dragContainerCssClass;
            }
            set
            {
                dragContainerCssClass = value;
            }
        }

        private string leafNodeCssClass = "treeView-leafNode";
        public string LeafNodeCssClass
        {
            get
            {
                return leafNodeCssClass;
            }
            set
            {
                leafNodeCssClass = value;
            }
        }

        private bool showCheckBox = false;
        public bool ShowCheckBox
        {
            get { return showCheckBox; }
            set { showCheckBox = value; }
        }

        private bool showNodeIco = true;
        public bool ShowNodeIco
        {
            get
            {
                return showNodeIco;
            }
            set
            {
                showNodeIco = value;
            }
        }

        private string nodedblclick = "onNodeDblclick";
        public string Nodedblclick
        {
            get
            {
                return nodedblclick;
            }
            set
            {
                nodedblclick = value;
            }
        }

        private bool enableOndblclick = false;
        public bool EnableOndblclick
        {
            get
            {
                return enableOndblclick;
            }
            set
            {
                enableOndblclick = value;
            }
        }

        #endregion

        #region Behiver Properties
        private bool enableDrag = false;
        public bool EnableDrag
        {
            get
            {
                return enableDrag;
            }
            set
            {
                enableDrag = value;
            }
        }

        private bool isAjaxLoad = false;
        public bool IsAjaxLoad
        {
            get
            {
                return isAjaxLoad;
            }
            set
            {
                isAjaxLoad = value;
            }
        }

        private TreeViewPostType postType = TreeViewPostType.None;
        public TreeViewPostType PostType
        {
            get
            {
                return postType;
            }
            set
            {
                postType = value;
            }
        }

        private string currentNodeId = string.Empty;
        public string CurrentNodeId
        {
            get
            {
                if (string.IsNullOrEmpty(currentNodeId))
                    currentNodeId = Page.Request.Form[ID + "CurrentNodeId"];

                return currentNodeId;
            }
            set
            {
                currentNodeId = value;
            }
        }


        private TreeViewSelectionMode selectionMode = TreeViewSelectionMode.RelatedMultiple;
        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                return selectionMode;
            }
            set
            {
                selectionMode = value;
            }
        }

        #endregion

        public string Text
        { get; set; }

        private int runat = 0;
        public int Runat
        {
            get
            {
                return runat;
            }
            set
            {
                runat = value;
            }
        }

        private string onNodeClick = "treeViewNodeAction";
        public string OnNodeClick
        {
            get
            {
                return onNodeClick;
            }
            set
            {
                onNodeClick = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitWebResource();
        }

        private void InitScript()
        {
            //StringBuilder script = new StringBuilder();
            //script.AppendFormat("var treeViewSetting=new Object();\n", treeView_bottom_line);
            //script.AppendFormat("treeViewSetting.treeView_bottom_line=\"{0}\";\n", treeView_bottom_line);
            //script.AppendFormat("treeViewSetting.treeView_middle_line=\"{0}\";\n", treeView_middle_line);
            //script.AppendFormat("treeViewSetting.treeView_root_line=\"{0}\";\n", treeView_root_line);
            //script.AppendFormat("treeViewSetting.treeView_top_line=\"{0}\";\n", treeView_top_line);
            //script.AppendFormat("treeViewSetting.treeView_vertical_line=\"{0}\";\n", treeView_vertical_line);
            //script.AppendFormat("treeViewSetting.treeView_folder=\"{0}\";\n", treeView_folder);
            //script.AppendFormat("treeViewSetting.treeView_folder_open=\"{0}\";\n", treeView_folder_open);
            //script.AppendFormat("treeViewSetting.treeView_minus=\"{0}\";\n", treeView_minus);
            //script.AppendFormat("treeViewSetting.treeView_node=\"{0}\";\n", treeView_node);
            //script.AppendFormat("treeViewSetting.treeView_plus=\"{0}\";\n", treeView_plus);
            //script.AppendFormat("treeViewSetting.checkbox_checked=\"{0}\";\n", checkbox_checked);
            //script.AppendFormat("treeViewSetting.checkbox_half_checked=\"{0}\";\n", checkbox_half_checked);
            //script.AppendFormat("treeViewSetting.checkbox_unchecked=\"{0}\";\n", checkbox_unchecked);
            //script.AppendFormat("treeViewSetting.context_menu_gradient=\"{0}\";\n", context_menu_gradient);
            //script.AppendFormat("treeViewSetting.drag_drop_ind1=\"{0}\";\n", drag_drop_ind1);
            //script.AppendFormat("treeViewSetting.drag_drop_ind2=\"{0}\";\n", drag_drop_ind2);
            //script.AppendFormat("treeViewSetting.folder_close=\"{0}\";\n", folder_close);
            //script.AppendFormat("treeViewSetting.folder_dots=\"{0}\";\n", folder_dots);
            //script.AppendFormat("treeViewSetting.folder_folder=\"{0}\";\n", folder_folder);
            //script.AppendFormat("treeViewSetting.folder_lastsub=\"{0}\";\n", folder_lastsub);
            //script.AppendFormat("treeViewSetting.folder_open=\"{0}\";\n", folder_open);
            //script.AppendFormat("treeViewSetting.folder_sub=\"{0}\";\n", folder_sub);
            //script.AppendFormat("treeViewSetting.treeView_onLoad=\"{0}\";\n", treeView_onLoad);
            //script.AppendFormat("treeViewSetting.enableDrag={0};\n", enableDrag.ToString().ToLower());
            //script.AppendFormat("treeViewSetting.selectionMode=\"{0}\";\n", selectionMode.ToString());
            //script.AppendFormat("treeViewSetting.treeView_drag_disable=\"{0}\";\n", treeView_drag_disable.ToString());
            //script.AppendFormat("treeViewSetting.treeView_drag_enable=\"{0}\";\n", treeView_drag_enable.ToString());

            StringBuilder options = new StringBuilder();
            options.Append("{");
            options.AppendFormat("id:\"{0}\",", ID);
            options.AppendFormat("selectionMode:\"{0}\",", selectionMode);
            options.AppendFormat("ajaxLoading:{0},", isAjaxLoad ? "true" : "false");
            options.AppendFormat("enableDrag:{0},", enableDrag ? "true" : "false");
            options.AppendFormat("nodeClick:\"{0}\",", OnNodeClick);
            options.AppendFormat("treeView_root_line:\"{0}\",", treeView_root_line);
            options.AppendFormat("treeView_folder:\"{0}\",", treeView_folder);
            options.AppendFormat("treeView_node:\"{0}\",", treeView_node);
            options.AppendFormat("treeView_bottom_line:\"{0}\",", treeView_bottom_line);
            options.AppendFormat("treeView_middle_line:\"{0}\",", treeView_middle_line);
            options.AppendFormat("treeView_vertical_line:\"{0}\",", treeView_vertical_line);
            options.AppendFormat("treeView_minus:\"{0}\",", treeView_minus);
            options.AppendFormat("treeView_plus:\"{0}\",", treeView_plus);
            options.AppendFormat("treeView_top_line:\"{0}\",", treeView_top_line);
            options.AppendFormat("treeView_onLoad:\"{0}\"", treeView_onLoad);
            options.Append("}");
            //script.AppendFormat("var {0}=new TreeView(\"{0}\",{1});", ID, options.ToString());
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("treeViewSetting"))
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "treeViewSetting", script.ToString(), true);

            Page.Response.Write("<script type=\"text/javascript\">");
            Page.Response.Write(string.Format("var {0}=new TreeView(\"{0}\",{1});", ID, options.ToString()));
            Page.Response.Write("</script>");
        }

        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "TreeView.Resources.TreeView.js"))
            {

                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.TreeView.js");
                clientManager.RegisterClientScriptInclude(type, "TreeView.Resources.TreeView.js", url);
            }

            treeView_bottom_line = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-bottom-line.gif");
            treeView_middle_line = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-middle-line.gif");
            treeView_root_line = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-root-line.gif");
            treeView_top_line = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-top-line.gif");
            treeView_vertical_line = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-vertical-line.gif");
            treeView_folder = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView_folder.gif");
            treeView_folder_open = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView_folder_open.gif");
            treeView_minus = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView_minus.gif");
            treeView_node = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView_node.gif");
            treeView_plus = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView_plus.gif");
            checkbox_checked = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.checkbox-checked.gif");
            checkbox_half_checked = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.checkbox-half-checked.gif");
            checkbox_unchecked = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.checkbox-unchecked.gif");
            context_menu_gradient = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.context-menu-gradient.gif");
            drag_drop_ind1 = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.dragDrop_ind1.gif");
            drag_drop_ind2 = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.dragDrop_ind2.gif");
            folder_close = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_close.gif");
            folder_dots = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_dots.gif");
            folder_folder = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_folder.gif");
            folder_lastsub = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_lastsub.gif");
            folder_open = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_open.gif");
            folder_sub = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.folder_sub.gif");
            treeView_onLoad = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-onLoad.gif");

            if (EnableDrag)
            {
                string dragURL = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.TreeViewDrag.js");
                clientManager.RegisterClientScriptInclude(type, "TreeView.Resources.TreeViewDrag.js", dragURL);

                treeView_drag_enable = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-drag-enable.gif");
                treeView_drag_disable = clientManager.GetWebResourceUrl(type, "AgileEAP.TreeView.TreeView.Resources.treeView-drag-disable.gif");
            }

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(string.Format("<div>TreeView-{0}<div>", ID));
                return;
            }

            InitScript();
            string cssFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.TreeView.TreeView.Resources.TreeView.css");
            writer.Write(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssFile));

            writer.AddAttribute("id", string.Format("{0}_TreeViewContainer", ID));
            writer.AddAttribute("class", ContainerCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (IsAjaxLoad)
            {
                writer.AddAttribute("id", ID + "PromptMessage");
                writer.AddAttribute("class", ID + "PromptMessage");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }

            if (EnableDrag)
            {
                writer.AddAttribute("id", ID + "DragContainer");
                writer.AddAttribute("class", DragContainerCssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }

            HiddenField hidCurrentNodeId = new HiddenField();
            hidCurrentNodeId.ID = ID + "CurrentNodeId";
            hidCurrentNodeId.Value = CurrentNodeId;
            hidCurrentNodeId.RenderControl(writer);

            writer.AddAttribute("id", ID);
            writer.AddAttribute("class", TreeCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (Nodes != null && Nodes.Count > 0)
            {
                Nodes.Owner = this;
                Nodes.Render(writer, 0);
            }
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}
