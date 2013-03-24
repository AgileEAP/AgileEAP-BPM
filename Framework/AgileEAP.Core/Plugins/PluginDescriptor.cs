using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using AgileEAP.Core.Infrastructure;

namespace AgileEAP.Core.Plugins
{
    public class PluginDescriptor : IComparable<PluginDescriptor>
    {
        public PluginDescriptor()
        {
            this.SupportedVersions = new List<string>();
        }


        public PluginDescriptor(Assembly referencedAssembly, FileInfo originalAssemblyFile,
            Type pluginType)
            : this()
        {
            this.ReferencedAssembly = referencedAssembly;
            this.OriginalAssemblyFile = originalAssemblyFile;
            this.PluginType = pluginType;
        }
        /// <summary>
        /// Plugin type
        /// </summary>
        public virtual string PluginFileName { get; set; }

        private bool autoInstall = true;
        /// <summary>
        /// Plugin type
        /// </summary>
        public virtual bool AutoInstall
        {
            get
            {
                return autoInstall;
            }
            set
            {
                autoInstall = value;
            }
        }

        /// <summary>
        /// Plugin type
        /// </summary>
        public virtual Type PluginType { get; set; }

        /// <summary>
        /// The assembly that has been shadow copied that is active in the application
        /// </summary>
        public virtual Assembly ReferencedAssembly { get; internal set; }

        /// <summary>
        /// The original assembly file that a shadow copy was made from it
        /// </summary>
        public virtual FileInfo OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// Gets or sets the plugin group
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        public virtual string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the system name
        /// </summary>
        public virtual string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Gets or sets the supported versions of AgileEAPCommerce
        /// </summary>
        public virtual IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether plugin is installed
        /// </summary>
        public virtual bool Installed { get; set; }

        /// <summary>
        /// plugin login url
        /// </summary>
        public virtual string LoginUrl { get; set; }

        public virtual string ConfigurationUrl { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual bool CanChangeEnabled { get; set; }

        public virtual T Instance<T>() where T : class, IPluginInstaller
        {
            T instance;
            //if (!EngineContext.Current.ContainerManager.Scope().TryResolve(PluginType, out instance))
            //{
            //not resolved
            try
            {
                instance = Activator.CreateInstance(PluginType) as T;
            }
            catch (MissingMethodException)
            {
                instance = EngineContext.Current.ContainerManager.ResolveUnregistered(PluginType) as T;
            }
            //}
            if (instance != null)
                instance.PluginDescriptor = this;
            return instance;
        }

        public IPluginInstaller Instance()
        {
            return Instance<IPluginInstaller>();
        }

        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);
            else
                return FriendlyName.CompareTo(other.FriendlyName);
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PluginDescriptor;
            return other != null &&
                SystemName != null &&
                SystemName.Equals(other.SystemName);
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }
    }
}
