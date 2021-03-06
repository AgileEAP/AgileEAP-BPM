﻿#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Infrastructure
 * Module:  Dict
 * Descrption:
 * CreateDate: 2010/11/18 13:40:13
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
    public partial class DictDetail : BasePage
    {
        private DictService dictService = new DictService();
        DictItemService dictItemService = new DictItemService();

        #region ---界面处理方法---

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack && !IsAjaxPost)
                ShowPageDetail();
        }

        protected void ShowPageDetail()
        {
            Dict parent = null;
            if (PageContext.Action == ActionType.Add)
            {
                parent = dictService.GetDomain(Request.QueryString["ParentID"]);
                if (parent != null)
                {
                    chbParentID.Text = parent.Text;
                    chbParentID.Value = parent.ID;
                }
                return;
            }

            Dict entity = dictService.GetDomain(CurrentId);
            if (entity == null) return;

            SetControlValues(entity);

            parent = dictService.GetDomain(entity.ParentID);
            if (parent != null)
            {
                chbParentID.Text = parent.Text;
                chbParentID.Value = parent.ID;
            }

            //List<string> dictItemIDs = dictItemService.All().Where(p => p.DictID == entity.ID).Select(p => p.ID).ToList();
            this.rptDict.DataSource = dictItemService.All().Where(p => p.DictID == entity.ID).OrderBy(d => d.SortOrder);
            this.rptDict.DataBind();

        }
        #endregion

        #region ---操作处理方法---
        protected Dict GetDomainObject()
        {
            Dict dict = dictService.GetDomain(CurrentId);

            if (dict == null)
            {
                dict = new Dict();
                dict.ID = CurrentId;
            }

            GetControlValues(ref dict);

            return dict;
        }
        public string Delete(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                DictItem item = dictItemService.GetDomain(argument);
                if (item != null)
                {
                    repository.Delete<DictItem>(item);
                    repository.ClearCache<DictItem>();
                    doResult = DoResult.Success;

                    //获取提示信息
                    actionMessage = RemarkAttribute.GetEnumRemark(doResult);

                    ajaxResult.Result = doResult;
                    ajaxResult.RetValue = CurrentId;
                    ajaxResult.PromptMsg = actionMessage;
                    AddActionLog<Dict>(string.Format("删除字典项{0}={1}", item.Value, item.Text), doResult);
                }
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                Dict dict = JsonConvert.DeserializeObject<Dict>(argument);

                if (string.IsNullOrEmpty(dict.ID))
                    dict.ID = IdGenerator.NewComb().ToString();
                dict.Creator = User.ID;

                foreach (var item in dict.DictItems)
                {
                    item.ID = string.IsNullOrWhiteSpace(item.ID) ? IdGenerator.NewComb().ToString() : item.ID;
                    item.DictID = dict.ID;
                    item.Creator = User.ID;
                }

                dictService.SaveOrUpdate(dict);
                dictService.ClearCache();
                dictItemService.ClearCache();

                doResult = DoResult.Success;

                //获取提示信息
                actionMessage = string.Format("保存字典{0}成功", dict.Name);

                //记录操作日志
                AddActionLog(dict, doResult, actionMessage);

                Dictionary<string, object> retValue = new Dictionary<string, object>();
                List<string> itemIds = dict.DictItems.Select(d => d.ID).ToList();
                retValue.Add("ID", dict.ID);
                retValue.Add("DictIds", itemIds);

                ajaxResult.Result = doResult;
                ajaxResult.RetValue = retValue;
                ajaxResult.PromptMsg = actionMessage;

            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
        #endregion
    }
}
