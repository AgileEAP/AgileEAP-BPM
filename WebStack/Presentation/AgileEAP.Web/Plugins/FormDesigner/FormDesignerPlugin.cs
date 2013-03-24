using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AgileEAP.Core.Plugins;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Localization;

namespace AgileEAP.Plugin.FormDesigner
{
    public class FormDesignerPlugin : BasePlugin
    {
        public override void Install()
        {
            MenuProvider menuProvider = new MenuProvider();
            menuProvider.BuildMenu(PluginManagerMenu, PluginDescriptor);

            base.Install();
        }

        public override void Uninstall()
        {
            MenuProvider menuProvider = new MenuProvider();
            menuProvider.DeleteMenu(PluginDescriptor);

            base.Uninstall();
        }
    }
}