using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;
using AgileEAP.WebControls;
using AgileEAP.Core;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Plugin.Authorize
{
    public partial class ChooseParentResource : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            InitTree();
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        private void InitTree()
        {
            AjaxTree1.PostType = PostType.NoPost;
            AjaxTree1.IsAjaxLoad = false;
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.ShowCheckBox = true;
            AjaxTree1.SelectionMode = SelectionMode.Single;
            AjaxTree1.Nodes.Clear();

            Resource resource =repository.All<Resource>().FirstOrDefault(o => o.ID == AppID);
            AjaxTreeNode appNode = new AjaxTreeNode()
            {
                ID = resource.ID,
                Text = resource.Text,
                Value = resource.Type.ToString(),
                Tag = "root",
                IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/resource.gif", WebUtil.GetRootPath(), Skin),
                NodeState = NodeState.Open
            };

            AjaxTree1.Nodes.Add(appNode);

            List<Resource> resources =repository.All<Resource>().Where(o => o.ParentID == appNode.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var res in resources)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = res.ID,
                    Text = res.Text,
                    Value = res.ID,
                    Tag = res.Type.ToString(),
                    IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(res.Type.Cast<ResourceType>(ResourceType.Menu)))
                };

                BuildTree(node);

                appNode.ChildNodes.Add(node);
            }
        }

        /// <summary>
        /// 设置目录图标
        /// </summary>
        /// <param name="ResourceType"></param>
        /// <returns></returns>
        private string getResourceIcon(ResourceType ResourceType)
        {
            string icon = "page.png";

            switch (ResourceType)
            {
                case ResourceType.Button:
                    icon = "button.png";
                    break;
                case ResourceType.Menu:
                    icon = "menu.gif";
                    break;
                case ResourceType.Page:
                    icon = "page.png";
                    break;
            }

            return icon;
        }

        /// <summary>
        /// 创建目录树方法
        /// </summary>
        /// <param name="tn">目录树的节点</param>
        private void BuildTree(AjaxTreeNode tn)
        {
            List<Resource> resources =repository.All<Resource>().Where(o => o.ParentID == tn.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var res in resources)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = res.ID,
                    Text = res.Text,
                    Value = res.ID,
                    Tag = res.Type.ToString(),
                    NodeIcoSrc = tn.NodeIcoSrc,
                    IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(res.Type.Cast<ResourceType>(ResourceType.Menu)))
                };

                tn.ChildNodes.Add(node);
                //递归获取目录树
                BuildTree(node);
            }
        }
    }
}