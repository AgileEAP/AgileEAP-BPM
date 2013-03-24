﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AgileEAP.Core.Data;
using AgileEAP.Core.ExceptionHandler;
namespace AgileEAP.Core.Data.Config
{
    public class DatabaseElement : ConfigurationElement, IDatabaseCfg
    {
        //Fixme: eliminate the duplicate code in this class and NHiberanteBurrowCfgSection
        private readonly IDictionary<string, object> savedSettings = new Dictionary<string, object>();

        #region IDatabaseCfg Members

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "DefaultDatabase", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters =
                         " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string Name
        {
            get { return (string)Get("name"); }
            set { Set("name", value); }
        }

        /// <summary>
        /// designate the NHibernate config file of this persistent unit.
        /// </summary>
        [ConfigurationProperty("nh-config-file", IsRequired = true, IsKey = false)]
        [StringValidator(InvalidCharacters =
                         "!@#$%^&*()[]{};'\"|", MaxLength = 160)]
        public string NHConfigFile
        {
            get { return (string)Get("nh-config-file"); }
            set { Set("nh-config-file", value); }
        }

        /// <summary>
        /// designates the implementation of IInterceptorFactory with which Burrow will create managed Session 
        /// </summary>
        [ConfigurationProperty("interceptorFactory", IsRequired = false, IsKey = false)]
        [StringValidator(InvalidCharacters =
                         "!@#$%^&*()[]{};'\"|", MaxLength = 160)]
        public string InterceptorFactory
        {
            get { return (string)Get("interceptorFactory"); }
            set { Set("interceptorFactory", value); }
        }

        #endregion

        private void Set(string key, object value)
        {
            new Util().CheckCanChangeCfg();
            savedSettings[key] = value;
        }

        private object Get(string key)
        {
            if (savedSettings.ContainsKey(key))
            {
                return savedSettings[key];
            }
            else
            {
                return this[key];
            }
        }

    }
}
