using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using AgileEAP.Core;
using AgileEAP.Core.Caching;

namespace AgileEAP.Core.Extensions
{
    public static class EnumExtension
    {
        private static ILogger logger = LogManager.GetLogger(typeof(EnumExtension));

        /// <summary>
        /// 获取枚举注释值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetRemark(this Enum value)
        {
            string cacheKey = string.Format("{0}_remark_{1}", value.GetType().FullName, value.ToSafeString());
            string remark = CacheManager.GetData<string>(cacheKey);

            if (string.IsNullOrEmpty(remark))
            {
                Type type = value.GetType();
                FieldInfo fieldInfo = type.GetField(value.ToString());

                try
                {
                    object[] attrs = fieldInfo.GetCustomAttributes(typeof(RemarkAttribute), false);
                    foreach (RemarkAttribute attr in attrs)
                    {
                        remark = attr.Remark;
                    }

                    if (!string.IsNullOrEmpty(remark))
                        CacheManager.Add(cacheKey, remark);
                }
                catch (Exception ex) { logger.Error(ex); }
            }

            return remark;
        }

        /// <summary>
        /// 获取枚举注释值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRemarks(this Enum value)
        {
            if (value == null) return null;
            Type type = value.GetType();
            string cacheKey = string.Format("all_{0}_remark", type.FullName);
            return CacheManager.Get<Dictionary<string, string>>(cacheKey, () =>
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();

                    foreach (var field in type.GetFields())
                    {
                        if (field.FieldType.IsEnum)
                        {
                            object tmp = field.GetValue(value);
                            Enum enumValue = (Enum)tmp;
                            int intValue = (int)tmp;
                            result.Add(intValue.ToSafeString(), enumValue.GetRemark());
                        }
                    }

                    return result;
                });
        }
    }
}
