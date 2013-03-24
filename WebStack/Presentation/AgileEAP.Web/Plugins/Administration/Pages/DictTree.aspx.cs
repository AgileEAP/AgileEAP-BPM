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

using System.IO;

namespace AgileEAP.Administration
{
    public partial class DictTree : BasePage
    {
        DictService dictService = new DictService();
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
            AjaxTree1.ShowCheckBox = false;
            AjaxTree1.Nodes.Clear();

            Dict appDict = dictService.All().FirstOrDefault(o => o.ID == AppID);
            AjaxTreeNode appNode = new AjaxTreeNode()
            {
                ID = appDict.ID,
                Text = appDict.Text,
                Value = appDict.Name,
                Tag = "root",
                NodeState = NodeState.Open,
                IcoSrc = string.Format("{0}Plugins/eCloud/Content/Themes/{1}/Images/dictionary.png", WebUtil.GetRootPath(), Skin)
            };
            AjaxTree1.Nodes.Add(appNode);

            List<Dict> dicts = dictService.All().Where(o => o.ParentID == appNode.ID).OrderBy(o => o.SortOrder).ToList();
            foreach (var dict in dicts)
            {
                AjaxTreeNode node = new AjaxTreeNode()
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
        private void BuildTree(AjaxTreeNode tn)
        {
            List<Dict> dicts = dictService.All().Where(o => o.ParentID == tn.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var dict in dicts)
            {
                AjaxTreeNode node = new AjaxTreeNode()
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


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="argument"></param>
        public string DeleteTreeNode(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                Dict dict = dictService.GetDomain(argument);

                if (dict != null)
                {
                    dictService.Delete(dict);
                    doResult = DoResult.Success;
                }
                else
                {
                    doResult = DoResult.Failed;
                }

                //获取提示信息
                actionMessage = string.Format("删除字典{0}成功", dict.Name);

                //记录操作日志
                AddActionLog(dict, doResult, actionMessage);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = dict.ParentID;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
    }
}