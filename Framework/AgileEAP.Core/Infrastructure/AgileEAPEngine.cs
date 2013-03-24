﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using AgileEAP.Core.Configuration;
using AgileEAP.Core.Data;
using AgileEAP.Core.Infrastructure.DependencyManagement;

namespace AgileEAP.Core.Infrastructure
{
    public class AgileEAPEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Ctor

        /// <summary>
		/// Creates an instance of the content engine using default settings and configuration.
		/// </summary>
		public AgileEAPEngine() 
            : this(EventBroker.Instance, new ContainerConfigurer())
		{
		}

		public AgileEAPEngine(EventBroker broker, ContainerConfigurer configurer)
		{
            var config = ConfigurationManager.GetSection("AgileEAPConfigure") as AgileEAPConfigure;
            InitializeContainer(configurer, broker, config);
		}
        
        #endregion

        #region Utilities

        private void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }
        
        private void InitializeContainer(ContainerConfigurer configurer, EventBroker broker, AgileEAPConfigure config)
        {
            var builder = new ContainerBuilder();

            _containerManager = new ContainerManager(builder.Build());
            configurer.Configure(this, _containerManager, broker, config);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Initialize components and plugins in the AgileEAP environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(AgileEAPConfigure config)
        {
            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            if (databaseInstalled)
            {
                //startup tasks
                RunStartupTasks();
            }
        }

        public T Resolve<T>(string key = "") where T : class
		{
            return ContainerManager.Resolve<T>(key);
		}

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public Array ResolveAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

		#endregion

        #region Properties

        public IContainer Container
        {
            get { return _containerManager.Container; }
        }

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}