using System;
using System.Collections.Generic;
using System.Text;
using EntCaching = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace AgileEAP.Core.Caching.Expirations
{
    
    [Serializable]
    /// <summary>
    /// 扩展格式过期
    /// </summary>
    /// <remarks>
    /// 扩展格式语法 : <br/><br/>
    /// 
    /// 分钟       - 0-59 <br/>
    /// 小时         - 0-23 <br/>
    /// 一个月的第几天 - 1-31 <br/>
    /// 月份        - 1-12 <br/>
    /// 一周的第几天  - 0-6 (星期天是 0) <br/>
    /// 通配符    - * 代表每 <br/>
    /// 例子： <br/>
    /// * * * * *    - 每分钟过期<br/>
    /// 5 * * * *    - 每小时的第5分钟过期 <br/>
    /// * 21 * * *   - 每天的第21小时的每分钟过期 <br/>
    /// 31 15 * * *  - 每天下午的3：31过期 <br/>
    /// 7 4 * * 6    - 每周六的凌晨 4:07 过期 <br/>
    /// 15 21 4 7 *  - 7月4日晚上 9:15 过期 <br/>
    ///	6 6 6 6 1    - 
    ///	第6分钟 AND 第6天 AND 第6月 AND 周一过期
    /// </remarks>
    public class ExtendedFormatTime : ExpirationPolicy
    {
        public ExtendedFormatTime(string format)
        {
            expiration = new EntCaching.ExtendedFormatTime(format);
        }
    }
}
