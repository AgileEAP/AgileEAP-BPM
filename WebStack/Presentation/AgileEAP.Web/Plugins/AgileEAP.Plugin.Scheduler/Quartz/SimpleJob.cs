using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Logging;
using Quartz;

namespace AgileEAP.Plugin.Scheduler.Quartz
{
    public class SimpleJob : IJob
    {
        public const string Message = "msg";
        private static readonly ILog log = LogManager.GetLogger(typeof(SimpleJob));


        /// <summary> 
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            // This job simply prints out its job name and the
            // date and time that it is running
            JobKey jobKey = context.JobDetail.Key;

            string message = context.JobDetail.JobDataMap.GetString(Message);

            log.InfoFormat("SimpleJob: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            log.InfoFormat("SimpleJob: msg: {0}", message);

            System.Diagnostics.Debug.WriteLine("SimpleJob: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            System.Diagnostics.Debug.WriteLine("SimpleJob: msg: {0}", message);
        }
    }
}
