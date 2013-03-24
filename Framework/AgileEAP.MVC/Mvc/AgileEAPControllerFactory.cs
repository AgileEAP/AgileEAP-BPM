using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using AgileEAP.Core;

namespace AgileEAP.MVC
{
    public class AgileEAPControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            try
            {
                GlobalLogger.Debug<AgileEAPControllerFactory>(controllerType.ToString());
                IController controller = base.GetControllerInstance(requestContext, controllerType);
                GlobalLogger.Debug<AgileEAPControllerFactory>(controller.ToString());

                return controller;
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<AgileEAPControllerFactory>(ex);

                throw;
            }
        }
    }
}
