
<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ObjectRoleManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.ObjectRoleManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	
	
	<%if (false)
      { %>
		<script   src="../Scripts/jquery-vsdoc.js" 
        type="text/javascript"></script> 
    <%}%>	
    <script type="text/javascript">
        function save() {
            var roles = getCheckedNodes();
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: JSON2.stringify(roles) }, function (value) {

                var ajaxResult = JSON2.parse(value);
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null && ajaxResult.PromptMsg != "") {
                        alert(ajaxResult.PromptMsg);
                        if (window.parent)
                            window.parent.closeDialog();
                    }
                }
                else {
                    alert("保存出错，请与管理员联系！");
                }
            });
        }

      
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
  
  <agile:AjaxTree ID="AjaxTree1" Runat="server" />

    </div>
</asp:Content>


