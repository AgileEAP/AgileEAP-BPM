using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;


using NHibernate;
using NHibernate.Driver;
using NHibernate.Util;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Core.Data
{
    public interface IEAConnection
    {
        DatabaseType DatabaseType
        {
            get;
            set;
        }

        string ConnectionString
        {
            get;
            set;
        }

        IDbConnection DbConnection
        {
            get;
            set;
        }

        IDbConnection Open();
    }

    public class EAConnection : IEAConnection
    {
        public DatabaseType DatabaseType
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public string DataDriver
        {
            get;
            set;
        }


        public IDbConnection DbConnection
        {
            get;
            set;
        }

        public IDbConnection Open()
        {
            if (DbConnection == null)
            {
                Type type = ReflectHelper.TypeFromAssembly(DataDriver, "NHibernate", false);
                NHibernate.Driver.IDriver driver = Activator.CreateInstance(type) as NHibernate.Driver.IDriver;
                DbConnection = driver.CreateConnection();
                DbConnection.ConnectionString = ConnectionString;
                DbConnection.Open();
            }
            else if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.ConnectionString = ConnectionString;
                DbConnection.Open();
            }

            return DbConnection;
        }
    }
}
