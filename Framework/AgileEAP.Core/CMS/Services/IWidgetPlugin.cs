using System.Collections.Generic;
using System.Web.Routing;
using AgileEAP.Core.Plugins;

namespace AgileEAP.Core.Cms
{
    /// <summary>
    /// Provides an interface for creating tax providers
    /// </summary>
    public partial interface IWidgetPlugin : IPluginInstaller
    {
        /// <summary>
        /// Get a list of supported widget zones; if empty list is returned, then all zones are supported
        /// </summary>
        /// <returns>A list of supported widget zones</returns>
        IList<WidgetZone> SupportedWidgetZones();

        /// <summary>
        /// Gets a route for plugin configuration
        /// </summary>
        /// <param name="widgetId">Widget identifier</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        void GetConfigurationRoute(string widgetId, out string actionName, out string controllerName, out RouteValueDictionary routeValues);
        

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetId">Widget identifier</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        void GetDisplayWidgetRoute(string widgetId, out string actionName, out string controllerName, out RouteValueDictionary routeValues);
    }
}
