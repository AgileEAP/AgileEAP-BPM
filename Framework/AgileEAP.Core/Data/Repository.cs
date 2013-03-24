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
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Data;


using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Domain;
using AgileEAP.Core.Caching;

namespace AgileEAP.Core.Data
{
    public class Repository<TId> : IRepository<TId>
    {
        #region Properties
        private ILogger logger = LogManager.GetLogger(typeof(Repository<TId>));
        private object objLock = new object();
        private DataContext context = null;
        protected DataContext Context
        {
            get
            {
                if (context == null)
                {
                    lock (objLock)
                    {
                        if (context == null) context = new DataContext();
                    }
                }
                return context;
            }
            set
            {
                context = value;
            }
        }

        #endregion

        #region Contructor
        public Repository()
        {
        }
        #endregion

        #region IRepository

        /// <summary>
        /// 根据对象主键Load一个对象
        /// </summary>
        public TDomain Load<TDomain>(TId id) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession(typeof(TDomain));
            try
            {
                return session.Load<TDomain>(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        public void Save(DomainObject<TId> domain)
        {
            if (domain == null) return;
            Type domainType = domain.GetType();
            using (ITransaction trans = UnitOfWork.BeginTransaction(domainType))
            {
                ISession session = GetSession(domainType);
                session.Save(domain);
                session.Flush();

                trans.Commit();
            }

            ClearCache(domainType);
        }
        /// <summary>
        /// 新增或修改对象
        /// </summary>
        /// <param name="domain"></param>
        public virtual void SaveOrUpdate(DomainObject<TId> domain)
        {
            if (domain == null) return;

            Type domainType = domain.GetType();
            using (ITransaction trans = UnitOfWork.BeginTransaction(domainType))
            {
                ISession session = GetSession(domainType);

                if (session.Get(domainType, domain.ID) != null)
                    session.Clear();

                session.SaveOrUpdate(domain);
                session.Flush();

                trans.Commit();
            }

            ClearCache(domainType);
        }


        /// <summary>
        /// 修改一个对象
        /// </summary>
        /// <param name="domain"></param>
        public virtual void Update(DomainObject<TId> domain)
        {
            if (domain == null) return;
            Type domainType = domain.GetType();
            using (ITransaction trans = UnitOfWork.BeginTransaction(domainType))
            {
                ISession session = GetSession(domainType);
                if (session.Get(domainType, domain.ID) != null)
                    session.Clear();
                session.Update(domain);
                session.Flush();

                trans.Commit();
            }

            ClearCache(domainType);
        }


        /// <summary>
        /// 删除数据，如果在数据表中有"IsDelete"字段，则调用此方法是实现假删除，否则数据被真正删除
        /// </summary>
        /// <param name="value">要删除的对象</param>
        public virtual void Delete<TDomain>(object value) where TDomain : DomainObject<TId>, new()
        {
            if (value == null) return;

            Type domainType = typeof(TDomain);
            using (ITransaction trans = UnitOfWork.BeginTransaction(domainType))
            {
                Type type = value.GetType();
                ISession session = GetSession(domainType);
                if (type == typeof(TId))
                {
                    TDomain obj = new TDomain() { ID = (TId)value };
                    session.Delete(obj);
                }
                else if (type == typeof(Dictionary<string, object>) || type == typeof(IDictionary<string, object>))
                {
                    Delete(domainType, value as Dictionary<string, object>);
                }
                else if (type == domainType)
                {
                    if (domainType.ExistsProperty("IsDelete"))
                    {
                        PropertyInfo pi = domainType.GetProperties().FirstOrDefault(o => o.Name.Equals("IsDelete", StringComparison.OrdinalIgnoreCase));
                        try
                        {
                            pi.SetValue(value, 1, null);
                            session.Update(value);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(string.Format("Virtual delete domain {0} where id is {1} error!", domainType, value), ex);
                            throw;
                        }
                    }
                    else
                    {
                        session.Delete(value);
                    }
                }

                session.Flush();

                trans.Commit();
            }

            ClearCache(domainType);
        }

        /// <summary>
        /// 指定条件删除对象
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public virtual void Delete(Type type, IDictionary<string, object> parameters)
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(type))
            {
                StringBuilder cmdText = new StringBuilder();
                cmdText.AppendFormat("delete from {0} where 1=1 ", DataBaseUtil.GetTableName(type));
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    cmdText.AppendFormat(" and {0}=:{1}", keyValuePair.Key, keyValuePair.Key);
                }

                IQuery query = GetSession(type).CreateSQLQuery(cmdText.ToString());//.CreateQuery(cmdText.ToSafeString());
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    if (keyValuePair.Value is string)
                    {
                        query.SetString(keyValuePair.Key, keyValuePair.Value.ToSafeString());
                    }
                    else
                    {
                        query.SetParameter(keyValuePair.Key, keyValuePair.Value);
                    }
                }

                query.ExecuteUpdate();

                trans.Commit();
            }

            ClearCache(type);
        }


        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        public TDomain GetDomain<TDomain>(TId id) where TDomain : DomainObject<TId>, new()
        {
            if (string.IsNullOrEmpty(id.ToSafeString())) return default(TDomain);

            ISession session = GetSession(typeof(TDomain));
            try
            {
                return session.Get<TDomain>(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        /// <summary>
        /// 查询符合条件的一条记录
        /// </summary>
        /// <typeparam name="TDomain">对象</typeparam>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public TDomain FindOne<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession(typeof(TDomain));
            try
            {
                ICriteria criteria = CreateCriteria(session, typeof(TDomain), parameters);

                return criteria.UniqueResult<TDomain>();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }


        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        public IList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                return CreateCriteria(session, typeof(TDomain), parameters).List<TDomain>();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <returns></returns>
        public IList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, string dataFilterCriteria) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                Type type = typeof(TDomain);
                return CreateSQLQuery(session, type, string.Format("select * from {0} where 1=1 ", DataBaseUtil.GetTableName(type)), parameters, dataFilterCriteria).List<TDomain>();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }


        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public PageOfList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, string dataFilterCriteria, PageInfo pageInfo) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                Type type = typeof(TDomain);
                IQuery query = CreateSQLQuery(session, type, string.Format("select * from {0} where 1=1 ", DataBaseUtil.GetTableName(type)), parameters, dataFilterCriteria)
                      .SetFirstResult((pageInfo.PageIndex - 1) * pageInfo.PageSize)
                      .SetMaxResults(pageInfo.PageSize);

                PageOfList<TDomain> result = new PageOfList<TDomain>();
                result.ItemList = query.List<TDomain>() as List<TDomain>;

                IQuery query2 = CreateSQLQuery(session, type, string.Format("select count(*) from {0} where 1=1 ", DataBaseUtil.GetTableName(type)), parameters, dataFilterCriteria);

                pageInfo.ItemCount = Count<TDomain>(parameters);

                result.PageInfo = pageInfo;

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }


        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public PageOfList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, PageInfo pageInfo) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                ICriteria criteria = CreateCriteria(session, typeof(TDomain), parameters);

                criteria.SetFirstResult((pageInfo.PageIndex - 1) * pageInfo.PageSize)
                         .SetMaxResults(pageInfo.PageSize);

                PageOfList<TDomain> result = new PageOfList<TDomain>();
                result.ItemList = criteria.List<TDomain>() as List<TDomain>;
                pageInfo.ItemCount = Count<TDomain>(parameters);

                result.PageInfo = pageInfo;

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        /// <summary>
        /// 返回所以结果
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <returns></returns>
        public List<TDomain> All<TDomain>() where TDomain : DomainObject<TId>, new()
        {
            return CacheManager.Get<List<TDomain>>(string.Format("{0}_All_Key", typeof(TDomain).Name), () =>
                  {
                      ISession session = GetSession<TDomain>();
                      try
                      {
                          return session.CreateCriteria(typeof(TDomain)).List<TDomain>() as List<TDomain>;
                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex);
                          throw;
                      }
                      finally
                      {
                          session.AutoClose<TDomain>();
                      }
                  });
        }


        /// <summary>
        /// 获取返回结果行数
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        public long Count<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                ICriteria criteria = CreateCriteria(session, typeof(TDomain), parameters);
                return criteria.SetProjection(Projections.RowCount()).UniqueResult().ToLong();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        public virtual ISession GetSession(Type domainType)
        {
            return UnitOfWork.GetSession(domainType);
        }

        public virtual ISession GetSession<TDomain>()
        {
            return UnitOfWork.GetSession(typeof(TDomain));
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="sql"></param>
        public void ExecuteSql<TDomain>(string sql) where TDomain : DomainObject<TId>, new()
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(TDomain)))
            {
                GetSession(typeof(TDomain)).CreateSQLQuery(sql).ExecuteUpdate();

                trans.Commit();
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="sql">sql语句中的参数以:开头</param>
        /// <param name="parameters">参数</param>
        public void ExecuteSql<TDomain>(string sql, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(TDomain)))
            {
                ISQLQuery query = GetSession(typeof(TDomain)).CreateSQLQuery(sql);

                foreach (var parameter in parameters)
                {
                    query.SetParameter(parameter.Key, parameter.Value);
                }

                query.ExecuteUpdate();

                trans.Commit();
            }
        }

        /// <summary>
        /// Flush Session到数据库
        /// </summary>
        public void CommitChanges<TDomain>()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                session.Flush();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <typeparam name="TDomain"></typeparam>
        public void ClearCache<TDomain>()
        {
            string cacheKey = string.Format("{0}_All_Key", typeof(TDomain).Name);
            CacheManager.Remove(cacheKey);
        }

        public void ClearCache(Type type)
        {
            string cacheKey = string.Format("{0}_All_Key", type.Name);
            CacheManager.Remove(cacheKey);
        }

        /// <summary>
        /// 把Session 缓存清空
        /// </summary>
        public void Clear<TDomain>()
        {
            ISession session = GetSession<TDomain>();
            try
            {
                session.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                session.AutoClose<TDomain>();
            }
        }

        #endregion

        #region Internal Methods

        protected IQuery CreateSQLQuery(ISession session, Type domainType, string initSql, IDictionary<string, object> parameters, string dataFilterCriteria)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(initSql);
            foreach (var parameter in parameters)
            {
                if (parameter.Value is Condition)
                {
                    Condition c = (Condition)parameter.Value;
                    if (!string.IsNullOrEmpty(c.Expression))
                        cmdText.AppendFormat(" and {0} ", c.Expression);
                }
                else
                {
                    cmdText.AppendFormat(" and {0}=:{0}", parameter.Key);
                }
            }

            if (!string.IsNullOrEmpty(dataFilterCriteria))
            {
                cmdText.AppendFormat(" {0} ", dataFilterCriteria);
            }

            logger.Debug(cmdText.ToSafeString());

            IQuery query = session.CreateSQLQuery(cmdText.ToString()).AddEntity(domainType);

            foreach (var parameter in parameters)
            {
                if (!(parameter.Value is Condition))
                {
                    query.SetParameter(parameter.Key, parameter.Value);
                }
            }

            return query;
        }

        protected virtual ICriteria CreateCriteria(ISession session, Type domainType, IDictionary<string, object> parameters)
        {
            ICriteria criteria = session.CreateCriteria(domainType);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    if (keyValuePair.Value == null)
                    {
                        criteria.Add(Expression.IsNull(keyValuePair.Key));
                    }
                    else if (keyValuePair.Value is DateTimePair)
                    {
                        criteria.Add(((DateTimePair)keyValuePair.Value).ToExpression(keyValuePair.Key.Trim()));
                    }
                    else if (keyValuePair.Value is Condition)
                    {
                        Condition c = keyValuePair.Value as Condition;
                        if (c != null && !string.IsNullOrEmpty(c.Expression))
                            criteria.Add(Expression.Sql((keyValuePair.Value as Condition).Expression));
                    }
                    else
                    {
                        criteria.Add(ToSafeExpression(domainType, keyValuePair.Key, keyValuePair.Value));
                    }
                }
            }

            return criteria;
        }

        protected virtual SimpleExpression ToSafeExpression(Type type, string propetyName, object value)
        {
            object result = null;
            PropertyInfo property = null;
            string realPropertyName = propetyName;

            try
            {
                property = type.GetPropertyIgnoreCase(propetyName);
                realPropertyName = property.Name;

                if (property.PropertyType == typeof(string))
                {
                    result = value.ToSafeString();
                }
                else if (property.PropertyType == typeof(int))
                {
                    result = value.ToInt(0);
                }
                else if (property.PropertyType == typeof(short))
                {
                    result = (short)value.ToInt();
                }
                else
                {
                    result = Convert.ChangeType(value, property.PropertyType);
                }
            }
            catch
            {
                logger.Error(string.Format("Value {0} ChageType {1} to {2} Error!", value, value.GetType(), property.PropertyType));
                result = value;
            }

            return Expression.Eq(realPropertyName, result);
        }

        #endregion

        #region IDataContex
        /// <summary>
        /// 运行存储过程，返回DataSet
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public DataSet ExecuteProcDataSet<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteProcDataSet(UnitOfWork.GetEAConnection(typeof(TDomain)), procName, parameters);
        }
        /// <summary>
        /// 运行存储过程，返回单个object
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public object ExecuteProcScalar<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteProcScalar(UnitOfWork.GetEAConnection(typeof(TDomain)), procName, parameters);
        }

        /// <summary>
        /// 运行存储过程，不返回值
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        public void ExecuteProcNonQuery<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            Context.ExecuteProcNonQuery(UnitOfWork.GetEAConnection(typeof(TDomain)), procName, parameters);
        }

        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataSet(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters);
        }

        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataSet(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText);
        }

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataTable(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters);
        }

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <param name="orderBy">排序条件如：a desc ,b asc </param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters, string orderBy, PageInfo pageInfo) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataTable(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters, orderBy, pageInfo);
        }

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters, PageInfo pageInfo) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataTable(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters, string.Empty, pageInfo);
        }

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteDataTable(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText);
        }

        /// <summary>
        /// 执行sql命令，返回单个对象值
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public object ExecuteScalar<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteScalar(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters);
        }

        /// <summary>
        /// 执行sql命令，返回单个对象值
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        public object ExecuteScalar<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new()
        {
            return Context.ExecuteScalar(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText);
        }

        /// <summary>
        /// 执行sql命令，不返回结果
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public void ExecuteNonQuery<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new()
        {
            Context.ExecuteNonQuery(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText, parameters);
        }

        /// <summary>
        /// 执行sql命令，不返回结果
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        public void ExecuteNonQuery<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new()
        {
            Context.ExecuteNonQuery(UnitOfWork.GetEAConnection(typeof(TDomain)), cmdText);
        }

        #endregion

        #region DML

        /// <summary>
        /// 获取数据库所有用户的表
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IList<string> GetTables(IEAConnection conn = null)
        {
            try
            {
                if (conn == null) conn = UnitOfWork.GetEAConnection();

                string cmdText = string.Empty;
                if (conn.DatabaseType == DatabaseType.MySQL)
                {
                    cmdText = "show tables";
                }
                else if (conn.DatabaseType == DatabaseType.Oracle)
                {
                    cmdText = "select table_name as Name from user_tables order by table_name";
                }
                else
                {
                    cmdText = "SELECT table_name as Name FROM INFORMATION_SCHEMA.TABLES order by TABLE_TYPE,table_name";
                }
                IList<string> result = new List<string>();
                DataTable dt = Context.ExecuteDataTable(conn, cmdText);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.SafeAdd(dr[0].ToSafeString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public IList<string> GetTableColumns(string table, IEAConnection conn = null)
        {
            try
            {
                if (conn == null) conn = UnitOfWork.GetEAConnection();

                string cmdText = string.Empty;
                if (conn.DatabaseType == DatabaseType.MySQL)
                {
                    cmdText = "desc " + table;
                }
                else if (conn.DatabaseType == DatabaseType.Oracle)
                {
                    cmdText = string.Format(@"SELECT
                                            USER_TAB_COLS.COLUMN_NAME 
                                        FROM USER_TAB_COLS
                                        inner join user_col_comments
                                        on user_col_comments.TABLE_NAME = USER_TAB_COLS.TABLE_NAME
                                        and user_col_comments.COLUMN_NAME = USER_TAB_COLS.COLUMN_NAME
                                        and USER_TAB_COLS.TABLE_NAME = '{0}'", table);
                }
                else
                {
                    cmdText = string.Format("SELECT Name FROM SysColumns WHERE id=Object_Id('{0}')", table);
                }
                IList<string> result = new List<string>();
                DataTable dt = Context.ExecuteDataTable(conn, cmdText);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.SafeAdd(dr[0].ToSafeString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return null;
        }

        #endregion

        #region ILinq

        public IQueryable<T> Query<T>()
        {
            ISession session = GetSession(typeof(T));
            return session.Query<T>();
        }
        #endregion
    }
}
