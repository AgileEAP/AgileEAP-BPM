using System;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Infrastructure.DependencyManagement;

namespace AgileEAP.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the 
    /// various services composing the AgileEAP engine. Edit functionality, modules
    /// and implementations access most AgileEAP functionality through this 
    /// interface.
    /// </summary>
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        
        /// <summary>
        /// Initialize components and plugins in the AgileEAP environment.
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(AgileEAPConfigure config);

        T Resolve<T>(string key = "") where T : class;

        object Resolve(Type type);

        Array ResolveAll(Type serviceType);

        T[] ResolveAll<T>();
    }
}
