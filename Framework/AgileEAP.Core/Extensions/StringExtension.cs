using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;

namespace AgileEAP.Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 字符串转换为int,出错时返回defaultValue
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short ToShort(this string text, short defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            short value = 0;

            if (short.TryParse(text, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// 字符串转换为long,出错时返回0
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static short ToShort(this string text)
        {
            return ToShort(text, 0);
        }

        /// <summary>
        /// 字符串转换为int,出错时返回defaultValue
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string text, int defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            int value = 0;

            if (int.TryParse(text, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// 字符串转换为long,出错时返回0
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ToInt(this string text)
        {
            return ToInt(text, 0);
        }

        /// <summary>
        /// 字符串转换为long,出错时返回defaultValue
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this string text, long defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            long value = 0;

            if (long.TryParse(text, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// 字符串转换为int,出错时返回0
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static long ToLong(this string text)
        {
            return ToLong(text, 0);
        }

        /// <summary>
        /// 字符串转换为double,出错时返回defaultValue
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string text, double defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            double value = 0;

            if (double.TryParse(text, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// 字符串转换为double,出错时返回0
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ToDouble(this string text)
        {
            return ToDouble(text, 0.00);
        }

        /// <summary>
        /// 字符串转换为DateTime,出错时返回当前时间
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string text)
        {
            return ToDateTime(text, DateTime.Now);
        }

        /// <summary>
        /// 字符串转换为DateTime,出错时返回默认值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string text, DateTime defaultValue)
        {
            DateTime result = defaultValue;

            return DateTime.TryParse(text, out result) ? result : defaultValue;
        }

        public static string ToContactString(this string[] array)
        {
            if (array == null) return string.Empty;

            StringBuilder result = new StringBuilder();

            foreach (string item in array)
            {
                if (!string.IsNullOrEmpty(item))
                    result.Append(item).Append(",");
            }

            return result.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 把为null,"","null"字符串都转换为ststring.Empty
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToEmpty(this string text)
        {
            if (text == null || text == "" || text == "null")
            {
                text = string.Empty;

            }
            return text;
        }

        /// <summary>
        ///比较忽悠大小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool EqualIgnoreCase(this string text, string value)
        {
            return string.Equals(text, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 把格式是{小时:分钟:秒}字符串转换为TimeSpan
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string text)
        {
            string[] values = text.Split(':');

            if (values.Length == 3)
                return new TimeSpan(values[0].ToInt(), values[1].ToInt(), values[2].ToInt());
            else if (values.Length == 4)
                return new TimeSpan(values[0].ToInt(), values[1].ToInt(), values[2].ToInt(), values[3].ToInt());

            throw new FormatException(string.Format("{0}不是有效的TimeSpan格式，正确格式是{{小时:分钟:秒}}或{{天:小时:分钟:秒}}", text));
        }

        public static string GetUerName(this string id)
        {
            DataContext dataContext = new DataContext();
            IEAConnection conn = UnitOfWork.GetEAConnection();
            object value = dataContext.ExecuteScalar(conn, string.Format("select top 1 UserName from AC_Operator where id='{0}'", id));
            if (value == null) value = dataContext.ExecuteScalar(conn, string.Format("select top 1 UserName from AC_Operator where LoginName='{0}'", id));

            return value.ToSafeString();
        }

    }
}
