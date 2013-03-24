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
using AgileEAP.Core.Caching;

namespace AgileEAP.Web.Controllers
{
    public class AuthorizeController : BaseController
    {
        private readonly IAuthenticationService authenticationService;
        public AuthorizeController(IAuthenticationService authenticationService, IRepository<string> repository)
            : base(repository)
        {
            this.authenticationService = authenticationService;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            authenticationService.SignOut();
            return Redirect("/login");// RedirectToAction("Login");
        }

        public ActionResult DenyAccess()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = repository.Query<Operator>().FirstOrDefault(o => o.LoginName == model.LoginName && o.Password == CryptographyManager.EncodePassowrd(model.Password));
                    if (user != null)
                    {
                        //sign in 
                        authenticationService.SignIn(new User() { ID = user.ID, Name = user.UserName, UserType = user.UserType, LoginName = user.LoginName, Theme = user.Skin ?? "Default", OrgID = user.OwnerOrg }, model.RememberMe);

                        AddActionLog<Operator>(LogType.Login, user.UserName + "登录IDC云平台！", DoResult.LoginSuccess);
                        user.LastLogin = DateTime.Now;
                        repository.SaveOrUpdate(user);
                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl.Length > 3 && !returnUrl.EndsWith("logout"))
                            return Redirect(returnUrl);

                        return Redirect(System.Web.Security.FormsAuthentication.DefaultUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("LoginName", "账户或密码错误");
                    }
                }
                catch (Exception ex)
                {
                    string actionMessage = string.Format("用户{0}登录出错", model.LoginName);
                    //记录操作日志
                    AddActionLog<Operator>(LogType.Login, actionMessage, DoResult.Failed);
                    GlobalLogger.Error<AuthorizeController>(string.Format("用户{0}登录出错,Error:{1}", model.LoginName, ex.Message));
                }
            }

            return View(model);
        }
    }
}
