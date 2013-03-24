using System;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using StackExchange.Profiling;//MvcMiniProfiler;
using StackExchange.Profiling.MVCHelpers;
//using MvcMiniProfiler.MVCHelpers;
using AgileEAP.Core;
using AgileEAP.Core.Web;
using AgileEAP.Core.Data;
using AgileEAP.Core.Domain;
using AgileEAP.Core.Infrastructure;
using AgileEAP.MVC;
using AgileEAP.MVC.EmbeddedViews;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Themes;

namespace AgileEAP.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : AgileEAPApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //do not register HandleErrorAttribute. use classic error handling mode
            //filters.Add(new HandleErrorAttribute());

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("{*pathInfo}.exe");

            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "AgileEAP.Web.Controllers" }
            );
        }

        protected override void Start()
        {
            base.Start();

            MvcHandler.DisableMvcResponseHeader = true;
            //initialize engine context
            EngineContext.Initialize(false);

            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            //start scheduled tasks
            //if (databaseInstalled)
            //{
            //    TaskManager.Instance.Initialize();
            //    TaskManager.Instance.Start();
            //}

            //set dependency resolver
            var dependencyResolver = new AgileEAPDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);
            //AgileEAPControllerFactory agileEAPControllerFactory = new MVC.AgileEAPControllerFactory();
            //ControllerBuilder.Current.SetControllerFactory(agileEAPControllerFactory);

            //model binders
            ModelBinders.Binders.Add(typeof(AgileEAPModel), new AgileEAPModelBinder());

            if (databaseInstalled)
            {
                //remove all view engines
                ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
            }

            //Add some functionality on top of the default ModelMetadataProvider
            ModelMetadataProviders.Current = new AgileEAPMetadataProvider();

            //Registering some regular mvc stuf
            AreaRegistration.RegisterAllAreas();
            //if (databaseInstalled &&
            //    EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerInPublicStore)
            //{
            //    GlobalFilters.Filters.Add(new ProfilingActionFilter());
            //}
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AgileEAPValidatorFactory()));

            //register virtual path provider for embedded views
            var embeddedViewResolver = EngineContext.Current.Resolve<IEmbeddedViewResolver>();
            var embeddedProvider = new EmbeddedViewVirtualPathProvider(embeddedViewResolver.GetEmbeddedViews());
            HostingEnvironment.RegisterVirtualPathProvider(embeddedProvider);

            if (databaseInstalled)
            {
                //if (EngineContext.Current.Resolve<StoreInformationSettings>().MobileDevicesSupported)
                //{
                //    //Enable the mobile detection provider (if enabled)
                //    HttpCapabilitiesBase.BrowserCapabilitiesProvider = new FiftyOne.Foundation.Mobile.Detection.MobileCapabilitiesProvider();
                //}
                //else
                {
                    //set BrowserCapabilitiesProvider to null because 51Degrees assembly always sets it to MobileCapabilitiesProvider
                    //it'll allow us to use default browserCaps.config file
                    HttpCapabilitiesBase.BrowserCapabilitiesProvider = null;
                }
            }

            WebApiConfig.Register();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //  EnsureDatabaseIsInstalled();

            //if (DataSettingsHelper.DatabaseIsInstalled()) //&& 
            ////  EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerInPublicStore)
            //{
            //    try
            //    {
            //        MiniProfiler.Start();
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(ex);
            //    }
            //}
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //if (DataSettingsHelper.DatabaseIsInstalled())// &&
            ////EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerInPublicStore)
            //{
            //    //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
            //    MiniProfiler.Stop();
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //we don't do it in Application_BeginRequest because a user is not authenticated yet
            SetWorkingCulture();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //disable compression (if enabled). More info - http://stackoverflow.com/questions/3960707/asp-net-mvc-weird-characters-in-error-page
            CompressAttribute.DisableCompression(HttpContext.Current);
            //log error
            LogException(Server.GetLastError());
        }

        protected void EnsureDatabaseIsInstalled()
        {
            //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //string installUrl = string.Format("{0}install", webHelper.GetLocation());
            //if (!webHelper.IsStaticResource(this.Request) &&
            //    !DataSettingsHelper.DatabaseIsInstalled() &&
            //    !webHelper.GetThisPageUrl(false).StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    this.Response.Redirect(installUrl);
            //}
        }

        protected void SetWorkingCulture()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                try
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    if (!webHelper.IsStaticResource(this.Request))
                    {
                        //public store
                        var workContext = EngineContext.Current.Resolve<IWorkContext>();
                        if (workContext.User != null && workContext.Language != null)
                        {
                            var culture = new CultureInfo(workContext.Language.LanguageCulture);
                            Thread.CurrentThread.CurrentCulture = culture;
                            Thread.CurrentThread.CurrentUICulture = culture;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalLogger.Error<MvcApplication>("set SetWorkingCulture error", ex);
                }
            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            try
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}