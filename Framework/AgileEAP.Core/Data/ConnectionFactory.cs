using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace AgileEAP.Core.Data
{
    public sealed class ConnectionFactory
    {
        public static IDbConnection CreateConnection(DatabaseType databaseType, string connectionString)
        {
            //string connectionClassName = databaseType != DatabaseType.Oracle ? "System.Data.SqlClient.SqlConnection" : "Oracle.DataAccess.Client.OracleConnection";
            //return Activator.CreateInstance(Type.GetType(connectionClassName), new object[] { connectionString }) as IDbConnection; 

            if (databaseType != DatabaseType.Oracle) return new SqlConnection(connectionString);

#if SuportOracle
            return new Oracle.DataAccess.Client.OracleConnection(connectionString);
#endif

            return null;
        }
    }
}
