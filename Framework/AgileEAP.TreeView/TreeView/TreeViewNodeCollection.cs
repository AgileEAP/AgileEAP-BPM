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
    ToolboxData("<{0}:TreeViewNode runat=server></{0}:TreeViewNode>")]
    public class TreeViewNodeCollection : IList<TreeViewNode>
    {
        private IList<TreeViewNode> innerNodes = new List<TreeViewNode>();

        public IList<TreeViewNode> InnerNodes
        {
            get
            {
                return innerNodes;
            }
            set
            {
                innerNodes = value;
            }
        }

        #region IList<TreeViewNode> 成员

        public int IndexOf(TreeViewNode item)
        {
            return innerNodes.IndexOf(item);
        }

        public void Insert(int index, TreeViewNode item)
        {
            innerNodes.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            innerNodes.RemoveAt(index);
        }

        public TreeViewNode this[int index]
        {
            get
            {
                return innerNodes[index];
            }
            set
            {
                innerNodes[index] = value;
            }
        }

        #endregion

        #region ICollection<TreeViewNode> 成员

        public void Add(TreeViewNode item)
        {
            innerNodes.Add(item);
        }

        public void Clear()
        {
            innerNodes.Clear();
        }

        public bool Contains(TreeViewNode item)
        {
            return innerNodes.Contains(item);
        }

        public void CopyTo(TreeViewNode[] array, int arrayIndex)
        {
            innerNodes.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return innerNodes.Count; }
        }

        public bool IsReadOnly
        {
            get { return innerNodes.IsReadOnly; }
        }

        public bool Remove(TreeViewNode item)
        {
            return innerNodes.Remove(item);
        }

        #endregion

        #region IEnumerable<TreeViewNode> 成员

        public IEnumerator<TreeViewNode> GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        #endregion

        #region Render Control

        public TreeViewNode ParentNode
        {
            get;
            set;
        }

        public TreeView Owner
        {
            get;
            set;
        }

        private TreeViewNodeState nodesSate = TreeViewNodeState.Close;
        public TreeViewNodeState NodesState
        {
            get
            {
                return nodesSate;
            }
            set
            {
                nodesSate = value;
            }
        }


        public string GetCssClass(int depth)
        {
            string cssClass = "line-vertical";

            if (depth == 0 && ParentNode != null && ParentNode.IsLast) cssClass = "line-none";

            return cssClass;

        }

        public string GetStyle(int depth)
        {
            string style = "background: url(" + Owner.TreeView_vertical_line + ") repeat-y 0px 0px;";

            if (depth == 0 && ParentNode != null && ParentNode.IsLast) style = "";

            return style;

        }

        internal void Render(HtmlTextWriter writer, int depth)
        {
            if (Count > 0)
            {
                if (depth > 0 && NodesState != TreeViewNodeState.Open) writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                string style = (depth == 0 || (ParentNode != null && ParentNode.IsLast)) ? "" : "background: url(" + Owner.TreeView_vertical_line + ") repeat-y 0px 0px;";
                if (!string.IsNullOrEmpty(style))
                    writer.AddAttribute("style", style);
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                for (int i = 0; i < Count; i++)
                {
                    innerNodes[i].Depth = depth;
                    innerNodes[i].ParentNode = ParentNode;
                    innerNodes[i].Owner = Owner;
                    innerNodes[i].RenderControl(writer, i, Count - 1);
                }
                writer.RenderEndTag();
            }
        }

        public void AjaxRender(HtmlTextWriter writer, int depth, bool parentIsLast)
        {
            if (ParentNode == null)
            {
                ParentNode = new TreeViewNode();
                ParentNode.IsLast = parentIsLast;
            }

            Render(writer, depth);
        }
        #endregion
    }
}
