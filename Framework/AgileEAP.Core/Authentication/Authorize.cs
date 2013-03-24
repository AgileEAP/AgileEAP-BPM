using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Utility;
using AgileEAP.Core.ExceptionHandler;
using AgileEAP.Core.Security;
using AgileEAP.Core.Data;

namespace AgileEAP.Core.Authentication
{
    public class Authorize
    {
        public static string NewToken(string loginName, string safePwd, DateTime? expireTime = null)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                throw new EAPException("login is null or empty!");
            }

            return System.Web.HttpUtility.UrlEncode(CryptographyManager.DesEncode(string.Format("{0};{1};{2}", loginName, safePwd, expireTime ?? DateTime.Now.AddHours(72))));
        }

        internal static IUser GetUser(string loginName, string safePwd)
        {
            try
            {
                DataTable dt = new DataContext().ExecuteDataTable(UnitOfWork.GetEAConnection(), "select top 1 * from AC_Operator ",
                                    ParameterBuilder.BuildParameters()
                                    .SafeAdd("LoginName", loginName)
                                    .SafeAdd("Password", safePwd));
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new User()
                    {
                        ID = dr["ID"].ToSafeString(),
                        Name = dr["UserName"].ToSafeString(),
                        UserType = (short)dr["UserType"],
                        LoginName = dr["LoginName"].ToSafeString(),
                        Theme = dr["Skin"].ToSafeString() ?? "Default",
                        OrgID = dr["OwnerOrg"].ToSafeString()
                    };
                }
            }
            catch (Exception ex)
            {
                GlobalLogger.Error<Authorize>(string.Format("获取用户loginName={0}出错", loginName), ex);
                throw;
            }

            return null;
        }
    }
}
