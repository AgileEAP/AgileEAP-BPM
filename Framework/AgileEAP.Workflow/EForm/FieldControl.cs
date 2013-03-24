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
using System.Data.Common;
using System.Text;
using System.Xml.Linq;
using System.Web;

using AgileEAP.Core.Extensions;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.EForm
{
    public abstract class FieldControl : XmlElement
    {
        #region Properties

        /// <summary>
        /// 控件类型
        /// </summary>
        public ControlType ControlType
        {
            get
            {
                return Attibutes.GetSafeValue<ControlType>("type");
            }
            set
            {
                Attibutes.SafeAdd("type", value);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Value
        {
            get
            {
                return Attibutes.GetSafeValue<string>("value");
            }
            set
            {
                Attibutes.SafeAdd("value", value);
            }
        }

        /// <summary>
        /// CSS样式
        /// </summary>
        public string CssClass
        {
            get
            {
                return Attibutes.GetSafeValue<string>("cssClass");
            }
            set
            {
                Attibutes.SafeAdd("cssClass", value);
            }
        }

        /// <summary>
        /// 查询控件
        /// </summary>
        public FormField Field
        {
            get;
            set;
        }

        public string ParamChar
        {
            get
            {
                //if (Parent != null && Parent.Parent is FormView)
                //{
                //    short databaseType = (Parent.Parent as QueryDefine).DBSource.DBType;

                //    if (databaseType == (short)DatabaseType.Oracle)
                //        return ":";
                //}

                return "@";
            }
        }

        /// <summary>
        /// 数字验证代码
        /// </summary>
        public string NumberValidateCode
        {
            get
            {
                if (Field.DataType == DataType.Integer || Field.DataType == DataType.Float)
                    return " onkeypress=\"return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46\" onpaste=\"return !clipboardData.getData('text').match(/\\D/)\" ondragenter=\"return false\" style=\"ime-mode:Disabled\" ";

                return string.Empty;
            }
        }
        #endregion

        #region Construtor
        /// <summary>
        /// 构造函数
        /// </summary>
        public FieldControl()
        {
            ElementName = "control";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xml">字符串参数</param>
        /// <param name="isXmlFile">是否是xml文件</param>
        public FieldControl(IXmlElement parent, XElement xElem)
            : base(parent, "control", xElem)
        {
            Field = parent as FormField;
        }
        #endregion

        #region Methods

        /// <summary>
        /// 返回查询条件
        /// </summary>
        /// <param name="filterValues"></param>
        public virtual KeyValuePair<string, IDictionary<string, object>> GetFilterCondition(IDictionary<string, object> filterValues)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            if (HasValue(filterValues))
            {
                KeyValuePair<string, object> parameter = NewParameter(Field.Name, filterValues);
                parameters.SafeAdd(parameter.Key, parameter.Value);
            }

            return new KeyValuePair<string, IDictionary<string, object>>(string.Format(" And {0}={1}{0} ", Field.Name, ParamChar), parameters);
        }

        /// <summary>
        /// 转换为Html
        /// </summary>
        /// <returns></returns>
        public abstract string ToHtml(IDictionary<string, object> values);

        /// <summary>
        /// 新建sql参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="filterValues">参数值字典</param>
        /// <returns></returns>
        public KeyValuePair<string, object> NewParameter(string paramName, IDictionary<string, object> filterValues)
        {
            string filterValue = GetFilterValue<string>(filterValues, paramName);
            object value = null;

            if (Field.DataType == DataType.DateTime)
            {
                value = filterValue.ToDateTime();
            }
            else if (Field.DataType == DataType.String)
            {
                value = filterValue.ToSafeString();
            }
            else if (Field.DataType == DataType.Integer)
            {
                value = filterValue.Replace("-", "").Replace("/", "").ToInt();
            }
            else if (Field.DataType == DataType.Float)
            {
                value = filterValue.ToDouble();
            }

            return new KeyValuePair<string, object>(paramName.StartsWith(ParamChar) ? paramName : ParamChar + paramName, value);
        }

        /// <summary>
        /// 新建sql参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="filterValues">参数值字典</param>
        /// <returns></returns>
        public KeyValuePair<string, object> NewCustomParameter(string paramName, IDictionary<string, object> filterValues)
        {
            string filterValue = GetFilterValue<string>(filterValues, paramName);
            object value = null;

            if (Field.DataType == DataType.DateTime)
            {
                value = filterValue.ToDateTime();
            }
            else if (Field.DataType == DataType.String)
            {
                string strValue = filterValue;
                if (strValue.Contains("年"))
                {
                    strValue = strValue.Replace("年", "").Replace("月", "");
                    if (strValue.Length == 5)
                    {
                        strValue = strValue.Substring(0, 4) + "0" + strValue.Substring(4, 1);
                    }
                    value = strValue.ToInt();
                }
            }
            else if (Field.DataType == DataType.Integer)
            {
                string strValue = filterValue.Trim();

                if (strValue.Contains("年"))
                {
                    strValue = strValue.Replace("年", "").Replace("月", "");
                    if (strValue.Length == 5)
                    {
                        strValue = strValue.Substring(0, 4) + "0" + strValue.Substring(4, 1);
                    }
                    value = strValue.ToInt();
                }
                else
                {

                    value = strValue.ToInt();
                }
            }
            else if (Field.DataType == DataType.Float)
            {
                string strValue = filterValue.Trim();

                if (strValue.Contains("年"))
                {
                    strValue = strValue.Replace("年", "").Replace("月", "");
                    if (strValue.Length == 5)
                    {
                        strValue = strValue.Substring(0, 4) + "0" + strValue.Substring(4, 1);
                    }
                    value = strValue.ToInt();
                }
                else
                {
                    value = filterValue.ToDouble();
                }
            }

            return new KeyValuePair<string, object>(paramName.StartsWith(ParamChar) ? paramName : ParamChar + paramName, value);
        }

        /// <summary>
        /// 获取网站的根地址
        /// </summary>
        /// <returns></returns>
        public string GetRootPath()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            appPath = (appPath.Length > 1 && !appPath.EndsWith("/")) ? appPath + "/" : "/";

            return string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, appPath);
        }

        protected bool HasValue(IDictionary<string, object> filterValues)
        {
            return HasValue(filterValues, Field.Name);
        }

        protected bool HasValue(IDictionary<string, object> filterValues, string paramName)
        {
            string safeKey = HttpUtility.UrlEncode(paramName);

            return filterValues.ContainsKey(safeKey) && !string.IsNullOrEmpty(filterValues[safeKey].ToSafeString());
        }

        protected TValue GetFilterValue<TValue>(IDictionary<string, object> filterValues, string paramName)
        {
            paramName = HttpUtility.UrlEncode(paramName);
            object value = null;
            if (filterValues.TryGetValue(paramName, out value))
            {
                return value.Cast<TValue>(default(TValue));
            }

            if (typeof(TValue) == typeof(string)) return string.Empty.Cast<TValue>();

            return default(TValue);
        }

        public object GetFieldValue(IDictionary<string, object> values)
        {
            if (values == null || !values.ContainsKey(Field.Name)) return Field.DefaultValue;

            return values[Field.Name];
        }

        #endregion
    }
}
