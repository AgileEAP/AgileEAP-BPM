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
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;
namespace AgileEAP.EForm
{
    public class CheckBox : FieldControl
    {
        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBox()
        { ControlType = ControlType.CheckBox; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">表单字段</param>
        public CheckBox(FormField field)
        {
            ControlType = ControlType.CheckBox;
            Field = field;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public CheckBox(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.TextBox; }

        #endregion

        #region Methods

        /// <summary>
        /// 返回查询条件
        /// </summary>
        /// <param name="filterValues"></param>
        public override KeyValuePair<string, IDictionary<string, object>> GetFilterCondition(IDictionary<string, object> filterValues)
        {
            string paramValue = GetFilterValue<string>(filterValues, Field.Name + "data").Trim(',').Trim();
            string inputValue = GetFilterValue<string>(filterValues, Field.Name).Trim(',').Trim();

            if (inputValue == "批量值" && !string.IsNullOrEmpty(paramValue))
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(new KeyValuePair<string, object>(ParamChar + Field.Name, paramValue));

                return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0} in (select value from DD_BatchValue where BatchDataID={1}{0}) ", Field.Name, ParamChar), parameters);
            }

            if (Field.DataType == DataType.String)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (HasValue(filterValues))
                {
                    KeyValuePair<string, object> parameter = NewParameter(Field.Name, filterValues);
                    parameters.SafeAdd(parameter.Key, parameter.Value);
                }

                return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0}={1}{0}", Field.Name, ParamChar), parameters);
                //  return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0} like '%'{2}{1}{0}{2}'%' ", Field.Name, ParamChar, ParamChar == "@" ? "+" : "||"), parameters);
            }

            return base.GetFilterCondition(filterValues);
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public override string ToHtml(IDictionary<string, object> values)
        {
            string checkedValue = GetFieldValue(values).ToSafeString().Trim();
            checkedValue = (checkedValue == "是" || checkedValue == "1") ? "checked" : string.Empty;
            return string.Format("<input name=\"{0}\"  type=\"checkbox\" id=\"{0}\" class=\"checkbox\" isRequired=\"{1}\" {2} {3} {4} />", Field.Name, Field.Required, NumberValidateCode, checkedValue, Field.AccessPattern == AccessPattern.ReadOnly ? "disabled" : string.Empty);
        }
        #endregion
    }
}
