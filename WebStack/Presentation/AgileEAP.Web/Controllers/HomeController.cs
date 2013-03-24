using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.MVC.Controllers;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;
using AgileEAP.MVC;
using AgileEAP.Core.Caching;

namespace AgileEAP.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }

        // [Authorization]
        public ActionResult Index()
        {
            #region Load MainMenu
            var authorizeService = EngineContext.Current.Resolve<IAuthorizeService>();
            //取得用户的角色
            List<string> privilegeIDs = authorizeService.GetPrivilegeIDs(workContext.User.ID);
            //根据权限id取到资源id集合
            string appName = Configure.Get<string>("AppID", "eCloud");
           // string appID = repository.Query<App>().FirstOrDefault(o => o.Name == Configure.Get<string>("AppID", "eCloud")).ID;
            IList<string> resIDs = repository.Query<AgileEAP.Infrastructure.Domain.Privilege>().Where(o => privilegeIDs.Contains(o.ID)).Select(o => o.ResourceID).ToList();
            ViewData["MenuItems"] = repository.All<Resource>().Where(r => r.Type == (short)ResourceType.Menu && (r.ParentID == null || r.ParentID == appName) && (workContext.User.UserType == (short)UserType.Administrator || resIDs.Contains(r.ID))).OrderBy(r => r.SortOrder).ToList();
            #endregion

            return View();
        }

        public ActionResult Navigate(string menuID)
        {
            IEnumerable<Resource> menuItems = repository.All<Resource>().Where(r => r.Type == (short)ResourceType.Menu && r.ParentID == menuID).OrderBy(r => r.SortOrder).ToList();

            return View(menuItems);
        }
    }
}
