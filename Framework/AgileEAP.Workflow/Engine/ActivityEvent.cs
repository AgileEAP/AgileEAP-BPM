using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;

namespace AgileEAP.Workflow.Engine
{
    public class EventPublisher
    {
        public static void Publish<TEvent>(TEvent eventMessage) where TEvent : IEventUUID
        {
            string uuid = eventMessage.UUID;
            IConsumer<TEvent> consumer = AgileEAP.Core.Infrastructure.EngineContext.Current.Resolve<IConsumer<TEvent>>(uuid);
            if (consumer != null)
            {
                try
                {
                    consumer.HandleEvent(eventMessage);
                }
                catch (Exception ex)
                {
                    GlobalLogger.Error<EventPublisher>(string.Format("execute IConsumer<{0}> HandleEvent error message:{1} ", typeof(TEvent)), JsonConvert.SerializeObject(eventMessage), ex);

                    throw;
                }
            }
        }
    }

    public interface IConsumer<TEvent>
    {
        void HandleEvent(TEvent eventMessage);
    }

    public interface IEventUUID
    {
        string UUID { get; }
    }

    public class ActivityExecutingEvent : IEventUUID
    {
        public ActivityContext Context
        {
            get;
            set;
        }

        public string UUID { get; set; }
    }

    public class ActivityExecutedEvent : IEventUUID
    {
        public ActivityContext Context
        {
            get;
            set;
        }

        public string UUID { get; set; }
    }

    public class WorkItemExecutingEvent : IEventUUID
    {
        public WorkItemContext Context
        {
            get;
            set;
        }

        public string UUID { get; set; }
    }

    public class WorkItemExecutedEvent : IEventUUID
    {
        public WorkItemContext Context
        {
            get;
            set;
        }

        public string UUID { get; set; }
    }

    public class WorkItemErrorEvent : IEventUUID
    {
        public WorkItemContext Context
        {
            get;
            set;
        }

        public string UUID { get; set; }
    }
}

