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
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;


using NHibernate;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Core.Data
{
    /// <summary>
    /// 数据上下文类，主要用于Ado.net的相关的操作
    /// </summary>
    public class DataContext //: IDataContext
    {
        ILogger logger = LogManager.GetLogger(typeof(DataContext));

        public DataContext()
        { }
        #region IDataContext

        #region Ado Related Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        private IDbCommand PrepareCommand(string cmdText, IDbConnection conn, CommandType cmdType)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private IDbCommand PrepareCommand(string cmdText, IDbConnection conn)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = cmdText;

            return cmd;
        }

        private void AddParameter(IDbCommand cmd, string paramName, object value)
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
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        private void AddParameters(DatabaseType databaseType, IDbCommand cmd, IDictionary<string, object> parameters)
        {
            if (cmd == null)
            {

                logger.Error("Command对象为空");
                throw new ArgumentNullException();
            }

            if (parameters == null)
            {
                logger.Info("参数字典为空");
                return;
            }

            bool isParamertered = cmd.CommandText.Contains('$');

            string paramChar = DataUtil.GetParamChar(databaseType);// cmd is SqlCommand ? "@" : ":";

            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(cmd.CommandText);

            if (!isParamertered && !cmd.CommandText.TrimEnd().EndsWith("1=1"))
                cmdText.Append(" where 1=1");

            foreach (var parameter in parameters)
            {
                if (parameter.Value is Condition)
                {
                    string c = ((Condition)parameter.Value).Expression;
                    if (!string.IsNullOrEmpty(c))
                        cmdText.AppendFormat(" and {0} ", c);
                }
                else
                {
                    if (isParamertered)
                        cmdText = cmdText.Replace("$" + parameter.Key, paramChar + parameter.Key);
                    else
                        cmdText.AppendFormat(" and {0}={1}{0}", parameter.Key, paramChar);

                    AddParameter(cmd, parameter.Key, parameter.Value);
                }
            }

            cmd.CommandText = cmdText.ToString();
        }

        public DataSet ExecuteProcDataSet(IEAConnection eaconn, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            {
                using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
                {
                    AddParameters(eaconn.DatabaseType, cmd, parameters);

                    return new ADODataAdapter().FillDataSet(cmd);
                }
            }
        }

        public DataTable ExecuteProcDataTable(IEAConnection eaconn, string procName, IDictionary<string, object> parameters)
        {
            DataSet ds = ExecuteProcDataSet(eaconn, procName, parameters);

            if (ds == null || ds.Tables.Count == 0)
                return null;

            return ds.Tables[0];
        }

        public object ExecuteProcScalar(IEAConnection eaconn, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);

                return cmd.ExecuteScalar();
            }
        }

        public void ExecuteProcNonQuery(IEAConnection eaconn, string procName, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(procName, conn, CommandType.StoredProcedure))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);

                cmd.ExecuteNonQuery();
            }
        }

        public DataSet ExecuteDataSet(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);

                return new ADODataAdapter().FillDataSet(cmd);
            }
        }

        public DataSet ExecuteDataSet(IEAConnection eaconn, string cmdText)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                DataSet ds = new DataSet();

                return new ADODataAdapter().FillDataSet(cmd);
            }
        }

        public DataTable ExecuteDataTable(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters)
        {
            DataSet ds = ExecuteDataSet(eaconn, cmdText, parameters);

            if (ds == null || ds.Tables.Count == 0)
                return null;

            return ds.Tables[0];
        }

        /// <summary>
        /// 计算查询结果数
        /// </summary>
        /// <param name="eaconn"></param>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public long Count(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);

                cmd.CommandText = string.Format("select count(*) from ({0})T ", cmd.CommandText);

                return cmd.ExecuteScalar().ToLong();
            }
        }

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="cmdText">sql命令</param>
        /// <param name="parameters">条件参数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters, string orderBy, PageInfo pageInfo)
        {
            pageInfo.ItemCount = Count(eaconn, cmdText, parameters);

            if (!string.IsNullOrEmpty(orderBy) && !orderBy.Replace(" ", "").ToLower().StartsWith("orderby"))
            {
                orderBy = string.Format("order by {0} ", orderBy);
            }

            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                //AddParameters(eaconn.DatabaseType, cmd, parameters);
                //cmd.CommandText = string.Format("select count(*) from ({0})T ", cmd.CommandText);

                AddParameters(eaconn.DatabaseType, cmd, parameters);

                string paramChar = DataUtil.GetParamChar(eaconn.DatabaseType);
                parameters.SafeAdd(paramChar + "PageIndex", pageInfo.PageIndex);
                parameters.SafeAdd(paramChar + "PageSize", pageInfo.PageSize);

                if (eaconn.DatabaseType == DatabaseType.MySQL)
                {
                    cmd.CommandText = string.Format("{0} {1} limit {2}, {3} ", cmd.CommandText, orderBy, (pageInfo.PageIndex - 1) * pageInfo.PageSize, pageInfo.PageSize);
                }
                else if (eaconn.DatabaseType == DatabaseType.Oracle)
                {
                    cmd.CommandText = string.Format("SELECT * FROM(SELECT ROWNUM RN,T.* FROM ({0} {2})T WHERE ROWNUM <= {1}PageIndex*{1}PageSize) WHERE RN >=({1}PageIndex-1)*{1}PageSize+1", cmd.CommandText, paramChar, orderBy);
                }
                else
                {
                    cmd.CommandText = string.Format("select Top ({0}PageSize) * from (select  ROW_NUMBER() OVER( {2} ) AS row_number, * from ({1}) P ) T Where row_number>({0}PageIndex-1)*{0}PageSize", paramChar, cmd.CommandText, string.IsNullOrEmpty(orderBy) ? "ORDER BY CURRENT_TIMESTAMP" : orderBy);
                }

                AddParameter(cmd, "PageIndex", pageInfo.PageIndex);
                AddParameter(cmd, "PageSize", pageInfo.PageSize);
                DataSet ds = new ADODataAdapter().FillDataSet(cmd);

                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
        }

        public DataTable ExecuteDataTable(IEAConnection eaconn, string cmdText)
        {
            DataSet ds = ExecuteDataSet(eaconn, cmdText);

            if (ds == null || ds.Tables.Count == 0)
                return null;

            return ds.Tables[0];
        }

        public DataTable ExecuteTop1(IEAConnection eaconn, string cmdText)
        {
            if (eaconn.DatabaseType == DatabaseType.Oracle)
            {
                return ExecuteDataTable(eaconn, string.Format("select * from ({0}) T where rownum=1", cmdText));
            }

            return ExecuteDataTable(eaconn, string.Format("select top 1 * from ({0}) T ", cmdText));
        }

        public object ExecuteScalar(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);
                return cmd.ExecuteScalar();
            }
        }


        public object ExecuteScalar(IEAConnection eaconn, string cmdText)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                return cmd.ExecuteScalar();
            }
        }

        public object ExecuteScalarWithTrans(IEAConnection eaconn, string cmdText)
        {
            object value = null;

            using (IDbConnection conn = eaconn.Open())
            {
                using (System.Data.IDbTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
                    {
                        try
                        {
                            cmd.Transaction = trans;
                            value = cmd.ExecuteScalar();
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            GlobalLogger.Error<DataContext>(string.Format("执行sql={0}出错", cmdText), ex);
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }

            return value.ToSafeString();
        }

        public void ExecuteNonQuery(IEAConnection eaconn, string cmdText, IDictionary<string, object> parameters)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                AddParameters(eaconn.DatabaseType, cmd, parameters);

                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteNonQuery(IEAConnection eaconn, string cmdText)
        {
            using (IDbConnection conn = eaconn.Open())
            using (IDbCommand cmd = PrepareCommand(cmdText, conn, CommandType.Text))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Insert(IEAConnection eaconn, DataTable dt, IDictionary<string, string> mappings)
        {
            if (eaconn.DatabaseType != DatabaseType.Oracle)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(eaconn.ConnectionString))
                {
                    foreach (KeyValuePair<string, string> item in mappings)
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(item.Key, item.Value));
                    }
                    bulkCopy.DestinationTableName = dt.TableName;
                    bulkCopy.WriteToServer(dt);
                }

                return;
            }

            if (!string.IsNullOrEmpty(dt.TableName))
                ExecuteNonQuery(eaconn, string.Format("alter   table   {0}   NOLOGGING", dt.TableName.ToUpper()));

            foreach (DataRow dr in dt.Rows)
            {
                IDictionary<string, object> parameters = mappings.Values.ToDictionary(field => field, field => dr[field]);

                ExecuteNonQuery(eaconn, @"INSERT INTO DD_BatchValue
                                               (ID
                                               ,BatchDataID
                                               ,Value
                                               ,CreateTime
                                               ,Creator)
                                         VALUES
                                               (:ID,
                                                :BatchDataID,
			                                    :Value,
			                                    to_date(:CreateTime, 'yyyy-mm-dd hh24:mi:ss'),
                                                :Creator)", parameters);
            }

            if (!string.IsNullOrEmpty(dt.TableName))
                ExecuteNonQuery(eaconn, string.Format("alter   table   {0}   LOGGING", dt.TableName.ToUpper()));
        }

        public void CreateTable(IEAConnection eaconn, string tableName, string cmdText)
        {
            string sqlStr = string.Empty;
            if (eaconn.DatabaseType != DatabaseType.Oracle)
            {
                sqlStr = string.Format("SELECT top 1 table_name as [Name] FROM INFORMATION_SCHEMA.TABLES where table_name=@table_name");
            }
            else
            {
                tableName = tableName.ToUpper();
                sqlStr = string.Format("select table_name as Name from user_tables where table_name=:table_name");
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("table_name", tableName);

            if (ExecuteScalar(eaconn, sqlStr, parameters) == null)
            {
                ExecuteNonQuery(eaconn, cmdText);
            }
        }

        #endregion

        #endregion

        #region Internal Help
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
