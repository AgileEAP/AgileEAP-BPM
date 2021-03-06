using System.Collections.Generic;

namespace AgileEAP.Core.Cms
{
    /// <summary>
    /// Widget service interface
    /// </summary>
    public partial interface IWidgetService
    {
        /// <summary>
        /// Load widget plugin provider by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found widget plugin</returns>
        IWidgetPlugin LoadWidgetPluginBySystemName(string systemName);

        /// <summary>
        /// Load all widget plugins
        /// </summary>
        /// <returns>widget plugins</returns>
        IList<IWidgetPlugin> LoadAllWidgetPlugins();



        /// <summary>
        /// Delete widget
        /// </summary>
        /// <param name="widget">Widget</param>
        void DeleteWidget(Widget widget);

        /// <summary>
        /// Gets all widgets
        /// </summary>
        /// <returns>Widgets</returns>
        IList<Widget> GetAllWidgets();

        /// <summary>
        /// Gets all widgets
        /// </summary>
        /// <param name="widgetZone">Widget zone</param>
        /// <returns>Widgets</returns>
        IList<Widget> GetAllWidgetsByZone(WidgetZone widgetZone);

        /// <summary>
        /// Gets a widget
        /// </summary>
        /// <param name="widgetId">Widget identifier</param>
        /// <returns>Widget</returns>
        Widget GetWidgetById(string widgetId);

        /// <summary>
        /// Inserts widget
        /// </summary>
        /// <param name="widget">Widget</param>
        void InsertWidget(Widget widget);

        /// <summary>
        /// Updates the widget
        /// </summary>
        /// <param name="widget">Widget</param>
        void UpdateWidget(Widget widget);
    }
}
