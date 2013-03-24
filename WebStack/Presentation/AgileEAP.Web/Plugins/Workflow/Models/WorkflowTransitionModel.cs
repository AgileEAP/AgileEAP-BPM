using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.Workflow.Models
{
    public class WorkflowTransitionModel
    {
        public string ID { get; set; }
        public string DestActInstName { get; set; }
        public string CurrentState { get; set; }
        public string Executor { get; set; }
        public string ExecutorName { get; set; }
        public string OrgName { get; set; }
        public DateTime ExecuteTime { get; set; }
    }
}
