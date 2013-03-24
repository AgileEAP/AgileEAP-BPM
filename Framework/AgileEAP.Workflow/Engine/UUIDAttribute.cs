using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Workflow.Engine
{
    public class UUIDAttribute : Attribute
    {
        public string UUID
        {
            get;
            set;
        }

        public UUIDAttribute(string uuid)
        {
            UUID = uuid;
        }
    }
}
