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

namespace AgileEAP.Plugin.WorkflowDesigner
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.MapLocalizedRoute("WorkflowDesigner.Workflow.Design",
                                "WorkflowDesigner",
                                new { controller = "Workflow", action = "WorkflowDesigner", },
                                new[] { "AgileEAP.Plugin.WorkflowDesigner.Controllers" });

                routes.MapLocalizedRoute("WorkflowDesigner.Default.Route",
                    "WorkflowDesigner/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new[] { "AgileEAP.Plugin.WorkflowDesigner.Controllers" });
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件eClient路由出错{0}", ex);
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