using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core
{
    public class LogManager
    {
        public static ILogger GetLogger(string loggerName)
        {
            return new Logger(loggerName);
        }

        public static ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }
    }
}
