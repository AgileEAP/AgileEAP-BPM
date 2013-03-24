using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AgileEAP.Core.Infrastructure;

namespace AgileEAP.UI.Resources
{
    public interface IResourcesPublisher
    {
        IEnumerable<IResourceManifestProvider> RegisterResources();
    }

    public class ResourcesPublisher : IResourcesPublisher
    {
        private readonly ITypeFinder _typeFinder;

        public ResourcesPublisher(ITypeFinder typeFinder)
        {
            this._typeFinder = typeFinder;
        }

        public IEnumerable<IResourceManifestProvider> RegisterResources()
        {
            var resourceManifestProviderTypes = _typeFinder.FindClassesOfType<IResourceManifestProvider>();
            var resourceManifestProviders = new List<IResourceManifestProvider>();
            foreach (var providerType in resourceManifestProviderTypes)
            {
                var provider = Activator.CreateInstance(providerType) as IResourceManifestProvider;
                resourceManifestProviders.Add(provider);
            }

            return resourceManifestProviders;
        }
    }
}
