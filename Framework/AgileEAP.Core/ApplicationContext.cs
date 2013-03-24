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
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

using AgileEAP.Core.Extensions;

namespace AgileEAP.Core
{
    [Serializable]
    public sealed class ApplicationContext
    {
        private static string ContextKey = "Application_Context_Key";
        public static readonly string CurrentUserKey = "Current_User_Key";
        private object lockSessions = new object();
        private object lockValues = new object();
        private IDictionary<string, object> values = null;
        private IDictionary<string, object> sessions = null;

        public static string IocCfgPartten = "*.cfg.ioc";
        public readonly static char[] MenuArgsSplitChar = { '$' };
        public readonly static char[] ArgumentSplitChar = { ',' };

        public static readonly int ValidateCodeLength = 4;
        public static readonly string Domain = "http://localhost";
        public static readonly string ValidateCodeKey = "Validate_Code_Key";
        public static readonly string IndexPage = "~/Default.aspx";
        public static readonly string CurrentId = "CurrentId";
        public readonly static string PageContextKey = "PageContext_Key";
        public static readonly string StartFilter = "filter";
        public static readonly string DefaultSkin = "Default";
        public static readonly string ErrorPage = "Error.aspx";
        public static readonly string ActiveMenuKey = "ActiveMenuId";
        public const string CurrentUserId = "CurrentUserId";//当前选中用户ID

        public static string Log4CfgFile
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return Path.Combine(HttpContext.Current.Server.MapPath("~"), "CfgFiles\\log4net.cfg.xml");
                }

                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CfgFiles\\log4net.cfg.xml");
            }
        }

        private ApplicationContext()
        {
            Initilize();
        }

        public IDictionary<string, object> Values
        {
            get
            {
                if (values == null)
                {
                    lock (lockValues)
                    {
                        if (values == null) values = new Dictionary<string, object>();
                    }
                }
                return values;
            }
        }

        public IDictionary<string, object> Sessions
        {
            get
            {
                if (sessions == null)
                {
                    lock (lockSessions)
                    {
                        if (sessions == null)
                            sessions = new Dictionary<string, object>();
                    }
                }

                return sessions;
            }
        }

        public static ApplicationContext Current
        {
            get
            {
                if (null != HttpContext.Current)
                {
                    //if (HttpContext.Current.Session != null)
                    //{
                    //    if (null == HttpContext.Current.Session[ContextKey])
                    //    {
                    //        HttpContext.Current.Session[ContextKey] = new ApplicationContext();
                    //    }

                    //    return HttpContext.Current.Session[ContextKey] as ApplicationContext;
                    //}
                    //else
                    //{
                    if (null == HttpContext.Current.Items[ContextKey])
                    {
                        HttpContext.Current.Items[ContextKey] = new ApplicationContext();
                    }

                    return HttpContext.Current.Items[ContextKey] as ApplicationContext;
                    //}
                }

                if (null == CallContext.GetData(ContextKey))
                {
                    CallContext.SetData(ContextKey, new ApplicationContext());
                }

                return CallContext.GetData(ContextKey) as ApplicationContext;
            }
        }

        public static bool IsWebApp
        {
            get
            {
                return null != HttpContext.Current;
            }
        }

        public static int CacheReportDataExpired
        {
            get
            {
                return ConfigurationManager.AppSettings["CacheReportDataExpired"].Cast<int>(15);
            }
        }

        void Initilize()
        {
            values = new Dictionary<string, object>();
            sessions = new Dictionary<string, object>();
        }
    }
}
