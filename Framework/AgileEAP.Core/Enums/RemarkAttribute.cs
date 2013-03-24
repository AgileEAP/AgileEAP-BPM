using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;

namespace AgileEAP.Core
{
    /// <summary>
    /// 用于注释枚举值
    /// </summary>
    public class RemarkAttribute : Attribute
    {
        private static ILogger logger = LogManager.GetLogger(typeof(RemarkAttribute));
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        private string _remark = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
            }
        }

        public RemarkAttribute(string remark)
        {
            _remark = remark;
        }

        public static string GetEnumRemark(Enum value)
        {
            Type type = value.GetType();
            string cacheKey = string.Format("{0}_Remark_{1}", type.Name, value);
            if (_cache.ContainsKey(cacheKey))
                return _cache[cacheKey];
            else
            {
                FieldInfo fieldInfo = type.GetField(value.ToString());

                string remark = string.Empty;
                try
                {
                    object[] attrs = fieldInfo.GetCustomAttributes(typeof(RemarkAttribute), false);

                    foreach (RemarkAttribute attr in attrs)
                    {
                        remark = attr.Remark;
                    }

                    if (!string.IsNullOrEmpty(remark))
                        _cache.SafeAdd(cacheKey, remark);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }

                return remark;
            }
        }

        public static string GetTypeRemark(Type type)
        {
            string cacheKey = string.Format("{0}_Remark", type.Name);
            if (_cache.ContainsKey(cacheKey))
                return _cache[cacheKey];
            else
            {
                string remark = string.Empty;
                try
                {
                    object[] attrs = type.GetCustomAttributes(typeof(RemarkAttribute), false);

                    foreach (RemarkAttribute attr in attrs)
                    {
                        remark = attr.Remark;
                    }

                    if (!string.IsNullOrEmpty(remark))
                        _cache.Add(cacheKey, remark);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }

                return remark;
            }
        }
    }
}
