using System.Collections.Generic;
using System.Web.Mvc;

namespace AgileEAP.MVC.Themes
{
    public class ThemeableRazorViewEngine : ThemeableBuildManagerViewEngine
    {
        public ThemeableRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
                                          {                                            
                                              //default
                                              "~/Areas/{2}/Views/{1}/{0}.cshtml", 
                                              //"~/Areas/{2}/Views/{1}/{0}.vbhtml", 
                                              "~/Areas/{2}/Views/Shared/{0}.cshtml", 
                                              //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                          };

            AreaMasterLocationFormats = new[]
                                            {
                                                //default
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml", 
                                                //"~/Areas/{2}/Views/{1}/{0}.vbhtml", 
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml", 
                                                //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                            };

            AreaPartialViewLocationFormats = new[]
                                                 {                                                   
                                                    //default
                                                    "~/Areas/{2}/Views/{1}/{0}.cshtml", 
                                                   // "~/Areas/{2}/Views/{1}/{0}.vbhtml", 
                                                    "~/Areas/{2}/Views/Shared/{0}.cshtml", 
                                                    //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                                 };

            ViewLocationFormats = new[]
                                      {
                                            ////themes
                                            //"~/Content/Themes/{3}/Views/{1}/{0}.cshtml", 
                                            ////"~/Themes/{2}/Views/{1}/{0}.vbhtml", 
                                            //"~/Content/Themes/{3}/Views/Shared/{0}.cshtml",
                                            ////"~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                            //default
                                            "~/Views/{1}/{0}.cshtml", 
                                            //"~/Views/{1}/{0}.vbhtml", 
                                            "~/Views/Shared/{0}.cshtml",
                                            //"~/Views/Shared/{0}.vbhtml",
                                      };

            MasterLocationFormats = new[]
                                        {
                                            ////themes
                                            //"~/Content/Themes/{3}/Views/{1}/{0}.cshtml", 
                                            ////"~/Themes/{2}/Views/{1}/{0}.vbhtml", 
                                            //"~/Content/Themes/{3}/Views/Shared/{0}.cshtml", 
                                            ////"~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                            //default
                                            "~/Views/{1}/{0}.cshtml", 
                                            //"~/Views/{1}/{0}.vbhtml", 
                                            "~/Views/Shared/{0}.cshtml", 
                                            //"~/Views/Shared/{0}.vbhtml"
                                        };

            PartialViewLocationFormats = new[]
                                             {
                                                // //themes
                                                //"~/Content/Themes/{3}/Views/{1}/{0}.cshtml", 
                                                ////"~/Themes/{2}/Views/{1}/{0}.vbhtml", 
                                                //"~/Content/Themes/{3}/Views/Shared/{0}.cshtml", 
                                                ////"~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                                //default
                                                "~/Views/{1}/{0}.cshtml", 
                                                //"~/Views/{1}/{0}.vbhtml", 
                                                "~/Views/Shared/{0}.cshtml", 
                                                //"~/Views/Shared/{0}.vbhtml",
                                             };

            FileExtensions = new[] { "cshtml" };//, "vbhtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            var runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions);
            //return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            var runViewStartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions);
        }
    }
}
