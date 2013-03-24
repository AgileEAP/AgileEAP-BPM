using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Core
{
    public static class IdGenerator
    {
        public static Guid VoidGuid
        {
            get { return new Guid(); }
        }

        ///<summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static Guid NewComb()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        /// <summary>
        /// 产生Guid
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 从 SQL SERVER 返回的 GUID 中生成时间信息
        /// </summary>
        /// <param name="guid">包含时间信息的 COMB </param>
        /// <returns>时间</returns>
        public static DateTime GetDateFromComb(System.Guid guid)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        /// <summary>
        /// 把字符串转换为Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ParseToGuid(string value)
        {
            if (string.IsNullOrEmpty(value)) return VoidGuid;

            Guid result;
            try
            {
                result = new Guid(value);
            }
            catch { result = VoidGuid; }

            return result;
        }

        /// <summary>
        /// 判断字符串是否是Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsGuid(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Equals("null", StringComparison.OrdinalIgnoreCase))
                return false;

            Guid result;
            try
            {
                result = new Guid(value);

                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 获取UUID
        /// </summary>
        /// <param name="code">种子编码</param>
        /// <returns></returns>
        public static string NewUUID(string code)
        {

            string sql = string.Empty;
            IEAConnection conn = UnitOfWork.GetEAConnection();
            if (conn.DatabaseType == DatabaseType.MySQL)
            {
                sql = string.Format(@"select IFNULL(Prefix,'')+right(cast(power(10,Length) as char)+CurrentValue+Step,Length)+IFNULL(Suffix,'') from AB_UUID where Code='{0}';
update AB_UUID set CurrentValue=CurrentValue+Step where Code='{0}';", code);
            }
            else if (conn.DatabaseType == DatabaseType.Oracle)
            {
                //            substr(str,length(str) - len + 1,len)
                sql = string.Format(@"select NVL(Prefix,'')||TO_CHAR(power(10,Length))||TO_CHAR(CurrentValue+Step)||NVL(Suffix,'') from AB_UUID where Code='{0}';
update AB_UUID set CurrentValue=CurrentValue+Step where Code='{0}'", code);
            }
            else
            {
                sql = string.Format(@"select Prefix+right(cast(power(10,Length) as varchar)+CurrentValue+Step,Length)+Suffix from AB_UUID where Code='{0}'
update AB_UUID set CurrentValue=CurrentValue+Step where Code='{0}'", code);
            }

            return new DataContext().ExecuteScalarWithTrans(conn, sql).ToSafeString();
        }

        /// <summary>
        /// 获取种子当前值
        /// </summary>
        /// <param name="code">种子编码</param>
        /// <returns></returns>
        public static int NewValue(string code)
        {
            string sql = string.Format(@"select CurrentValue+Step from AB_UUID where Code='{0}';
update AB_UUID set CurrentValue=CurrentValue+Step where Code='{0}';", code);

            return new DataContext().ExecuteScalarWithTrans(UnitOfWork.GetEAConnection(), sql).ToInt(1);
        }

    }
}
