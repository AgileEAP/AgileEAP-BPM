using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Extensions;
using AgileEAP.MVC;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;

namespace AgileEAP.MVC
{
    public class GridFilter
    {
        public string SortCommand { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public static class KendoExtensions
    {
        public static GridFilter GetFilter(this DataSourceRequest command)
        {
            if (command.PageSize == 0)
            {
                command.PageSize = Configure.Get<int>("PageSize", 15);
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            string sortcommand = null;
            if (command.Sorts != null && command.Sorts.Count > 0)
            {
                string sortDirection = ListSortDirection.Ascending == command.Sorts[0].SortDirection ? "asc" : "desc";
                sortcommand = string.Format("order by {0} {1}", command.Sorts[0].Member, sortDirection);
            }

            if (command.Filters != null)
            {
                foreach (IFilterDescriptor filterDescriptor in command.Filters)
                {
                    parameters.SafeAdd(ApplyFilter(filterDescriptor));
                }
            }

            return new GridFilter()
            {

                PageInfo = new PageInfo() { PageIndex = command.Page, PageSize = command.PageSize },
                Parameters = parameters,
                SortCommand = sortcommand
            };
        }

        public static IDictionary<string, object> ApplyFilter(IFilterDescriptor filter)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            string filerMessage = string.Empty;
            List<string> convertValue = new List<string>();
            string filerOperators = string.Empty;
            if (filter is CompositeFilterDescriptor)
            {
                foreach (IFilterDescriptor childFilter in ((CompositeFilterDescriptor)filter).FilterDescriptors)
                {
                    parameters.SafeAdd(ApplyFilter(childFilter));
                }
            }
            else
            {
                FilterDescriptor filterDescriptor = (FilterDescriptor)filter;
                filerMessage = filterDescriptor.Member;
                convertValue.Add(filterDescriptor.Value.ToSafeString());
                switch (filterDescriptor.Operator)
                {
                    case FilterOperator.IsEqualTo:
                        parameters.SafeAdd(filerMessage, filterDescriptor.Value);
                        break;
                    case FilterOperator.IsNotEqualTo:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0}<>'{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.StartsWith:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0} like '{1}%'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.Contains:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0} like '%{1}%'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.EndsWith:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0} like '%{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.IsLessThanOrEqualTo:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0} <='{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.IsLessThan:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0}<'{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.IsGreaterThanOrEqualTo:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0}>='{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                    case FilterOperator.IsGreaterThan:
                        parameters.SafeAdd(IdGenerator.NewComb().ToSafeString(), new Condition(string.Format("{0}>'{1}'", filerMessage, filterDescriptor.Value)));
                        break;
                }
            }

            return parameters;
        }
    }
}
