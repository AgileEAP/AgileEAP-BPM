using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class InfoBlockModel : AgileEAPModel
    {
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool RecentlyViewedProductsEnabled { get; set; }
        public bool CompareProductsEnabled { get; set; }
        public bool BlogEnabled { get; set; }
        public bool SitemapEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public bool AllowPrivateMessages { get; set; }
    }
}