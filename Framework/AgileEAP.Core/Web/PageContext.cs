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

namespace AgileEAP.Core.Web
{
    /// <summary>
    /// 页面上下文对象
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class PageContext<TId>
    {
        private IDictionary<string, object> innerValues = new Dictionary<string, object>();

        public PageContext()
        { }

        /// <summary>
        /// 获取参数的值
        /// </summary>
        /// <param name="key">参数的key</param>
        /// <param name="value">参数的值</param>
        /// <returns>是否获取到正确的值</returns>
        public bool TryGetValue(string key, out object value)
        {
            return innerValues.TryGetValue(key, out value);
        }

        /// <summary>
        /// 设置参数的值
        /// </summary>
        /// <param name="key">参数的key</param>
        /// <param name="value">参数的值</param>
        public void SetValue(string key, object value)
        {
            if (value == null) return;

            if (innerValues.ContainsKey(key))
                innerValues[key] = value;
            else
                innerValues.Add(key, value);
        }

        /// <summary>
        /// 获取参数的值
        /// </summary>
        /// <param name="key">参数的key</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            object value = null;

            TryGetValue(key, out value);
            return value;
        }


        private short runat = 1;
        /// <summary>
        /// 页面运行方式，0表示弹出，1表示转身，2表示ajax操作。
        /// </summary>
        public short Runat
        {
            get
            {
                return runat;
            }
            set
            {
                runat = value;
            }
        }

        /// <summary>
        /// 当前操作
        /// </summary>
        public ActionType Action
        {
            get
            {
                object value;
                return TryGetValue("Action", out value) ? (ActionType)value : ActionType.Update;
            }
            set
            {
                SetValue("Action", value);
            }
        }

        /// <summary>
        /// 上次浏览的url页面
        /// </summary>
        public string LastUrl
        {
            get
            {
                object value;
                return TryGetValue("LastUrl", out value) ? value.ToString() : string.Empty;
            }
            set
            {
                SetValue("LastUrl", value);
            }
        }

        /// <summary>
        /// 当前选择的记录项主键
        /// </summary>
        public TId CurrentId
        {
            get
            {
                object value;
                return TryGetValue("CurrentId", out value) ? (TId)value : default(TId);
            }
            set
            {
                SetValue("CurrentId", value);
            }
        }

        private int pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }


        /// <summary>
        /// 页面大小(每页显示多少条记录)
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long ItemCount
        {
            get;
            set;
        }

        /// <summary>
        /// 查询参数字典
        /// </summary>
        public IDictionary<string, object> FilterParameters
        {
            get
            {
                object value;
                return TryGetValue("FilterParameters", out value) ? value as IDictionary<string, object> : null;
            }
            set
            {
                SetValue("FilterParameters", value);
            }
        }
    }
}
