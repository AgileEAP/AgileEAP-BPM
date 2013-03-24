<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="OrgTreeForChoose.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.OrgTreeForChoose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%=HtmlExtensions.RequireStyles("jQueryContextMenu_default") %>
    <%=HtmlExtensions.RequireScripts("jQueryContextMenu") %>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript">
        $(document).ready(function () {
            //默认展开一级菜单
            expandNode('0', '1');
        });
    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <agile:AjaxTree ID="AjaxTree1" Runat="server" />
    <script language="javascript" type="text/javascript">
        //返回选择对象
        function save() {
            var item = getCurrentNode();
            var retValue = new Object();
            retValue.text = item.text;
            retValue.value = item.id;
            window.returnValue = retValue;
            window.close();
        }
        function exit()
        {
            window.close();
        }
    </script>
</asp:Content>
