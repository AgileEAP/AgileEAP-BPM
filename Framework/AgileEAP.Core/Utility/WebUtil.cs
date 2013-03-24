using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

using AgileEAP.Core.Authentication;

namespace AgileEAP.Core.Utility
{
    public static class WebUtil
    {
        public static void PromptMsg(string message)
        {
            if (HttpContext.Current == null) return;

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Page), "alertMessage", string.Format("alert('{0}');", message), true);
            }
        }

        public static void ExecuteJs(string script)
        {
            if (HttpContext.Current == null) return;

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(typeof(Page), "ExecuteJs", script, true);
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="window">true表示窗口，false表示div</param>
        public static void CloseDialog()
        {

            if (HttpContext.Current == null) return;

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(typeof(Page), "closeWindow", "closeDialog(0);", true);
            }
        }

        public static void RefreshParent()
        {
            if (HttpContext.Current == null) return;

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(typeof(Page), "clientRefreshParent", "refreshParent()", true);
            }
        }

        public static void CloseRefreshParent()
        {
            if (HttpContext.Current == null) return;

            Page page = HttpContext.Current.Handler as Page;

            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(typeof(Page), "closeRefreshParent", "closeDialog(1);", true);
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (System.Web.HttpContext.Current == null)
                return "127.0.0.1";

            //绕过代理获取客户真实IP
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                string[] ips = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' });
                if (ips != null && ips.Length > 0)
                    return ips[0];
            }

            //返回默认IP
            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            appPath = (appPath.Length > 1 && !appPath.EndsWith("/")) ? appPath + "/" : "/";

            return string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, appPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSiteDirectory()
        {
            return HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);
        }

        public static string GetRequestFullUrl()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            string requestURL = appPath.Length == 1 ? absolutePath.Substring(1) : absolutePath.Substring(appPath.Length + 1);
            if (requestURL.Length > 1 && requestURL.StartsWith("/")) requestURL = requestURL.TrimStart('/');

            return requestURL;
        }

        public static User GetCurrentUser()
        {
            return HttpContext.Current.Session[ApplicationContext.CurrentUserKey] as User;
        }
    }
}
