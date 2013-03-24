using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Infrastructure;
using AgileEAP.Core.Extensions;

using System.Web.Mvc;

using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using System.ComponentModel;

using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Enums;

using AgileEAP.Plugin.Workflow;
using AgileEAP.Plugin.Workflow.Models;

using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace AgileEAP.Plugin.Workflow.Controllers
{
    public class MonitorController : BaseController
    {
        private IWorkflowEngine workflowEngine;

        public MonitorController(IWorkflowEngine workflowEngine, IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {
            this.workflowEngine = workflowEngine;
        }

        public ActionResult ProcessInst()
        {
            return View();
        }

        public ActionResult ProcessInst_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            IDictionary<string, object> parameters = filter.Parameters;
            if (parameters != null && parameters.ContainsKey("CurrentState"))
            {
                string currentState = parameters.GetSafeValue<string>("CurrentState");
                switch (currentState)
                {
                    case "未启动":
                        parameters.SafeAdd("CurrentState", 1);
                        break;
                    case "运行":
                        parameters.SafeAdd("CurrentState", 2);
                        break;
                    case "挂起":
                        parameters.SafeAdd("CurrentState", 3);
                        break;
                    case "完成":
                        parameters.SafeAdd("CurrentState", 4);
                        break;
                    case "终止":
                        parameters.SafeAdd("CurrentState", 5);
                        break;
                    case "取消":
                        parameters.SafeAdd("CurrentState", 6);
                        break;
                }
            }

            IPageOfList<ProcessInst> data = repository.FindAll<ProcessInst>(parameters, filter.SortCommand ?? "order by StartTime desc", filter.PageInfo);
            var result = new DataSourceResult()
            {
                Data = data.Select(o => new ProcessInstModel() { ID = o.ID, Name = o.Name, CurrentState = o.CurrentState.Cast<ProcessInstStatus>(ProcessInstStatus.Running).GetRemark(), ProcessVersion = o.ProcessVersion, StartTime = o.StartTime, EndTime = o.EndTime }),
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        public ActionResult Transition()
        {
            return View();
        }

        public ActionResult Transition_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            IPageOfList<AgileEAP.Workflow.Domain.Transition> data = repository.FindAll<AgileEAP.Workflow.Domain.Transition>(filter.Parameters, filter.SortCommand ?? "order by transTime desc", filter.PageInfo);

            var result = new DataSourceResult()
            {
                Data = data,
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        public ActionResult TraceLog()
        {
            return View();
        }

        public ActionResult TraceLog_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            var data = repository.ExecuteDataTable<TraceLog>(@"select * from (select tl.Message,tl.ClientIP,tl.CreateTime,pi.Name as ProcessInstName, ai.Name as ActivityInstName,wi.Name as WorkItemName,ot.UserName as Operator
                                                            from WF_TraceLog tl join WF_ProcessDef pd on tl.ProcessID = pd.ID
                                                            inner join WF_ProcessInst pi on tl.ProcessInstID = pi.ID 
                                                            inner join WF_ActivityInst ai on tl.ActivityInstID = ai.ID
                                                            inner join WF_WorkItem wi on tl.WorkItemID = wi.ID
                                                            inner join AC_Operator ot on tl.Operator = ot.ID) t ",
                                                             filter.Parameters, filter.SortCommand, filter.PageInfo);

            var result = new DataSourceResult()
            {
                Data = data.ToList<TraceLogModel>(),
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }
    }
}
