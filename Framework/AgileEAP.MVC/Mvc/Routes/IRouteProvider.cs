﻿using System.Web.Routing;

namespace AgileEAP.MVC.Routes
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}
