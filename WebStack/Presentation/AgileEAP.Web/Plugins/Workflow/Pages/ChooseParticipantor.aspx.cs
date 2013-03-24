using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using AgileEAP.WebControls;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

using System.IO;
using System.Data;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.Plugin.Workflow
{
    public partial class ChooseParticipantor : BasePage
    {
        WorkflowEngine workflowEngine = new WorkflowEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitTree();
        }


        protected override void HandleAjaxPost()
        {
            System.Diagnostics.Debug.Write("none");
        }


        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            //处理AjaxPost
            if (IsAjaxPost)
            {
                base.HandleAjaxPost();
            }
        }

        #region AjaxTree有关

        /// <summary>
        /// 初始化树
        /// </summary>
        private void InitTree()
        {
            AjaxTree1.PostType = PostType.None;
            AjaxTree1.ShowNodeIco = true;
            AjaxTree1.ShowCheckBox = string.Equals(Request.QueryString["Entry"], "Choose") || string.Equals(Request.QueryString["Entry"], "ConfigureRolePrivilege");
            AjaxTree1.IsAjaxLoad = true;
            AjaxTree1.SelectionMode = SelectionMode.Single;
            AjaxTree1.EnableOndblclick = true;
            AjaxTree1.Nodes.Clear();

            IList<Participantor> all = workflowEngine.GetRoleAndOrgParticipantors();
            Participantor orgParticipant = all.FirstOrDefault(p => p.ParticipantorType == ParticipantorType.Org && p.ParentID == "-1");

            //加载第一层中国电信广东分公司节点
            AjaxTreeNode parentNode = new AjaxTreeNode()
            {
                ID = orgParticipant.ID,
                Text = orgParticipant.Name,
                Value = orgParticipant.ID,
                Tag = Enum.Parse(typeof(ParticipantorType), orgParticipant.ParticipantorType.ToString()).ToString(),
                NodeState = NodeState.Open,
                IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(orgParticipant.ParticipantorType.ToString())),
                VirtualNodeCount = all.Count(o => o.ParentID == orgParticipant.ID)
            };
            AjaxTree1.Nodes.Add(parentNode);

            //加载第一层角色树节点
            AjaxTreeNode roleNode = new AjaxTreeNode()
            {
                ID = "role",
                Text = "角色",
                Value = "roleTree",
                Tag = "Role",
                NodeState = NodeState.Open,
                IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon("Role")),
                VirtualNodeCount = all.Count(o => o.ParticipantorType == ParticipantorType.Role)
            };
            AjaxTree1.Nodes.Add(roleNode);

            //第二层组织结构节点
            IList<Participantor> participantors = all.Where(p => p.ParentID == orgParticipant.ID).ToList();
            foreach (var org in participantors)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = org.ID,
                    Text = org.Name,
                    Value = org.ID,
                    Tag = Enum.Parse(typeof(ParticipantorType), org.ParticipantorType.ToString()).ToString(),
                    IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(org.ParticipantorType.ToString())),
                    VirtualNodeCount = all.Count(o => o.ParentID == org.ID)
                };

                parentNode.ChildNodes.Add(node);
            }

            //第二层角色节点
            IList<Participantor> roles = all.Where(p => p.ParticipantorType == ParticipantorType.Role).ToList();
            foreach (var role in roles)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = role.ID,
                    Text = role.Name,
                    Value = role.ID,
                    Tag = Enum.Parse(typeof(ParticipantorType), role.ParticipantorType.ToString()).ToString(),
                    IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(role.ParticipantorType.ToString())),
                    VirtualNodeCount = all.Count(o => o.ParentID == role.ID)
                };
                roleNode.ChildNodes.Add(node);
            }


        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private AjaxTreeNodeCollection getChildrenNodes(string id)
        {
            AjaxTreeNodeCollection result = new AjaxTreeNodeCollection();

            IList<Participantor> participantors = workflowEngine.GetRoleAndOrgParticipantors().Where(p => p.ParentID == id).ToList();
            foreach (Participantor participantor in participantors)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = participantor.ID,
                    Text = participantor.Name,
                    Value = participantor.ID,
                    Tag = Enum.Parse(typeof(ParticipantorType), participantor.ParticipantorType.ToString()).ToString(),
                    IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(participantor.ParticipantorType.ToSafeString())),
                    VirtualNodeCount = workflowEngine.GetRoleAndOrgParticipantors().Count(o => o.ParentID == participantor.ID),
                };

                result.Add(node);
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
        /// 设置目录图标
        /// </summary>
        /// <param name="ResourceType"></param>
        /// <returns></returns>
        private string getResourceIcon(string icon)
        {
            return "orgtree.gif";
        }

        /// <summary>
        /// 创建目录树方法
        /// </summary>
        /// <param name="tn">目录树的节点</param>
        private void BuildTree(AjaxTreeNode tn)
        {
            List<Participantor> resources = workflowEngine.GetRoleAndOrgParticipantors().Where(o => o.ParentID == tn.ID).OrderBy(o => o.SortOrder).ToList();

            foreach (var res in resources)
            {
                AjaxTreeNode node = new AjaxTreeNode()
                {
                    ID = res.ID,
                    Text = res.Name,
                    Value = res.ID,
                    Tag = Enum.Parse(typeof(ParticipantorType), res.ParticipantorType.ToString()).ToString(),
                    NodeIcoSrc = tn.NodeIcoSrc,
                    // LinkUrl = string.Format("{0}AuthorizeCenter/OrgUserList.aspx?orgid={1}", WebUtil.GetRootPath(), res.ID),
                    //Target = "ifrMain",
                    IcoSrc = string.Format("{0}Plugins/Workflow/Content/Themes/{1}/Images/{2}", WebUtil.GetRootPath(), Skin, getResourceIcon(res.ParticipantorType.ToString()))
                };

                tn.ChildNodes.Add(node);
                //递归获取目录树
                BuildTree(node);
            }
        }
        #endregion

        //获取组织或角色下的人员
        public string GetOrgOrRoleUsers(string argument)
        {
            string type = Request["Type"];
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                ParticipantorType participantorType = (ParticipantorType)Enum.Parse(typeof(ParticipantorType), type);
                var participantors = workflowEngine.GetPersonParticipantors(participantorType, argument);
                if (participantors == null)
                {
                    actionResult = DoResult.Other;
                    actionMessage = "返回数据为空";
                    ajaxResult.RetValue = null;
                }
                else
                {
                    actionResult = DoResult.Success;

                    //获取提示信息
                    actionMessage = RemarkAttribute.GetEnumRemark(actionResult);

                    ajaxResult.RetValue = participantors;
                }

                ajaxResult.Result = actionResult;
                ajaxResult.PromptMsg = actionMessage;

            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
    }
}