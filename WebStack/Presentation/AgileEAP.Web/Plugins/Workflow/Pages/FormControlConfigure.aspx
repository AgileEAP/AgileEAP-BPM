<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    ClientIDMode="Static" CodeBehind="FormControlConfigure.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.FormControlConfigure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
     #eForm
     {
        width:80%;
        height:90%;
        margin:auto;
     }
     .eForm_lable
     {
       text-align:right;
       float:left;
     }
     .eForm_input
     {
     text-align:left;
     }
    </style>
    <script type="text/javascript">
        function save(me) {
            var field = { url: $("#txtURL").val(), rows: $("#txtRows").val(), cols: $("#txtCols").val() };
            window.returnValue = field;
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="eform">
        <div id="divChooseBox" runat="server">
            <div class="eForm_lable">
                链接地址</div>
            <div class="eForm_input">
                <asp:TextBox ID="txtURL" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="divTextBox" runat="server">
            <div class="eForm_lable">
                行高
            </div>
            <div class="eForm_input">
                <asp:TextBox ID="txtRows" runat="server"></asp:TextBox>
            </div>
            <div class="eForm_lable">
                列高
            </div>
            <div class="eForm_input">
                <asp:TextBox ID="txtCols" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
