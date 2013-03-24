using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Authentication
{
    public class Profile:IProfile
    {
        public string Name
        {
            get;
            set;
        }

        public string OrgID
        {
            get;
            set;
        }

        public string OrgName
        {
            get;
            set;
        }

        public string OrgPath
        {
            get;
            set;
        }

        public string CorpID
        {
            get;
            set;
        }

        public string CorpName
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }
    }
}
