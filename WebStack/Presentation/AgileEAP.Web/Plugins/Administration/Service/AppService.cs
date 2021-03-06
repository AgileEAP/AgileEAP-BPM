#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Infrastructure
 * Module:  App
 * Descrption:
 * CreateDate: 2010/11/23 10:05:33
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
    public class AppService :  BaseService<string,App>
    {
		#region Fields
		private readonly ILogger log = LogManager.GetLogger(typeof(AppService));
		#endregion
		
		#region Constructors
		
		public AppService(){ }
		#endregion

        #region IAppService Imp

        #endregion
		
		#region Internal Methods

        #endregion
    }
}