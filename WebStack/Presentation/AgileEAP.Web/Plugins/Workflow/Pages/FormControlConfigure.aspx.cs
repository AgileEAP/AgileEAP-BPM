using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AgileEAP.Plugin.Workflow
{
    public partial class FormControlConfigure : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];
            if (type == "ChooseBox")
            {
                txtURL.Text = Request.QueryString["url"];
                divChooseBox.Visible = true;
                divTextBox.Visible = false;
            }
            else if (type == "TextBox")
            {
                txtCols.Text = Request.QueryString["cols"];
                txtRows.Text = Request.QueryString["rows"];
                divChooseBox.Visible = false;
                divTextBox.Visible = true;
            }
        }
    }
}