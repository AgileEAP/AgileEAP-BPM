namespace AgileEAP.Core.Plugins
{
    public abstract class BasePlugin : IPluginInstaller
    {
        protected BasePlugin()
        {
        }

        /// <summary>
        /// 插件管理菜单
        /// </summary>
        public const string PluginManagerMenu = "System_Plugin_Manager";

        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        public virtual PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(this.PluginDescriptor.SystemName);
            PluginDescriptor.Installed = true;
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(this.PluginDescriptor.SystemName);
            PluginDescriptor.Installed = false;
        }

    }
}
