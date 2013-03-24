<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ChooseDict.aspx.cs" Inherits="AgileEAP.Administration.ChooseDict" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
        .ajaxTree-container
        {
            position: absolute;
            left: -18px;
            overflow: auto;
            height: 100%;
            width: 110%;
        }
        html, body, form
        {
            margin: 0px;
            height: 100%;
            width: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function chooseConfirm() {
            var currentNode = AjaxTree1.getCurrentNode();
            window.returnValue = currentNode.value; ;
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <agile:TreeView ID="AjaxTree1" Runat="server" />
</asp:Content>
