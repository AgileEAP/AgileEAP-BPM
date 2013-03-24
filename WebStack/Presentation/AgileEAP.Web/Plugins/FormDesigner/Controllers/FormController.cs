using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;
using AgileEAP.Core.Caching;
using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC.Security;

using System.Text;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Plugin.FormDesigner.Models;

namespace AgileEAP.Plugin.FormDesigner.Controllers
{
    public class FormController : BaseController
    {
        public FormController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }
        public ActionResult Form()
        {
            try
            {
                string processDefID = Request.QueryString["processDefID"];
                string eFormID = Request.QueryString["eFormID"];
                string entry = Request.QueryString["Entry"];
                eForm formInfo = null;
                if (string.IsNullOrEmpty(processDefID) && !string.IsNullOrEmpty(eFormID))
                {
                    formInfo = repository.GetDomain<eForm>(eFormID);
                }
                if (formInfo != null && !string.IsNullOrEmpty(formInfo.Content))
                {
                    Form form = JsonConvert.DeserializeObject<Form>(formInfo.Content);
                    string processInstID = Request.QueryString["processInstID"];
                    //ProcessForm processForm = null;
                    IDictionary<string, object> values = new Dictionary<string, object>();
                    //if (!string.IsNullOrEmpty(processInstID))
                    //{
                    //    IDictionary<string, object> parameters = new Dictionary<string, object>();
                    //    parameters.SafeAdd("processInstID", processInstID);
                    //    processForm = repository.FindOne<ProcessForm>(parameters);
                    //    if (processForm != null)
                    //    {
                    //        parameters.Clear();
                    //        parameters.Add("ID", processForm.BizID);
                    //        DataTable dt = repository.ExecuteDataTable<ProcessForm>(string.Format("select * from {0}", processForm.BizTable), parameters);
                    //        if (dt != null && dt.Rows.Count > 0)
                    //        {

                    //            foreach (DataRow row in dt.Rows)
                    //            {
                    //                foreach (var field in form.Fields)//manualActivity.Form.Fields)
                    //                {

                    //                    if (field.ControlType == ControlType.SysVariable)
                    //                    {
                    //                        switch (field.DefaultValue.ToInt())
                    //                        {
                    //                            case (short)SystemControlType.OrgID:
                    //                                Organization org = repository.Query<Organization>().FirstOrDefault(o => o.Code == workContext.User.OrgID);
                    //                                values.SafeAdd(field.Name, org.Name);
                    //                                if (!string.IsNullOrEmpty(field.ExtendData))
                    //                                {
                    //                                    values.SafeAdd(field.ExtendData, workContext.User.OrgID);
                    //                                }
                    //                                break;
                    //                            case (short)SystemControlType.UserID:
                    //                                values.SafeAdd(field.Name, workContext.User.Name);
                    //                                if (!string.IsNullOrEmpty(field.ExtendData))
                    //                                {
                    //                                    values.SafeAdd(field.ExtendData, workContext.User.ID);
                    //                                }
                    //                                break;
                    //                            case (short)SystemControlType.CurrentDate:
                    //                                values.SafeAdd(field.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //                                break;
                    //                        }

                    //                    }
                    //                    else if (dt.Columns.Contains(field.Name))
                    //                    {
                    //                        if (!string.IsNullOrEmpty(row[field.Name].ToSafeString()))
                    //                        {
                    //                            values.SafeAdd(field.Name, row[field.Name]);
                    //                        }
                    //                        else
                    //                        {
                    //                            if (!string.IsNullOrEmpty(field.DefaultValue))
                    //                                values.SafeAdd(field.Name, field.DefaultValue);
                    //                        }
                    //                    }

                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //if (processForm == null)
                    //{
                        foreach (var field in form.Fields)//manualActivity.Form.Fields)
                        {
                            if (field.ControlType == ControlType.SysVariable)
                            {
                                switch (field.DefaultValue.ToInt())
                                {
                                    case (short)SystemControlType.OrgID:
                                        Organization org = repository.Query<Organization>().FirstOrDefault(o => o.Code == workContext.User.OrgID);
                                        values.SafeAdd(field.Name, org.Name);
                                        if (!string.IsNullOrEmpty(field.ExtendData))
                                        {
                                            values.SafeAdd(field.ExtendData, workContext.User.OrgID);
                                        }
                                        break;
                                    case (short)SystemControlType.UserID:
                                        values.SafeAdd(field.Name, workContext.User.Name);
                                        if (!string.IsNullOrEmpty(field.ExtendData))
                                        {
                                            values.SafeAdd(field.ExtendData, workContext.User.ID);
                                        }
                                        break;
                                    case (short)SystemControlType.CurrentDate:
                                        values.SafeAdd(field.Name, DateTime.Now.ToSafeDateTime());
                                        break;
                                }
                            }
                            else if (field.ControlType != ControlType.SysVariable && field.ControlType != ControlType.HiddenInput)
                            {
                                if (!string.IsNullOrEmpty(field.DefaultValue))
                                    values.SafeAdd(field.Name, field.DefaultValue);
                            }
                            else
                            {
                                // values.SafeAdd(field.Name, workContext.);
                            }
                        }
                    //}
                    //ViewData["Values"] = values;
                    //ViewData["Form"] = form;//manualActivity.Form;
                        FormModel formModel = new FormModel()
                        {
                            Form = form,
                            Values = values
                        };

                }
            }
            catch (Exception ex)
            {
                log.Error("初始化表单失败" + ex);
            }
            return View();
        }
    }
}
