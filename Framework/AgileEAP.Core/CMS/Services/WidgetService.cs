using System;
using System.Collections.Generic;
using System.Linq;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;
using AgileEAP.Core.Plugins;

namespace AgileEAP.Core.Cms
{
    /// <summary>
    /// Widget service
    /// </summary>
    public partial class WidgetService : IWidgetService
    {
        #region Constants
        private const string WIDGETS_BY_ID_KEY = "AgileEAP.widget.id-{0}";
        private const string WIDGETS_ALL_KEY = "AgileEAP.widget.all";
        private const string WIDGETS_PATTERN_KEY = "AgileEAP.widget.";

        #endregion

        #region Fields

        private readonly IRepository<string> repository = new Repository<string>();
        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="eventPublisher">Event published</param>
        public WidgetService(IPluginFinder pluginFinder)
        {
            _pluginFinder = pluginFinder;
        }

        #endregion

        #region Methods



        /// <summary>
        /// Load widget plugin provider by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found widget plugin</returns>
        public virtual IWidgetPlugin LoadWidgetPluginBySystemName(string systemName)
        {
            var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IWidgetPlugin>(systemName);
            if (descriptor != null)
                return descriptor.Instance<IWidgetPlugin>();

            return null;
        }

        /// <summary>
        /// Load all widget plugins
        /// </summary>
        /// <returns>widget plugins</returns>
        public virtual IList<IWidgetPlugin> LoadAllWidgetPlugins()
        {
            return _pluginFinder.GetPlugins<IWidgetPlugin>().ToList();
        }

        /// <summary>
        /// Delete widget
        /// </summary>
        /// <param name="widget">Widget</param>
        public virtual void DeleteWidget(Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException("widget");

            repository.Delete<Widget>(widget);

            CacheManager.Remove(WIDGETS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets all widgets
        /// </summary>
        /// <returns>Widgets</returns>
        public virtual IList<Widget> GetAllWidgets()
        {
            string key = WIDGETS_ALL_KEY;
            return CacheManager.Get(key, () =>
            {
                var query = from w in repository.Query<Widget>()
                            orderby w.DisplayOrder
                            select w;
                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all widgets by zone
        /// </summary>
        /// <param name="widgetZone">Widget zone</param>
        /// <returns>Widgets</returns>
        public virtual IList<Widget> GetAllWidgetsByZone(WidgetZone widgetZone)
        {
            var allWidgets = GetAllWidgets();
            var widgets = allWidgets
                .Where(w => w.WidgetZone == widgetZone)
                .OrderBy(w => w.DisplayOrder)
                .ToList();
            return widgets;
        }

        /// <summary>
        /// Gets a widget
        /// </summary>
        /// <param name="widgetId">Widget identifier</param>
        /// <returns>Widget</returns>
        public virtual Widget GetWidgetById(string widgetId)
        {
            if (string.IsNullOrEmpty(widgetId))
                return null;

            string key = string.Format(WIDGETS_BY_ID_KEY, widgetId);
            return CacheManager.Get(key, () =>
            {
                var widget = repository.Query<Widget>().FirstOrDefault(o => o.ID == widgetId);
                return widget;
            });
        }

        /// <summary>
        /// Inserts widget
        /// </summary>
        /// <param name="widget">Widget</param>
        public virtual void InsertWidget(Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException("widget");

            repository.SaveOrUpdate(widget);

            //cache
            CacheManager.Remove(WIDGETS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the widget
        /// </summary>
        /// <param name="widget">Widget</param>
        public virtual void UpdateWidget(Widget widget)
        {
            if (widget == null)
                throw new ArgumentNullException("widget");

            repository.Update(widget);

            //cache
            CacheManager.Remove(WIDGETS_PATTERN_KEY);

        }

        #endregion
    }
}
