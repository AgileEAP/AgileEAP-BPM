<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="RolePrivilegeManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.RolePrivilegeManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">

        //校验脚本

        function save() {
            var priveleges = getCheckedNodeIds();

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: priveleges }, function (value) {
                var ajaxResult = JSON2.parse(value);
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null && ajaxResult.PromptMsg != "") {
                        alert(ajaxResult.PromptMsg);

                        if (window.parent)
                            window.parent.closeDialog();
                    }
                }
                else {
                    alert("系统出错，请与管理员联系！");
                }
            });
        }
    </script>
        <agile:AjaxTree ID="AjaxTree1" Runat="server" />
</asp:Content>
