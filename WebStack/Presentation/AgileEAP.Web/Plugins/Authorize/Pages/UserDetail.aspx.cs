using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using AgileEAP.Core;
using AgileEAP.Core.Data;

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
using System.IO;

namespace AgileEAP.Plugin.Authorize
{
    public partial class UserDetail : BasePage
    {
        protected bool ShowPageDetail()
        {
            if (PageContext.Action == ActionType.Add)
            {
                dtpExpireTime.Text = DateTime.Now.ToShortDateString();
                dtpLastLogin.Text = DateTime.Now.ToShortDateString();
                dtpStartTime.Text = DateTime.Now.ToShortDateString();
                dtpEndTime.Text = DateTime.Now.ToShortDateString();
                dtpBirthday.Text = DateTime.Now.ToShortDateString();
                dtpWorkFromDate.Text = DateTime.Now.ToShortDateString();
                dtpInDate.Text = DateTime.Now.ToShortDateString();
                dtpOutDate.Text = DateTime.Now.ToShortDateString();

                Organization org = repository.GetDomain<Organization>(Request.QueryString["OrgID"]);
                if (org != null)
                {
                    chbMajorOrgID.Text = org.Name;
                    chbMajorOrgID.Value = org.ID;
                }
                return false;
            }

            Operator _operator = repository.GetDomain<Operator>(CurrentId);
            Employee employee = repository.GetDomain<Employee>(CurrentId);

            if (_operator == null) return false;

            SetControlValues(_operator);
            SetControlValues(employee);
            txtOldPassword.Value = _operator.Password;

            Organization organization = repository.GetDomain<Organization>(employee.MajorOrgID);
            if (organization != null)
            {
                chbMajorOrgID.Text = organization.Name;
                chbMajorOrgID.Value = organization.ID;
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsAjaxPost)
            {
                ShowPageDetail();
            }
        }
        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                Operator opr = JsonConvert.DeserializeObject<Operator>(argument);
                Operator old = repository.FindOne<Operator>(ParameterBuilder.BuildParameters().SafeAdd("ID", CurrentId));

                if ((old == null || old.LoginName != opr.LoginName) & repository.FindOne<Operator>(ParameterBuilder.BuildParameters().SafeAdd("LoginName", opr.LoginName)) != null)
                {
                    ajaxResult.PromptMsg = "此登录名已存在！";
                    return JsonConvert.SerializeObject(ajaxResult);
                }

                opr.Skin = Configure.Get("DefaultSkin", "Default");
                opr.Creator = User.ID;
                opr.ID = old != null ? old.ID : IdGenerator.NewComb().ToString();

                if (old != null)
                {
                    if (string.IsNullOrEmpty(opr.Password))
                    {
                        opr.Password = old.Password;
                    }
                    else
                    {
                        opr.Password = CryptographyManager.EncodePassowrd(opr.Password);
                    }
                }
                else
                {
                    opr.Password = CryptographyManager.EncodePassowrd(opr.Password);
                }

                string orgID = Request.Form["OrgID"];
                Employee emp = JsonConvert.DeserializeObject<Employee>(argument);
                emp.ID = opr.ID;
                emp.OperatorID = emp.ID;
                emp.MajorOrgID = orgID;
                emp.Email = opr.Email;
                emp.Name = opr.UserName;
                emp.LoginName = opr.LoginName;
                emp.Creator = opr.Creator;

                string orgPath = GetOrgCodePath(orgID);
                if (orgID == "OrgRootID")
                {
                    orgPath = Configure.Get("AreaCode", "GDProvince");
                }
                opr.OwnerOrg =  orgPath;
                emp.OwnerOrg = orgPath;

                EmployeeOrg empOrg = new EmployeeOrg();
                if (PageContext.Action == ActionType.Add)
                {
                    empOrg.ID = IdGenerator.NewComb().ToString();
                    empOrg.OrgID = orgID;
                    empOrg.EmployeeID = emp.ID;
                    empOrg.IsMajor = 1;
                }
                else
                {
                    empOrg.ID =repository.FindOne<EmployeeOrg>(ParameterBuilder.BuildParameters().SafeAdd("EmployeeID", Request.Form["CurrentId"])).ID;
                    empOrg.EmployeeID = emp.ID;
                    empOrg.IsMajor = 1;
                    empOrg.OrgID = orgID;
                }

                UnitOfWork.ExecuteWithTrans<Operator>(() =>
                    {
                        repository.SaveOrUpdate(opr);
                        repository.SaveOrUpdate(emp);
                        repository.SaveOrUpdate(empOrg);
                    });

                doResult = DoResult.Success;

                //获取提示信息
                actionMessage = string.Format("修改人员信息{0}成功", emp.Name);

                //记录操作日志
                AddActionLog(emp, doResult, actionMessage);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = emp.ID;
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