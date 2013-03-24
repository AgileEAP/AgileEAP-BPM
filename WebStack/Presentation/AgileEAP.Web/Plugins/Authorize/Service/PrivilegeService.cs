#region Description
/*================================================================================
 *  Copyright (c) agile.  All rights reserved.
 * ===============================================================================
 * Solution: AuthorizeCenter
 * Module:  Privilege
 * Descrption:
 * CreateDate: 2010/11/18 13:55:34
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
    public class PrivilegeService :  BaseService<string,Privilege>
    {
		#region Fields
		private readonly ILogger log = LogManager.GetLogger(typeof(PrivilegeService));
		#endregion
		
		#region Constructors
		
		public PrivilegeService(){ }
		#endregion

        #region IPrivilegeService Imp

        public string GetPidByResId(string resourceID)
        {

            DataTable dt = repository.ExecuteDataTable<Privilege>(string.Format("select id from AC_Privilege where (type={0} or type={1}) and ResourceID='{2}'", (int)ResourceType.Menu, (int)ResourceType.Page, resourceID));

           if (dt.Rows.Count > 0)
           {
               return dt.Rows[0][0].ToString();
           }
           return "";

        }
        #endregion
		
		#region Internal Methods

        #endregion
    }
}