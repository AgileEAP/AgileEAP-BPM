using System;
using System.Web.Routing;
using AgileEAP.Core.Plugins;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Localization;

namespace AgileEAP.Plugin.Workflow
{
    /// <summary>
    /// Workflow Plugin 
    /// </summary>
    public class WorkflowPlugin : BasePlugin
    {
        public WorkflowPlugin()
        {
        }



        public override void Install()
        {
            base.Install();

            WebApiConfig.Register();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }

    }
}
