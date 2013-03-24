using System.Collections.Generic;
using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Common
{
    public class CurrencySelectorModel : AgileEAPModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public CurrencyModel CurrentCurrency { get; set; }
    }
}