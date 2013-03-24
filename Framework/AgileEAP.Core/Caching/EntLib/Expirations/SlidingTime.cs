using System;
using System.Collections.Generic;
using System.Text;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace AgileEAP.Core.Caching.Expirations
{
    /// <summary>
    ///  滑动过期
    /// </summary>
    [Serializable]
    public class SlidingTime : ExpirationPolicy
    {
        public SlidingTime(TimeSpan slidingExpiration)
        {
            expiration = new EntCaching.SlidingTime(slidingExpiration);
        }
    }
}
