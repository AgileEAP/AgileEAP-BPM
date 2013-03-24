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
    public partial class CatalogTree : BasePage
    {
        CatalogService catalogService = new CatalogService();
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
            AjaxTree1.ShowCheckBox = string.Equals(Request.QueryString["Entry"], "Choose");
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.SelectionMode = SelectionMode.Single;
            AjaxTree1.Nodes.Clear();

            Catalog appCatalog = catalogService.All().FirstOrDefault(o => o.ParentID == "0");
            AjaxTreeNode appNode = new AjaxTreeNode()
            {
                ID = appCatalog.ID,
                Text = appCatalog.CatalogName,
                Value = appCatalog.ID,
                Tag = "root",
                NodeState = NodeState.Open,
                IcoSrc = string.Format("{0}Plugins/eCloud/Content/Themes/{1}/Images/menu.gif", WebUtil.GetRootPath(), Skin)
            };
            AjaxTree1.Nodes.Add(appNode);

            List<Catalog> catalogs = catalogService.All().Where(o => o.ParentID == appNode.ID).OrderBy(o => o.SortOrder).ToList();
            foreach (var catalog in catalogs)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = catalog.ID,
                    Text = catalog.CatalogName,
                    Value = catalog.ID,
                    Tag = catalog.ID,
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
            string icon = "menu.gif";

            switch (dictType)
            {
                case "parentDict":
                    icon = "menu.gif";
                    break;
                case "childDict":
                    icon = "menu.gif";
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
            List<Catalog> catalogs = catalogService.All().Where(o => o.ParentID == tn.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var catalog in catalogs)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = catalog.ID,
                    Text = catalog.CatalogName,
                    Value = catalog.ID,
                    Tag = catalog.ID,
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
                Catalog catalog = catalogService.GetDomain(argument);

                List<Catalog> childDict = catalogService.All().Where(o => o.ParentID == catalog.ID).ToList(); //删除的目录下的子目录

                List<UploadFile> uploadFile = new UploadFileService().All().Where(o => o.CatalogID == catalog.ID).ToList();//删除的目录下的文件

                if (childDict.Count > 0 || uploadFile.Count > 0)
                {
                    actionMessage = "该目录下存在子目录或文件，不允许删除！";
                    ajaxResult.PromptMsg = actionMessage;
                }
                else
                {
                    if (catalog != null)
                    {
                        catalogService.Delete(catalog);
                        doResult = DoResult.Success;
                    }
                    else
                    {
                        doResult = DoResult.Failed;
                    }
                    string directoryPath = Path.Combine(Server.MapPath("~"), catalog.Path);
                    if (Directory.Exists(directoryPath))
                        Directory.Delete(directoryPath);  //删除物理目录路径
                    //获取提示信息
                    actionMessage = string.Format("删除目录{0}成功", catalog.Path);
                }

                //记录操作日志
                AddActionLog(catalog, doResult, actionMessage);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = catalog.ParentID;
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