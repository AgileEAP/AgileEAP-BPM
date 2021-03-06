﻿using AgileEAP.Core.Configuration;

namespace AgileEAP.Core.Localization
{
    public class LocalizationSettings : ISettings
    {
        /// <summary>
        /// Default admin area language identifer
        /// </summary>
        public string DefaultAdminLanguageId { get; set; }

        /// <summary>
        /// Use images for language selection
        /// </summary>
        public bool UseImagesForLanguageSelection { get; set; }

        /// <summary>
        /// A value indicating whether SEO friendly URLs with multiple languages are enabled
        /// </summary>
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        /// <summary>
        /// A value indicating whether to load all records on application startup
        /// </summary>
        public bool LoadAllLocaleRecordsOnStartup { get; set; }
    }
}