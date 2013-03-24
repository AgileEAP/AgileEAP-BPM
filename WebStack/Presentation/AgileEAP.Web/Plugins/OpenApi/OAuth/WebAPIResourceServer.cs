using System.Net.Http;
using System.Security.Principal;
using System.Web;
using DotNetOpenAuth.OAuth2;

namespace AgileEAP.Plugin.OpenApi
{
    public class WebAPIResourceServer : ResourceServer
    {
        public WebAPIResourceServer(IAccessTokenAnalyzer accessTokenAnalyzer)
            : base(accessTokenAnalyzer)
        {
        }
        public new IPrincipal GetPrincipal(HttpRequestMessage request, params string[] requiredScopes)
        {
            return base.GetPrincipal(new HttpRequestWrapper(HttpContext.Current.Request), requiredScopes);
        }
    }
}