using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Tracing;

using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using AgileEAP.Core;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Localization;
using AgileEAP.MVC.WebApi;

namespace AgileEAP.Web
{
    public class WebApiConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            //var builder = new ContainerBuilder();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //builder.RegisterType<TokenAuthentication>().As<ITokenAuthentication>().SingleInstance();

            //var container = builder.Build();
            //var resolver = new AutofacWebApiDependencyResolver(container);
            //config.MessageHandlers.Add(new WebApiAuthenticationHandler(container.Resolve<ITokenAuthentication>()));
            //config.DependencyResolver = resolver;
            ////  builder.RegisterWebApiFilterProvider(config);
            //config.Routes.MapHttpRoute(name: "AgileEAP.Web.Api",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
           // TraceConfig.Register(config);
        }
    }
}
