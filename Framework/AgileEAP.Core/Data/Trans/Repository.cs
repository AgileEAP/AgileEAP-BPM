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

using log4net;
using NHibernate;
using NHibernate.Criterion;
using SunTek.EAFrame.Core.Utility;
using SunTek.EAFrame.Core.Extensions;
using SunTek.EAFrame.Core.Domain;
using SunTek.EAFrame.Core.Enums;

namespace SunTek.EAFrame.Core.Data.Trans
{
    public class Repository<TId, TDomain>: IRepository<TId, TDomain> where TDomain : DomainObject<TId>, new()
    {
        #region Properties
        private ILog log = LogManager.GetLogger(typeof(Repository<TId, TDomain>));
        #endregion

        #region Contructor
        public Repository()
        {
        }
        #endregion

        #region IRepository

        public virtual TDomain Load(TId id)
        {
            return Session.Load<TDomain>(id);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(TDomain entity)
        {
            Session.Save(entity);
            Session.Flush();
        }
        /// <summary>
        /// 新增或修改数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SaveOrUpdate(TDomain entity)
        {
            Session.SaveOrUpdate(entity);
            Session.Flush();
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TDomain entity)
        {
            Session.Update(entity);
            Session.Flush();
        }


        /// <summary>
        /// 删除数据，如果在数据表中有"IsDelete"字段，则调用此方法是实现假删除，否则数据被真正删除
        /// </summary>
        /// <param name="entity">要删除的对象</param>
        public virtual void Delete(TDomain entity)
        {
            if (IsVirtualDelete())
            {
                PropertyInfo pi = typeof(TDomain).GetProperties().FirstOrDefault(o => o.Name.Equals("IsDelete", StringComparison.OrdinalIgnoreCase));

                try
                {
                    pi.SetValue(entity, 1, null);
                    Session.Update(entity);
                    Session.Flush();
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Virtual delete entity {0} where id is {1} error!", typeof(TDomain), entity.ID), ex);
                }
            }
            else
            {
                Session.Delete(entity);
                Session.Flush();
            }
        }


        /// <summary>
        /// 删除数据，如果在数据表中有"IsDelete"字段，则实现假删除，否则数据被真正删除
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(TId id)
        {
            TDomain entity = Session.Load<TDomain>(id);
            Delete(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual void Delete(Type type, IDictionary<string, object> parameters)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("delete from {0} where 1=1 ", type.Name);
            foreach (KeyValuePair<string, object> keyValuePair in parameters)
            {
                cmdText.AppendFormat(" and {0}=:{1}", keyValuePair.Key, keyValuePair.Key);
            }

            IQuery query = Session.CreateQuery(cmdText.ToSafeString());
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
        }

        /// <summary>
        /// 从session缓存中移除传入对象
        /// </summary>
        /// <param name="entity">传入要移除对象</param>
        public virtual void Evict(TDomain entity)
        {
            Session.Evict(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exampleInstance"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        public virtual List<TDomain> FindAll(TDomain exampleInstance, params string[] propertiesToExclude)
        {
            if (Session.IsDirty()) Session.Flush();

            ICriteria criteria = Session.CreateCriteria(typeof(TDomain));
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<TDomain>() as List<TDomain>;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exampleInstance"></param>
        /// <param name="pageInfo"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        public virtual PageOfList<TDomain> FindAll(TDomain exampleInstance, PageInfo pageInfo, params string[] propertiesToExclude)
        {
            ICriteria criteria = CreateCriteria(exampleInstance, propertiesToExclude);

            ICriteria criteria1 = criteria.Clone() as ICriteria;

            IMultiCriteria multiCriteria = Session.CreateMultiCriteria();
            multiCriteria.Add(criteria.SetFirstResult(pageInfo.PageIndex).SetMaxResults(pageInfo.PageSize));
            multiCriteria.Add(criteria1.SetProjection(Projections.RowCount()));

            PageOfList<TDomain> result = new PageOfList<TDomain>();
            result.PageInfo = pageInfo;

            IList values = multiCriteria.List();

            if (values != null && values.Count == 2)
            {
                result.ItemList = DataUtil.ToEntityList<TDomain>(values[0] as IList);
                pageInfo.ItemCount = (long)((IList)values[1])[0];
            }

            result.PageInfo = pageInfo;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exampleInstance"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        public virtual TDomain FindOne(TDomain exampleInstance, params string[] propertiesToExclude)
        {
            List<TDomain> result = FindAll(exampleInstance, propertiesToExclude);
            if (result.Count > 1)
            {
                throw new NonUniqueResultException(result.Count);
            }
            else if (result.Count == 1)
            {
                return result[0];
            }

            return default(TDomain);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual TDomain FindOne(IDictionary<string, object> parameters)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TDomain));

            if (parameters != null)
            {

                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    if (keyValuePair.Value == null)
                    {
                        criteria.Add(Expression.IsNull(keyValuePair.Key));
                    }
                    else if (keyValuePair.Value is string)
                    {
                        criteria.Add(Expression.Eq(keyValuePair.Key, keyValuePair.Value.ToString()));
                    }
                    else if (keyValuePair.Value is DataTimePair)
                    {
                        criteria.Add(((DataTimePair)keyValuePair.Value).ToExpression());
                    }
                    else
                    {
                        criteria.Add(ToSafeExpression(keyValuePair.Key, keyValuePair.Value));
                    }
                }
            }

            return criteria.UniqueResult<TDomain>();
        }

        public virtual TDomain GetDomain(TId id)
        {
            if (string.IsNullOrEmpty(id.ToSafeString())) return default(TDomain);

            return Session.Get<TDomain>(id);
        }

        public virtual TDomain GetSafeDomain(TId id)
        {
            return GetDomain(id) ?? new TDomain();
        }

        public virtual TDomain GetStatelessDomain(TId id)
        {
            Session.Clear();
            TDomain result = id == null ? null : Session.Get<TDomain>(id);
            Session.Clear();

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<TDomain> FindAll(IDictionary<string, object> parameters)
        {
            return CreateCriteria(parameters).List<TDomain>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="pageInfo"></param>
        /// <returns>返回分页数据</returns>
        public virtual PageOfList<TDomain> FindAll(IDictionary<string, object> parameters, PageInfo pageInfo)
        {
            ICriteria criteria = CreateCriteria(parameters);

            criteria.SetFirstResult((pageInfo.PageIndex - 1) * pageInfo.PageSize)
                     .SetMaxResults(pageInfo.PageSize);

            PageOfList<TDomain> result = new PageOfList<TDomain>();
            result.ItemList = criteria.List<TDomain>() as List<TDomain>;
            pageInfo.ItemCount = criteria.SetProjection(Projections.RowCount()).UniqueResult().ToLong();

            result.PageInfo = pageInfo;

            return result;
        }


        public virtual long GetCount(IDictionary<string, object> parameters)
        {
            ICriteria criteria = CreateCriteria(parameters);
            return criteria.SetProjection(Projections.RowCount()).UniqueResult().ToLong();
        }

        public virtual long GetCount(TDomain exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = CreateCriteria(exampleInstance, propertiesToExclude);

            return criteria.SetProjection(Projections.RowCount()).UniqueResult().ToLong();
        }

        public virtual string GetMax(IDictionary<string, object> parameters, string propertyName)
        {
            ICriteria criteria = CreateCriteria(parameters);
            return criteria.SetProjection(Projections.Max(propertyName)).UniqueResult().ToSafeString();
        }

        public virtual List<TDomain> List()
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TDomain));
            return criteria.List<TDomain>() as List<TDomain>;
        }

        public virtual List<TDomain> FuzzyQuery(IDictionary<string, object> parameters)
        {
            ICriteria criteria = CreateCriteria(parameters);

            return criteria.List<TDomain>() as List<TDomain>;
        }


        public virtual ISession Session
        {
            get { return NHibernateSessionManager.GetSession(typeof(TDomain)); }
        }

        #endregion

        #region Transaction Related Methods
        public virtual void CommitChanges()
        {
            Session.Flush();
            Session.Clear();
        }

        public virtual void BeginTransaction()
        {
            Session.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            Session.Transaction.Commit();
        }

        public virtual void RollbackTransaction()
        {
            Session.Transaction.Rollback();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// 用来判断对象是否包含IsDelete字段
        /// </summary>
        /// <returns></returns>
        protected bool IsVirtualDelete()
        {
            Type type = typeof(TDomain);//获取类型
            var pi = type.GetProperties().FirstOrDefault(p => p.Name.Equals("IsDelete", StringComparison.OrdinalIgnoreCase));

            return pi != null;
        }
        /// <summary>
        /// 用来判断对象是否包含IsValid字段
        /// </summary>
        /// <returns></returns>
        protected bool IsHaveIsValid()
        {
            Type type = typeof(TDomain);//获取类型
            var pi = type.GetProperties().FirstOrDefault(p => p.Name.Equals("IsValid", StringComparison.OrdinalIgnoreCase));

            return pi != null;
        }
        /// <summary>
        /// 用来判断对象是否包含IsDelete字段
        /// </summary>
        /// <returns></returns>
        protected bool IsVirtualDelete(Type type)
        {
            var pi = type.GetProperties().FirstOrDefault(p => p.Name.Equals("IsDelete", StringComparison.OrdinalIgnoreCase));

            return pi != null;
        }
        /// <summary>
        /// 用来判断对象是否包含IsValid字段
        /// </summary>
        /// <returns></returns>
        protected bool IsHaveIsValid(Type type)
        {
            var pi = type.GetProperties().FirstOrDefault(p => p.Name.Equals("IsValid", StringComparison.OrdinalIgnoreCase));

            return pi != null;
        }

        protected virtual ICriteria CreateCriteria(IDictionary<string, object> parameters)
        {
            if (Session.IsDirty())
            {
                Session.Flush();
            }

            ICriteria criteria = Session.CreateCriteria(typeof(TDomain));

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    if (keyValuePair.Value == null)
                    {
                        criteria.Add(Expression.IsNull(keyValuePair.Key));
                    }
                    else if (keyValuePair.Value is string)
                    {
                        criteria.Add(Expression.Eq(keyValuePair.Key, keyValuePair.Value));
                        criteria.Add(Expression.IsNotNull(keyValuePair.Key));
                        //criteria.Add(Expression.Like(keyValuePair.Key, keyValuePair.Value.ToString(), MatchMode.Anywhere));
                    }
                    else if (keyValuePair.Value is DataTimePair)
                    {
                        criteria.Add(((DataTimePair)keyValuePair.Value).ToExpression());
                    }
                    else
                    {
                        criteria.Add(ToSafeExpression(keyValuePair.Key, keyValuePair.Value));
                    }
                }
            }

            return criteria;
        }

        protected virtual SimpleExpression ToSafeExpression(string propetyName, object value)
        {
            object result = null;
            PropertyInfo property = null;
            string realPropertyName = propetyName;

            try
            {
                Type type = typeof(TDomain);

                property = type.GetPropertyIgnoreCase(propetyName);

                realPropertyName = property.Name;
                result = Convert.ChangeType(value, property.PropertyType);

            }
            catch
            {
                log.Error(string.Format("Value {0} ChageType {1} to {2} Error!", value, value.GetType(), property.PropertyType));
                result = value;
            }

            return Expression.Eq(realPropertyName, result);
        }

        protected virtual ICriteria CreateCriteria(TDomain exampleInstance, params string[] propertiesToExclude)
        {
            if (Session.IsDirty())
            {
                Session.Flush();
            }

            ICriteria criteria = Session.CreateCriteria(typeof(TDomain));

            if (exampleInstance != null)
            {
                Example example = Example.Create(exampleInstance);

                foreach (string propertyToExclude in propertiesToExclude)
                {
                    example.ExcludeProperty(propertyToExclude);
                }

                criteria.Add(example);
            }

            return criteria;
        }

        #endregion
    }
}
