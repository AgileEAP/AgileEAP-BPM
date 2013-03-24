#region Description
/*==============================================================================
 *  Copyright (c) suntektech co.,ltd. All Rights Reserved.
 * ===============================================================================
 * 描述：报表模型数据源控件类
 * 作者：trh
 * 创建时间：2010-06-10
 * ===============================================================================
 * 历史记录：
 * 描述：
 * 作者：
 * 修改时间：
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Configuration;

using AgileEAP.Core.Data;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;

namespace AgileEAP.EForm
{
    /// <summary>
    /// 报表模型数据源控件类
    /// </summary>
    public abstract class DataSourceControl : FieldControl
    {
        protected IRepository<string> repository = new Repository<string>();
        #region Properties

        /// <summary>
        /// 数据源类型
        /// </summary>
        public DataSourceType DataSourceType
        {
            get
            {
                return Attibutes.GetSafeValue<DataSourceType>("dataSourceType");
            }
            set
            {
                Attibutes.SafeAdd("dataSourceType", value);
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public DataContext DataContext
        {
            get;
            set;
        }

        /// <summary>
        /// 绑定值
        /// </summary>
        public string ValueField
        {
            get
            {
                return Attibutes.GetSafeValue<string>("valueField");
            }
            set
            {
                Attibutes.SafeAdd("valueField", value);
            }
        }

        /// <summary>
        /// 绑定文本
        /// </summary>
        public string TextField
        {
            get
            {
                return Attibutes.GetSafeValue<string>("textField");
            }
            set
            {
                Attibutes.SafeAdd("textField", value);
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSourceControl()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public DataSourceControl(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        {
            if (xElem.Attribute("dataSource") != null)
            {
                //Domain.DataSource ds = new DataSourceService().GetDomain(xElem.Attribute("dataSource").Value);

                //if (ds != null)
                //{
                //    DBSource db = new DBSourceService().GetDomain(ds.DBSourceID) ?? new DBSource()
                //    {
                //        DBType = (short)(DatabaseManager.Instance.GetDatabase().NHConfiguration.Properties["connection.driver_class"].Contains("Oracle") ? DatabaseType.Oracle : DatabaseType.Mssql2005)
                //    };

                //    DataSource = new DataSource()
                //    {
                //        CmdText = ds.CmdText,
                //        DatabaseType = db.DBType.Cast<DatabaseType>(DatabaseType.Mssql2005),
                //        ConnectionString = db.ConnectionString ?? DatabaseManager.Instance.GetDatabase().NHConfiguration.Properties["connection.connection_string"],
                //        Name = ds.Name,
                //        Id = ds.ID
                //    };
                //}
            }
        }

        #endregion

        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        public override XElement ToXElement()
        {
            if (DataContext != null)
                Attibutes.SafeAdd("dataSource", Field.DataSource);
            return new XElement(ElementName, Attibutes.Where(o => o.Value != null && !string.IsNullOrEmpty(o.Value.ToSafeString())).Select(o => new XAttribute(o.Key, o.Value)));
        }


        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetDataSource()
        {
            //if (DataContext == null) return new DataTable();
            //try
            //{
            //    string cacheKey = string.Format("DataSource_Cache_Key_{0}", DataContext.Id);
            //    DataTable result = CacheManager.GetData<DataTable>(cacheKey);

            //    if (result == null || result.Rows.Count == 0)
            //    {
            //        result = DataContext.Execute();
            //        CacheManager.Add(cacheKey, result);
            //    }

            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    log.Error(string.Format("ConnectionString={0},sql={1}, error={2}", DataContext.ConnectionString, DataContext.CmdText, ex));
            //}

            return new DataTable();
        }

    }
}
