using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.DatabaseStudio
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            //loadWorkflow
        }
    }
}