using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.WorkflowDesigner
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            //WorkflowDesigner
           // manifest.DefineScript("initWorkflowDesigner").SetUrl("initWorkflowDesigner.js").SetVersion("1.0");
            manifest.DefineScript("workflowDesigner").SetUrl("workflowDesigner.js").SetVersion("1.0");

            //workflowDesigner_default
            manifest.DefineStyle("workflowDesigner_default").SetUrl("Themes/Default/workflowDesigner.css").SetVersion("1.0");
            //ActivityDetail_default
            manifest.DefineStyle("ActivityDetail_default").SetUrl("Themes/Default/ActivityDetail.css").SetVersion("1.0");
            //ActivityDetailjs
            manifest.DefineScript("ActivityDetailjs").SetUrl("ActivityDetail.js").SetVersion("1.0");

            //ConnectionDetail_default
            manifest.DefineStyle("ConnectionDetail_default").SetUrl("Themes/Default/ConnectionDetail.css").SetVersion("1.0");
            //ConnectionDetailjs
            manifest.DefineScript("ConnectionDetailjs").SetUrl("ConnectionDetail.js").SetVersion("1.0");
        
        }
    }
}