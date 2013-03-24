using System;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Tracing;

using AgileEAP.Core;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Localization;
using AgileEAP.MVC.WebApi;

namespace AgileEAP.Plugin.Workflow
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {

                routes.Add("WorkflowRoute", new System.Web.Routing.Route("Workflow/{page}.aspx/{*path}", routeHandler: new PluginRouteHandler()));

                routes.MapLocalizedRoute("Workflow.eForm.Index",
                                "Workflow/eForm",
                                new { controller = "eForm", action = "Index", },
                                new[] { "AgileEAP.Plugin.Workflow.Controllers" });

                routes.MapLocalizedRoute("Workflow.Process.Tracking",
                "Workflow/Process/Tracking",
                new { controller = "Design", action = "DrawWorkflow", },
                new[] { "AgileEAP.Plugin.Workflow.Controllers" });

                routes.MapRoute("Workflow.Default.Route",
                    "Workflow/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new[] { "AgileEAP.Plugin.Workflow.Controllers" });

           
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件Workflow路由出错{0}", ex);
            }
        }
        public int Priority
        {
            get
            {
                return 12;
            }
        }
    }
}
