<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="MyWorkItemDetail.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.MyWorkItemDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=HtmlExtensions.RequireStyles("tab_default") %>
    <%=HtmlExtensions.RequireScripts("tab") %>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
        html, body
        {
            height: 100%;
            width: 100%;
            overflow: hidden;
        }

        form
        {
            height: 100%;
        }

        #page_container
        {
            height: 98%;
        }

        #main_bg
        {
            height: 98%;
        }

        #main_bg2
        {
            height: 99%;
            width: 99%;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(function () {
            // switchTab("processChart2");
            switchTab("handle");
            $("#ifHandle").attr("src", "<%= Url %>");

        });

        function rollback() {
            var frm = window.frames["ifHandle"].rollback();
        }

        function save() {
            var frm = window.frames["ifHandle"].save();
        }

        function modifyApply() {
            var frm = window.frames["ifHandle"].modifyApply();
        }

        function abandon() {
            var frm = window.frames["ifHandle"].abandon();
        }

        function submit() {
            window.frames["ifHandle"].submit();
        }

        function reject() {
            window.frames["ifHandle"].reject();
        }

        function manualComplete() {
            window.frames["ifHandle"].manualComplete();
        }
        function systemComplete() {
            window.frames["ifHandle"].systemComplete();
        }

        function cancel() {
            if ($.query.get("Entry") != "StartProcess") {
                window.parent.parent.$("#actionDialog").dialog("close");
            }
            else {
                window.parent.$("#actionDialog").dialog("close");
            }
        }
        function switchTab(curTab) {
            var tabs = $("#tabcontainer").find("li");
            var tabCount = tabs.size();
            var index = tabs.index($("#" + curTab));
            tabs.each(function (i) {
                if (this.id == curTab) {
                    this.className = i == tabCount - 1 ? "tabBg7" : "tabBg1";
                    if (curTab == "processChart2") {
                        $("#tab" + curTab).css({ "visibility": "visible" });
                    }
                    else {
                        $("#tab" + curTab).show();
                    }
                }
                else {
                    if (this.id == "processChart2") {
                        $("#tab" + this.id).css({ "visibility": "hidden" });
                    }
                    else {
                        $("#tab" + this.id).hide();
                    }
                    if (i == 0) {
                        this.className = "tabBg4";
                    }
                    else if (i + 1 == index) {
                        this.className = "tabBg3";
                    }
                    else if (i == index + 1 && i == tabCount - 1) {
                        this.className = "tabBg6";
                    }
                    else if (i == index + 1) {
                        this.className = "tabBg2";
                    } else if (i == tabCount - 1) {
                        this.className = "tabBg5";
                    }
                    else {
                        this.className = "tabBg3";
                    }
                }
            });
        }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <ul id="tabcontainer">
        <li id="handle" onclick="switchTab('handle')">处理</li>
        <li id="processTracking" onclick="switchTab('processTracking')">跟踪</li>
       <%-- <li id="processChart" onclick="switchTab('processChart')">流程图</li>--%>
        <li id="processChart2" onclick="switchTab('processChart2')">流程图</li>
    </ul>
    <div id="main_bg">
        <div id="main_bg2">
            <div id="tabhandle" style="height: 100%;">
                <iframe id="ifHandle" name="ifHandle" class="autoHeight" src="" width="100%" height="95%"
                    frameborder="0" scrolling="no"></iframe>
            </div>
            <div id="tabprocessTracking">
                <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
                    DataKeyNames="ID">
                    <Columns>
                        <asp:TemplateField HeaderText="环节">
                            <ItemTemplate>
                                <%#Eval("DestActInstName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <ItemTemplate>
                                <%# Eval("CurrentState").ToSafeString().Cast<AgileEAP.Workflow.Enums.WorkItemStatus>().GetRemark()%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="处理部门">
                            <ItemTemplate>
                                <%#GetOrgNameByUserID(Eval("Executor").ToString()).Name%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="处理人">
                            <ItemTemplate>
                                <%#Eval("ExecutorName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="处理时间">
                            <ItemTemplate>
                                <%#Eval("ExecuteTime")==null||Eval("ExecuteTime").ToString()==""?((DateTime)Eval("TransTime")).ToString("yyyy-MM-dd HH:mm:ss"):((DateTime)Eval("ExecuteTime")).ToString("yyyy-MM-dd HH:mm:ss")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </agile:PagedGridView>
            </div>
          <%--   <div id="tabprocessChart" style="height: 90%; display: none">
                <iframe id="ifProcessTracking" name="ifProcessTracking" class="autoHeight" src="ProcessChart.aspx?processDefID=<%=ProcessDefID %>&ProcessInstID=<%=ProcessInstID %>"
                    width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
            </div>--%>
            <div id="tabprocessChart2" style="height: 90%;">
                <iframe id="ifProcessTracking2" name="ifProcessTracking" class="autoHeight" src="Design/DrawWorkflow?processDefID=<%=ProcessDefID %>&ProcessInstID=<%=ProcessInstID %>"
                    width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
            </div>
            </div>
        </div>
</asp:Content>
