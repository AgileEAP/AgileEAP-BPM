using System;
using System.Collections.Generic;
using System.Text;
using Caching = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace AgileEAP.Core.Caching.Expirations
{
    /// <summary>
    /// 绝对时间过期
    /// </summary>
    [Serializable]
    public class AbsoluteTime : ExpirationPolicy
    {
        public AbsoluteTime(DateTime absoluteTime)
        {
            expiration = new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(absoluteTime);
        }

        public AbsoluteTime(TimeSpan timeFromNow)
        {
            expiration = new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(timeFromNow);
        }

    }
}
