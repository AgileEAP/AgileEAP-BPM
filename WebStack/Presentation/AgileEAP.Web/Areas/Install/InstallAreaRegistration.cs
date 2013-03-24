using System.Web.Mvc;

namespace AgileEAP.Web.Areas.Install
{
    public class InstallAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Install";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Install_default",
                "Install/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
