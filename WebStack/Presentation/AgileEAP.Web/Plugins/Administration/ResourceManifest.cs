using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.Administration
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            
            //manifest.DefineStyle("eclient_layout_default").SetUrl("Themes/Default/layout.css").SetVersion("1.0");
            //manifest.DefineStyle("index_default").SetUrl("Themes/Default/index.css").SetVersion("1.0");
            //manifest.DefineStyle("order_default").SetUrl("Themes/Default/order.css").SetVersion("1.0");
            //manifest.DefineStyle("orderinfo_default").SetUrl("Themes/Default/orderinfo.css").SetVersion("1.0");
            //manifest.DefineStyle("applydetail_default").SetUrl("Themes/Default/applydetail.css").SetVersion("1.0");
            //manifest.DefineStyle("additional_default").SetUrl("Themes/Default/additional.css").SetVersion("1.0");
        }
    }
}