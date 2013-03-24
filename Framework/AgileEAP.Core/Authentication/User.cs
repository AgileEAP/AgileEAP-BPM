using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Domain;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Authentication;
using Newtonsoft.Json;

namespace AgileEAP.Core.Authentication
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class User : IUser
    {

        public string ID
        {
            get;
            set;
        }

        public string LoginName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public short UserType
        {
            get;
            set;
        }

        public string OrgID
        {
            get;
            set;
        }

        public string Theme
        {
            get;
            set;
        }
    }
}
