using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace EAFrame.WebControls
{
    /// <summary>
    /// �˵���༭��
    /// </summary>
    public class MenuItemGroupEditor : CollectionEditor
    {
        public MenuItemGroupEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// �˵����Ƿ���Զ�ѡ
        /// </summary>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// ���ؼ�����������
        /// </summary>
        /// <returns></returns>
        protected override Type CreateCollectionItemType()
        {
            return typeof(MenuItemGroup);
        }
    }
}
