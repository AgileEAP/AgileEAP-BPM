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

using AgileEAP.Core.Extensions;
using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.EForm
{
    public class DatePicker : FieldControl
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

        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public DatePicker()
        { ControlType = ControlType.DatePicker; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">表单字段</param>
        public DatePicker(FormField field)
        {
            ControlType = ControlType.DatePicker;

            Field = field;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public DatePicker(IXmlElement parent, XElement xElem)
            : base(parent, xElem)
        { ControlType = ControlType.DatePicker; }


        #endregion

        #region Methods

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public override string ToHtml(IDictionary<string, object> values)
        {
            string date = GetFieldValue(values).ToSafeString().Cast<DateTime>(DateTime.Now).ToString("yyyy-MM-dd");

            return Field.AccessPattern == AccessPattern.ReadOnly ? string.Format("<input name=\"{0}\" type=\"text\" id=\"{0}\" readonly value=\"{1}\" />", Field.Name, date) : string.Format("<input name=\"{0}\" type=\"text\" id=\"{0}\" class=\"Wdate\" onclick='var skin=getSkin();WdatePicker({{skin:skin}})' isRequired=\"{1}\" value=\"{2}\" />", Field.Name, Field.Required, date);
        }
        #endregion
    }
}
