using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AgileEAP.Plugin.OpenApi.HostSample.Infrastructure.OAuth;

namespace AgileEAP.Plugin.OpenApi.HostSample.Controllers {
    public class HomeController : Controller {
        string ConnectionString {
            get {
                string databasePath = Path.Combine(Server.MapPath(Request.ApplicationPath), "App_Data");
                if (!Directory.Exists(databasePath)) {
                    Directory.CreateDirectory(databasePath);
                }
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString.Replace("|DataDirectory|", databasePath);
                return connectionString;
            }
        }
        public ActionResult Index() {
            var dc = new DataClassesDataContext(ConnectionString);
            ViewBag.DBExists = dc.DatabaseExists();
            return View();
        }
     
    }
}