using System.Collections.Generic;
using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class StoreThemeSelectorModel : AgileEAPModel
    {
        public StoreThemeSelectorModel()
        {
            AvailableStoreThemes = new List<StoreThemeModel>();
        }

        public IList<StoreThemeModel> AvailableStoreThemes { get; set; }

        public StoreThemeModel CurrentStoreTheme { get; set; }
    }
}