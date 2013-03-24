using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.MVC.WebApi
{
    public interface ITokenAuthentication
    {
        bool AuthorizeToken(string token);
    }
}
