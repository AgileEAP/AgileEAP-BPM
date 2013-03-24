using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace AgileEAP.Core.Data
{
    public interface IDatabaseCfg
    {
        string Name { get; set; }

        string NHConfigFile { get; set; }

        /// <summary>
        /// designates the implementation of IInterceptorFactory with which Burrow will create managed Session 
        /// </summary>
        string InterceptorFactory { get; set; }
    }
}
