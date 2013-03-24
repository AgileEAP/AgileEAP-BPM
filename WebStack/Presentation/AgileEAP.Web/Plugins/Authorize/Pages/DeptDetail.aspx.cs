using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;




using AgileEAP.Core;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Utility;
using AgileEAP.Core.FastInvoker;
using AgileEAP.WebControls;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Plugin.Authorize
{
    public partial class DeptDetail : BasePage
    {

        public override bool Authorize(AgileEAP.Core.Authentication.IUser user, string requestUrl)
        {
            return base.Authorize(user, requestUrl, false);
        }
        public string ParentID
        {
            get
            {
                return Request.QueryString["ParentId"] ?? string.Empty;
            }
        }

        //public override bool IsAdmin
        //{
        //    get
        //    {
        //        bool _IsAdmin = false;
        //        if (PageContext.Action == ActionType.Add)
        //        {
        //            _IsAdmin = User.UserType == (short)UserType.Administrator;
        //        }
        //        return _IsAdmin;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsAjaxPost)
            {
                txtCreator.Text = User.LoginName;
                txtCreateTime.Text = DateTime.Now.ToShortDateString();
                BindCbo();
                ShowPageDetail();
            }
        }
        protected void ShowPageDetail()
        {
            Organization org =repository.GetDomain<Organization>(CurrentId);
            if (org == null) return;

            SetControlValues(org);

            cboArea.SelectedValue = org.Area;
        }

        private void BindCbo()
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            string areaCode = Configure.Get("RootAreaCode", "GDProvince");
            parameter.Add("Name", areaCode);
            Dict dictCity = repository.FindOne<Dict>(parameter);
            parameter.Clear();
            parameter.Add("ParentID", dictCity.ID);
            IList<Dict> citys = repository.FindAll<Dict>(parameter).OrderBy(o => o.SortOrder).ToList();
            foreach (var item in citys)
            {
                cboArea.Items.Add(new ComboxItem() { Value = item.Name, Text = item.Text });
            }
        }

        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            string parentID = string.Empty;
            try
            {
                Organization org = JsonConvert.DeserializeObject<Organization>(argument);
                string actionType = Request.Form["ActionType"];
                if (actionType.Equals("Add"))
                //if (PageContext.Action == ActionType.Add)
                {
                    IDictionary<string, object> parameter = new Dictionary<string, object>();
                    parameter.SafeAdd("Name", org.Name);
                    parameter.SafeAdd("Code", org.Code);
                    IList<Organization> listOrganization =repository.FindAll<Organization>(parameter);
                    if (listOrganization.Count == 0)
                    {
                        org.ID = string.IsNullOrWhiteSpace(CurrentId) ? IdGenerator.NewComb().ToString() : CurrentId;
                        org.ParentID = ParentID;
                        parentID = ParentID;
                        org.Creator = User.ID;
                        //org.Modifier = org.Creator;
                        //org.ModifyTime = org.CreateTime;
                    }
                    else
                    {
                        doResult = DoResult.Failed;
                        //获取提示信息
                        actionMessage = "部门名称已存在";

                        //记录操作日志
                        log.Error(string.Format("新增部门：{0}", actionMessage));
                        ajaxResult.Result = doResult;
                        ajaxResult.PromptMsg = actionMessage;
                        return JsonConvert.SerializeObject(ajaxResult);
                    }
                }
                else if (actionType.Equals("Update"))
                //else if (PageContext.Action == ActionType.Update)
                {
                    Organization orgDetail =repository.GetDomain<Organization>(CurrentId);
                    org.ParentID = orgDetail.ParentID;
                    parentID = org.ParentID;
                    org.ID = orgDetail.ID;
                }

                string areaCode = Configure.Get("AreaCode", "GDProvince");
                if (parentID == areaCode)
                {
                    org.OwnerOrg = string.Format("{0}/{1}/{2}", areaCode, org.Area, org.Code);
                }
                else if (parentID == "-1")
                {
                    org.OwnerOrg = areaCode;
                }
                else
                {
                    Organization parentOrg =repository.GetDomain<Organization>(parentID);
                    org.OwnerOrg = string.Format("{0}/{1}", parentOrg.OwnerOrg, org.Code);
                }

                repository.SaveOrUpdate(org);

                doResult = DoResult.Success;
                //获取提示信息
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);

                //记录操作日志
                AddActionLog(org, doResult, actionMessage);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = org.ID;
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