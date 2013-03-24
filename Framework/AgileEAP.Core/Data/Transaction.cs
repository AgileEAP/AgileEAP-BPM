using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using NHibernate;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;

namespace AgileEAP.Core
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }

    public class Transaction : ITransaction
    {
        private bool commited = false;
        ILogger logger = LogManager.GetLogger(typeof(Transaction));
        private object objLock = new object();
        private readonly Queue<string> innerQueue = new Queue<string>();

        private Queue<string> activateDatabases = new Queue<string>();
        public Queue<string> ActivateDatabases
        {
            get
            {
                if (activateDatabases == null)
                {
                    lock (objLock)
                    {
                        if (activateDatabases == null)
                            activateDatabases = new Queue<string>();
                    }
                }
                return activateDatabases;
            }
            set
            {
                activateDatabases = value;
            }
        }

        internal Transaction()
        {
            if (innerQueue == null)
                innerQueue = new Queue<string>();
        }

        /// <summary>
        /// 开始数据库事物
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <returns></returns>
        internal bool BeginTransaction(string database)
        {
            ISession session = UnitOfWork.GetSession(database);
            if (session != null && session.Transaction != null && !session.Transaction.IsActive)
            {
                session.Transaction.Begin();
                logger.DebugFormat("database {0} begin transaction success", database);
            }

            ActivateDatabases.Enqueue(database);
            return true;
        }

        /// <summary>
        /// 提交事物
        /// </summary>
        public void Commit()
        {
            if (activateDatabases.Count > 0)
                innerQueue.Enqueue(ActivateDatabases.Dequeue());

            if (activateDatabases.Count == 0)
            {
                lock (objLock)
                {
                    try
                    {
                        foreach (var database in innerQueue.Distinct())
                        {

                            ISession session = UnitOfWork.GetSession(database);
                            if (session != null && session.Transaction != null && session.Transaction.IsActive)
                                session.Transaction.Commit();
                            logger.DebugFormat("database {0} commit transaction success", database);
                        }

                        commited = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public void Rollback()
        {
            lock (objLock)
            {
                foreach (var database in innerQueue.Distinct().Reverse())
                {
                    try
                    {
                        ISession session = UnitOfWork.GetSession(database);
                        if (session != null && session.Transaction != null && session.Transaction.IsActive)
                            session.Transaction.Rollback();
                        logger.DebugFormat("database {0} rollback transaction success", database);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("数据库{0}回滚事物失败", database), ex);
                    }
                }

                commited = true;
                activateDatabases.Clear();
            }
        }

        public void Dispose()
        {
            if (activateDatabases.Count != 0) return;

            try
            {
                if (!commited) Rollback();

                foreach (var database in innerQueue.Distinct())
                {
                    try
                    {
                        ISession session = UnitOfWork.GetSession(database);
                        if (session != null && session.IsOpen)
                        {
                            session.Close();
                            logger.DebugFormat("database {0} transaction close session  success", database);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("数据库{0}关闭失败", database), ex);
                    }

                    string cacheKey = string.Format("{0}Transaction{1}", database, Thread.CurrentThread.ManagedThreadId.ToString());
                    ApplicationContext.Current.Sessions.Remove(string.Format("{0}{1}", database, Thread.CurrentThread.ManagedThreadId.ToString()));
                    CacheManager.Remove(cacheKey);
                    logger.DebugFormat("database {0} transaction clear cache  success", database);
                }
            }
            catch (Exception ex)
            {
                logger.Error("释放事物失败", ex);
            }
            finally
            {
                innerQueue.Clear();
                activateDatabases.Clear();
            }
        }
    }
}
