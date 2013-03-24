using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;




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
    public partial class StartProcessForm : BasePage
    {
        IWorkflowEngine engine = new WorkflowEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string workItemID = Request.QueryString["workItemID"];
                WorkItem workItem = engine.Persistence.Repository.GetDomain<WorkItem>(workItemID);
                ActivityInst activityInst = engine.Persistence.GetActivityInst(workItem.ActivityInstID);
                ManualActivity manualActivity = engine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID) as ManualActivity;

                AgileEAP.EForm.FormView formView = new AgileEAP.EForm.FormView();
                formView.Form = manualActivity.Form;
                hidDataSource.Value = manualActivity.Form.DataSource;

                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.SafeAdd("processInstID", Request.QueryString["processInstID"]);
                ProcessForm processForm = engine.Persistence.Repository.FindOne<ProcessForm>(parameters);
                if (processForm != null)
                {
                    parameters.Clear();
                    parameters.Add("ID", processForm.BizID);
                    DataTable dt = engine.Persistence.Repository.ExecuteDataTable<ProcessForm>(string.Format("select * from {0}", processForm.BizTable), parameters);

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

                eForm.Controls.Add(formView);
            }
        }


        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            string processInstID = Request.QueryString["processInstID"];
            ProcessForm processForm;
            try
            {
                Dictionary<string, object> formValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(argument);

                string table = Request.Form["DataSource"];
                StringBuilder sbFields = new StringBuilder();
                StringBuilder sbValues = new StringBuilder();
                string cmdText = string.Empty;
                StringBuilder sbUpdateValues = new StringBuilder();

                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.SafeAdd("ProcessInstID", processInstID);
                processForm = engine.Persistence.Repository.FindOne<ProcessForm>(parameters);
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
                    string bizID = IdGenerator.NewComb().ToString();
                    formValues.SafeAdd("ID", bizID);
                    foreach (var item in formValues)
                    {
                        sbFields.Append(item.Key).Append(",");
                        sbValues.Append(":").Append(item.Key).Append(",");
                    }

                    processForm = new ProcessForm()
                    {
                        ID = IdGenerator.NewComb().ToString(),
                        BizID = bizID,
                        BizTable = table,
                        CreateTime = DateTime.Now,
                        Creator = User.ID,
                        ProcessInstID = processInstID,
                        KeyWord = sbValues.ToString()
                    };
                    cmdText = string.Format("insert into {0}({1}) values({2})", table, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                }

                string workItemID = Request.QueryString["workItemID"];
                UnitOfWork.ExecuteWithTrans<WorkItem>(() =>
                {
                    engine.Persistence.Repository.ExecuteSql<WorkItem>(cmdText, formValues);
                    engine.Persistence.Repository.SaveOrUpdate(processForm);
                    engine.CompleteWorkItem(workItemID, formValues);
                });

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = string.Format("启动工作流实例{0}工作项{1}", processForm.ProcessInstID, workItemID);

                //记录操作日志
                AddActionLog(processForm, actionResult, actionMessage);

                ajaxResult.Result = actionResult;
                ajaxResult.RetValue = processForm.BizID;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);

        }
    }
}