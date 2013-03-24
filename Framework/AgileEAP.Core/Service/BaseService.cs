using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AgileEAP.Core.Domain;
using AgileEAP.Core.Data;
using AgileEAP.Core.Caching;

namespace AgileEAP.Core.Service
{
    /// <summary>
    /// 领域服务基类
    /// </summary>
    public class BaseService<TId, TDomain> : IBaseService<TId, TDomain> where TDomain : DomainObject<TId>, new()
    {
        protected IRepository<TId> repository;

        protected BaseService(IRepository<TId> repository)
        {
            this.repository = repository;
        }

        public BaseService()
        {
            this.repository = new Repository<TId>();
        }

        #region IBaseService

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="domain">对象</param>
        public virtual void Add(TDomain domain)
        {
            repository.Save(domain);
            ClearCache();
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="domain">对象</param>
        public virtual void SaveOrUpdate(TDomain domain)
        {
            repository.SaveOrUpdate(domain);
            ClearCache();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="value">可以是对象值、对象主键、条件参数</param>
        public virtual void Delete(object value)
        {
            if (value is IDictionary<string, object>)
            {
                repository.Delete(typeof(TDomain), value as IDictionary<string, object>);
            }
            else
            {
                repository.Delete<TDomain>(value);
            }

            ClearCache();
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="domain">对象</param>
        public virtual void Update(TDomain domain)
        {
            repository.Update(domain);
            ClearCache();
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        public virtual TDomain GetDomain(TId id)
        {
            return repository.GetDomain<TDomain>(id);
        }

        /// <summary>
        /// 查询所有符合条件的记录,显示有效数据和没被删除标记数据
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<TDomain> FindAll(IDictionary<string, object> parameters)
        {
            return repository.FindAll<TDomain>(parameters);
        }

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <returns></returns>
        public IList<TDomain> FindAll(IDictionary<string, object> parameters, string dataFilterCriteria)
        {
            return repository.FindAll<TDomain>(parameters, dataFilterCriteria);
        }

        /// <summary>
        /// 查询所有符合条件的记录，返回分页结果
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <param name="pageInfo">页面信息</param>
        /// <returns></returns>
        public virtual IPageOfList<TDomain> FindAll(IDictionary<string, object> parameters, PageInfo pageInfo)
        {
            return repository.FindAll<TDomain>(parameters, pageInfo);
        }

        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public PageOfList<TDomain> FindAll(IDictionary<string, object> parameters, string dataFilterCriteria, PageInfo pageInfo)
        {
            return repository.FindAll<TDomain>(parameters, dataFilterCriteria, pageInfo);
        }
        /// <summary>
        /// 返回所以对象，并缓存，慎用
        /// </summary>
        /// <returns></returns>
        public virtual IList<TDomain> All()
        {
            string cacheKey = string.Format("{0}_All_Key", typeof(TDomain).Name);
            IList<TDomain> result = CacheManager.GetData<IList<TDomain>>(cacheKey);

            if (result == null)
            {
                result = FindAll(null);

                CacheManager.Add(cacheKey, result);
            }

            return result;
        }


        public void ClearCache()
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
        /// 清除Session缓存
        /// </summary>
        public void ClearSessionCache()
        {
            repository.Clear<TDomain>();
        }

        /// <summary>
        /// 查询符合条件的一条记录
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public virtual TDomain FindOne(IDictionary<string, object> parameters)
        {
            return repository.FindOne<TDomain>(parameters);
        }

        /// <summary>
        /// 根据查询条件返回符合条件的记录数
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        public virtual long Count(IDictionary<string, object> parameters)
        {
            return repository.Count<TDomain>(parameters);
        }

        #endregion
    }
}
