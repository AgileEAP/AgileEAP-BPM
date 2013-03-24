using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Data;
using AgileEAP.Core.Events;
using AgileEAP.Core.Fakes;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Infrastructure.DependencyManagement;
using AgileEAP.Core.Plugins;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Authentication.External;
using AgileEAP.Core.Cms;
using AgileEAP.Core.Mobile;
using AgileEAP.Core.Localization;
using AgileEAP.Core.Security;
using AgileEAP.MVC.EmbeddedViews;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Themes;
using AgileEAP.MVC.UI;
using AgileEAP.UI.Resources;

namespace AgileEAP.MVC
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            //HttpContextBase hcb = new HttpContextWrapper(HttpContext.Current) as HttpContextBase;
            //WebHelper a = new WebHelper(hcb);
            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //data layer
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest();

            //cache manager
            //builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("AgileEAP_cache_static").SingleInstance();
            //builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("AgileEAP_cache_per_request").InstancePerHttpRequest();


            //work context
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerHttpRequest();
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            builder.RegisterType<MobileDeviceHelper>().As<IMobileDeviceHelper>().InstancePerHttpRequest();

            // builder.RegisterGeneric(typeof(ConfigurationProvider<>)).As(typeof(IConfigurationProvider<>));
            // builder.RegisterSource(new SettingsSource());

            //services
            //pass MemoryCacheManager to LocalizationService as cacheManager (cache locales between requests)
            //builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerHttpRequest();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerHttpRequest();

            //pass MemoryCacheManager to LocalizedEntityService as cacheManager (cache locales between requests)
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>().InstancePerHttpRequest();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerHttpRequest();


            builder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerHttpRequest();
            builder.RegisterType<PageTitleBuilder>().As<IPageTitleBuilder>().InstancePerHttpRequest();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerHttpRequest();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerHttpRequest();


            builder.RegisterType<ExternalAuthorizer>().As<IExternalAuthorizer>().InstancePerHttpRequest();
            builder.RegisterType<OpenAuthenticationService>().As<IOpenAuthenticationService>().InstancePerHttpRequest();


            builder.RegisterType<EmbeddedViewResolver>().As<IEmbeddedViewResolver>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.Register(c => new Logger("AgilEAP")).As<ILogger>().SingleInstance();

            builder.RegisterSource(new SettingsSource());
            //builder.RegisterSource(new WorkRegistrationSource());

            builder.RegisterType<ResourceManager>().As<IResourceManager>().InstancePerLifetimeScope();
            builder.RegisterType<ResourceWriter>().As<IResourceWriter>().InstancePerLifetimeScope();

            //var resourceManifestProviders = typeFinder.FindClassesOfType(typeof(IResourceManifestProvider)).ToList();
            builder.RegisterAssemblyTypes(typeFinder.GetAssemblies().ToArray())
              .AssignableTo<IResourceManifestProvider>()
              .AsImplementedInterfaces();

            // builder.RegisterInstance(resourceManifestProviders).As<IEnumerable<IResourceManifestProvider>>().SingleInstance();
            //builder.Register(c => typeFinder.FindClassesOfType(typeof(IResourceManifestProvider))).As<IEnumerable<IResourceManifestProvider>>().SingleInstance();
            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerHttpRequest();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) => c.Resolve<IConfigurationProvider<TSettings>>().Settings)
                .InstancePerHttpRequest()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

    public class Work<T> where T : class
    {
        private readonly Func<Work<T>, T> _resolve;

        public Work(Func<Work<T>, T> resolve)
        {
            _resolve = resolve;
        }

        public T Value
        {
            get { return _resolve(this); }
        }
    }


    class WorkValues<T> where T : class
    {
        public WorkValues(IComponentContext componentContext)
        {
            ComponentContext = componentContext;
            Values = new Dictionary<Work<T>, T>();
        }

        public IComponentContext ComponentContext { get; private set; }
        public IDictionary<Work<T>, T> Values { get; private set; }
    }

    /// <summary>
    /// Support the <see cref="Meta{T}"/>
    /// types automatically whenever type T is registered with the container.
    /// Metadata values come from the component registration's metadata.
    /// </summary>
    class WorkRegistrationSource : IRegistrationSource
    {
        static readonly MethodInfo CreateMetaRegistrationMethod = typeof(WorkRegistrationSource).GetMethod(
            "CreateMetaRegistration", BindingFlags.Static | BindingFlags.NonPublic);

        private static bool IsClosingTypeOf(Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var swt = service as IServiceWithType;
            if (swt == null || !IsClosingTypeOf(swt.ServiceType, typeof(Work<>)))
                return Enumerable.Empty<IComponentRegistration>();

            var valueType = swt.ServiceType.GetGenericArguments()[0];

            var valueService = swt.ChangeType(valueType);

            var registrationCreator = CreateMetaRegistrationMethod.MakeGenericMethod(valueType);

            return registrationAccessor(valueService)
                .Select(v => registrationCreator.Invoke(null, new object[] { service, v }))
                .Cast<IComponentRegistration>();
        }

        public bool IsAdapterForIndividualComponents
        {
            get { return true; }
        }

        static IComponentRegistration CreateMetaRegistration<T>(Service providedService, IComponentRegistration valueRegistration) where T : class
        {
            var rb = RegistrationBuilder.ForDelegate(
                (c, p) =>
                {
                    //var workContextAccessor = EngineContext.Current;// c.Resolve<IWorkContextAccessor>();
                    return new Work<T>(w =>
                    {
                        var workContext = EngineContext.Current;// workContextAccessor.GetContext();
                        if (workContext == null)
                            return default(T);

                        var workValues = workContext.Resolve<WorkValues<T>>();

                        T value;
                        if (!workValues.Values.TryGetValue(w, out value))
                        {
                            value = (T)workValues.ComponentContext.ResolveComponent(valueRegistration, p);
                            workValues.Values[w] = value;
                        }
                        return value;
                    });
                })
                .As(providedService)
                .Targeting(valueRegistration);

            return rb.CreateRegistration();
        }
    }

}
