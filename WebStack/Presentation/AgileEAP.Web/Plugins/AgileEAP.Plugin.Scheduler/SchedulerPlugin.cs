using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AgileEAP.Core.Plugins;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Localization;
using AgileEAP.Plugin.Scheduler.Quartz;

namespace AgileEAP.Plugin.Scheduler
{
    public class SchedulerPlugin : BasePlugin
    {
        QuartzClient quartzClient = new QuartzClient();

        public override void Install()
        {
            base.Install();
            quartzClient.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();

            quartzClient.UnInstall();
        }
    }
}