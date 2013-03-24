using System;
using System.Collections.Generic;
using System.Linq;
using AgileEAP.Core;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;

namespace AgileEAP.Core.Localization
{
    /// <summary>
    /// Provides information about localization
    /// </summary>
    public partial class LocalizationService : ILocalizationService
    {
        #region Constants
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "AgileEAP.lsr.all-{0}";
        private const string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "AgileEAP.lsr.{0}-{1}";
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "AgileEAP.lsr.";
        #endregion

        #region Fields

        private readonly IRepository<string> repository;
        private readonly IWorkContext workContext;
        #endregion

        #region Ctor
        public LocalizationService(IWorkContext workContext, IRepository<string> repository)
        {
            this.workContext = workContext;
            this.repository = repository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            //if (localeStringResource == null)
            //    throw new ArgumentNullException("localeStringResource");

            //_lsrRepository.Delete(localeStringResource);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            ////event notification
            //_eventPublisher.EntityDeleted(localeStringResource);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceById(string localeStringResourceId)
        {
            throw new Exception("none impl");
            //if (localeStringResourceId == 0)
            //    return null;

            //var localeStringResource = _lsrRepository.GetById(localeStringResourceId);

            //return localeStringResource;
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            //if (_workContext.WorkingLanguage != null)
            //    return GetLocaleStringResourceByName(resourceName, _workContext.WorkingLanguage.Id);

            //return null;
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName, string languageId,
            bool logIfNotFound = true)
        {
            //LocaleStringResource localeStringResource = null;

            //if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            //{
            //    //load all records

            //    // using an empty string so the request can still be logged
            //    if (string.IsNullOrEmpty(resourceName))
            //        resourceName = string.Empty;
            //    resourceName = resourceName.Trim().ToLowerInvariant();

            //    var resources = GetAllResourcesByLanguageId(languageId);
            //    if (resources.ContainsKey(resourceName))
            //    {
            //        var localeStringResourceId = resources[resourceName].Id;
            //        localeStringResource = _lsrRepository.GetById(localeStringResourceId);
            //    }
            //}
            //else
            //{
            //    //gradual loading
            //    var query = from lsr in _lsrRepository.Table
            //                orderby lsr.ResourceName
            //                where lsr.LanguageId == languageId && lsr.ResourceName == resourceName
            //                select lsr;
            //    localeStringResource = query.FirstOrDefault();
            //}
            //if (localeStringResource == null && logIfNotFound)
            //    _logger.Warning(string.Format("Resource string ({0}) not found. Language ID = {1}", resourceName, languageId));
            //return localeStringResource;
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public virtual Dictionary<string, LocaleStringResource> GetAllResourcesByLanguageId(string languageId)
        {
            //string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, languageId);
            //return _cacheManager.Get(key, () =>
            //                                  {
            //                                      var query = from l in _lsrRepository.Table
            //                                                  orderby l.ResourceName
            //                                                  where l.LanguageId == languageId
            //                                                  select l;
            //                                      var localeStringResourceDictionary =
            //                                          query.ToDictionary(s => s.ResourceName.ToLowerInvariant());
            //                                      return localeStringResourceDictionary;
            //                                  });
            throw new Exception("none impl");
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            //if (localeStringResource == null)
            //    throw new ArgumentNullException("localeStringResource");

            //_lsrRepository.Insert(localeStringResource);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            ////event notification
            //_eventPublisher.EntityInserted(localeStringResource);
            throw new Exception("none impl");
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            //if (localeStringResource == null)
            //    throw new ArgumentNullException("localeStringResource");

            //_lsrRepository.Update(localeStringResource);

            ////cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            ////event notification
            //_eventPublisher.EntityUpdated(localeStringResource);
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (workContext.Language != null)
                return GetResource(resourceKey, workContext.Language.ID);

            return string.Empty;
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="returnEmptyIfNotFound">A value indicating whether to empty string will be returned if a resource is not found and default value is set to empty string</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey, string languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            //string result = string.Empty;
            //var resourceKeyValue = resourceKey;
            //if (resourceKeyValue == null)
            //    resourceKeyValue = string.Empty;
            //resourceKeyValue = resourceKeyValue.Trim().ToLowerInvariant();
            //if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            //{
            //    //load all records
            //    var resources = GetAllResourcesByLanguageId(languageId);

            //    if (resources.ContainsKey(resourceKeyValue))
            //    {
            //        var lsr = resources[resourceKeyValue];
            //        if (lsr != null)
            //            result = lsr.ResourceValue;
            //    }
            //}
            //else
            //{
            //    //gradual loading
            //    string key = string.Format(LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKeyValue);
            //    string lsr = _cacheManager.Get(key, () =>
            //    {
            //        var query = from l in _lsrRepository.Table
            //                    where l.ResourceName == resourceKeyValue
            //                    && l.LanguageId == languageId
            //                    select l.ResourceValue;
            //        return query.FirstOrDefault();
            //    });

            //    if (lsr != null)
            //        result = lsr;
            //}
            //if (String.IsNullOrEmpty(result))
            //{
            //    if (logIfNotFound)
            //        _logger.Warning(string.Format("Resource string ({0}) is not found. Language ID = {1}", resourceKey, languageId));

            //    if (!String.IsNullOrEmpty(defaultValue))
            //    {
            //        result = defaultValue;
            //    }
            //    else
            //    {
            //        if (!returnEmptyIfNotFound)
            //            result = resourceKey;
            //    }
            //}
            //return result;

            return string.Empty;
            //throw new Exception("none impl");
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        #endregion
    }
}
