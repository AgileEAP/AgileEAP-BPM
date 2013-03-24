using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;
using AgileEAP.Core.Caching;
using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC.Security;

namespace AgileEAP.Plugin.iReport.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
