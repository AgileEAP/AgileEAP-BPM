using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class BaseException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public BaseException(string msg) : base(msg) { }

                        /// <summary>
        /// Initializes a new instance of the Exception class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected BaseException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public BaseException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
