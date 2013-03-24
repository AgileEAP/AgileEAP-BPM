using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.MVC
{
    public class LoginModel : AgileEAPModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string SecurityCode { get; set; }
    }
}
