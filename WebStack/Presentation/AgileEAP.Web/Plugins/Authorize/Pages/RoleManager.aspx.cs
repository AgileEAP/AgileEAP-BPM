﻿#region Description
/*================================================================================
 *  Copyright (c) agile.  All rights reserved.
 * ===============================================================================
 * Solution: AuthorizeCenter
 * Module:  Role
 * Descrption:
 * CreateDate: 2010/11/18 13:55:37
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:
 * Remark:
 * Update Time:
 * Author:generated by codesmithTemplate
 * 
 * ===============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;



using AgileEAP.Core;
using AgileEAP.Core.Data;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;

using AgileEAP.WebControls;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;


namespace AgileEAP.Plugin.Authorize
{
    public partial class RoleManager : BasePage
    {

        #region ---界面处理方法---

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvList.PageChanged += new PagedGridView.PagintEventHandler(gvList_PageChanged);

        }

        void gvList_PageChanged(object sender, PagingArgs e)
        {
            ShowList(gvList, new PageInfo(e.PageIndex, e.PageSize, e.ItemCount));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
            }
        }

        #endregion

        #region ---操作处理方法---
        /// <summary>
        /// 转向明细页面
        /// </summary>
        /// <param name="param"></param>
        protected void Redirect(string param)
        {
            var currentIdParam = PageContext.Action == ActionType.Add ? string.Empty : string.Format("&CurrentId={0}", CurrentId);
            Response.Redirect(string.Format("RoleDetail.aspx?LastUrl={0}&Runat=1&ActionFlag={1}{2}{3}", Request.Url.PathAndQuery, PageContext.Action, currentIdParam, string.IsNullOrEmpty(param) ? param : "&" + param));
        }

        /// <summary>
        /// 新增
        /// </summary>
        public void Add()
        {
            PageContext.Action = ActionType.Add;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }

        /// <summary>
        /// 查看
        /// </summary>
        public void View()
        {
            PageContext.Action = ActionType.View;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }

        /// <summary>
        /// 修改
        /// </summary>
        public void Update()
        {
            PageContext.Action = ActionType.Update;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void Search()
        {
            ShowList(gvList, new PageInfo(1, gvList.PageSize, gvList.ItemCount));
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
        }

        /// <summary>
        /// 显示列表信息
        /// </summary>
        /// <param name="gvList">GridView对象</param>
        /// <param name="pageInfo">分页信息</param>
        public void ShowList(PagedGridView gvList, PageInfo pageInfo)
        {
            IDictionary<string, object> parameters = GetFilterParameters();
            parameters.SafeAdd("dataFilter", new Condition(GetFilterString()));
            if (User.UserType != 0)
                parameters.SafeAdd("ID", new Condition(string.Format(" OwnerOrg like '{0}%' ", GetOrgPath())));
            IPageOfList<Role> result = repository.FindAll<Role>(parameters, "Order By Name", pageInfo);
            gvList.ItemCount = result.PageInfo.ItemCount;
            gvList.DataSource = result;
            gvList.DataBind();
        }

        public string GetAppsDDl()
        {
            return GetAppsDDl("");
        }

        public string GetAppsDDl(string selectedValue)
        {
            StringBuilder sb = new StringBuilder("<select onchange='onAppSelect(this)'><option  value='-1'>请选择</option>");
            List<App> apps =repository.FindAll<App>(GetFilterParameters()).ToList();
            foreach (App app in apps)
            {
                sb.Append(string.Format("<option value='{0}'", app.ID));
                if (!string.IsNullOrWhiteSpace(selectedValue) && app.ID == selectedValue)
                {
                    sb.Append(" selected='selected' ");
                }
                sb.Append(string.Format(">{0}</option>", app.Text));
            }

            sb.Append("</select>");

            return sb.ToSafeString();

        }

        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                Role role = JsonConvert.DeserializeObject<Role>(argument);
                role.Creator = User.ID;
                role.OwnerOrg = GetOrgPath();
                role.CreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(role.ID))
                {
                    IDictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("Name", role.Name);
                    Role roleCheck = repository.FindOne<Role>(para);
                    if (roleCheck != null)
                    {
                        ajaxResult.PromptMsg = "角色名称重复，请重新输入";
                        return JsonConvert.SerializeObject(ajaxResult);
                    }

                    role.ID = IdGenerator.NewComb().ToString();
                    repository.SaveOrUpdate(role);
                }
                else
                {
                    repository.Update(role);
                }

                doResult = DoResult.Success;

                //获取提示信息
                actionMessage = string.Format("保存角色{0}成功", role.Name);

                //记录操作日志
                AddActionLog(role, doResult, actionMessage);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = role.ID;
                ajaxResult.PromptMsg = actionMessage;

            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }


        public string Delete(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(argument))
                {
                    string roleID = argument;

                    IRepository<string> repository = new Repository<string>();
                    IDictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.SafeAdd("RoleID", roleID);
                    IList<ObjectRole> objectRoleList = repository.FindAll<ObjectRole>(parameters);
                    if (objectRoleList.Count == 0)
                    {
                        repository.Delete<Role>(roleID);
                        repository.ExecuteSql<RolePrivilege>(string.Format("Delete from AC_RolePrivilege where RoleID='{0}'", roleID));
                        doResult = DoResult.Success;
                        actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                    }
                    else
                    {
                        doResult = DoResult.Failed;
                        actionMessage = "请先解除该角色与操作员的关联！";
                    }

                    ajaxResult.RetValue = CurrentId;
                    ajaxResult.PromptMsg = actionMessage;
                }

                ajaxResult.Result = doResult;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }


        public string GetUserNameByUserId(string userId)
        {
            Operator operatorInfo = repository.GetDomain<Operator>(userId);
            if (operatorInfo != null)
            {
                return operatorInfo.UserName;
            }
            return "";
        }

        public string GetAppNameById(object id)
        {
            if (!string.IsNullOrWhiteSpace(id.ToSafeString()))
            {
                App app = repository.GetDomain<App>(id.ToSafeString());
                if (app != null)
                {
                    return app.Text;
                }
            }
            return "";
        }

        #endregion


    }
}
