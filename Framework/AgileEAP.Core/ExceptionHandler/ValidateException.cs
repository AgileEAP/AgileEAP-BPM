using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class ValidateException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidateException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public ValidateException(string msg) : base(msg) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public ValidateException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
