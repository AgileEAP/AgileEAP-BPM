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
using System.Text;

using NHibernate;
using NHibernate.Cfg;
using AgileEAP.Core.Domain;

namespace AgileEAP.Core.Data
{
    public interface IRepository<TId> : IDataContext<TId>
    {
        /// <summary>
        /// 根据对象主键Load一个对象
        /// </summary>
        TDomain Load<TDomain>(TId id) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 保存一个对象
        /// </summary>
        void Save(DomainObject<TId> entity);

        /// <summary>
        /// 保存或修改一个对象
        /// </summary>
        void SaveOrUpdate(DomainObject<TId> entity);

        /// <summary>
        ///修改一个对象
        /// </summary>
        void Update(DomainObject<TId> entity);

        /// <summary>
        ///删除一个对象
        /// </summary>
        void Delete<TDomain>(object value) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 根据条件删除表中数据
        /// </summary>
        /// <param name="type">实体对象类型</param>
        /// <param name="parameters">条件参数</param>
        void Delete(Type type, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        TDomain GetDomain<TDomain>(TId id) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 查询符合条件的一条记录
        /// </summary>
        /// <typeparam name="TDomain">对象</typeparam>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        TDomain FindOne<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        IList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 查询所有符合条件的记录
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <returns></returns>
        IList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, string dataFilterCriteria) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        PageOfList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, PageInfo pageInfo) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 查找返回分结果集
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <param name="dataFilterCriteria">数据过滤条件</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        PageOfList<TDomain> FindAll<TDomain>(IDictionary<string, object> parameters, string dataFilterCriteria, PageInfo pageInfo) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 返回所以结果，会自动Cache对内存中，不能滥用
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <returns></returns>
        List<TDomain> All<TDomain>() where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 获取返回结果行数
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        long Count<TDomain>(IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="sql"></param>
        void ExecuteSql<TDomain>(string sql) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="TDomain">对象类型</typeparam>
        /// <param name="sql">sql语句中的参数以:开头</param>
        /// <param name="parameters">参数</param>
        void ExecuteSql<TDomain>(string sql, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 把Session 缓存清空
        /// </summary>
        void Clear<TDomain>();

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <typeparam name="TDomain"></typeparam>
        void ClearCache<TDomain>();

        /// <summary>
        /// Linq查询接口
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        System.Linq.IQueryable<T> Query<T>();

        /// <summary>
        /// 获取数据库所有用户的表
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        IList<string> GetTables(IEAConnection conn = null);

        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        IList<string> GetTableColumns(string table, IEAConnection conn = null);
    }
}
