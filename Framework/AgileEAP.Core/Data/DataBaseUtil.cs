using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using AgileEAP.Core.ExceptionHandler;

namespace AgileEAP.Core.Data
{
    public static class DataBaseUtil
    {
        static ILogger logger = LogManager.GetLogger(typeof(DataBaseUtil));
        private static IDictionary<string, string> cache = new Dictionary<string, string>();

        /// <summary>
        /// 根据程序集在配置文件中查找数据库名称
        /// </summary>
        /// <param name="assemlbyName"></param>
        /// <returns></returns>
        public static string GetDataBaseName(string assemlbyName)
        {
            if (cache.ContainsKey(assemlbyName)) return cache[assemlbyName];

            IList<Database> databases = DatabaseManager.Instance.Databases;
            foreach (Database database in databases)
            {
                ICollection<PersistentClass> classMappings = database.NHConfiguration.ClassMappings;
                if (classMappings != null && classMappings.Count > 0)
                {
                    PersistentClass first = classMappings.FirstOrDefault(o => o.MappedClass.Assembly.FullName.Equals(assemlbyName, StringComparison.OrdinalIgnoreCase));
                    if (first != null)
                    {
                        cache.Add(assemlbyName, database.Name);
                        return database.Name;
                    }
                }
            }

            throw new EAPException(string.Format("程序集{0}没有配置持久化数据库！", assemlbyName));
        }

        /// <summary>
        /// 根据类型获取表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            string cacheKey = string.Format("MappingTable_{0}", type.FullName);
            if (cache.ContainsKey(cacheKey)) return cache[cacheKey];

            IList<Database> databases = DatabaseManager.Instance.Databases;
            foreach (Database database in databases)
            {
                ICollection<PersistentClass> classMappings = database.NHConfiguration.ClassMappings;
                if (classMappings != null && classMappings.Count > 0)
                {
                    PersistentClass first = classMappings.FirstOrDefault(o => o.EntityName.Equals(type.FullName, StringComparison.OrdinalIgnoreCase));
                    if (first != null)
                    {
                        logger.Debug(string.Format("classname={0},type.fullname={1},tablename={2}", first.ClassName, type.FullName, first.Table.Name));
                        if (!cache.ContainsKey(cacheKey))
                            cache.Add(cacheKey, first.Table.Name);

                        return first.Table.Name;
                    }
                }
            }

            throw new EAPException(string.Format("程序集{0}没有配置持久化数据库！", type));
        }
    }
}
