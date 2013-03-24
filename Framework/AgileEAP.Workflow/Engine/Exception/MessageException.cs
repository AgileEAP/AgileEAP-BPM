using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Workflow.Engine
{
    public class MessageException : WFException
    {
        public string PromptMessage
        {
            get;
            set;
        }

        public MessageException()
        {
            PromptMessage = string.Empty;
        }

        public MessageException(string promptMsg)
        {
            PromptMessage = promptMsg;
        }

        public MessageException(string promptMsg, Type srcType)
        {
            PromptMessage = promptMsg;
            Source = srcType.FullName;
        }

        public override string ToString()
        {
            return string.Format("流程异常:提示消息={0},出错源={1},内部消息={2},内部错误={3}", PromptMessage, Source, Message, InnerException);
        }
    }
}
