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
using System.Data;
using NHibernate;

using AgileEAP.Core.Domain;
namespace AgileEAP.Core.Data
{
    /// <summary>
    /// 数据上下文类
    /// </summary>
    public interface IDataContext<TId>
    {
        /// <summary>
        /// 运行存储过程，返回DataSet
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        DataSet ExecuteProcDataSet<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 运行存储过程，返回单个object
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        object ExecuteProcScalar<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 运行存储过程，不返回值
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">条件参数</param>
        void ExecuteProcNonQuery<TDomain>(string procName, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        DataSet ExecuteDataSet<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        DataSet ExecuteDataSet<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters, PageInfo pageInfo) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <param name="orderBy">排序条件如：a desc ,b asc </param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        DataTable ExecuteDataTable<TDomain>(string cmdText, IDictionary<string, object> parameters, string orderBy, PageInfo pageInfo) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        DataTable ExecuteDataTable<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回单个对象值
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        object ExecuteScalar<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，返回单个对象值
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        object ExecuteScalar<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，不返回结果
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <returns></returns>
        void ExecuteNonQuery<TDomain>(string cmdText, IDictionary<string, object> parameters) where TDomain : DomainObject<TId>, new();

        /// <summary>
        /// 执行sql命令，不返回结果
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <returns></returns>
        void ExecuteNonQuery<TDomain>(string cmdText) where TDomain : DomainObject<TId>, new();
    }
}
