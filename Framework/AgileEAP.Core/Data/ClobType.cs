using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AgileEAP.Core.Data
{
    public abstract class PatchForOracleCLobType : IUserType
    {
        public PatchForOracleCLobType()
        {
        }

        public bool IsMutable
        {
            get { return true; }
        }
        public System.Type ReturnedType
        {
            get { return typeof(String); }
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { NHibernateUtil.String.SqlType };
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }
        public new bool Equals(object x, object y)
        {
            return x == y;
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }
        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            return NHibernate.NHibernateUtil.StringClob.NullSafeGet(rs, names[0]);
        }

        public abstract void NullSafeSet(IDbCommand cmd, object value, int index);

        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }

    //配置 <property name="Content" column="Content" length="2147483647"  type=" AgileEAP.Core.Data.OracleClob, AgileEAP.Core"/>

    public class OracleClob : PatchForOracleCLobType
    {
        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
#if SuportOracle
            if (cmd is Oracle.DataAccess.Client.OracleCommand)
            {
                //CLob、NClob类型的字段，存入中文时参数的OracleDbType必须设置为OracleDbType.Clob 
                //否则会变成乱码（Oracle 10g client环境） 
                Oracle.DataAccess.Client.OracleParameter param = cmd.Parameters[index] as Oracle.DataAccess.Client.OracleParameter;
                if (param != null)
                {
                    param.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Clob;//.OracleType = OracleType.Clob;// 关键就这里啦 
                    param.IsNullable = true;
                }
            }
#endif
            NHibernate.NHibernateUtil.StringClob.NullSafeSet(cmd, value, index);
        }
    }

}
