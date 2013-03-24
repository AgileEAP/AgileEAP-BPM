#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NHibernate;
using NHibernate.Cfg;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;

namespace AgileEAP.Core
{
    public class UnitOfWork
    {
        public static object objLock = new object();

        private UnitOfWork() { }

        /// <summary>
        /// overloaded version of <see cref="GetSession(System.Type)"/> in a single-Database environment
        /// </summary>
        /// <returns></returns>
        public static ISession GetSession()
        {
            return DataBaseManager.GetDatabase().Session;
        }

        /// <summary>
        /// Gets a managed ISession
        /// </summary>
        /// <param name="entityType">
        /// The entity type whose mapping is included in the SessionFactory,
        /// when there are multiple databases, Burrow use this to locate the right one
        /// </param>
        /// <returns>The Burrow managed ISession</returns>
        /// <remarks>
        /// Please do not try to close or commit transaction of this session as its status and transaction are controlled by Burrow.
        /// To get an unmanaged session please use GetSessionFactory()
        /// To setup the interceptor for every managed ISession for a persistent Unit, <see cref="IPersistenceUnitCfg.InterceptorFactory"/>
        /// </remarks>
        public static ISession GetSession(System.Type entityType)
        {
            return DataBaseManager.GetDatabase(entityType).Session;
        }

        /// <summary>
        /// Get database connection string
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetConnectionString(Type entityType)
        {
            return DataBaseManager.GetDatabase(entityType).NHConfiguration.Properties["connection.connection_string"];
        }


        private static DatabaseType GetDatabaseType(string driver)
        {
            DatabaseType databaseType = DatabaseType.MsSQL2008;

            if (driver.EndsWith("OracleClientDriver"))
            {
                databaseType = DatabaseType.Oracle;
            }
            else if (driver.EndsWith("MySqlDataDriver"))
            {
                databaseType = DatabaseType.MySQL;
            }

            return databaseType;
        }


        public static DatabaseType CurrentDatabaseType
        {
            get
            {
                Database database = DataBaseManager.GetDatabase();
                string driver = database.NHConfiguration.Properties["connection.driver_class"];
                return GetDatabaseType(driver);
            }
        }

        public static EAConnection GetEAConnection(Type entityType)
        {
            Database database = DataBaseManager.GetDatabase(entityType);
            string driver = database.NHConfiguration.Properties["connection.driver_class"];
            DatabaseType databaseType = GetDatabaseType(driver);
            NHibernate.Engine.ISessionFactoryImplementor sessionFactory = database.SessionFactory as NHibernate.Engine.ISessionFactoryImplementor;

            return new EAConnection()
            {
                ConnectionString = database.NHConfiguration.Properties[NHibernate.Cfg.Environment.ConnectionString],
                DatabaseType = databaseType,
                DataDriver = driver,
                DbConnection = sessionFactory.ConnectionProvider.GetConnection()
            };
        }

        public static EAConnection GetEAConnection()
        {
            Database database = DataBaseManager.GetDatabase();
            string driver = database.NHConfiguration.Properties["connection.driver_class"];
            DatabaseType databaseType = GetDatabaseType(driver);

            NHibernate.Engine.ISessionFactoryImplementor sessionFactory = database.SessionFactory as NHibernate.Engine.ISessionFactoryImplementor;
            return new EAConnection()
            {
                ConnectionString = database.NHConfiguration.Properties["connection.connection_string"],
                DatabaseType = databaseType,
                DataDriver = driver,
                DbConnection = sessionFactory.ConnectionProvider.GetConnection()
            };
        }

        /// <summary>
        /// Gets a managed ISession
        /// </summary>
        /// <param name="persistenceUnitName"></param>
        /// <returns></returns>
        /// <remarks>an overload of <see cref="GetSession(Type)"/> that can be used when you have one entityType mapped in multiple persistenceUnits</remarks>
        public static ISession GetSession(string persistenceUnitName)
        {
            return DataBaseManager.GetDatabase(persistenceUnitName).Session;
        }

        /// <summary>
        /// Gets the ISessionFactory
        /// </summary>
        /// <param name="entityType">the entity type whose mapping is included in the SessionFactory, 
        /// when there are multiple databases, Burrow use this to locate the right one</param>
        /// <returns>the sessionFactory</returns>
        /// <remarks>
        /// For getting a Session please use <see cref="GetSession()"/> as it's managed by Burrow. 
        /// If you use OpenSession() of this SessionFactory, 
        /// the session you get won't be managed by Burrow 
        /// and you will be responsible for managing the status of that session yourself 
        /// </remarks>
        public static ISessionFactory GetSessionFactory(System.Type entityType)
        {
            return DataBaseManager.GetDatabase(entityType).SessionFactory;
        }
        /// <summary>
        /// Gets the ISessionFactory
        /// </summary>
        /// <remarks>an overload of <see cref="GetSessionFactory(Type)"/> that can be used when you have one entityType mapped in multiple persistenceUnits</remarks>
        public static ISessionFactory GetSessionFactory(string persistenceUnitName)
        {
            return DataBaseManager.GetDatabase(persistenceUnitName).SessionFactory;
        }

        internal static DatabaseManager DataBaseManager
        {
            get
            {
                return DatabaseManager.Instance;
            }
        }

        /// <summary>
        /// 开始数据库事物
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns></returns>
        public static Transaction BeginTransaction(string database)
        {
            string cacheKey = string.Format("{0}Transaction{1}", database, Thread.CurrentThread.ManagedThreadId.ToString());

            Transaction trans = ApplicationContext.Current.Sessions.GetSafeValue<Transaction>(cacheKey);
            if (trans == null)
            {
                trans = new Transaction();
                ApplicationContext.Current.Sessions.SafeAdd(cacheKey, trans);
            }

            trans.BeginTransaction(database);

            return trans;
        }

        /// <summary>
        /// 开始数据库事物
        /// </summary>
        /// <param name="domainType">实体对象类型</param>
        /// <returns></returns>
        public static Transaction BeginTransaction(Type domainType)
        {
            return BeginTransaction(DataBaseManager.GetDatabase(domainType).Name);
        }

        /// <summary>
        /// 按事物执行方法
        /// </summary>
        /// <param name="action"></param>
        public static void ExecuteWithTrans<T>(Action action)
        {
            ITransaction trans = BeginTransaction(typeof(T));

            try
            {
                action();

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
        }

        /// <summary>
        /// 按事物执行方法
        /// </summary>
        /// <param name="func"></param>
        public static TResult ExecuteWithTrans<T, TResult>(Func<TResult> func)
        {
            TResult result = default(TResult);
            ITransaction trans = BeginTransaction(typeof(T));

            try
            {
                result = func();

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }

            return result;
        }

    }
}
