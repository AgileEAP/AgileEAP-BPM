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
using System.Xml.Linq;
using System.Data.SqlClient;

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.EForm
{
    public class MonthPicker : FieldControl
    {
        #region Properties
        /// <summary>
        /// 日期格式
        /// </summary>
        public string Format
        {
            get
            {
                return Attibutes.GetSafeValue<string>("format");
            }
            set
            {
                Attibutes.SafeAdd("format", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return Attibutes.GetSafeValue<string>("defaultValue");
            }
            set
            {
                Attibutes.SafeAdd("defaultValue", value);
            }
        }
        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthPicker()
        { ControlType = ControlType.MonthPicker; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public MonthPicker(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.MonthPicker; }

        #endregion

        #region Methods

        /// <summary>
        /// 返回查询条件
        /// </summary>
        /// <param name="filterValues"></param>
        public override KeyValuePair<string, IDictionary<string, object>> GetFilterCondition(IDictionary<string, object> filterValues)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            if (HasValue(filterValues))
            {
                KeyValuePair<string, object> parameter = NewCustomParameter(Field.Name, filterValues);
                parameters.SafeAdd(parameter.Key, parameter.Value);
            }

            return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0}={1}{0} ", Field.Name, ParamChar), parameters);
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public override string ToHtml(IDictionary<string, object> values)
        {
            return string.Format("<input name=\"{0}\" type=\"text\" id=\"{0}\" class=\"report_monthpicker\" onfocus=\"WdatePicker({{dateFmt:'yyyy年M月',minDate:'{1}-1',maxDate:'{2}-{3}'}})\" value=\"{4}\" isRequired=\"{5}\" />", Field.Name, DateTime.Now.Year - 1, DateTime.Now.Year, DateTime.Now.Month, string.Format("{0}年{1}月", DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month), Field.Required);
        }
        #endregion
    }
}
