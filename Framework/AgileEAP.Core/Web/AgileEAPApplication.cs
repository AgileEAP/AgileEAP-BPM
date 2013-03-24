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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Routing;
using AgileEAP.Core;

namespace AgileEAP.Core.Web
{
    public class AgileEAPApplication : HttpApplication
    {
        protected ILogger logger = LogManager.GetLogger(typeof(AgileEAPApplication));

        protected void Application_Start()
        {
            Start();
        }

        protected void Application_End()
        {
            Stop();
        }

        protected virtual void Start()
        {
            try
            {
                FileInfo log4CfgFile = new FileInfo(ApplicationContext.Log4CfgFile);
                log4net.Config.XmlConfigurator.Configure(log4CfgFile);
            }
            catch (Exception ex)
            {
                GlobalLogger.Error(ex.Message);
            }
        }

        protected virtual void Stop()
        {

        }

        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                // 在出现未处理的错误时运行的代码
                Exception ex = Server.GetLastError();
                if (ex is HttpUnhandledException)
                {
                    // 记录异常信息
                    logger.Error(string.Format("访问页面{0}出错！", Request.Path), ex);
#if !DEBUG
                    Response.Redirect("~/Error.aspx", true);
#endif
#if DEBUG
                    Response.Write(ex.Message);
                    Response.End();
#endif
                }
            }
            catch (Exception sysErr)
            {
                logger.Error(sysErr);
            }
        }
    }
}
