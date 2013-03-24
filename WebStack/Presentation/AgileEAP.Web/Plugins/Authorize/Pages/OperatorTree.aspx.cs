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

namespace AgileEAP.Plugin.Authorize
{
    public partial class OperatorTree : BasePage
    {
        IAuthorizeService authService = new AuthorizeService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitTree();
            }
        }
        /// <summary>
        /// 设置目录图标
        /// </summary>
        /// <param name="ResourceType"></param>
        /// <returns></returns>
        private string getResourceIcon(ResourceType ResourceType)
        {
            string icon = "orgtree.gif";

            switch (ResourceType)
            {
                case ResourceType.Button:
                    icon = "orgtree.gif";
                    break;
                case ResourceType.Menu:
                    icon = "orgtree.gif";
                    break;
                case ResourceType.Page:
                    icon = "group.png";
                    break;
            }

            return icon;
        }


        /// <summary>
        /// 初始化树
        /// </summary>
        private void InitTree()
        {
            AjaxTree1.PostType = PostType.None;
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.ShowCheckBox = false;
            AjaxTree1.IsAjaxLoad = true;
            AjaxTree1.SelectionMode = SelectionMode.Single;
            AjaxTree1.Nodes.Clear();

            List<Organization> organizations =repository.All<Organization>().Where(o => o.ParentID == "OR1000000001" && o.Status == 0).OrderBy(o => o.SortOrder).ToList();//中国电信广东公司 的id
            foreach (var org in organizations)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = org.ID,
                    Text = org.Name,
                    Value = org.ID,
                    Tag = org.Type.ToString(),
                    IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(ResourceType.Menu)),
                    VirtualNodeCount = repository.All<Organization>().Count(o => o.ParentID == org.ID) + authService.GetEmployees(org.ID).Count
                };

                AjaxTree1.Nodes.Add(node);
            }
        }

        protected override void HandleAjaxPost()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //处理AjaxPost
            if (IsAjaxPost)
            {
                base.HandleAjaxPost();
            }
        }
        private AjaxTreeNodeCollection getChildrenNodes(string id)
        {
            AjaxTreeNodeCollection result = new AjaxTreeNodeCollection();

            IList<Organization> organizations = repository.All<Organization>().Where(o => o.ParentID == id && o.Status == 0).OrderBy(o => o.SortOrder).ToList<Organization>();

            if (organizations != null)
            {
                foreach (var org in organizations)
                {
                    AjaxTreeNode node = new AjaxTreeNode()
                    {
                        ID = org.ID,
                        Text = org.Name,
                        Value = org.ID,
                        Tag = org.Type.ToString(),
                        IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(ResourceType.Menu)),
                        VirtualNodeCount = repository.All<Organization>().Count(o => o.ParentID == org.ID) + authService.GetEmployees(org.ID).Count
                    };
                    result.Add(node);
                }
            }

            IList<Employee> employees =authService.GetEmployees(id);

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    AjaxTreeNode newNode = CreateNode(employee);
                    result.Add(newNode);
                }
            }

            return result;
        }

        public string AjaxExpand(string id)
        {
            bool isLastNode = Request.Form["IsLastNode"].Cast<bool>(false);
            AjaxTreeNodeCollection ajaxTreeNodes = getChildrenNodes(id);
            ajaxTreeNodes.Owner = AjaxTree1;
            ajaxTreeNodes.NodesState = NodeState.Open;
            Html32TextWriter writer = new Html32TextWriter(Response.Output);
            ajaxTreeNodes.AjaxRender(writer, 1, isLastNode);

            return string.Empty;
        }

        /// <summary>
        /// 创建人员节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private AjaxTreeNode CreateNode(Employee model)
        {
            if (model == null) return null;
            return new AjaxTreeNode()
            {
                ID = model.ID,
                Text = model.Name,
                Value = model.ID,
                ShowCheckBox = true,
                IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(ResourceType.Page))
            };
        }

    }
}