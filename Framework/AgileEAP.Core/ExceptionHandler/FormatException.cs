using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class FormatException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        public FormatException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public FormatException(string msg) : base(msg) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public FormatException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
