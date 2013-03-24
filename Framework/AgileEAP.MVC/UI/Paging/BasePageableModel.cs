//Contributor : MVCContrib

using System;
using System.Collections;
using System.Collections.Generic;
using AgileEAP.Core;

namespace AgileEAP.MVC.UI.Paging
{
    public abstract class BasePageableModel : IPageableModel
    {
        #region Methods

        public IEnumerator GetEnumerator()
        {
            return new List<string>().GetEnumerator();
        }

        public virtual void LoadPagedList<T>(IPageOfList<T> pagedList)
        {
            FirstItem = (pagedList.PageInfo.PageIndex * pagedList.PageInfo.PageSize) + 1;
            LastItem = Math.Min((int)pagedList.PageInfo.ItemCount, ((pagedList.PageInfo.PageIndex * pagedList.PageInfo.PageSize) + pagedList.PageInfo.PageSize));
            PageNumber = pagedList.PageInfo.PageIndex + 1;
            PageSize = pagedList.PageInfo.PageSize;
            TotalItems = (int)pagedList.PageInfo.ItemCount;
            TotalPages = pagedList.PageInfo.PageCount;
        }

        #endregion

        #region Properties

        public int FirstItem { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int LastItem { get; set; }

        public int PageIndex
        {
            get
            {
                if (PageNumber > 0)
                    return PageNumber - 1;
                else
                    return 0;
            }
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        #endregion
    }
}