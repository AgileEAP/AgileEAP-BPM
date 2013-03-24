using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.Mvc;

using AgileEAP.Core;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Infrastructure;
using AgileEAP.Core.Extensions;

using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;

using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Enums;
using AgileEAP.Plugin.Workflow.Models;

using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace AgileEAP.Plugin.Workflow.Controllers
{
    public class EngineController : BaseController
    {
        private IWorkflowEngine workflowEngine;

        public EngineController(IWorkflowEngine workflowEngine, IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {
            this.workflowEngine = workflowEngine;
        }

        public ActionResult MyWorkItem()
        {
            return View();
        }

        public ActionResult MyWorkItem_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            IDictionary<string, object> parameters = filter.Parameters;
            if (Request.QueryString["Entry"] == "WaitExecute")
            {
                parameters.SafeAdd("CurrentState", (short)WorkItemStatus.WaitExecute);
            }
            else if (Request.QueryString["Entry"] == "Compeleted")
            {
                parameters.SafeAdd("CurrentState", new Condition(string.Format(" CurrentState !={0}", (short)WorkItemStatus.WaitExecute)));
            }

            string sortCommand = filter.SortCommand ?? "order by CreateTime desc";
            IPageOfList<WorkItem> data = workContext.IsAdmin ? repository.FindAll<WorkItem>(parameters, sortCommand, filter.PageInfo) : workflowEngine.GetMyWorkItems(workContext.User.ID, parameters, sortCommand, filter.PageInfo);
            var result = new DataSourceResult()
            {
                Data = data.Select(o => new WorkItemModel() { ID = o.ID, Creator = o.Creator, Name = o.Name, CreatorName = o.CreatorName, ProcessInstID = o.ProcessInstID, ProcessInstName = o.ProcessInstName, StartTime = o.StartTime, CurrentState = o.CurrentState.Cast<WorkItemStatus>(WorkItemStatus.WaitExecute).GetRemark() }),
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        public ActionResult StartProcess()
        {
            return View();
        }

        public ActionResult StartProcess_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            filter.Parameters.SafeAdd("CurrentState", (short)ProcessStatus.Release);
            IPageOfList<ProcessDef> data = repository.FindAll<ProcessDef>(filter.Parameters, filter.SortCommand ?? "order by CreateTime desc", filter.PageInfo);
            var result = new DataSourceResult()
            {
                Data = data,
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        public ActionResult Process()
        {
            return View();
        }

        public ActionResult Process_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            IPageOfList<ProcessDef> data = repository.FindAll<ProcessDef>(filter.Parameters, filter.SortCommand ?? "order by CreateTime desc", filter.PageInfo);
            var result = new DataSourceResult()
            {
                Data = data.Select(o => new ProcessDefModel()
                {
                    ID = o.ID,
                    CategoryID = o.CategoryID,
                    CurrentFlag = o.CurrentFlag == 1 ? "是" : "否",
                    IsActive = o.IsActive == 1 ? "是" : "否",
                    Name = o.Name,
                    Text = o.Text,
                    Version = o.Version,
                    CurrentState = o.CurrentState.Cast<ProcessStatus>(ProcessStatus.Candidate).GetRemark()
                }),
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        #region 流程定义管理

        public ActionResult GetProcessDefID(string processInstID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("恢复流程实例{0}", processInstID);
            try
            {
                string id = repository.Query<ProcessInst>().Where(p => p.ID == processInstID).FirstOrDefault().ProcessDefID;
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = id;
                actionMessage += "成功";
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        public ActionResult DeleteProcess(string processDefID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("删除流程定义{0}", processDefID);
            try
            {
                ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);
                if (processDef != null)
                {
                    repository.Delete<ProcessDef>(processDef);
                    ajaxResult.Result = DoResult.Success;
                    ajaxResult.RetValue = processDefID;
                    actionMessage += "成功";
                    AddActionLog<ProcessDef>(processDef, ajaxResult.Result, actionMessage);
                }
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
                AddActionLog<ProcessDef>(actionMessage, ajaxResult.Result);
            }

            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        public ActionResult PublishProcess(string processDefID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("发布流程{0}", processDefID);
            try
            {
                ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);
                if (processDef != null)
                {
                    processDef.CurrentState = (short)ProcessStatus.Release; //已发布
                    repository.SaveOrUpdate(processDef);
                    ajaxResult.Result = DoResult.Success;
                    ajaxResult.RetValue = AgileEAP.Workflow.Enums.ProcessStatus.Release.GetRemark();
                    actionMessage = actionMessage + "成功";
                }
            }
            catch (Exception ex)
            {
                actionMessage = actionMessage + "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;

            return Json(ajaxResult);
        }

        public ActionResult TerminateProcess(string processDefID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("停止流程{0}", processDefID);
            try
            {
                ProcessDef processDef = repository.GetDomain<ProcessDef>(processDefID);
                if (processDef != null)
                {
                    processDef.CurrentState = (short)ProcessStatus.Terminated; //
                    repository.SaveOrUpdate(processDef);
                    ajaxResult.Result = DoResult.Success;
                    ajaxResult.RetValue = ProcessStatus.Terminated.GetRemark();
                    actionMessage = actionMessage + "成功";
                }
            }
            catch (Exception ex)
            {
                actionMessage = actionMessage + "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;

            return Json(ajaxResult);
        }

        #endregion

        #region 流程实例管理

        /// <summary>
        /// 启动流程生成一个实例
        /// </summary>
        /// <returns></returns>
        public JsonResult StartProcessInst()
        {
            string processDefID = Request.Form["processDefID"];
            try
            {
                string processInstID = workflowEngine.CreateAProcess(processDefID);
                workflowEngine.StartAProcess(processInstID, null);
                WorkItem wi = repository.Query<WorkItem>().FirstOrDefault(o => o.ProcessInstID == processInstID);

                ProcessDefine processdefine = workflowEngine.GetProcessDefine(processDefID);
                return Json(new { ProcessInstID = processInstID, WorkItemID = wi.ID, StartURL = processdefine.StartURL });
            }
            catch (Exception ex)
            {
                log.Error(string.Format("启动流程{0}的实例失败", processDefID), ex);
            }

            return Json(null);
        }

        /// <summary>
        /// 将当前状态改为“挂起”
        /// </summary>
        /// <returns></returns>
        public ActionResult SuspendProcessInst(string processInstID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("挂起流程实例{0}", processInstID);
            try
            {
                IWorkflowEngine engine = new WorkflowEngine();
                engine.TerminateProcessInst(processInstID);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = AgileEAP.Workflow.Enums.ProcessInstStatus.Suspended.GetRemark();
                actionMessage = actionMessage + "成功";
            }
            catch (Exception ex)
            {
                actionMessage = actionMessage + "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;

            return Json(ajaxResult);
        }

        public ActionResult TerminateProcessInst(string processInstID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("终止流程实例{0}", processInstID);
            try
            {
                IWorkflowEngine engine = new WorkflowEngine();
                engine.TerminateProcessInst(processInstID);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = AgileEAP.Workflow.Enums.ProcessInstStatus.Terminated.GetRemark();
                actionMessage = actionMessage + "成功";
            }
            catch (Exception ex)
            {
                actionMessage = actionMessage + "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;

            return Json(ajaxResult);
        }

        public ActionResult DeleteProcessInst(string processInstID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("删除流程实例{0}", processInstID);
            try
            {
                IWorkflowEngine engine = new WorkflowEngine();
                engine.DeleteProcessInst(processInstID);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = AgileEAP.Workflow.Enums.ProcessInstStatus.Terminated.GetRemark();
                actionMessage += "成功";
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }

            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        /// <summary>
        /// 将当前状态改为“运行”
        /// </summary>
        /// <returns></returns>
        public ActionResult ResumeProcessInst(string processInstID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("恢复流程实例{0}", processInstID);
            try
            {
                IWorkflowEngine engine = new WorkflowEngine();
                engine.ResumeProcessInst(processInstID);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = AgileEAP.Workflow.Enums.ProcessInstStatus.Running.GetRemark();
                actionMessage += "成功";
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<ProcessInst>(actionMessage, ajaxResult.Result);
            }
            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        #endregion

        public ActionResult WorkItemDetail()
        {


            try
            {
                string actionURL = string.Empty;
                WorkItem workItem = workflowEngine.Persistence.Repository.GetDomain<WorkItem>(Request.QueryString["workItemID"]);
                ActivityInst activityInst = workflowEngine.Persistence.GetActivityInst(workItem.ActivityInstID);

                string processDefID = workItem.ProcessID;
                string processInstID = workItem.ProcessInstID;
                Activity activity = workflowEngine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID);
                if (activity is ManualActivity)
                {
                    ManualActivity manualActivity = workflowEngine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID) as ManualActivity;
                    if (manualActivity.CustomURL.URLType == URLType.DefaultURL)
                    {
                        actionURL = "/Workflow/eForm" + Request.Url.Query;
                    }
                    else
                    {
                        actionURL = string.Format("{0}{1}", workItem.ActionURL ?? manualActivity.CustomURL.SpecifyURL, Request.Url.Query);
                    }
                }
                else
                {
                    actionURL = string.Format("{0}{1}", workItem.ActionURL, Request.Url.Query);
                }

                if (!actionURL.StartsWith("/")) actionURL = "/" + actionURL;


                IList<WorkflowTransitionModel> workflowTransitions = repository.ExecuteDataTable<ActivityInst>(string.Format(@"select a.ID, a.DestActID,a.DestActInstName,b.Executor,b.ExecutorName,b.ExecuteTime,a.TransTime,
                                                                                (case  when b.CurrentState is null then 5 else  b.CurrentState end) as CurrentState,
                                                                                (select om.Name from OM_Organization om inner join OM_Employee eo on om.ID=eo.MajorOrgID where eo.ID=b.Executor ) as OrgName 
                                                                                from WF_Transition a left join WF_WorkItem b on a.ProcessInstID=b.ProcessInstID and a.DestActInstID=b.ActivityInstID   
                                                                                where a.ProcessInstID ='{0}'
                                                                                order by a.TransTime", processInstID))
                                                                                .ToList<WorkflowTransitionModel>();
                ViewData["Url"] = actionURL;
                ViewData["ProcessDefID"] = processDefID;
                ViewData["ProcessInstID"] = processInstID;
                ViewData["workflowTransitions"] = workflowTransitions;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return View();
        }
    }
}
