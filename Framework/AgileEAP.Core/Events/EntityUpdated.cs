﻿
namespace AgileEAP.Core.Events
{
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdated<T> where T : Domain.DomainObject<string>
    {
        private readonly T _entity;

        public EntityUpdated(T entity)
        {
            _entity = entity;
        }

        public T Entity { get { return _entity; } }
    }
}
