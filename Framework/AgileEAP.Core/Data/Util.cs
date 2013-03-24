using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileEAP.Core.ExceptionHandler;
namespace AgileEAP.Core.Data
{
    internal class Util
    {
        public void CheckCanChangeCfg()
        {
            throw new EAPException("Configuration Setting can only be changed on the fly when the environment is shut down");
        }
    }
}
