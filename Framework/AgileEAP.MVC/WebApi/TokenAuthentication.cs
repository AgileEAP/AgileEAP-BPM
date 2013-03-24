using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.MVC.WebApi
{
    public class TokenAuthentication : ITokenAuthentication
    {
        public bool AuthorizeToken(string token)
        {
            return token == "agileEAP";
        }
    }
}
