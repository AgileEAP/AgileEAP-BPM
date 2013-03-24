using System;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Web.Security;
using System.Web.Compilation;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Localization;

namespace AgileEAP.Plugin.iReport
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.MapLocalizedRoute("iReport.Default.Route",
                    "iReport/{action}/{id}",
                    new { plugin = "AgileEAP.Plugin.iReport", controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new[] { "AgileEAP.Plugin.iReport.Controllers" });

                //routes.MapLocalizedRoute("iReport.Default.Route",
                //    "iReport/{action}/{id}",
                //    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                //    new[] { "AgileEAP.Plugin.iReport.Controllers" });
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件AgileEAP.Plugin.iReport路由出错{0}", ex);
            }
        }
        public int Priority
        {
            get
            {
                return 13;
            }
        }
    }
}