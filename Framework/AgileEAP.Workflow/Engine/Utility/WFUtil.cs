using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Utility;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 工作流上下文
    /// </summary>
    public static class WFUtil
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public static IUser User
        {
            get
            {
                IUser cacheUser = ApplicationContext.Current.Sessions.GetSafeValue<IUser>("cache_workflow_user");
                if (cacheUser == null)
                {
                    IAuthenticationService authenticationService = AgileEAP.Core.Infrastructure.EngineContext.Current.Resolve<IAuthenticationService>();
                    cacheUser = authenticationService.GetAuthenticatedUser();
                    if (cacheUser == null)
                    {
                        cacheUser = new User()
                        {
                            LoginName = "WorkflowEngine",
                            ID = "WorkflowEngine",
                            Name = "工作流引擎",
                            UserType = 2
                        };
                    }

                    ApplicationContext.Current.Sessions.SafeAdd("cache_workflow_user", cacheUser);
                }

                return cacheUser;
            }
        }

        /// <summary>
        /// 默认最小时间
        /// </summary>
        public readonly static DateTime minDate = new DateTime(2000, 1, 1);

        /// <summary>
        /// 默认最大时间
        /// </summary>
        public readonly static DateTime MaxDate = new DateTime(2099, 1, 1);

        /// <summary>
        /// 默认变量前缀
        /// </summary>
        public readonly static char ExpressionVariablePrefix = ':';

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="exception"></param>
        public static void HandleException(WFException exception)
        {
            Trace.Print(exception);
            throw exception;
        }
    }
}
