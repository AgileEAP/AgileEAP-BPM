using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Workflow.Domain;
using AgileEAP.Core.Infrastructure;

namespace AgileEAP.Workflow.Engine
{
    public class DefaultExecutor
    {
        public static bool Execute(string uuid, WorkItem wi)
        {
            IAutoActivityHandler handler = AgileEAP.Core.Infrastructure.EngineContext.Current.Resolve<IAutoActivityHandler>(uuid);
            if (handler != null)
            {
                try
                {
                    return handler.Execute(wi);
                }
                catch (Exception ex)
                {
                    GlobalLogger.Error<DefaultExecutor>(string.Format("执行自动活动{0}自动处理{1}出错", uuid, handler.GetType().FullName), ex);
                    throw;
                }
            }

            throw new Exception(string.Format("活动{0}没有注册{1}处理IAutoActivityHandler", wi.ActivityInstName, uuid));
        }
    }
}
