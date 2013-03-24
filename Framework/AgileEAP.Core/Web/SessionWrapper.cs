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
using System.Web;
using System.Web.UI;
using System.Web.SessionState;


namespace AgileEAP.Core.Web
{
    /// <summary>
    /// Session包装类
    /// </summary>
    public class SessionWrapper
    {
        private HttpSessionState session;

        public SessionWrapper()
        {
            session = HttpContext.Current.Session;
        }

        public SessionWrapper(Page page)
        {
            session = page.Session;
        }

        public SessionWrapper(HttpSessionState session)
        {
            this.session = session;
        }

        /// <summary>
        /// 把对象添加到会话
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            session.Add(key, value);
        }

        /// <summary>
        /// 把对象从会话中移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            session.Remove(key);
        }

        /// <summary>
        /// 清除会话中的对象
        /// </summary>
        public void Clear()
        {
            session.Clear();
        }

        /// <summary>
        /// 获取会话对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return session[index];
            }
            set
            {
                session[index] = value;
            }
        }

        /// <summary>
        /// 获取会话对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return session[name];
            }
            set
            {
                session[name] = value;
            }
        }

        /// <summary>
        /// 获取会话对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name) where T : class
        {
            return session[name] as T;
        }
    }
}
