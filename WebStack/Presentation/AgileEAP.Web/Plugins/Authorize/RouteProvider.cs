using System;
using System.Web.Routing;
using AgileEAP.Core;
using AgileEAP.MVC.Routes;

namespace AgileEAP.Plugin.Authorize
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                //    routes.MapPageRoute("Authorize.RoleManager", "Authorize/RoleManager", "~/Plugins/Authorize/Pages/RoleManager.aspx");
                //    routes.MapPageRoute("Authorize.ResourceManager", "Authorize/ResourceManager", "~/Plugins/Authorize/Pages/ResourceManager.aspx");
                routes.Add("AuthorizeRoute", new System.Web.Routing.Route("AuthorizeCenter/{page}/{*path}", routeHandler: new PluginRouteHandler()));
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件Authorize路由出错{0}", ex);
            }
        }
        public int Priority
        {
            get
            {
                return 11;
            }
        }
    }
}
