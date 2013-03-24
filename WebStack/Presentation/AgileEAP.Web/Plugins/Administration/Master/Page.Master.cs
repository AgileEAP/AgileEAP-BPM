using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;


using AgileEAP.Core.FastInvoker;
using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Authentication;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;
using AgileEAP.Core.Web;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Utility;
using AgileEAP.WebControls;
using System.Text;

namespace AgileEAP.Administration
{
    public partial class PageMaster : BaseMasterPage
    {
        #region Properties
        // <summary>
        /// 系统日志对象
        /// </summary>
        protected ILogger log = LogManager.GetLogger(typeof(User));
        IRepository<string> repository = EngineContext.Current.Resolve<IRepository<string>>();

        private IUser cacheUser;
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public IUser User
        {
            get
            {
                if (cacheUser != null)
                    return cacheUser;

                IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                cacheUser = authenticationService.GetAuthenticatedUser();
                return cacheUser;
            }
        }

        public string Skin
        {
            get
            {
                return User.Theme ?? ApplicationContext.DefaultSkin;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            commandBar.CommandExecute += new CommandBar.CommandExecuteEventHandler(commandBar_CommandExecute);
        }

        /// <summary>
        /// 调用操作按钮执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void commandBar_CommandExecute(object sender, CommandExecuteEventArgs e)
        {
            try
            {
                object[] args = { e.CommandArgument };
                Type type = this.Page.GetType();
                MethodInfo methodInfo = type.GetMethod(e.CommandName);
                FastInvokeHandler invoker = BaseMethodInvoker.GetMethodInvoker(methodInfo);
                invoker(this.Page, args);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                WebUtil.PromptMsg(string.Format("按钮触发{0}出错！", e.CommandName));
            }
        }

        /// <summary>
        /// 加载页面操作按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadPageBar()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string requestUrl = Request.Url.AbsolutePath.Remove(0, appPath.Length).TrimStart("/".ToCharArray());
            string entry = Request.QueryString["Entry"].ToSafeString();

            Resource resource = repository.All<Resource>().FirstOrDefault(r => r.URL.EqualIgnoreCase(requestUrl) && r.Entry.ToSafeString().EqualIgnoreCase(entry));
            if (resource == null) return;

            ShowNavigation(resource);

            if (resource.ShowToolBar.Cast<Options>(Options.Yes) == Options.No)
            {
                commandBar.Visible = false;
                return;
            }

            #region 页面操作权限验证 ljz 添加
            //取得用户的角色

            var authorizeService = EngineContext.Current.Resolve<IAuthorizeService>();
            List<string> privilegeIDs = authorizeService.GetPrivilegeIDs(User.ID);
            //根据权限id取到操作项id集合
            List<string> operateIds = repository.All<Privilege>().Where(p => p.ResourceID == resource.ID && !string.IsNullOrWhiteSpace(p.OperateID) && (User.UserType == (short)UserType.Administrator || privilegeIDs.Contains(p.ID))).Select(p => p.OperateID).ToList() ?? new List<string>();

            #endregion

            IList<Operate> operates = repository.All<Operate>().Where(o => operateIds.Contains(o.ID)).OrderBy(o => o.SortOrder).ToList();//UserBiz.GetAuthorizeActions(User, requestUrl, entry);
            foreach (Operate operate in operates)
            {
                CommandItem item = new CommandItem();
                item.Text = operate.OperateName;
                item.CommandName = operate.CommandName;
                item.CommandArgument = operate.CommandArgument;
                item.OnClientClick = operate.CommandName;
                item.Runat = operate.Runat;
                item.DoValid = operate.IsVerify == 1;

                commandBar.Items.Add(item);
            }
        }
        //加载页面导航信息
        private void ShowNavigation(Resource resource)
        {
            if (resource != null && resource.ShowNavigation.Cast<Options>(Options.No) == Options.Yes)
            {
                string appPath = HttpContext.Current.Request.ApplicationPath;
                string requestUrl = Request.Url.AbsolutePath.Remove(0, appPath.Length).TrimStart("/".ToCharArray());
                string entry = Request.QueryString["Entry"].ToSafeString();

                List<Resource> menus = repository.All<Resource>().Where(r => r.Type == (short)(int)ResourceType.Menu || r.Type == (short)(int)ResourceType.Page).ToList();
                var func = ExpressionUtil.MakeRecursion<List<Resource>, Resource, string>(f => (all, r) => { Resource parent = all.FirstOrDefault(a => a.ID == r.ParentID); return parent == null ? r.Text : string.Format("{0}>>{1}", f(all, parent), r.Text); });
                navigation.InnerHtml = "<label style='font-weight:bold;color:#696969'>当前位置：" + func(menus, resource) + "</label>";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["AjaxAction"]))
            {
                //加载页面操作按钮
                LoadPageBar();
            }
        }
    }
}
