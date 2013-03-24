using AgileEAP.Core.Domain;

namespace AgileEAP.Core.Localization
{
    /// <summary>
    /// Represents a locale string resource
    /// </summary>
    public partial class LocaleStringResource : DomainObject<string>
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public virtual string LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public virtual string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public virtual string ResourceValue { get; set; }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual Language Language { get; set; }
    }

}
