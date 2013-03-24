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

namespace AgileEAP.Plugin.OpenApi
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            try
            {
            
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<RouteProvider>("初始化插件AgileEAP.Plugin.OpenApi路由出错{0}", ex);
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