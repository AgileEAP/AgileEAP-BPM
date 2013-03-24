using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Infrastructure;
using AgileEAP.Core.Extensions;

using System.Web.Mvc;

using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using System.ComponentModel;
using AgileEAP.Core;
using AgileEAP.Workflow.Domain;

namespace AgileEAP.Plugin.Workflow.Controllers
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