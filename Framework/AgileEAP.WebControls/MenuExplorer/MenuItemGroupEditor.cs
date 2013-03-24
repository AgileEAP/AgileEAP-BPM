using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace EAFrame.WebControls
{
    /// <summary>
    /// 菜单组编辑器
    /// </summary>
    public class MenuItemGroupEditor : CollectionEditor
    {
        public MenuItemGroupEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// 菜单组是否可以多选
        /// </summary>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// 返回集合子项类型
        /// </summary>
        /// <returns></returns>
        protected override Type CreateCollectionItemType()
        {
            return typeof(MenuItemGroup);
        }
    }
}
