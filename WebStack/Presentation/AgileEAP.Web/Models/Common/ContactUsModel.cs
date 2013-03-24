using System.Web.Mvc;
using FluentValidation.Attributes;
using AgileEAP.MVC;
using AgileEAP.Web.Validators.Common;

namespace AgileEAP.Web.Models.Common
{
    [Validator(typeof(ContactUsValidator))]
    public class ContactUsModel : AgileEAPModel
    {
        [AllowHtml]
        [AgileEAPResourceDisplayName("ContactUs.Email")]
        public string Email { get; set; }

        [AllowHtml]
        [AgileEAPResourceDisplayName("ContactUs.Enquiry")]
        public string Enquiry { get; set; }

        [AllowHtml]
        [AgileEAPResourceDisplayName("ContactUs.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}