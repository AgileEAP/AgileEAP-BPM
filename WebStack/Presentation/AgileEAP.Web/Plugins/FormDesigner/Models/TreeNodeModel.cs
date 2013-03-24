using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.FormDesigner.Models
{
    public class TreeNodeModel
    {
        private IList<TreeNodeModel> _children;
        #region Properties
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public object attr { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IList<TreeNodeModel> children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<TreeNodeModel>();
                }
                return _children;
            }
            set
            {
                _children = value;

            }
        }
        // public bool Checked { get; set; }
        #endregion
    }

    public class TreeNode
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentID { get; set; }
        public string ParentName { get; set; }
        public string Type { get; set; }
        public string Checked { get; set; }
    }
}
