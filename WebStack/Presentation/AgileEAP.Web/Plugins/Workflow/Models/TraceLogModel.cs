using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.Workflow.Models
{
    public class TraceLogModel
    {
        public string Operator { get; set; }
        public string ClientIP { get; set; }
        public string Message { get; set; }
        public string ProcessInstName { get; set; }
        public string ActivityInstName { get; set; }
        public string WorkItemName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
