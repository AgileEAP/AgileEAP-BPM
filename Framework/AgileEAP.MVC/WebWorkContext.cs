using System;
using System.Linq;
using System.Web;
using AgileEAP.Core;
using AgileEAP.Core.Localization;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC.Localization;
using AgileEAP.Core.Extensions;

namespace AgileEAP.MVC
{
    /// <summary>
    /// Working context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        private readonly HttpContextBase httpContext;
        private readonly IAuthenticationService authenticationService;

        public WebWorkContext(HttpContextBase httpContext, IAuthenticationService authenticationService)
        {
            this.httpContext = httpContext;
            Language = new Language()
            {
                LanguageCulture = "zh-CN"
            };

            IsDebug = System.Configuration.ConfigurationManager.AppSettings["RunMode"] == "Debug";
            this.authenticationService = authenticationService;
        }

        private IUser cacheUser;
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public IUser User
        {
            get
            {
                return cacheUser ?? (cacheUser = authenticationService.GetAuthenticatedUser());
            }
            set
            {
                cacheUser = value;
            }
        }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        public Language Language
        {
            get;
            set;
        }

        public string Theme
        {
            get
            {
                return User == null ? "Default" : User.Theme;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return User.UserType == (short)UserType.Administrator;
            }
        }

        public bool IsDebug { get; set; }
    }
}
