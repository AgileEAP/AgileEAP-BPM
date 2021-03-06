using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Features.Metadata;
using AgileEAP.Environment.Extensions.Models;

namespace AgileEAP.UI.Resources
{
    public class ResourceManager : IResourceManager, IUnitOfWorkDependency
    {
        private readonly Dictionary<Tuple<String, String>, RequireSettings> _required = new Dictionary<Tuple<String, String>, RequireSettings>();
        private readonly List<LinkEntry> _links = new List<LinkEntry>();
        private readonly Dictionary<string, MetaEntry> _metas = new Dictionary<string, MetaEntry> {
            { "generator", new MetaEntry { Content = "AgileEAP", Name = "generator" } }
        };
        private readonly Dictionary<string, IList<ResourceRequiredContext>> _builtResources = new Dictionary<string, IList<ResourceRequiredContext>>(StringComparer.OrdinalIgnoreCase);
        private readonly IEnumerable<IResourceManifestProvider> _providers;
        private ResourceManifest _dynamicManifest;
        private List<String> _headScripts;
        private List<String> _footScripts;
        private IEnumerable<IResourceManifest> _manifests;

        private const string NotIE = "!IE";

        private static string ToAppRelativePath(string resourcePath)
        {
            if (!String.IsNullOrEmpty(resourcePath) && !Uri.IsWellFormedUriString(resourcePath, UriKind.Absolute))
            {
                resourcePath = VirtualPathUtility.ToAppRelative(resourcePath);
            }
            return resourcePath;
        }

        private static string FixPath(string resourcePath, string relativeFromPath)
        {
            if (!String.IsNullOrEmpty(resourcePath) && !VirtualPathUtility.IsAbsolute(resourcePath) && !Uri.IsWellFormedUriString(resourcePath, UriKind.Absolute))
            {
                // appears to be a relative path (e.g. 'foo.js' or '../foo.js', not "/foo.js" or "http://..")
                if (String.IsNullOrEmpty(relativeFromPath))
                {
                    throw new InvalidOperationException("ResourcePath cannot be relative unless a base relative path is also provided.");
                }
                resourcePath = VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(relativeFromPath, resourcePath));
            }
            return resourcePath;
        }

        private static TagBuilder GetTagBuilder(ResourceDefinition resource, string url)
        {
            var tagBuilder = new TagBuilder(resource.TagName);
            tagBuilder.MergeAttributes(resource.TagBuilder.Attributes);
            if (!String.IsNullOrEmpty(resource.FilePathAttributeName))
            {
                if (!String.IsNullOrEmpty(url))
                {
                    if (VirtualPathUtility.IsAppRelative(url))
                    {
                        url = VirtualPathUtility.ToAbsolute(url);
                    }
                    tagBuilder.MergeAttribute(resource.FilePathAttributeName, url, true);
                }
            }
            return tagBuilder;
        }

        public static void WriteResource(TextWriter writer, ResourceDefinition resource, string url, string condition, Dictionary<string, string> attributes)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                if (condition == NotIE)
                {
                    writer.WriteLine("<!--[if " + condition + "]>-->");
                }
                else
                {
                    writer.WriteLine("<!--[if " + condition + "]>");
                }
            }

            var tagBuilder = GetTagBuilder(resource, url);

            if (attributes != null)
            {
                // todo: try null value
                tagBuilder.MergeAttributes(attributes, true);
            }

            writer.WriteLine(tagBuilder.ToString(resource.TagRenderMode));

            if (!string.IsNullOrEmpty(condition))
            {
                if (condition == NotIE)
                {
                    writer.WriteLine("<!--<![endif]-->");
                }
                else
                {
                    writer.WriteLine("<![endif]-->");
                }
            }
        }

        public ResourceManager(IEnumerable<IResourceManifestProvider> resourceProviders)
        {
            _providers = resourceProviders;
        }

        public IEnumerable<IResourceManifest> ResourceProviders
        {
            get
            {
                if (_manifests == null)
                {
                    var builder = new ResourceManifestBuilder();
                    foreach (var provider in _providers)
                    {
                        Type providerType = provider.GetType();
                        string pluginName = string.Empty;// providerType.Namespace.Substring(16);
                        string location = string.Empty;// string.Format("~/Plugins/{0}", pluginName);
                        if (providerType.Namespace == "AgileEAP.Web")
                        {
                            pluginName = "AgileEAP.Web";
                            location = "~/";
                        }
                        else
                        {
                            pluginName = providerType.Namespace.Substring(16);
                            location = string.Format("~/Plugins/{0}", pluginName);
                        }

                        builder.Feature = new Feature()
                        {
                            Descriptor = new FeatureDescriptor()
                            {
                                Id = pluginName,
                                Name = pluginName,
                                Extension = new ExtensionDescriptor()
                                {
                                    Location = location
                                }
                            }
                        };

                        try
                        {
                            provider.BuildManifests(builder);
                        }
                        catch (Exception ex)
                        {
                            AgileEAP.Core.GlobalLogger.Error<ResourceManager>(ex);
                        }
                    }
                    _manifests = builder.ResourceManifests;
                }
                return _manifests;
            }
        }

        public virtual ResourceManifest DynamicResources
        {
            get
            {
                return _dynamicManifest ?? (_dynamicManifest = new ResourceManifest());
            }
        }

        public virtual RequireSettings Require(string resourceType, string resourceName, int group)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            if (resourceName == null)
            {
                throw new ArgumentNullException("resourceName");
            }
            RequireSettings settings;
            var key = new Tuple<string, string>(resourceType, resourceName);
            if (!_required.TryGetValue(key, out settings))
            {
                settings = new RequireSettings { Type = resourceType, Name = resourceName, Group = group };
                _required[key] = settings;
            }
            _builtResources[resourceType] = null;
            return settings;
        }

        public virtual RequireSettings Include(string resourceType, string resourcePath, string resourceDebugPath, int group)
        {
            return Include(resourceType, resourcePath, null, null, group);
        }

        public virtual RequireSettings Include(string resourceType, string resourcePath, string resourceDebugPath, string relativeFromPath, int group)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            if (resourcePath == null)
            {
                throw new ArgumentNullException("resourcePath");
            }

            if (VirtualPathUtility.IsAppRelative(resourcePath))
            {
                // ~/ ==> convert to absolute path (e.g. /AgileEAP/..)
                resourcePath = VirtualPathUtility.ToAbsolute(resourcePath);
            }
            resourcePath = FixPath(resourcePath, relativeFromPath);
            resourceDebugPath = FixPath(resourceDebugPath, relativeFromPath);
            return Require(resourceType, ToAppRelativePath(resourcePath), group).Define(d => d.SetUrl(resourcePath, resourceDebugPath));
        }

        public virtual void RegisterHeadScript(string script)
        {
            if (_headScripts == null)
            {
                _headScripts = new List<string>();
            }
            _headScripts.Add(script);
        }

        public virtual void RegisterFootScript(string script)
        {
            if (_footScripts == null)
            {
                _footScripts = new List<string>();
            }
            _footScripts.Add(script);
        }

        public virtual void NotRequired(string resourceType, string resourceName)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            if (resourceName == null)
            {
                throw new ArgumentNullException("resourceName");
            }
            var key = new Tuple<string, string>(resourceType, resourceName);
            _builtResources[resourceType] = null;
            _required.Remove(key);
        }

        public virtual ResourceDefinition FindResource(RequireSettings settings)
        {
            return FindResource(settings, true);
        }

        private ResourceDefinition FindResource(RequireSettings settings, bool resolveInlineDefinitions)
        {
            // find the resource with the given type and name
            // that has at least the given version number. If multiple,
            // return the resource with the greatest version number.
            // If not found and an inlineDefinition is given, define the resource on the fly
            // using the action.
            var name = settings.Name ?? "";
            var type = settings.Type;
            var resource = (from p in ResourceProviders
                            from r in p.GetResources(type)
                            where name.Equals(r.Key, StringComparison.OrdinalIgnoreCase)
                            orderby r.Value.Version descending
                            select r.Value).FirstOrDefault();
            if (resource == null && _dynamicManifest != null)
            {
                resource = (from r in _dynamicManifest.GetResources(type)
                            where name.Equals(r.Key, StringComparison.OrdinalIgnoreCase)
                            orderby r.Value.Version descending
                            select r.Value).FirstOrDefault();
            }
            if (resolveInlineDefinitions && resource == null)
            {
                // Does not seem to exist, but it's possible it is being
                // defined by a Define() from a RequireSettings somewhere.
                if (ResolveInlineDefinitions(settings.Type))
                {
                    // if any were defined, now try to find it
                    resource = FindResource(settings, false);
                }
            }
            return resource;
        }

        private bool ResolveInlineDefinitions(string resourceType)
        {
            bool anyWereDefined = false;
            foreach (var settings in GetRequiredResources(resourceType).Where(settings => settings.InlineDefinition != null))
            {
                // defining it on the fly
                var resource = FindResource(settings, false);
                if (resource == null)
                {
                    // does not already exist, so define it
                    resource = DynamicResources.DefineResource(resourceType, settings.Name).SetBasePath(settings.BasePath);
                    anyWereDefined = true;
                }
                settings.InlineDefinition(resource);
                settings.InlineDefinition = null;
            }
            return anyWereDefined;
        }

        public virtual IEnumerable<RequireSettings> GetRequiredResources(string type)
        {
            List<RequireSettings> result = new List<RequireSettings>();// = _required.Where(r => r.Key.Item1 == type).Select(r => r.Value);
            try
            {
                IDictionary<int, List<RequireSettings>> group = new Dictionary<int, List<RequireSettings>>();
                foreach (var item in _required)
                {
                    if (item.Key.Item1 == type)
                    {
                        if (!group.ContainsKey(item.Value.Group))
                            group[item.Value.Group] = new List<RequireSettings>();
                        group[item.Value.Group].Add(item.Value);
                    }
                }

                foreach (var g in group.Reverse())
                {
                    result.AddRange(g.Value);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                result = _required.Where(r => r.Key.Item1 == type).Select(r => r.Value).ToList();
            }

            return result;
        }

        public virtual IList<LinkEntry> GetRegisteredLinks()
        {
            return _links.AsReadOnly();
        }

        public virtual IList<MetaEntry> GetRegisteredMetas()
        {
            return _metas.Values.ToList().AsReadOnly();
        }

        public virtual IList<String> GetRegisteredHeadScripts()
        {
            return _headScripts == null ? null : _headScripts.AsReadOnly();
        }

        public virtual IList<String> GetRegisteredFootScripts()
        {
            return _footScripts == null ? null : _footScripts.AsReadOnly();
        }

        public virtual IList<ResourceRequiredContext> BuildRequiredResources(string resourceType)
        {
            IList<ResourceRequiredContext> requiredResources;
            if (_builtResources.TryGetValue(resourceType, out requiredResources) && requiredResources != null)
            {
                return requiredResources;
            }
            var allResources = new OrderedDictionary();
            foreach (var settings in GetRequiredResources(resourceType))
            {
                var resource = FindResource(settings);
                if (resource == null)
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "A '{1}' named '{0}' could not be found.", settings.Name, settings.Type));
                }
                ExpandDependencies(resource, settings, allResources);
            }
            requiredResources = (from DictionaryEntry entry in allResources
                                 select new ResourceRequiredContext { Resource = (ResourceDefinition)entry.Key, Settings = (RequireSettings)entry.Value }).ToList();
            _builtResources[resourceType] = requiredResources;
            return requiredResources;
        }

        public virtual IList<ResourceRequiredContext> FindRequiredResources(RequireSettings settings)
        {
            IList<ResourceRequiredContext> requiredResources;
            string key = settings.Type + settings.Name;
            if (_builtResources.TryGetValue(key, out requiredResources) && requiredResources != null)
            {
                return requiredResources;
            }
            var allResources = new OrderedDictionary();

            var resource = FindResource(settings);
            if (resource == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "A '{1}' named '{0}' could not be found.", settings.Name, settings.Type));
            }
            ExpandDependencies(resource, settings, allResources);
            requiredResources = (from DictionaryEntry entry in allResources
                                 select new ResourceRequiredContext { Resource = (ResourceDefinition)entry.Key, Settings = (RequireSettings)entry.Value }).ToList();
            _builtResources[key] = requiredResources;

            return requiredResources;
        }

        public virtual IDictionary<string, IList<ResourceRequiredContext>> BuildRequiredGroupResources(string resourceType, ResourceLocation? includeLocation)
        {
            IDictionary<string, IList<ResourceRequiredContext>> requiredGroupResources = new Dictionary<string, IList<ResourceRequiredContext>>(StringComparer.OrdinalIgnoreCase);
            IList<ResourceRequiredContext> requiredResources;

            foreach (var settings in GetRequiredResources(resourceType).Where(r => includeLocation.HasValue ? r.Location == includeLocation.Value : true))
            {
                if (_builtResources.TryGetValue(resourceType, out requiredResources) && requiredResources != null)
                {
                    if (!requiredGroupResources.ContainsKey(settings.Name))
                        requiredGroupResources.Add(settings.Name, requiredResources);
                }
                else
                {
                    var resource = FindResource(settings);
                    if (resource == null)
                    {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "A '{1}' named '{0}' could not be found.", settings.Name, settings.Type));
                    }

                    var allResources = new OrderedDictionary();
                    ExpandDependencies(resource, settings, allResources);

                    requiredResources = (from DictionaryEntry entry in allResources
                                         select new ResourceRequiredContext { Resource = (ResourceDefinition)entry.Key, Settings = (RequireSettings)entry.Value }).ToList();
                    _builtResources[settings.Name] = requiredResources;
                    requiredGroupResources.Add(settings.Name, requiredResources);
                }
            }

            return requiredGroupResources;
        }

        protected virtual void ExpandDependencies(ResourceDefinition resource, RequireSettings settings, OrderedDictionary allResources)
        {
            if (resource == null)
            {
                return;
            }
            // Settings is given so they can cascade down into dependencies. For example, if Foo depends on Bar, and Foo's required
            // location is Head, so too should Bar's location.
            // forge the effective require settings for this resource
            // (1) If a require exists for the resource, combine with it. Last settings in gets preference for its specified values.
            // (2) If no require already exists, form a new settings object based on the given one but with its own type/name.
            settings = allResources.Contains(resource)
                ? ((RequireSettings)allResources[resource]).Combine(settings)
                : new RequireSettings { Type = resource.Type, Name = resource.Name }.Combine(settings);
            if (resource.Dependencies != null)
            {
                var dependencies = from d in resource.Dependencies
                                   select FindResource(new RequireSettings { Type = resource.Type, Name = d });
                foreach (var dependency in dependencies)
                {
                    if (dependency == null)
                    {
                        continue;
                    }
                    ExpandDependencies(dependency, settings, allResources);
                }
            }
            allResources[resource] = settings;
        }

        public void RegisterLink(LinkEntry link)
        {
            _links.Add(link);
        }

        public void SetMeta(MetaEntry meta)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
            {
                return;
            }
            _metas[meta.Name] = meta;
        }

        public void AppendMeta(MetaEntry meta, string contentSeparator)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
            {
                return;
            }
            MetaEntry existingMeta;
            if (_metas.TryGetValue(meta.Name, out existingMeta))
            {
                meta = MetaEntry.Combine(existingMeta, meta, contentSeparator);
            }
            _metas[meta.Name] = meta;
        }

    }
}
