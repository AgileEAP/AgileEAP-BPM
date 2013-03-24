using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class BusinessException : BaseException
    {
       /// <summary>
        /// 
        /// </summary>
        public BusinessException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public BusinessException(string msg) : base(msg) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public BusinessException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
