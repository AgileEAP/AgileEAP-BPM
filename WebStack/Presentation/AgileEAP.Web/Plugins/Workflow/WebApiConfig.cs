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
using AgileEAP.Workflow.Engine;

namespace AgileEAP.Plugin.Workflow
{
    public class WebApiConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<TokenAuthentication>().As<ITokenAuthentication>().SingleInstance();
            builder.RegisterType<WorkflowEngine>().As<IWorkflowEngine>().InstancePerApiRequest();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.MessageHandlers.Add(new WebApiAuthenticationHandler(container.Resolve<ITokenAuthentication>()));
            config.DependencyResolver = resolver;
            //  builder.RegisterWebApiFilterProvider(config);
            config.Routes.MapHttpRoute(name: "Workflow.Web.Api",
                routeTemplate: "workflow/api/{action}",
                defaults: new { controller = "WorkflowApi", id = RouteParameter.Optional });

            //删除了
            TraceConfig.Register(config);
        }
    }

    public static class TraceConfig
    {
        /// <summary>
        /// Registers the <see cref="ITraceWriter"/> implementation to use
        /// for this application.
        /// </summary>
        /// <param name="configuration">The <see cref="HttpConfiguration"/> in which
        /// to register the trace writer.</param>
        public static void Register(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            SystemDiagnosticsTraceWriter traceWriter =
                new SystemDiagnosticsTraceWriter()
                {
                    MinimumLevel = TraceLevel.Info,
                    IsVerbose = false
                };

            configuration.Services.Replace(typeof(ITraceWriter), traceWriter);
        }
    }
}
