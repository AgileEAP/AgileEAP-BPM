using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AgileEAP.Core.Data
{
    public interface IConfigurator
    {
        void Config(IMultiDatabaseCfg cfg);
        void Config(IDatabaseCfg dbCfg, NHibernate.Cfg.Configuration nhCfg);
    }
}
