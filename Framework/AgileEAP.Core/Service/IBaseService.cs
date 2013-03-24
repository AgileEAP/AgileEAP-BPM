using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Domain;
using AgileEAP.Core.Data;

namespace AgileEAP.Core.Service
{
    /// <summary>
    /// 领域服务基类
    /// </summary>
    public interface IBaseService<TId, TDomain> where TDomain : DomainObject<TId>, new()
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="domain">对象</param>
        void Add(TDomain domain);

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="domain">对象</param>
        void SaveOrUpdate(TDomain domain);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="value">可以是对象值、对象主键、条件参数</param>
        void Delete(object value);

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="domain">对象</param>
        void Update(TDomain domain);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        TDomain GetDomain(TId id);

        /// <summary>
        /// 查询符合条件的一条记录
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        TDomain FindOne(IDictionary<string, object> parameters);

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        IList<TDomain> FindAll(IDictionary<string, object> parameters);

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <returns></returns>
        IList<TDomain> FindAll(IDictionary<string, object> parameters, string dataFilterCriteria);

        /// <summary>
        /// 查询所有符合条件的记录，返回分页结果
        /// </summary>
        /// <param name="parameters">条件参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        IPageOfList<TDomain> FindAll(IDictionary<string, object> parameters, PageInfo pageInfo);

        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        PageOfList<TDomain> FindAll(IDictionary<string, object> parameters, string dataFilterCriteria, PageInfo pageInfo);

        /// <summary>
        /// 返回所以对象，并缓存，慎用
        /// </summary>
        /// <returns></returns>
        IList<TDomain> All();

        /// <summary>
        /// 清除Session缓存
        /// </summary>
        void ClearSessionCache();
    }
}
