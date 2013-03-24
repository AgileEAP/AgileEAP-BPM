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
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml.Linq;

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.EForm
{
    /// <summary>
    /// 选取对话框
    /// </summary>
    public class ChooseBox : FieldControl
    {
        #region Properties

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

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChooseBox()
        { ControlType = ControlType.ChooseBox; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">表单字段</param>
        public ChooseBox(FormField field)
        {
            ControlType = ControlType.TextBox;
            Url = field.URL.Trim().ToLower().StartsWith("http") ? field.URL : string.Format("{0}{1}", GetRootPath(), field.URL);
            Url += (Url.IndexOf("?") > 0 ? "&" : "?") + "field=" + field.Name;
            Field = field;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public ChooseBox(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.ChooseBox; }

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

            parameters.Add(new KeyValuePair<string, object>(ParamChar + Field.Name, paramValue));

            return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0} in (select value from DD_BatchValue where BatchDataID=@{0}) ", Field.Name), parameters);
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public override string ToHtml(IDictionary<string, object> values)
        {
            string value = GetFieldValue(values).ToSafeString();
            string dateKey = Field.Name + "data";
            string dataValue = values != null && values.ContainsKey(dateKey) ? values[dateKey].ToSafeString() : Field.DefaultValue;

            StringBuilder html = new StringBuilder();
            if (Field.AccessPattern == AccessPattern.ReadOnly)
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" value=\"{1}\"  class=\"text\" readonly />", Field.Name, value);
                html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" id=\"{0}data\" value=\"{1}\" />", Field.Name, dataValue);
            }
            else
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" value=\"{1}\" onclick=\"openChooseBoxDialog('选择{2}','{3}',{4},{5})\" class=\"eForm_chooseBox\" isRequired=\"{6}\" />", Field.Name, value, Field.Text, Url, dialogWidth, dialogHeight, Field.Required);
                html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" id=\"{0}data\" value=\"{1}\" />", Field.Name, dataValue);
            }

            return html.ToString();
        }
        #endregion
    }
}
