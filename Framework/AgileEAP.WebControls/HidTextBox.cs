#region Description
/*================================================================================
 *  Copyright (c) Hiway.  All rights reserved.
 * ===============================================================================
 * Solution: Trenhui.WebControls
 * Module:  WebControls
 * Descrption:Create a HidTextBox for page
 * CreateDate: 2009-7-15
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:
 * Remark:
 * Update Time:
 * Author:
 * 
 * ===============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.Security.Permissions;

namespace AgileEAP.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Text"),
    ToolboxData("<{0}:HidTextBox runat=server></{0}:HidTextBox>")]
    public class HidTextBox : HtmlInputHidden
    {
        /// <summary>
        /// 
        /// </summary>
        public override string ClientID
        {
            get
            {
                return this.ID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }
    }
}
