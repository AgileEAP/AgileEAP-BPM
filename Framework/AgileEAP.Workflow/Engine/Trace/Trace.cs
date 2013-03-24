using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Domain;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 跟踪日志
    /// </summary>
    public class Trace
    {

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Print(string message)
        {
            Print(message, null);
        }

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Print(WFException exception)
        {
            Print(exception.ToSafeString(), null);
        }

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Print(string message, Exception ex)
        {
            ILogger log = LogManager.GetLogger("ProcessTrace");
            log.Info(message, ex);
        }

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="log">日志记录</param>
        public static void Print(TraceLog log)
        {
            IRepository<string> repository = AgileEAP.Core.Infrastructure.EngineContext.Current.Resolve<IRepository<string>>();
            repository.SaveOrUpdate(log);
        }
    }
}
