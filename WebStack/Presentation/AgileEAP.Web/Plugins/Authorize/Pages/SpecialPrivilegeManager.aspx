<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="SpecialPrivilegeManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.SpecialPrivilegeManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=HtmlExtensions.RequireStyles("tab_default") %>
    <%=HtmlExtensions.RequireStyles("jQueryContextMenu_default") %>
    <%=HtmlExtensions.RequireScripts("tab") %>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript" language="javascript">
        function save(me, argument) {
            var selected = $("#hidSelected").val();
            if (selected == 1) {
                var frm = window.frames["ifrMain-0"].save(me, argument);
            }
            else {
                var frm = window.frames["ifrMain-1"].save(me, argument);
            }


        }

        function openSPAdd() {
            var userID = $.query.get("UserID");
            $("#ifrMain-0").attr("src", "SpecialPrivilegeDetail.aspx?AuthFlag=1&UserID=" + userID + "&r=" + Math.random());
        }
        function openSPMinus() {
            var userID = $.query.get("UserID");
            $("#ifrMain-1").attr("src", "SpecialPrivilegeDetail.aspx?AuthFlag=2&UserID=" + userID + "&r=" + Math.random());
        }

        $(function () {
            resizeWindow();
            openSPAdd();
            openSPMinus();
        });

        $(window).resize(function () {
            resizeWindow();
        });

        function selectVMOptionTab(i) {
            switch (i) {
                case 1:
                    document.getElementById("divAdd").className = "tabBg1";
                    document.getElementById("divRemove").className = "tabBg6";
                    $("#tabAdd").show();
                    $("#tabRemove").hide();
                    $("#hidSelected").val(1);
                    break;
                case 2:
                    document.getElementById("divAdd").className = "tabBg4";
                    document.getElementById("divRemove").className = "tabBg7";
                    $("#tabAdd").hide();
                    $("#tabRemove").show();
                    $("#hidSelected").val(2);
                    break;
            }
        }

        function resizeWindow() {
            var windowHeight = getWindowHeight();
            if (windowHeight == 0) return;
            document.getElementById("main_bg2").style.width = document.body.offsetWidth - 10 + "px";
            document.getElementById("main_bg2").style.height = windowHeight - 52 + "px";
            document.getElementById("tabAdd").style.height = document.getElementById("main_bg2").style.height;
            document.getElementById("tabAdd").style.width = document.getElementById("main_bg2").style.width;
            document.getElementById("tabRemove").style.height = document.getElementById("main_bg2").style.height;
            document.getElementById("tabRemove").style.width = document.getElementById("main_bg2").style.width;
        }

    </script>
    <style type="text/css">
        #ctl00_navigation {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div style="padding-left: 7px; height: 21px;">
        <a>
            <div id="divAdd" class="tabBg1" onclick="selectVMOptionTab(1);">
                增加
            </div>
        </a><a>
            <div id="divRemove" class="tabBg6" onclick="selectVMOptionTab(2);">
                删减
            </div>
        </a>
        <input id="hidSelected" type="hidden" value="1" />
    </div>
    <div id="main_bg">
        <div id="main_bg2">
            <div id="tabAdd">
                <iframe id="ifrMain-0" name="ifrMain-0" src="" width="100%" height="100%" frameborder="0"
                    scrolling="auto"></iframe>
            </div>
            <div id="tabRemove">
                <iframe id="ifrMain-1" name="ifrMain-1" src="" width="100%" height="100%" frameborder="0"
                    scrolling="auto"></iframe>
            </div>
        </div>
    </div>
</asp:Content>
