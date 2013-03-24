using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace AgileEAP.Core.Extensions
{
    public static class ListControlExtension
    {
        public static int IndexOfByValue(this ListControl ddl, string value)
        {
            return ddl.Items.IndexOf(ddl.Items.FindByValue(value));
        }

        public static int IndexOfByText(this ListControl  ddl, string text)

        {
            return ddl.Items.IndexOf(ddl.Items.FindByText(text));
        }
    }
}
