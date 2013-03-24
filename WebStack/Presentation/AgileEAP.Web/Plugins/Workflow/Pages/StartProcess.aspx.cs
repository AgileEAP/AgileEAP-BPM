using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Service;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Enums;
using AgileEAP.Core.Web;
using AgileEAP.Core;


namespace AgileEAP.Plugin.Workflow
{
    public partial class StartProcess : BasePage
    {
        private ProcessDefService processDefService = new ProcessDefService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ShowList();
            }
        }

        private void ShowList()
        {
            List<ProcessDef> process = processDefService.All().Where(o => o.CurrentState == 1).OrderBy(o => o.CategoryID).ToList();//状态为“已发布”的流程列表
            gvList.DataSource = process;
            gvList.DataBind();
        }

        public string StartProcessInst(string processDefID)
        {
            AjaxResult ajaxResult = new AjaxResult();
            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;

            try
            {
                WorkflowEngine engine = new WorkflowEngine(); // TODO: Initialize to an appropriate value
                string processInstID = engine.CreateAProcess(processDefID);// string.Empty; // TODO: Initialize to an appropriate value
                engine.StartAProcess(processInstID, null);

                ProcessInst pi = repository.GetDomain<ProcessInst>(processInstID);
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ProcessInstID", processInstID);

                WorkItem wi = repository.FindOne<WorkItem>(parameters);

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);

                //记录操作日志
                AddActionLog(pi, actionResult, actionMessage);

                ajaxResult.Result = actionResult;
                ajaxResult.RetValue = pi.ID + '$' + wi.ID;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);

        }

        public string GetDictItemText(string value, string dict)
        {

            return value.GetDictItemText(dict);
        }
    }
}