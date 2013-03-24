using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.FormDesigner
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("FormDesignerjs").SetUrl("FormDesigner.js").SetVersion("1.0");
            manifest.DefineScript("ConfigureControljs").SetUrl("ConfigureControl.js").SetVersion("1.0");

            //workflowDesigner_default
            manifest.DefineStyle("FormDesigner_default").SetUrl("Themes/Default/FormDesigner.css").SetVersion("1.0");
            manifest.DefineStyle("ConfigureControl_default").SetUrl("Themes/Default/ConfigureControl.css").SetVersion("1.0");
            manifest.DefineStyle("FormUI_default").SetUrl("Themes/Default/FormUI.css").SetVersion("1.0");
            manifest.DefineStyle("eForm_default").SetUrl("Themes/Default/eForm.css").SetVersion("1.0");
        }
    }
}