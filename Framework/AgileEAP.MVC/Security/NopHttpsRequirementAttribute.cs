using System;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.ExceptionHandler;

namespace AgileEAP.MVC.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AgileEAPHttpsRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public AgileEAPHttpsRequirementAttribute(SslRequirement sslRequirement)
        {
            this.SslRequirement = sslRequirement;
        }
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            
            
            // only redirect for GET requests, 
            // otherwise the browser might not propagate the verb and request body correctly.
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var currentConnectionSecured = filterContext.HttpContext.Request.IsSecureConnection;
            //currentConnectionSecured = webHelper.IsCurrentConnectionSecured();

            switch (this.SslRequirement)
            {
                case SslRequirement.Yes:
                    {
                        if (!currentConnectionSecured)
                        {
                            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                            if (webHelper.SslEnabled())
                            {
                                //redirect to HTTPS version of page
                                //string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
                                string url = webHelper.GetThisPageUrl(true, true);
                                filterContext.Result = new RedirectResult(url);
                            }
                        }
                    }
                    break;
                case SslRequirement.No:
                    {
                        if (currentConnectionSecured)
                        {
                            var webHelper = EngineContext.Current.Resolve<IWebHelper>();

                            //redirect to HTTP version of page
                            //string url = "http://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
                            string url = webHelper.GetThisPageUrl(true, false);
                            filterContext.Result = new RedirectResult(url);
                        }
                    }
                    break;
                default:
                    throw new EAPException("Not supported SslProtected parameter");
            }
        }

        public SslRequirement SslRequirement { get; set; }
    }
}
