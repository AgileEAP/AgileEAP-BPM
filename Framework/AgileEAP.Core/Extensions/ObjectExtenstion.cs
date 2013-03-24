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
using System.Reflection;
using System.Linq;
using System.Data;
using System.Data.SqlClient;



namespace AgileEAP.Core.Extensions
{
    public static class ObjectExtension
    {
        static ILogger logger = LogManager.GetLogger(typeof(ObjectExtension));

        /// <summary>
        /// 字符串转换为int,出错时返回defaultValue
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this object objValue, int defaultValue)
        {
            if (objValue == null)
                return defaultValue;

            return objValue.ToString().ToInt();
        }

        /// <summary>
        /// 字符串转换为long,出错时返回0
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static int ToInt(this object objValue)
        {
            return ToInt(objValue, 0);
        }

        /// <summary>
        /// 字符串转换为long,出错时返回defaultValue
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this object objValue, long defaultValue)
        {
            if (objValue == null)
                return defaultValue;

            return objValue.ToString().ToLong();
        }

        /// <summary>
        /// 字符串转换为int,出错时返回0
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static long ToLong(this object objValue)
        {
            return ToLong(objValue, 0);
        }

        /// <summary>
        /// 字符串转换为double,出错时返回defaultValue
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object objValue, double defaultValue)
        {
            if (objValue == null)
                return defaultValue;

            return objValue.ToString().ToDouble();
        }

        /// <summary>
        /// 字符串转换为double,出错时返回0
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object objValue)
        {
            return ToDouble(objValue, 0.00);
        }

        public static string ToSafeString(this object objValue)
        {
            string value = string.Empty;

            if (objValue != null) value = objValue.ToString();

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="objValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static TValue GetPropertyValue<TValue>(this object objValue, string propertyName)
        {
            if (objValue == null) return default(TValue);

            Type type = objValue.GetType();
            if (type.IsClass)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null) return default(TValue);

                object value = property.GetValue(objValue, null);

                if (value == null) return default(TValue);

                if (typeof(TValue) == typeof(Guid)) value = new Guid(value.ToSafeString());

                try
                {
                    return (TValue)Convert.ChangeType(value, typeof(TValue));
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }

            return default(TValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="objValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static void SetPropertyValue(this object objValue, string propertyName, object value)
        {
            if (objValue == null)
            {

                Type type = objValue.GetType();
                if (type.IsClass)
                {
                    PropertyInfo property = type.GetProperty(propertyName);
                    if (property == null)
                    {
                        property.SetValue(objValue, value, null);
                    }
                }
            }
        }

        //public static string GetParamChar(this IDbCommand cmd)
        //{
        //    if (cmd == null) return "@";

        //    return cmd != null && cmd is SqlCommand ? "@" : ":";
        //}

        public static string ToJson(this object targetObject)
        {
            return targetObject != null ? Newtonsoft.Json.JsonConvert.SerializeObject(targetObject) : string.Empty;
        }

        /// <summary>
        /// 把json格式的字符串，反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列化为对象类型</typeparam>
        /// <param name="jsonValue">json格式的字符串</param>
        /// <returns>反序列化为对象</returns>
        public static T DeserializeObject<T>(this string jsonValue) where T : class
        {
            if (!string.IsNullOrEmpty(jsonValue))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonValue);
            }

            return default(T);
        }

        /// <summary>
        /// 构造递归Lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="targetObject"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Func<T, TResult> MakeRecursion<T, TResult>(this object targetObject, Func<Func<T, TResult>, Func<T, TResult>> fun)
        {
            return x => fun(MakeRecursion(targetObject, fun))(x);
        }


        public static object ConvertTo(this object originalValue, Type targetType)
        {
            Object convertedValue = null;
            if (originalValue != null)
            {
                if (targetType.IsAssignableFrom(originalValue.GetType()))
                {
                    return originalValue;
                }

                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
                if (tc.CanConvertFrom(originalValue.GetType()))
                {
                    convertedValue = tc.ConvertFrom(originalValue);
                }
                else
                {
                    convertedValue = tc.ConvertFromString(originalValue.ToString());
                }
            }

            return convertedValue;
        }

        public static T ConvertTo<T>(this object originalValue, T defaultValue = default(T))
        {
            if (originalValue == null) return defaultValue;

            Type targetType = typeof(T);
            Object convertedValue = originalValue.ConvertTo(targetType);

            return (T)(convertedValue ?? defaultValue);
        }
    }
}
