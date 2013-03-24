using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Workflow.Domain;

namespace AgileEAP.Workflow.Engine
{
    [ServiceContract]
    public interface IWorkflowService
    {
        [OperationContract]
        string GetProcessDefine(string processDefID);

        [OperationContract]
        string GetProcessActivityInsts(string processInstID);

        [OperationContract]
        string GetProcessTransitions(string processInstID);
    }

    public class WorkflowService : IWorkflowService
    {
        IRepository<string> repository = new Repository<string>();

        public string GetProcessDefine(string processDefID)
        {
            return (repository.GetDomain<ProcessDef>(processDefID) ?? new ProcessDef()).Content;
        }

        public string GetProcessActivityInsts(string processInstID)
        {
            IList<ActivityInst> activityInsts = repository.Query<ActivityInst>().Where(a => a.ProcessInstID == processInstID).ToList();

            return JsonConvert.SerializeObject(activityInsts);
        }

        public string GetProcessTransitions(string processInstID)
        {
            IList<AgileEAP.Workflow.Domain.Transition> transactions = repository.Query<AgileEAP.Workflow.Domain.Transition>().Where(t => t.ProcessInstID == processInstID).ToList();

            return JsonConvert.SerializeObject(transactions);
        }
    }
}
