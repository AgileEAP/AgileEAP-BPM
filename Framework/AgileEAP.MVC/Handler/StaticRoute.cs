using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AgileEAP.MVC.Handler
{
    public class StaticRoute
    {
        static IHttpHandler staticFileHandler = null;

        static StaticRoute()
        {
            if (staticFileHandler == null)
            {
                Type type = Type.GetType("System.Web.StaticFileHandler");
                staticFileHandler = Activator.CreateInstance(type) as IHttpHandler;
            }
        }

        public static IHttpHandler StaticFileHandler
        {
            get
            {
                if (staticFileHandler == null)
                {
                    Type type = Type.GetType("System.Web.StaticFileHandler");
                    staticFileHandler = Activator.CreateInstance(type) as IHttpHandler;
                }

                return staticFileHandler;
            }
        }
    }
}
