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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace AgileEAP.Core.Data
{
    public class DataUtil
    {
        public static List<TEntity> ToEntityList<TEntity>(IList list) where TEntity : class
        {
            List<TEntity> result = new List<TEntity>();

            foreach (object item in list)
            {
                result.Add(item as TEntity);
            }

            return result;
        }

        /// <summary>
        /// 转换集合信息
        /// </summary>
        /// <typeparam name="DestT">目标类型</typeparam>
        /// <param name="objList">要转换的集合</param>
        /// <returns></returns>
        public static IList<DestT> ToGenericList<DestT>(object objList) where DestT : class, new()
        {
            Type srcType = objList.GetType().GetGenericArguments()[0];
            if (objList == null) return null;

            IList<DestT> result = new List<DestT>();

            Type destType = typeof(DestT);

            PropertyInfo[] properties = srcType.GetProperties();
            if (objList is IEnumerable)
            {
                IEnumerable srcList = (IEnumerable)objList;
                foreach (var item in srcList)
                {
                    var destItem = new DestT();
                    foreach (var property in properties)
                    {
                        object value = property.GetValue(item, null);
                        var destProperty = destType.GetProperty(property.Name);
                        if (destProperty != null)
                            destProperty.SetValue(destItem, value, null);
                    }
                    result.Add(destItem);
                }

                //FieldInfo[] fields = srcType.GetFields();
                //foreach (var item in srcList)
                //{
                //    var destItem = new DestT();
                //    foreach (var filed in fields)
                //    {
                //        object value = filed.GetValue(item);
                //        var destFiled = destType.GetField(filed.Name);
                //        if (destFiled != null)
                //            destFiled.SetValue(destItem, value);
                //    }
                //    result.Add(destItem);
                //}
            }
            return result;
        }


        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="srcDT">数据</param>
        /// <param name="relation">数据库表列名与对象属性名对应关系；如果列名与实体对象属性名相同，该参数可为空</param>
        /// <returns>对象列表</returns>
        public static List<T> ToList<T>(DataTable srcDT, Hashtable relation)
        {
            List<T> list = new List<T>();
            T destObj = default(T);
            if (srcDT != null && srcDT.Rows.Count > 0)
            {
                foreach (DataRow row in srcDT.Rows)
                {
                    destObj = ToEntity<T>(row, relation);
                    if (destObj != null)
                        list.Add(destObj);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="srcDT">数据</param>
        /// <returns>对象列表</returns>
        public static List<T> ToList<T>(DataTable srcDT)
        {
            List<T> list = new List<T>();
            T destObj = default(T);

            if (srcDT != null && srcDT.Rows.Count > 0)
            {
                foreach (DataRow row in srcDT.Rows)
                {
                    destObj = ToEntity<T>(row, null);
                    if (destObj != null)
                        list.Add(destObj);
                }
            }

            return list;
        }

        /// <summary>
        ///  将数据行转换成数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public static T ToEntity<T>(DataRow row, Hashtable relation)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            PropertyInfo temp = null;
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name) &&
                    row[prop.Name] != DBNull.Value)
                {
                    SetPropertyValue(prop, destObj, row[prop.Name]);
                }
            }

            if (relation != null)
            {

                foreach (string name in relation.Keys)
                {
                    temp = type.GetProperty(relation[name].ToString());
                    if (temp != null &&
                        row[name] != DBNull.Value)
                    {
                        SetPropertyValue(temp, destObj, row[name]);
                    }
                }
            }

            return destObj;
        }

        /// <summary>
        /// 为对象的属性赋值
        /// </summary>
        /// <param name="prop">属性</param>
        /// <param name="destObj">目标对象</param>
        /// <param name="value">源值</param>
        private static void SetPropertyValue(PropertyInfo prop, object destObj, object value)
        {
            object temp = ChangeType(prop.PropertyType, value);
            prop.SetValue(destObj, temp, null);
        }

        /// <summary>
        /// 用于类型数据的赋值
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="value">原值</param>
        /// <returns></returns>
        private static object ChangeType(Type type, object value)
        {
            int temp = 0;
            if ((value == null) && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }
            if (value == null)
            {
                return null;
            }
            if (type == value.GetType())
            {
                return value;
            }
            if (type.IsEnum)
            {
                if (value is string)
                {
                    return Enum.Parse(type, value as string);
                }
                return Enum.ToObject(type, value);
            }

            if (type == typeof(bool) && typeof(int).IsInstanceOfType(value))
            {
                temp = int.Parse(value.ToString());
                return temp != 0;
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type type1 = type.GetGenericArguments()[0];
                object obj1 = ChangeType(type1, value);
                return Activator.CreateInstance(type, new object[] { obj1 });
            }
            if ((value is string) && (type == typeof(Guid)))
            {
                return new Guid(value as string);
            }
            if ((value is string) && (type == typeof(Version)))
            {
                return new Version(value as string);
            }
            if (!(value is IConvertible))
            {
                return value;
            }
            return Convert.ChangeType(value, type);
        }

        public static string GetParamChar(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.Oracle)
                return ":";

            if (databaseType == DatabaseType.MySQL)
                return "?";

            return "@";
        }
    }
}
