using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.Workflow
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("eForm_default").SetUrl("Themes/Default/eForm.css").SetVersion("1.0");

        }
    }
}