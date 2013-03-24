#region Description
/*==============================================================================
 *  Copyright (c) suntektech co.,ltd. All Rights Reserved.
 * ===============================================================================
 * 描述：报表模型字段配置类
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
using System.Data;
using System.Text;
using System.Xml.Linq;
using System.Data.SqlClient;

using AgileEAP.Core.Data;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.EForm
{
    public class ChooseTree : DataSourceControl
    {
        #region Properties

        /// <summary>
        /// 根结点
        /// </summary>
        public string InitRoot
        {
            get
            {
                return Attibutes.GetSafeValue<string>("initRoot");
            }
            set
            {
                Attibutes.SafeAdd("initRoot", value);
            }
        }

        /// <summary>
        /// 子结点字段
        /// </summary>
        public string ChildField
        {
            get
            {
                return Attibutes.GetSafeValue<string>("childField", "ID");
            }
            set
            {
                Attibutes.SafeAdd("childField", value);
            }
        }

        /// <summary>
        ///父结点字段
        /// </summary>
        public string ParentField
        {
            get
            {
                return Attibutes.GetSafeValue<string>("parentField", "ParentID");
            }
            set
            {
                Attibutes.SafeAdd("parentField", value);
            }
        }

        /// <summary>
        ///数据来源的表名
        /// </summary>
        public string TableName
        {
            get
            {
                return Attibutes.GetSafeValue<string>("tableName", "TableName");
            }
            set
            {
                Attibutes.SafeAdd("tableName", value);
            }
        }

        /// <summary>
        ///集合子级显示
        /// </summary>
        public string ParentFieldText
        {
            get
            {
                return Attibutes.GetSafeValue<string>("parentFieldText", "ParentFieldText");
            }
            set
            {
                Attibutes.SafeAdd("parentFieldText", value);
            }
        }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url
        {
            get
            {
                return Attibutes.GetSafeValue<string>("url");
            }
            set
            {
                Attibutes.SafeAdd("url", value);
            }
        }

        /// <summary>
        /// 隐藏值
        /// </summary>
        public string HiddenValue
        {
            get;
            set;
        }

        private string dialogTitle = string.Empty;
        /// <summary>
        /// 选择框标题
        /// </summary>
        public string DialogTitle
        {
            get
            {
                if (string.IsNullOrEmpty(dialogTitle)) dialogTitle = "选择" + Field.Text;

                return dialogTitle;
            }
            set
            {
                dialogTitle = value;
            }
        }

        private int dialogWidth = 500;
        /// <summary>
        /// 选择框宽度
        /// </summary>
        public int DialogWidth
        {
            get
            {
                return dialogWidth;
            }
            set
            {
                dialogWidth = value;
            }
        }

        private int dialogHeight = 500;
        /// <summary>
        /// 选择框宽度
        /// </summary>
        public int DialogHeight
        {
            get
            {
                return dialogHeight;
            }
            set
            {
                dialogHeight = value;
            }
        }

        /// <summary>
        /// 是否默认生成全部根结点
        /// </summary>
        public bool IsGenerateRoot
        {
            get
            {
                return Attibutes.GetSafeValue<bool>("isGenerateRoot", false);
            }
            set
            {
                Attibutes.SafeAdd("isGenerateRoot", value);
            }
        }

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChooseTree()
        { ControlType = ControlType.ChooseTree; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ChooseTree(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.ChooseTree; }

        #endregion

        #region Methods
        /// <summary>
        /// 返回查询条件
        /// </summary>
        /// <param name="filterValues"></param>
        /// <returns></returns>
        public override KeyValuePair<string, IDictionary<string, object>> GetFilterCondition(IDictionary<string, object> filterValues)
        {
            string paramValue = GetFilterValue<string>(filterValues, Field.Name + "data").Trim(',');

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(paramValue))
                return default(KeyValuePair<string, IDictionary<string, object>>);

            parameters.Add(new KeyValuePair<string, object>(ParamChar + Field.Name.ToLower(), null));

            if (Field.DataType == DataType.String && !string.IsNullOrEmpty(paramValue))
            {
                paramValue = string.Join(",", paramValue.Split(',').Select(o => string.Format("'{0}'", o[o.Length - 2] == '_' ? o.Substring(0, o.Length - 2) : o)).ToArray());
            }
            else
            {
                paramValue = string.Join(",", paramValue.Split(',').Select(o => string.Format("{0}", o[o.Length - 2] == '_' ? o.Substring(0, o.Length - 2) : o)).ToArray());
            }

            return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0} in ({1}) ", Field.Name, paramValue), parameters);
        }


        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public override string ToHtml(IDictionary<string, object> values)
        {
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<input name=\"{1}\" type=\"text\" id=\"{1}\" onclick=\"openChooseTreeDialog('{7}','{0}','{1}','{2}','{3}',{4},{5})\" class=\"report_choosetree\" isRequired=\"{6}\" value='全部' />", Field.DataSource, Field.Name, Url, DialogTitle, DialogWidth, DialogHeight, Field.Required, Field.DataSource);
            html.AppendLine();
            html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" id=\"{0}data\" isRequired=\"{1}\" value=\"{2}\" />", Field.Name, Field.Required, GetFieldValue(values));
            return html.ToString();
        }
        #endregion
    }
}
