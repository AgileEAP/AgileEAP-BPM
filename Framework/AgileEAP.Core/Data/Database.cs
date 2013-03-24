using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Utility;
using NHibernate;


namespace AgileEAP.Core.Data
{
    /// <summary>
    /// A persistant Unit is a unit of a ORM management
    /// </summary>
    /// <remarks>
    /// it consists of a Database, a NHibernate SessionManager, a Nhibernate SessionFactory.
    /// It can be shared by multiple domain layer assemblies 
    /// It's heavy weight
    /// </remarks>
    public class Database
    {
        private readonly IDatabaseCfg configuration;
        private readonly NHibernate.Cfg.Configuration nHConfiguration;
        private ISessionFactory sessionFactory;
        private ILogger logger = LogManager.GetLogger(typeof(Database));

        internal Database(IDatabaseCfg cfg, IConfigurator configurator)
        {
            try
            {
                configuration = cfg;
                nHConfiguration = CreateNHConfiguration();
                if (configurator != null)
                    configurator.Config(cfg, nHConfiguration);

                ReBuildSessionfactory();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("初始化数据库{0}出错", cfg.Name), ex);
                throw ex;
            }
        }

        /// <summary>
        /// the name of the PU
        /// </summary>
        /// <remarks>
        /// Set at the configuration File
        /// </remarks>
        public string Name
        {
            get { return configuration.Name; }
        }

        /// <summary>
        /// The configuration section that sets this Persistence Unit in the configuration file
        /// </summary>
        /// <remarks>
        /// This class stored the setting information associated with this PU
        /// </remarks>
        public IDatabaseCfg Configuration
        {
            get { return configuration; }
        }

        internal ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

        /// <summary>
        /// Get a managed Session
        /// </summary>
        /// <returns></returns>
        public ISession Session
        {
            get
            {
                string safeKey = string.Format("{0}{1}", Name, Thread.CurrentThread.ManagedThreadId.ToString());

                ISession session = ApplicationContext.Current.Sessions.GetSafeValue<ISession>(safeKey);
                if (session == null)
                {
                    session = SessionFactory.OpenSession();
                    ApplicationContext.Current.Sessions.SafeAdd(safeKey, session);
                }
                else if (!session.IsConnected)
                    session.Reconnect();
                return session;
            }
        }

        /// <summary>
        /// The nhibernate configuration of this session Manager
        /// </summary>
        public NHibernate.Cfg.Configuration NHConfiguration
        {
            get { return nHConfiguration; }
        }

        /// <summary>
        /// Rebuild the Session factory
        /// </summary>
        /// <remarks>
        /// in case you need to change the NHConfiguration on the fly
        /// </remarks>
        public void ReBuildSessionfactory()
        {
            try
            {
                sessionFactory = nHConfiguration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                logger.Error("Nhibernate BuildSessionfactory Error", ex);
                throw;
            }
        }

        public static Database Instance(System.Type t)
        {
            return DatabaseManager.Instance.GetDatabase(t);
        }

        ///<summary>
        /// Create a NHibernate Configuration
        ///</summary>
        ///<returns></returns>
        private NHibernate.Cfg.Configuration CreateNHConfiguration()
        {
            NHibernate.Cfg.Configuration retVal = new NHibernate.Cfg.Configuration();
            if (!string.IsNullOrEmpty(configuration.NHConfigFile))
            {
                string configFile = configuration.NHConfigFile;
                if (!Path.IsPathRooted(configFile))
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    configFile = configuration.NHConfigFile.StartsWith("~") ? configuration.NHConfigFile.Replace("~", baseDirectory) : Path.Combine(baseDirectory, configuration.NHConfigFile);
                }

                try
                {
                    retVal.Configure(configFile);
                }
                catch (Exception ex)
                {
                    logger.Error("NH Configure Error!", ex);
                }
            }
            return retVal;
        }
    }
}