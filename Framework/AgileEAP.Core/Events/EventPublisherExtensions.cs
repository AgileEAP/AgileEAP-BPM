namespace AgileEAP.Core.Events
{
    public static class EventPublisherExtensions
    {
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : Domain.DomainObject<string>
        {
            eventPublisher.Publish(new EntityInserted<T>(entity));
        }

        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : Domain.DomainObject<string>
        {
            eventPublisher.Publish(new EntityUpdated<T>(entity));
        }

        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : Domain.DomainObject<string>
        {
            eventPublisher.Publish(new EntityDeleted<T>(entity));
        }
    }
}