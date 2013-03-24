using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NHibernate;
using NHibernate.Engine;

using AgileEAP.Core.Caching;
using AgileEAP.Core.ExceptionHandler;

namespace AgileEAP.Core.Data
{
    public class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static ILogger logger = LogManager.GetLogger(typeof(DatabaseManager));
        private static object lockObj = new object();

        private readonly IList<Database> databases = new List<Database>();

        private DatabaseManager() { }

        /// <summary>
        /// The singleton Instance of this class
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        lock (lockObj)
                        {
                            Initialize(MultiDatabaseCfg.Configuration);
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// All the existing persistant Units in this application
        /// </summary>
        public IList<Database> Databases
        {
            get { return databases; }
        }

        public static void Initialize(IMultiDatabaseCfg configuration)
        {
            try
            {
                instance = new DatabaseManager();
                foreach (IDatabaseCfg pus in configuration.DatabaseCfgs)
                {
                    instance.Databases.Add(new Database(pus, configuration.Configurator));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        internal Database GetDatabase(System.Type t)
        {
            if (Databases.Count == 0)
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        Initialize(MultiDatabaseCfg.Configuration);
                    }
                }

                if (Databases.Count == 0) throw new EAPException("系统数据库数量为0，初始化数据库时可能出错，请检查");
            }

            if (Databases.Count == 1)
            {
                return Databases[0];
            }

            Database db = CacheManager.GetData<Database>(t.FullName);
            if (db != null) return db;

            foreach (Database pu in databases)
            {
                ISessionFactoryImplementor sfi = (ISessionFactoryImplementor)pu.SessionFactory;
                if (sfi.GetClassMetadata(t) != null)
                {
                    CacheManager.Add(t.FullName, pu, 43200);
                    return pu;
                }
            }

            throw new EAPException("Persistence Unit cannot be found for " + t);
        }

        public Database GetDatabase(string name)
        {
            foreach (Database unit in databases)
                if (unit.Name == name)
                    return unit;
            throw new ArgumentException("Cannot find persistant unit  named " + name);
        }

        public Database GetDatabase()
        {
            if (databases == null || databases.Count == 0)
                throw new EAPException(
                    "Unable to get default database without an entity type when there are no database.");

            foreach (Database database in databases)
            {
                if (database.Name.Equals(MultiDatabaseCfg.Configuration.DefaultDatabase, StringComparison.OrdinalIgnoreCase))
                    return database;
            }
            return databases[0];
        }

        public static void ResetInstance()
        {
            instance = null;
        }
    }
}
