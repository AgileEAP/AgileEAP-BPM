#region Description
/*==============================================================================
 *  Copyright (c) suntektech co.,ltd. All Rights Reserved.
 * ===============================================================================
 * 描述：电子表单控件
 * 作者：trh
 * 创建时间：2010-06-10
 * ===============================================================================
 * 历史记录：
 * 描述：
 * 作者：
 * 修改时间：
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;
using System.Linq;

using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Enums;

namespace AgileEAP.EForm
{
    [
 AspNetHostingPermission(SecurityAction.Demand,
     Level = AspNetHostingPermissionLevel.Minimal),
 AspNetHostingPermission(SecurityAction.InheritanceDemand,
     Level = AspNetHostingPermissionLevel.Minimal),
 ToolboxData("<{0}:FormView runat=server></{0}:FormView>")]
    public class FormView : WebControl
    {
        public Form Form
        {
            get;
            set;
        }

        public IDictionary<string, object> Values
        {
            get;
            set;
        }

        private int showPerRow = 1;
        /// <summary>
        /// 一行包含的控件数
        /// </summary>
        public int ShowPerRow
        {
            get
            {
                return showPerRow;
            }
            set
            {
                showPerRow = value;
            }
        }

        private void RenderHeader(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "eFormHeader");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(Form.Title);
            writer.RenderEndTag();
        }

        public string GetValidatorScript(FormField field)
        {
            string script = string.Empty;

            if (field.DataType == DataType.Integer)
            {
                return string.Format("$(\"#{0}\").formValidator().regexValidator({{ regexp: \"intege\", datatype: \"enum\", onerror: \"【{1}】必须是整数！\" }});", field.Name, field.Text);
            }
            else if (field.DataType == DataType.Float)
            {
                return string.Format("$(\"#{0}\").formValidator().regexValidator({{ regexp: \"decmal\", datatype: \"enum\", onerror: \"【{1}】必须是浮点数！\" }});", field.Name, field.Text);
            }

            return script;
        }

        private void RenderFormView(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, "eForm");
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);

            #region 生成表单
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "eForm");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            StringBuilder validatorScript = new StringBuilder();
            validatorScript.AppendLine("    <script language=\"javascript\" type=\"text/javascript\">");
            validatorScript.AppendLine("        function validator() {");

            if (Form != null && Form.Fields != null && Form.Fields.Count > 0)
            {
                for (int i = 0; i < Form.Fields.Count; i++)
                {
                    FormField field = Form.Fields[i];

                    validatorScript.AppendLine(GetValidatorScript(field));

                    if (i >= showPerRow && i % showPerRow == 0)
                    {
                        writer.RenderEndTag();
                    }
                    if (i % showPerRow == 0)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    }

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "eForm_lable");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (field.Required)
                    {
                        writer.Write("<em>*</em>");
                    }
                    writer.Write(field.Text + (field.Text.EndsWith("：") ? string.Empty : "："));
                    writer.RenderEndTag();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, field.ControlType == ControlType.SingleCombox ? "eForm_input_combox" : "eForm_input");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.Write(AgileEAP.EForm.ObjectFactory.CreateControl(field).ToHtml(Values));
                    writer.RenderEndTag();
                }

                //补齐tr
                int remain = Form.Fields.Count % showPerRow;
                if (remain > 0)
                {
                    for (int i = 0; i < showPerRow - remain; i++)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "eForm_lable");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.RenderEndTag();

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "eForm_input");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.RenderEndTag();
                    }
                }

                writer.RenderEndTag();
            }

            validatorScript.AppendLine("        }");
            validatorScript.AppendLine("    </script>");

            writer.RenderEndTag();
            #endregion

            //writer.RenderEndTag();

            writer.Write(validatorScript.ToString());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "eFormView");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            RenderHeader(writer);

            RenderFormView(writer);

            writer.RenderEndTag();
        }
    }
}

