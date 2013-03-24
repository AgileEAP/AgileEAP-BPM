<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="OperatorTree.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.OperatorTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div style="overflow: auto; height: 100%;">
        <agile:AjaxTree ID="AjaxTree1" Runat="server" />
    </div>
    <script type="text/javascript" language="javascript">

        //返回选择对象
        function chooseConfirm() {
            var item = getCurrentNode();
            var retValue = new Object();
            retValue.text = item.text;
            retValue.value = item.value;
            retValue.tag = item.tag;
            window.returnValue = retValue;
            window.close();
        }
    </script>
</asp:Content>
