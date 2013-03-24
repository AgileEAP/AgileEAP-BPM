using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AgileEAP.Core.Authentication
{
    public interface IAuthorize
    {
        bool Authorize(IUser user, string requestURL);

        IEnumerable<ToolbarItem> GetToolBarItems(IUser user, string requestURL, string entry);
    }
}
