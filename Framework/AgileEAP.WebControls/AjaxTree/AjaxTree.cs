//************************************************************
//项目:WebControls　　　　　　　　 
//版权:Copyright(c) 2008,谭任辉 　　　　　　　　　　　　　
//模块名:AjaxTree
//说明:异步加载树
//版本：1.0
//修改历史：
//			V1.0
//说明：
//备注：完成
//	2009-9-15 by 谭任辉
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
using System.Drawing.Design;
using System.Security.Permissions;

namespace AgileEAP.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Nodes"),
    ToolboxData("<{0}:AjaxTree runat=server></{0}:AjaxTree>")]
    public class AjaxTree : CompositeControl
    {
        #region Style Properties

        private string ajaxTree_bottom_line = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_bottom_line
        {
            get
            {
                return ajaxTree_bottom_line;
            }
            set
            {
                ajaxTree_bottom_line = value;
            }
        }

        private string ajaxTree_middle_line = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_middle_line
        {
            get
            {
                return ajaxTree_middle_line;
            }
            set
            {
                ajaxTree_middle_line = value;
            }
        }

        private string ajaxTree_root_line = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_root_line
        {
            get
            {
                return ajaxTree_root_line;
            }
            set
            {
                ajaxTree_root_line = value;
            }
        }

        private string ajaxTree_top_line = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_top_line
        {
            get
            {
                return ajaxTree_top_line;
            }
            set
            {
                ajaxTree_top_line = value;
            }
        }

        private string ajaxTree_vertical_line = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_vertical_line
        {
            get
            {
                return ajaxTree_vertical_line;
            }
            set
            {
                ajaxTree_vertical_line = value;
            }
        }

        private string ajaxTree_folder = string.Empty;
        private string parentNodeIcon = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string ParentNodeIcon
        {
            get
            {
                return string.IsNullOrEmpty(parentNodeIcon) ? ajaxTree_folder : parentNodeIcon;
            }
            set
            {
                parentNodeIcon = value;
            }
        }

        private string ajaxTree_folder_open = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string ParentNode_Open
        {
            get
            {
                return ajaxTree_folder_open;
            }
            set
            {
                ajaxTree_folder_open = value;
            }
        }

        private string ajaxTree_minus = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_minus
        {
            get
            {
                return ajaxTree_minus;
            }
            set
            {
                ajaxTree_minus = value;
            }
        }

        private string ajaxTree_node = string.Empty;
        private string leafNodeIcon = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string LeafNodeIcon
        {
            get
            {
                return string.IsNullOrEmpty(leafNodeIcon) ? ajaxTree_node : leafNodeIcon;
            }
            set
            {
                leafNodeIcon = value;
            }
        }

        private string ajaxTree_plus = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_plus
        {
            get
            {
                return ajaxTree_plus;
            }
            set
            {
                ajaxTree_plus = value;
            }
        }

        private string checkbox_checked = string.Empty;
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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

        private string ajaxTree_onLoad = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_onload
        {
            get
            {
                return ajaxTree_onLoad;
            }
            set
            {
                ajaxTree_onLoad = value;
            }
        }

        private string ajaxTree_drag_enable = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_drag_enable
        {
            get
            {
                return ajaxTree_drag_enable;
            }
            set
            {
                ajaxTree_drag_enable = value;
            }
        }

        private string ajaxTree_drag_disable = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTree_drag_disable
        {
            get
            {
                return ajaxTree_drag_disable;
            }
            set
            {
                ajaxTree_drag_disable = value;
            }
        }

        private AjaxTreeNodeCollection nodes = new AjaxTreeNodeCollection();
        /// <summary>
        /// 
        /// </summary>
        public AjaxTreeNodeCollection Nodes
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

        private string containerCssClass = "ajaxTree-container";
        /// <summary>
        /// 
        /// </summary>
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

        private string treeCssClass = "ajaxTree-tree";
        /// <summary>
        /// 
        /// </summary>
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

        private string parentNodeCssClass = "ajaxTree-parentNode";
        /// <summary>
        /// 
        /// </summary>
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

        private string dragContainerCssClass = "ajaxTree-drag-container";
        /// <summary>
        /// 
        /// </summary>
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

        private string leafNodeCssClass = "ajaxTree-leafNode";
        /// <summary>
        /// 
        /// </summary>
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

        private bool showCheckBox = true;
        /// <summary>
        /// 
        /// </summary>
        public bool ShowCheckBox
        {
            get { return showCheckBox; }
            set { showCheckBox = value; }
        }

        private bool showNodeIco = true;
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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

        private PostType postType = PostType.None;
        /// <summary>
        /// 
        /// </summary>
        public PostType PostType
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

        private string selectNodeIds = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string[] SelectNodeIds
        {
            get
            {
                if (string.IsNullOrEmpty(selectNodeIds))
                    selectNodeIds = Page.Request.Form["ajaxTreeSelectNodeIds"];
                return string.IsNullOrEmpty(selectNodeIds) ? null : selectNodeIds.Trim(',').Split(',');
            }
            set
            {
                selectNodeIds = string.Join(",", value);
            }
        }

        private string currentNodeId = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string CurrentNodeId
        {
            get
            {
                if (string.IsNullOrEmpty(currentNodeId))
                    currentNodeId = Page.Request.Form["ajaxTreeCurrentNodeId"];

                return currentNodeId;
            }
            set
            {
                currentNodeId = value;
            }
        }


        private string ajaxTreeTag = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AjaxTreeTag
        {
            get
            {
                if (string.IsNullOrEmpty(currentNodeId))
                    ajaxTreeTag = Page.Request.Form["ajaxTreeTag"];
                return ajaxTreeTag;
            }
            set
            {
                ajaxTreeTag = value;
            }
        }

        private SelectionMode selectionMode = SelectionMode.RelatedMultiple;
        /// <summary>
        /// 
        /// </summary>
        public SelectionMode SelectionMode
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

        /// <summary>
        /// 
        /// </summary>
        public string Text
        { get; set; }

        private int runat = 0;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitWebResource();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("ajaxTreeSetting.ajaxTree_bottom_line=\"{0}\";\n", ajaxTree_bottom_line);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_middle_line=\"{0}\";\n", ajaxTree_middle_line);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_root_line=\"{0}\";\n", ajaxTree_root_line);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_top_line=\"{0}\";\n", ajaxTree_top_line);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_vertical_line=\"{0}\";\n", ajaxTree_vertical_line);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_folder=\"{0}\";\n", ajaxTree_folder);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_folder_open=\"{0}\";\n", ajaxTree_folder_open);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_minus=\"{0}\";\n", ajaxTree_minus);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_node=\"{0}\";\n", ajaxTree_node);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_plus=\"{0}\";\n", ajaxTree_plus);
            script.AppendFormat("ajaxTreeSetting.checkbox_checked=\"{0}\";\n", checkbox_checked);
            script.AppendFormat("ajaxTreeSetting.checkbox_half_checked=\"{0}\";\n", checkbox_half_checked);
            script.AppendFormat("ajaxTreeSetting.checkbox_unchecked=\"{0}\";\n", checkbox_unchecked);
            script.AppendFormat("ajaxTreeSetting.context_menu_gradient=\"{0}\";\n", context_menu_gradient);
            script.AppendFormat("ajaxTreeSetting.drag_drop_ind1=\"{0}\";\n", drag_drop_ind1);
            script.AppendFormat("ajaxTreeSetting.drag_drop_ind2=\"{0}\";\n", drag_drop_ind2);
            script.AppendFormat("ajaxTreeSetting.folder_close=\"{0}\";\n", folder_close);
            script.AppendFormat("ajaxTreeSetting.folder_dots=\"{0}\";\n", folder_dots);
            script.AppendFormat("ajaxTreeSetting.folder_folder=\"{0}\";\n", folder_folder);
            script.AppendFormat("ajaxTreeSetting.folder_lastsub=\"{0}\";\n", folder_lastsub);
            script.AppendFormat("ajaxTreeSetting.folder_open=\"{0}\";\n", folder_open);
            script.AppendFormat("ajaxTreeSetting.folder_sub=\"{0}\";\n", folder_sub);
            script.AppendFormat("ajaxTreeSetting.ajaxTree_onLoad=\"{0}\";\n", ajaxTree_onLoad);
            script.AppendFormat("ajaxTreeSetting.enableDrag={0};\n", enableDrag.ToString().ToLower());
            script.AppendFormat("ajaxTreeSetting.selectionMode=\"{0}\";\n", selectionMode.ToString());
            script.AppendFormat("ajaxTreeSetting.ajaxTree_drag_disable=\"{0}\";\n", ajaxTree_drag_disable.ToString());
            script.AppendFormat("ajaxTreeSetting.ajaxTree_drag_enable=\"{0}\";\n", ajaxTree_drag_enable.ToString());


            //if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxTreeSetting"))
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ajaxTreeSetting", script.ToString(), true);

            Page.Response.Write("<script type=\"text/javascript\">");
            Page.Response.Write(script.ToString());
            Page.Response.Write("</script>");
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "AjaxTree.Resources.AjaxTree.js"))
            {
                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.AjaxTree.js");
                clientManager.RegisterClientScriptInclude(type, "AjaxTree.Resources.AjaxTree.js", url);

                if (EnableDrag)
                {
                    url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.AjaxTreeDrag.js");
                    clientManager.RegisterClientScriptInclude(type, "AjaxTree.Resources.AjaxTreeDrag.js", url);
                }
            }

            ajaxTree_bottom_line = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-bottom-line.gif");
            ajaxTree_middle_line = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-middle-line.gif");
            ajaxTree_root_line = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-root-line.gif");
            ajaxTree_top_line = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-top-line.gif");
            ajaxTree_vertical_line = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-vertical-line.gif");
            ajaxTree_folder = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree_folder.gif");
            ajaxTree_folder_open = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree_folder_open.gif");
            ajaxTree_minus = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree_minus.gif");
            ajaxTree_node = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree_node.gif");
            ajaxTree_plus = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree_plus.gif");
            checkbox_checked = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.checkbox-checked.gif");
            checkbox_half_checked = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.checkbox-half-checked.gif");
            checkbox_unchecked = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.checkbox-unchecked.gif");
            context_menu_gradient = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.context-menu-gradient.gif");
            drag_drop_ind1 = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.dragDrop_ind1.gif");
            drag_drop_ind2 = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.dragDrop_ind2.gif");
            folder_close = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_close.gif");
            folder_dots = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_dots.gif");
            folder_folder = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_folder.gif");
            folder_lastsub = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_lastsub.gif");
            folder_open = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_open.gif");
            folder_sub = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.folder_sub.gif");
            ajaxTree_onLoad = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-onLoad.gif");
            ajaxTree_drag_enable = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-drag-enable.gif");
            ajaxTree_drag_disable = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.AjaxTree.Resources.ajaxTree-drag-disable.gif");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(string.Format("<div>AjaxTree-{0}<div>", ID));
                return;
            }

            InitScript();
            string cssFile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.AjaxTree.Resources.AjaxTree.css");
            writer.Write(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssFile));

            writer.AddAttribute("id", string.Format("{0}_AjaxTreeContainer", ID));
            writer.AddAttribute("class", ContainerCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (IsAjaxLoad)
            {
                writer.AddAttribute("id", "ajaxTreePromptMessage");
                writer.AddAttribute("class", "ajaxTreePromptMessage");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }

            if (EnableDrag)
            {
                writer.AddAttribute("id", "ajaxTreeDragContainer");
                writer.AddAttribute("class", DragContainerCssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }

            HiddenField hidAjaxTreeTag = new HiddenField();
            hidAjaxTreeTag.ID = "ajaxTreeTag";
            hidAjaxTreeTag.Value = AjaxTreeTag;
            hidAjaxTreeTag.RenderControl(writer);

            HiddenField hidCurrentNodeId = new HiddenField();
            hidCurrentNodeId.ID = "ajaxTreeCurrentNodeId";
            hidCurrentNodeId.Value = CurrentNodeId;
            hidCurrentNodeId.RenderControl(writer);

            writer.AddAttribute("id", "ajaxTree_Div_Id");
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
