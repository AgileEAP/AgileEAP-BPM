using System;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Data;
using System.Collections.Generic;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;

namespace AgileEAP.Core.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase httpContext;
        private readonly TimeSpan expirationTimeSpan;
        private readonly DataContext dataContext;
        private IUser cachedUser;
        private ILogger log = LogManager.GetLogger(typeof(FormsAuthenticationService));
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
            this.expirationTimeSpan = FormsAuthentication.Timeout;
            this.dataContext = new DataContext();
        }

        public virtual void SignIn(IUser user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var userData = string.Format("{0};{1};{2};{3};{4}", user.ID, user.Name, user.UserType, user.Theme, user.OrgID);
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.LoginName,
                now,
                now.AddMinutes(10),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Expires = now.Add(expirationTimeSpan);
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            httpContext.Response.Cookies.Add(cookie);
            if (httpContext != null && httpContext.Request != null && !string.IsNullOrEmpty(httpContext.Request["token"]))
            {
                httpContext.Request.Cookies.Add(cookie);
            }
        }

        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public virtual IUser GetAuthenticatedUser()
        {
            if (cachedUser != null) return cachedUser;

            if (httpContext == null || httpContext.Request == null)
            {
                return null;
            }

            if (httpContext != null && httpContext.Request != null && !string.IsNullOrEmpty(httpContext.Request["token"]))
            {

                string[] token = CryptographyManager.DesDecode(httpContext.Request["token"]).Split(';');
                string loginName = token[0];
                string safePwd = token[1];
                DateTime expiredTime = token[2].Cast<DateTime>(DateTime.Now);

                if (expiredTime < DateTime.Now)
                {
                    return null;
                }

                cachedUser = AgileEAP.Core.Authentication.Authorize.GetUser(loginName, safePwd);

                SignIn(cachedUser, false);
            }
            else if (httpContext.User != null && httpContext.User.Identity is FormsIdentity)
            {
                var formsIdentity = (FormsIdentity)httpContext.User.Identity;
                cachedUser = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            }

            return cachedUser;
        }

        public virtual IUser GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("ticket");
            }

            //var userData = string.Format("{0};{1};{2};{3};{4}", user.ID, user.Name, user.UserType, user.Theme;user.OrgID);
            var userData = ticket.UserData.Split(';');
            return new User()
            {
                LoginName = ticket.Name,
                ID = userData[0],
                Name = userData[1],
                UserType = userData[2].ToShort(2),
                Theme = userData[3],
                OrgID = userData[4]
            };
        }

        public bool Authorize(IUser user, string requestURL)
        {
            if (user == null) return false;

            if (user.UserType == (short)UserType.Administrator) return true;

            try
            {
                string sql = @"select r.URL from AC_Resource r  inner join AC_Privilege p on  p.ResourceID=r.ID where r.URL=$RequestURL and p.ID in
                                (
                                select b.PrivilegeID from OM_ObjectRole a inner join AC_RolePrivilege b on a.RoleID=b.RoleID
											                                   where a.ObjectID=$UserID and 
											                                   not exists(select id from AC_SpecialPrivilege c where  c.OperatorID=$UserID and c.AuthFlag=2 and c.PrivilegeID=b.PrivilegeID)
                                union all 
                                select d.PrivilegeID from AC_SpecialPrivilege d where d.OperatorID=$UserID and d.AuthFlag=1
                                )";

                return dataContext.ExecuteScalar(UnitOfWork.GetEAConnection(), sql, ParameterBuilder.BuildParameters()
                                                                                                                 .SafeAdd("UserID", user.ID)
                                                                                                                 .SafeAdd("RequestURL", requestURL)) != null;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("验证用户{0}权限出错", (user ?? new User() { LoginName = "匿名用户" }).LoginName), ex);
            }

            return false;
        }


        public IEnumerable<ToolbarItem> GetToolBarItems(IUser user, string requestURL, string entry)
        {
            try
            {
                string sql = string.Empty;
                IDictionary<string, object> parameters = ParameterBuilder.BuildParameters()
                                                                         .SafeAdd("RequestURL", requestURL)
                                                                         .SafeAdd("Entry", entry);
                if (user.UserType == (short)UserType.Administrator)
                {
                    sql = @"select o.OperateName as Name,o.CommandName as Command,o.CommandArgument as Argument from AC_Operate o  inner join AC_Privilege p on o.ID=p.OperateID  
                            inner join AC_Resource r  on p.ResourceID=r.ID where r.URL=$RequestURL and ( r.Entry=$Entry or r.Entry is null)";
                }
                else
                {
                    parameters.SafeAdd("UserID", user.ID);
                    sql = @"select o.OperateName as Name,o.CommandName as Command,o.CommandArgument as Argument from AC_Operate o  inner join AC_Privilege p on o.ID=p.OperateID  
            inner join AC_Resource r  on p.ResourceID=r.ID where r.URL=$RequestURL and ( r.Entry=$Entry or r.Entry is null)
            and  p.ID in (select b.PrivilegeID from OM_ObjectRole a inner join AC_RolePrivilege b on a.RoleID=b.RoleID
              where a.ObjectID=$UserID and 
              not exists(select id from AC_SpecialPrivilege c where  c.OperatorID=$UserID and c.AuthFlag=2 and c.PrivilegeID=b.PrivilegeID)
	           union all 
              select d.PrivilegeID from AC_SpecialPrivilege d where d.OperatorID=$UserID and d.AuthFlag=1
              )";
                }

                return dataContext.ExecuteDataTable(UnitOfWork.GetEAConnection(), sql, parameters).ToList<ToolbarItem>();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取用户{0}页面{1}工具栏按钮权限出错", user.ID, requestURL + entry), ex);
            }

            return null;
        }

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            throw new NotImplementedException();
        }
    }
}