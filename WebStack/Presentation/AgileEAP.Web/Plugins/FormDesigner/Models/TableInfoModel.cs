using AgileEAP.MVC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.FormDesigner.Models
{
    public class TableInfoModel
    {
        #region Properties

        /// <summary>
        /// 名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public IList<string> Columns { get; set; }
        #endregion
    }
}
