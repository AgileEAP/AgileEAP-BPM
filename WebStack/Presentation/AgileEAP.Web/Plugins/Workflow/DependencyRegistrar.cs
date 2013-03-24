using System;
using System.Reflection;
using System.Linq;

using Autofac;
using Autofac.Integration.Mvc;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Infrastructure.DependencyManagement;
using AgileEAP.Workflow.Engine;
using AgileEAP.MVC.WebApi;

namespace AgileEAP.Plugin.Workflow
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(Autofac.ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<WorkflowEngine>().As<IWorkflowEngine>().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeFinder.GetAssemblies().ToArray())
           .AssignableTo<IAutoActivityHandler>()
           .AsImplementedInterfaces();


            Assembly[] assemblies = typeFinder.GetAssemblies().ToArray();
            builder.RegisterAssemblyTypes(assemblies)
           .AssignableTo<IAutoActivityHandler>()
           .AsImplementedInterfaces();

            //regisger  [UUID("ApplyQuota-2.0-Approve-ActivityExecutedEvent")]
            var handlers = typeFinder.FindClassesOfType(typeof(IAutoActivityHandler)).ToList();
            foreach (var handler in handlers)
            {
                object[] attrs = handler.GetCustomAttributes(typeof(UUIDAttribute), false);
                string uuid = string.Empty;
                if (attrs != null)
                {
                    foreach (UUIDAttribute attr in attrs)
                    {
                        uuid = attr.UUID;
                    }
                    builder.RegisterType(handler).As<IAutoActivityHandler>().Keyed<IAutoActivityHandler>(uuid)
                    .InstancePerHttpRequest();
                }
            }

            //regisger  [UUID("ApplyQuota-2.0-Approve-ActivityExecutedEvent")]
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                object[] attrs = consumer.GetCustomAttributes(typeof(UUIDAttribute), false);
                string uuid = string.Empty;
                if (attrs != null)
                {
                    foreach (UUIDAttribute attr in attrs)
                    {
                        uuid = attr.UUID;
                    }
                    var interfaceType = consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>))[0];
                    builder.RegisterType(consumer).As(interfaceType)
                    .Keyed(uuid, interfaceType)
                    .InstancePerHttpRequest();
                }
            }
        }

        public int Order
        {
            get { return 9; }
        }
    }
}