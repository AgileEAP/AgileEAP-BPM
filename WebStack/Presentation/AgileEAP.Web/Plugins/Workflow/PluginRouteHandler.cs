using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Net;
using System.Web.Security;
using System.Web.Compilation;

namespace AgileEAP.Plugin.Workflow
{
    public class PluginRouteHandler : IRouteHandler
    {
        public PluginRouteHandler() { }

        public IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            string page = (string)requestContext.RouteData.Values["page"];
            string virtualPath = string.Format("~/Plugins/Workflow/Pages/{0}.aspx", page);

            bool shouldValidate = false;

            if (shouldValidate && !UrlAuthorizationModule.CheckUrlAccessForPrincipal(
                virtualPath, requestContext.HttpContext.User,
                              requestContext.HttpContext.Request.HttpMethod))
            {
                requestContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                requestContext.HttpContext.Response.End();
                return null;
            }
            else
            {
               // HttpContext.Current.RewritePath(virtualPath);
                HttpContext.Current.Items.Add("page", page);

                if (virtualPath.EndsWith(".aspx"))
                    return (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page));
                //else
                //{
                //    var asmxPos = virtualPath.IndexOf(".asmx", StringComparison.OrdinalIgnoreCase);
                //    if (asmxPos >= 0)
                //    {
                //        // What goes here?  This isn't working...
                //        var asmxOnlyVirtualPath = virtualPath.Substring(0, asmxPos + 5);
                //        return new System.Web.Services.Protocols.WebServiceHandlerFactory().GetHandler(
                //            HttpContext.Current, HttpContext.Current.Request.HttpMethod, asmxOnlyVirtualPath, HttpContext.Current.Server.MapPath(asmxOnlyVirtualPath));
                //    }
                //    else
                //        return new StaticRoute();
                //}

                requestContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                requestContext.HttpContext.Response.End();
                return null;
            }
        }
    }
}