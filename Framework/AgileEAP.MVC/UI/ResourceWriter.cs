using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Optimization;

using AgileEAP.UI.Resources;
using AgileEAP.Core;

namespace AgileEAP.MVC.UI
{
    public class ResourceWriter : IResourceWriter
    {
        private HttpContextBase httpContext;
        private IResourceManager resourceManager;
        private IWorkContext workContext;
        public ResourceWriter(HttpContextBase httpContext, IWorkContext workContext, IResourceManager resourceManager)
        {
            this.httpContext = httpContext;
            this.resourceManager = resourceManager;
            this.workContext = workContext;
        }

        public void HeadScripts(TextWriter Output)
        {
            WriteResources(Output, "script", ResourceLocation.Head, null);
            WriteLiteralScripts(Output, resourceManager.GetRegisteredHeadScripts());
        }

        public void FootScripts(TextWriter Output)
        {
            WriteResources(Output, "script", ResourceLocation.Foot, null);
            WriteLiteralScripts(Output, resourceManager.GetRegisteredFootScripts());
        }

        public void Metas(TextWriter Output)
        {
            foreach (var meta in resourceManager.GetRegisteredMetas())
            {
                Output.WriteLine(meta.GetTag());
            }
        }

        public void HeadCss(TextWriter Output)
        {
            HeadLinks(Output);
            StylesheetLinks(Output);
        }

        public void HeadLinks(TextWriter Output)
        {
            foreach (var link in resourceManager.GetRegisteredLinks())
            {
                Output.WriteLine(link.GetTag());
            }
        }

        public void StylesheetLinks(TextWriter Output)
        {
            WriteResources(Output, "stylesheet", null, null);
        }

        public void Style(HtmlHelper Html, TextWriter Output, ResourceDefinition Resource, string Url, string Condition, Dictionary<string, string> TagAttributes)
        {
            // do not write to Output directly as Styles are rendered in Zones
            ResourceManager.WriteResource(Html.ViewContext.Writer, Resource, Url, Condition, TagAttributes);
        }

        public void Resource(TextWriter Output, ResourceDefinition Resource, string Url, string Condition, Dictionary<string, string> TagAttributes)
        {
            ResourceManager.WriteResource(Output, Resource, Url, Condition, TagAttributes);
        }

        private static void WriteLiteralScripts(TextWriter output, IEnumerable<string> scripts)
        {
            if (scripts == null)
            {
                return;
            }
            foreach (string script in scripts)
            {
                output.WriteLine(script);
            }
        }

        private void WriteResources(TextWriter Output, string resourceType, ResourceLocation? includeLocation, ResourceLocation? excludeLocation)
        {
            bool debugMode = workContext.IsDebug;
            var defaultSettings = new RequireSettings
            {
                DebugMode = debugMode,
                Culture = CultureInfo.CurrentUICulture.Name,
            };
            var appPath = httpContext.Request.ApplicationPath;

            if (debugMode == true)
            {
                #region debugMode
                var requiredResources = resourceManager.BuildRequiredResources(resourceType);
                foreach (var context in requiredResources.Where(r =>
                    (includeLocation.HasValue ? r.Settings.Location == includeLocation.Value : true) &&
                    (excludeLocation.HasValue ? r.Settings.Location != excludeLocation.Value : true)))
                {

                    var path = context.GetResourceUrl(defaultSettings, appPath);
                    var condition = context.Settings.Condition;
                    var attributes = context.Settings.HasAttributes ? context.Settings.Attributes : null;
                    //IHtmlString result;
                    //if (resourceType == "stylesheet")
                    //{
                    //    result = Style(Url: path, Condition: condition, Resource: context.Resource, TagAttributes: attributes);
                    //}
                    //else
                    //{
                    //    result = Resource(Url: path, Condition: condition, Resource: context.Resource, TagAttributes: attributes);
                    //}
                    ResourceManager.WriteResource(Output, context.Resource, path, condition, attributes);

                    // Output.Write(result);
                }
                #endregion
            }
            else
            {
                BundleCollection bundles = BundleTable.Bundles;
                var requiredGroupResources = resourceManager.BuildRequiredGroupResources(resourceType, includeLocation);
                foreach (var requiredGroupResource in requiredGroupResources)
                {
                    var disableCompressResources = requiredGroupResource.Value.Where(o => o.Resource.EnableCompress == false).ToArray();
                    if (disableCompressResources != null)
                    {
                        foreach (var context in disableCompressResources)
                        {
                            var path = context.GetResourceUrl(defaultSettings, appPath);
                            var condition = context.Settings.Condition;
                            var attributes = context.Settings.HasAttributes ? context.Settings.Attributes : null;
                            ResourceManager.WriteResource(Output, context.Resource, path, condition, attributes);
                        }
                    }
                    string virtualPath = string.Format("~/{0}/{1}", resourceType, requiredGroupResource.Key);
                    bool hasValue = BundleResolver.Current.IsBundleVirtualPath(virtualPath);
                    if (!hasValue)
                    {
                        Bundle bundle = null;
                        if (resourceType == "stylesheet")
                        {
                            bundle = new StyleBundle(virtualPath);
                        }
                        else
                        {
                            bundle = new ScriptBundle(virtualPath);
                        }


                        foreach (var context in requiredGroupResource.Value)
                        {
                            if (context.Resource.EnableCompress)
                            {
                                var bundlePath = "~" + context.GetResourceUrl(defaultSettings, appPath);
                                bundle.Include(bundlePath);
                                hasValue = true;
                            }
                        }

                        if (hasValue)
                            bundles.Add(bundle);
                    }

                    if (hasValue)
                    {
                        ResourceManager.WriteResource(Output, requiredGroupResource.Value[0].Resource, bundles.ResolveBundleUrl(virtualPath), null, null);
                    }
                }
            }
        }

        public void WriteResources(TextWriter Output, string resourceType, string resourceName)
        {
            bool debugMode = workContext.IsDebug;
            var settings = new RequireSettings
            {
                DebugMode = debugMode,
                Culture = CultureInfo.CurrentUICulture.Name,
                Type = resourceType,
                Name = resourceName
            };
            var appPath = httpContext.Request.ApplicationPath;

            var requiredResources = resourceManager.FindRequiredResources(settings);
            if (debugMode == true)
            {
                foreach (var context in requiredResources)
                {
                    var path = context.GetResourceUrl(settings, appPath);
                    var condition = context.Settings.Condition;
                    var attributes = context.Settings.HasAttributes ? context.Settings.Attributes : null;
                    ResourceManager.WriteResource(Output, context.Resource, path, condition, attributes);
                }
            }
            else
            {

                var disableCompressResources = requiredResources.Where(o => o.Resource.EnableCompress == false).ToArray();
                if (disableCompressResources != null)
                {
                    foreach (var context in disableCompressResources)
                    {
                        var path = context.GetResourceUrl(settings, appPath);
                        var condition = context.Settings.Condition;
                        var attributes = context.Settings.HasAttributes ? context.Settings.Attributes : null;
                        ResourceManager.WriteResource(Output, context.Resource, path, condition, attributes);
                    }
                }

                BundleCollection bundles = BundleTable.Bundles;
                string virtualPath = string.Format("~/{0}/{1}", resourceType, resourceName);
                if (!BundleResolver.Current.IsBundleVirtualPath(virtualPath))
                {
                    Bundle bundle = null;
                    if (resourceType == "stylesheet")
                    {
                        bundle = new StyleBundle(virtualPath);
                    }
                    else
                    {
                        bundle = new ScriptBundle(virtualPath);
                    }

                    foreach (var context in requiredResources)
                    {
                        if (context.Resource.EnableCompress)
                        {
                            var bundlePath = "~" + context.GetResourceUrl(settings, appPath);
                            bundle.Include(bundlePath);
                        }
                    }

                    bundles.Add(bundle);
                }

                ResourceManager.WriteResource(Output, requiredResources[0].Resource, bundles.ResolveBundleUrl(virtualPath), null, null);

            }
        }

    }
}
