﻿#region Description
/*================================================================================
 *  Copyright (c) agile.  All rights reserved.
 * ===============================================================================
 * Solution: AuthorizeCenter
 * Module:  Operator
 * Descrption:
 * CreateDate: 2010/11/18 13:55:37
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;



using AgileEAP.Core;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;

using AgileEAP.WebControls;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Plugin.Authorize
{
    public partial class OperatorManager : BasePage
    {

        #region ---界面处理方法---

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
            }
        }

        #endregion

        #region ---操作处理方法---
		/// <summary>
        /// 转向明细页面
        /// </summary>
        /// <param name="param"></param>
        protected  void Redirect(string param)
        {
            var currentIdParam = PageContext.Action == ActionType.Add ? string.Empty : string.Format("&CurrentId={0}", CurrentId);
            Response.Redirect(string.Format("OperatorDetail.aspx?LastUrl={0}&Runat=1&ActionFlag={1}{2}{3}", Request.Url.PathAndQuery, PageContext.Action, currentIdParam, string.IsNullOrEmpty(param) ? param : "&" + param));
        }
		
       
	

        public string GetOrgNameByOrgId(string orgId)
        {
            Organization org =repository.GetDomain<Organization>(orgId);
            return org==null?"":org.Name;
        }
        #endregion
    }
}
