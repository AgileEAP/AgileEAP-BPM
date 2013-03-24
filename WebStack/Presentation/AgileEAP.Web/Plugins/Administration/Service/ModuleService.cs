#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Infrastructure
 * Module:  Module
 * Descrption:
 * CreateDate: 2010/11/23 10:05:34
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:
 * Remark:
 * Update Time:
 * Author:generated by codesmithTemplate
 * 
 * ===============================================================================*/
#endregion
using System;
using System.Data;
using System.Collections.Generic;


using AgileEAP.Core;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Service;


using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Infrastructure.Service
{
    public class ModuleService :  BaseService<string,Module>
    {
		#region Fields
		private readonly ILogger log = LogManager.GetLogger(typeof(ModuleService));
		#endregion
		
		#region Constructors
		
		public ModuleService(){ }
		#endregion

        #region IModuleService Imp

        #endregion
		
		#region Internal Methods

        #endregion
    }
}