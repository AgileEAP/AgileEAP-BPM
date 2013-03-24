@inherits AgileEAP.MVC.ViewEngines.Razor.WebViewPage
@using AgileEAP.MVC
@using AgileEAP.Core
@using AgileEAP.Core.Extensions
@using AgileEAP.Workflow.Enums
@using AgileEAP.Plugin.Workflow.Models
@using Kendo.Mvc.UI
@using AgileEAP.Plugin.Workflow;
@{
    Script.Require("tab").AtHead();
    Style.Require("tab_" + Theme);
    Style.Require("site_" + Theme);
    Script.Require("site").AtHead();
    var Url = ViewData["Url"];
    var ProcessDefID = ViewData["ProcessDefID"];
    var ProcessInstID = ViewData["ProcessInstID"];
    var workflowTransitions = ViewData["workflowTransitions"] as IList<WorkflowTransitionModel>;
}
<!DOCTYPE html>
<html>
<head>
    <title>@Version.eClientName</title>
    @Html.Metas()
    @Html.HeadCss()
    @Html.HeadScripts()
</head>
<style type="text/css">
    html, body, form
    {
        height: 100%;
        width: 100%;
        overflow: hidden;
    }

    form
    {
        background: white;
    }

    #tabcontainer
    {
        margin: 0px;
        padding-left: 7px;
        background-color: #E4E7EC;
        height: 21px;
        border-bottom: 3px solid #368DE8;
    }
    td
    {
     border-bottom: 1px solid #ccc;
     border-right: 1px solid #ccc;   
    }
</style>
<script type="text/javascript">
    $(function () {
        switchTab("handle");
        $("#ifHandle").attr("src", '@Html.Raw(Url)');
        $("#ifProcessTracking").attr("src", '/Workflow/Process/Tracking?processDefID=@Html.Raw(ProcessDefID)&ProcessInstID=@Html.Raw(ProcessInstID)');
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
                if (curTab == "processChart") {
                    $("#tab" + curTab).css({ "visibility": "visible" });
                }
                else {
                    $("#tab" + curTab).show();
                }
            }
            else {
                if (this.id == "processChart") {
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
<body>
    @Html.BuilderToolbar()
    <form>
    <ul id="tabcontainer">
        <li id="handle" onclick="switchTab('handle')">处理</li>
        <li id="processTracking" onclick="switchTab('processTracking')">跟踪</li>
        <li id="processChart" onclick="switchTab('processChart')">流程图</li>
    </ul>
    @*   <div id="main_bg">*@
    <div id="tabhandle" style="height: 100%;">
        <iframe id="ifHandle" name="ifHandle" class="autoHeight" src="" width="100%" height="100%"
            frameborder="0" scrolling="no"></iframe>
    </div>
    <div id="tabprocessTracking" style="height: 100%">
        <table style="width: 100%; text-align: center">
            <thead>
                <tr>
                    <th>环节</th>
                    <th>状态</th>
                    <th>处理部门</th>
                    <th>处理人</th>
                    <th>处理时间</th>
                </tr>
            </thead>
            <tbody>
                @{foreach (var workflowTransition in workflowTransitions)
                  {
                    <tr>
                        <td>@workflowTransition.DestActInstName</td>
                        <td>
                            @(workflowTransition.CurrentState.Cast<WorkItemStatus>(WorkItemStatus.WaitExecute).GetRemark())
                        </td>
                        <td>
                            @workflowTransition.OrgName
                        </td>
                        <td>@workflowTransition.ExecutorName</td>
                        <td>@workflowTransition.ExecuteTime</td>
                    </tr>
                  }}
            </tbody>
        </table>
    </div>
    <div id="tabprocessChart" style="height: 100%;">
        <iframe id="ifProcessTracking" name="ifProcessTracking" class="autoHeight" width="100%"
            height="100%" frameborder="0" scrolling="no"></iframe>
    </div>
    @*</div>*@
    </form>
</body>
</html>
