using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Core.Caching
{

    public class EntLibCache : ICache
    {
        EntCaching.ICacheManager entLibCache = EntCaching.CacheFactory.GetCacheManager();

        public EntLibCache()
        {
            try
            {
                if (entLibCache == null)
                    entLibCache = EntCaching.CacheFactory.GetCacheManager();
            }
            catch (Exception ex)
            {
                ILogger logger = LogManager.GetLogger(typeof(EntLibCache));
                logger.Error("创建Cache对象出错!", ex);
            }
        }

        //[InjectionConstructor]
        //public Cache(string cacheManagerName)
        //{
        //    try
        //    {
        //        cacheManager = EntCaching.CacheFactory.GetCacheManager(cacheManagerName);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILogger logger = LogManager.GetLogger(typeof(Cache));
        //        logger.Error("创建Cache对象出错!", ex);
        //    }
        //}

        #region ICache
        /// <summary>
        /// 当前缓存数据项的个数
        /// </summary>
        public int Count
        {
            get { return entLibCache.Count; }
        }

        /// <summary>
        /// 如果缓存中已存在数据项键值，则返回true
        /// </summary>
        /// <param name="key">数据项键值</param>
        /// <returns>数据项是否存在</returns>
        public bool Contains(string key)
        {
            return entLibCache.Contains(key);
        }

        /// <summary>
        /// 根据键值返回缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return entLibCache.GetData(key); }
            set { entLibCache.Add(key, value); }
        }

        public object Get(string key)
        {
            return entLibCache.GetData(key);
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (entLibCache.Contains(key))
                return (T)entLibCache[key];

            return default(T);
        }

        /// <summary>
        /// 添加缓存数据。
        /// 如果另一个相同键值的数据已经存在，原数据项将被删除，新数据项被添加。
        /// </summary>
        /// <param name="key">缓存数据的键值</param>
        /// <param name="value">缓存的数据，可以为null值</param>
        public void Add(string key, object value, int cacheTime = 600)
        {
            Add(key, value, CacheItemPriority.Normal, new Expirations.AbsoluteTime(new TimeSpan(1, cacheTime, 0)));
        }

        /// <summary>
        /// 添加缓存数据。
        /// 如果另一个相同键值的数据已经存在，原数据项将被删除，新数据项被添加。
        /// </summary>
        /// <param name="key">缓存数据的键值</param>
        /// <param name="value">缓存的数据，可以为null值</param>
        /// <param name="expiratTime">缓存过期时间间隔</param>
        public void Add(string key, object value, TimeSpan expiratTime)
        {
            Add(key, value, CacheItemPriority.Normal, new Expirations.AbsoluteTime(expiratTime));
        }

        /// <summary>
        /// 添加缓存数据。
        /// </summary>
        /// <param name="key">缓存数据的键值</param>
        /// <param name="value">缓存的数据，可以为null值</param>
        /// <param name="policy">数据过期策略(AbsoluteTime/ExtendedFormat/FileDependency/NeverExpired/SlidingTime)，可以为空</param>
        public void Add(string key, object value, ExpirationPolicy policy)
        {
            Add(key, value, CacheItemPriority.Normal, policy);
        }

        /// <summary>
        /// 添加缓存数据。
        /// 如果另一个相同键值的数据已经存在，原数据项将被删除，新数据项被添加。
        /// </summary>
        /// <param name="key">缓存数据的键值</param>
        /// <param name="value">缓存的数据，可以为null值</param>
        /// <param name="scavengingPriority">缓存数据清除优先级</param>
        /// <param name="policy">数据过期策略(AbsoluteTime/ExtendedFormat/FileDependency/NeverExpired/SlidingTime)，可以为空</param>
        public void Add(string key, object value, CacheItemPriority scavengingPriority, ExpirationPolicy policy)
        {
            int priority = (int)scavengingPriority;

            if (policy == null)
                entLibCache.Add(key, value, (EntCaching.CacheItemPriority)priority, null);
            else
                entLibCache.Add(key, value, (EntCaching.CacheItemPriority)priority, null, policy.GetExpiration());
        }

        /// <summary>
        /// 删除缓存数据项
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            entLibCache.Remove(key);
        }

        /// <summary>
        /// 删除所有缓存数据项
        /// </summary>
        public void RemoveAll()
        {
            entLibCache.Flush();
        }
        #endregion

    }
}
