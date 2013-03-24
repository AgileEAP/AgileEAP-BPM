using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using AgileEAP.Core;
using AgileEAP.Workflow;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Domain;
using AgileEAP.Core.Data;

namespace AgileEAP.Workflow.UnitTests
{
    [TestClass]
    public class WorkflowEngineTest
    {
        [TestMethod]
        public void GetActiveWorkItemsTest()
        {
            IWorkflowEngine engine = new WorkflowEngine();

            var processInstID = "4c571b40-0732-4d0c-8434-a08400b24258";
            var value = engine.GetActiveWorkItems(processInstID);

            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void ErrorActivityInstTest()
        {
            IWorkflowEngine engine = new WorkflowEngine();

            var processInstID = "1e5438f0-9ef6-4bc3-8fe5-a0b000f97ef9";
            engine.ErrorActivityInst(processInstID);

            Assert.IsNotNull(true);
        }

        [TestMethod]
        public void FormJsonTest()
        {
            IWorkflowEngine engine = new WorkflowEngine();

            var processDefID = "06528f7e-5a95-49ce-8e04-7fe4890569a0";
            ProcessDefine processDefine = engine.GetProcessDefine(processDefID);

            string json = AgileEAP.Core.JsonConvert.SerializeObject(processDefine, new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = false });
            ProcessDefine processDefine2 = AgileEAP.Core.JsonConvert.DeserializeObject<ProcessDefine>(json, new ActivityConvert());

            Assert.IsNotNull(true);
        }


        [TestMethod]
        public void ActivityConvertTest()
        {
            IRepository<string> repository = new Repository<string>();
            ProcessDefine processDefine = new ProcessDefine(repository.Query<ProcessDef>().First(o => o.Name == "PatchVirtualMachine").Content);

            //System.Web.Script.Serialization.JavaScriptSerializer javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            //string ajson = javaScriptSerializer.Serialize(processDefine);
            string json = AgileEAP.Core.JsonConvert.SerializeObject(processDefine, new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
            ProcessDefine processDefine2 = AgileEAP.Core.JsonConvert.DeserializeObject<ProcessDefine>(json, new ActivityConvert());

            Assert.AreEqual(processDefine.Name, processDefine2.Name);
        }
    }
}
