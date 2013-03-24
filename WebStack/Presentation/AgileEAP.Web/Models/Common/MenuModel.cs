using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class MenuModel : AgileEAPModel
    {
        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
    }
}