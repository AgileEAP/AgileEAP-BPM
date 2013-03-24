using System.Web.Mvc;
using System.Web.Routing;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Localization;

namespace AgileEAP.Web.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //home page
            routes.MapLocalizedRoute("Home_Index",
                            "",
                            new { controller = "Home", action = "Index" },
                            new[] { "AgileEAP.Web.Controllers" });

            routes.MapRoute(
                "DenyAccess", // Route name
                "DenyAccess/", // URL with parameters
                new { controller = "Authorize", action = "DenyAccess" },
                new[] { "AgileEAP.Web.Controllers" }
            );

            //login, register
            routes.MapLocalizedRoute("Login",
                            "login",
                            new { controller = "Authorize", action = "Login" },
                            new[] { "AgileEAP.Web.Controllers" });

            routes.MapLocalizedRoute("Logout",
                            "logout/",
                            new { controller = "Authorize", action = "Logout" },
                            new[] { "AgileEAP.Web.Controllers" });

            routes.MapLocalizedRoute("Navigate",
                "Navigate/",
                new { controller = "Home", action = "Navigate" },
                new[] { "AgileEAP.Web.Controllers" });

            //routes.MapLocalizedRoute("Register",
            //                    "register/",
            //                    new { controller = "Authorize", action = "Register" },
            //                    new[] { "AgileEAP.Web.Controllers" });


            //routes.MapLocalizedRoute("RegisterResult",
            //                "registerresult/{resultId}",
            //                new { controller = "Authorize", action = "RegisterResult" },
            //                new { resultId = @"\d+" },
            //                new[] { "AgileEAP.Web.Controllers" });

            ////contact us
            //routes.MapLocalizedRoute("ContactUs",
            //                "contactus",
            //                new { controller = "Common", action = "ContactUs" },
            //                new[] { "AgileEAP.Web.Controllers" });

            ////passwordrecovery
            //routes.MapLocalizedRoute("PasswordRecovery",
            //                "passwordrecovery",
            //                new { controller = "Customer", action = "PasswordRecovery" },
            //                new[] { "AgileEAP.Web.Controllers" });
            //routes.MapLocalizedRoute("PasswordRecoveryConfirm",
            //                "passwordrecovery/confirm",
            //                new { controller = "Customer", action = "PasswordRecoveryConfirm" },
            //                new[] { "AgileEAP.Web.Controllers" });
        }

        public int Priority
        {
            get
            {
                return -1;
            }
        }
    }
}
