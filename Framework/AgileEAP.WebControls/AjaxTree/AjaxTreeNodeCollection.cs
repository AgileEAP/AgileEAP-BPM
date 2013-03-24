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
    ToolboxData("<{0}:AjaxTreeNode runat=server></{0}:AjaxTreeNode>")]
    public class AjaxTreeNodeCollection : IList<AjaxTreeNode>
    {
        private IList<AjaxTreeNode> innerNodes = new List<AjaxTreeNode>();
        /// <summary>
        /// 
        /// </summary>
        public IList<AjaxTreeNode> InnerNodes
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

        #region IList<AjaxTreeNode> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(AjaxTreeNode item)
        {
            return innerNodes.IndexOf(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, AjaxTreeNode item)
        {
            innerNodes.Insert(index, item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            innerNodes.RemoveAt(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AjaxTreeNode this[int index]
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

        #region ICollection<AjaxTreeNode> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(AjaxTreeNode item)
        {
            innerNodes.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            innerNodes.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(AjaxTreeNode item)
        {
            return innerNodes.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(AjaxTreeNode[] array, int arrayIndex)
        {
            innerNodes.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return innerNodes.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return innerNodes.IsReadOnly; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(AjaxTreeNode item)
        {
            return innerNodes.Remove(item);
        }

        #endregion

        #region IEnumerable<AjaxTreeNode> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<AjaxTreeNode> GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        #endregion

        #region Render Control
        /// <summary>
        /// 
        /// </summary>
        public AjaxTreeNode ParentNode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public AjaxTree Owner
        {
            get;
            set;
        }

        private NodeState nodesSate = NodeState.Close;
        /// <summary>
        /// 
        /// </summary>
        public NodeState NodesState
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string GetCssClass(int depth)
        {
            string cssClass = "line-vertical";

            if (depth == 0 && ParentNode != null && ParentNode.IsLast) cssClass = "line-none";

            return cssClass;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string GetStyle(int depth)
        {
            string style = "background: url(" + Owner.AjaxTree_vertical_line + ") repeat-y 0px 0px;";

            if (depth == 0 && ParentNode != null && ParentNode.IsLast) style = "";

            return style;

        }

        internal void Render(HtmlTextWriter writer, int depth)
        {
            if (Count > 0)
            {
                if (depth > 0 && NodesState != NodeState.Open) writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                string style = (depth == 0 || (ParentNode != null && ParentNode.IsLast)) ? "" : "background: url(" + Owner.AjaxTree_vertical_line + ") repeat-y 0px 0px;";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="depth"></param>
        /// <param name="parentIsLast"></param>
        public void AjaxRender(HtmlTextWriter writer, int depth, bool parentIsLast)
        {
            if (ParentNode == null)
            {
                ParentNode = new AjaxTreeNode();
                ParentNode.IsLast = parentIsLast;
            }

            Render(writer, depth);
        }
        #endregion
    }
}
