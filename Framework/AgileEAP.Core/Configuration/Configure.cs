using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Core.Caching;

namespace AgileEAP.Core
{
    public class Configure
    {
        /// <summary>
        /// 获取配置参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string Get(string paramName)
        {
            return CacheManager.Get<string>(paramName, () =>
                {
                    try
                    {
                        return new DataContext().ExecuteScalar(UnitOfWork.GetEAConnection(), "select Value from AB_SysParam ",
                            ParameterBuilder.BuildParameters().SafeAdd("Name", paramName)).ToSafeString();
                    }
                    catch (Exception ex)
                    {
                        GlobalLogger.Error<Configure>(string.Format("获取系统参数{0}出错", paramName), ex);
                        throw;
                    }
                });
        }


        /// <summary>
        /// 获取配置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static T Get<T>(string paramName)
        {
            return Get(paramName).Cast<T>();
        }

        /// <summary>
        /// 获取配置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static T Get<T>(string paramName, T defaultValue)
        {
            try
            {
                string paramValue = Get(paramName);
                return string.IsNullOrEmpty(paramValue) ? defaultValue : paramValue.Cast<T>();
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<Configure>(string.Format("获取系统参数{0}出错", paramName), ex);

                return defaultValue;
            }
        }

        /// <summary>
        /// 获取配置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static T DirectGet<T>(string paramName, T defaultValue)
        {
            try
            {
                string paramValue = new DataContext().ExecuteScalar(UnitOfWork.GetEAConnection(), "select Value from AB_SysParam ",
                    ParameterBuilder.BuildParameters().SafeAdd("Name", paramName)).ToSafeString();
                return string.IsNullOrEmpty(paramValue) ? defaultValue : paramValue.Cast<T>();
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<Configure>(string.Format("获取系统参数{0}出错", paramName), ex);
                throw;
            }
        }
    }
}
