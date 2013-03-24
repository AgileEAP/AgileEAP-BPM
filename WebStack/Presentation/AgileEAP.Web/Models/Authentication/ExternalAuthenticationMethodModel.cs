using System.Web.Routing;
using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Authentication
{
    public class ExternalAuthenticationMethodModel : AgileEAPModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}