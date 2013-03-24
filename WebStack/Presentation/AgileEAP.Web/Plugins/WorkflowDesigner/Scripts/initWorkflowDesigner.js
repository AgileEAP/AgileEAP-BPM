
(function (window, undefined) {
    var selectactivity = new Object();
    var connectionLabel = "";
    var lineStyle = "Straight";
    var ProcessDefine = function () {
        this.Version = "1.0";
        this.Name = "ProcessDefine";
        this.Author = "yy";
        this.Transitions = [];
        this.Activities = [];
    };
    var processDefine = new ProcessDefine();
    jsPlumb.importDefaults({
        Connector: ["Straight"],
        ConnectionOverlays: [
					["Arrow", { location: 1, foldback: 0.1, width: 10, direction: -1, length: 5 }],
					["Label", {
					    location: 0.1,
					    id: "label",
					    cssClass: "aLabel"
					}]
        ],
        ConnectorZIndex: 50
    });
    document.oncontextmenu = function (e) { return false };
    var connectorPaintStyle = {
        lineWidth: 2,
        strokeStyle: "Green",
        joinstyle: "round",
        outlineColor: "white",
        outlineWidth: 2
    };
    var connectorHoverStyle = {
        lineWidth: 7,
        strokeStyle: "#2e2aF8"
    };
    var exampleGreyEndpointOptions = {
        isSource: true,
        isTarget: true,
        DragOptions: { cursor: 'pointer', zIndex: 2000 },
        Endpoints: [["Dot", { radius: 10 }], ["Dot", { radius: 10 }]],
        EndpointStyles: [{ fillStyle: "#2e2aF8" }, { fillStyle: "#2e2aF8" }],
        connector: [lineStyle, { stub: [40, 60], gap: 10 }],
        paintStyle: { fillStyle: "#2e2aF8", radius: 10 },
        connectorStyle: connectorPaintStyle,
        hoverPaintStyle: connectorHoverStyle,
        maxConnections: 5,
        scope: "process",
        connectorHoverStyle: connectorHoverStyle
    };
    function init() {
        $("#sel_zoomscale").change(function () {
            var zoomscale = $("#sel_zoomscale").val();
            zoomscale = zoomscale.substring(0, zoomscale.indexOf("%")) / 100;
            $(".designeractivity").each(function () {
                $(this).css("width", $(this).width() * zoomscale + "px");
                $(this).css("height", $(this).height() * zoomscale + "px");
                $(this).find("img").css("width", $(this).find("img").width() * zoomscale + "px");
                $(this).find("img").css("height", $(this).find("img").height() * zoomscale + "px");
                $(this).find("label").css("top", $(this).find("img").height() + "px");
                jsPlumb.repaintEverything();
            });
        });
     
        $("#contexmenu_del").click(function () {
            if (selectactivity != null) {
                jsPlumb.removeAllEndpoints(selectactivity.id);
                $(selectactivity).remove();
            }

        });
        $(".menuedit").mouseleave(function () {
            $(this).hide();
        });
        $("#linestyle").mouseleave(function () {
            $("#linestyle").hide();
        });
        $(".line").click(function () {
            switch (this.id) {
                case "linestyle_straight": lineStyle = "Straight";break;
                case "linestyle_flowchart": lineStyle = "Flowchart"; break;
                case "linestyle_bezier": lineStyle = "Bezier"; break;
            };
            $("#linestyle").hide();
        });
        $("#toolboxshow").click(function () {
            if ($("#toolbox_content").css("display") == "block") {
                $("#toolbox_content").hide();
                $("#toolboxshow").attr("src", "../../Plugins/WorkflowDesigner/Content/Themes/Default/images/hide.png");
            }
            else {
                $("#toolbox_content").show();
                $("#toolboxshow").attr("src", "../../Plugins/WorkflowDesigner/Content/Themes/Default/images/Show.jpg");

            }

        });
    }
    function initToolbar() {
        $(".toolbar").click(function () {
            switch (this.id) {
                case "header_toolbar_new":
                    if (confirm("是否保存该流程图")) {
                        saveProcessDefine();
                    }
                    processDefine.ID = null;
                    jsPlumb.reset();
                    $(".designeractivity").each(function () {
                        $(this).remove();
                    });
                    break;
                case "header_toolbar_open": break;
                case "header_toolbar_save":
                    saveProcessDefine();
                    break;
                case "header_toolbar_back": break;
                case "header_toolbar_redo": break;
                case "header_toolbar_zoomenlarge":
                    $(".designeractivity").each(function () {
                        if ($(this).width() < 200) {
                            $(this).css("width", $(this).width() * 2 + "px");
                            $(this).css("height", $(this).height() * 2 + "px");
                            $(this).find("img").css("width", $(this).find("img").width() * 2 + "px");
                            $(this).find("img").css("height", $(this).find("img").height() * 2 + "px");
                            $(this).find("label").css("top", $(this).find("img").height() + "px");
                          
                        }
                    });
                    jsPlumb.repaintEverything();
                    break;
                case "header_toolbar_zoomdecrease":
                    $(".designeractivity").each(function () {
                        if ($(this).width() > 20) {
                            $(this).css("width", $(this).width() * 0.5 + "px");
                            $(this).css("height", $(this).height() * 0.5 + "px");
                            $(this).find("img").css("width", $(this).find("img").width() * 0.5 + "px");
                            $(this).find("img").css("height", $(this).find("img").height() * 0.5 + "px");
                            $(this).find("label").css("top", $(this).find("img").height() + "px");
                           
                        }
                    });
                    jsPlumb.repaintEverything();
                    break;
                case "header_toolbar_drawLine":
                    e = window.event || arguments[0];
                    var pointx = (e.clientX + $(document).find("#designer_content").scrollLeft()) || e.offsetX;
                    var pointy = (e.clientY + $(document).find("#designer_content").scrollTop()) || e.offsetY;
                    $("#linestyle").css("left", pointx);
                    $("#linestyle").css("top", pointy);
                    $("#linestyle").show();
                    break;
            };
        });
    }
    function initMenu() {
        $(".menu").click(function () {
            e = window.event || arguments[0];
            var pointx = (e.clientX + $(document).find("#designer_content").scrollLeft()) || e.offsetX;
            var pointy = (e.clientY + $(document).find("#designer_content").scrollTop()) || e.offsetY;
            $("#" + $(this).attr("id") + "_list").css("left", pointx);
            $("#" + $(this).attr("id") + "_list").css("top", pointy);
            $("#" + $(this).attr("id") + "_list").show();
        });
    }
    function initView() {
        $(".viewmenu").click(function () {
            switch (this.id) {
                case "viewmenu_toolbar":
                    if ($("#toolbars").is(":hidden")) {
                        $("#toolbars").show();
                    }
                    else {
                        $("#toolbars").hide();
                    }
                    resizeWindow();
                    break;
                case "viewmenu_toolbox":
                    if ($("#main_leftcontent").is(":hidden")) {
                        //  $("#main_designercontent").css("clear", "both");
                        $("#main_leftcontent").show();
                    }
                    else {
                        $("#main_leftcontent").hide();
                        //$("#main_designercontent").css("clear", "none");
                    }
                    resizeWindow();
                    break;
                case "viewmenu_designer": if ($("#main_designercontent").is(":hidden")) {
                    $("#main_designercontent").show();
                }
                else {
                    $("#main_designercontent").hide();
                }
                    resizeWindow();
                    break;
                case "editmenu_attribute": if ($("#main_attributecontent").is(":hidden")) {
                    $("#main_attributecontent").show();
                }
                else {
                    $("#main_attributecontent").hide();
                }
                    resizeWindow();
                    break;
            };
        });
    }
    function initProcessDefine(proDefine) {
        processDefine.Activities = proDefine.Activities;
        var connections = jsPlumb.getConnections({ scope: "process" });
        for (var j = 0; j < connections.length; j++) {
            for (var i = 0; i < processDefine.Transitions.length; i++) {
                if (processDefine.Transitions[i].SourceOrientation == connections[j].endpoints[0].anchor.type && processDefine.Transitions[i].SinkOrientation == connections[j].endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == connections[j].sourceId && processDefine.Transitions[i].DestActivity == connections[j].targetId) {
                    connectionLabel = connections[j];
                    setConnectionLabel(processDefine.Transitions[i].Name);
                }
            }
        }
    }
    function saveProcessDefine() {
        var checkActivities = new Array();
        for (var j = 0; j < processDefine.Activities.length; j++) {
            if (processDefine.Activities[j].NewID) {
                for (var i = 0; i < checkActivities.length; i++) {
                    if (processDefine.Activities[j].NewID == checkActivities[i]) {
                        alert("活动存在相同ID,相同ID为" + processDefine.Activities[j].NewID + "，请检查后在保存!");
                        return false;
                    }
                }
                checkActivities.push(processDefine.Activities[j].NewID);
            }
            else {
                for (var i = 0; i < checkActivities.length; i++) {
                    if (processDefine.Activities[j].ID == checkActivities[i]) {
                        alert("活动存在相同ID,相同ID为" + processDefine.Activities[j].ID + "，请检查后在保存!");
                        return false;
                    }
                }
                checkActivities.push(processDefine.Activities[j].ID);
            }

        }


        for (var i = 0; i < processDefine.Transitions.length; i++) {
            for (var j = 0; j < processDefine.Activities.length; j++) {
                if (processDefine.Transitions[i].SrcActivity == processDefine.Activities[j].ID && processDefine.Activities[j].NewID) {
                    processDefine.Transitions[i].SrcActivity = processDefine.Activities[j].NewID;
                }
                if (processDefine.Transitions[i].DestActivity == processDefine.Activities[j].ID && processDefine.Activities[j].NewID) {
                    processDefine.Transitions[i].DestActivity = processDefine.Activities[j].NewID;
                }
            }
        }
        $(processDefine.Activities).each(function () {
            var me = this;
            $(".designeractivity").each(function () {
                if (this.id == me.ID) {
                    me.Style.Left = $(this).offset().left - $("#main_leftcontent").width();
                    me.Style.Top = $(this).offset().top - $("#header").height() - $("#tabcontainer").height() - $("#designer_title").height();
                    me.Style.Width = $(this).width();
                    me.Style.Height = $(this).height();
                    me.Style.ZIndex = 0;
                }
            });
        });
        for (var j = 0; j < processDefine.Activities.length; j++) {
            if (processDefine.Activities[j].NewID) {
                processDefine.Activities[j].ID = processDefine.Activities[j].NewID;
            }
        }
        var processDefContent = JSON2.stringify(processDefine);
        var Name = $.trim($("#processName").val());
        var text = $.trim($("#processText").val());
        var version = $.trim($("#version").val());
        var description = $.trim($("#description").val());
        var startor = $.trim($("#startor").val());
        var isActive = $.trim($("#isActive").val());
        var currentFlag = $.trim($("#currentFlag").val());
        var currentStatus = $.trim($("#currentStatus").val());
        var categoryID = $.trim($("#categoryID").val());
        $.post("/WorkflowDesigner/Workflow/SaveProcessDefine", { categoryID: categoryID, processDefContent: processDefContent, Name: Name, text: text, version: version, description: description, startor: startor, isActive: isActive, currentFlag: currentFlag, currentStatus: currentStatus }, function (value) {
            var ajaxResult = value;
            var message = "操作失败";
            if (ajaxResult && ajaxResult.Result == 1) {
                message = "操作成功";
                alert(message);
            }
        });
    }
    function startdraw(container) {
        $(".ativity").draggable({
            helper: "clone"
        });
        $("#" + container).droppable({
            drop: function (event, ui) {
                if (ui.draggable[0].className.indexOf("ativity ui-draggable") >= 0) {
                    var text = $.trim($(ui.draggable[0]).text());
                    var id = $.trim($(ui.draggable[0]).attr("id")) + new Date().getTime();
                    if ($.trim($(ui.draggable[0]).attr("id")) == "StartActivity" && $("#" + container).find("div[activitytype='StartActivity']")[0] != undefined) {
                        alert("流程图只能有一个开始活动");
                        return false;
                    }
                    else if ($.trim($(ui.draggable[0]).attr("id")) == "EndActivity" && $("#" + container).find("div[activitytype='EndActivity']")[0] != undefined) {
                        alert("流程图只能有一个结束活动");
                        return false;
                    }
                    else {
                        var img = $(ui.draggable[0]).children()[0];
                        var pointx = (event.clientX + $(document).find("#" + container).scrollLeft() - 20) || event.offsetX;
                        var pointy = (event.clientY + $(document).find("#" + container).scrollTop() - 20) || event.offsetY;
                        var activityResource = "<div id=\"" + id + "\"  class=\"designeractivity\" name=\"" + text + "\" ActivityType=\"" + $.trim($(ui.draggable[0]).attr("id")) + "\" style=\"left:" + pointx + "px;top:" + pointy + "px;width:40px;height:40px;position:absolute\"><img style=\"width:40px;height:40px;\" src=\"" + $(img).attr("src") + "\" /><label style=\"width:100px;position:absolute;Top:40px;left:-30px\">" + text + "</label></div>"
                        $("#" + container).append(activityResource);
                        $("#" + id).bind("mouseover", function () { jsPlumb.show(id, $("#" + id)); });
                        $("#" + id).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/Workflow/Design/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + id + "&ActivityType=" + $.trim($(ui.draggable[0]).attr("id")), 850, 580, true); });
                        $("#" + id).bind("contextmenu", function () {
                            selectactivity = $("#" + id)[0];
                            e = window.event || arguments[0];
                            var pointx = (e.clientX + $(document).find("#" + container).scrollLeft()) || e.offsetX;
                            var pointy = (e.clientY + $(document).find("#" + container).scrollTop()) || e.offsetY;
                            $("#contexmenu").css("left", pointx);
                            $("#contexmenu").css("top", pointy);
                            $("#contexmenu").show();
                        });
                        var activity = { Name: text, ID: id, ActivityType: $.trim($(ui.draggable[0]).attr("id")), Style: {} };
                        processDefine.Activities.push(activity);
                        var exampleGreyEndpointOptions = {
                            isSource: true,
                            isTarget: true,
                            DragOptions: { cursor: 'pointer', zIndex: 2000 },
                            Endpoints: [["Dot", { radius: 10 }], ["Dot", { radius: 10 }]],
                            EndpointStyles: [{ fillStyle: "#2e2aF8" }, { fillStyle: "#2e2aF8" }],
                            connector: [lineStyle, { stub: [40, 60], gap: 10 }],
                            paintStyle: { fillStyle: "#2e2aF8", radius: 10 },
                            connectorStyle: connectorPaintStyle,
                            hoverPaintStyle: connectorHoverStyle,
                            maxConnections: 5,
                            scope: "process",
                            connectorHoverStyle: connectorHoverStyle
                        };
                        jsPlumb.addEndpoint(id, exampleGreyEndpointOptions, { anchor: "BottomCenter" });
                        jsPlumb.addEndpoint(id, exampleGreyEndpointOptions, { anchor: "TopCenter" });
                        jsPlumb.addEndpoint(id, exampleGreyEndpointOptions, { anchor: "LeftMiddle" });
                        jsPlumb.addEndpoint(id, exampleGreyEndpointOptions, { anchor: "RightMiddle" });
                        jsPlumb.draggable(jsPlumb.getSelector($("#" + id)), { containment: $("#" + container) });
                        $(".designeractivity").each(function () {
                            jsPlumb.hide(this.id, true);
                        });
                        $(".designeractivity").each(function () {
                            jsPlumb.show(this.id);
                        });
                    }
                }
            }
        });
        init();
        initMenu();
        initToolbar();
        initView();
    }
    function drawActivityInst(activity, currentState, container) {
        var imgPath = "/Plugins/WorkflowDesigner/Content/Themes/Default/images/";
        var resource = "/Plugins/WorkflowDesigner/Content/Themes/Default/images/" + activity.ActivityType + ".png";
        switch (currentState) {
            case -1: resource = imgPath + activity.ActivityType + "4.png";
                break;
            case 0: resource = imgPath + activity.ActivityType + ".png";
                break;
            case 1: resource = imgPath + activity.ActivityType + "2.png";
                break;
            case 2: break;
            case 3: resource = imgPath + activity.ActivityType + "2.png";
                break;
            case 4: resource = imgPath + activity.ActivityType + "3.png";
                break;
            case 5: resource = imgPath + activity.ActivityType + "5.png";
                break;
            case 8: resource = imgPath + activity.ActivityType + "6.png";
                break;
        }
        if ($("#" + container).find($("#" + activity.ID))[0] != undefined) {
            $("#" + container).find($("#" + activity.ID)).remove();
        }
        var left = activity.Style.Left + $("#main_leftcontent").width();
        var height = activity.Style.Top + $("#header").height() + $("#tabcontainer").height() + $("#designer_title").height();
        var activityResource = "<div id=\"" + activity.ID + "\"  class=\"designeractivity\" name=\"" + activity.Name + "\" ActivityType=\"" + activity.ActivityType + "\"  style=\"left:" + left + "px;top:" + height + "px;width:40px;height:40px;position:absolute\"><img style=\"width:40px;height:40px;\" src=\"" + resource + "\" /><label style=\"width:100px;position:absolute;Top:40px;left:-30px\">" + activity.Name + "</label></div>"
        $("#" + container).append(activityResource);
        $("#" + activity.ID).bind("mouseover", function () { jsPlumb.show(activity.ID, $("#" + activity.ID)); });
        $("#" + activity.ID).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/Workflow/Design/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + activity.ID + "&ActivityType=" + $(activity).attr("ActivityType"), 850, 580, true); });
        $("#" + activity.ID).bind("contextmenu", function () {
            selectactivity = $("#" + activity.ID)[0];
            e = window.event || arguments[0];
            var pointx = (e.clientX + $(document).find("#" + container).scrollLeft()) || e.offsetX;
            var pointy = (e.clientY + $(document).find("#" + container).scrollTop()) || e.offsetY;
            $("#contexmenu").css("left", pointx);
            $("#contexmenu").css("top", pointy);
            $("#contexmenu").show();
        });
    }
    function drawConnection(transition, activityInstList, transitionControlList, container) {
        var currentStatus = 0;
        var status = 0;
        if (activityInstList != null && activityInstList.length > 0) {
            for (var i = 0; i < activityInstList.length; i++) {
                if (transition.DestActivity == activityInstList[i].ActivityDefID)//查找目标
                {
                    switch (activityInstList[i].CurrentState) {
                        case -1: currentStatus = -1; break;//未运行到
                        case 0: currentStatus = 0; break;//初始状态
                        case 1: currentStatus = 1; break;//将运行
                        case 2: currentStatus = 2; break;//运行出错
                        case 3: currentStatus = 3; break;// 挂起
                        case 4: currentStatus = 4; break;//已运行
                        case 5: currentStatus = 5; break;//回退
                    }
                    for (var j = 0; j < activityInstList.length; j++) {
                        if (transition.SrcActivity == activityInstList[j].ActivityDefID && activityInstList[i].CurrentState == 4) {
                            for (var n = 0; n < transitionControlList.length; n++) {
                                if (transitionControlList[n].DestActID == activityInstList[i].ActivityDefID && transitionControlList[n].SrcActID == transition.SrcActivity) {
                                    switch (activityInstList[j].CurrentState) {
                                        case -1: currentStatus = -1; break;
                                        case 0: currentStatus = -1; break;
                                        case 3: currentStatus = -1; break;
                                        case 4: currentStatus = 4; break;
                                        case 5: currentStatus = 0; break;
                                    }
                                    break;
                                }
                                else {
                                    currentStatus = -1;
                                }
                            }
                            status = 1;
                        }

                        if (transition.SrcActivity == activityInstList[j].ActivityDefID && currentStatus != 4) {
                            for (var m = 0; m < transitionControlList.length; m++) {
                                if (transitionControlList[m].DestActID == activityInstList[i].ActivityDefID && transitionControlList[m].SrcActID == transition.SrcActivity) {

                                    if (currentStatus == 5) {
                                        for (var k = 0; k < transitionControlList.length; k++) {
                                            if (transitionControlList[k].DestActID == transition.SrcActivity && transitionControlList[k].SrcActID == activityInstList[i].ActivityDefID) {
                                                currentStatus = 5;
                                                break;
                                            }
                                            else {
                                                currentStatus = 0;
                                            }
                                        }

                                    }
                                    else {
                                        if (currentStatus == -1)
                                            currentStatus = 1;
                                        if (currentStatus == 3)
                                            currentStatus = 1;


                                    }
                                    break;

                                }
                                else {
                                    if (currentStatus != 5 && currentStatus != 0 && currentStatus != 4) {
                                        currentStatus = -1;
                                    }

                                }
                            }

                            status = 1;
                        }
                    }
                    if (status != 1 && currentStatus != 5) {
                        currentStatus = -1;
                        drawActivityInst(transition.SrcActivity, -1);
                    }
                    else if (status != 1 && currentStatus == 5) {
                        currentStatus = 0;
                    }

                }
                if (transition.SrcActivity == activityInstList[i].ActivityDefID) {
                    for (var p = 0; p < activityInstList.length; p++) {

                        if (currentStatus != 1 && currentStatus != 4 && activityInstList[i].CurrentState == 4 || activityInstList[p].CurrentState == -1) {
                            currentStatus = -1;
                        }
                    }
                }
            }
        }
        else {
            currentStatus = 0;
        }

        var connectionStyle = "Green";
        switch (currentStatus) {
            case -1: connectionStyle = "Yellow"; break;
            case 0: connectionStyle = "Green"; break;
            case 1: connectionStyle = "Red"; break;
            case 2:; break;
            case 4: connectionStyle = "Gray"; break;
            case 3:; break;
            case 5: connectionStyle = "Blue"; break;
        }
        var sourceAnchor = transition.SourceOrientation;
        var sinkAnchor = transition.SinkOrientation;
        switch (transition.SourceOrientation) {
            case "Left": sourceAnchor = "LeftMiddle"; break;
            case "Top": sourceAnchor = "TopCenter"; break;
            case "Right": sourceAnchor = "RightMiddle"; break;
            case "Bottom": sourceAnchor = "BottomCenter"; break;
        }
        switch (transition.SinkOrientation) {
            case "Left": sinkAnchor = "LeftMiddle"; break;
            case "Top": sinkAnchor = "TopCenter"; break;
            case "Right": sinkAnchor = "RightMiddle"; break;
            case "Bottom": sinkAnchor = "BottomCenter"; break;
        }

        jsPlumb.addEndpoint(transition.SrcActivity, exampleGreyEndpointOptions, { anchor: "BottomCenter" });
        jsPlumb.addEndpoint(transition.SrcActivity, exampleGreyEndpointOptions, { anchor: "TopCenter" });
        jsPlumb.addEndpoint(transition.SrcActivity, exampleGreyEndpointOptions, { anchor: "LeftMiddle" });
        jsPlumb.addEndpoint(transition.SrcActivity, exampleGreyEndpointOptions, { anchor: "RightMiddle" });
        jsPlumb.addEndpoint(transition.DestActivity, exampleGreyEndpointOptions, { anchor: "BottomCenter" });
        jsPlumb.addEndpoint(transition.DestActivity, exampleGreyEndpointOptions, { anchor: "TopCenter" });
        jsPlumb.addEndpoint(transition.DestActivity, exampleGreyEndpointOptions, { anchor: "LeftMiddle" });
        jsPlumb.addEndpoint(transition.DestActivity, exampleGreyEndpointOptions, { anchor: "RightMiddle" });
        jsPlumb.connect({
            paintStyle: { fillStyle: connectionStyle, strokeStyle: connectionStyle, lineWidth: 2, radius: 10 },
            source: transition.SrcActivity,
            target: transition.DestActivity,
            anchors: [sourceAnchor, sinkAnchor],
            connector: lineStyle,
            hoverPaintStyle: connectorHoverStyle,
            connectorHoverStyle: connectorHoverStyle,
            endpoints: [["Dot", { radius: 10 }], ["Dot", { radius: 10 }]],
            scope: "process",
            endpointStyles: [{ fillStyle: "#2e2aF8" }, { fillStyle: "#2e2aF8" }]
        });

    }
    var WorkflowDesigner = function () {
        this.me = this;
    }
    WorkflowDesigner.init = function (url, processDefID, processInstID, container, designerDefine, defineUrl) {
        if (processDefID != null && processDefID != "" && processDefID != true) {
            jsPlumb.importDefaults({
                Connector: ["Straight"],
                MaxConnections: 2,
                ConnectionOverlays: [
					["Arrow", { location: 1, foldback: 0.1, width: 10, direction: -1, length: 5 }],
					["Label", {
					    location: 0.1,
					    id: "label",
					    cssClass: "aLabel"
					}]
                ],
                ConnectorZIndex: 50
            });
            if (designerDefine == 1) {
                $.post(defineUrl, { ProcessDefineID: processDefID }, function (value) {
                    var ajaxResult = value;
                    var message = "操作失败";
                    if (ajaxResult && ajaxResult.Result == 1) {
                        $("#processName").val(ajaxResult.RetValue.Name);
                        $("#processText").val(ajaxResult.RetValue.Text);
                        $("#version").val(ajaxResult.RetValue.Version);
                        $("#description").val(ajaxResult.RetValue.Description);
                        $("#startor").val(ajaxResult.RetValue.Startor);
                        $("#isActive").val(ajaxResult.RetValue.IsActive);
                        $("#currentFlag").val(ajaxResult.RetValue.CurrentFlag);
                        $("#currentStatus").val(ajaxResult.RetValue.CurrentState);
                        $("#categoryID").val(ajaxResult.RetValue.CategoryID);
                    }
                    else {
                        return alert("加载失败");
                    }
                });
            }
            $.post(url, { ProcessDefID: processDefID, ProcessInstID: processInstID }, function (value) {
                var ajaxResult = value;
                var message = "操作失败";
                if (ajaxResult && ajaxResult.Result == "Success") {
                    if (ajaxResult.RetValue.processInstID != null && ajaxResult.RetValue.processInstID != "" && ajaxResult.RetValue.processInstID != "null") {
                        if (document.getElementById("leftcontent")) {
                            $("#leftcontent").show();
                        }
                        try {
                            for (var j = 0; ajaxResult.RetValue.processDefine.Activities.length > j; j++) {
                                var i = 0;
                                for (var k = 0; k < ajaxResult.RetValue.activityInsts.length; k++) {
                                    var activityDefID = ajaxResult.RetValue.activityInsts[k].ActivityDefID;
                                    if (activityDefID == ajaxResult.RetValue.processDefine.Activities[j].ID) {
                                        i = 1;
                                        var currentState = ajaxResult.RetValue.activityInsts[k].CurrentState;
                                        drawActivityInst(ajaxResult.RetValue.processDefine.Activities[j], currentState, container);
                                    }
                                }
                                if (i == 0) {
                                    drawActivityInst(ajaxResult.RetValue.processDefine.Activities[j], 0, container);
                                }
                            }
                        }
                        catch (ex) {
                        }
                    }
                    else {
                        for (var q = 0; q < ajaxResult.RetValue.processDefine.Activities.length; q++) {
                            if (document.getElementById("leftcontent")) {
                                $("#leftcontent").hide();
                            }
                            drawActivityInst(ajaxResult.RetValue.processDefine.Activities[q], 0, container);
                        }
                    }
                    if (ajaxResult.RetValue.processDefine.Transitions.length > 0) {
                        for (var j = 0; j < ajaxResult.RetValue.processDefine.Transitions.length; j++) {
                            drawConnection(ajaxResult.RetValue.processDefine.Transitions[j], ajaxResult.RetValue.activityInsts, ajaxResult.RetValue.transList, container);
                        }
                    }
                    jsPlumb.addEndpoint($(".designeractivity"), exampleGreyEndpointOptions, { anchor: "BottomCenter" });
                    jsPlumb.addEndpoint($(".designeractivity"), exampleGreyEndpointOptions, { anchor: "TopCenter" });
                    jsPlumb.addEndpoint($(".designeractivity"), exampleGreyEndpointOptions, { anchor: "LeftMiddle" });
                    jsPlumb.addEndpoint($(".designeractivity"), exampleGreyEndpointOptions, { anchor: "RightMiddle" });
                    $(".designeractivity").each(function () {
                        jsPlumb.hide(this.id, true);
                    });
                    $(".designeractivity").each(function () {
                        jsPlumb.show(this.id);
                    });
                    jsPlumb.draggable(jsPlumb.getSelector(".designeractivity"), { containment: $("#" + container) });
                    initProcessDefine(ajaxResult.RetValue.processDefine);
                }
            });
        }
        else {
            processDefID = "ProcessDefine" + new Date().getTime();
        }
        processDefine.ID = processDefID;
        startdraw(container);
        $(document).click(function () {
            $("#contexmenu").hide();
            $(".designeractivity").each(function () {
                jsPlumb.hide(this.id, true);
            });
            $(".designeractivity").each(function () {
                jsPlumb.show(this.id);
            });
        });
    }
    jsPlumb.bind("contextmenu", function (conn, originalEvent) {
        if (confirm("是否删除该连接")) {
            for (var i = 0; i < processDefine.Transitions.length; i++) {
                if (processDefine.Transitions[i].SourceOrientation == conn.endpoints[0].anchor.type && processDefine.Transitions[i].SinkOrientation == conn.endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == conn.sourceId && processDefine.Transitions[i].DestActivity == conn.targetId) {
                    processDefine.Transitions.splice(i, 1);
                }
            }
            jsPlumb.detach(conn);
        }
    });
    jsPlumb.bind("dblclick", function (conn, originalEvent) {
        var transitionID = "";
        for (var i = 0; i < processDefine.Transitions.length; i++) {
            if (processDefine.Transitions[i].SourceOrientation == conn.endpoints[0].anchor.type && processDefine.Transitions[i].SinkOrientation == conn.endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == conn.sourceId && processDefine.Transitions[i].DestActivity == conn.targetId) {
                transitionID = processDefine.Transitions[i].ID;
                connectionLabel = conn;
            }
        }
        window.parent.parent.openDialog("actionDialog3", '连接线配置', "/Workflow/Design/ConnectionDetail?ProcessDefID=" + processDefine.ID + "&TransitionID=" + transitionID, 850, 580, true);
    });
    jsPlumb.bind("connectionDrag", function (connection) {
        console.log("connection " + connection.id + " is being dragged");
    });
    jsPlumb.bind("connectionDragStop", function (connection) {
        console.log("connection " + connection.id + " was dragged");
    });
    jsPlumb.bind("jsPlumbConnection", function (connetction) {
        if (processDefine.Transitions) {
            processDefine.Transitions.push({ ID: connetction.connection.id, SourcePoint: { X: $("#" + connetction.sourceId).offset().left - $("#main_leftcontent").width(), Y: $("#" + connetction.sourceId).offset().top - $("#header").height() - $("#tabcontainer").height() - $("#designer_title").height(), Z: 0 }, SinkPoint: { X: $("#" + connetction.targetId).offset().left - $("#main_leftcontent").width(), Y: $("#" + connetction.targetId).offset().top - $("#header").height() - $("#tabcontainer").height() - $("#designer_title").height(), Z: 0 }, SourceOrientation: connetction.sourceEndpoint.anchor.type, SinkOrientation: connetction.targetEndpoint.anchor.type, SrcActivity: connetction.sourceId, DestActivity: connetction.targetId, Name: connetction.connection.id, Priority: 3, IsDefault: false, Expression: "" });
        }
    });
    function editActivityName(id, Name) {
        $("#" + id).find("label").text(Name);
    }
    function setConnectionLabel(text) {
        connectionLabel.setLabel(text);
    }
    window.editActivityName = editActivityName;
    window.setConnectionLabel = setConnectionLabel;
    window.WorkflowDesigner = WorkflowDesigner;
    window.processDefine = processDefine;
})(window)