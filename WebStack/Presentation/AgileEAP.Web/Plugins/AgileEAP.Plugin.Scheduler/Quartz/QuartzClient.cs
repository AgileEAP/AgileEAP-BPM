using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

//using Common.Logging;
using Quartz;
using Quartz.Impl;

namespace AgileEAP.Plugin.Scheduler.Quartz
{
    public class QuartzClient
    {
        public virtual void Install()
        {
            //  ILog log = LogManager.GetLogger(typeof(QuartzClient));

            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteClient";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting expoter
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:555/QuartzScheduler";

            //// job initialization plugin handles our xml reading, without it defaults are used
            //properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            //properties["quartz.plugin.xml.fileNames"] = "~/quartz_jobs.xml";

            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            // define the job and ask it to run

            IJobDetail job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("remotelyAddedJob", "default")
                .Build();

            JobDataMap map = job.JobDataMap;
            map.Put("msg", "Your remotely added job has executed!");

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("remotelyAddedTrigger", "default")
                .ForJob(job.Key)
                .WithCronSchedule("/5 * * ? * *")
                .Build();

            // schedule the job
            sched.ScheduleJob(job, trigger);

            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 15);
            job = JobBuilder.Create<SimpleJob>()
    .WithIdentity("job2", "group1")
    .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                           .WithIdentity("trigger2", "group1")
                                           .StartAt(startTime)
                                           .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).WithRepeatCount(20))
                                           .Build();

            sched.ScheduleJob(job, trigger);

            // log.Info("Remote job scheduled.");
        }

        public virtual void UnInstall()
        {
            //  ILog log = LogManager.GetLogger(typeof(QuartzClient));

            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteClient";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting expoter
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:555/QuartzScheduler";

            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            sched.Shutdown(true);
        }
    }
}
