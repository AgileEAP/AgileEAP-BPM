using System;
using System.Web.Routing;
using AgileEAP.Core;
using AgileEAP.MVC.Routes;

namespace AgileEAP.Plugin.Administration
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.Add("AdministrationRoute", new System.Web.Routing.Route("Infrastructure/{page}/{*path}", routeHandler: new PluginRouteHandler()));
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件Administration路由出错{0}", ex);
            }
        }
        public int Priority
        {
            get
            {
                return 10;
            }
        }
    }
}
