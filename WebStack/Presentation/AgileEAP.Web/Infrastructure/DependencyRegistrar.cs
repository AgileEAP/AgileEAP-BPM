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
using AgileEAP.Core.Localization;
using AgileEAP.Web.Controllers;
using AgileEAP.MVC.EmbeddedViews;
using AgileEAP.MVC.Routes;
using AgileEAP.MVC.Themes;
using AgileEAP.MVC.UI;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AuthorizeService>().As<IAuthorizeService>().InstancePerHttpRequest();
            builder.RegisterType<UtilService>().As<IUtilService>().InstancePerHttpRequest();
            //we cache presentation models between requests
            //builder.RegisterType<CatalogController>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("AgileEAP_cache_static"));
            //builder.RegisterType<TopicController>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("AgileEAP_cache_static"));
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
