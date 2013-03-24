using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Log;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Plugins;

namespace AgileEAP.MVC.Controllers
{
    [Compress]
    public class BaseController : Controller
    {
        protected readonly IRepository<string> repository = null;// new Repository<string>();
        protected readonly IWorkContext workContext;
        protected readonly ILogger log = LogManager.GetLogger(typeof(BaseController));
        //private readonly List<string> IgnoreURLs = new List<string> { "/login", "/authorize/logout" };
        private readonly bool ignoreAuthorize = false;
        public BaseController(IWorkContext workContext, IRepository<string> repository)
        {
            this.workContext = workContext;
            this.repository = repository;
        }

        public BaseController(IRepository<string> repository)
        {
            this.repository = repository;
            this.ignoreAuthorize = true;
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            return User.Identity.IsAuthenticated ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        #region Authorize
        protected virtual AuthResult DoAuthorize(HttpContextBase httpContext)
        {
            if (ignoreAuthorize) return AuthResult.Success;

            if (workContext == null || workContext.User == null) return AuthResult.Timeout;

            if (workContext.User.UserType == (short)UserType.Administrator || workContext.User.UserType == (short)UserType.CorpAdmin) return AuthResult.Success;

            try
            {
                var authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                string requestURL = WebUtil.GetRequestFullUrl();
                bool isSuccess = authenticationService.Authorize(workContext.User, requestURL);
                if (!isSuccess)
                {
                    AddActionLog<ActionLog>(string.Format("用户{0}没有访问{1}的权限", workContext.User.LoginName, requestURL), DoResult.Failed);
                }

                return isSuccess ? AuthResult.Success : AuthResult.NoAccessPrivelege;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("验证用户{0}权限出错", workContext.User.LoginName), ex);
            }

            return AuthResult.NoAccessPrivelege;
        }
        #endregion

        protected string GetPluginName(ControllerContext controllerContext)
        {
            if (controllerContext.RouteData != null && controllerContext.RouteData.DataTokens["Namespaces"] != null)
            {
                string[] namespaces = controllerContext.RouteData.DataTokens["Namespaces"] as string[];
                if (namespaces != null && namespaces.Length > 0 && namespaces[0].StartsWith("AgileEAP.Plugin.")) return namespaces[0].Substring(16, namespaces[0].Length - 28);
            }

            return null;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }

            AuthResult authResult = DoAuthorize(filterContext.HttpContext);
            if (authResult != AuthResult.Success)
            {
                string loginUrl = null;
                string plugin = GetPluginName(this.ControllerContext);
                if (!string.IsNullOrEmpty(plugin))
                {
                    IPluginFinder pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
                    PluginDescriptor pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName(GetPluginName(this.ControllerContext), false);
                    if (pluginDescriptor != null && !string.IsNullOrEmpty(pluginDescriptor.LoginUrl))
                        loginUrl = string.Format("{0}{1}", System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath, pluginDescriptor.LoginUrl);
                }

                loginUrl = loginUrl ?? System.Web.Security.FormsAuthentication.LoginUrl ?? "/login";

                if (authResult == AuthResult.Timeout)
                {
                    var response = filterContext.HttpContext.Response;
                    response.Clear();
                    response.StatusCode = 401;
                    filterContext.Result = new RedirectResult(loginUrl);
                }
                else if (authResult == AuthResult.NoAccessPrivelege)
                {
                    filterContext.Result = new LoginResult(loginUrl, filterContext.HttpContext.Request.Url.ToString());
                }
            }
        }

        #region View
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion

        #region ActionLog
        /// <summary>
        /// 添加用户操作日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="logType">日志类型：登陆，操作，接口等</param>
        /// <param name="actionResult">操作结果</param>
        /// <param name="message">操作信息</param>
        public void AddActionLog<T>(LogType logType, string actionMessage, DoResult actionResult)
        {
            Type type = typeof(T);
            ActionLog actionLog = new ActionLog();
            actionLog.Result = (short)actionResult;
            actionLog.LogType = (short)logType;
            actionLog.ID = IdGenerator.NewComb().ToString();
            if (workContext != null && workContext.User != null)
            {
                actionLog.UserID = workContext.User.ID;
                actionLog.UserName = workContext.User.LoginName;
            }
            actionLog.Message = actionMessage;
            actionLog.CreateTime = DateTime.Now;
            actionLog.AppModule = type.Assembly.FullName.Split(',')[0];
            actionLog.ClientIP = WebUtil.GetClientIP();
            try
            {
                repository.Save(actionLog);
            }
            catch (Exception ex)
            {
                log.Error("添加日志出错", ex);
            }
        }

        /// <summary>
        /// 添加用户操作日志
        /// </summary>
        /// <param name="message">操作信息</param>
        ///  <param name="actionResult">操作结果</param>
        public void AddActionLog<T>(string actionMessage, DoResult actionResult) where T : class, new()
        {
            AddActionLog<T>(LogType.Operate, actionMessage, actionResult);
        }

        /// <summary>
        /// 添加用户操作日志
        /// </summary>
        /// <param name="entity">业务对象</param>
        /// <param name="logType">日志类型：登陆，操作，接口等</param>
        /// <param name="actionResult">操作结果</param>
        /// <param name="message">操作信息</param>
        public void AddActionLog<T>(T entity, DoResult actionResult, string actionMessage) where T : class, new()
        {
            string module = entity.GetType().Assembly.FullName.Split(',')[0];
            ActionLog actionLog = new ActionLog();
            actionLog.Content = entity.ToXml();
            actionLog.Result = (short)actionResult;
            actionLog.LogType = (short)LogType.Operate;
            actionLog.ID = IdGenerator.NewComb().ToString();

            if (User != null)
            {
                actionLog.UserID = workContext.User.ID;
                actionLog.UserName = workContext.User.LoginName;
            }
            actionLog.Message = actionMessage;
            actionLog.CreateTime = DateTime.Now;
            actionLog.AppModule = module;
            actionLog.ClientIP = WebUtil.GetClientIP();
            try
            {
                repository.Save(actionLog);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("添加日志{0}出错", entity.ToXml()), ex);
            }
        }

        #endregion

        #region overide

        protected override System.Web.Mvc.JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new AgileEAP.MVC.Controllers.JsonResult2
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected virtual System.Web.Mvc.JsonResult Json(object data, params Newtonsoft.Json.JsonConverter[] converters)
        {
            return new AgileEAP.MVC.Controllers.JsonResult2
            {
                Data = data,
                Converters = converters,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion

        sealed public class LoginResult : ActionResult
        {
            private string LoginUrl;
            private string returnUrl;

            override public void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.Clear();
                response.StatusCode = 302;
                response.RedirectLocation = WebUtil.GetRootPath() + "DenyAccess?return=" + HttpUtility.UrlEncode(returnUrl) + "&login=" + HttpUtility.UrlEncode(LoginUrl);
            }

            public LoginResult(string loginUrl = null, string returnUrl = null)
            {
                this.LoginUrl = loginUrl;
                this.returnUrl = returnUrl;
            }
        }
    }

    sealed public class JsonResult2 : System.Web.Mvc.JsonResult
    {
        public Newtonsoft.Json.JsonConverter[] Converters
        {
            get;
            set;
        }

        public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JsonRequest_GetNotAllowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                //  JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                //  response.Write(javaScriptSerializer.Serialize(this.Data)); new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = false }
                string json = (Converters != null) ? AgileEAP.Core.JsonConvert.SerializeObject(this.Data, Converters) : AgileEAP.Core.JsonConvert.SerializeObject(this.Data);
                response.Write(json);
            }
        }
    }
    public enum AuthResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 登陆超时
        /// </summary>
        Timeout = 2,

        /// <summary>
        /// 没有访问权限
        /// </summary>
        NoAccessPrivelege = 3
    }
}
