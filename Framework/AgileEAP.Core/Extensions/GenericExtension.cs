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


namespace AgileEAP.Core.Extensions
{
    /// <summary>
    /// 泛型扩展
    /// </summary>
    public static class GenericExtension
    {
        static ILogger logger = LogManager.GetLogger(typeof(GenericExtension));

        public static bool In<T>(this T value, IEnumerable<T> c)
        {
            return c.Any(i => i.Equals(value));
        }


        public static T Cast<T>(this string value)
        {
            return Cast<T>(value, default(T));
        }

        public static T Cast<T>(this object objValue, T defaultValue)
        {
            T result = default(T);
            try
            {
                if (objValue == null) return result;

                if (objValue is T)
                {
                    return (T)objValue;
                }

                if (typeof(T) == typeof(bool))
                {
                    object boolValue = objValue.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

                    return (T)boolValue;
                }
                else if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), objValue.ToString(), true);
                }
                else if (typeof(T) == typeof(Guid))
                {
                    Guid guid;
                    Guid.TryParse(objValue.ToSafeString(), out guid);
                    object objGuid = guid;
                    return (T)objGuid;
                }

                result = (T)Convert.ChangeType(objValue, typeof(T));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("把类型为{0}的值{1}转换为类型{2}时出错", objValue.GetType(), objValue, typeof(T)), ex);
                result = defaultValue;
            }

            return result;
        }

        public static T If<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class
        {
            if (t == null) throw new ArgumentNullException();
            if (predicate(t)) action(t);
            return t;
        }

        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : struct
        {
            return predicate(t) ? func(t) : t;
        }

        public static string If(this string s, Predicate<string> predicate, Func<string, string> func)
        {
            return predicate(s) ? func(s) : s;
        }

        public static T If<T>(this T t, Predicate<T> predicate, Action<T> action, Action<T> action2) where T : class
        {
            if (predicate(t))
                action(t);
            else
                action2(t);

            return t;
        }

        public static IDictionary<TKey, TValue> Action<TKey, TValue>(this IDictionary<TKey, TValue> t, Predicate<IDictionary<TKey, TValue>> predicate, Action<IDictionary<TKey, TValue>> action, Action<IDictionary<TKey, TValue>> action2)
        {
            if (predicate(t))
                action(t);
            else
                action2(t);

            return t;
        }

        public static IDictionary<TKey, TValue> SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> t, TKey key, TValue value)
        {
            if (t == null)
            {
                t = new Dictionary<TKey, TValue>();
            }

            return t.If(o => o.ContainsKey(key), o => o[key] = value, o => o.Add(key, value));
        }

        public static IDictionary<TKey, TValue> SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> t, KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (t == null)
            {
                t = new Dictionary<TKey, TValue>();
            }

            return t.If(o => o.ContainsKey(keyValuePair.Key), o => o[keyValuePair.Key] = keyValuePair.Value, o => o.Add(keyValuePair.Key, keyValuePair.Value));
        }

        public static IDictionary<TKey, TValue> SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> t, IDictionary<TKey, TValue> dict)
        {
            if (t == null)
            {
                t = new Dictionary<TKey, TValue>();
            }

            if (dict == null) return t;

            foreach (KeyValuePair<TKey, TValue> item in dict)
            {
                t.SafeAdd(item);
            }

            return t;
        }

        public static TResult GetSafeValue<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key, TResult defautValue)
        {
            TResult value = defautValue;
            TValue objValue;

            try
            {
                if (dict.TryGetValue(key, out objValue))
                {
                    if (objValue is TResult)
                    {
                        return (TResult)(object)objValue;
                    }

                    if (typeof(TResult) == typeof(bool))
                    {
                        object boolValue = objValue.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

                        return (TResult)boolValue;
                    }
                    else if (typeof(TResult).IsEnum)
                    {
                        return (TResult)Enum.Parse(typeof(TResult), objValue.ToString(), true);
                    }
                    else if (typeof(TResult) == typeof(Guid))
                    {
                        object guid = new Guid(objValue.ToString());

                        return (TResult)guid;
                    }

                    value = (TResult)Convert.ChangeType(objValue, typeof(TResult));
                }
            }
            catch (Exception ex)
            {
                value = defautValue;
                logger.Error(ex);
            }

            return value;
        }

        public static TResult GetSafeValue<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return GetSafeValue<TKey, TValue, TResult>(dict, key, default(TResult));
        }

        public static TResult GetSafeValue<TResult>(this IDictionary<string, object> dict, string key)
        {
            return GetSafeValue<string, object, TResult>(dict, key, default(TResult));
        }

        public static TResult GetSafeValue<TResult>(this IDictionary<string, object> dict, string key, TResult defaultValue)
        {
            return GetSafeValue<string, object, TResult>(dict, key, defaultValue);
        }

        public static TResult GetSafeValue<TResult>(this IDictionary<string, string> dict, string key)
        {
            return GetSafeValue<string, string, TResult>(dict, key, default(TResult));
        }

        public static TResult GetSafeValue<TResult>(this IDictionary<string, string> dict, string key, TResult defaultValue)
        {
            return GetSafeValue<string, string, TResult>(dict, key, defaultValue);
        }

        public static IList<TValue> SafeAdd<TValue>(this IList<TValue> list, TValue value)
        {
            if (list == null) list = new List<TValue>();

            if (!list.Contains(value)) list.Add(value);

            return list;
        }

        public static IList<TValue> SafeAdd<TValue>(this IList<TValue> list, IEnumerable<TValue> values)
        {
            if (list == null) list = new List<TValue>();

            foreach (var value in values)
            {
                list.SafeAdd(value);
            }

            return list;
        }

        public static void ForEach<T>(this IQueryable<T> source, Action<T> func)
        {
            foreach (var item in source)
                func(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            if (enumeration != null)
            {
                foreach (T item in enumeration)
                {
                    action(item);
                }
            }
        }

        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }

        public static string ToXml<TEntity>(this TEntity entity)
        {
            try
            {
                Type type = typeof(TEntity);
                if (entity == null)
                {
                    return string.Format("Type {0} object is null", type.Name);
                }

                StringBuilder xml = new StringBuilder();
                xml.AppendFormat("<{0}>", type.Name);
                foreach (var pi in type.GetProperties())
                {
                    string value = pi.GetValue(entity, null).ToSafeString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        xml.AppendFormat("<{0}>", pi.Name);
                        xml.Append(value);
                        xml.AppendFormat("<{0}/>", pi.Name);
                    }
                }
                xml.AppendFormat(string.Format("<{0}/>", type.Name));
                return xml.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return string.Empty;
        }
    }
}
