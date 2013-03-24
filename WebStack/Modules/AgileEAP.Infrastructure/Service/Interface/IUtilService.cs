using System;
namespace AgileEAP.Infrastructure.Service
{
    public interface IUtilService
    {
        System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.DictItem> GetDictItems(string dictName);
        string GetDictItemText(string dictItemValue, string defaultValue = "AgileEAP");
        void SaveDict(AgileEAP.Infrastructure.Domain.Dict domain);
    }
}
