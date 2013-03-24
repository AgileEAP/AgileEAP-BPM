using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.Workflow.Models
{
    public class ProcessInstModel
    {
        public string ID { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public string CurrentState { get; set; }
        public string Description { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime FinalTime { get; set; }
        public short IsTimeOut { get; set; }
        public DateTime LimitTime { get; set; }
        public string Name { get; set; }
        public string ParentActivityID { get; set; }
        public string ParentProcessID { get; set; }
        public string ProcessDefID { get; set; }
        public string ProcessDefName { get; set; }
        public string ProcessVersion { get; set; }
        public DateTime RemindTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime TimeOutTime { get; set; }
    }
}
