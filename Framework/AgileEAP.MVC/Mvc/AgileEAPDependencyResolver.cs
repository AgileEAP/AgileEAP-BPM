using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AgileEAP.Core.Infrastructure;

namespace AgileEAP.MVC
{
    public class AgileEAPDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == null || serviceType.Name.StartsWith("_")) return null;

            try
            {
                //return EngineContext.Current.IsRegister(serviceType) ? EngineContext.Current.Resolve(serviceType) : null;
                return EngineContext.Current.Resolve(serviceType);
            }
            catch
            {
                AgileEAP.Core.GlobalLogger.Error<AgileEAPDependencyResolver>(string.Format("注冊Type={0}出錯", serviceType.Name));
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
                return (IEnumerable<object>)EngineContext.Current.Resolve(type);
            }
            catch
            {
                return null;
            }
        }
    }
}
