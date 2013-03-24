using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;
using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Authentication;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.MVC.Security;
using AgileEAP.Core.Web;

namespace AgileEAP.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {
        }

        public ActionResult ModifyPassword()
        {
            return View();
        }
        public ActionResult UserManage(string LoginName, string OldPwd, string NewPwd)
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };

            string actionMessage = string.Empty;
            try
            {
                Operator loginModel = repository.Query<Operator>().Where(l => l.LoginName == LoginName && l.Password == CryptographyManager.EncodePassowrd(OldPwd)).FirstOrDefault();
                if (loginModel != null)
                {
                    repository.ExecuteSql<Operator>(string.Format("update AC_Operator set Password='{0}' where LoginName='{1}' and PassWord='{2}'", CryptographyManager.EncodePassowrd(NewPwd), LoginName, CryptographyManager.EncodePassowrd(OldPwd)));
                    ajaxResult.Result = DoResult.Success;
                    ////获取提示信息
                    actionMessage = string.Format("修改用户{0}密码成功", LoginName);

                    ////记录操作日志
                    AddActionLog<Operator>(actionMessage, ajaxResult.Result);
                }
                else
                {
                    actionMessage = string.Format("修改用户{0}密码失败", LoginName);
                }
                ajaxResult.RetValue = loginModel;
                ajaxResult.PromptMsg = actionMessage;

                //repository.SaveOrUpdate(applyInfo);


            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                //记录操作日志
                //AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
                log.Error(actionMessage, ex);
            }

            return Json(ajaxResult);
        }
    }
}
