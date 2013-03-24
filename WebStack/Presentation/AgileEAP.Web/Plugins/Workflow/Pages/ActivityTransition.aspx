<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ActivityTransition.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.ActivityTransition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=HtmlExtensions.RequireStyles("tab_default") %>
    <%=HtmlExtensions.RequireScripts("tab") %>

    <script language="javascript" type="text/javascript">

        function save(me, argument) {
            var transition = { IsDefault: $("#chkIsDefault").attr("checked") == "checked", Name: $("#txtName").val(), Expression: $("#txtExpression").val() };
            var value = JSON2.stringify(transition);
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, ProcessDefID: $.query.get("ProcessDefID"), TransitionID: $.query.get("TransitionID") }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("操作成功！");
                    window.parent.closeDialog(0, "actionDialog2");
                }
                else {
                    alert("操作失败");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <ul id="tabcontainer">
        <li id="transition" class="tabBg7">基本</li>
    </ul>
    <div id="main_bg" style="height: 395px; width: 100%;">
        <div id="main_bg2" style="height: 390px">
            <table id="tblTransition" style="width: 80%">
                <tr>
                    <td style="text-align: right; width: 30%">
                        名称
                    </td>
                    <td style="text-align: left; width: 70%">
                        <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 30%">
                        默认连接
                    </td>
                    <td style="text-align: left; width: 70%">
                        <asp:CheckBox ID="chkIsDefault" runat="server" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 30%; vertical-align: top;">
                        条件表达式
                    </td>
                    <td style="text-align: left; width: 70%">
                        <asp:TextBox ID="txtExpression" runat="server" ClientIDMode="Static" TextMode="MultiLine"
                            Rows="6" Columns="300" Width="350px" />
                        <%--                        <textarea id="txtExpression" rows="6" cols="350px"></textarea>--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
