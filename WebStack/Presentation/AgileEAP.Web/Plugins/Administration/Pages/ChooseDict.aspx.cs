using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;
using AgileEAP.WebControls;


namespace AgileEAP.Administration
{
    public partial class ChooseDict : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                InitTree();
            }
            catch (Exception ex)
            {
                log.Error("初始化字典树出错！", ex);
            }
        }
        /// <summary>
        /// 初始化树
        /// </summary>
        private void InitTree()
        {
            AjaxTree1.PostType = TreeViewPostType.AjaxPost;
            AjaxTree1.IsAjaxLoad = false;
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.ShowCheckBox = true;
            AjaxTree1.SelectionMode = TreeViewSelectionMode.Single;
            AjaxTree1.Nodes.Clear();

            Dict appDict = repository.All<Dict>().FirstOrDefault(o => o.ID == AppID);
            TreeViewNode appNode = new TreeViewNode()
            {
                ID = appDict.ID,
                Text = appDict.Text,
                Value = appDict.Name,
                Tag = "root",
                NodeState = TreeViewNodeState.Open,
                IcoSrc = string.Format("{0}Plugins/eCloud/Content/Themes/{1}/Images/dictionary.png", WebUtil.GetRootPath(), Skin)
            };
            AjaxTree1.Nodes.Add(appNode);

            List<Dict> dicts = repository.All<Dict>().Where(o => o.ParentID == appNode.ID).OrderBy(o => o.SortOrder).ToList();
            foreach (var dict in dicts)
            {
                TreeViewNode node = new TreeViewNode()
                {
                    ID = dict.ID,
                    Text = dict.Text,
                    Value = dict.ID,
                    Tag = dict.ID,
                    IcoSrc = string.Format("{0}Plugins/eCloud/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon("childDict"))
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
        private string getResourceIcon(string dictType)
        {
            string icon = "accessories_dictionary.png";

            switch (dictType)
            {
                case "parentDict":
                    icon = "dictionary.png";
                    break;
                case "childDict":
                    icon = "accessories_dictionary.png";
                    break;
            }

            return icon;
        }

        /// <summary>
        /// 创建目录树方法
        /// </summary>
        /// <param name="tn">目录树的节点</param>
        private void BuildTree(TreeViewNode tn)
        {
            List<Dict> dicts = repository.All<Dict>().Where(o => o.ParentID == tn.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var dict in dicts)
            {
                TreeViewNode node = new TreeViewNode()
                {
                    ID = dict.ID,
                    Text = dict.Text,
                    Value = dict.ID,
                    Tag = dict.ID,
                    NodeIcoSrc = tn.NodeIcoSrc,
                    IcoSrc = string.Format("{0}Plugins/eCloud/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon("childDict"))
                };

                tn.ChildNodes.Add(node);
                //递归获取目录树
                BuildTree(node);
            }
        }
    }
}