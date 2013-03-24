using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using System.Text;

using AgileEAP.Core.Data;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;
using AgileEAP.Infrastructure.Service;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.EForm
{
    public class SingleCombox : DataSourceControl
    {
        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleCombox()
        { ControlType = ControlType.SingleCombox; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">表单字段对象</param>
        public SingleCombox(FormField field)
        {
            ControlType = ControlType.SingleCombox;
            Field = field;
            DictID = field.DataSource;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public SingleCombox(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.SingleCombox; }

        /// <summary>
        /// 字典类型ID
        /// </summary>
        public string DictID
        {
            get
            {
                return Attibutes.GetSafeValue<string>("dictID");
            }
            set
            {
                Attibutes.SafeAdd("dictID", value);
            }
        }
        #endregion

        #region Methods

        public IList<DictItem> GetDictData()
        {
            return repository.All<DictItem>().Where(d => d.DictID == DictID).ToList();
        }

        /// <summary>
        /// 返回查询条件
        /// </summary>
        /// <param name="filterValues"></param>
        /// <returns></returns>
        public override KeyValuePair<string, IDictionary<string, object>> GetFilterCondition(IDictionary<string, object> filterValues)
        {
            string paramValue = GetFilterValue<string>(filterValues, Field.Name + "data").Trim(',');

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(paramValue) || paramValue == "-1")
                return default(KeyValuePair<string, IDictionary<string, object>>);

            parameters.Add(new KeyValuePair<string, object>(ParamChar + Field.Name, null));

            return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0} in ({1}) ", Field.Name, Field.DataType == DataType.String ? string.Join(",", paramValue.Split(',').Select(p => string.Format("'{0}'", p)).ToArray()) : paramValue), parameters);
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
            html.Append("<div id=\"singleComboxContainer\">");
            IList<DictItem> dataSource = GetDictData();
            if (dataSource != null && dataSource.Count > 0)
            {   //没有数据不创建激发事件
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" onclick=\"comboxExpand('combox{0}');\" class=\"eForm_textbox_combox\" isRequired=\"{1}\" value='{2}' />",
                    Field.Name, Field.Required, value);
            }
            else
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\"  tag=\"combox\" class=\"eForm_textbox_combox\" isRequired=\"{1}\" value='{2}' />",
                      Field.Name, Field.Required, value);
            }
            html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" id=\"{0}data\" tag=\"combox\" isRequired=\"{1}\" value='{2}' />", Field.Name, Field.Required, dataValue);

            string height = string.Empty;
            if (dataSource != null && dataSource.Count > 10)
            {
                height = "height:150px;";
            }

            html.AppendFormat("<div id='combox{0}' class=\"cbList\" style=\"{1}\">", Field.Name, height);

            html.Append("<div id=\"cbItems\">");

            html.AppendLine("<ul>");

            if (dataSource != null)
            {
                foreach (var dict in dataSource)
                    html.AppendFormat("<li><a href=\"javascript:void(0)\" data='{0}' class=\"single_combox_item_a\" onclick=\"comboxChooseItem(this,'{2}')\" >{1}</a></li>", dict.Value, dict.Text, Field.Name);
            }

            html.AppendLine("</ul>");
            html.AppendLine("</div>");

            html.AppendLine("</div>");
            html.AppendLine("</div>");

            return html.ToSafeString();
        }
        #endregion
    }
}
