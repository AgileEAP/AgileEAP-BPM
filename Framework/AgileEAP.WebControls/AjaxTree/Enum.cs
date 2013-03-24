using System;
using System.Collections.Generic;
using System.Text;

namespace AgileEAP.WebControls
{

    /// <summary>
    /// 
    /// </summary>
    public enum AjaxTreeNodeType
    {
        /// <summary>
        /// 
        /// </summary>
        LeafNode = 0,
        /// <summary>
        /// 
        /// </summary>
        ParentNode = 1
    }
    /// <summary>
    /// 
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// 
        /// </summary>
        NoPost = 0,
        /// <summary>
        /// 
        /// </summary>
        AjaxPost = 1,
        /// <summary>
        /// 
        /// </summary>
        Post = 2,
        /// <summary>
        /// 
        /// </summary>
        None = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public enum AjaxTreeSkin
    {
        /// <summary>
        /// 
        /// </summary>
        Defalut = 0,
        /// <summary>
        /// 
        /// </summary>
        Foler = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NodeState
    {
        /// <summary>
        /// 
        /// </summary>
        Close = 0,
        /// <summary>
        /// 
        /// </summary>
        Open = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SelectionMode
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