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

using AgileEAP.Core.Data;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.Plugin.Workflow
{
    public partial class MyWorkItemDetail : BasePage
    {
        public string Url = string.Empty;
        public string ProcessDefID = string.Empty;
        public string ProcessInstID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IWorkflowEngine engine = new WorkflowEngine();
                    WorkItem workItem = engine.Persistence.Repository.GetDomain<WorkItem>(Request.QueryString["workItemID"]);
                    ActivityInst activityInst = engine.Persistence.GetActivityInst(workItem.ActivityInstID);

                    ProcessDefID = workItem.ProcessID;
                    ProcessInstID = workItem.ProcessInstID;
                    Activity activity = engine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID);
                    if (activity is ManualActivity)
                    {
                        ManualActivity manualActivity = engine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID) as ManualActivity;
                        if (manualActivity.CustomURL.URLType == URLType.DefaultURL)
                        {
                            Url = "/Workflow/eForm" + Request.Url.Query;
                        }
                        else
                        {
                            Url = string.Format("{0}{1}{2}", RootPath, workItem.ActionURL ?? manualActivity.CustomURL.SpecifyURL, Request.Url.Query);
                        }
                    }
                    else
                    {
                        Url = string.Format("{0}{1}{2}", RootPath, workItem.ActionURL, Request.Url.Query);
                    }

                    //IList<WorkItem> workItems = repository.Query<WorkItem>().Where(o => o.ProcessInstID == workItem.ProcessInstID).OrderBy(o => o.ExecuteTime).ToList();
                    //ProcessInst pi = repository.GetDomain<ProcessInst>(workItem.ProcessInstID);
                    //if (pi.CurrentState == (short)ProcessInstStatus.Completed || pi.CurrentState == (short)ProcessInstStatus.Terminated)
                    //{
                    //    workItems.Add(new WorkItem()
                    //    {
                    //        Name = "结束",
                    //        ExecutorName = "工作流引擎",
                    //        Executor = "WorkflowEngine"
                    //    });
                    //}

                    System.Data.DataTable dt = repository.ExecuteDataTable<ActivityInst>(string.Format(@"select a.ID, a.DestActID,a.DestActInstName,b.Executor,b.ExecutorName,b.ExecuteTime,a.TransTime,
(case  when b.CurrentState is null then 5 else  b.CurrentState end) as CurrentState
from WF_Transition a left join WF_WorkItem b on a.ProcessInstID=b.ProcessInstID and a.DestActInstID=b.ActivityInstID   
where a.ProcessInstID ='{0}'
order by a.TransTime", ProcessInstID));

                    gvList.DataSource = dt;
                    gvList.DataBind();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }
        }

    }
}