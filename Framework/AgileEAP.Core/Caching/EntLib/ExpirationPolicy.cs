using System;
using System.Collections.Generic;
using System.Text;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching;

namespace AgileEAP.Core.Caching
{
    /// <summary>
    /// 缓存数据项过期接口
    /// </summary>
    public abstract class ExpirationPolicy
    {
        protected EntCaching.ICacheItemExpiration expiration = null;
        public EntCaching.ICacheItemExpiration GetExpiration()
        {
            return expiration;
        }
    }
}
