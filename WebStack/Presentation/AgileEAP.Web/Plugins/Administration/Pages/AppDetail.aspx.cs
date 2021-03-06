﻿#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Infrastructure
 * Module:  App
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
using AgileEAP.Core.Utility;
using AgileEAP.Core.FastInvoker;
using AgileEAP.WebControls;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Administration
{
    public partial class AppDetail : BasePage
    {
        private AppService appService = new AppService();

        #region ---界面处理方法---

        protected bool ShowPageDetail()
        {
            if (PageContext.Action == ActionType.Add)
            {
                txtCreator.Text = User.LoginName;
                dtpUseTime.Text = DateTime.Now.ToShortDateString();
                dtpCreateTime.Text = DateTime.Now.ToShortDateString();
                return false;
            }

            App entity = appService.GetDomain(CurrentId);

            if (entity == null) return false;

            SetControlValues(entity);

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowPageDetail();
            }
        }
        #endregion

        #region ---操作处理方法---
        protected App GetDomainObject()
        {
            App app = appService.GetDomain(CurrentId);

            if (app == null)
            {
                app = new App();
                app.ID = CurrentId;
            }

            GetControlValues(ref app);

            return app;
        }

        public void Save(string argument)
        {
            try
            {
                App app = GetDomainObject();

                appService.SaveOrUpdate(app);

                WebUtil.PromptMsg("保存操作成功");
                WebUtil.CloseRefreshParent();
            }
            catch (Exception ex)
            {
                log.Error("保存操作失败", ex);

                WebUtil.PromptMsg("保存操作失败");
            }
        }

        #endregion
    }
}
