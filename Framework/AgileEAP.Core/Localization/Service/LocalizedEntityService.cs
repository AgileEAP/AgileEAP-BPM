using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AgileEAP.Core;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;

namespace AgileEAP.Core.Localization
{
    /// <summary>
    /// Provides information about localizable entities
    /// </summary>
    public partial class LocalizedEntityService : ILocalizedEntityService
    {
        #region Constants

        private const string LOCALIZEDPROPERTY_KEY = "AgileEAP.localizedproperty.{0}-{1}-{2}-{3}";
        private const string LOCALIZEDPROPERTY_PATTERN_KEY = "AgileEAP.localizedproperty.";

        #endregion

        #region Fields

        private readonly IRepository<string> repository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="repository">Localized property repository</param>
        public LocalizedEntityService(IRepository<string> repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        public virtual void DeleteLocalizedProperty(LocalizedProperty localizedProperty)
        {
            //if (localizedProperty == null)
            //    throw new ArgumentNullException("localizedProperty");

            //_localizedPropertyRepository.Delete(localizedProperty);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);

        }

        /// <summary>
        /// Gets a localized property
        /// </summary>
        /// <param name="localizedPropertyId">Localized property identifier</param>
        /// <returns>Localized property</returns>
        public virtual LocalizedProperty GetLocalizedPropertyById(string localizedPropertyId)
        {
            //if (localizedPropertyId == 0)
            //    return null;

            //var localizedProperty = _localizedPropertyRepository.GetById(localizedPropertyId);
            //return localizedProperty;
            throw new Exception("none impl");
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <param name="localeKey">Locale key</param>
        /// <returns>Found localized value</returns>
        public virtual string GetLocalizedValue(string languageId, string entityId, string localeKeyGroup, string localeKey)
        {
            //string key = string.Format(LOCALIZEDPROPERTY_KEY, languageId, entityId, localeKeyGroup, localeKey);
            //return _cacheManager.Get(key, () =>
            //{
            //    var query = from lp in _localizedPropertyRepository.Table
            //                where lp.LanguageId == languageId &&
            //                lp.EntityId == entityId &&
            //                lp.LocaleKeyGroup == localeKeyGroup &&
            //                lp.LocaleKey == localeKey
            //                select lp.LocaleValue;
            //    var localeValue = query.FirstOrDefault();
            //    //little hack here. nulls aren't cacheable so set it to ""
            //    if (localeValue == null)
            //        localeValue = "";
            //    return localeValue;
            //});
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets localized properties
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <returns>Localized properties</returns>
        public virtual IList<LocalizedProperty> GetLocalizedProperties(string entityId, string localeKeyGroup)
        {
            //if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
            //    return new List<LocalizedProperty>();

            //var query = from lp in _localizedPropertyRepository.Table
            //            orderby lp.Id
            //            where lp.EntityId == entityId &&
            //                  lp.LocaleKeyGroup == localeKeyGroup
            //            select lp;
            //var props = query.ToList();
            //return props;
            throw new Exception("none impl");
        }

        /// <summary>
        /// Inserts a localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        public virtual void InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            //if (localizedProperty == null)
            //    throw new ArgumentNullException("localizedProperty");

            //_localizedPropertyRepository.Insert(localizedProperty);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
            throw new Exception("none impl");
        }

        /// <summary>
        /// Updates the localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        public virtual void UpdateLocalizedProperty(LocalizedProperty localizedProperty)
        {
            //if (localizedProperty == null)
            //    throw new ArgumentNullException("localizedProperty");

            //_localizedPropertyRepository.Update(localizedProperty);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
            throw new Exception("none impl");
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Ley selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : Domain.DomainObject<string>, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            int languageId) where T : Domain.DomainObject<string>, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (languageId == 0)
                throw new ArgumentOutOfRangeException("languageId", "Language ID should not be 0");

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.ID, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            string localeValueStr = CommonHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    DeleteLocalizedProperty(prop);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    UpdateLocalizedProperty(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //insert
                    prop = new LocalizedProperty()
                    {
                        ID = entity.ID,
                        LanguageId = languageId,
                        LocaleKey = localeKey,
                        LocaleKeyGroup = localeKeyGroup,
                        LocaleValue = localeValueStr
                    };
                    InsertLocalizedProperty(prop);
                }
            }
        }

        #endregion
    }
}