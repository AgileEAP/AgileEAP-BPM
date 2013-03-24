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
using AgileEAP.Infrastructure.Domain;


namespace AgileEAP.Plugin.Workflow
{
    public partial class ActivityManager : BasePage
    {
        WorkflowEngine wfEngine = new WorkflowEngine();
        public string ProcessDefID
        {
            get
            {
                return Request.QueryString["ProcessDefID"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(ProcessDefID))
                {
                    ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
                }
            }
        }
        /// <summary>
        /// 显示列表信息
        /// </summary>
        /// <param name="gvList">GridView对象</param>
        /// <param name="pageInfo">分页信息</param>
        public void ShowList(PagedGridView gvList, PageInfo pageInfo)
        {
            WorkflowEngine wfEngine = new WorkflowEngine();
            IList<Activity> result = wfEngine.GetProcessDefine(ProcessDefID).Activities.OrderBy(a => a.ActivityType).ToList();

            gvList.ItemCount = result.Count;
            gvList.DataSource = result;
            gvList.DataBind();
        }

        public void Delete(string id)
        {
            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;

            try
            {
                string processDefID = Request.Form["ProcessDefID"];
                string activityID = id;
                ProcessDefine processDefine = wfEngine.GetProcessDefine(ProcessDefID);
                Activity activity = processDefine.Activities.FirstOrDefault(a => a.ID == activityID);
                processDefine.Activities.Remove(activity);

                foreach (var transition in processDefine.Transitions.Where(t => t.DestActivity == activityID || t.SrcActivity == activityID).ToList())
                {
                    processDefine.Transitions.Remove(transition);
                }

                // ProcessDefService processDefService = new ProcessDefService();
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.SafeAdd("Name", processDefine.ID);
                parameters.SafeAdd("Version", processDefine.Version);
                ProcessDef processDef = repository.FindOne<ProcessDef>(parameters);
                processDef.Content = processDefine.ToXml();

                repository.SaveOrUpdate(processDef);

                wfEngine.ClearProcessCache();

                actionResult = DoResult.Success;
                //获取提示信息
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);

                //显示提示信息
                WebUtil.PromptMsg(actionMessage);

                //刷新页面
                Refresh();

            }
            catch (Exception ex)
            {
                //获取提示信息
                actionMessage = RemarkAttribute.GetEnumRemark(actionResult);
                log.Error(ex);
            }

        }


        private bool IsRelate()
        {
            bool isTrue = false;
            List<AgileEAP.Workflow.Definition.Transition> transitions = wfEngine.GetProcessDefine(ProcessDefID).Transitions;
            foreach (var transition in transitions)
            {
                if (transition.SrcActivity == CurrentId || transition.DestActivity == CurrentId)
                {
                    isTrue = true;
                    break;
                }
            }
            return isTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        public void Update()
        {
            PageContext.Action = ActionType.Update;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }
        /// <summary>
        /// 转向明细页面
        /// </summary>
        /// <param name="param"></param>
        protected void Redirect(string param)
        {
            var currentIdParam = PageContext.Action == ActionType.Add ? string.Empty : string.Format("&CurrentId={0}", CurrentId);
            Response.Redirect(string.Format("ActivityDetail.aspx?ActionFlag={0}{1}{2}", PageContext.Action, currentIdParam, string.IsNullOrEmpty(param) ? param : "&" + param));
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
        }
        /*qlj 2013.1.8改  将表单独立于流程之外*/
        public string GetActivityForm(string activityName)
        {
            string actionMessage = string.Empty;
            string processDefID = Request.Form["ProcessDefID"];
            try
            {
                ProcessDefine processDefine = wfEngine.GetProcessDefine(ProcessDefID);
                ManualActivity activity = processDefine.Activities.FirstOrDefault(a => a.ID == activityName) as ManualActivity;
                if (activity != null && activity.eForm != null)//activity.Form != null)
                {
                    AgileEAP.Infrastructure.Domain.eForm formInfo = repository.GetDomain<eForm>(activity.eForm);
                    if (formInfo != null && !string.IsNullOrEmpty(formInfo.Content))
                    {
                        Form form = JsonConvert.DeserializeObject<Form>(formInfo.Content);
                        return JsonConvert.SerializeObject(new
                        {
                            DataSource = form.DataSource,//activity.Form.DataSource,
                            Title = form.Title,//activity.Form.Title,
                            Fields = form.Fields.Select(f => new//activity.Form.Fields.Select(f => new
                                {
                                    AccessPattern = f.AccessPattern.Cast<AccessPattern>(AccessPattern.ReadOnly).ToSafeString(),
                                    ControlType = f.ControlType.Cast<ControlType>(ControlType.TextBox).ToSafeString(),
                                    DataSource = f.DataSource,
                                    DataType = f.DataType.Cast<DataType>(DataType.String).ToSafeString(),
                                    DefaultValue = f.DefaultValue ?? string.Empty,
                                    Name = f.Name,
                                    Required = f.Required,
                                    Text = f.Text,
                                    SortOrder = f.SortOrder,
                                    X = f.X,
                                    Y = f.Y,
                                    Z = f.Z,
                                    Width = f.Width,
                                    Height = f.Height,
                                    Url = f.URL
                                    //AccessPattern = f.AccessPattern.Cast<AccessPattern>(AccessPattern.ReadOnly).GetRemark(),
                                    //ControlType = f.ControlType.Cast<ControlType>(ControlType.TextBox),
                                    //DataSource = f.DataSource,
                                    //DataType = f.DataType.Cast<DataType>(DataType.String),
                                    //DefaultValue = f.DefaultValue ?? string.Empty,
                                    //Name = f.Name,
                                    //Required = f.Required.ToSafeString().EqualIgnoreCase("False") ? "否" : "是",
                                    //Text = f.Text,
                                    //SortOrder = f.SortOrder
                                }).ToArray()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //获取提示信息
                string message = string.Format("选择流程{0}活动{1}表单出错", processDefID, activityName);
                log.Error(message, ex);
                AddActionLog<ProcessDef>(message, DoResult.Failed);
            }

            return string.Empty;
        }
    }
}