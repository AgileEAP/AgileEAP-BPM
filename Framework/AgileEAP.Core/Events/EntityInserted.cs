
namespace AgileEAP.Core.Events
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInserted<T> where T : Domain.DomainObject<string>
    {
        private readonly T _entity;

        public EntityInserted(T entity)
        {
            _entity = entity;
        }

        public T Entity { get { return _entity; } }
    }
}
