using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Workflow.Engine
{
    public class WFException : ApplicationException
    {
        public WFException()
        { }

        public WFException(string source, string message)
            : this(source, message, null)
        { }

        public WFException(string source, string mesage, Exception innerException)
            : base(mesage, innerException)
        { Source = source; }

        public override string ToString()
        {
            return string.Format("流程异常:出错源={0},内部消息={1},内部错误={2}",Source, Message, InnerException);
        }
    }
}
