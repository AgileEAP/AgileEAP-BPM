using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 流程上下文
    /// </summary>
    public class ProcessContext
    {
        public ProcessDefine ProcessDefine
        {
            get;
            set;
        }

        //public ProcessDef ProcessDef
        //{
        //    get;
        //    set;
        //}

        public ProcessInst ProcessInst
        {
            get;
            set;
        }

        public IDictionary<string, object> Parameters
        {
            get;
            set;
        }


        public ProcessContext()
        { }
    }
}
