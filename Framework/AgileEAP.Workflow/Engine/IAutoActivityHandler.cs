using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileEAP.Workflow.Domain;

namespace AgileEAP.Workflow.Engine
{
    public interface IAutoActivityHandler
    {
        bool Execute(WorkItem wi);
    }
}
