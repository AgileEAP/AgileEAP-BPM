using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;


using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.FastInvoker;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Web;
using AgileEAP.Core.Domain;
using AgileEAP.WebControls;
using AgileEAP.Core.Log;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Plugin.Authorize
{
    public class BasePage : Page
    {
        protected ILogger log = LogManager.GetLogger(typeof(BasePage));
        protected IRepository<string> repository = EngineContext.Current.Resolve<IRepository<string>>();// new Repository<string>();

        //public new User User
        //{
        //    get
        //    {
        //        if (Session[ApplicationContext.CurrentUserKey] == null)
        //        {
        //            Session[ApplicationContext.CurrentUserKey] = new User { ID = "2d701cb5-65b4-4b6d-9b66-a013001ca1a4", Theme = "Default", LoginName = "suntek", UserType = 0 };
        //        }
        //        return Session[ApplicationContext.CurrentUserKey] as User;
        //    }
        //    set
        //    {
        //        Session[ApplicationContext.CurrentUserKey] = value;
        //    }
        //}
        private IUser cacheUser;
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public new IUser User
        {
            get
            {
                if (cacheUser != null)
                    return cacheUser;

                IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                cacheUser = authenticationService.GetAuthenticatedUser();
                return cacheUser;
            }
            set
            {
                cacheUser = value;
            }
        }

        private string orgPath = string.Empty;
        public string GetOrgPath()
        {
            if (string.IsNullOrEmpty(orgPath))
            {
                Employee employee = repository.FindOne<Employee>(ParameterBuilder.BuildParameters().SafeAdd("OperatorID", User.ID));
                string orgCode = repository.GetDomain<Organization>(employee.MajorOrgID).Code;

                var func = ExpressionUtil.MakeRecursion<string, string>(f => r =>
                {
                    Organization org = repository.All<Organization>().FirstOrDefault(o => o.ID.Trim() == r || o.Code.Trim() == r);
                    if (org == null)
                        return string.Empty;

                    if (string.IsNullOrEmpty(org.ParentID) || org.ParentID == "-1")
                        return org.Code;

                    return string.Format("{0}/{1}", f(org.ParentID.Trim()), org.Code);
                });

                orgPath = func(orgCode);
            }

            return orgPath;
        }

        protected bool IsAdmin
        {
            get
            {
                return User != null && User.UserType == (short)UserType.Administrator;
            }
        }

        public string Skin
        {
            get
            {
                return (User ?? new User() { Theme = Configure.Get<string>("DefaultSkin", "Default") }).Theme;
            }
        }

        public string AppID
        {
            get
            {
                return Configure.Get<string>("AppID", "VRMS");
            }
        }

        #region IAuthorize 成员

        public virtual bool Authorize(IUser user, string requestUrl)
        {
            return Authorize(user, requestUrl, true);
        }

        public virtual bool Authorize(IUser user, string requestUrl, bool isForce)
        {
            if (user == null)
            {
                Response.Redirect(string.Format("{0}login", WebUtil.GetRootPath()));
                Response.End();
            }

            if (!isForce || user.UserType == (short)UserType.Administrator) return true;

            try
            {
                var authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                string requestURL = WebUtil.GetRequestFullUrl();
                bool isSuccess = authenticationService.Authorize(User, requestURL);
                if (!isSuccess)
                {
                    AddActionLog<ActionLog>(string.Format("用户{0}没有访问{1}的权限", User.LoginName, requestURL), DoResult.Failed);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("验证用户{0}权限出错", User.LoginName), ex);
            }

            return false;
        }

        #region "页面加载中效果"
        /// <summary>
        /// 页面加载中效果
        /// </summary>
        protected void DisplayPageLoading(string message = "正在加载中")
        {
            HttpContext.Current.Response.Write("<div id=\"divParent\">");
            HttpContext.Current.Response.Write("<div id=\"divChild\">");
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
            HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
            HttpContext.Current.Response.Write("function animate(){");
            HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
            HttpContext.Current.Response.Write("if(elem != null) {");
            HttpContext.Current.Response.Write("if (pos==0) len += dir;");
            HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
            HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
            HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
            HttpContext.Current.Response.Write("elem.style.left = pos;");
            HttpContext.Current.Response.Write("elem.style.width = len;");
            HttpContext.Current.Response.Write("}}");
            HttpContext.Current.Response.Write("function remove_loading() {");
            HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
            HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
            HttpContext.Current.Response.Write("targelem.style.display='none';");
            HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
            HttpContext.Current.Response.Write("var divObj = document.getElementById('divChild');");
            HttpContext.Current.Response.Write("divObj.parentNode.removeChild(divObj);");
            HttpContext.Current.Response.Write("}");
            HttpContext.Current.Response.Write("window.onload = remove_loading;");
            HttpContext.Current.Response.Write("</script>");
            HttpContext.Current.Response.Write("<style>");
            HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
            HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
            HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
            HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("<div id=loader_container>");
            HttpContext.Current.Response.Write("<div id=loader>");
            HttpContext.Current.Response.Write("<div align=center>" + message + "...</div>");
            HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
            HttpContext.Current.Response.Write("</div></div>");
            HttpContext.Current.Response.Write("</div></div>");

            HttpContext.Current.Response.Flush();
        }
        #endregion

        protected virtual void HandleAjaxPost()
        {
            string ajaxArgument = Request.Form["AjaxArgument"];
            string ajaxAction = Request.Form["AjaxAction"];
            try
            {
                Type type = this.Page.GetType();
                MethodInfo methodInfo;
                try
                {
                    methodInfo = type.GetMethod(ajaxAction, new Type[] { typeof(string) });
                }
                catch
                {
                    methodInfo = type.GetMethod(ajaxAction);
                }
                if (methodInfo == null)
                {
                    methodInfo = type.GetMethod(ajaxAction);
                }
                FastInvokeHandler invoker = BaseMethodInvoker.GetMethodInvoker(methodInfo);

                object result = invoker(this.Page, new object[] { ajaxArgument });

                if (result != null)
                {
                    Response.Write(result.ToString());
                    Response.Flush();

                    Response.End();
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                log.Debug(string.Format("页面{0}异步执行{1}参数{2}", Request.Url, ajaxAction, ajaxArgument));
                System.Diagnostics.Debug.Write(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("页面{0}异步执行{1}方法出错，参数{2}", Request.Url, ajaxAction, ajaxArgument), ex);
                AjaxResult errorResult = new AjaxResult()
                {
                    Result = DoResult.Failed,
                    PromptMsg = "系统出错请联系管理员",
                };
                Response.Write(JsonConvert.SerializeObject(errorResult));
                Response.Flush();
                //Response.End();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            //验证是否有访问页面的权限
            DoAuthorize();

            //处理AjaxPost
            if (IsAjaxPost)
            {
                HandleAjaxPost();
            }

            base.OnPreInit(e);
        }

        private void DoAuthorize()
        {
            string requestURL = WebUtil.GetRequestFullUrl();
            if (!IsAjaxPost && !Authorize(User, requestURL))
            {
                try
                {
                    Response.Write(string.Format("对不起，您没有访问{0}权限！请联系管理员", Request.Url.ToSafeString()));
                    Response.Write(requestURL);
                    Response.End();
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    log.Debug(string.Format("对不起，您没有访问{0}权限！请联系管理员", Request.Url.ToSafeString()));
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        #endregion

        public bool IsAjaxPost
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Request["AjaxAction"]);
            }
        }

        private string currentId = string.Empty;
        public string CurrentId
        {
            get
            {
                currentId = !string.IsNullOrWhiteSpace(Request["CurrentId"]) ? Request["CurrentId"] : Request.Form["hidCurrentId"];
                if (string.IsNullOrEmpty(currentId))
                {
                    HidTextBox hidCurrentId = this.FindControl("hidCurrentId") as HidTextBox;
                    if (hidCurrentId != null && !string.IsNullOrWhiteSpace(hidCurrentId.Value))
                    {
                        currentId = hidCurrentId.Value;
                    }
                    else
                    {
                        currentId = IdGenerator.NewComb().ToSafeString();
                    }
                }
                return currentId;
            }
            set
            {
                HidTextBox hidCurrentId = this.FindControl("hidCurrentId") as HidTextBox;
                if (hidCurrentId != null) hidCurrentId.Value = value;
            }
        }


        /// <summary>
        /// 页面上下文对象
        /// </summary>
        private PageContext<string> pageContext;
        public virtual PageContext<string> PageContext
        {
            get
            {
                if (pageContext == null)
                {
                    pageContext = NewPageContext();
                }
                return pageContext;
            }
            set
            {
                pageContext = value;
            }
        }

        /// <summary>
        /// 新建上下页面上下文对象
        /// </summary>
        /// <returns></returns>
        protected PageContext<string> NewPageContext()
        {
            pageContext = new PageContext<string>();
            string actionFlag = Request.QueryString["ActionFlag"];
            if (!string.IsNullOrEmpty(actionFlag))
            {
                pageContext.Action = (ActionType)Enum.Parse(typeof(ActionType), actionFlag);
            }
            else
            {
                pageContext.Action = ActionType.View;
            }

            //string lastUrl = Request.QueryString["LastUrl"];
            //pageContext.LastUrl = string.IsNullOrEmpty(lastUrl) ? Request.Url.PathAndQuery : lastUrl;
            //pageContext.CurrentId = CurrentId;

            return pageContext;
        }

        /// <summary>
        /// 写异常日志,不进行提示。
        /// </summary>
        /// <param name="actionMessage"></param>
        /// <param name="exception"></param>
        protected void WriteErrorLog(string actionMessage, Exception exception)
        {
            log.Error(actionMessage, exception);
        }
        /// <summary>
        /// 读Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string ReadCookie(string key)
        {
            HttpCookie cookie = Request.Cookies[key];

            return cookie == null ? string.Empty : Server.UrlDecode(cookie.Value);
        }

        /// <summary>
        /// 写Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void WriteCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, Server.UrlEncode(value));
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 给页面表单对象赋值
        /// </summary>
        /// <param name="entity">要赋值的对象</param>
        protected virtual void SetControlValues<T>(T entity) where T : class, new()
        {
            //获取模板页的内容容器对象
            Control parent = ((Page.Master != null) ? Page.Master.FindControl("contentPlace") : this) ?? this;
            if (entity == null) return;
            Type type = entity.GetType();//获取类型
            var pi = (from p in type.GetProperties() where ((p.PropertyType.Equals(typeof(string)) || p.PropertyType.IsValueType) && !p.Name.Equals("TableName")) select p).ToArray<PropertyInfo>();//获取属性集合
            foreach (PropertyInfo p in pi)
            {
                try
                {
                    string controlName = getControlName(p.Name, p.PropertyType);

                    object propertyValue = p.GetValue(entity, null);//获取属性值
                    object value = p.PropertyType == typeof(DateTime) && propertyValue != null ? ((DateTime)propertyValue).ToShortDateString() : propertyValue;

                    Control control = parent.FindControl(controlName);
                    //判断是不是userid，如果是显示中文名
                    if (p.Name.Equals("Creator", StringComparison.OrdinalIgnoreCase))
                    {
                        Operator operatorInfo = repository.GetDomain<Operator>((string)value);
                        value = operatorInfo == null ? User.LoginName : operatorInfo.UserName;
                    }


                    if (control != null && value != null)
                    {
                        if (control is ComboBox)
                        {
                            ((ComboBox)control).Value = value.ToSafeString().Trim();
                        }
                        if (control is Combox)
                        {
                            ((Combox)control).SelectedValue = value.ToSafeString();
                        }
                        else if (control is DropDownList)
                        {
                            DropDownList ddlControl = ((DropDownList)control);
                            ddlControl.SelectedIndex = ddlControl.IndexOfByValue(value.ToSafeString().Trim());
                        }
                        else if (control is TextBox)
                        {
                            ((TextBox)control).Text = value.ToString().Trim();
                        }
                        else if (control is HtmlInputText)
                        {
                            ((HtmlInputText)control).Value = value.ToString().Trim();
                        }
                        else if (control is RadioButtonList)
                        {
                            RadioButtonList rblControl = ((RadioButtonList)control);
                            rblControl.IndexOfByValue(value.ToString().Trim());
                        }
                    }
                    else
                    {
                        controlName = string.Format("txt{0}", p.Name);
                        control = parent.FindControl(controlName);
                        if (control != null && value != null)
                        {
                            //string text = (value != null && p.PropertyType == typeof(DateTime)) ? ((DateTime)value).ToShortDateString() : value.ToSafeString();
                            if (control is TextBox)
                            {
                                ((TextBox)control).Text = value.ToString().Trim();
                            }
                            else if (control is HtmlInputText)
                            {
                                ((HtmlInputText)control).Value = value.ToString().Trim();
                            }
                        }
                        else
                        {
                            controlName = string.Format("rbl{0}", p.Name);
                            control = parent.FindControl(controlName);
                            if (control != null && value != null)
                            {
                                if (control is RadioButtonList)
                                {
                                    RadioButtonList rblControl = ((RadioButtonList)control);
                                    rblControl.SelectedIndex = rblControl.IndexOfByValue(value.ToString().Trim());
                                }
                            }
                            else
                            {
                                controlName = string.Format("ddl{0}", p.Name);
                                control = parent.FindControl(controlName);
                                if (control != null && value != null)
                                {
                                    if (control is DropDownList)
                                    {
                                        DropDownList ddlControl = ((DropDownList)control);
                                        ddlControl.SelectedIndex = ddlControl.IndexOfByValue(value.ToString().Trim());
                                    }
                                }
                                else
                                {
                                    controlName = string.Format("cbo{0}", p.Name);
                                    control = parent.FindControl(controlName);
                                    if (control != null && value != null)
                                    {
                                        if (control is ComboBox)
                                        {
                                            ComboBox cboControl = ((ComboBox)control);
                                            cboControl.Value = value.ToSafeString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        controlName = string.Format("dtp{0}", p.Name);
                                        control = parent.FindControl(controlName);
                                        if (control != null && value != null)
                                        {
                                            if (control is DatePicker)
                                            {
                                                DatePicker cboControl = ((DatePicker)control);
                                                cboControl.Text = value.ToSafeString().Trim();
                                            }
                                        }
                                    }
                                }
                                log.Debug(string.Format("Can't set entity {0} property {1} value {2}!", type, p.Name, value));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        /// <summary>
        /// 获取表单赋值的实体对象
        /// </summary>
        /// <returns>获取自动赋值的实体对象</returns>
        protected virtual void GetControlValues<T>(ref T entity) where T : class, new()
        {
            NameValueCollection form = Request.Form;
            string parentUniqueID = string.Empty;
            if (Page.Master != null)
            {
                Control parent = Page.Master.FindControl("contentPlace");
                if (parent != null)
                    parentUniqueID = string.Format("{0}$", parent.UniqueID);
            }

            Type type = entity.GetType();//获取类型
            var pi = (from p in type.GetProperties() where ((p.PropertyType.Equals(typeof(string)) || p.PropertyType.IsValueType) && !p.Name.Equals("TableName")) select p).ToArray<PropertyInfo>();//获取属性集合
            foreach (PropertyInfo p in pi)
            {

                if (p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase)) continue;

                string controlName = string.Format("{0}{1}", parentUniqueID, getControlName(p.Name, p.PropertyType));
                if (form[controlName] != null)
                {
                    try
                    {
                        if (!string.Equals(p.Name, "Creator"))
                        {
                            p.SetValue(entity, Convert.ChangeType(form[controlName], p.PropertyType), null);//为属性赋值，并转换键值的类型为该属性的类型
                        }
                        else if (PageContext.Action == ActionType.Add)
                        {
                            p.SetValue(entity, User.ID, null);//为属性赋值，并转换键值的类型为该属性的类型
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("Entity {0} Set Property  {1}  Value Error!", typeof(T), controlName), ex);
                    }
                }
                else
                {
                    controlName = string.Format("{0}txt{1}", parentUniqueID, p.Name);
                    if (form[controlName] == null)
                    {
                        controlName = string.Format("{0}rbl{1}", parentUniqueID, p.Name);
                        if (form[controlName] == null)
                        {
                            controlName = string.Format("{0}ddl{1}", parentUniqueID, p.Name);
                            if (form[controlName] == null)
                            {
                                controlName = string.Format("{0}dtp{1}", parentUniqueID, p.Name);
                                if (form[controlName] == null)
                                {
                                    controlName = string.Format("{0}chb{1}", parentUniqueID, p.Name);
                                    if (form[controlName] == null)
                                    {
                                        controlName = string.Format("{0}cbo{1}", parentUniqueID, p.Name);
                                        if (form[controlName] == null)
                                        {
                                            controlName = string.Format("cbo{0}data", p.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (form[controlName] != null)
                    {
                        try
                        {
                            p.SetValue(entity, Convert.ChangeType(form[controlName], p.PropertyType), null);//为属性赋值，并转换键值的类型为该属性的类型
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("Entity {0} Set Property  {1}  Value Error!", typeof(T), controlName), ex);
                        }
                    }
                    else
                    {
                        log.Error(string.Format("Entity {0} Can't Get Property  {1}  Value !", typeof(T), controlName));
                    }
                }
            }
        }

        /// <summary>
        /// 按控件命名规范获取控件名称
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyType">属性类型</param>
        /// <returns></returns>
        private string getControlName(string propertyName, Type propertyType)
        {
            if (propertyType == typeof(DateTime))
            {
                return string.Format("dtp{0}", propertyName);
            }
            else if (propertyName.EndsWith("Code") || (propertyName.EndsWith("ID") && !propertyName.Equals("ID", StringComparison.OrdinalIgnoreCase)))
            {
                return string.Format("chb{0}", propertyName);
            }

            if (propertyType == typeof(short) || propertyType == typeof(Int16))
                return string.Format("cbo{0}", propertyName);

            return string.Format("txt{0}", propertyName);
        }

        /// <summary>
        /// 保存当前页上下文信息
        /// </summary>
        /// <param name="context">页面上下文对象</param>
        public virtual void SavePageContext(PageContext<string> context)
        { }

        /// <summary>
        /// 恢复当前页上下文信息
        /// </summary>
        /// <returns></returns>
        public virtual PageContext<string> RestorePageContext()
        {
            return NewPageContext();
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns>返回查询条件的字典对象，</returns>
        protected virtual IDictionary<string, object> GetFilterParameters()
        {
            //获取模板页的内容容器对象
            Control parent = parent = ((Page.Master != null) ? Page.Master.FindControl("contentPlace") : this) ?? this;

            if (parent == null) return null;

            IDictionary<string, object> filterParameters = new Dictionary<string, object>();
            foreach (Control control in parent.Controls)
            {
                if (string.IsNullOrEmpty(control.ID)) continue;

                string controlId = control.ID;
                if (controlId.StartsWith(ApplicationContext.StartFilter))
                {
                    //约定查询控件的ID="filter"+查询对象的属性名
                    //约定smallint类型的属性对应的控件为DropDownList
                    //获取查询的属性名
                    string propertyName = controlId.Substring(ApplicationContext.StartFilter.Length);
                    if (control is DropDownList)
                    {
                        short value = -1;

                        if (short.TryParse(((DropDownList)control).SelectedValue, out value))
                        {
                            if (value != -1)
                                filterParameters.Add(propertyName, value);
                        }
                        else
                        {
                            filterParameters.Add(propertyName, ((DropDownList)control).SelectedValue);
                        }
                    }
                    else if (control is TextBox)
                    {
                        string value = ((TextBox)control).Text.Trim();
                        if (!string.IsNullOrEmpty(value))
                            filterParameters.Add(propertyName, value);
                    }
                    else if (control is HtmlInputText)
                    {
                        string value = ((HtmlInputText)control).Value;
                        if (!string.IsNullOrEmpty(value))
                            filterParameters.Add(propertyName, value);
                    }
                    else if (control is RadioButtonList)
                    {
                        filterParameters.Add(propertyName, ((RadioButtonList)control).SelectedValue.ToShort());
                    }
                }
            }

            return filterParameters;
        }

        protected virtual void SetFilterControlValues(IDictionary<string, object> filter)
        {
            Control parent = ((Page.Master != null) ? Page.Master.FindControl("contentPlace") : this) ?? this;
            foreach (KeyValuePair<string, object> keyValuePair in filter)
            {
                string controlName = "filter" + keyValuePair.Key;
                Control control = parent.FindControl(controlName);
                object value = keyValuePair.Value;
                if (control != null && value != null)
                {
                    if (control is ComboBox)
                    {
                        ((ComboBox)control).Value = value.ToSafeString();
                    }
                    else if (control is DropDownList)
                    {
                        DropDownList ddlControl = ((DropDownList)control);
                        ddlControl.SelectedIndex = ddlControl.IndexOfByValue(value.ToSafeString());
                    }
                    else if (control is TextBox)
                    {
                        ((TextBox)control).Text = value.ToString();
                    }
                    else if (control is HtmlInputText)
                    {
                        ((HtmlInputText)control).Value = value.ToString();
                    }
                    else if (control is RadioButtonList)
                    {
                        RadioButtonList rblControl = ((RadioButtonList)control);
                        rblControl.IndexOfByValue(value.ToString());
                    }
                }
            }

        }


        /// <summary>
        /// 把Json数据赋值给对象
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        protected T DeserializeObject<T>(string jsonData) where T : DomainObject<string>, new()
        {
            IDictionary<string, object> propertyValues = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonData);
            T entity = new T();
            PropertyInfo[] perperties = typeof(T).GetProperties();
            foreach (KeyValuePair<string, object> valuePair in propertyValues)
            {
                if (!valuePair.Key.Equals("ID", StringComparison.OrdinalIgnoreCase) && valuePair.Value != null)
                {
                    var pi = perperties.FirstOrDefault(o => o.Name.Equals(valuePair.Key));
                    if (pi != null)
                    {
                        try
                        {
                            pi.SetValue(entity, Convert.ChangeType(valuePair.Value, pi.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("Property {0} set value {1} Error!", pi.Name, valuePair.Value), ex);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// 把Json数据赋值给对象
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        protected T DeserializeObject<T>(string id, string jsonData) where T : DomainObject<string>, new()
        {
            IDictionary<string, object> propertyValues = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonData);
            T entity = repository.GetDomain<T>(id);

            if (entity == null) entity = new T();
            PropertyInfo[] perperties = typeof(T).GetProperties();
            foreach (KeyValuePair<string, object> valuePair in propertyValues)
            {
                if (!valuePair.Key.Equals("ID", StringComparison.OrdinalIgnoreCase) && valuePair.Value != null)
                {
                    var pi = perperties.FirstOrDefault(o => o.Name.Equals(valuePair.Key));
                    if (pi != null)
                    {
                        try
                        {
                            pi.SetValue(entity, Convert.ChangeType(valuePair.Value, pi.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            log.Error(string.Format("Property {0} set value {1} Error!", pi.Name, valuePair.Value), ex);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entity.ID)) entity.ID = id;

            return entity;
        }

        /// <summary>
        /// 添加用户操作日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="logType">日志类型：登陆，操作，接口等</param>
        /// <param name="DoResult">操作结果</param>
        /// <param name="message">操作信息</param>
        public void AddActionLog<T>(LogType logType, string actionMessage, DoResult DoResult)
        {
            Type type = typeof(T);
            ActionLog actionLog = new ActionLog();
            actionLog.Result = (short)DoResult;
            actionLog.LogType = (short)logType;
            actionLog.ID = IdGenerator.NewComb().ToString();
            if (User != null)
            {
                actionLog.UserID = User.ID;
                actionLog.UserName = User.LoginName;
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
        ///  <param name="DoResult">操作结果</param>
        public void AddActionLog<T>(string actionMessage, DoResult DoResult) where T : class, new()
        {
            AddActionLog<T>(LogType.Operate, actionMessage, DoResult);
        }

        /// <summary>
        /// 添加用户操作日志
        /// </summary>
        /// <param name="entity">业务对象</param>
        /// <param name="logType">日志类型：登陆，操作，接口等</param>
        /// <param name="DoResult">操作结果</param>
        /// <param name="message">操作信息</param>
        public void AddActionLog<T>(T entity, DoResult DoResult, string actionMessage) where T : class, new()
        {
            string module = entity.GetType().Assembly.FullName.Split(',')[0];
            ActionLog actionLog = new ActionLog();
            actionLog.Content = entity.ToXml();
            actionLog.Result = (short)DoResult;
            actionLog.LogType = (short)LogType.Operate;
            actionLog.ID = IdGenerator.NewComb().ToString();

            if (User != null)
            {
                actionLog.UserID = User.ID;
                actionLog.UserName = User.LoginName;
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

        /// <summary>
        /// 清除显示界面显示控件的值
        /// </summary>
        protected virtual void ClearValues(Type type)
        {
            //获取模板页的内容容器对象
            Control parent = ((Page.Master != null) ? Page.Master.FindControl("contentPlace") : this) ?? this;

            var pi = (from p in type.GetProperties() where ((p.PropertyType.Equals(typeof(string)) || p.PropertyType.IsValueType) && !p.Name.Equals("TableName")) select p).ToArray<PropertyInfo>();//获取属性集合

            foreach (PropertyInfo p in pi)
            {
                string controlName = getControlName(p.Name, p.PropertyType);

                Control control = parent.FindControl(controlName);

                if (control != null)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Text = string.Empty;
                    }
                    else if (control is HtmlInputText)
                    {
                        ((HtmlInputText)control).Value = string.Empty;
                    }
                }
                else
                {
                    controlName = string.Format("txt{0}", p.Name);
                    control = parent.FindControl(controlName);
                    if (control != null)
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        else if (control is HtmlInputText)
                        {
                            ((HtmlInputText)control).Value = string.Empty;
                        }
                    }
                    else
                    {
                        log.Debug(string.Format("Can't clear control {0}'s value!", controlName));
                    }
                }
            }
        }

        /// <summary>
        /// 网站虚拟根路径
        /// </summary>
        public string RootPath
        {
            get
            {
                return WebUtil.GetRootPath();
            }
        }

        /// <summary>
        /// 获取数据过滤的sql
        /// </summary>
        /// <param name="filterField">数据关联字段</param>
        /// <returns>o（）</returns>
        public string GetFilterString(string filterField)
        {
            if (User.UserType == (short)UserType.Administrator) return string.Empty;
            var authorizeService = EngineContext.Current.Resolve<IAuthorizeService>();
            List<string> dataPriveleges = authorizeService.GetDataPriveleges(User.ID);

            //组织过滤字符串
            string filterStr = string.Join(" or ", dataPriveleges.Select(dataPrivelege => string.Format("{0} like '{1}%'", filterField, dataPrivelege)).ToArray());

            return string.Format("({0} {1} creator='{2}') ", filterStr, string.IsNullOrEmpty(filterStr) ? string.Empty : "or", User.ID);
        }


        /// <summary>
        /// 获取过滤的sql片段
        /// </summary>
        /// <returns>o（）</returns>
        public string GetFilterString()
        {
            return GetFilterString("OwnerOrg");
        }

        /// <summary>
        /// 获取人员所在部门名称
        /// </summary>
        /// <param name="dictItemID"></param>
        /// <returns></returns>
        public Organization GetOrgNameByUserID(string userID)
        {
            var authorizeService = EngineContext.Current.Resolve<IAuthorizeService>();
            IList<Organization> orgs = authorizeService.GetOrgNameByUserID(userID);
            if (orgs != null)
            {
                foreach (var org in orgs) //如有多个部门，返回第一个
                {
                    return org;
                }
            }
            return new Organization();
        }

        public string GetOrgCodePath(string orgCode)
        {
            return repository.All<Organization>().FirstOrDefault(o => o.ID.Trim() == orgCode).Code;

            //var func = ExpressionUtil.MakeRecursion<string, string>(f => r =>
            //{
            //    Organization org = repository.All<Organization>().FirstOrDefault(o => o.ID.Trim() == r || o.Code.Trim() == r);
            //    if (org == null)
            //        return string.Empty;

            //    if (string.IsNullOrEmpty(org.ParentID) || org.ParentID == "-1")
            //        return org.Code;

            //    return string.Format("{0}/{1}", f(org.ParentID.Trim()), org.Code);
            //});

            //return func(orgCode);
        }
    }
}
