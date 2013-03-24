using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AgileEAP.Core;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;
using AgileEAP.WebControls;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Enums;
using AgileEAP.Core.Utility;

using System.Reflection;

namespace AgileEAP.Plugin.Workflow
{
    public partial class ActivityTransition : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string processDefID = Request.QueryString["ProcessDefID"];
                string srcActivity = Request.QueryString["SrcActivity"];
                string destActivity = Request.QueryString["DestActivity"];
                try
                {
                    WorkflowEngine engine = new WorkflowEngine();
                    ProcessDefine processDefine = engine.GetProcessDefine(processDefID);

                    AgileEAP.Workflow.Definition.Transition transition = processDefine.Transitions.FirstOrDefault(t => t.SrcActivity == srcActivity && t.DestActivity == destActivity);

                    bool isDefault = string.IsNullOrEmpty(transition.Name) && string.IsNullOrEmpty(transition.Expression);
                    txtExpression.Text = transition.Expression;
                    txtName.Text = isDefault ? "默认连接" : transition.Name;
                    chkIsDefault.Checked = isDefault ? true : transition.IsDefault;
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("显示流程{0}迁移srcActivity={1},destActivity={2}信息出错", processDefID, srcActivity, destActivity), ex);
                }
            }
        }

        public string Save(string json)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            string processDefID = Request.QueryString["ProcessDefID"];
            string srcActivity = Request.QueryString["SrcActivity"];
            string destActivity = Request.QueryString["DestActivity"];
            try
            {
                WorkflowEngine engine = new WorkflowEngine();
                ProcessDefine processDefine = engine.GetProcessDefine(processDefID);
                ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);

                AgileEAP.Workflow.Definition.Transition transition = processDefine.Transitions.FirstOrDefault(t => t.SrcActivity == srcActivity && t.DestActivity == destActivity);
                IDictionary<string, object> values = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);

                transition.IsDefault = values.GetSafeValue<bool>("IsDefault", false);
                transition.Name = values.GetSafeValue<string>("Name", transition.Name);
                transition.Expression = values.GetSafeValue<string>("Expression", transition.Expression);
                processDef.Content = processDefine.ToXml();

                repository.SaveOrUpdate(processDef);

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);

                //记录操作日志
                AddActionLog(processDef, actionResult, string.Format("保存流程{0}迁移srcActivity={1},destActivity={2}信息", processDefID, srcActivity, destActivity));

                ajaxResult.Result = actionResult;
                ajaxResult.RetValue = srcActivity + ":" + destActivity;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                AddActionLog<ProcessDef>(string.Format("保存流程{0}迁移srcActivity={1},destActivity={2}信息", processDefID, srcActivity, destActivity), ajaxResult.Result);
                log.Error(string.Format("显示流程{0}迁移srcActivity={1},destActivity={2}信息出错", processDefID, srcActivity, destActivity), ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
    }
}