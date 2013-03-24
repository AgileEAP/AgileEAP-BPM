#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

using AgileEAP.Core.Utility;

namespace AgileEAP.Core.Web
{
    /// <summary>
    /// 模板页面的基类
    /// </summary>
    public class BaseMasterPage : MasterPage
    {
        /// <summary>
        /// 网站Url的根路径
        /// </summary>
        public string RootPath
        {
            get
            {
                return WebUtil.GetRootPath();
            }

        }

        /// <summary>
        /// 列表高度
        /// </summary>
        public string GVHeight
        {
            get;
            set;

        }

        /// <summary>
        /// 新的会话包装对象
        /// </summary>
        public new SessionWrapper Session
        {
            get
            {
                return new SessionWrapper(this.Page);
            }
        }

        /// <summary>
        /// 读Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string ReadCookie(string key)
        {
            HttpCookie cookie = Request.Cookies[key];

            return cookie == null ? string.Empty : Server.UrlDecode(cookie.Value);
        }

        /// <summary>
        /// 写Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void WriteCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, Server.UrlEncode(value));

            Response.Cookies.Add(cookie);
        }
    }
}
