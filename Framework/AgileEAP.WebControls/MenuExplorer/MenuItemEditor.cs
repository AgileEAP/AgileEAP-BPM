using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;


namespace EAFrame.WebControls
{
    /// <summary>
    /// 菜单项编辑器
    /// </summary>
    public class MenuItemEditor : CollectionEditor
    {
        public MenuItemEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// 是否要多选
        /// </summary>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// 返回子项类型
        /// </summary>
        /// <returns></returns>
        protected override Type CreateCollectionItemType()
        {
            return typeof(MenuItem);
        }
    }
}
