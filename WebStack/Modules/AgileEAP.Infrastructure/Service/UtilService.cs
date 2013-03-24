using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Service;
using AgileEAP.Core.Extensions;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Infrastructure.Service
{
    public class UtilService : IUtilService
    {
        private readonly ILogger logger = LogManager.GetLogger(typeof(AuthorizeService));
        private readonly IRepository<string> repository;

        public UtilService(IRepository<string> repository)
        {
            this.repository = repository;
        }

        public UtilService()
        {
            this.repository = new Repository<string>();
        }


        #region IDictService Imp
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="dictName">字典名</param>
        /// <returns></returns>
        public IList<DictItem> GetDictItems(string dictName)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Name", dictName);
            Dict dict = repository.FindOne<Dict>(parameters);
            if (dict != null)
            {
                string dictID = dict.ID;
                parameters.Clear();
                parameters.Add("DictID", dictID);
                return repository.FindAll<DictItem>(parameters);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取字典项值
        /// </summary>
        /// <param name="dictName">字典项名</param>
        /// <returns></returns>
        public string GetDictItemText(string dictItemValue, string defaultValue = "AgileEAP")
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Value", dictItemValue);
            DictItem item = repository.FindOne<DictItem>(parameters);

            return item != null ? item.Text : defaultValue;
        }

        public void SaveDict(Dict domain)
        {
            using (Transaction trans = UnitOfWork.BeginTransaction(typeof(Dict)))
            {
                repository.SaveOrUpdate(domain);

                foreach (var item in domain.DictItems)
                    repository.SaveOrUpdate(item);

                trans.Commit();
            }
        }
        #endregion
    }
}
