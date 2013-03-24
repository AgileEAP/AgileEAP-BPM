using System;
using System.Collections.Generic;
using System.Text;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace AgileEAP.Core.Caching.Expirations
{
    /// <summary>
    /// 从不过期
    /// </summary>
    [Serializable]
    public class NeverExpired : ExpirationPolicy
    {
        public NeverExpired()
        {
            expiration = new EntCaching.NeverExpired();
        }
    }
}
