
(function (window, undefined) {
    var selectactivity = new Object();
    var ActivityType=
    {
        StartActivity : "StartActivity",
        ManualActivity :  "ManualActivity",
        RouterActivity :  "RouterActivity",
        SubflowActivity :  "SubflowActivity",
        AutoActivity :  "AutoActivity",
        EndActivity :  "EndActivity",
        ProcessActivity: "ProcessActivity"
    }
    var connectionLabel = "";
    var startConnection = 0;
    var CurrentStatus =
        {
            waitPublish: 0,
            Published: 1,
            stopPublish: 2
        };
    var operate = new Array();//记录操作历史
    var temporaryObject = new Array();
    var temporaryDrop = new Array();
    var lineStyle = "Straight";
    var ProcessDefine = function () {
        this.Version = "1.0";
        this.Name = "ProcessDefine";
        this.Author = "AgileEAP";
        this.Transitions = [];
        this.Activities = [];
    };
    var processDefine = new ProcessDefine();
    jsPlumb.importDefaults({
        Connector: ["Straight"],
        ConnectionOverlays: [
					["Arrow", { location: 1, foldback: 0.1, width: 7, direction: -1, length: 5 }],
					["Label", {
					    location: 0.5,
					    id: "label",
					    cssClass: "aLabel",
					    LabelStyle: "z-index: 21"
					}]
        ],
        ConnectorZIndex: 50
    });
    document.oncontextmenu = function (e) { return false };
    var containerment;
    var connectorPaintStyle = {
        lineWidth: 2,
        strokeStyle: "Green",
        joinstyle: "round",
        outlineColor: "white",
        outlineWidth: 2
    };
    var connectorHoverStyle = {
        lineWidth: 7,
        strokeStyle: "#528E21"
    };
    var exampleGreyEndpointOptions = {
        isSource: true,
        isTarget: true,
        DragOptions: { cursor: 'pointer', zIndex: 2000 },
        Endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
        EndpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }],
        connector: [lineStyle, { stub: [40, 60], gap: 10 }],
        paintStyle: { fillStyle: "#528E21", radius: 7 },
        connectorStyle: connectorPaintStyle,
        hoverPaintStyle: connectorHoverStyle,
        maxConnections: 5,
        scope: "process",
        connectorHoverStyle: connectorHoverStyle
    };
    Array.prototype.indexOf = function (o) {
        var _self = this;
        for (var i = 0; i < _self.length; i++) {
            if (_self[i] == o) {
                return i;
            }
        }
        return -1;
    }
    function delControl(id, cutElement) {
        if (id) {
            jsPlumb.removeAllEndpoints(id);
            var ActivitiesCount = processDefine.Activities.length;
            for (var j = 0; j < ActivitiesCount; j++) {
                if (id == processDefine.Activities[j].ID) {
                    var position = processDefine.Activities.indexOf(processDefine.Activities[j]);
                    processDefine.Activities[j].Style.Left = $("#" + id).offset().left;
                    processDefine.Activities[j].Style.Top = $("#" + id).offset().top;
                    processDefine.Activities[j].Style.Width = $("#" + id).width();
                    processDefine.Activities[j].Style.Height = $("#" + id).height();
                    processDefine.Activities[j].Style.ZIndex = 0;
                    if (operate.length < 10) {
                        operate.push({ bindEvent: "activitycontextmenu", bindObject: processDefine.Activities[j], bind: "designerActivity", result: "delActivity" });
                    }
                    else {
                        operate.shift();
                        operate.push({ bindEvent: "activitycontextmenu", bindObject: processDefine.Activities[j], bind: "designerActivity", result: "delActivity" });
                    }
                    if (cutElement) {
                        selectactivity = processDefine.Activities[j];
                    }
                    processDefine.Activities.splice(position, 1);
                    ActivitiesCount--;
                    j = -1;

                }
            }
            var connectionCount = processDefine.Transitions.length;
            for (var i = 0 ; i < connectionCount; i++) {
                if (processDefine.Transitions[i].SrcActivity == id || processDefine.Transitions[i].DestActivity == id) {
                    var conPosition = processDefine.Transitions.indexOf(processDefine.Transitions[i]);
                    processDefine.Transitions.splice(conPosition, 1);
                    connectionCount--;
                    i = -1;
                }
            }
            if (operate.length < 10) {
                operate.push({ bindEvent: "conncontextmenu", bindObject: processDefine.Transitions[i], bind: "connection", result: "delconnection" });
            }
            else {
                operate.shift();
                operate.push({ bindEvent: "conncontextmenu", bindObject: processDefine.Transitions[i], bind: "connection", result: "delconnection" });
            }
            $("#" + id).remove();
        }
    }
    function cutControl(id) {
        delControl(id, true);
    }
    function copyControl(id) {
        for (var j = 0; j < processDefine.Activities.length; j++) {
            if (id == processDefine.Activities[j].ID) {
                var position = processDefine.Activities.indexOf(processDefine.Activities[j]);
                processDefine.Activities[j].Style.Left = $("#" + id).offset().left;
                processDefine.Activities[j].Style.Top = $("#" + id).offset().top;
                processDefine.Activities[j].Style.Width = $("#" + id).width();
                processDefine.Activities[j].Style.Height = $("#" + id).height();
                processDefine.Activities[j].Style.ZIndex = 0;
                var newID = id + new Date().getTime();
                for (var ActivitieElement in processDefine.Activities[j]) {
                    if (!selectactivity) {
                        selectactivity = new Object();
                    }
                    selectactivity[ActivitieElement] = processDefine.Activities[j][ActivitieElement];
                }
                selectactivity.ID = newID;
            }
        }
    }
    function initContextMenu() {
        $(".designeractivity").contextMenu({
            menu: 'ulcontrol'
        }, function (action, el, pos) {
            var id = $(el.context).attr("id");
            if (action == "delControl") {
                if (confirm("您确定要删除该活动吗？")) {
                    delControl(id);
                }
                return;
            }
            if (action == "cutControl") {
                cutControl(id);
                return;
            }
            if (action == "copyControl") {
                copyControl(id);
                return;
            }
            if (action == "attrControl") {
                var activityType = $("#" + id).attr("activitytype");
                window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + $.query.get("processDefID") + "&ActivityID=" + id + "&ActivityType=" + activityType, 850, 580, true);
                return;
            }
            if (action == "formControl") {
              
                var activityType = $("#" + id).attr("activitytype");
                if (activityType == ActivityType.ManualActivity && processDefine.Activities) {
                    // window.parent.parent.openDialog("actionDialog2", '设计表单', "/FormDesigner/Home/FormDesigner?ProcessDefID=" + $.query.get("processDefID") + "&ActivityID=" + id + "&ActivityType=" + activityType, 850, 580, true);
                    for (var i = 0; i < processDefine.Activities.length; i++) {
                        if (id == processDefine.Activities[i].ID) {
                            processDefine.Activities[i].eForm = processDefine.Activities[i].eForm ? processDefine.Activities[i].eForm : new Date().getTime();

                            openDialog2({ title: "表单设计器", url: "/FormDesigner/Home/FormDesigner?eFormID=" + processDefine.Activities[i].eForm, dialogType: 1, showModal: true, height: screen.height - 80, width: screen.width - 10, windowStyle: { style: "dialogLeft:0;dialogTop:-100;edge: Raised; center: Yes; resizable: Yes; status: no;scrollbars=no;", auguments: window } });
                        }
                    }
                }
                return;
            }
            
        });
    }
    function initDocumentContextMunu() {
        $(document).bind("contextmenu", function () {
            var e = window.event ? window.event : arguments[0];
            var x; var y;
            var o = { menu: "uldocument", inSpeed: 150, outSpeed: 75, position: document };
            var x = (e.clientX + $(document).scrollLeft()) || e.offsetX;
            var y = (e.clientY + $(document).scrollTop()) || e.offsetY;
            $('#' + o.menu).addClass('contextMenu');
            var menu = $('#' + o.menu, o.position);
            $(menu).css({ top: y, left: x }).fadeIn(o.inSpeed);
            $(menu).find('A').mouseover(function () {
                $(menu).find('LI.hover').removeClass('hover');
                $(this).parent().addClass('hover');
            }).mouseout(function () {
                $(menu).find('LI.hover').removeClass('hover');
            });
            $('#' + o.menu, o.position).find('A').unbind('click');
            $('#' + o.menu, o.position).find('LI:not(.disabled) A').click(function () {
                $(document).unbind('click').unbind('keypress');
                $(".contextMenu", o.position).hide();
                var action = $(this).attr('href').substr(1);
                switch (action) {
                    case "pasteControl":
                        if (selectactivity && selectactivity.ID) {
                            var id = selectactivity.ID;
                            var name = selectactivity.Name;
                            var activityType = selectactivity.ActivityType;
                            var left = x; //selectactivity.Style.Left;
                            var top = y; //selectactivity.Style.Top;
                            var img = "/Plugins/WorkflowDesigner/Content/Themes/Default/images/" + selectactivity.ActivityType + ".png";
                            var activityResource = "<div id=\"" + id + "\"  class=\"designeractivity\" name=\"" + name + "\" ActivityType=\"" + activityType + "\" style=\"left:" + left + "px;top:" + top + "px;width:40px;height:40px;position:absolute\"><img style=\"width:40px;height:40px;\" src=\"" + img + "\" /><label style=\"width:100px;position:absolute;Top:40px;left:-30px\">" + name + "</label></div>"
                            $("#" + containerment).append(activityResource);
                            $("#" + id).bind("mouseover", function () { jsPlumb.show(id, $("#" + id)); });
                            $("#" + id).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + id + "&ActivityType=" + activityType, 850, 580, true); });
                            initContextMenu();
                            var activity = selectactivity;
                            processDefine.Activities.push(activity);
                            var exampleGreyEndpointOptions = {
                                isSource: true,
                                isTarget: true,
                                DragOptions: { cursor: 'pointer', zIndex: 2000 },
                                Endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                                EndpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }],
                                connector: [lineStyle, { stub: [40, 60], gap: 10 }],
                                paintStyle: { fillStyle: "#528E21", radius: 7 },
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
                            jsPlumb.draggable(jsPlumb.getSelector($("#" + id)),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + containerment)
                       });
                            $(".designeractivity").each(function () {
                                jsPlumb.hide(this.id, true);
                            });
                            $(".designeractivity").each(function () {
                                jsPlumb.show(this.id);
                            });
                            if (operate.length < 10) {
                                operate.push({ bindEvent: "addActivity", bindObject: activity, bind: "designerActivity", result: "addActivity" });
                            }
                            else {
                                operate.shift();
                                operate.push({ bindEvent: "addActivity", bindObject: activity, bind: "designerActivity", result: "addActivity" });
                            }
                            selectactivity = null;
                        }
                        break;
                }
                return false;
            });
            setTimeout(function () { // Delay for Mozilla
                $(document).click(function () {
                    $(menu).fadeOut(o.outSpeed);
                    return false;
                });
            }, 0);
        });
    }
    function initUI() {
        $("#tabs").tabs();
        // fix the classes
        $(".tabs-bottom .ui-tabs-nav, .tabs-bottom .ui-tabs-nav > *")
                .removeClass("ui-corner-all ui-corner-top")
                .addClass("ui-corner-bottom");
        $(".tabs-bottom .ui-tabs-nav").appendTo(".tabs-bottom");
    }
    //初始化鼠标事件
    function init() {
        $("#main_designercontent").bind("click", function () {
            // $("#contexmenu").hide();
            $(".designeractivity").each(function () {
                jsPlumb.hide(this.id, true);
            });
            $(".designeractivity").each(function () {
                jsPlumb.show(this.id);
                e = window.event || argument[0];
                if (!e.ctrlKey) {
                    this.selectedActivity = false;
                }
                if ($(this).attr("standardactivity")) {
                    $(this).removeAttr("standardactivity");
                }
                if ($(this).attr("selectedactivity")) {
                    $(this).removeAttr("selectedactivity");
                }
            });
        });
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
        //删除活动
        //$("#contexmenu_del").click(function () {
        //    if (selectactivity != null) {
        //        jsPlumb.removeAllEndpoints(selectactivity.id);
        //        var ActivitiesCount = processDefine.Activities.length;
        //        for (var j = 0; j < ActivitiesCount; j++) {
        //            if (selectactivity.id == processDefine.Activities[j].ID) {
        //                var position = processDefine.Activities.indexOf(processDefine.Activities[j]);
        //                processDefine.Activities[j].Style.Left = $("#" + selectactivity.id).offset().left;
        //                processDefine.Activities[j].Style.Top = $("#" + selectactivity.id).offset().top;
        //                processDefine.Activities[j].Style.Width = $("#" + selectactivity.id).width();
        //                processDefine.Activities[j].Style.Height = $("#" + selectactivity.id).height();
        //                processDefine.Activities[j].Style.ZIndex = 0;
        //                if (operate.length < 10) {
        //                    operate.push({ bindEvent: "activitycontextmenu", bindObject: processDefine.Activities[j], bind: "designerActivity", result: "delActivity" });
        //                }
        //                else {
        //                    operate.shift();
        //                    operate.push({ bindEvent: "activitycontextmenu", bindObject: processDefine.Activities[j], bind: "designerActivity", result: "delActivity" });
        //                }
        //                processDefine.Activities.splice(position, 1);
        //                ActivitiesCount--;
        //                j = -1;

        //            }
        //        }
        //        var connectionCount = processDefine.Transitions.length;
        //        for (var i = 0 ; i < connectionCount; i++) {
        //            if (processDefine.Transitions[i].SrcActivity == selectactivity.id || processDefine.Transitions[i].DestActivity == selectactivity.id) {
        //                var conPosition = processDefine.Transitions.indexOf(processDefine.Transitions[i]);
        //                processDefine.Transitions.splice(conPosition, 1);
        //                connectionCount--;
        //                i = -1;
        //            }
        //        }
        //        $(selectactivity).remove();
        //    }

        //});

        $(".menuedit").mouseleave(function () {
            $(this).hide();
        });
        $("#linestyle").mouseleave(function () {
            $("#linestyle").hide();
        });
        $(".line").click(function () {
            switch (this.id) {
                case "linestyle_straight": lineStyle = "Straight"; break;
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
    function changePosition(currentObject) {
        for (var i = 0; i < currentObject.length; i++) {
            var left = currentObject[i].left;
            var top = currentObject[i].top;
            currentObject[i].left = $("#" + currentObject[i].ID).css("left");
            currentObject[i].top = $("#" + currentObject[i].ID).css("top");
            $("#" + currentObject[i].ID).css("left", left);
            $("#" + currentObject[i].ID).css("top", top);
        }
    }
    //前进一步
    function redo() {
        if (temporaryObject.length > 0) {
            var currentOperate = temporaryObject.pop();
            var currentObject = currentOperate.bindObject;
            switch (currentOperate.bindEvent) {
                case "drag":
                    if (currentObject) {
                        $("#" + currentObject.helper[0].id).css("left", currentObject.position.left);
                        $("#" + currentObject.helper[0].id).css("top", currentObject.position.top);
                        console.log("前进到回退前，回到原来位置" + currentOperate.result);
                        operate.push({ bindEvent: "drag", bindObject: temporaryDrop.shift(), bind: currentOperate.bind, result: currentOperate.result });
                        temporaryDrop.push(currentObject);
                    } break;
                case "addActivity":
                    var id = currentObject.ID;
                    var name = currentObject.Name;
                    var activityType = currentObject.ActivityType;
                    var left = currentObject.Style.Left;
                    var top = currentObject.Style.Top;
                    var img = "/Plugins/WorkflowDesigner/Content/Themes/Default/images/" + currentObject.ActivityType + ".png";
                    var activityResource = "<div id=\"" + id + "\"  class=\"designeractivity\" name=\"" + name + "\" ActivityType=\"" + activityType + "\" style=\"left:" + left + "px;top:" + top + "px;width:40px;height:40px;position:absolute\"><img style=\"width:40px;height:40px;\" src=\"" + img + "\" /><label style=\"width:100px;position:absolute;Top:40px;left:-30px\">" + name + "</label></div>"
                    $("#" + containerment).append(activityResource);
                    $("#" + id).bind("mouseover", function () { jsPlumb.show(id, $("#" + id)); });
                    $("#" + id).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + id + "&ActivityType=" + activityType, 850, 580, true); });
                    initContextMenu();
                    var activity = currentObject;
                    processDefine.Activities.push(activity);
                    var exampleGreyEndpointOptions = {
                        isSource: true,
                        isTarget: true,
                        DragOptions: { cursor: 'pointer', zIndex: 2000 },
                        Endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                        EndpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }],
                        connector: [lineStyle, { stub: [40, 60], gap: 10 }],
                        paintStyle: { fillStyle: "#528E21", radius: 7 },
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
                    jsPlumb.draggable(jsPlumb.getSelector($("#" + id)),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + containerment)
                       });
                    $(".designeractivity").each(function () {
                        jsPlumb.hide(this.id, true);
                    });
                    $(".designeractivity").each(function () {
                        jsPlumb.show(this.id);
                    });
                    operate.push(currentOperate);
                    break;
                case "activitycontextmenu":
                    var id = currentObject.ID;
                    jsPlumb.removeAllEndpoints(id);
                    var ActivitiesCount = processDefine.Activities.length;
                    for (var j = 0; j < ActivitiesCount; j++) {
                        if (id == processDefine.Activities[j].ID) {
                            var position = processDefine.Activities.indexOf(processDefine.Activities[j]);
                            processDefine.Activities[j].Style.Left = $("#" + id).offset().left;
                            processDefine.Activities[j].Style.Top = $("#" + id).offset().top;
                            processDefine.Activities[j].Style.Width = $("#" + id).width();
                            processDefine.Activities[j].Style.Height = $("#" + id).height();
                            processDefine.Activities[j].Style.ZIndex = 0;
                            currentOperate = { bindEvent: "activitycontextmenu", bindObject: processDefine.Activities[j], bind: currentOperate.bind, result: currentOperate.result };
                            processDefine.Activities.splice(position, 1);
                            ActivitiesCount--;
                            j = -1;
                        }
                    }
                    var connectionCount = processDefine.Transitions.length;
                    for (var i = 0 ; i < connectionCount; i++) {
                        if (processDefine.Transitions[i].SrcActivity == id || processDefine.Transitions[i].DestActivity == id) {
                            var conPosition = processDefine.Transitions.indexOf(processDefine.Transitions[i]);
                            processDefine.Transitions.splice(conPosition, 1);
                            connectionCount--;
                            i = -1;
                        }
                    }
                    $("#" + id).remove();
                    console.log("回退到加入活动前，删除活动");
                    operate.push(currentOperate);
                    break;
                case "connection":
                    var anchors;
                    startConnection = 3;
                    if (currentObject.sourceEndpoint) {
                        if (currentObject.sourceEndpoint.anchor.type != "Perimeter") {
                            anchors = [currentObject.sourceEndpoint.anchor.type, currentObject.targetEndpoint.anchor.type];
                        }
                        else {
                            anchors = [["Perimeter", { shape: "rectangle" }], ["Perimeter", { shape: "rectangle" }]];
                        }
                    }
                    else {
                        anchors = [currentObject.SourceOrientation, currentObject.SinkOrientation];
                    }
                    jsPlumb.show(currentObject.SrcActivity, $("#" + currentObject.SrcActivity));
                    jsPlumb.show(currentObject.DestActivity, $("#" + currentObject.DestActivity));
                    jsPlumb.connect({
                        paintStyle: { fillStyle: "Green", strokeStyle: "Green", lineWidth: 2, radius: 7 },
                        source: currentObject.SrcActivity,
                        target: currentObject.DestActivity,
                        anchors: anchors,
                        connector: lineStyle,
                        hoverPaintStyle: connectorHoverStyle,
                        connectorHoverStyle: connectorHoverStyle,
                        endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                        scope: "process",
                        endpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }]
                    });
                    processDefine.Transitions.push(currentObject);
                    console.log("前进到删除连接前，重连对象");

                    break;
                case "conncontextmenu":
                    for (var i = 0; i < processDefine.Transitions.length; i++) {
                        var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                        var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                        if (sourceAnchor == currentObject.sourceEndpoint.anchor.type && sinkAnchor == currentObject.targetEndpoint.anchor.type && processDefine.Transitions[i].SrcActivity == currentObject.sourceId && processDefine.Transitions[i].DestActivity == currentObject.targetId) {
                            operate.push({ bindEvent: "conncontextmenu", bindObject: processDefine.Transitions[i], bind: "connection", result: "connection" });
                            processDefine.Transitions.splice(i, 1);
                        }
                    }
                    jsPlumb.detach(currentObject);
                    console.log("前进到连接前，删除连接");
                    break;
                case "zoomenlarge": zoomenlarge(); operate.push(currentOperate); break;
                case "zoomdecrease": zoomdecrease(); operate.push(currentOperate); break;
                case "leftActivity":
                    changePosition(currentObject);
                    operate.push(currentOperate);
                    break;
                case "centerActivity":
                    changePosition(currentObject);
                    operate.push(currentOperate);
                    break;
                case "rightActivity":
                    changePosition(currentObject);
                    operate.push(currentOperate);
                    break;
            }
            startConnection = 1;
            jsPlumb.repaintEverything();
        }
    }

    //回退一步
    function undo() {
        if (operate.length > 0) {
            var currentOperate = operate.pop();
            var currentObject = currentOperate.bindObject;
            switch (currentOperate.bindEvent) {
                case "drag":
                    if (currentObject) {
                        $("#" + currentObject.helper[0].id).css("left", currentObject.position.left);
                        $("#" + currentObject.helper[0].id).css("top", currentObject.position.top);
                        jsPlumb.repaintEverything();
                        console.log("回退到拖拽前，回到原来位置" + currentOperate.result);
                        temporaryObject.push({ bindEvent: "drag", bindObject: temporaryDrop.pop(), bind: currentOperate.bind, result: currentOperate.result });
                        temporaryDrop.unshift(currentObject);
                    } break;
                case "addActivity":
                    var id = currentObject.ID;
                    jsPlumb.removeAllEndpoints(id);
                   // jsPlumb.detachAllConnections(id);
                    var ActivitiesCount = processDefine.Activities.length;
                    for (var j = 0; j < ActivitiesCount; j++) {
                        if (id == processDefine.Activities[j].ID) {
                            var position = processDefine.Activities.indexOf(processDefine.Activities[j]);
                            processDefine.Activities[j].Style.Left = $("#" + id).offset().left;
                            processDefine.Activities[j].Style.Top = $("#" + id).offset().top;
                            processDefine.Activities[j].Style.Width = $("#" + id).width();
                            processDefine.Activities[j].Style.Height = $("#" + id).height();
                            processDefine.Activities[j].Style.ZIndex = 0;
                            currentOperate = { bindEvent: "addActivity", bindObject: processDefine.Activities[j], bind: currentOperate.bind, result: currentOperate.result };
                            processDefine.Activities.splice(position, 1);
                            ActivitiesCount--;
                            j = -1;
                        }
                    }
                    var connectionCount = processDefine.Transitions.length;
                    for (var i = 0 ; i < connectionCount; i++) {
                        if (processDefine.Transitions[i].SrcActivity == id || processDefine.Transitions[i].DestActivity == id) {
                            var conPosition = processDefine.Transitions.indexOf(processDefine.Transitions[i]);
                            processDefine.Transitions.splice(conPosition, 1);
                            connectionCount--;
                            i = -1;
                        }
                    }
                    $("#" + id).remove();
                    console.log("回退到加入活动前，删除活动");
                    temporaryObject.push(currentOperate);
                    break;
                case "activitycontextmenu":
                    var id = currentObject.ID;
                    var name = currentObject.Name;
                    var activityType = currentObject.ActivityType;
                    var left = currentObject.Style.Left;
                    var top = currentObject.Style.Top;
                    var img = "/Plugins/WorkflowDesigner/Content/Themes/Default/images/" + currentObject.ActivityType + ".png";
                    var activityResource = "<div id=\"" + id + "\"  class=\"designeractivity\" name=\"" + name + "\" ActivityType=\"" + activityType + "\" style=\"left:" + left + "px;top:" + top + "px;width:40px;height:40px;position:absolute\"><img style=\"width:40px;height:40px;\" src=\"" + img + "\" /><label style=\"width:100px;position:absolute;Top:40px;left:-30px\">" + name + "</label></div>"
                    $("#" + containerment).append(activityResource);
                    $("#" + id).bind("mouseover", function () { jsPlumb.show(id, $("#" + id)); });
                    $("#" + id).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + id + "&ActivityType=" + activityType, 850, 580, true); });
                    //$("#" + id).bind("contextmenu", function () {
                    //    selectactivity = $("#" + id)[0];
                    //    e = window.event || arguments[0];
                    //    var pointx = (e.clientX + $(document).find("#" + containerment).scrollLeft()) || e.offsetX;
                    //    var pointy = (e.clientY + $(document).find("#" + containerment).scrollTop()) || e.offsetY;
                    //    $("#contexmenu").css("left", pointx);
                    //    $("#contexmenu").css("top", pointy);
                    //    $("#contexmenu").show();
                    //});
                    initContextMenu();
                    var activity = currentObject;
                    processDefine.Activities.push(activity);
                    var exampleGreyEndpointOptions = {
                        isSource: true,
                        isTarget: true,
                        DragOptions: { cursor: 'pointer', zIndex: 2000 },
                        Endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                        EndpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }],
                        connector: [lineStyle, { stub: [40, 60], gap: 10 }],
                        paintStyle: { fillStyle: "#528E21", radius: 7 },
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
                    jsPlumb.draggable(jsPlumb.getSelector($("#" + id)),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + containerment)
                       });
                    $(".designeractivity").each(function () {
                        jsPlumb.hide(this.id, true);
                    });
                    $(".designeractivity").each(function () {
                        jsPlumb.show(this.id);
                    });
                    temporaryObject.push(currentOperate);
                    break;
                case "connection":
                    for (var i = 0; i < processDefine.Transitions.length; i++) {
                        var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                        var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                        if (sourceAnchor == currentObject.sourceEndpoint.anchor.type && sinkAnchor == currentObject.targetEndpoint.anchor.type && processDefine.Transitions[i].SrcActivity == currentObject.sourceId && processDefine.Transitions[i].DestActivity == currentObject.targetId) {
                            temporaryObject.push({ bindEvent: "connection", bindObject: processDefine.Transitions[i], bind: "connection", result: "lineConnection" });
                            processDefine.Transitions.splice(i, 1);
                        }
                    }
                    //for (var i = 0; i < processDefine.Transitions.length; i++) {
                    //    var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                    //    var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                    //    if (sourceAnchor == currentObject.SourceOrientation && sinkAnchor == currentObject.SinkOrientation && processDefine.Transitions[i].SrcActivity == currentObject.SrcActivity && processDefine.Transitions[i].DestActivity == currentObject.DestActivity) {
                    //        processDefine.Transitions.splice(i, 1);
                    //    }
                    //}
                    jsPlumb.detach(currentObject);
                    console.log("回退到连接前，删除连接");
                    //temporaryObject.push(currentOperate);
                    break;
                case "conncontextmenu":
                    startConnection = 2;
                    var anchors;
                    if (currentObject.endpoints) {
                        if (currentObject.endpoints[0].anchor.type != "Perimeter") {
                            anchors = [currentObject.endpoints[0].anchor.type, currentObject.endpoints[1].anchor.type];
                        }
                        else {
                            anchors = [["Perimeter", { shape: "rectangle" }], ["Perimeter", { shape: "rectangle" }]];
                        }
                    }
                    else if (currentObject.sourceEndpoint) {
                        if (currentObject.sourceEndpoint.anchor.type != "Perimeter") {
                            anchors = [currentObject.sourceEndpoint.anchor.type, currentObject.targetEndpoint.anchor.type];
                        }
                        else {
                            anchors = [["Perimeter", { shape: "rectangle" }], ["Perimeter", { shape: "rectangle" }]];
                        }
                    }
                    else {
                        anchors = [currentObject.SourceOrientation, currentObject.SinkOrientation];
                    }

                    jsPlumb.show(currentObject.SrcActivity, $("#" + currentObject.SrcActivity));
                    jsPlumb.show(currentObject.DestActivity, $("#" + currentObject.DestActivity));
                    jsPlumb.connect({
                        paintStyle: { fillStyle: "Green", strokeStyle: "Green", lineWidth: 2, radius: 7 },
                        source: currentObject.SrcActivity,
                        target: currentObject.DestActivity,
                        anchors: anchors,
                        connector: lineStyle,
                        hoverPaintStyle: connectorHoverStyle,
                        connectorHoverStyle: connectorHoverStyle,
                        endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                        scope: "process",
                        endpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }]
                    });
                    processDefine.Transitions.push(currentObject);
                    //temporaryObject.push(currentOperate);
                    console.log("回退到删除活动前，重连对象");
                    break;
                case "zoomenlarge":
                    zoomdecrease();
                    temporaryObject.push(currentOperate);
                    break;
                case "zoomdecrease":
                    zoomenlarge();
                    temporaryObject.push(currentOperate);
                    break;
                case "leftActivity":
                    changePosition(currentObject);
                    temporaryObject.push(currentOperate);
                    break;
                case "centerActivity":
                    changePosition(currentObject);
                    temporaryObject.push(currentOperate);
                    break;
                case "rightActivity":
                    changePosition(currentObject);
                    temporaryObject.push(currentOperate);
                    break;

            }
            startConnection = 1;
            jsPlumb.repaintEverything();
        }
    }
    function zoomdecrease() {
        $(".designeractivity").each(function () {
            if ($(this).width() > 20) {
                $(this).css("width", $(this).width() * 0.5 + "px");
                $(this).css("height", $(this).height() * 0.5 + "px");
                $(this).find("img").css("width", $(this).find("img").width() * 0.5 + "px");
                $(this).find("img").css("height", $(this).find("img").height() * 0.5 + "px");
                $(this).find("label").css("top", $(this).find("img").height() + "px");

            }
        });
    }
    function zoomenlarge() {
        $(".designeractivity").each(function () {
            if ($(this).width() < 200) {
                $(this).css("width", $(this).width() * 2 + "px");
                $(this).css("height", $(this).height() * 2 + "px");
                $(this).find("img").css("width", $(this).find("img").width() * 2 + "px");
                $(this).find("img").css("height", $(this).find("img").height() * 2 + "px");
                $(this).find("label").css("top", $(this).find("img").height() + "px");

            }
        });
    }
    //初始化工具栏
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
                case "header_toolbar_back": undo(); break;
                case "header_toolbar_redo": redo(); break;
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
                    if (operate.length < 10) {
                        operate.push({ bindEvent: "zoomenlarge", bindObject: null, bind: "point", result: "editPoint" });
                    }
                    else {
                        operate.shift();
                        operate.push({ bindEvent: "zoomenlarge", bindObject: null, bind: "point", result: "editPoint" });
                    }
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
                    if (operate.length < 10) {
                        operate.push({ bindEvent: "zoomdecrease", bindObject: null, bind: "point", result: "editPoint" });
                    }
                    else {
                        operate.shift();
                        operate.push({ bindEvent: "zoomdecrease", bindObject: null, bind: "point", result: "editPoint" });
                    }
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
                case "header_toolbar_left":
                    var field = new Object();
                    var selectItems = new Array();
                    $(".designeractivity").each(function () {
                        if ($(this).attr("standardActivity")) {
                            field.standard = this;
                            var left = $(this).css("left");
                            var top = $(this).css("top");
                            var width = $(this).width();
                            field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                            field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                            field.width = width;
                        }
                        else if ($(this).attr("selectedActivity")) {
                            selectItems.push({ ID: $(this).attr("id"), left: $(this).css("left"), top: $(this).css("top") });
                        }
                    });
                    if (field.standard) {
                        for (var i = 0; i < selectItems.length; i++) {
                            var left = field.left + "px";
                            $("#" + selectItems[i].ID).css("left", left);
                        }
                        jsPlumb.repaintEverything();
                        if (operate.length < 10) {
                            operate.push({ bindEvent: "leftActivity", bindObject: selectItems, bind: "activity", result: "leftActivity" });
                        }
                        else {
                            operate.shift();
                            operate.push({ bindEvent: "leftActivity", bindObject: selectItems, bind: "activity", result: "leftActivity" });
                        }
                    }
                    else {
                        $(".designeractivity").each(function () {
                            if ($(this).attr("standardactivity")) {
                                $(this).removeAttr("standardactivity");
                            }
                            if ($(this).attr("selectedactivity")) {
                                $(this).removeAttr("selectedactivity");
                            }
                        });
                    }
                    break;
                case "header_toolbar_center":
                    var field = new Object();
                    var selectItems = new Array();
                    $(".designeractivity").each(function () {
                        if ($(this).attr("standardActivity")) {
                            field.standard = this;
                            var left = $(this).css("left");
                            var top = $(this).css("top");
                            var width = $(this).width();
                            field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                            field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                            field.width = width
                        }
                        else if ($(this).attr("selectedActivity")) {
                            // selectItems.push(this);
                            selectItems.push({ ID: $(this).attr("id"), left: $(this).css("left"), top: $(this).css("top") });
                        }
                    });
                    if (field.standard) {
                        for (var i = 0; i < selectItems.length; i++) {
                            var left = field.left + field.width / 2 - $("#" + selectItems[i].ID).width() / 2 + "px";
                            $("#" + selectItems[i].ID).css("left", left);
                        }
                        jsPlumb.repaintEverything();
                        if (operate.length < 10) {
                            operate.push({ bindEvent: "centerActivity", bindObject: selectItems, bind: "activity", result: "centerActivity" });
                        }
                        else {
                            operate.shift();
                            operate.push({ bindEvent: "centerActivity", bindObject: selectItems, bind: "activity", result: "centerActivity" });
                        }
                    }
                    else {
                        $(".designeractivity").each(function () {
                            if ($(this).attr("standardactivity")) {
                                $(this).removeAttr("standardactivity");
                            }
                            if ($(this).attr("selectedactivity")) {
                                $(this).removeAttr("selectedactivity");
                            }
                        });
                    }
                    break;
                case "header_toolbar_right":
                    var field = new Object();
                    var selectItems = new Array();
                    $(".designeractivity").each(function () {
                        if ($(this).attr("standardActivity")) {
                            field.standard = this;
                            var left = $(this).css("left");
                            var top = $(this).css("top");
                            var width = $(this).width();
                            field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                            field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                            field.width = width
                        }
                        else if ($(this).attr("selectedActivity")) {
                            //selectItems.push(this);
                            selectItems.push({ ID: $(this).attr("id"), left: $(this).css("left"), top: $(this).css("top") });
                        }
                    });
                    if (field.standard) {
                        for (var i = 0; i < selectItems.length; i++) {
                            var standardleft = field.left + field.width;
                            var width = $("#" + selectItems[i].ID).width();
                            var left = standardleft - width + "px";
                            $("#" + selectItems[i].ID).css("left", left);
                        }
                        jsPlumb.repaintEverything();
                        if (operate.length < 10) {
                            operate.push({ bindEvent: "rightActivity", bindObject: selectItems, bind: "activity", result: "rightActivity" });
                        }
                        else {
                            operate.shift();
                            operate.push({ bindEvent: "rightActivity", bindObject: selectItems, bind: "activity", result: "rightActivity" });
                        }
                    }
                    else {
                        $(".designeractivity").each(function () {
                            if ($(this).attr("standardactivity")) {
                                $(this).removeAttr("standardactivity");
                            }
                            if ($(this).attr("selectedactivity")) {
                                $(this).removeAttr("selectedactivity");
                            }
                        });
                    }
                    break;
            };
        });
    }

    //初始化菜单
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

    //初始化视图
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

    //初始化流程图结束，同步流程信息到变量processDefine中
    function initProcessDefine(proDefine) {
        startConnection = 1;
        processDefine.Activities = proDefine.Activities;
        processDefine.Transitions = proDefine.Transitions;
        var connections = jsPlumb.getConnections({ scope: "process" });
        for (var j = 0; j < connections.length; j++) {
            for (var i = 0; i < processDefine.Transitions.length; i++) {
                var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                switch (sourceAnchor) {
                    case "Left": sourceAnchor = "LeftMiddle"; break;
                    case "Top": sourceAnchor = "TopCenter"; break;
                    case "Right": sourceAnchor = "RightMiddle"; break;
                    case "Bottom": sourceAnchor = "BottomCenter"; break;
                }
                switch (sinkAnchor) {
                    case "Left": sinkAnchor = "LeftMiddle"; break;
                    case "Top": sinkAnchor = "TopCenter"; break;
                    case "Right": sinkAnchor = "RightMiddle"; break;
                    case "Bottom": sinkAnchor = "BottomCenter"; break;
                }
                if (sourceAnchor == connections[j].endpoints[0].anchor.type && sinkAnchor == connections[j].endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == connections[j].sourceId && processDefine.Transitions[i].DestActivity == connections[j].targetId) {
                    connectionLabel = connections[j];
                    setConnectionLabel(processDefine.Transitions[i].Name);
                }
            }
        }
    }

    //保存流程图
    function saveProcessDefine() {
        if ($.trim($("#currentStatus").val()) == CurrentStatus.Published) {
            alert("流程为发布状态，无法保存，修改发布状态的流程可能会影响该流程实例");
            return;
        }
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


        var Name = $.trim($("#processName").val());
        var text = $.trim($("#processText").val());
        var version = $.trim($("#version").val());
        var description = $.trim($("#description").val());
        var startor = $.trim($("#startor").val());
        var isActive = $.trim($("#isActive").val());
        var currentFlag = $.trim($("#currentFlag").val());
        var currentStatus = $.trim($("#currentStatus").val());
        var categoryID = $.trim($("#categoryID").val());
        processDefine.Name = text;
        processDefine.Version = version;
        processDefine.Author = startor;
        processDefine.StartURL = $.trim($("#starturl").val());
        processDefine.ID = Name;//2012.11.16
        var processDefContent = JSON2.stringify(processDefine);
        var processDefID = $.query.get("processDefID");
        if ($.query.get("action") == 'cloneProcess' && processDefID) {//代码bug，如果对方传过来的参数为&
            processDefID = null;
        }
        $.post("/WorkflowDesigner/Workflow/SaveProcessDefine", { processDefID: processDefID, categoryID: categoryID, processDefContent: processDefContent, Name: Name, text: text, version: version, description: description, startor: startor, isActive: isActive, currentFlag: currentFlag, currentStatus: currentStatus }, function (value) {
            var ajaxResult = value;
            alert(ajaxResult.PromptMsg);
        });
    }


    //开始绘制流程图
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
                        $("#" + id).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + processDefine.ID + "&ActivityID=" + id + "&ActivityType=" + $.trim($(ui.draggable[0]).attr("id")), 850, 580, true); });
                        //$("#" + id).bind("contextmenu", function () {
                        //    selectactivity = $("#" + id)[0];
                        //    e = window.event || arguments[0];
                        //    var pointx = (e.clientX + $(document).find("#" + container).scrollLeft()) || e.offsetX;
                        //    var pointy = (e.clientY + $(document).find("#" + container).scrollTop()) || e.offsetY;
                        //    $("#contexmenu").css("left", pointx);
                        //    $("#contexmenu").css("top", pointy);
                        //    $("#contexmenu").show();
                        //});
                        $("#" + id).bind("click", function () {
                          
                            e = window.event || arguments[0];
                            if (window.event) //停止事件向下传播
                                window.event.cancelBubble = true;
                            else {
                                e.stopPropagation();
                            }
                        });
                        initContextMenu();
                        $("#" + id).bind("mousedown", function () {
                            e = window.event || arguments[0];
                            if (window.event) //停止事件向下传播
                                window.event.cancelBubble = true;
                            else {
                                e.stopPropagation();
                            }
                            var me = this;
                            if (e.ctrlKey) {
                                this.selectedActivity = true;
                                $(this).attr("selectedActivity", "true");
                            }
                            else {
                                if (!this.selectedActivity) {
                                    $(".designeractivity").each(function () {
                                        jsPlumb.hide(this.id, true);
                                        jsPlumb.show(this.id);
                                    });
                                    jsPlumb.show(id, $("#" + id));
                                    $(".designeractivity").each(function () {
                                        $(this).removeAttr("standardActivity")
                                    });
                                    $(this).attr("standardActivity", "true");
                                }
                                else {

                                    $(".designeractivity").draggable("disable");
                                    // $(this).bind('dragstart', function (event, ui) { console.log("1")});
                                    startmove = 0;
                                    e = window.event || arguments[0];
                                    preleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                                    pretop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                                    //preleft = $(this).offset().left;
                                    //pretop = $(this).offset().top;
                                    var top = $(this).offset().top;
                                    var left = $(this).offset().left;
                                    var buttom = 0;
                                    var right = 0;
                                    $(".designeractivity").each(function () {
                                        if (this.selectedActivity) {
                                            if ($(this).offset().left < left) {
                                                left = $(this).offset().left;
                                            }
                                            if ($(this).offset().left > right) {
                                                right = $(this).offset().left;
                                            }
                                            if ($(this).offset().top < top) {
                                                top = $(this).offset().top;
                                            }
                                            if ($(this).offset().top > buttom) {
                                                buttom = $(this).offset().top;
                                            }
                                        }
                                    });
                                    var width = right - left + $(this).width();
                                    var height = buttom - top + $(this).height();
                                    var rect = "<div id=\"rect\" style=\"border:1px dashed #B6B6B6; position:absolute;z-index:999999999999;width:" + width + "px;height:" + height + "px;left:" + left + "px;top:" + top + "px\"></div>"
                                    $("#" + containerment).append(rect);
                                    //$("#rect").draggable();
                                    //$("#rect").bind("mousedown",function () {
                                    //    //alert("1");
                                    //    $("#rect").bind("mousemove", function () {
                                    //        e = window.event || arguments[0];
                                    //        var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                                    //        var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                                    //        var left = endleft - preleft + $("#rect").offset().left;
                                    //        var top = endtop - pretop + $("#rect").offset().top;
                                    //        $("#rect").css("left", left);
                                    //        $("#rect").css("top", top);
                                    //    });
                                    //});
                                    document.onmousemove = null;
                                    $(document).unbind("mousemove");
                                    // document.onmouseup = null;
                                    //  document.onmouseup = activityMouseup;
                                    $(document).bind("mouseup", function () {
                                        $(document).unbind("mousemove");
                                        $("#rect").remove();
                                        $(".designeractivity").draggable("enable");
                                        if (startmove == 1) {
                                            e = window.event || arguments[0];
                                            var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                                            var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                                            var left = endleft - preleft;
                                            var top = endtop - pretop;
                                            // var me = this;
                                            $(".designeractivity").each(function () {
                                                if (this.selectedActivity) {
                                                    this.style.left = left + $(this).offset().left + "px";
                                                    this.style.top = top + $(this).offset().top + "px";
                                                }
                                                // $(this).css("left", endleft);
                                                // $(this).css("top", endtop);
                                                // alert(this.style.left);
                                            });
                                            jsPlumb.repaintEverything();

                                        }
                                        startmove = 0;
                                        document.onmouseup = null;
                                        document.onmousemove = null;
                                    });
                                    // document.onmousemove = rectMouseMove;
                                    $(document).bind("mousemove", function () {
                                        startmove = 1;
                                    });
                                    document.onmousemove = null;
                                }
                            }
                        });
                        var activity = { Name: text, ID: id, ActivityType: $.trim($(ui.draggable[0]).attr("id")), Style: {} };
                        processDefine.Activities.push(activity);
                        var exampleGreyEndpointOptions = {
                            isSource: true,
                            isTarget: true,
                            DragOptions: { cursor: 'pointer', zIndex: 2000 },
                            Endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
                            EndpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }],
                            connector: [lineStyle, { stub: [40, 60], gap: 10 }],
                            paintStyle: { fillStyle: "#528E21", radius: 7 },
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
                        jsPlumb.draggable(jsPlumb.getSelector($("#" + id)),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + container)
                       });
                        $(".designeractivity").each(function () {
                            jsPlumb.hide(this.id, true);
                        });
                        $(".designeractivity").each(function () {
                            jsPlumb.show(this.id);
                        });
                        if (operate.length < 10) {
                            operate.push({ bindEvent: "addActivity", bindObject: activity, bind: "designerActivity", result: "addActivity" });
                        }
                        else {
                            operate.shift();
                            operate.push({ bindEvent: "addActivity", bindObject: activity, bind: "designerActivity", result: "addActivity" });
                        }
                    }
                }
                else {
                    temporaryDrop.push(ui);
                }
                startConnection = 1;
                //  alert("123");
            }
        });
        initUI();
        init();
        initMenu();
        initToolbar();
        initView();

    }


    //初始化流程活动
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
        $("#" + activity.ID).bind("dblclick", function () { window.parent.parent.openDialog("actionDialog2", '活动配置', "/WorkflowDesigner/Workflow/ActivityDetail?ProcessDefID=" + $.query.get("processDefID") + "&ActivityID=" + activity.ID + "&ActivityType=" + $(activity).attr("ActivityType"), 850, 580, true); });
        //$("#" + activity.ID).bind("contextmenu", function () {
        //    selectactivity = $("#" + activity.ID)[0];
        //    e = window.event || arguments[0];
        //    var pointx = (e.clientX + $(document).find("#" + container).scrollLeft()) || e.offsetX;
        //    var pointy = (e.clientY + $(document).find("#" + container).scrollTop()) || e.offsetY;
        //    $("#contexmenu").css("left", pointx);
        //    $("#contexmenu").css("top", pointy);
        //    $("#contexmenu").show();
        //});
        initContextMenu();
        $("#" + activity.ID).bind("click", function () {
           
            e = window.event || arguments[0];
            if (window.event) //停止事件向下传播
                window.event.cancelBubble = true;
            else {
                e.stopPropagation();
            }
        });
        $("#" + activity.ID).bind("mousedown", function () {
            e = window.event || arguments[0];
            if (window.event) //停止事件向下传播
                window.event.cancelBubble = true;
            else {
                e.stopPropagation();
            }
            var me = this;
            if (e.ctrlKey) {
                this.selectedActivity = true;
                $(this).attr("selectedActivity", "true");
            }
            else {
                if (!this.selectedActivity) {
                    $(".designeractivity").each(function () {
                        jsPlumb.hide(this.id, true);
                        jsPlumb.show(this.id);
                        $(this).removeAttr("standardActivity")
                    });
                    jsPlumb.show(activity.ID, $("#" + activity.ID));
                    $(this).attr("standardActivity", "true");
                }
                else {
                    $(".designeractivity").draggable("disable");
                    // $(this).bind('dragstart', function (event, ui) { console.log("1")});
                    startmove = 0;
                    e = window.event || arguments[0];
                    preleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                    pretop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                    //preleft = $(this).offset().left;
                    //pretop = $(this).offset().top;
                    var top = $(this).offset().top;
                    var left = $(this).offset().left;
                    var buttom = 0;
                    var right = 0;
                    $(".designeractivity").each(function () {
                        if (this.selectedActivity) {
                            if ($(this).offset().left < left) {
                                left = $(this).offset().left;
                            }
                            if ($(this).offset().left > right) {
                                right = $(this).offset().left;
                            }
                            if ($(this).offset().top < top) {
                                top = $(this).offset().top;
                            }
                            if ($(this).offset().top > buttom) {
                                buttom = $(this).offset().top;
                            }
                        }
                    });
                    var width = right - left + $(this).width();
                    var height = buttom - top + $(this).height();
                    var rect = "<div id=\"rect\" style=\"border:1px dashed #B6B6B6; position:absolute;z-index:999999999999;width:" + width + "px;height:" + height + "px;left:" + left + "px;top:" + top + "px\"></div>"
                    $("#" + containerment).append(rect);
                    //$("#rect").draggable();
                    //$("#rect").bind("mousedown",function () {
                    //    //alert("1");
                    //    $("#rect").bind("mousemove", function () {
                    //        e = window.event || arguments[0];
                    //        var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                    //        var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                    //        var left = endleft - preleft + $("#rect").offset().left;
                    //        var top = endtop - pretop + $("#rect").offset().top;
                    //        $("#rect").css("left", left);
                    //        $("#rect").css("top", top);
                    //    });
                    //});
                    document.onmousemove = null;
                    $(document).unbind("mousemove");
                    // document.onmouseup = null;
                    //  document.onmouseup = activityMouseup;
                   
                    // document.onmousemove = rectMouseMove;
                    $(document).bind("mousemove", function () {
                        startmove = 1;
                        //alert("2");
                        //$(document).unbind("mouseup");
                        $(document).bind("mouseup", function () {
                           // alert("1");
                            $(document).unbind("mousemove");
                            //$(document).unbind("mouseup");
                            $("#rect").remove();
                            $(".designeractivity").draggable("enable");
                            if (startmove == 1) {
                                e = window.event || arguments[0];
                                var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                                var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
                                var left = endleft - preleft;
                                var top = endtop - pretop;
                                // var me = this;
                                $(".designeractivity").each(function () {
                                    if (this.selectedActivity) {
                                        this.style.left = left + $(this).offset().left + "px";
                                        this.style.top = top + $(this).offset().top + "px";
                                    }
                                    // $(this).css("left", endleft);
                                    // $(this).css("top", endtop);
                                    // alert(this.style.left);
                                });
                                jsPlumb.repaintEverything();

                            }
                            startmove = 0;
                            document.onmouseup = null;
                            document.onmousemove = null;
                        });
                    });
                    document.onmousemove = null;
                }
            }
        });
    }
    function rectMouseMove() {
        startmove = 1;
        e = window.event || arguments[0];
        var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
        var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
        var left = endleft - preleft + $("#rect").offset().left;
        var top = endtop - pretop + $("#rect").offset().top;
        $("#rect").css("left", left);
        $("#rect").css("top", top);
        console.log("移动");
        //startmove = 1;
        //e = window.event || arguments[0];
        //var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
        //var endtop = (e.clientY+ $(document).scrollTop()) || e.offsetY;
        //var left = endleft - preleft + $("#rect").offset().left;
        //var top = endtop - pretop + $("#rect").offset().top;
        //$("#rect").css("left", left);
        //$("#rect").css("top", top);
        //var left = endleft - preleft;
        //var top = endtop - pretop;
        //$(".designeractivity").each(function () {
        //    if (this.selectedActivity) {
        //        //$(this).mousedown();
        //        //$(this).mousemove();
        //       // $(this).mousem();
        //    }

        //});
    }
    function activityMouseup() {
        $(document).unbind("mousemove");
        $("#rect").remove();
        $(".designeractivity").draggable("enable");
        if (startmove == 1) {
            e = window.event || arguments[0];
            var endleft = (e.clientX + $(document).scrollLeft()) || e.offsetX;
            var endtop = (e.clientY + $(document).scrollTop()) || e.offsetY;
            var left = endleft - preleft;
            var top = endtop - pretop;
            $(".designeractivity").each(function () {
                if (this.selectedActivity) {
                    this.style.left = left + $(this).offset().left + "px";
                    this.style.top = top + $(this).offset().top + "px";
                }
            });
            jsPlumb.repaintEverything();

        }
        startmove = 0;
        document.onmouseup = null;
        document.onmousemove = null;
    }

    //初始化流程线
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
            paintStyle: { fillStyle: connectionStyle, strokeStyle: connectionStyle, lineWidth: 2, radius: 7 },
            source: transition.SrcActivity,
            target: transition.DestActivity,
            anchors: [sourceAnchor, sinkAnchor],
            connector: lineStyle,
            hoverPaintStyle: connectorHoverStyle,
            connectorHoverStyle: connectorHoverStyle,
            endpoints: [["Dot", { radius: 7 }], ["Dot", { radius: 7 }]],
            scope: "process",
            endpointStyles: [{ fillStyle: "#528E21" }, { fillStyle: "#528E21" }]
        });

    }


    //初始化流程
    var WorkflowDesigner = function () {
        this.me = this;
        this.show = 1;
    }
    WorkflowDesigner.draw = function (url, proDefContent, container) {
        $.post(url, { processDefContent: proDefContent }, function (value) {
            var ajaxResult = value;
            var message = "操作失败";
            if (ajaxResult && ajaxResult.Result == "Success") {
                jsPlumb.removeEveryEndpoint();
                for (var q = 0; q < ajaxResult.RetValue.processDefine.Activities.length; q++) {
                    if (document.getElementById("leftcontent")) {
                        $("#leftcontent").hide();
                    }
                    drawActivityInst(ajaxResult.RetValue.processDefine.Activities[q], 0, container);
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
                jsPlumb.draggable(jsPlumb.getSelector(".designeractivity"),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + container)
                       });
                initProcessDefine(ajaxResult.RetValue.processDefine);
            }
        });

    }

    WorkflowDesigner.reloadXML = function (url) {
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
        var Name = $.trim($("#processName").val());
        var text = $.trim($("#processText").val());
        var version = $.trim($("#version").val());
        var description = $.trim($("#description").val());
        var startor = $.trim($("#startor").val());
        var isActive = $.trim($("#isActive").val());
        var currentFlag = $.trim($("#currentFlag").val());
        var currentStatus = $.trim($("#currentStatus").val());
        var categoryID = $.trim($("#categoryID").val());
        processDefine.Name = text;
        processDefine.Version = version;
        processDefine.Author = startor;
        processDefine.StartURL = $.trim($("#starturl").val());
        processDefine.ID = Name;//2012.11.16
        var processDefContent = JSON2.stringify(processDefine);
        $.post(url, { processDefContent: processDefContent }, function (value) {
            var ajaxResult = value;
            $("#XMLdesigner_content").val(ajaxResult.RetValue);
        });
    };
    //流程图设计静态初始化方法
    WorkflowDesigner.init = function (url, processDefID, processInstID, container, designerDefine, defineUrl) {
        containerment = container;
        if (processDefID != null && processDefID != "" && processDefID != true) {
            if (designerDefine == 1) {
                $.post(defineUrl, { ProcessDefineID: processDefID }, function (value) {
                    var ajaxResult = value;
                    var message = "操作失败";
                    if (ajaxResult && ajaxResult.Result == 1) {
                        if ($.query.get("action") == 'cloneProcess') {
                            ajaxResult.RetValue.CurrentState = 0;
                        }
                        if (ajaxResult.RetValue.CurrentState == 1)//1表示流程为发布状态，0为未发布状态，2为终止状态
                        {
                            alert("该流程为发布状态，无法修改保存");
                        }
                        $("#processName").val(ajaxResult.RetValue.Name);
                        $("#processText").val(ajaxResult.RetValue.Text);
                        $("#version").val(ajaxResult.RetValue.Version);
                        $("#description").val(ajaxResult.RetValue.Description);
                        $("#startor").val(ajaxResult.RetValue.Startor);
                        $("#isActive").val(ajaxResult.RetValue.IsActive);
                        $("#currentFlag").val(ajaxResult.RetValue.CurrentFlag);
                        $("#currentStatus").val(ajaxResult.RetValue.CurrentState);
                        $("#categoryID").val(ajaxResult.RetValue.CategoryID);
                        processDefine.ID = ajaxResult.RetValue.Name;//2012.11.16
                        $("#XMLdesigner_content").val(ajaxResult.RetValue.Content);
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
                    if (!ajaxResult.RetValue.processDefine.StartURL) {
                        ajaxResult.RetValue.processDefine.StartURL = "workflow / eform"
                    }
                    $("#starturl").val(ajaxResult.RetValue.processDefine.StartURL);
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
                    jsPlumb.draggable(jsPlumb.getSelector(".designeractivity"),
                       {
                           start: function (event, ui) {
                               console.log("开始拖动");
                               if (operate.length < 10) {
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                               else {
                                   operate.shift();
                                   operate.push({ bindEvent: "drag", bindObject: ui, bind: "designeractivity", result: "dragActivity" });
                               }
                           },
                           containment: $("#" + container)
                       });
                    initProcessDefine(ajaxResult.RetValue.processDefine);
                }
            });
        }
        else {
            processDefID = "ProcessDefine" + new Date().getTime();
            processDefine.ID = processDefID;
        }
        //  processDefine.ID = processDefID;
        startdraw(container);
        initDocumentContextMunu();

    }


    //连接线操作
    jsPlumb.bind("contextmenu", function (conn, originalEvent) {
        var o = { menu: "ulline", inSpeed: 150, outSpeed: 75, position: document };
        var e = originalEvent;
        e.preventDefault();
        if (window.event) //停止事件向下传播
            window.event.cancelBubble = true;
        else {
            e.stopPropagation();
        }
        var x;
        var y;
        (e.pageX) ? x = e.pageX : x = e.clientX + e.scrollLeft;
        (e.pageY) ? y = e.pageY : x = e.clientX + e.scrollTop;
        $('#' + o.menu).addClass('contextMenu');
        var menu = $('#' + o.menu, o.position);
        $(menu).css({ top: y, left: x }).fadeIn(o.inSpeed);
        $(menu).find('A').mouseover(function () {
            $(menu).find('LI.hover').removeClass('hover');
            $(this).parent().addClass('hover');
        }).mouseout(function () {
            $(menu).find('LI.hover').removeClass('hover');
        });
        $(document).keypress(function (e) {
            switch (e.keyCode) {
                case 38: // up
                    if ($(menu).find('LI.hover').size() == 0) {
                        $(menu).find('LI:last').addClass('hover');
                    } else {
                        $(menu).find('LI.hover').removeClass('hover').prevAll('LI:not(.disabled)').eq(0).addClass('hover');
                        if ($(menu).find('LI.hover').size() == 0) $(menu).find('LI:last').addClass('hover');
                    }
                    break;
                case 40: // down
                    if ($(menu).find('LI.hover').size() == 0) {
                        $(menu).find('LI:first').addClass('hover');
                    } else {
                        $(menu).find('LI.hover').removeClass('hover').nextAll('LI:not(.disabled)').eq(0).addClass('hover');
                        if ($(menu).find('LI.hover').size() == 0) $(menu).find('LI:first').addClass('hover');
                    }
                    break;
                case 13: // enter
                    $(menu).find('LI.hover A').trigger('click');
                    break;
                case 27: // esc
                    $(document).trigger('click');
                    break
            }
        });
        $('#' + o.menu, o.position).find('A').unbind('click');
        $('#' + o.menu, o.position).find('LI:not(.disabled) A').click(function () {
            $(document).unbind('click').unbind('keypress');
            $(".contextMenu", o.position).hide();
            var action = $(this).attr('href').substr(1);
            switch (action) {
                case "delControl":
                    if (confirm("是否删除该连接")) {
                        for (var i = 0; i < processDefine.Transitions.length; i++) {
                            var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                            var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                            switch (sourceAnchor) {
                                case "Left": sourceAnchor = "LeftMiddle"; break;
                                case "Top": sourceAnchor = "TopCenter"; break;
                                case "Right": sourceAnchor = "RightMiddle"; break;
                                case "Bottom": sourceAnchor = "BottomCenter"; break;
                            }
                            switch (sinkAnchor) {
                                case "Left": sinkAnchor = "LeftMiddle"; break;
                                case "Top": sinkAnchor = "TopCenter"; break;
                                case "Right": sinkAnchor = "RightMiddle"; break;
                                case "Bottom": sinkAnchor = "BottomCenter"; break;
                            }
                            if (sourceAnchor == conn.endpoints[0].anchor.type && sinkAnchor == conn.endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == conn.sourceId && processDefine.Transitions[i].DestActivity == conn.targetId) {
                                if (operate.length < 10) {
                                    operate.push({ bindEvent: "conncontextmenu", bindObject: processDefine.Transitions[i], bind: "connection", result: "delconnection" });
                                }
                                else {
                                    operate.shift();
                                    operate.push({ bindEvent: "conncontextmenu", bindObject: processDefine.Transitions[i], bind: "connection", result: "delconnection" });
                                }
                                processDefine.Transitions.splice(i, 1);
                            }
                        }
                        jsPlumb.detach(conn);

                    }
                    break;
                case "attrControl":
                    var transitionID = "";
                    for (var i = 0; i < processDefine.Transitions.length; i++) {
                        var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
                        var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
                        switch (sourceAnchor) {
                            case "Left": sourceAnchor = "LeftMiddle"; break;
                            case "Top": sourceAnchor = "TopCenter"; break;
                            case "Right": sourceAnchor = "RightMiddle"; break;
                            case "Bottom": sourceAnchor = "BottomCenter"; break;
                        }
                        switch (sinkAnchor) {
                            case "Left": sinkAnchor = "LeftMiddle"; break;
                            case "Top": sinkAnchor = "TopCenter"; break;
                            case "Right": sinkAnchor = "RightMiddle"; break;
                            case "Bottom": sinkAnchor = "BottomCenter"; break;
                        }
                        if (sourceAnchor == conn.endpoints[0].anchor.type && sinkAnchor == conn.endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == conn.sourceId && processDefine.Transitions[i].DestActivity == conn.targetId) {
                            transitionID = processDefine.Transitions[i].ID;
                            connectionLabel = conn;
                        }
                    }
                    window.parent.parent.openDialog("actionDialog3", '连接线配置', "/WorkflowDesigner/Workflow/ConnectionDetail?ProcessDefID=" + processDefine.ID + "&TransitionID=" + transitionID, 650, 380, true);
                    break;
            }
            return false;
        });
        setTimeout(function () { // Delay for Mozilla
            $(document).click(function () {
                $(menu).fadeOut(o.outSpeed);
                return false;
            });
        }, 0);
    });
    jsPlumb.bind("dblclick", function (conn, originalEvent) {
        var transitionID = "";
        for (var i = 0; i < processDefine.Transitions.length; i++) {
            var sourceAnchor = processDefine.Transitions[i].SourceOrientation;
            var sinkAnchor = processDefine.Transitions[i].SinkOrientation;
            switch (sourceAnchor) {
                case "Left": sourceAnchor = "LeftMiddle"; break;
                case "Top": sourceAnchor = "TopCenter"; break;
                case "Right": sourceAnchor = "RightMiddle"; break;
                case "Bottom": sourceAnchor = "BottomCenter"; break;
            }
            switch (sinkAnchor) {
                case "Left": sinkAnchor = "LeftMiddle"; break;
                case "Top": sinkAnchor = "TopCenter"; break;
                case "Right": sinkAnchor = "RightMiddle"; break;
                case "Bottom": sinkAnchor = "BottomCenter"; break;
            }
            if (sourceAnchor == conn.endpoints[0].anchor.type && sinkAnchor == conn.endpoints[1].anchor.type && processDefine.Transitions[i].SrcActivity == conn.sourceId && processDefine.Transitions[i].DestActivity == conn.targetId) {
                transitionID = processDefine.Transitions[i].ID;
                connectionLabel = conn;
            }
        }
        window.parent.parent.openDialog("actionDialog3", '连接线配置', "/WorkflowDesigner/Workflow/ConnectionDetail?ProcessDefID=" + processDefine.ID + "&TransitionID=" + transitionID, 650, 380, true);
    });
    jsPlumb.bind("jsPlumbConnection", function (connection) {
        if (startConnection != 0) {//1.表示流程初始化结束，开始绘制流程线
            if (startConnection == 1) {
                var label = "";
                if (connection.connection.getLabel() != null) {
                    label = connection.connection.getLabel();
                }
                if (connection.sourceId != connection.targetId) {
                    processDefine.Transitions.push({ ID: connection.connection.id, SourcePoint: { X: $("#" + connection.sourceId).offset().left - $("#main_leftcontent").width(), Y: $("#" + connection.sourceId).offset().top - $("#header").height() - $("#tabcontainer").height() - $("#designer_title").height(), Z: 0 }, SinkPoint: { X: $("#" + connection.targetId).offset().left - $("#main_leftcontent").width(), Y: $("#" + connection.targetId).offset().top - $("#header").height() - $("#tabcontainer").height() - $("#designer_title").height(), Z: 0 }, SourceOrientation: connection.sourceEndpoint.anchor.type, SinkOrientation: connection.targetEndpoint.anchor.type, SrcActivity: connection.sourceId, DestActivity: connection.targetId, Name: label, Priority: 3, IsDefault: false, Expression: "" });
                    if (operate.length < 10) {
                        operate.push({ bindEvent: "connection", bindObject: connection, bind: "connection", result: "lineConnection" });
                    }
                    else {
                        operate.shift();
                        operate.push({ bindEvent: "connection", bindObject: connection, bind: "connection", result: "lineConnection" });
                    }
                }
            }
            if (startConnection == 2) {
                temporaryObject.push({ bindEvent: "conncontextmenu", bindObject: connection, bind: "connection", result: "delconnection" });
            }
            if (startConnection == 3) {
                operate.push({ bindEvent: "connection", bindObject: connection, bind: "connection", result: "lineconnection" });
            }
        }

    });

    //同步流程图的活动名称和流程线名称
    function editActivityName(id, Name) {
        $("#" + id).find("label").text(Name);
    }
    function setConnectionLabel(text) {
        connectionLabel.getOverlay("label").setLabel(text);
    }

    window.editActivityName = editActivityName;
    window.setConnectionLabel = setConnectionLabel;
    window.WorkflowDesigner = WorkflowDesigner;
    window.processDefine = processDefine;
})(window)