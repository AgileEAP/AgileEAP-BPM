using Autofac;
using Autofac.Integration.Mvc;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Infrastructure.DependencyManagement;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Plugin.Authorize
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 2; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<AuthorizeService>().As<IAuthorizeService>().InstancePerHttpRequest();
        }
    }
}
