using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTek.EAFrame.Core.Data.Trans
{
    public class SessionManager
    {
        public static ITrans BeginTransaction()
        {
            return new Transaction();
        }
    }
}
