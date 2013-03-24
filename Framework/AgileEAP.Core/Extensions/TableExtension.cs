using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Utility;
namespace AgileEAP.Core.Extensions
{
    public static class TableExtension
    {
        /// <summary>
        /// 把Table转换为实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TEntity> ToList<TEntity>(this DataTable dt)
        {
            return Data.DataUtil.ToList<TEntity>(dt);
        }
    }
}
