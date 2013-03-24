using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace AgileEAP.Core.Extensions
{
    public static class PageExtenstion
    {
        public static void Alert(this Page page, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
                page.ClientScript.RegisterStartupScript(page.GetType(), DateTime.Now.Ticks.ToString(), "alert('" + msg + "');", true);
        }

        public static void ClientRefresh(this Page page)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), DateTime.Now.Ticks.ToString(), "refresh();", true);
        }

        public static void ClientRefreshParent(this Page page)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), DateTime.Now.Ticks.ToString(), "refreshParent();", true);
        }

        public static void ClientRefreshAll(this Page page)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), DateTime.Now.Ticks.ToString(), "refreshAll();", true);
        }

    }
}
