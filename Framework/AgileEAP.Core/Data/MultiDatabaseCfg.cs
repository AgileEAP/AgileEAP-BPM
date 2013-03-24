using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileEAP.Core.Data;
using AgileEAP.Core.Data.Config;

namespace AgileEAP.Core.Data
{
    public class MultiDatabaseCfg
    {
        private static IMultiDatabaseCfg configuration;
        public static IMultiDatabaseCfg Configuration
        {
            get
            {
                if (configuration == null) configuration = Config(Configurator);

                return configuration;
            }
        }

        public static IConfigurator Configurator
        {
            get;
            set;
        }

        private static IMultiDatabaseCfg Config(IConfigurator configurator)
        {
            IMultiDatabaseCfg cfg = MultiDatabaseSection.CreateInstance();
            if (configurator != null)
                cfg.Configurator = configurator;

            if (cfg.Configurator != null)
                cfg.Configurator.Config(cfg);

            return cfg;
        }
    }
}
