
namespace EAFrame.WebControls
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// XTree XML数据列表
    /// </summary>
    public class XTreeCollection : List<XTreeItem>
    {
        /// <summary>
        /// 返回构造XML字符串
        /// </summary>
        /// <returns>构造XML字符串</returns>
        public override string ToString()
        {
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xmlBuilder.Append("<tree>");
            foreach (XTreeItem item in this)
            {
                xmlBuilder.Append("<tree ");
                xmlBuilder.Append("text=\"" + DataSecurity.XmlEncode(item.Text) + "\" ");
                xmlBuilder.Append("title=\"" + DataSecurity.XmlEncode(item.Title) + "\" ");
                xmlBuilder.Append("arrModelId=\"" + DataSecurity.XmlEncode(item.ArrModelId) + "\" ");
                xmlBuilder.Append("arrModelName=\"" + DataSecurity.XmlEncode(item.ArrModelName) + "\" ");
                xmlBuilder.Append("arrPurview=\"" + DataSecurity.XmlEncode(item.ArrPurview) + "\" ");
                xmlBuilder.Append("nodeId=\"" + DataSecurity.XmlEncode(item.NodeId) + "\" ");
                xmlBuilder.Append("target=\"" + DataSecurity.XmlEncode(item.Target) + "\" ");
                xmlBuilder.Append("expand=\"" + DataSecurity.XmlEncode(item.Expand) + "\" ");
                xmlBuilder.Append("action=\"" + DataSecurity.XmlEncode(item.Action) + "\" ");
                xmlBuilder.Append("src=\"" + DataSecurity.XmlEncode(item.XmlSrc) + "\" ");
                xmlBuilder.Append("anchorType=\"" + DataSecurity.XmlEncode(item.AnchorType) + "\" ");
                xmlBuilder.Append("icon=\"" + DataSecurity.XmlEncode(item.Icon) + "\" ");
                xmlBuilder.Append("nodeType=\"" + DataSecurity.XmlEncode(item.NodeType) + "\" ");
                xmlBuilder.Append("enable=\"" + DataSecurity.XmlEncode(item.Enable) + "\" ");
                xmlBuilder.Append("linkUrl=\"" + DataSecurity.XmlEncode(item.LinkUrl) + "\" ");
                xmlBuilder.Append(" />");
            }

            xmlBuilder.Append("</tree>\n");
            return xmlBuilder.ToString();
        }
    }
}
