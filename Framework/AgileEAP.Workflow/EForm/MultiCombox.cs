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
    public class MultiCombox : DataSourceControl
    {
        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiCombox()
        { ControlType = ControlType; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">表单字段对象</param>
        public MultiCombox(FormField field)
        {
            ControlType = ControlType.MultiCombox;
            Field = field;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public MultiCombox(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.MultiCombox; }

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
            return AgileEAP.Core.Infrastructure.EngineContext.Current.Resolve<IRepository<string>>().All<DictItem>().Where(d => d.DictID == DictID).ToList();
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
            StringBuilder html = new StringBuilder();
            html.Append("<Div id=\"multipleComboxContainer\">");
            if ((DataSourceType == DataSourceType.Dict && GetDictData() != null) || DataContext != null)
            {   //没有数据不创建激发事件
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" tag=\"combox\" onclick=\"comboxExpand('combox{0}');\" class=\"report_textbox_combox\" isRequired=\"{1}\" value='全部' />",
                    Field.Name, Field.Required);
            }
            else
            {
                html.AppendFormat("<input name=\"{0}\" type=\"text\" id=\"{0}\" class=\"report_textbox_combox\" isRequired=\"{1}\" value='全部' />",
                      Field.Name, Field.Required);
            }
            html.AppendFormat("<input name=\"{0}data\" type=\"hidden\" id=\"{0}data\" isRequired=\"{1}\" value='-1' />", Field.Name, Field.Required);

            html.Append("</Div>");


            string height = string.Empty;

            if (DataSourceType == DataSourceType.Dict && GetDictData().Count > 10)
            {
                height = "height:150px;";
            }
            else if (DataContext != null && GetDataSource() != null && GetDataSource().Rows.Count > 10)
            {
                height = "height:150px;";
            }

            html.AppendFormat("<div id='combox{0}' class=\"cbList\" style=\"{1}\">", Field.Name, height);

            html.Append("<div style=\"height:20px;\">");
            html.AppendFormat("<a href=\"javascript:void(0)\" onclick=\"comboxSelectAll('{0}');\"/>全选</a>&nbsp;<a href=\"javascript:void(0)\"  onclick=\"comboxDeSelect('{0}');\"/>反选</a>&nbsp;<a href=\"javascript:void(0)\" onclick=\"comboxGetSelected('{0}');\"/>确定</a></div>", Field.Name);

            html.Append("<div id=\"cbItems\">");
            html.Append("<ul>");

            if (DataSourceType == DataSourceType.Dict)
            {
                foreach (var dict in GetDictData())
                    html.AppendFormat("<li><a href=\"javascript:void(0)\" onclick='comboxChooseItem(this)'><input type=\"checkbox\" {2} value=\"{0}\" style=\"height:12px; border:0px solid red;\" checked />{1}</a></li>", dict.Value, dict.Text, dict.Value.Equals(Value, StringComparison.OrdinalIgnoreCase) ? "selected" : string.Empty);

            }
            else if (DataContext != null)
            {
                DataTable dt = GetDataSource();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                        html.AppendFormat("<li><a href=\"javascript:void(0)\" onclick='comboxChooseItem(this)'><input type=\"checkbox\" {2} value=\"{0}\" style=\"height:12px; border:0px solid red;\" checked />{1}</a></li>", dr[ValueField], dr[TextField], dr[ValueField].ToSafeString().Equals(Value, StringComparison.OrdinalIgnoreCase) ? "selected" : string.Empty);

                }
            }
            html.Append("</ul>");
            html.Append("</div>");
            html.Append("</div>");

            return html.ToString();
        }

        #endregion
    }
}
