using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Infrastructure;
namespace AgileEAP.MVC.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private  IAuthenticationService authenticationService;
        //public AuthorizationAttribute(IAuthenticationService authenticationService)
        //{
        //    this.authenticationService = authenticationService;
        //}
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
            if (authenticationService.GetAuthenticatedUser() == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                //filterContext.Result = new RedirectResult("~/Error");
            }
        }
    }
}
