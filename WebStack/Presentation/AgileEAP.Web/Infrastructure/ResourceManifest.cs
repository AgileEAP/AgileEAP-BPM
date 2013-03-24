using AgileEAP.UI.Resources;

namespace AgileEAP.Web
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("site_layout_default").SetUrl("Themes/Default/layout.css").SetVersion("1.0");
        }
    }
}