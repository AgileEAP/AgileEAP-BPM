using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class DataAccessException : BaseException
    {
            /// <summary>
        /// 
        /// </summary>
        public DataAccessException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public DataAccessException(string msg) : base(msg) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public DataAccessException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
