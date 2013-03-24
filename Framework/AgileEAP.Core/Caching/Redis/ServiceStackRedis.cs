using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceStack.Common.Extensions;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Text;
using ServiceStack.Redis.Support;

namespace AgileEAP.Core.Caching
{
    public class ServiceStackRedis : ICache
    {
        protected PooledRedisClientManager prcm = null;

        public PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //支持读写分离，均衡负载
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5,//“写”链接池链接数
                MaxReadPoolSize = 5,//“写”链接池链接数
                AutoStart = true,
            });
        }

        public ServiceStackRedis()
        {
            // RedisClient redisClient = new RedisClient("", 6379, "123");

            string[] redisServers = Configure.DirectGet<string>("RedisServer", "172.16.70.15:6379").Split(';');
            prcm = CreateManager(redisServers, redisServers);
        }

        public T Get<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        public void Add(string key, object data, int cacheTime = 30)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.Add(key, data, TimeSpan.FromMinutes(cacheTime));
            }
        }

        public bool Contains(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.ContainsKey(key);
            }
        }

        public void Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.Remove(key);
            }
        }

        public void RemoveAll()
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.RemoveAll(redis.GetAllKeys());
            }
        }

        public int Count
        {
            get
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.GetAllKeys().Count;
                }
            }
        }
    }
}
