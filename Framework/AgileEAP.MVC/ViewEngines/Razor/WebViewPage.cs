#region Using...

using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Localization;
using AgileEAP.MVC.Localization;
using AgileEAP.MVC.Themes;
using AgileEAP.UI.Resources;
using AgileEAP.MVC.UI;
using AgileEAP.Core.Caching;

#endregion

namespace AgileEAP.MVC.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ScriptRegister _scriptRegister;
        private ResourceRegister _stylesheetRegister;
        private ILocalizationService _localizationService;
        private Localizer _localizer;
        private IWorkContext _workContext;

        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    //null localizer
                    //_localizer = (format, args) => new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));

                    //default localizer
                    _localizer = (format, args) =>
                                     {
                                         var resFormat = _localizationService.GetResource(format);
                                         if (string.IsNullOrEmpty(resFormat))
                                         {
                                             return new LocalizedString(format);
                                         }
                                         return
                                             new LocalizedString((args == null || args.Length == 0)
                                                                     ? resFormat
                                                                     : string.Format(resFormat, args));
                                     };
                }
                return _localizer;
            }
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }
        }

        public AgileEAPVersion Version
        {
            get;
            private set;

        }

        public override void InitHelpers()
        {
            base.InitHelpers();
            try
            {
                try
                {
                    Version = CacheManager.Get<AgileEAPVersion>("eCloud_CurrentVersion", () =>
                    {
                        //IRepository<string> repository = EngineContext.Current.Resolve<IRepository<string>>();
                        //string currentVersion = Configure.Get("CurrentVersion", "PCI");
                        //Dict dict = repository.Query<Dict>().FirstOrDefault(o => o.Name == currentVersion);// && o.ParentID == "Version");
                        //var items = repository.Query<DictItem>().Where(o => o.DictID == dict.ID).ToArray();
                        //return new AgileEAPVersion()
                        //{
                        //    ID = currentVersion,
                        //    EnglishName = (items.FirstOrDefault(o => o.Value == "EnglishName") ?? new DictItem()).Text,
                        //    eClientName = (items.FirstOrDefault(o => o.Value == "eClientName") ?? new DictItem()).Text,
                        //    eCloudName = (items.FirstOrDefault(o => o.Value == "eCloudName") ?? new DictItem()).Text,
                        //    FullName = (items.FirstOrDefault(o => o.Value == "FullName") ?? new DictItem()).Text,
                        //    ICP = (items.FirstOrDefault(o => o.Value == "ICP") ?? new DictItem()).Text,
                        //    ShortName = (items.FirstOrDefault(o => o.Value == "ShortName") ?? new DictItem()).Text,
                        //    AppName = (items.FirstOrDefault(o => o.Value == "AppName") ?? new DictItem() { Text = "eCloud" }).Text,
                        //};

                        return new AgileEAPVersion()
                            {
                                ID = "PCI",
                                EnglishName = "AgileEAP Co.,Ltd",
                                eClientName = "AgileEAP",
                                eCloudName = "AgileEAP",
                                FullName = "AgileEAP",
                                ICP = "AgileEAP",
                                ShortName = "AgileEAP",
                                AppName = "eCloud",
                            };
                    });
                }
                catch (Exception ex)
                {
                    Version = new AgileEAPVersion()
                    {
                        ID = "PCI",
                        EnglishName = "AgileEAP Co.,Ltd",
                        eClientName = "AgileEAP",
                        eCloudName = "AgileEAP",
                        FullName = "AgileEAP",
                        ICP = "AgileEAP",
                        ShortName = "AgileEAP",
                        AppName = "eCloud",
                    };
                    GlobalLogger.Info<WebViewPage<TModel>>("初步化Version失败", ex);
                }

                _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                _workContext = EngineContext.Current.Resolve<IWorkContext>();
            }
            catch (Exception ex)
            {
                GlobalLogger.Info<WebViewPage<TModel>>("初步化workContext失败", ex);
            }
        }

        public HelperResult RenderWrappedSection(string name, object wrapperHtmlAttributes)
        {
            Action<TextWriter> action = delegate(TextWriter tw)
                                {
                                    var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(wrapperHtmlAttributes);
                                    var tagBuilder = new TagBuilder("div");
                                    tagBuilder.MergeAttributes(htmlAttributes);

                                    var section = RenderSection(name, false);
                                    if (section != null)
                                    {
                                        tw.Write(tagBuilder.ToString(TagRenderMode.StartTag));
                                        section.WriteTo(tw);
                                        tw.Write(tagBuilder.ToString(TagRenderMode.EndTag));
                                    }
                                };
            return new HelperResult(action);
        }

        public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
        {
            return IsSectionDefined(sectionName) ? RenderSection(sectionName) : defaultContent(new object());
        }

        //public override string Layout
        //{
        //    get
        //    {
        //        var layout = base.Layout;

        //        if (!string.IsNullOrEmpty(layout))
        //        {
        //            var filename = System.IO.Path.GetFileNameWithoutExtension(layout);
        //            ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

        //            if (viewResult.View != null)
        //            {
        //                layout = (viewResult.View as RazorView).ViewPath;
        //            }
        //        }

        //        return layout;
        //    }
        //    set
        //    {
        //        base.Layout = value;
        //    }
        //}

        /// <summary>
        /// Return a value indicating whether the working language and theme support RTL (right-to-left)
        /// </summary>
        /// <returns></returns>
        public bool ShouldUseRtlTheme()
        {
            var supportRtl = _workContext.Language.Rtl;
            if (supportRtl)
            {
                //ensure that the active theme also supports it
                var themeProvider = EngineContext.Current.Resolve<IThemeProvider>();
                var themeContext = EngineContext.Current.Resolve<IThemeContext>();
                supportRtl = themeProvider.GetThemeConfiguration(themeContext.WorkingDesktopTheme).SupportRtl;
            }
            return supportRtl;
        }

        private string basePath = null;
        public string BasePath
        {
            get
            {
                if (string.IsNullOrEmpty(basePath))
                {
                    IWebHelper webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    string appPath = webHelper.GetLocation();
                    string viewPath = this.VirtualPath.Replace("~/", appPath);
                    var viewsPartIndex = viewPath.IndexOf("/Views", StringComparison.OrdinalIgnoreCase);
                    if (viewsPartIndex >= 0)
                    {
                        basePath = viewPath.Substring(0, viewsPartIndex);
                    }
                    else
                    {
                        basePath = appPath;// System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;// viewPath.Substring(0, viewPath.IndexOf("/"));
                    }
                }

                return basePath;
            }
        }

        public string Theme
        {
            get
            {
                return _workContext != null && !string.IsNullOrEmpty(_workContext.Theme) ? _workContext.Theme.ToLower() : "default";
            }
        }

        public ScriptRegister Script
        {
            get
            {
                return _scriptRegister ??
                    (_scriptRegister = new WebViewScriptRegister(this, Html.ViewDataContainer, ResourceManager));
            }
        }

        private IResourceManager _resourceManager;
        public IResourceManager ResourceManager
        {
            get { return _resourceManager ?? (_resourceManager = EngineContext.Current.Resolve<IResourceManager>()); }
        }

        public ResourceRegister Style
        {
            get
            {
                return _stylesheetRegister ??
                    (_stylesheetRegister = new ResourceRegister(Html.ViewDataContainer, ResourceManager, "stylesheet"));
            }
        }

        class CaptureScope : IDisposable
        {
            readonly WebPageBase _viewPage;
            readonly Action<IHtmlString> _callback;

            public CaptureScope(WebPageBase viewPage, Action<IHtmlString> callback)
            {
                _viewPage = viewPage;
                _callback = callback;
                _viewPage.OutputStack.Push(new HtmlStringWriter());
            }

            void IDisposable.Dispose()
            {
                var writer = (HtmlStringWriter)_viewPage.OutputStack.Pop();
                _callback(writer);
            }
        }

        class WebViewScriptRegister : ScriptRegister
        {
            private readonly WebPageBase _viewPage;

            public WebViewScriptRegister(WebPageBase viewPage, IViewDataContainer container, IResourceManager resourceManager)
                : base(container, resourceManager)
            {
                _viewPage = viewPage;
            }

            public override IDisposable Head()
            {
                return new CaptureScope(_viewPage, s => ResourceManager.RegisterHeadScript(s.ToString()));
            }

            public override IDisposable Foot()
            {
                return new CaptureScope(_viewPage, s => ResourceManager.RegisterFootScript(s.ToString()));
            }
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}