using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;
using AgileEAP.WebControls;

using System.IO;

namespace AgileEAP.Plugin.Authorize
{
    public partial class OrgTreeForChoose : BasePage
    {
        public override bool Authorize(Core.Authentication.IUser user, string requestUrl, bool isForce)
        {
            return base.Authorize(user, requestUrl, false);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            InitTree();
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        private void InitTree()
        {
            AjaxTree1.PostType = PostType.None;
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.ShowCheckBox = !string.Equals(Request.QueryString["Entry"], "Operator");
            AjaxTree1.IsAjaxLoad = false;
            AjaxTree1.SelectionMode = SelectionMode.Single;
            AjaxTree1.Nodes.Clear();

            List<string> dataPriveleges =new AuthorizeService().GetDataPriveleges(User.ID);

            string root = Configure.Get("OrgRootID");
            Organization org = repository.GetDomain<Organization>(root);
            if (org == null) return;
            AjaxTreeNode parentNode = new AjaxTreeNode()
            {
                ID = org.ID,
                Text = org.Name,
                Value = org.Code,
                Tag = org.ID,
                NodeState = AgileEAP.WebControls.NodeState.Open,
                IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/orgtree.gif", WebUtil.GetRootPath(), Skin),
                Target = "ifrMain",
                VirtualNodeCount = repository.All<Organization>().Count(o => o.ParentID == root)
            };

            AjaxTree1.Nodes.Add(parentNode);

            BuildOrgTree(parentNode, dataPriveleges);
        }

        private void BuildOrgTree(AjaxTreeNode tn, List<string> dataPriveleges)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ParentID", tn.ID);
            dic.Add("Status", "0");
            IList<Organization> orgs =repository.FindAll<Organization>(dic);//.Where(o => IsAdmin || dataPriveleges.Exists(p => o.OwnerOrg.StartsWith(p))).OrderBy(o => o.SortOrder).ToList();
            if (orgs.Count == 0 && string.Equals(Request.QueryString["Entry"], "Operator"))
            {
                BuildUserTree(tn);
            }
            foreach (var item in orgs)
            {
                string value = string.Format("{0}/{1}", tn.Value, item.ID);

                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = item.ID,
                    Text = item.Name,
                    Value = value,
                    Tag = "Org",
                    NodeIcoSrc = tn.NodeIcoSrc,
                    IcoSrc = string.Format("{0}Plugins/Authorize/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(item.Type.Cast<ResourceType>(ResourceType.Menu))),
                    Target = "ifrMain"
                };

                tn.ChildNodes.Add(node);
                BuildOrgTree(node, dataPriveleges);
            }
        }

        private void BuildUserTree(AjaxTreeNode tn)
        {
            Organization org = repository.GetDomain<Organization>(tn.ID);
            List<Operator> operatorList = repository.Query<Operator>().Where(o => o.OwnerOrg.Equals(org.Code)).ToList();
            foreach (var item in operatorList)
            {
                //string value = string.Format("{0}/{1}", tn.Value, item.ID);

                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = item.ID,
                    Text = item.UserName,
                    Value = item.ID,
                    Tag = "Operator",
                    ShowCheckBox = true,
                    Checked = false,
                };
                tn.ChildNodes.Add(node);
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
                    icon = "orgtree.gif";
                    break;
            }

            return icon;
        }
    }
}