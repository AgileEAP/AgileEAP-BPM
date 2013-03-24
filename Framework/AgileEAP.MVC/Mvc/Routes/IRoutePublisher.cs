using System.Web.Routing;

namespace AgileEAP.MVC.Routes
{
    public interface IRoutePublisher
    {
        void RegisterRoutes(RouteCollection routeCollection);
    }
}
