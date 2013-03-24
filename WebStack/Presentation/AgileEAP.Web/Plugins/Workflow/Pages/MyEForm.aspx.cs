using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;




using AgileEAP.Core;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;

using AgileEAP.WebControls;
using AgileEAP.Core.Data;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.Plugin.Workflow
{
    public partial class MyEForm : BasePage
    {
        IWorkflowEngine engine = new WorkflowEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            string processDefID = Request.QueryString["processDefID"];
            string entry = Request.QueryString["Entry"];
            ManualActivity manualActivity = null;
            if (!string.IsNullOrEmpty(processDefID) && entry.EqualIgnoreCase("StartProcess"))
            {
                Activity startActivity = engine.Persistence.GetStartActivity(processDefID);
                manualActivity = engine.Persistence.GetOutActivities(processDefID, startActivity.ID)[0] as ManualActivity;
            }
            else
            {
                string workItemID = Request.QueryString["workItemID"];
                WorkItem workItem = repository.GetDomain<WorkItem>(workItemID);
                ActivityInst activityInst = engine.Persistence.GetActivityInst(workItem.ActivityInstID);
                manualActivity = engine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID) as ManualActivity;
            }

            AgileEAP.EForm.FormView formView = new AgileEAP.EForm.FormView();
            formView.Form = manualActivity.Form;
            hidDataSource.Value = manualActivity.Form.DataSource;

            string processInstID = Request.QueryString["processInstID"];
            if (!string.IsNullOrEmpty(processInstID))
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.SafeAdd("processInstID", processInstID);
                ProcessForm processForm = repository.FindOne<ProcessForm>(parameters);
                if (processForm != null)
                {
                    parameters.Clear();
                    parameters.Add("ID", processForm.BizID);
                    DataTable dt = repository.ExecuteDataTable<ProcessForm>(string.Format("select * from {0}", processForm.BizTable), parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IDictionary<string, object> values = new Dictionary<string, object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (DataColumn column in dt.Columns)
                            {
                                values.SafeAdd(column.ColumnName, row[column.ColumnName]);
                            }
                        }
                        formView.Values = values;
                    }
                }
            }

            eForm.Controls.Add(formView);
        }

        public string Save(string json)
        {
            AjaxResult ajaxResult = new AjaxResult();
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            ProcessForm processForm = null;
            try
            {
                string processDefID = Request.QueryString["processDefID"];
                string entry = Request.QueryString["Entry"];
                string processInstID = Request.QueryString["processInstID"];
                string workItemID = Request.QueryString["workItemID"];
                UnitOfWork.ExecuteWithTrans<ProcessDef>(() =>
                {
                    if (!string.IsNullOrEmpty(processDefID) && entry.EqualIgnoreCase("StartProcess"))
                    {
                        processInstID = engine.CreateAProcess(processDefID);// string.Empty; // TODO: Initialize to an appropriate value
                        engine.StartAProcess(processInstID, null);
                        WorkItem wi = repository.Query<WorkItem>().FirstOrDefault(w => w.ProcessInstID == processInstID);
                        workItemID = wi.ID;
                        AddActionLog(wi, DoResult.Success, string.Format("启动流程实像{0}", processInstID));
                    }

                    Dictionary<string, object> formValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string table = Request.Form["DataSource"];
                    StringBuilder sbFields = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();
                    StringBuilder sbUpdateValues = new StringBuilder();
                    string cmdText = string.Empty;

                    processForm = repository.Query<ProcessForm>().FirstOrDefault(pf => pf.ProcessInstID == processInstID);
                    if (processForm != null)
                    {
                        foreach (var item in formValues)
                        {
                            if (sbUpdateValues.Length > 0)
                                sbUpdateValues.Append(",");
                            sbUpdateValues.AppendFormat("{0} = :{0}", item.Key);
                        }
                        cmdText = string.Format("update {0} set {1} where ID ='{2}'", table, sbUpdateValues.ToString(), processForm.BizID);
                    }
                    else
                    {
                        formValues.SafeAdd("ID", IdGenerator.NewComb().ToSafeString());
                        formValues.SafeAdd("Applicant", User.ID);
                        formValues.SafeAdd("ApplyTime", DateTime.Now);

                        foreach (var item in formValues)
                        {
                            sbFields.Append(item.Key).Append(",");
                            sbValues.Append(":").Append(item.Key).Append(",");
                        }

                        processForm = new ProcessForm()
                        {
                            ID = IdGenerator.NewComb().ToString(),
                            BizID = formValues.GetSafeValue<string>("ID"),
                            BizTable = table,
                            CreateTime = DateTime.Now,
                            Creator = User.ID,
                            ProcessInstID = processInstID,
                            KeyWord = sbValues.ToString()
                        };
                        cmdText = string.Format("insert into {0}({1}) values({2})", table, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                    }

                    engine.Persistence.Repository.ExecuteSql<WorkItem>(cmdText, formValues);
                    engine.Persistence.Repository.SaveOrUpdate(processForm);
                    // engine.CompleteWorkItem(User, workItemID, formValues);
                });

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = string.Format("保存工作流实例{0}工作项{1}", processForm.ProcessInstID, workItemID);

                //记录操作日志
                AddActionLog(processForm, actionResult, actionMessage);

                ajaxResult.Result = actionResult;
                ajaxResult.RetValue = processForm.BizID;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                //记录操作日志
                AddActionLog(processForm, ajaxResult.Result, actionMessage);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);

        }

        public string Submit(string json)
        {
            AjaxResult ajaxResult = new AjaxResult();
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            ProcessForm processForm = null;
            try
            {
                string processDefID = Request.QueryString["processDefID"];
                string entry = Request.QueryString["Entry"];
                string processInstID = Request.QueryString["processInstID"];
                string workItemID = Request.QueryString["workItemID"];
                UnitOfWork.ExecuteWithTrans<ProcessDef>(() =>
                {
                    if (!string.IsNullOrEmpty(processDefID) && entry.EqualIgnoreCase("StartProcess"))
                    {
                        processInstID = engine.CreateAProcess(processDefID);// string.Empty; // TODO: Initialize to an appropriate value
                        engine.StartAProcess(processInstID, null);
                        WorkItem wi = repository.Query<WorkItem>().FirstOrDefault(w => w.ProcessInstID == processInstID);
                        workItemID = wi.ID;
                        AddActionLog(wi, DoResult.Success, string.Format("启动流程实像{0}", processInstID));
                    }

                    Dictionary<string, object> formValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string table = Request.Form["DataSource"];
                    StringBuilder sbFields = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();
                    StringBuilder sbUpdateValues = new StringBuilder();
                    string cmdText = string.Empty;

                    processForm = repository.Query<ProcessForm>().FirstOrDefault(pf => pf.ProcessInstID == processInstID);
                    if (processForm != null)
                    {
                        foreach (var item in formValues)
                        {
                            if (sbUpdateValues.Length > 0)
                                sbUpdateValues.Append(",");
                            sbUpdateValues.AppendFormat("{0} = :{0}", item.Key);
                        }
                        cmdText = string.Format("update {0} set {1} where ID ='{2}'", table, sbUpdateValues.ToString(), processForm.BizID);
                    }
                    else
                    {
                        formValues.SafeAdd("ID", IdGenerator.NewComb().ToSafeString());
                        formValues.SafeAdd("Applicant", User.ID);
                        formValues.SafeAdd("ApplyTime", DateTime.Now);

                        foreach (var item in formValues)
                        {
                            sbFields.Append(item.Key).Append(",");
                            sbValues.Append(":").Append(item.Key).Append(",");
                        }

                        processForm = new ProcessForm()
                        {
                            ID = IdGenerator.NewComb().ToString(),
                            BizID = formValues.GetSafeValue<string>("ID"),
                            BizTable = table,
                            CreateTime = DateTime.Now,
                            Creator = User.ID,
                            ProcessInstID = processInstID,
                            KeyWord = sbValues.ToString()
                        };
                        cmdText = string.Format("insert into {0}({1}) values({2})", table, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                    }

                    engine.Persistence.Repository.ExecuteSql<WorkItem>(cmdText, formValues);
                    engine.Persistence.Repository.SaveOrUpdate(processForm);
                    engine.CompleteWorkItem(workItemID, formValues);
                });

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = string.Format("完成工作流实例{0}工作项{1}", processForm.ProcessInstID, workItemID);

                //记录操作日志
                AddActionLog(processForm, actionResult, actionMessage);

                ajaxResult.Result = actionResult;
                ajaxResult.RetValue = processForm.BizID;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                //记录操作日志
                AddActionLog(processForm, ajaxResult.Result, actionMessage);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);

        }

        public string Rollback(string json)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string workItemID = Request.QueryString["workItemID"];
            string actionMessage = string.Format("退回工作流实例工作项{0}", workItemID);
            try
            {
                //回退工作项
                engine.RollbackWorkItem(User, workItemID);

                ajaxResult.Result = DoResult.Success;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<WorkItem>(actionMessage, ajaxResult.Result);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
    }
}