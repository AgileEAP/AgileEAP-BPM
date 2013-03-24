using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.Workflow.Models
{
    public class ProcessDefModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string CategoryID { get; set; }
        public string CurrentState { get; set; }
        public string CurrentFlag { get; set; }
        public string IsActive { get; set; }
        public string Version { get; set; }
    }
}
