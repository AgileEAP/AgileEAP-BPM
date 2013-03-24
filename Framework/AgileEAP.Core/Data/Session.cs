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
using System.Data.Common;


using NHibernate;
using NHibernate.Cfg;

namespace AgileEAP.Core.Data
{
    /// <summary>
    /// Session扩展
    /// </summary>
    public static class SessionExtension
    {
        static readonly ILogger logger = LogManager.GetLogger(typeof(SessionExtension));

        /// <summary>
        /// 执行存储过程返回object
        /// </summary>
        /// <param name="session"></param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static void AutoClose<TDomain>(this ISession session)
        {
            if (!ApplicationContext.IsWebApp && session != null && session.IsOpen && (session.Transaction == null || !session.Transaction.IsActive))
            {
                session.Close();
                string safeKey = string.Format("{0}{1}", DatabaseManager.Instance.GetDatabase(typeof(TDomain)).Name, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                ApplicationContext.Current.Sessions.Remove(safeKey);
            }
        }



        /// <summary>
        /// 执行存储过程返回DataSet
        /// </summary>
        /// <param name="session"></param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static DataSet ExecuteProcDataSet(this ISession session, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
                {
                    AddParameters(cmd, parameters);
                    return new ADODataAdapter().FillDataSet(cmd);
                }
            }
        }

        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <param name="session"></param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static DataTable ExecuteProcDataTable(this ISession session, string procName, IDictionary<string, object> parameters)
        {
            DataSet ds = session.ExecuteProcDataSet(procName, parameters);

            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 执行存储过程返回object
        /// </summary>
        /// <param name="session"></param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static object ExecuteProcScalar(this ISession session, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="session"></param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static int ExecuteProcNonQuery(this ISession session, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行SQL命令返回DataSet
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(this ISession session, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(cmdText, session.Connection, CommandType.Text))
                {
                    AddParameters(cmd, parameters);
                    return new ADODataAdapter().FillDataSet(cmd);
                }
            }
        }

        /// <summary>
        /// 执行SQL命令返回DataSet
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(this ISession session, string cmdText)
        {
            return session.ExecuteDataSet(cmdText, null);
        }

        /// <summary>
        ///  执行SQL命令返回DataTable
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(this ISession session, string cmdText, IDictionary<string, object> parameters)
        {
            DataSet ds = session.ExecuteDataSet(cmdText, parameters);

            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 执行SQL命令返回DataTable
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(this ISession session, string cmdText)
        {
            return session.ExecuteDataTable(cmdText, null);
        }

        /// <summary>
        /// 执行SQL命令返回对象
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(this ISession session, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行SQL命令返回对象
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(this ISession session, string cmdText)
        {
            return session.ExecuteScalar(cmdText, null);
        }

        /// <summary>
        /// 执行SQL命令返回影响的行数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">字典参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this ISession session, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = session.Connection)
            {
                using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  执行SQL命令返回影响的行数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmdText">sql语句</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this ISession session, string cmdText)
        {
            return session.ExecuteNonQuery(cmdText, null);
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        private static IDbCommand PrepareCommand(string cmdText, IDbConnection conn, CommandType cmdType)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            return cmd;
        }

        /// <summary>
        /// 添加命令参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        private static void AddParameter(IDbCommand cmd, string paramName, object value)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                logger.Error("参数为空");
                throw new ArgumentNullException();
            }

            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = paramName;
            parameter.Value = value;

            cmd.Parameters.Add(parameter);
        }

        /// <summary>
        /// 添加命令参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        private static void AddParameters(IDbCommand cmd, IDictionary<string, object> parameters)
        {
            if (cmd == null)
            {
                logger.Error("Command对象为空");
                throw new ArgumentNullException();
            }

            if (parameters == null)
            {
                logger.Debug("参数字典为空");
                return;
            }

            foreach (string key in parameters.Keys)
            {
                AddParameter(cmd, key, parameters[key]);
            }
        }



        #region Internate Help
        private class ADODataAdapter : DbDataAdapter
        {
            public DataTable FillDataTable(IDataReader dr)
            {
                DataTable dataTable = new DataTable();
                Fill(dataTable, dr);
                return dataTable;
            }

            public DataTable FillDataTable(IDbCommand cmd)
            {
                DataTable dataTable = new DataTable();
                DataTable[] dataTables = new DataTable[] { dataTable };
                CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
                this.Fill(dataTables, 0, 0, cmd, fillCommandBehavior);

                return dataTable;
            }

            public DataSet FillDataSet(IDbCommand cmd)
            {
                DataSet ds = new DataSet();
                CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
                this.Fill(ds, 0, 0, "Table", cmd, fillCommandBehavior);

                return ds;
            }
        }
        #endregion
    }
}
