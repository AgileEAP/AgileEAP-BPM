using AgileEAP.Core.Localization;
using AgileEAP.Core.Authentication;

namespace AgileEAP.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        IUser User { get; set; }


        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language Language { get; set; }

        string Theme { get; }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; }

        /// <summary>
        ///  Get or set value indicating whether in debug mode
        /// </summary>
        bool IsDebug { get; }
    }
}
