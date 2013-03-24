using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;


namespace EAFrame.WebControls
{
    /// <summary>
    /// �˵���༭��
    /// </summary>
    public class MenuItemEditor : CollectionEditor
    {
        public MenuItemEditor(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// �Ƿ�Ҫ��ѡ
        /// </summary>
        /// <returns></returns>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        protected override Type CreateCollectionItemType()
        {
            return typeof(MenuItem);
        }
    }
}
