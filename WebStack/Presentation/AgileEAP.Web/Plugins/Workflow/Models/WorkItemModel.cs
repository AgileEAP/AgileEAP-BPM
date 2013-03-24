using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.Workflow.Models
{
    public class WorkItemModel
    {
        public  string ID { get; set; }
        public  string Name { get; set; }
        public  string ProcessInstID { get; set; }
        public  string ProcessInstName { get; set; }
        public  string Creator { get; set; }
        public  string CreatorName { get; set; }
        public string CurrentState { get; set; }
        public  DateTime StartTime { get; set; }
    }
}
