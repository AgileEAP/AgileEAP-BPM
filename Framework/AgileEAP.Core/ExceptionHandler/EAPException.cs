using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AgileEAP.Core.ExceptionHandler
{
    [Serializable]
    public class EAPException : BaseException
    {
        /// <summary>
        /// 
        /// </summary>
        public EAPException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public EAPException(string msg) : base(msg) { }

                /// <summary>
        /// Initializes a new instance of the Exception class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected EAPException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

                /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
		/// <param name="messageFormat">The exception message format.</param>
		/// <param name="args">The exception message arguments.</param>
        public EAPException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public EAPException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
