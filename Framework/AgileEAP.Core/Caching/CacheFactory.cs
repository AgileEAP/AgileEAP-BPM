using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Caching
{
    internal class CacheFactory
    {
        public static ICache CreateCache<T>() where T : ICache, new()
        {
            return new T();
        }

        public static ICache CreateCache(string cacheName)
        {
            Type type = Type.GetType(string.Format("AgileEAP.Core.Caching.{0}", cacheName));
            return Activator.CreateInstance(type) as ICache;
        }
    }
}
