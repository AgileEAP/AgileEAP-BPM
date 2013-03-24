using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Mail
{
    public class Email
    {
        public string Subject
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string From
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }
    }
}
