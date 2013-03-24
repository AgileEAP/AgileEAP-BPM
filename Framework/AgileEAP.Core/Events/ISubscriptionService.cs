using System.Collections.Generic;

namespace AgileEAP.Core.Events
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
