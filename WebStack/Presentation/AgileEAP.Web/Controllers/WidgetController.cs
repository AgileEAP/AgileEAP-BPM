using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AgileEAP.Core.Data;
using AgileEAP.Core.Cms;
using AgileEAP.MVC.Controllers;
using AgileEAP.Web.Models.Cms;
using AgileEAP.Core;
namespace AgileEAP.Web.Controllers
{
    public class WidgetController : BaseController
    {
        #region Fields
        IWidgetService widgetService;
        #endregion

        #region Constructors

        public WidgetController(IWorkContext workContext, IRepository<string> repository, IWidgetService widgetService)
            : base(workContext, repository)
        {
            this.widgetService = widgetService;
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public ActionResult WidgetsByZone(WidgetZone widgetZone)
        {
            //model
            var model = new List<WidgetModel>();

            var widgets = widgetService.GetAllWidgetsByZone(widgetZone);
            foreach (var widget in widgets)
            {
                var widgetPlugin = widgetService.LoadWidgetPluginBySystemName(widget.PluginSystemName);
                if (widgetPlugin == null || !widgetPlugin.PluginDescriptor.Installed)
                    continue;   //don't throw an exception. just process next widget.

                var widgetModel = new WidgetModel();

                string actionName;
                string controllerName;
                RouteValueDictionary routeValues;
                widgetPlugin.GetDisplayWidgetRoute(widget.ID, out actionName, out controllerName, out routeValues);
                widgetModel.ActionName = actionName;
                widgetModel.ControllerName = controllerName;
                widgetModel.RouteValues = routeValues;

                model.Add(widgetModel);
            }

            return PartialView(model);
        }

        #endregion
    }
}
