using System.Web.Mvc;

namespace AgileEAP.MVC
{
    public class AgileEAPModel
    {
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
    }
    public class AgileEAPEntityModel : AgileEAPModel
    {
        public virtual string ID { get; set; }
    }
}
