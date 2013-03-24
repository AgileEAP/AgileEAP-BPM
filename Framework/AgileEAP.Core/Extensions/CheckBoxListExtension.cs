using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace AgileEAP.Core.Extensions
{
    public static class CheckBoxListExtension
    {
        public static int IndexOfByValue(this CheckBoxList cbl, string value)
        {
            return cbl.Items.IndexOf(cbl.Items.FindByValue(value));
        }

        public static int IndexOfByText(this CheckBoxList cbl, string text)
        {
            return cbl.Items.IndexOf(cbl.Items.FindByText(text));
        }

        public static void SelectByValue(this CheckBoxList cbl, string value)
        {
            int index = cbl.IndexOfByValue(value);

            if (index > -1)
                cbl.Items[index].Selected = true;
        }

        public static void SelectAll(this CheckBoxList cbl, bool selected)
        {
            foreach (System.Web.UI.WebControls.ListItem item in cbl.Items)
            {
                item.Selected = selected;
            }
        }

        public static void SelectByText(this CheckBoxList cbl, string text)
        {
            int index = cbl.IndexOfByText(text);

            if (index > -1)
                cbl.Items[index].Selected = true;
        }
    }
}
