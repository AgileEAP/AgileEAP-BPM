﻿using System;
using System.Collections.Generic;
using System.Linq;
using AgileEAP.Core.Configuration;

namespace AgileEAP.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Configures the inversion of control container with services used by AgileEAP.
    /// </summary>
    public class ContainerConfigurer
    {
        /// <summary>
        /// Known configuration keys used to configure services.
        /// </summary>
        public static class ConfigurationKeys
        {
            /// <summary>Key used to configure services intended for medium trust.</summary>
            public const string MediumTrust = "MediumTrust";
            /// <summary>Key used to configure services intended for full trust.</summary>
            public const string FullTrust = "FullTrust";
        }

        public virtual void Configure(IEngine engine, ContainerManager containerManager, EventBroker broker, AgileEAPConfigure configuration)
        {
            //other dependencies
            containerManager.AddComponentInstance<AgileEAPConfigure>(configuration, "AgileEAP.configuration");
            containerManager.AddComponentInstance<IEngine>(engine, "AgileEAP.engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "AgileEAP.containerConfigurer");

            //type finder
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("AgileEAP.typeFinder");

            //register dependencies provided by other assemblies
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
                var drInstances = new List<IDependencyRegistrar>();
                foreach (var drType in drTypes)
                    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
                //sort
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                    dependencyRegistrar.Register(x, typeFinder);
            });

            //event broker
            containerManager.AddComponentInstance(broker);

            //service registration
            containerManager.AddComponent<DependencyAttributeRegistrator>("AgileEAP.serviceRegistrator");
            var registrator = containerManager.Resolve<DependencyAttributeRegistrator>();
            var services = registrator.FindServices();
            var configurations = GetComponentConfigurations(configuration);
            services = registrator.FilterServices(services, configurations);
            registrator.RegisterServices(services);
        }

        protected virtual string[] GetComponentConfigurations(AgileEAPConfigure configuration)
        {
            var configurations = new List<string>();
            string trustConfiguration = (CommonHelper.GetTrustLevel() > System.Web.AspNetHostingPermissionLevel.Medium)
                ? ConfigurationKeys.FullTrust
                : ConfigurationKeys.MediumTrust;
            configurations.Add(trustConfiguration);
            return configurations.ToArray();
        }
    }
}
