using System;
using System.Collections.Generic;

namespace AgileEAP.UI.Resources
{
    public interface IResourceManager : IDependency
    {
        IEnumerable<RequireSettings> GetRequiredResources(string type);
        IList<ResourceRequiredContext> BuildRequiredResources(string resourceType);
        IList<ResourceRequiredContext> FindRequiredResources(RequireSettings settings);
        IDictionary<string, IList<ResourceRequiredContext>> BuildRequiredGroupResources(string resourceType, ResourceLocation? includeLocation);
        IList<LinkEntry> GetRegisteredLinks();
        IList<MetaEntry> GetRegisteredMetas();
        IList<String> GetRegisteredHeadScripts();
        IList<String> GetRegisteredFootScripts();
        IEnumerable<IResourceManifest> ResourceProviders { get; }
        ResourceManifest DynamicResources { get; }
        ResourceDefinition FindResource(RequireSettings settings);
        void NotRequired(string resourceType, string resourceName);
        RequireSettings Include(string resourceType, string resourcePath, string resourceDebugPath, int group = 0);
        RequireSettings Include(string resourceType, string resourcePath, string resourceDebugPath, string relativeFromPath, int group = 0);
        RequireSettings Require(string resourceType, string resourceName, int group = 0);
        void RegisterHeadScript(string script);
        void RegisterFootScript(string script);
        void RegisterLink(LinkEntry link);
        void SetMeta(MetaEntry meta);
        void AppendMeta(MetaEntry meta, string contentSeparator);
    }
}
