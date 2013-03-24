using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class FaviconModel : AgileEAPModel
    {
        public bool Uploaded { get; set; }
        public string FaviconUrl { get; set; }
    }
}