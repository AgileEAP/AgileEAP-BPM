<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="OperatorManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.OperatorManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script language="javascript" type="text/javascript">
        function setRole(me, argument) {
            var frm = window.frames["ifrMain"].setRole(me, argument);

        }

        function setSpecialPrivilege(me, argument) {
            var frm = window.frames["ifrMain"].setSpecialPrivilege(me, argument);


        }

        $(window).resize(function () {
            resizeWindow();
        });

        $(function () {
            resizeWindow();
        });

        function resizeWindow() {
            var $divContent = $("#divContent");
            var $treeContainer = $("#treeContainer");
            var $resourceContainer = $("#resourceContainer");
            var oWidth = document.body.offsetWidth;
            var oHeight = getWindowHeight() - 30;
            var treeWidth = oWidth * 0.24;
            $divContent.width(oWidth);
            $divContent.height(oHeight);
            $treeContainer.width(treeWidth);
            $resourceContainer.width(oWidth - treeWidth - 1);
            $treeContainer.height(oHeight);
            $resourceContainer.height(oHeight);
        }
    </script>
    <div id="divContent">
        <div id="treeContainer" style="float: left; border-collapse: collapse; border-top: 1px #bfbfbf solid;
            border-right: 1px #bfbfbf solid;">
            <iframe id="ifrTree" name="ifrTree" src="orgtree.aspx" width="100%" height="100%"
                frameborder="0" scrolling="no"></iframe>
        </div>
        <div id="resourceContainer" style="float: left; margin-left: 0px; border-collapse: collapse;
            border-top: 1px #bfbfbf solid;">
            <iframe id="ifrMain" name="ifrMain" src="OrgUserList.aspx?orgid=<%=Configure.Get("OrgRootID") %>"
                width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
        </div>
    </div>
</asp:Content>
