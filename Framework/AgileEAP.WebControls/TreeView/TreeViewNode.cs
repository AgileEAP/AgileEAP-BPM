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
    /// <summary>
    /// 
    /// </summary>
    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Nodes"),
    ToolboxData("<{0}:TreeViewNode runat=server></{0}:TreeViewNode>")]
    public class TreeViewNode : WebControl
    {
        #region Style

        private Image imgIco = new Image();
        private bool isInitIcoSrc = false;
        private string treeView_bottom_line = string.Empty;
        private string treeView_middle_line = string.Empty;
        private string treeView_root_line = string.Empty;
        private string treeView_top_line = string.Empty;
        private string treeView_vertical_line = string.Empty;
        private string treeView_folder = string.Empty;
        private string treeView_folder_open = string.Empty;
        private string treeView_minus = string.Empty;
        private string treeView_node = string.Empty;
        private string treeView_plus = string.Empty;
        private string checkbox_checked = string.Empty;
        private string checkbox_half_checked = string.Empty;
        private string checkbox_unchecked = string.Empty;
        private string context_menu_gradient = string.Empty;
        private string drag_drop_ind1 = string.Empty;
        private string drag_drop_ind2 = string.Empty;
        private string folder_close = string.Empty;
        private string folder_dots = string.Empty;
        private string folder_folder = string.Empty;
        private string folder_lastsub = string.Empty;
        private string folder_open = string.Empty;
        private string folder_sub = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Text
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tag
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Extend
        {
            get;
            set;
        }

        private string cssClass = "treeView-leafNode";
        /// <summary>
        /// 
        /// </summary>
        public new string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        private string cssStyle = "";
        /// <summary>
        /// 
        /// </summary>
        public string CssStyle
        {
            get { return cssStyle; }
            set { cssStyle = value; }
        }

        private TreeView onwer = null;
        /// <summary>
        /// 
        /// </summary>
        public TreeView Owner
        {
            get
            {
                return onwer;
            }
            set
            {
                onwer = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeViewNode ParentNode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Depth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Runat
        { get; set; }


        private TreeViewNodeCollection childNodes = new TreeViewNodeCollection();
        /// <summary>
        /// 
        /// </summary>
        public TreeViewNodeCollection ChildNodes
        {
            get
            {
                return childNodes;
            }
            set
            {
                childNodes = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowNodeIco
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeViewNodeType NodeType
        { get; set; }

        private string linkUrl = "javascript:void(0)";
        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl
        {
            get
            {
                return linkUrl;
            }
            set
            {
                linkUrl = value;
            }
        }

        private string target = "_self";
        /// <summary>
        /// 
        /// </summary>
        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? ShowCheckBox
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? EnableDrag
        {
            get;
            set;
        }

        private TreeViewNodeState nodeState = TreeViewNodeState.Close;
        /// <summary>
        /// 
        /// </summary>
        public TreeViewNodeState NodeState
        {
            get
            {
                return nodeState;
            }
            set
            {
                nodeState = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Checked
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IcoSrc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string NodeIcoSrc
        {
            get;
            set;
        }

        private string id = null;
        /// <summary>
        /// 
        /// </summary>
        public new string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString();

                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParentNodeIcon
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string LeafNodeIcon
        {
            get;
            set;
        }

        private bool isLast = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsLast
        {
            get
            {
                return isLast;
            }
            set
            {
                isLast = value;
            }
        }

        private int virtualNodeCount = 0;
        /// <summary>
        /// 
        /// </summary>
        public int VirtualNodeCount
        {
            get
            {
                return virtualNodeCount;
            }
            set
            {
                virtualNodeCount = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return ChildNodes.Count > 0 || virtualNodeCount > 0;
            }
        }
        #endregion

        #region Behivor
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

        private TreeViewPostType postType = TreeViewPostType.NoPost;
        /// <summary>
        /// 
        /// </summary>
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
        #endregion

        private void InitIcoSrc()
        {
            if (Owner == null) throw new Exception("Node's owner is null!");

            treeView_bottom_line = Owner.TreeView_bottom_line;
            treeView_middle_line = Owner.TreeView_middle_line;
            treeView_root_line = Owner.TreeView_root_line;
            treeView_top_line = Owner.TreeView_top_line;
            treeView_vertical_line = Owner.TreeView_vertical_line;
            treeView_folder = Owner.ParentNodeIcon;
            treeView_folder_open = Owner.ParentNode_Open;
            treeView_minus = Owner.TreeView_minus;
            treeView_node = Owner.LeafNodeIcon;
            treeView_plus = Owner.TreeView_plus;
            checkbox_checked = Owner.Checkbox_checked;
            checkbox_half_checked = Owner.Checkbox_half_checked;
            checkbox_unchecked = Owner.Checkbox_unchecked;
            context_menu_gradient = Owner.Context_menu_gradient;
            drag_drop_ind1 = Owner.Drag_drop_ind1;
            drag_drop_ind2 = Owner.Drag_drop_ind2;
            folder_close = Owner.Folder_close;
            folder_dots = Owner.Folder_dots;
            folder_folder = Owner.Folder_folder;
            folder_lastsub = Owner.Folder_lastsub;
            folder_open = Owner.Folder_open;
            folder_sub = Owner.Folder_sub;
            ShowNodeIco = Owner.ShowNodeIco;

            postType = Owner.PostType;
            isAjaxLoad = Owner.IsAjaxLoad;
            isInitIcoSrc = true;
        }

        //private string GetNodeIcoSrc(int index, int brotherCount)
        //{
        //    if (!isInitIcoSrc)
        //        InitIcoSrc();

        //    string icoSrc = string.Empty;
        //    cssClass = "line-middle";
        //    if (Depth == 0 && index == 0)
        //    {
        //        icoSrc = HasChildren ? treeView_plus : treeView_root_line;
        //        cssClass = "line-root";
        //    }
        //    else if (Depth == 0 && index == brotherCount)
        //    {
        //        cssClass = "line-bottom";
        //        icoSrc = HasChildren ? treeView_plus : treeView_bottom_line;
        //    }
        //    else if (Depth == 0)
        //    {
        //        icoSrc = HasChildren ? treeView_plus : treeView_minus;
        //    }
        //    else if (index == 0 && index != brotherCount)
        //    {
        //        cssClass = string.Empty;// cssClass = "line-middle";
        //        icoSrc = HasChildren ? treeView_plus : treeView_middle_line;
        //    }

        //    else if (index == brotherCount)
        //    {
        //        icoSrc = HasChildren ? treeView_plus : treeView_bottom_line;
        //        cssClass = "line-bottom";
        //    }
        //    else
        //    {
        //        icoSrc = HasChildren ? treeView_plus : treeView_middle_line;
        //    }

        //    return icoSrc;
        //}

        private string GetNodeIcoSrc(int index, int brotherCount)
        {
            if (!isInitIcoSrc)
                InitIcoSrc();

            string icoSrc = string.Empty;
            CssStyle = "background: url(" + Owner.TreeView_middle_line + ") white no-repeat;";
            if (Depth == 0 && index == 0)
            {
                icoSrc = HasChildren ? treeView_plus : treeView_root_line;
                CssStyle = string.Format(" background: url({0}) white no-repeat;_background: url({0}) white no-repeat 0px 3px;", brotherCount > 0 ? treeView_top_line : treeView_root_line);
            }
            else if (Depth == 0 && index == brotherCount)
            {
                CssStyle = "background: url(" + Owner.TreeView_bottom_line + ") white no-repeat;";
                icoSrc = HasChildren ? treeView_plus : treeView_bottom_line;
            }
            else if (Depth == 0)
            {
                icoSrc = HasChildren ? treeView_plus : treeView_minus;
            }
            else if (index == 0 && index != brotherCount)
            {
                //  CssStyle = string.Empty;
                icoSrc = HasChildren ? treeView_plus : treeView_middle_line;
            }

            else if (index == brotherCount)
            {
                icoSrc = HasChildren ? treeView_plus : treeView_bottom_line;
                CssStyle = "background: url(" + Owner.TreeView_bottom_line + ") white no-repeat;";
            }
            else
            {
                icoSrc = HasChildren ? treeView_plus : treeView_middle_line;
            }

            if (HasChildren && ChildNodes != null && NodeState == TreeViewNodeState.Open)
                icoSrc = treeView_minus;

            return icoSrc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetIcoSrc()
        {
            if (!isInitIcoSrc)
                InitIcoSrc();

            return HasChildren ? ParentNodeIcon ?? treeView_folder : LeafNodeIcon ?? treeView_node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="index"></param>
        /// <param name="brotherCount"></param>
        public void RenderControl(HtmlTextWriter writer, int index, int brotherCount)
        {
            isLast = index == brotherCount;

            if (string.IsNullOrEmpty(NodeIcoSrc))
                NodeIcoSrc = GetNodeIcoSrc(index, brotherCount);

            if (string.IsNullOrEmpty(IcoSrc))
                IcoSrc = GetIcoSrc();

            string state = "toggleNode";
            if (isAjaxLoad && virtualNodeCount > 0)
            {
                state = "ajaxLoad";
            }

            writer.AddAttribute("id", ID);
            //writer.AddAttribute("class", CssClass);
            writer.AddAttribute("title", string.IsNullOrEmpty(ToolTip) ? Text : ToolTip);
            writer.AddAttribute("text", Text);
            writer.AddAttribute("bindvalue", Value);
            writer.AddAttribute("style", CssStyle);
            writer.AddAttribute("nodeState", state);
            writer.AddAttribute("virtualNodeCount", childNodes != null && childNodes.Count > 0 ? childNodes.Count.ToString() : VirtualNodeCount.ToString());
            writer.AddAttribute("isLast", isLast ? "true" : "false");
            writer.AddAttribute("postType", postType.ToString());
            if (!string.IsNullOrEmpty(Extend))
                writer.AddAttribute("extend", Extend);
            if (!string.IsNullOrEmpty(Tag))
                writer.AddAttribute("tag", Tag);

            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            if (!string.IsNullOrEmpty(NodeIcoSrc))
            {
                if (HasChildren)
                {
                    // imgIco.Attributes.Add("onclick", string.Format("onNodeIcoClick('{0}')", ID));
                    imgIco.Attributes.Add("nodeIcoState", state);
                }
                else if (Depth == 0 && !HasChildren && index != brotherCount)
                {
                    imgIco.Style.Add("visibility", "hidden");
                }

                imgIco.ImageUrl = NodeIcoSrc;
                imgIco.RenderControl(writer);
            }
            if (ShowCheckBox ?? Owner.ShowCheckBox)
            {
                writer.AddAttribute("id", string.Format("chk_{0}", ID));
                // writer.AddAttribute("onclick", string.Format("onTreeNodeChecked(this,'{0}','{1}')", ID, Owner.SelectionMode));

                if (Checked)
                    writer.AddAttribute("checked", "true");
                writer.AddAttribute("type", "checkbox");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }

            if (ShowNodeIco && !string.IsNullOrEmpty(IcoSrc))
            {
                imgIco = new Image();
                imgIco.ImageUrl = IcoSrc;
                imgIco.RenderControl(writer);
            }

            writer.AddAttribute("href", LinkUrl);
            writer.AddAttribute("target", Target);
            //writer.AddAttribute("onclick", string.Format("{0}('{1}','{2}')", Action, ID, Value));
            //if (Owner.EnableOndblclick)
            //    writer.AddAttribute("ondblclick", string.Format("{0}('{1}','{2}')", onwer.Nodedblclick, ID, Value));

            if (EnableDrag ?? onwer.EnableDrag)
                writer.AddAttribute("dragNode", onwer.EnableDrag.ToString().ToLower());
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(Text);
            writer.RenderEndTag();

            if ((!isAjaxLoad || NodeState == TreeViewNodeState.Open) && HasChildren)
            {
                ChildNodes.ParentNode = this;
                ChildNodes.Owner = Owner;
                ChildNodes.NodesState = NodeState;
                ChildNodes.Render(writer, Depth + 1);
            }

            writer.RenderEndTag();
        }

    }
}
