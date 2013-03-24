using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Web
{
    public class AjaxResult
    {
        public AjaxResult() { Result = DoResult.Failed; }

        public AjaxResult(DoResult actionResult, string promptMsg, string retValue)
        {
            this.Result = actionResult;
            this.PromptMsg = promptMsg;
            this.RetValue = retValue;
        }

        public DoResult Result
        {
            get;
            set;
        }

        public object RetValue
        {
            get;
            set;
        }

        public string PromptMsg
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        public string DebugMessage
        {
            get;
            set;
        }
    }
}
