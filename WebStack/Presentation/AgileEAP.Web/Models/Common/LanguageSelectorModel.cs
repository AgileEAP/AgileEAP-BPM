using System.Collections.Generic;
using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class LanguageSelectorModel : AgileEAPModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }

        public bool UseImages { get; set; }
    }
}