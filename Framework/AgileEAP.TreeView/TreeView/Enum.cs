using System;
using System.Collections.Generic;
using System.Text;

namespace AgileEAP.WebControls
{
    public enum TreeViewNodeType
    {
        LeafNode = 0,
        ParentNode = 1
    }

    public enum TreeViewPostType
    {
        NoPost = 0,

        AjaxPost = 1,

        Post = 2,

        None = 3
    }

    public enum TreeViewSkin
    {
        Defalut = 0,
        Foler = 1
    }

    public enum TreeViewNodeState
    {
        Close = 0,
        Open = 1
    }

    public enum TreeViewSelectionMode
    {
        /// <summary>
        /// 单选
        /// </summary>
        Single = 0,

        /// <summary>
        /// 多选
        /// </summary>
        Multiple = 1,

        /// <summary>
        /// 级联多选
        /// </summary>
        RelatedMultiple = 2,

        /// <summary>
        /// 叶子结点单选
        /// </summary>
        LeafSingle = 3,

        /// <summary>
        /// 叶子结点多选，不向上级联
        /// </summary>
        LeafMultiple = 4
    }
}