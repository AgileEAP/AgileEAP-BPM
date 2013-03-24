using System;
using System.Collections.Generic;
using System.Text;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace AgileEAP.Core.Caching.Expirations
{
    /// <summary>
    /// 文件依赖过期
    /// </summary>
    [Serializable]
    public class FileDependency : ExpirationPolicy
    {
        public FileDependency(string fullFileName) 
        {
            expiration = new EntCaching.FileDependency(fullFileName);
        }
    }
}
