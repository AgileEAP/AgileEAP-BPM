#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core
{
    public interface IPageOfList<T> : IEnumerable<T>
    {
        PageInfo PageInfo { get; set; }
    }

    public class PageOfList<T> : IPageOfList<T>
    {
        public PageOfList()
        {
            ItemList = new List<T>();
        }
        public PageOfList(IEnumerable<T> items, PageInfo pageInfo)
        {
            ItemList = new List<T>();
            ItemList.AddRange(items);
            PageInfo = pageInfo;
        }

        public List<T> ItemList
        { get; set; }

        #region IPageOfList<T> Members

        public PageInfo PageInfo { get; set; }
        #endregion

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return ItemList.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ItemList.GetEnumerator();
        }

        #endregion
    }

    public class PageInfo
    {
        public PageInfo()
        {
            PageIndex = 1;
            PageSize = 15;
            ItemCount = 0;
        }

        public PageInfo(int pageIndex, int pageSize, long itemCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            ItemCount = itemCount;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 0;

                return (int)Math.Ceiling(ItemCount / (double)PageSize);
            }
        }
        public long ItemCount { get; set; }
    }

    // <summary>
    /// 分页事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnPagingHandler(object sender, PagingEventArgs e);

    /// <summary>
    /// PagingEventArgs 的摘要说明
    /// </summary>
    public class PagingEventArgs
    {
        public PageInfo PageInfo { get; set; }

        public PagingEventArgs()
        {
        }
        public PagingEventArgs(PageInfo pageInfo)
        {
            PageInfo = pageInfo;
        }
    }
}
