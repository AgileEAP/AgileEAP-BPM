using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileEAP.Core;

namespace AgileEAP.Plugin.FormDesigner.Models
{
    public class FormModel
    {
        public Form Form { get; set; }

        public IDictionary<string, object> Values { get; set; }
    }
}
