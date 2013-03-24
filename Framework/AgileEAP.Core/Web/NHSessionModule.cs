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
using System.Web.SessionState;

using AgileEAP.Core.ExceptionHandler;
using NHibernate;


namespace AgileEAP.Core.Web
{
    public class NHSessionModule : IHttpModule, IRequiresSessionState
    {
        ILogger logger = LogManager.GetLogger(typeof(NHSessionModule));
        public void Init(HttpApplication context)
        {
            context.EndRequest += (o, e) =>
            {
                try
                {
                    foreach (KeyValuePair<string, object> item in ApplicationContext.Current.Sessions)
                    {
                        ISession session = item.Value as ISession;
                        if (session.Transaction != null && session.Transaction.IsActive)
                            session.Transaction.Rollback();

                        if (session.IsOpen)
                            session.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Session Close  Error!", ex);
                }
                finally
                {
                    ApplicationContext.Current.Sessions.Clear();
                }
            };
        }

        public void Dispose()
        {
        }
    }
}
