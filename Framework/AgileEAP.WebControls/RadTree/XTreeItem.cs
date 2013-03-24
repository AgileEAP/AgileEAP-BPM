
namespace EAFrame.WebControls
{
    /// <summary>
    /// XTree XML数据项
    /// </summary>
    public class XTreeItem
    {
        private string m_Text;
        private string m_Title;
        private string m_ArrModelId;
        private string m_ArrModelName;
        private string m_ArrPurview;
        private string m_NodeId;
        private string m_Target;
        private string m_Expand;
        private string m_Action;
        private string m_Src;
        private string m_AnchorType;
        private string m_Icon;
        private string m_NodeType;
        private string m_Enable;
        private string m_LinkUrl;
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 节点链接的Title元素
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        /// <summary>
        /// 模型ID数组
        /// </summary>
        public string ArrModelId
        {
            get { return m_ArrModelId; }
            set { m_ArrModelId = value; }
        }

        /// <summary>
        /// 模型名称数组
        /// </summary>
        public string ArrModelName
        {
            get { return m_ArrModelName; }
            set { m_ArrModelName = value; }
        }

        /// <summary>
        /// 权限数组
        /// </summary>
        public string ArrPurview
        {
            get { return m_ArrPurview; }
            set { m_ArrPurview = value; }
        }

        /// <summary>
        /// 节点ID
        /// </summary>
        public string NodeId
        {
            get { return m_NodeId; }
            set { m_NodeId = value; }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        /// <summary>
        /// 链接目标
        /// </summary>
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public string Expand
        {
            get { return m_Expand; }
            set { m_Expand = value; }
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        /// <summary>
        /// XML源数据地址
        /// </summary>
        public string XmlSrc
        {
            get { return m_Src; }
            set { m_Src = value; }
        }

        /// <summary>
        /// 处理是否链接，0为空链接，1有链接 无右键 2 有链接 有右键 3 无链接 有右键
        /// </summary>
        public string AnchorType
        {
            get { return m_AnchorType; }
            set { m_AnchorType = value; }
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string Enable
        {
            get { return m_Enable; }
            set { m_Enable = value; }
        }

        /// <summary>
        /// 静态生成方式，
        /// 1为列表文件分目录保存在所属栏目的文件夹中，
        /// 2为列表文件统一保存在指定的“List”文件夹中，
        /// 3为列表文件统一保存在一级栏目文件夹中
        /// </summary>
        public string LinkUrl
        {
            get { return m_LinkUrl; }
            set { m_LinkUrl = value; }
        }
    }
}
