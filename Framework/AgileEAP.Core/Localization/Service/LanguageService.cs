using System;
using System.Collections.Generic;
using System.Linq;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Data;

namespace AgileEAP.Core.Localization
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
        #region Constants
        private const string LANGUAGES_ALL_KEY = "AgileEAP.language.all-{0}";
        private const string LANGUAGES_BY_ID_KEY = "AgileEAP.language.id-{0}";
        private const string LANGUAGES_PATTERN_KEY = "AgileEAP.language.";
        #endregion

        #region Fields

        private readonly IRepository<string> repository;

        #endregion

        #region Ctor

        public LanguageService(IRepository<string> repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void DeleteLanguage(Language language)
        {
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Language collection</returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false)
        {
            throw new Exception("none impl");
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageById(int languageId)
        {
            throw new Exception("none impl");
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void InsertLanguage(Language language)
        {
            throw new Exception("none impl");
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void UpdateLanguage(Language language)
        {

        }

        #endregion
    }
}
