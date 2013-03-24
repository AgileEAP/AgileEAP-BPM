using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Plugins;
using AgileEAP.Web.Models.Plugin;

using Kendo.Mvc.UI;


namespace AgileEAP.Web.Controllers
{
    public class PluginController : BaseController
    {
        private readonly IPluginFinder pluginFinder;
        private readonly IWebHelper webHelper;

        public PluginController(IPluginFinder pluginFinder, IWebHelper webHelper, IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {
            this.pluginFinder = pluginFinder;
            this.webHelper = webHelper;
        }

        #region Utilities

        PluginModel ToPluginModel(PluginDescriptor pluginDescriptor)
        {
            PluginModel model = new PluginModel()
            {
                Author = pluginDescriptor.Author,
                FriendlyName = pluginDescriptor.FriendlyName,
                DisplayOrder = pluginDescriptor.DisplayOrder,
                SystemName = pluginDescriptor.SystemName,
                Group = pluginDescriptor.Group,
                Installed = pluginDescriptor.Installed,
                Version = pluginDescriptor.Version,
                ConfigurationUrl = pluginDescriptor.ConfigurationUrl,
                IsEnabled = pluginDescriptor.IsEnabled,
                CanChangeEnabled = pluginDescriptor.CanChangeEnabled,
            };

            return model;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Plugin_Filter([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<PluginModel> data = pluginFinder.GetPluginDescriptors(false).Select(p => ToPluginModel(p)).OrderBy(o => o.Group).ThenBy(o => o.SystemName);
            var result = new DataSourceResult()
            {
                Data = data,
                Total = data.Count()
            };

            return Json(result);
        }

        public ActionResult Install(string plugin)
        {
            string message = string.Format("插件{0}安装成功", plugin);
            try
            {
                var pluginDescriptor = pluginFinder.GetPluginDescriptors(false)
                    .Where(x => x.SystemName.Equals(plugin, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                if (pluginDescriptor != null && !pluginDescriptor.Installed)
                {
                    pluginDescriptor.Instance().Install();
                    //webHelper.RestartAppDomain();
                }
            }
            catch (Exception ex)
            {
                message = string.Format("插件{0}安装出错", plugin);
                log.Error(message, ex);
            }

            return Json(message);
        }

        public ActionResult Uninstall(string plugin)
        {
            string message = string.Format("插件{0}卸载装成功", plugin);
            try
            {
                var pluginDescriptor = pluginFinder.GetPluginDescriptors(false)
                    .Where(x => x.SystemName.Equals(plugin, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                if (pluginDescriptor != null && pluginDescriptor.Installed)
                {
                    pluginDescriptor.Instance().Uninstall();
                    //webHelper.RestartAppDomain();
                }
            }
            catch (Exception ex)
            {
                message = string.Format("插件{0}卸载装出错", plugin);
                log.Error(message, ex);
            }

            return Json(message);
        }

        public ActionResult Refresh()
        {
            //restart application
            webHelper.RestartAppDomain();
            return Redirect("/Plugin/index");
        }

        public ActionResult Import()
        {
            return View();
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Delete(string id)
        {
            var filename = id;
            var filePath = Path.Combine(Server.MapPath("~/TempFileDirectory/Plugins"), filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Download(string id)
        {
            var filename = id;
            var filePath = Path.Combine(Server.MapPath("~/TempFileDirectory/Plugins"), filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var r = new List<ViewDataUploadFilesResult>();
            string tmpPath = Server.MapPath("~/TempFileDirectory/Plugins");
            string installPath = Server.MapPath("~/Plugins");
            string thumbnailPath = Server.MapPath("~/Content/Images/plugin.png");
            if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);
            var statuses = new List<ViewDataUploadFilesResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fullPath = Path.Combine(tmpPath, Path.GetFileName(file.FileName));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                file.SaveAs(fullPath);

                ZipFile.ExtractToDirectory(fullPath, Path.Combine(installPath, Path.GetFileNameWithoutExtension(file.FileName)));

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Plugin/Download/" + file.FileName,
                    delete_url = "/Plugin/Delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(thumbnailPath),
                    delete_type = "GET",
                });
            }
            return Json(statuses);
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }


        public class ViewDataUploadFilesResult
        {
            public string name { get; set; }
            public int size { get; set; }
            public string type { get; set; }
            public string url { get; set; }
            public string delete_url { get; set; }
            public string thumbnail_url { get; set; }
            public string delete_type { get; set; }
        }
        #endregion
    }
}