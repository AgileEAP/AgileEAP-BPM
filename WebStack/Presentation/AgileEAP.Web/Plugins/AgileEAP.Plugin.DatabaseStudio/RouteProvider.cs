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

namespace AgileEAP.Plugin.DatabaseStudio
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.MapLocalizedRoute("DatabaseStudio.Default.Route",
                   "DatabaseStudio/{action}/{id}",
                   new { plugin = "AgileEAP.Plugin.DatabaseStudio", controller = "Home", action = "Index", id = UrlParameter.Optional },
                   new[] { "AgileEAP.Plugin.DatabaseStudio.Controllers" });
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件AgileEAP.Plugin.DatabaseStudio路由出错{0}", ex);
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