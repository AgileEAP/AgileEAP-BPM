(function (window, undefined) {
    var currentActivity = new Object();
    var divList = new Array();
    var ControlType =
    {
        Text: "Text",
        TextBox: "TextBox",
        DatePicker: "DatePicker",
        MonthPicker: "MonthPicker",
        YearPicker: "MonthPicker",
        SingleCombox: "SingleCombox",
        MultiCombox: "MultiCombox",
        CheckBox: "CheckBox",
        DropDown: "DropDown",
        ChooseBox: "ChooseBox",
        ChooseTree: "ChooseTree",
        Button: "Button",
        TextArea: "TextArea",
        Radio: "Radio",
        Email: "Email",
        Combox: "Combox",
        Upload: "Upload",
        HiddenInput: "HiddenInput",
        SysVariable: "SysVariable",
        Div: "Div",
        Grid: "Grid",
        Wizard: "Wizard",
        Image: "Image",
        DataTable: "DataTable",
        Tabs: "Tabs",
        Tree: "Tree",
        Chart: "Chart"
    };
    function html_encode(str) {
        var encodeHtml = "";
        if (str.length == 0) return "";
        encodeHtml = str.replace(/&/g, "&amp;");
        encodeHtml = encodeHtml.replace(/</g, "&lt;");
        encodeHtml = encodeHtml.replace(/>/g, "&gt;");
        encodeHtml = encodeHtml.replace(/ /g, "&nbsp;");
        encodeHtml = encodeHtml.replace(/\'/g, "&#39;");
        encodeHtml = encodeHtml.replace(/\"/g, "&quot;");
        encodeHtml = encodeHtml.replace(/\n/g, "<br>");
        return encodeHtml;
    }
    function html_decode(str) {
        var decodeHtml = "";
        if (str.length == 0) return "";
        decodeHtml = str.replace(/&amp;/g, "&");
        decodeHtml = decodeHtml.replace(/&lt;/g, "<");
        decodeHtml = decodeHtml.replace(/&gt;/g, ">");
        decodeHtml = decodeHtml.replace(/&nbsp;/g, " ");
        decodeHtml = decodeHtml.replace(/&#39;/g, "\'");
        decodeHtml = decodeHtml.replace(/&quot;/g, "\"");
        decodeHtml = decodeHtml.replace(/<br>/g, "\n");
        decodeHtml = decodeHtml.replace(/<BR>/g, "\n");
        return decodeHtml;
    }
    function getUser(UserID) {
        if (UserID) {
            return $.post("/FormDesigner/Home/GetUserInfo", { UserID: UserID });
        }
        else {
            return $.post("/FormDesigner/Home/GetUserInfo");
        }
    }
    function getOrgPath(UserID) {
        if (UserID) {
            return $.post("/FormDesigner/Home/GetOrgPath", { UserID: UserID });
        }
        else {
            return $.post("/FormDesigner/Home/GetOrgPath");
        }
    }
    function getGridInfo(options) {
        var me = $("#" + options.Name);
        $.post("/FormDesigner/Home/GetTableSource", { DataSource: options.DataSource }, function (retValue) {
            var ListItems = [];
            if (form.Fields && form.Fields.length > 0) {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == options.OldID) {
                        ListItems = form.Fields[i].ListItems;
                    }
                }
            }
            var columns = [];
            if (retValue && retValue.length > 0) {
                var retColumns = retValue[0];
                var columnList = "";
                for (var columnsAttr in retColumns) {
                    var text = columnsAttr;
                    var value = columnsAttr;
                    var hide = true;

                    for (var i = 0; i < ListItems.length; i++) {
                        if (ListItems[i].Value == columnsAttr) {//|| (ListItems[i].Value.indexOf("template:") > -1 && ListItems[i].Value.indexOf(columnsAttr) > -1)) {
                            hide = false;
                        }
                        if (ListItems[i].Value.indexOf("template:") > -1) {
                            templateText = ListItems[i].Text.substring(0, ListItems[i].Text.indexOf("("));
                            bindValue = ListItems[i].Text.substring(ListItems[i].Text.indexOf("(") + 1, ListItems[i].Text.length - 1);
                            if (bindValue == columnsAttr) {
                                hide = false;
                            }
                        }
                    }
                    if (hide) {
                        columns.push({
                            field: value, title: text, hidden: true
                        });
                    }
                }
            }
            for (var j = 0; j < ListItems.length; j++) {
                var columnsSame = false;
                if (retValue && retValue.length > 0) {
                    var retColumns = retValue[0];
                    for (var columnsAttr in retColumns) {
                        var text = columnsAttr;
                        var value = columnsAttr;
                        if (ListItems[j].Value == columnsAttr) {
                            text = ListItems[j].Text;
                            value = ListItems[j].Value;
                            columnsSame = true;
                            if (value.indexOf("Time") > -1) {
                                columns.push({
                                    field: value, title: text,
                                    //  template: "<div id='" + value + "'>${ " + value + "}</div>",
                                    //  format: "{0:MM/dd/yyyy HH:mm:tt}",
                                    type: "date",
                                    format: "{0:yyyy-MM-dd}",
                                    filterable: {
                                        ui: "datetimepicker"
                                    }
                                });
                            }
                            else {
                                columns.push({
                                    title: text, field: value
                                    // template: "#="+value+"==\"test\"?1:"+value+" #"
                                });
                            }
                            break;
                        }
                        else if (ListItems[j].Value.indexOf("template:") > -1) {
                            columnsSame = true;
                            templateValue = ListItems[j].Value;
                            templateText = ListItems[j].Text.substring(0, ListItems[j].Text.indexOf("("));
                            bindValue = ListItems[j].Text.substring(ListItems[j].Text.indexOf("(") + 1, ListItems[j].Text.length - 1);
                            templateValue = templateValue.substr(templateValue.indexOf(":") + 1);
                            templateValue = templateValue;//eval("'" + templateValue + "'");
                            // columns.splice(
                            columns.push({
                                title: templateText,
                                field: bindValue,
                                template: templateValue
                            });

                            break;
                        }
                    }
                    //if (!columnsSame) {
                    //    columns.push({
                    //        field: value, title: text, hidden: true
                    //    });
                    //}
                }
                if (!columnsSame) {
                    columns.push({
                        title: ListItems[j].Text
                    });
                }
            }
            //for (var j = 0; j < ListItems.length; j++) {
            //    var text = ListItems[j].Text;
            //    var value = ListItems[j].Value;
            //    columns.push({ field: value, title: text });
            //}
            me.empty();
            var pageSize = parseInt(options.DefaultValue);
            $("#" + options.Name).kendoGrid({
                dataSource: {
                    data: retValue,
                    pageSize: pageSize ? pageSize : 5
                },
                // groupable: true,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: false
                },
                filterable: {
                    messages: {
                        info: "显示符合以下条件的行", // sets the text on top of the filter menu
                        filter: "过滤", // sets the text for the "Filter" button
                        clear: "清除过滤", // sets the text for the "Clear" button

                        // when filtering boolean numbers
                        isTrue: "true", // sets the text for "isTrue" radio button
                        isFalse: "false", // sets the text for "isFalse" radio button

                        //changes the text of the "And" and "Or" of the filter menu
                        and: "并且",
                        or: "或者"

                    },
                    operators: {
                        string: {
                            eq: "等于",
                            neq: "不等于",
                            startswith: "开始于",
                            contains: "包含",
                            doesnotcontain: "不包含",
                            endswith: "结束于"
                        },
                        date: {
                            eq: "等于",
                            neq: "不等于",
                            gte: "大于等于",
                            gt: "大于",
                            lte: "小于等于",
                            lt: "小于"
                        }
                    }
                },
                selectable: "row",
                columns: columns
            });
            me.resizable('destroy');
            me.resizable({
                alsoResize: "#" + options.Name + " .k-grid-content",
                handles: "all"
            });
            $(".ui-resizable-handle").css("display", "none");
        });
    }
    function getTreeInfo(options) {
        $.post("/FormDesigner/Home/GetTreeInfo", { DataSource: options.DataSource, NodeImg: options.DefaultValue }, function (retValue) {
            var ListItems = null;
            if (form.Fields && form.Fields.length > 0) {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == options.OldID) {
                        ListItems = form.Fields[i].ListItems;
                    }
                }
            }
            disFreeAtr(retValue);
            var icons = false;
            var plugins = ["themes", "json_data", "checkbox", "sort", "ui"];
            if (!options.Required) {
                plugins = ["themes", "json_data", "sort", "ui"];
                var icons = true;
            }
            $.ajaxSetup({ cache: false });
            $("#" + options.Name).jstree({
                "plugins": plugins,
                "themes": {
                    "theme": "default",
                    "url": "/Plugins/Resources/Content/Themes/Default/jstree/themes/default/style.css",
                    "dots": true,
                    "icons": icons
                },
                "lang": {
                    "loading": "目录加载中……"
                },
                "json_data": {
                    //"ajax": {
                    //    "url": "/FormDesigner/Home/GetTreeInfo",
                    //    "data": { "DataSource": options.DataSource, "NodeImg": options.DefaultValue }// treeContent.dataSource }
                    //},
                    "data": retValue,
                    "progressive_render": true
                },
                "checkbox": {
                    //"two_state": true
                }
            }).bind("select_node.jstree", function (e, data) {
                if (selectNode) {
                    selectNode.call(this, e, data);
                    //     alert(data.rslt.obj.data("id"));
                }
            }).bind("open_node.jstree loaded.jstree", function (e, data) {
                if (ListItems.length > 0) {
                    //document.oncontextmenu = function (e) { return false };
                    initJSTreeContextMenu(ListItems, options.Name);
                }
            });
            $("#" + options.Name).css("overflow", "auto");
            //$("#" + options.Name).wrap("<div class='wrap'></div>");
            //$("#" + options.Name).resizable({
            //   // alsoResize: "#" + options.Name,
            //    handles: "all"
            //});
            $(".ui-resizable-handle").css("display", "none");
        });
    }
    function resizeWindow() {
        var windowHeight = getWindowHeight();
        if (windowHeight == 0) return;
        if ($("#" + "actionDialog", window.parent.document).length > 0 && window.parent != window) {
            $("#" + "actionDialog", window.parent.document).parent().css("left", "200px");
            $("#" + "actionDialog", window.parent.document).parent().css("top", "50px");
            $("#" + "actionDialog", window.parent.document).parent().css("height", "669px");
            $("#" + "actionDialog", window.parent.document).parent().css("width", "1050px");
            window.parent.document.getElementById("actionDialog").style.height = "669px";
            window.parent.document.getElementById("actionDialog").style.width = "1050px";
            document.getElementById("mainbody").style.height = windowHeight - $("#header").height() - 38 + "px";
            document.getElementById("leftcontainer").style.height = windowHeight - $("#header").height() - 38 + "px";
            document.getElementById("maincontainer").style.height = windowHeight - $("#header").height() - 38 + "px";
            document.getElementById("attrcontainer").style.height = windowHeight - $("#header").height() - 38 + "px";
            document.getElementById("container_designer").style.height = windowHeight - $("#header").height() - 108 + "px";
            document.getElementById("htmlcontainer_designer").style.height = windowHeight - $("#header").height() - 108 + "px";
            document.getElementById("jsoncontainer_designer").style.height = windowHeight - $("#header").height() - 108 + "px";
            document.getElementById("csscontainer_designer").style.height = windowHeight - $("#header").height() - 108 + "px";
            document.getElementById("jscontainer_designer").style.height = windowHeight - $("#header").height() - 108 + "px";
            document.getElementById("maincontainer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 8 + "px";
            document.getElementById("container_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("htmlcontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("jsoncontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("csscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("jscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
        }
        else {
            document.getElementById("mainbody").style.height = windowHeight - $("#header").height() - 8 + "px";
            document.getElementById("leftcontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            document.getElementById("maincontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            document.getElementById("attrcontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            document.getElementById("container_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            document.getElementById("htmlcontainer_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            document.getElementById("jsoncontainer_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            document.getElementById("csscontainer_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            document.getElementById("jscontainer_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            document.getElementById("maincontainer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 8 + "px";
            document.getElementById("container_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("htmlcontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("jsoncontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("csscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
            document.getElementById("jscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 54 + "px";
        }
    }
    function divContain(id) {
        if (divList && divList.length > 0) {
            for (var i = 0; i < divList.length; i++) {
                var me = $("#" + divList[i]);
                if (me.css("display") != "none") {
                    var preleft = me.parent().parent().parent().position().left;//me.position().left;
                    var pretop = me.parent().parent().parent().position().top; //me.position().top;
                    var left = $("#" + id).position().left;
                    var top = $("#" + id).position().top;
                    if (left > preleft && top > pretop && left + $("#" + id).width() < preleft + me.width() && top + $("#" + id).height() < pretop + me.height()) {
                        if (form.Fields && form.Fields.length > 0) {
                            for (var j = 0; j < form.Fields.length; j++) {
                                if (form.Fields[j].Name == id) {
                                    form.Fields[j].Container = divList[i];
                                }
                            }
                        }
                        $("#" + id).attr("parentControl", divList[i]);
                    }
                    else {
                        if (form.Fields && form.Fields.length > 0) {
                            for (var j = 0; j < form.Fields.length; j++) {
                                if (form.Fields[j].Name == id) {
                                    form.Fields[j].Container = "";
                                }
                            }
                        }
                        $("#" + id).attr("parentControl", "");
                    }
                }
            }
        }
    }
    function initDocumentContextMenu() {
        $("#container_designer").contextMenu({
            menu: 'uldocument'
        }, function (action, el, pos) {
            if (action == "pasteControl") {
                if (currentActivity) {
                    currentActivity.X = pos.docX - 180;
                    currentActivity.Y = pos.docY - 55;
                    formDesigner.addItem(currentActivity);
                    addField(currentActivity);
                    currentActivity = null;
                }
            }
        })
    };
    document.oncontextmenu = function (e) { return false };
    function initContextMenu() {
        $(".control").contextMenu({
            menu: 'ulcontrol'
        }, function (action, el, pos) {
            var controlID = $(el.context).attr("id");
            if ($(el.context).attr("controlType") == ControlType.Wizard || $(el.context).attr("controlType") == ControlType.Tabs || $(el.context).attr("controlType") == ControlType.Tree) {
                controlID = $("#" + controlID).children()[0].id;
            }
            if (action == "delControl") {
                if (confirm("您确定要删除该控件吗？")) {
                    for (var i = 0; i < form.Fields.length; i++) {
                        if (form.Fields[i].Name == controlID) {
                            var position = form.Fields.indexOf(form.Fields[i]);
                            form.Fields.splice(position, 1);
                            if ($(el.context).attr("controlType") == ControlType.Wizard || $(el.context).attr("controlType") == ControlType.Tabs || $(el.context).attr("controlType") == ControlType.Tree) {
                                $("#" + controlID).parent().remove();
                            }
                            else {
                                $("#" + controlID).remove();
                            }
                        }
                    }
                }
                return;
            }
            if (action == "cutControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        currentActivity = form.Fields[i];
                        var position = form.Fields.indexOf(form.Fields[i]);
                        form.Fields.splice(position, 1);
                        if ($(el.context).attr("controlType") == ControlType.Wizard || $(el.context).attr("controlType") == ControlType.Tabs || $(el.context).attr("controlType") == ControlType.Tree) {
                            $("#" + controlID).parent().remove();
                        }
                        else {
                            $("#" + controlID).remove();
                        }
                    }
                }
                return;
            }
            if (action == "copyControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        for (var field in form.Fields[i]) {
                            if (!currentActivity) {
                                currentActivity = new Object();
                            }
                            currentActivity.Name = new Date().getTime();
                            currentActivity[field] = form.Fields[i][field];
                        }
                        //currentActivity = form.Fields[i];

                    }
                }
                return;
            }
            if (action == "leftControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";text-align:left;";
                    }
                }
                $("#" + controlID).css("text-align", "left");
                return;
            }
            if (action == "centerControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";text-align:center;";
                    }
                }
                $("#" + controlID).css("text-align", "center");
                return;
            }
            if (action == "rightControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";text-align:right;";
                    }
                }
                $("#" + controlID).css("text-align", "right");
                return;
            }
            if (action == "upControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";text-align:right;";
                    }
                }
                $("#" + controlID).css("text-align", "right");
                return;
            }
            if (action == "bottomControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";text-align:right;";
                    }
                }
                $("#" + controlID).css("text-align", "right");
                return;
            }
            if (action == "middleControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        form.Fields[i].CustomStyle = form.Fields[i].CustomStyle + ";" + "line-height:" + $("#" + controlID).css("height") + ";";
                    }
                }
                $("#" + controlID).css("line-height", $("#" + controlID).css("height"));
                return;
            }
            if (action == "attrControl") {
                for (var i = 0; i < form.Fields.length; i++) {
                    if (form.Fields[i].Name == controlID) {
                        switch (form.Fields[i].ControlType) {
                            case ControlType.Text:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/TextConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Div:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/TextConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.ChooseBox:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/Choosebox?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Button:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/ButtonConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Radio:
                            case ControlType.DropDown:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/ChoiceBox?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.SysVariable:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/SystemControl?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Chart:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/ChartConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Tree:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/TreeConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.DataTable:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            case ControlType.Grid:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                            default:
                                window.parent.parent.openDialog("actionDialog2", "配置" + form.Fields[i].ControlType + "属性", "/FormDesigner/Home/ConfigureControl?ControlType=" + form.Fields[i].ControlType + "&ControlID=" + controlID, 650, 380, true);
                                break;
                        }

                    }
                }

                return;
            }
        });
    }
    function editeText(field) {
        $("#" + field.Name).bind("dblclick", function () {
            $("#" + field.Name).resizable('destroy');
            var editid = field.Name + "editlable";
            var value;
            if (!document.getElementById(editid)) {
                value = $("#" + field.Name).html();
                $("#" + field.Name).html("");
                // var inputitem = "<input id=\"" + editid + "\" type='text' style='width:100%;height:100%;border:0px' />";
                var inputitem = "<textarea id=\"" + editid + "\" type='text' style='width:100%;height:100%;border:0px;_height:'" + $("#" + field.Name).css("height") + "></textarea>";
                $(this).append(inputitem);
            }
            else {
                $("#" + editid).show();
            }
            $("#" + field.Name).resizable({
                alsoResize: "#" + field.Name + "editlable",
                handles: "all"
            });
            if (value) {
                value = html_decode(value);
                $("#" + editid).val(value);
                $("#" + field.Name + "editlable").bind("click", function (e) {
                    //  console.log("2");
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $(document).bind("click", function () {
                    var value = $("#" + editid).val();
                    value = html_encode(value);
                    $("#" + editid).hide();
                    $("#" + field.Name).html(value);
                    document.onmousedown = null;
                    formDesigner.editItem({ ID: field.Name, OldID: field.Name, Text: value });
                    $("#" + field.Name).resizable('destroy');
                    $("#" + field.Name).resizable({ handles: "all" });
                    $(".ui-resizable-handle").css("display", "none");
                    $("#" + field.Name).enableContextMenu();
                    $("#container_designer").enableContextMenu();
                    $(document).unbind("click");
                });
            }
            $("#" + field.Name).disableContextMenu();
            $("#container_designer").disableContextMenu();
        });
        $("#" + field.Name).resizable({ handles: "all" });
        initContextMenu();
    }
    function addField(field) {
        switch (field.ControlType) {
            case ControlType.Image:
                if (!field.DefaultValue) {
                    field.DefaultValue = '/plugins/FormDesigner/Content/Themes/Default/Images/blankImg.png';
                }
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Image + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><img id=\"" + field.Name + "Image\" src=\"" + field.DefaultValue + "\"</div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "Image",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Tabs:
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Wizard:
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Grid:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Grid + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;left:" + field.X + "px;top:" + field.Y + "px;\" id=\"" + field.Name + "\"></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.DataTable:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.DataTable + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;\" id=\"" + field.Name + "\"></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Tree:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Tree + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "tree\"><div id=\"" + field.Name + "\"></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).parent().bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).parent().resizable({
                    alsoResize: "#" + field.Name,
                    handles: "all"
                });
                $("#" + field.Name).parent().draggable({
                    start: function (event, ui) {
                        $("#" + field.Name).attr("startDraggable_left", ui.position.left);
                        $("#" + field.Name).attr("startDraggable_top", ui.position.top);
                    },
                    cursor: "move",
                    containment: "#container_designer"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Chart:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Chart + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    //alsoResize: "#" + field.Name + "chart",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Text:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Text + "\" style=\"position:absolute;z-index:10;border:1px solid #AAA;width:" + field.Width + "px;height:" + field.Height + "px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\">" + field.Text + "</div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                editeText(field);
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.HiddenInput:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.HiddenInput + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' data-bind=\"value:DefaultValue\" id=\"" + field.Name + "HiddenInput\" style=\"height:20px;width:170px\"></input></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "HiddenInput",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.TextBox:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.TextBox + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' data-bind=\"value:DefaultValue\" id=\"" + field.Name + "textbox\" style=\"height:20px;width:184px\"></input></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "textbox",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.TextArea:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.TextArea + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:45px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><textarea id=\"" + field.Name + "textarea\" data-bind=\"value:DefaultValue\" style=\"height:40px;width:184px\"></textarea></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "textarea",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Button:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Button + "\"  style=\"position:absolute;z-index:10;width:70px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div class='waterfall' id=\"" + field.Name + "Waterfall\" style='width:70px;height:25px;position:absolute;'></div><input type=\"button\" id=\"" + field.Name + "button\" style=\"width:100%;height:100%\" value='按钮'/></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "button",
                    handles: "all"
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "Waterfall",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.CheckBox:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.CheckBox + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left; height:25px;line-height:25px;'><label >选择项</label></div><div style='float:left; height:28px;line-height:28px;'><input type='checkbox' id=\"" + field.Name + "checkbox\"></input></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    handles: "all"
                    //   alsoResize: "#" + id + "checkbox"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Radio:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Radio + "\"  style=\"position:absolute;text-algin:left;z-index:10;border:1px solid #AAA;width:240px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left; height:25px;line-height:25px; '><input type='radio' id=\"" + field.Name + "radio\"></input></div><div style='height:25px;line-height:25px;'><label >选择项</label></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    handles: "all"
                    //  alsoResize: "#" + id + "radio"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.DropDown:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.DropDown + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left; height:25px;line-height:25px;'><label>下拉控件名：</label></div><div style='float:left;'><select id=\"" + field.Name + "dropdown\" style=\"height:25px;width:162px\"><option>item1</option><option>item2</option></select></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "dropdown",
                    handles: "all"
                });
                $("#" + field.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置DropDown属性', "/FormDesigner/Home/ConfigureControl?ControlType=DropDown&ControlID=" + field.Name, 650, 380, true);
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.DatePicker:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.DatePicker + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left; height:25px;line-height:25px;'><label>时间：</label></div><div style='float:left;'><input data-bind=\"value:DefaultValue\" type='text' id=\"" + field.Name + "date\"  class=\"Wdate\" style=\"height:20px;width:197px\" onclick='WdatePicker()'/></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "date",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;

            case ControlType.Email:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Email + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left; height:25px;line-height:25px;'><label>邮箱：</label></div><div style='float:left;'><input data-bind=\"value:DefaultValue\" type='email' id=\"" + field.Name + "email\" style=\"height:20px;width:197px\"/></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "email",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.ChooseBox:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.ChooseBox + "\"  style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' id=\"" + field.Name + "choosebox\" readonly='readonly' style=\"height:20px;width:184px;cursor:pointer;background:url(/Plugins/FormDesigner/Content/Themes/Default/Images/Combox.png) no-repeat right\"></input></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "choosebox",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Upload:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Upload + "\"  style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >附件：</label></div><div style='float:left;'><div id=\"" + field.Name + "upload\" style=\"height:25px;line-height:25px;width:184px;\"><img src='/Plugins/Workflow/Content/Themes/Default/Images/attachment.png' style='margin-bottom:-3px'/>上传附件/下载附件</div></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "upload",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.SysVariable:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.SysVariable + "\"  style=\"position:absolute;z-index:10;border:1px solid #AAA;width:240px;height:25px;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' data-bind=\"value:DefaultValue\" id=\"" + field.Name + "SysVariable\" style=\"height:20px;width:170px\"></input></div></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    alsoResize: "#" + field.Name + "SysVariable",
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;
            case ControlType.Div:
                var item = "<div class=\"control\" parentControl=\"" + field.Container + "\" controlType=\"" + ControlType.Div + "\"  style=\"position:absolute;z-index:5;border:1px solid #AAA;left:" + field.X + "px;top:" + field.Y + "px;" + field.CustomStyle + "\"id=\"" + field.Name + "\"></div>";
                $("#container_designer").append(item);
                $("#" + field.Name).bind("mousedown", function () {
                    e = window.event || arguments[0];
                    var me = $(this);
                    if (e.ctrlKey) {
                        me.attr("selectcontrol", true);
                    }
                    else {
                        $(".control").each(function () {
                            if ($(this).attr("standardcontrol")) {
                                $(this).removeAttr("standardcontrol");
                            }
                        });
                        $(".ui-resizable-handle").css("display", "none");
                        me.attr("standardcontrol", true);
                    }
                    me.find(".ui-resizable-handle").css("display", "block")
                    if (window.event) //停止事件向下传播
                        window.event.cancelBubble = true;
                    else {
                        e.stopPropagation();
                    }
                });
                $("#" + field.Name).resizable({
                    handles: "all"
                });
                field.OldID = field.Name;
                setContorlValue(field);
                break;

        }
        if (field.ControlType != ControlType.Wizard && field.ControlType != ControlType.Tabs && field.ControlType != ControlType.Tree) {
            $("#" + field.Name).draggable({
                start: function (event, ui) {
                    $("#" + field.Name).attr("startDraggable_left", ui.position.left);
                    $("#" + field.Name).attr("startDraggable_top", ui.position.top);
                },
                cursor: "move",
                containment: "#container_designer"
            });
        }
        initContextMenu();
    }
    function formInfos(options) {
        var saveform = $.extend({}, form, options);
        var formFields = saveform.Fields;
        for (var i = 0; i < formFields.length; i++) {
            var x = 0;
            var y = 0
            if (formFields[i].ControlType == ControlType.Wizard || formFields[i].ControlType == ControlType.Tabs || formFields[i].ControlType == ControlType.Tree) {
                x = $("#" + formFields[i].Name).parent().css("left");
                y = $("#" + formFields[i].Name).parent().css("top");
            }
            else {
                x = $("#" + formFields[i].Name).css("left");
                y = $("#" + formFields[i].Name).css("top");
            }
            formFields[i].X = x.substring(0, x.lastIndexOf("px"));
            formFields[i].Y = y.substring(0, y.lastIndexOf("px"));
            if (formFields[i].Width.toString().indexOf("%") < 0) {
                formFields[i].Width = $("#" + formFields[i].Name).width();
            }
            if (formFields[i].Height.toString().indexOf("%") < 0) {
                formFields[i].Height = $("#" + formFields[i].Name).height();
            }
            if ($("#" + formFields[i].Name).find("input")[0] != undefined)
                if ($("#" + formFields[i].Name).find("input").attr("type") == "checkbox") {
                    if ($("#" + formFields[i].Name).find("input:checked")[0] != undefined) {
                        formFields[i].DefaultValue = 1;
                    }
                    else {
                        formFields[i].DefaultValue = 0;
                    }
                }
                else
                    formFields[i].DefaultValue = $("#" + formFields[i].Name).find("input").val();
            if ($("#" + formFields[i].Name).find("textarea")[0] != undefined)
                formFields[i].DefaultValue = $("#" + formFields[i].Name).find("textarea").val();
        }
        return saveform;
    }
    function initForm() {
        if (form.Script) {
            if ($("#jscontainer_designer").length > 0) {
                $("#jscontainer_designer").val(decodeURIComponent(form.Script));
                // var po = document.createElement("script");
                //var jsContent=document.createTextNode(decodeURIComponent(form.Script));
                //po.type = "text/javascript";
                //po.appendChild(jsContent);
                // var s = document.getElementsByTagName("script")[0];
                // s.parentNode.appendChild(po);
                // (function () {
                //  eval(decodeURIComponent(form.Script));
                // })();
            }
        }
        if (form.Style) {
            if ($("#csscontainer_designer").length > 0) {
                $("#csscontainer_designer").val(decodeURIComponent(form.Style));
                (function () {
                    var sStyle = document.createElement("style");
                    sStyle.setAttribute("type", "text/css");
                    if (sStyle.styleSheet) { //ie
                        sStyle.styleSheet.cssText = decodeURIComponent(form.Style);
                    } else {
                        var csstext = document.createTextNode(decodeURIComponent(form.Style));
                        sStyle.appendChild(csstext);
                    }
                    document.getElementsByTagName('head')[0].appendChild(sStyle);
                })();
            }
        }
        if (form.Fields) {
            var WizardControl = new Array();
            var TabsControl = new Array();
            for (var i = 0; i < form.Fields.length; i++) {
                var field = form.Fields[i];
                if (ControlType.Wizard == field.ControlType) {
                    WizardControl.push(field);
                }
                else if (ControlType.Tabs == field.ControlType) {
                    TabsControl.push(field);
                }
                else {
                    addField(field);
                }
            }
            for (var m = 0; m < TabsControl.length; m++) {
                addField(TabsControl[m]);
            }
            for (var j = 0; j < WizardControl.length; j++) {
                addField(WizardControl[j]);
            }
        }

    }
    var initUI = function () {
        $("#tabs").tabs();
        // fix the classes
        $(".tabs-bottom .ui-tabs-nav, .tabs-bottom .ui-tabs-nav > *")
                .removeClass("ui-corner-all ui-corner-top")
                .addClass("ui-corner-bottom");
        $(".tabs-bottom .ui-tabs-nav").appendTo(".tabs-bottom");
        $("#left_accordion").accordion({
            collapsible: true
        });

    };
    function initDesign(formDesignerContain) {
        $("#" + formDesignerContain).droppable({
            drop: function (event, ui) {
                var top = $("#header").height();
                var left = 0;
                if ($("#leftcontainer").css("position") != "absolute") {
                    left = $("#leftcontainer").width();
                }
                var scrollLeft = $(this).scrollLeft();
                var scrollTop = $(this).scrollTop();
                var itemtop = ui.offset.top - top + scrollTop;
                var itemleft = ui.offset.left - left + scrollLeft;
                var id = new Date().getTime();
                if (ui.draggable[0].className.indexOf('left_li ui-draggable') < 0) {
                    id = ui.draggable[0].id;
                    if ($("#" + ui.draggable[0].id).attr("controlType") == ControlType.Div) {
                        var me = $("#" + ui.draggable[0].id);
                        var preleft = parseFloat(me.attr("startDraggable_left"));
                        var pretop = parseFloat(me.attr("startDraggable_top"));
                        var endleft = ui.position.left;
                        var endtop = ui.position.top;
                        var left = endleft - preleft;
                        var top = endtop - pretop;
                        $(".control").each(function () {
                            if ($(this).position().left > preleft && $(this).position().top > pretop && $(this).position().left + $(this).width() < preleft + me.width() && $(this).position().top + $(this).height() < pretop + me.height()) {
                                this.style.left = left + $(this).position().left + "px";
                                this.style.top = top + $(this).position().top + "px";
                            }
                        });
                    }
                    else if ($("#" + ui.draggable[0].id).attr("controlType") == ControlType.Wizard || $("#" + ui.draggable[0].id).attr("controlType") == ControlType.Tabs) {
                        var me = $("#" + ui.draggable[0].id);
                        var preleft = parseFloat(me.attr("startDraggable_left"));
                        var pretop = parseFloat(me.attr("startDraggable_top"));
                        var endleft = ui.position.left;
                        var endtop = ui.position.top;
                        var left = endleft - preleft;
                        var top = endtop - pretop;
                        $(".control").each(function () {
                            var meControl = $(this);
                            if (meControl.attr("parentControl") && meControl.attr("parentControl") != "null") {
                                this.style.left = left + parseFloat(meControl.css("left").substring(0, meControl.css("left").lastIndexOf("px"))) + "px";
                                this.style.top = top + parseFloat(meControl.css("top").substring(0, meControl.css("top").lastIndexOf("px"))) + "px";
                            }
                        });
                    }
                }
                else {
                    $(".ui-resizable-handle").css("display", "none");
                    switch ($.trim($(ui.draggable[0]).text())) {
                        case ControlType.Tree:
                            var item = "<div class=\"control\" controlType=\"" + ControlType.Tree + "\"   style=\"position:absolute; z-index: 10;width:200px;height:300px; border:1px solid #AAA;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div id=\"" + id + "tree\" style=\"width:200px;height:300px;\">无数据</div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "tree",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id + "tree", X: itemleft, Y: itemtop, Name: id + "tree", ControlType: ControlType.Tree, Text: "" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Tree属性', "/FormDesigner/Home/TreeConfigure?ControlType=" + ControlType.Tree + "&ControlID=" + id + "tree", 650, 530, true);
                            });
                            break;
                        case ControlType.Chart:
                            var item = "<div class=\"control\" controlType=\"" + ControlType.Chart + "\"   style=\"position:absolute; z-index: 10; border:1px solid #AAA;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div id=\"" + id + "chart\"><img src=\"/plugins/FormDesigner/Content/Themes/Default/Images/lineChart.png\" alt=\"图片\"></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "chart",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.Chart });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Chart属性', "/FormDesigner/Home/ChartConfigure?ControlType=" + ControlType.Chart + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Tabs:
                            formDesigner.addItem({ ID: id + "Tabs", X: itemleft, Y: itemtop, Name: id + "Tabs", ControlType: ControlType.Tabs, Width: 611, Height: 477 });
                            window.parent.openDialog("actionDialog2", '配置Tabs属性', "/FormDesigner/Home/WizardConfigure?ControlType=" + ControlType.Tabs + "&ControlID=" + id + "Tabs", 650, 380, true);
                            break;
                        case ControlType.Image:
                            var item = "<div class=\"control\" controlType=\"" + ControlType.Image + "\"   style=\"position:absolute; z-index: 10; border:1px solid #AAA;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><img id=\"" + id + "image\" src=\"/plugins/FormDesigner/Content/Themes/Default/Images/blankImg.png\" alt=\"图片\"></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "image",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.Image });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Image属性', "/FormDesigner/Home/ConfigureControl?ControlType=" + ControlType.Image + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Wizard:
                            formDesigner.addItem({ ID: id + "wizard", X: itemleft, Y: itemtop, Name: id + "wizard", ControlType: ControlType.Wizard, Width: 611, Height: 477 });
                            window.parent.openDialog("actionDialog2", '配置wizard属性', "/FormDesigner/Home/WizardConfigure?ControlType=" + ControlType.Wizard + "&ControlID=" + id + "wizard", 650, 380, true);
                            break;
                        case ControlType.Text:
                            var item = "<div controlType=\"" + ControlType.Text + "\"  class=\"control\" style=\"position:absolute;z-index: 10;font-size:12px;border:1px solid #AAA;width:100px;height:20px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            formDesigner.addItem({ ID: id, Name: id, X: itemleft, Y: itemtop, ControlType: "Text", DataType: "String", AccessPattern: "ReadOnly" });
                            $("#" + id).bind("dblclick", function () {
                                // document.onmousedown = null;
                                $("#" + id).resizable('destroy');
                                var editid = id + "editlable";
                                var value;
                                if (!document.getElementById(editid)) {
                                    value = $("#" + id).html();
                                    if (!value) {
                                        value = "文本";
                                    }
                                    $("#" + id).html("");
                                    //  var inputitem = "<input id=\"" + editid + "\" type='text' style='width:100%;height:100%;border:0px' />";
                                    var inputitem = "<textarea id=\"" + editid + "\" type='text' style='width:100%;height:100%;border:0px;_height:" + ($("#" + id).height() - 1) + "px'></textarea>";
                                    $(this).append(inputitem);
                                }
                                else {
                                    $("#" + editid).show();
                                }
                                $("#" + id).resizable({
                                    alsoResize: "#" + id + "editlable",
                                    handles: "all"
                                });
                                if (value) {
                                    value = html_decode(value);
                                    $("#" + editid).val(value);
                                    $("#" + id + "editlable").bind("click", function (e) {
                                        //  console.log("2");
                                        if (window.event) //停止事件向下传播
                                            window.event.cancelBubble = true;
                                        else {
                                            e.stopPropagation();
                                        }
                                    });

                                    $(document).bind("click", function () {
                                        var value = $("#" + editid).val();
                                        value = html_encode(value);
                                        $("#" + editid).hide();
                                        $("#" + id).html(value);
                                        document.onmousedown = null;
                                        formDesigner.editItem({ ID: id, OldID: id, Text: value, Name: id });
                                        $("#" + id).resizable('destroy');
                                        $("#" + id).resizable({ handles: "all" });
                                        $(".ui-resizable-handle").css("display", "none");
                                        $(document).unbind("click");
                                    });
                                }
                            });
                            $("#" + id).resizable({ handles: "all" });
                            break;
                        case ControlType.TextBox:
                            var item = "<div class=\"control\" controlType=\"" + ControlType.TextBox + "\"   style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' data-bind=\"value:DefaultValue\" id=\"" + id + "textbox\" style=\"height:20px;width:184px\"></input></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "textbox",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "TextBox" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置TextBox属性', "/FormDesigner/Home/ConfigureControl?ControlType=TextBox&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.TextArea:
                            var item = "<div controlType=\"" + ControlType.TextArea + "\"   class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:45px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><textarea id=\"" + id + "textarea\" data-bind=\"value:DefaultValue\" style=\"height:40px;width:184px\"></textarea></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "textarea",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "TextArea" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置TextArea属性', "/FormDesigner/Home/ConfigureControl?ControlType=TextArea&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Button:
                            var item = "<div controlType=\"" + ControlType.Button + "\"   class=\"control\" style=\"position:absolute;z-index:10;width:70px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div class='waterfall' id=\"" + id + "Waterfall\" style='width:70px;height:25px;position:absolute;'></div><input type=\"button\" id=\"" + id + "button\" style=\"width:100%;height:100%\" value='按钮'/></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "button",
                                handles: "all"
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "Waterfall",
                                handles: "all"

                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "Button" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置Button属性', "/FormDesigner/Home/ButtonConfigure?ControlType=Button&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.CheckBox:
                            var item = "<div controlType=\"" + ControlType.CheckBox + "\"   class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left; height:25px;line-height:25px;'><label >选择项</label></div><div style='float:left; height:28px;line-height:28px;'><input type='checkbox' data-bind=\"value:DefaultValue\" id=\"" + id + "checkbox\"></input></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                handles: "all"
                                //   alsoResize: "#" + id + "checkbox"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "CheckBox" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置CheckBox属性', "/FormDesigner/Home/ConfigureControl?ControlType=CheckBox&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Radio:
                            var item = "<div controlType=\"" + ControlType.Radio + "\"   class=\"control\" style=\"position:absolute;text-align:left; z-index: 10;border:1px solid #AAA;width:240px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left; height:25px;line-height:25px; '><input  type='radio' id=\"" + id + "checkbox\" value=\"" + id + "\"></input></div><div style='height:25px;line-height:25px;'><label >选择项</label></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                handles: "all"
                                //  alsoResize: "#" + id + "radio"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.Radio, ListItems: [{ Value: id, Text: "选择项" }] });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置Radio属性', "/FormDesigner/Home/ChoiceBox?ControlType=" + ControlType.Radio + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.DropDown:
                            var item = "<div controlType=\"" + ControlType.DropDown + "\"   class=\"control\" style=\"position:absolute; z-index: 10;border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left; height:25px;line-height:25px;'><label>下拉控件名：</label></div><div style='float:left;'><select id=\"" + id + "dropdown\" style=\"height:25px;width:162px\"><option>item1</option><option>item2</option></select></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "dropdown",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "DropDown" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置DropDown属性', "/FormDesigner/Home/ChoiceBox?ControlType=DropDown&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.DatePicker:
                            var item = "<div controlType=\"" + ControlType.DatePicker + "\"   class=\"control\" style=\"position:absolute; z-index: 10;border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left; height:25px;line-height:25px;'><label>时间：</label></div><div style='float:left;'><input data-bind=\"value:DefaultValue\" type='text' class=\"Wdate\" id=\"" + id + "date\" style=\"height:20px;width:197px\" onfocus='WdatePicker()'/></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "date",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "DatePicker" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置DateTime属性', "/FormDesigner/Home/ConfigureControl?ControlType=DatePicker&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Email:
                            var item = "<div controlType=\"" + ControlType.Email + "\"   class=\"control\" style=\"position:absolute; z-index: 10;border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left; height:25px;line-height:25px;'><label>邮箱：</label></div><div style='float:left;'><input data-bind=\"value:DefaultValue\" type='email' id=\"" + id + "email\" style=\"height:20px;width:197px\"/></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "email",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "Email" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.parent.openDialog("actionDialog2", '配置Email属性', "/FormDesigner/Home/ConfigureControl?ControlType=Email&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.ChooseBox:
                            var item = "<div controlType=\"" + ControlType.ChooseBox + "\"   class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >控件名：</label></div><div style='float:left;'><input type='text' id=\"" + id + "choosebox\" readonly='readonly' style=\"height:20px;width:184px;cursor:pointer;background:url(/Plugins/FormDesigner/Content/Themes/Default/Images/Combox.png) no-repeat right\"></input></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "choosebox",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "ChooseBox" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置ChooseBox属性', "/FormDesigner/Home/Choosebox?ControlType=ChooseBox&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Upload:
                            var item = "<div controlType=\"" + ControlType.Upload + "\"   class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >附件：</label></div><div id=\"" + id + "upload\" style=\"height:25px;line-height:25px;width:184px;\"><img src='/Plugins/Workflow/Content/Themes/Default/Images/attachment.png' style='margin-bottom:-3px'/>上传附件/下载附件</div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "upload",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: "Upload" });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Upload属性', "/FormDesigner/Home/ConfigureControl?ControlType=Upload&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.HiddenInput:
                            var item = "<div controlType=\"" + ControlType.HiddenInput + "\"   class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >隐藏控件：</label></div><div style='float:left;'><input type='text' data-bind=\"value:DefaultValue\" id=\"" + id + "Hidden\" style=\"height:20px;width:170px\"></input></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "Input",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.HiddenInput });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Hidden属性', "/FormDesigner/Home/ConfigureControl?ControlType=" + ControlType.HiddenInput + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.SysVariable:
                            var item = "<div  controlType=\"" + ControlType.SysVariable + "\"  class=\"control\" style=\"position:absolute; z-index: 10; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"><div style='float:left;height:25px;line-height:25px;'><label >系统控件：</label></div><div style='float:left;'><input type='text' readonly='readonly' data-bind=\"value:DefaultValue\" id=\"" + id + "SysVariable\" style=\"height:20px;width:170px\"></input></div></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                alsoResize: "#" + id + "SysVariable",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.SysVariable });
                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置SysVariable属性', "/FormDesigner/Home/SystemControl?ControlType=" + ControlType.SysVariable + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Div:
                            var item = "<div  controlType=\"" + ControlType.Div + "\"  class=\"control\" style=\"position:absolute; z-index: 5; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\"></div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                //alsoResize: "#" + id + "Div",
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.Div });

                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置Div容器属性', "/FormDesigner/Home/TextConfigure?ControlType=" + ControlType.Div + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.Grid:
                            var item = "<div  controlType=\"" + ControlType.Grid + "\"  class=\"control\" style=\"position:absolute; z-index: 5; border:1px solid #AAA;width:560px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\">双击绑定Grid</div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.Grid, Required: true });

                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置' + ControlType.Grid + '容器属性', "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + ControlType.Grid + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                        case ControlType.DataTable:
                            var item = "<div  controlType=\"" + ControlType.DataTable + "\"  class=\"control\" style=\"position:absolute; z-index: 5; border:1px solid #AAA;width:240px;height:25px;left:" + itemleft + "px;top:" + itemtop + "px;\"id=\"" + id + "\">双击绑定DataTable</div>";
                            $(this).append(item);
                            $("#" + id).bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + id).resizable({
                                handles: "all"
                            });
                            formDesigner.addItem({ ID: id, X: itemleft, Y: itemtop, Name: id, ControlType: ControlType.DataTable, Required: true });

                            $("#" + id).bind("dblclick", function () {
                                window.parent.openDialog("actionDialog2", '配置' + ControlType.DataTable + '容器属性', "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + ControlType.DataTable + "&ControlID=" + id, 650, 380, true);
                            });
                            break;
                    }
                    $("#" + id).attr("startDraggable_left", ui.position.left);
                    $("#" + id).attr("startDraggable_top", ui.position.top);

                }
                if ($("#" + id).length > 0) {
                    divContain(id);
                }
                $("#" + id).draggable({
                    start: function (event, ui) {
                        $("#" + id).attr("startDraggable_left", ui.position.left);
                        $("#" + id).attr("startDraggable_top", ui.position.top);
                    },
                    cursor: "move",
                    containment: "#container_designer",
                    handles: "all"
                });
            }
        });
        $("#" + formDesignerContain).bind("mousedown", function () {
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    $(this).removeAttr("standardcontrol");
                }
                if ($(this).attr("selectcontrol")) {
                    $(this).removeAttr("selectcontrol");
                }
            });
            $(".control").each(function () {
                if ($(this).attr("controltype") == ControlType.Div) {
                    var me = $(this);
                    var preleft = parseFloat(me.css("left").substring(0, me.css("left").lastIndexOf("px")));
                    var pretop = parseFloat(me.css("top").substring(0, me.css("top").lastIndexOf("px")));
                    if ($(this).css("display") == "none") {
                        $(".control").each(function () {
                            var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                            var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                            if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                                $(this).hide();
                            }
                        });
                    }
                    else {
                        $(".control").each(function () {
                            var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                            var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                            if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                                $(this).show();
                            }
                        });
                    }
                }
            });
            $(".ui-resizable-handle").css("display", "none");
        });
    }
    function initEvent(formDesignerContain) {
        initDocumentContextMenu();
        $(".left_li").bind("mouseover", function () {
            this.style.background = "#cecece";
            $(this).css("z-index", "999999");
        });
        $(".left_li").bind("mouseout", function () {
            this.style.background = "white";
        });
        $(".left_li").draggable({
            cursor: "move", helper: "clone"
        });
        $("#tabs").on("tabsbeforeactivate", function (event, ui) {
            if (ui.oldTab.find("a").attr("id") === "json" && ui.newTab.find("a").attr("id") === "view") {
                formDesigner.init({ formfirstInit: false, formContent: $("#jsoncontainer_designer").val() });
            }
            else if (ui.newTab.find("a").attr("id") === "json") {
                var Name = $("#formName").val();
                var Title = $("#formTitle").val();
                var DataSource = $("#formDataSource").val();
                var Script = encodeURIComponent($("#jscontainer_designer").val());
                var Style = encodeURIComponent($("#csscontainer_designer").val());
                var saveform = formDesigner.beforeSaved({ Name: Name, Title: Title, DataSource: DataSource, Script: Script, Style: Style });
                //  $("#jsoncontainer_designer").val(formatJson(JSON2.stringify(saveform)));
                $("#jsoncontainer_designer").val(JSON2.stringify(saveform));
            }
            if (ui.newTab.find("a").attr("id") === "html") {
                $("#htmlcontainer_designer").val(document.getElementById("container_designer").innerHTML);
            }
        })
        initDesign(formDesignerContain);
    }
    formDesigner = {
        init: function (options) {
            window.form = {
                DataSource: null,
                Title: "表单",
                Fields: new Array(),
                Script: null,
                Style: null
            };
            var defaults =
               {
                   formName: "formName",
                   formTitle: "formTitle",
                   formDataSource: "formDataSource",
                   formDescription: "formDescription",
                   formApp: "formApp",
                   formfirstInit: true,
                   formContent: form,
                   formDesignerContain: "container_designer"
               };
            options = $.extend({}, defaults, options);
            this.initUI();
            initEvent(options.formDesignerContain);
            var eFormID = $.query.get("eFormID");
            var activityID = $.query.get("ActivityID");
            if (eFormID) {
                if (options.formfirstInit) {
                    $.post("/FormDesigner/home/getFormInfo", { EFormID: eFormID }, function (retValue) {
                        if (retValue) {
                            var eFormContent = JSON2.parse(retValue.Content);
                            form = eFormContent;
                            initForm();

                            if ($("#" + options.formDataSource).length > 0)
                                $("#" + options.formDataSource).val(form.DataSource);
                            if ($("#" + options.formTitle).length > 0)
                                $("#" + options.formTitle).val(form.Title);
                            if ($("#" + options.formName).length > 0)
                                $("#" + options.formName).val(form.Name);
                            if ($("#" + options.formDescription).length > 0)
                                $("#" + options.formDescription).val(retValue.Description);
                            if ($("#" + options.formApp).length > 0)
                                $("#" + options.formApp).val(retValue.AppID);
                            initContextMenu();
                            $(".ui-resizable-handle").css("display", "none");
                        }
                    });
                }
                else {
                    $(".control").each(function () {
                        $(this).remove();
                    });
                    var eFormContent = JSON2.parse(options.formContent);
                    form = eFormContent;
                    initForm();
                    if ($("#" + options.formDataSource).length > 0)
                        $("#" + options.formDataSource).val(form.DataSource);
                    if ($("#" + options.formTitle).length > 0)
                        $("#" + options.formTitle).val(form.Title);
                    if ($("#" + options.formName).length > 0)
                        $("#" + options.formName).val(form.Name);
                    initContextMenu();
                    $(".ui-resizable-handle").css("display", "none");
                }
            }
            else if (activityID) {
                var parentWindow = window.dialogArguments || window.parent;
                var processDefine = parentWindow.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.processDefine;
                var activityID = $.query.get("ActivityID");
                //  if (activityID) {
                for (var i = 0; i < processDefine.Activities.length; i++) {
                    if (processDefine.Activities[i].ID == activityID) {
                        var activityForm = processDefine.Activities[i].Form;
                        if (activityForm.Fields) {
                            form = activityForm;
                        }
                        initForm();
                        if ($("#" + options.formDataSource).length > 0)
                            $("#" + options.formDataSource).val(form.DataSource);
                        if ($("#" + options.formTitle).length > 0)
                            $("#" + options.formTitle).val(form.Title);
                        if ($("#" + options.formName).length > 0)
                            $("#" + options.formName).val(form.Name);

                        initContextMenu();
                        $(".ui-resizable-handle").css("display", "none");
                    }
                    //    }
                }
            }


        },
        initUI: function () {
            initUI();
        },
        initEvent: function () {
            initEvent();
        },
        addItem: function (options) {
            var defaults = {
                ID: new Date().getTime(),
                SortOrder: 0,
                Name: this.ID,
                DataSource: null,
                Text: "控件名：",
                Required: false,
                ControlType: null,
                DataType: "String",
                AccessPattern: "Write",
                DefaultValue: "",
                Rows: 1,
                Cols: 1,
                Width: 240,
                Height: 25,
                Url: null,
                X: 0,
                Y: 0,
                Z: 0,
                Container: ''
            };
            var field = $.extend({}, defaults, options);
            if (typeof (form) == "undefined") {
                this.init();
            }

            form.Fields.push(field);

            initContextMenu();
        },
        editItem: function (options) {
            for (var i = 0; i < form.Fields.length; i++) {
                if (form.Fields[i].Name == options.OldID) {
                    // form.Fields[i] = options;
                    form.Fields[i] = $.extend({}, form.Fields[i], options);
                }
            }
        },
        beforeSaved: function (options) {
            var saveform = formInfos(options);
            return saveform;
        },
        saveform: function (options) {
            saveform = this.beforeSaved(options);
            if (saveform.DataSource) {
                var formResult = JSON2.stringify(saveform);
                var eFormID = $.query.get("eFormID");
                var formApp = $("#formApp").val();
                var formDescription = $("#formDescription").val();
                $.post("/FormDesigner/Home/saveForm", { FormApp: formApp, FormDescription: formDescription, Form: formResult, FormName: saveform.Name, EFormID: eFormID }, function (retValue) {
                    if (retValue.Result == 1) {
                        alert("保存成功");
                        return true;
                    }
                    else {
                        alert("保存失败");
                        return false;
                    }
                });
            }
            else {
                alert("请在右侧属性列表输入表单绑定的数据库表");
                return false;
            }

        },
        leftItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    var width = $(this).width();
                    field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                    field.width = width
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);

                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    var left = field.left + "px";
                    $(selectItems[i]).css("left", left);
                }
            }
        },
        centeredItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    var width = $(this).width();
                    field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                    field.width = width
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);

                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    var left = field.left + field.width / 2 - $(selectItems[i]).width() / 2 + "px";
                    $(selectItems[i]).css("left", left);
                }
            }
        },
        rightItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                labelwidth: 0,
                inputwidth: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    if ($(this).find("label").length > 0) {
                        field.labelwidth = $(this).find("label").width();
                    }
                    if ($(this).find("input").length > 0) {
                        field.inputwidth = $(this).find("input").width();
                    }
                    var width = $(this).width();
                    field.left = parseFloat(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseFloat(top.substring(0, top.lastIndexOf("px")));
                    field.width = width;
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);

                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    if (field.labelwidth > 0 && field.inputwidth > 0) {
                        var selectLabelwidth = $(selectItems[i]).find("label").width();
                        var selectwidth = $(selectItems[i]).width();
                        $(selectItems[i]).css("width", selectLabelwidth + field.inputwidth + 5 + "px");
                        $(selectItems[i]).find("input").css("width", field.inputwidth);
                    }
                    var standardleft = field.left + field.width;
                    var width = $(selectItems[i]).width();
                    var left = standardleft - width + "px";
                    $(selectItems[i]).css("left", left);
                }
            }
        },
        upItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                labelwidth: 0,
                inputwidth: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);
                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    var top = field.top + "px";
                    $(selectItems[i]).css("top", top);
                }
            }
        },
        bottomItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                height: 0,
                labelwidth: 0,
                inputwidth: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                    field.height = $(this).height();
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);
                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    var height = $(selectItems[i]).height();
                    var top = field.top + field.height - height + "px";
                    $(selectItems[i]).css("top", top);
                }
            }
        },
        middleItems: function (options) {
            var defaults = {
                left: 0,
                top: 0,
                width: 0,
                height: 0,
                labelwidth: 0,
                inputwidth: 0,
                standard: null
            };
            var field = $.extend({}, defaults, options);
            var selectItems = new Array();
            $(".control").each(function () {
                if ($(this).attr("standardcontrol")) {
                    field.standard = this;
                    var left = $(this).css("left");
                    var top = $(this).css("top");
                    field.left = parseInt(left.substring(0, left.lastIndexOf("px")));
                    field.top = parseInt(top.substring(0, top.lastIndexOf("px")));
                    field.height = $(this).height();
                }
                else if ($(this).attr("selectcontrol")) {
                    selectItems.push(this);
                }
            });
            if (field.standard && selectItems.length > 0) {
                for (var i = 0; i < selectItems.length; i++) {
                    var height = $(selectItems[i]).height();
                    var top = field.top + field.height / 2 - height / 2 + "px";
                    $(selectItems[i]).css("top", top);
                }
            }
        },
        clearAllItems: function () {
            form.Fields = null;
            $(".control").remove();
        },
        fullScreen: function (options) {
            var defaults =
                {
                    header: "header",
                    leftContainer: "leftcontainer",
                    maincontainer: "maincontainer",
                    attrcontainer: "attrcontainer"
                };
            var options = $.extend({}, defaults, options);
            $("#" + options.leftContainer).css({ "z-index": 10000, "position": "absolute" });
            $("#" + options.leftContainer).draggable(
                {
                    cursor: "move",
                    containment: "#container_designer",
                    handles: "all"
                }
                );
            $("#" + options.attrcontainer).hide();
            $("#header_toolbar_restorescreen").show();
            $("#header_toolbar_fullscreen").hide();
            if ($("#" + "actionDialog", window.parent.document).length > 0 && window.parent != window) {
                $("#" + "actionDialog", window.parent.document).parent().css("left", "0px");
                $("#" + "actionDialog", window.parent.document).parent().css("top", "0px");
                window.parent.document.getElementById("actionDialog").style.height = window.screen.availHeight - 88 + "px";
                window.parent.document.getElementById("actionDialog").style.width = window.screen.availWidth - 2 + "px";
                $("#" + "actionDialog", window.parent.document).parent().css("height", window.screen.availHeight - 88 + "px");
                $("#" + "actionDialog", window.parent.document).parent().css("width", window.screen.availWidth - 2 + "px");
                document.getElementById("leftcontainer").style.height = window.screen.availHeight - 180 - $("#header").height() + "px";
                document.getElementById("mainbody").style.height = window.screen.availHeight - $("#header").height() - 78 + "px";
                document.getElementById("maincontainer").style.height = window.screen.availHeight - $("#header").height() - 78 + "px";
                document.getElementById("container_designer").style.height = window.screen.availHeight - 190 - $("#header").height() + "px";
                document.getElementById("htmlcontainer_designer").style.height = window.screen.availHeight - 190 - $("#header").height() + "px";
                document.getElementById("csscontainer_designer").style.height = window.screen.availHeight - 190 - $("#header").height() + "px";
                document.getElementById("jsoncontainer_designer").style.height = window.screen.availHeight - 190 - $("#header").height() + "px";
                document.getElementById("jscontainer_designer").style.height = window.screen.availHeight - 190 - $("#header").height() + "px";
                document.getElementById("maincontainer").style.width = window.screen.availWidth - 2 + "px";
                document.getElementById("container_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("htmlcontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("csscontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("jscontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("jsoncontainer_designer").style.width = window.screen.availWidth - 52 + "px";
            }
            else {
                document.getElementById("leftcontainer").style.height = window.screen.availHeight - 150 - $("#header").height() + "px";
                document.getElementById("mainbody").style.height = window.screen.availHeight - $("#header").height() - 48 + "px";
                document.getElementById("maincontainer").style.height = window.screen.availHeight - $("#header").height() - 48 + "px";
                document.getElementById("container_designer").style.height = window.screen.availHeight - 120 - $("#header").height() + "px";
                document.getElementById("htmlcontainer_designer").style.height = window.screen.availHeight - 120 - $("#header").height() + "px";
                document.getElementById("csscontainer_designer").style.height = window.screen.availHeight - 120 - $("#header").height() + "px";
                document.getElementById("jscontainer_designer").style.height = window.screen.availHeight - 120 - $("#header").height() + "px";
                document.getElementById("jsoncontainer_designer").style.height = window.screen.availHeight - 120 - $("#header").height() + "px";
                document.getElementById("maincontainer").style.width = window.screen.availWidth - 2 + "px";
                document.getElementById("container_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("htmlcontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("csscontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("jscontainer_designer").style.width = window.screen.availWidth - 52 + "px";
                document.getElementById("jsoncontainer_designer").style.width = window.screen.availWidth - 52 + "px";
            }
            $(window).unbind("resize");
            return false;
        },
        restoreScreen: function (options) {
            var defaults =
             {
                 header: "header",
                 leftContainer: "leftcontainer",
                 maincontainer: "maincontainer",
                 attrcontainer: "attrcontainer"
             };
            var options = $.extend({}, defaults, options);
            $("#" + options.leftContainer).css({ "z-index": 10, "position": "static" });
            $("#" + options.leftContainer).draggable(
             'destroy'
                );
            $("#header_toolbar_restorescreen").hide();
            $("#header_toolbar_fullscreen").show();
            $("#attrcontainer").show();
            $(window).resize(function () {
                resizeWindow();
            });
            resizeWindow();
            //if ($("#" + "actionDialog", window.parent.document).length > 0) {
            //    $("#" + "actionDialog", window.parent.document).parent().css("left", "50px");
            //    $("#" + "actionDialog", window.parent.document).parent().css("top", "50px");
            //    $("#" + "actionDialog", window.parent.document).parent().css("height", window.screen.availHeight - 188 + "px");
            //    $("#" + "actionDialog", window.parent.document).parent().css("width", window.screen.availWidth - 220 + "px");
            //    window.parent.document.getElementById("actionDialog").style.height = window.screen.availHeight - 208 + "px";
            //    window.parent.document.getElementById("actionDialog").style.width = window.screen.availWidth - 220 + "px";
            //}
            //var windowHeight = getWindowHeight();
            //if (windowHeight == 0) return;
            //document.getElementById("mainbody").style.height = windowHeight - $("#header").height() - 8 + "px";
            //document.getElementById("leftcontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            //document.getElementById("maincontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            //document.getElementById("attrcontainer").style.height = windowHeight - $("#header").height() - 8 + "px";
            //document.getElementById("container_designer").style.height = windowHeight - $("#header").height() - 78 + "px";
            //document.getElementById("htmlcontainer_designer").style.height = windowHeight - $("#header").height() - 55 + "px";
            //document.getElementById("csscontainer_designer").style.height = windowHeight - $("#header").height() - 55 + "px";
            //document.getElementById("jscontainer_designer").style.height = windowHeight - $("#header").height() - 55 + "px";
            //document.getElementById("maincontainer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 6 + "px";
            //document.getElementById("container_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 52 + "px";
            //document.getElementById("htmlcontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 18 + "px";
            //document.getElementById("csscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 18 + "px";
            //document.getElementById("jscontainer_designer").style.width = document.body.offsetWidth - $("#leftcontainer").width() - $("#attrcontainer").width() - 18 + "px";

        }
    };
    //配置控件信息
    setContorlValue = function (options, configure) {
        // try {
        eval(decodeURIComponent(form.Script));
        var OldID = options.OldID;
        var me = $("#" + OldID);
        me.attr("ID", options.Name);
        switch (options.ControlType) {
            case ControlType.Tabs:
                if (me.length == 0) {
                    var ListItems;
                    var itemleft = 0;
                    var itemtop = 0;
                    var itemwidth = 611;
                    var itemheight = 477;
                    if (form.Fields && form.Fields.length > 0) {
                        for (var i = 0; i < form.Fields.length; i++) {
                            if (form.Fields[i].Name == options.OldID) {
                                ListItems = form.Fields[i].ListItems;
                                itemleft = form.Fields[i].X;
                                itemtop = form.Fields[i].Y;
                                itemwidth = form.Fields[i].Width;
                                itemheight = form.Fields[i].Height;
                            }
                        }
                        if (ListItems && ListItems.length > 0) {
                            var divItem = '';
                            divList = [];
                            var item = "<div id=\"" + new Date().getTime() + "\" class=\"control\" controlType=\"" + ControlType.Tabs + "\"   style=\"position:absolute; z-index: 4; border:1px solid #AAA;width:" + (itemwidth + 10) + "px;height:" + (itemheight + 10) + "px;left:" + itemleft + "px;top:" + itemtop + "px;\"> <div id=\"" + options.Name + "\" class=\"swMain\" style=\"width:" + itemwidth + "px;height:" + itemheight + "px;\"><ul>";
                            for (var i = 0; i < ListItems.length; i++) {
                                item = item + "<li><p><a href=\"#" + ListItems[i].Text + "\"><label class=\"stepNumber\"></label><span class=\"stepText\">" + ListItems[i].Value + "<br /></span></a></p></li>"
                                divItem = divItem + "<div id=\"" + ListItems[i].Text + "\"></div>";
                                divList.push(ListItems[i].Text);
                            }
                            item = item + "</ul>" + divItem + "</div></div>";
                            $("#container_designer").append(item);

                            $("#" + options.Name).parent().bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + options.Name).parent().draggable({
                                start: function (event, ui) {
                                    $("#" + options.Name).parent().attr("startDraggable_left", ui.position.left);
                                    $("#" + options.Name).parent().attr("startDraggable_top", ui.position.top);
                                },
                                cursor: "move",
                                containment: "#container_designer",
                                handles: "all"
                            });
                            if ($.browser.msie) {
                                var steps = $("#" + options.Name + " ul > li > p > a");
                                for (var i = 0; i < steps.length; i++) {
                                    var aHerf = $(steps[i]).attr("href");
                                    aHerf = aHerf.substring(aHerf.indexOf("#"), aHerf.length);
                                    $(steps[i]).attr("href", aHerf);
                                }
                            }
                            $("#" + options.Name).smartWizard(
                                {
                                    Tabs: 1,
                                    onShowStep: function (selStep) {
                                        var selStepID = $(selStep.attr("href")).attr("id");
                                        $(".control").each(function () {
                                            var me = $(this);
                                            if (me.attr("parentControl") == selStepID) {
                                                me.show();
                                            }
                                            else if (me.attr("parentControl") && me.attr("parentControl") != "null") {
                                                me.hide();
                                            }
                                        });
                                    }
                                });
                            var stepContainerID = $("#" + options.Name).find(".stepContainer").attr("id");
                            var stepContainerID = "#" + stepContainerID;
                            var allID = "#" + options.Name;
                            var objectID = {};
                            objectID[allID] = '';
                            objectID[stepContainerID] = '';
                            objectID[".content"] = '';
                            $("#" + options.Name).parent().resizable({
                                alsoResize: objectID,
                                handles: "all"
                            });
                        }
                    }
                }
                else {
                    me.smartWizard('destroy');
                    if (form.Fields && form.Fields.length > 0) {
                        for (var i = 0; i < form.Fields.length; i++) {
                            if (form.Fields[i].Name == options.OldID) {
                                ListItems = form.Fields[i].ListItems;
                            }
                        }
                        if (ListItems && ListItems.length > 0) {
                            var divItem = '';
                            divList = [];
                            var item = "<ul>";
                            for (var i = 0; i < ListItems.length; i++) {
                                item = item + "<li><p><a href=\"#" + ListItems[i].Text + "\"><label class=\"stepNumber\"></label><span class=\"stepText\">" + ListItems[i].Value + "<br /></span></a></p></li>"
                                divItem = divItem + "<div id=\"" + ListItems[i].Text + "\"></div>";
                                divList.push(ListItems[i].Text);
                            }
                            item = item + "</ul>" + divItem;
                            me.append(item);
                            if ($.browser.msie) {
                                var steps = $("#" + options.Name + " ul > li > p > a");
                                for (var i = 0; i < steps.length; i++) {
                                    var aHerf = $(steps[i]).attr("href");
                                    aHerf = aHerf.substring(aHerf.indexOf("#"), aHerf.length);
                                    $(steps[i]).attr("href", aHerf);
                                }
                            }
                            $("#" + options.Name).smartWizard({
                                Tabs: 1,
                                onShowStep: function (selStep) {
                                    var selStepID = $(selStep.attr("href")).attr("id");
                                    $(".control").each(function () {
                                        var me = $(this);
                                        if (me.attr("parentControl") == selStepID) {
                                            me.show();
                                        }
                                        else if (me.attr("parentControl") && me.attr("parentControl") != "null") {
                                            me.hide();
                                        }
                                    });
                                }
                            });
                            var stepContainerID = $("#" + options.Name).find(".stepContainer").attr("id");
                            var stepContainerID = "#" + stepContainerID;
                            var allID = "#" + options.Name;
                            var objectID = {};
                            objectID[allID] = '';
                            objectID[stepContainerID] = '';
                            objectID[".content"] = '';
                            $("#" + options.Name).parent().resizable({
                                alsoResize: objectID,
                                handles: "all"
                            });
                        }
                    }
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Tabs属性', "/FormDesigner/Home/WizardConfigure?ControlType=" + ControlType.Tabs + "&ControlID=" + options.Name, 650, 380, true);
                });
                initContextMenu();
                break;
            case ControlType.Wizard:
                if (me.length == 0) {
                    var ListItems;
                    var itemleft = 0;
                    var itemtop = 0;
                    var itemwidth = 611;
                    var itemheight = 477;
                    if (form.Fields && form.Fields.length > 0) {
                        for (var i = 0; i < form.Fields.length; i++) {
                            if (form.Fields[i].Name == options.OldID) {
                                ListItems = form.Fields[i].ListItems;
                                itemleft = form.Fields[i].X;
                                itemtop = form.Fields[i].Y;
                                itemwidth = form.Fields[i].Width;
                                itemheight = form.Fields[i].Height;
                            }
                        }
                        //formfield.AutoGenScript = "<script src=\"wizwrdComplete.js\"></script >";

                        //formfield.AutoGenScript+="$(\"#"+ID+"\").smartWizard({onShowStep: function (selStep) {var selStepID = $(selStep.attr(\"href\")).attr(\"id\");$(\".control\").each(function () {
                        //            var me = $(this);
                        //            if (me.attr("parentControl") == selStepID) {
                        //                me.show();
                        //            }
                        //            else if (me.attr("parentControl")) {
                        //                me.hide();
                        //            }
                        //        });
                        //    }
                        //});";
                        if (ListItems && ListItems.length > 0) {
                            var divItem = '';
                            divList = [];
                            var item = "<div id=\"" + new Date().getTime() + "\" class=\"control\" controlType=\"" + ControlType.Wizard + "\"   style=\"position:absolute; z-index: 4; border:1px solid #AAA;width:" + (itemwidth + 10) + "px;height:" + (itemheight + 10) + "px;left:" + itemleft + "px;top:" + itemtop + "px;\"> <div id=\"" + options.Name + "\" class=\"swMain\" style=\"width:" + itemwidth + "px;height:" + itemheight + "px;\"><ul>";
                            for (var i = 0; i < ListItems.length; i++) {
                                item = item + "<li><p><a href=\"#" + ListItems[i].Text + "\"><label class=\"stepNumber\">" + i + "</label><span class=\"stepText\">" + ListItems[i].Value + "<br /></span></a></p></li>"
                                divItem = divItem + "<div id=\"" + ListItems[i].Text + "\"></div>";
                                divList.push(ListItems[i].Text);
                            }
                            item = item + "</ul>" + divItem + "</div></div>";
                            $("#container_designer").append(item);

                            $("#" + options.Name).parent().bind("mousedown", function () {
                                e = window.event || arguments[0];
                                var me = $(this);
                                if (e.ctrlKey) {
                                    me.attr("selectcontrol", true);
                                }
                                else {
                                    $(".control").each(function () {
                                        if ($(this).attr("standardcontrol")) {
                                            $(this).removeAttr("standardcontrol");
                                        }
                                    });
                                    $(".ui-resizable-handle").css("display", "none");
                                    me.attr("standardcontrol", true);
                                }
                                me.find(".ui-resizable-handle").css("display", "block")
                                if (window.event) //停止事件向下传播
                                    window.event.cancelBubble = true;
                                else {
                                    e.stopPropagation();
                                }
                            });
                            $("#" + options.Name).parent().draggable({
                                start: function (event, ui) {
                                    $("#" + options.Name).parent().attr("startDraggable_left", ui.position.left);
                                    $("#" + options.Name).parent().attr("startDraggable_top", ui.position.top);
                                },
                                cursor: "move",
                                containment: "#container_designer",
                                handles: "all"
                            });
                            if ($.browser.msie) {
                                var steps = $("#" + options.Name + " ul > li > p > a");
                                for (var i = 0; i < steps.length; i++) {
                                    var aHerf = $(steps[i]).attr("href");
                                    aHerf = aHerf.substring(aHerf.indexOf("#"), aHerf.length);
                                    $(steps[i]).attr("href", aHerf);
                                }
                            }
                            $("#" + options.Name).smartWizard(
                                {
                                    onShowStep: function (selStep) {
                                        var selStepID = $(selStep.attr("href")).attr("id");
                                        $(".control").each(function () {
                                            var me = $(this);
                                            if (me.attr("parentControl") == selStepID) {
                                                me.show();
                                            }
                                            else if (me.attr("parentControl") && me.attr("parentControl") != "null") {
                                                me.hide();
                                            }
                                        });
                                    }
                                });
                            //$("#" + options.Name).parent().resizable({
                            //    alsoResize: "#" + options.Name,
                            //    handles: "all"
                            //});
                            var stepContainerID = $("#" + options.Name).find(".stepContainer").attr("id");
                            var stepContainerID = "#" + stepContainerID;
                            // $(stepContainerID).find(".content")
                            var allID = "#" + options.Name;
                            var objectID = {};
                            objectID[allID] = '';
                            objectID[stepContainerID] = '';
                            objectID[".content"] = '';
                            $("#" + options.Name).parent().resizable({
                                alsoResize: objectID,
                                handles: "all"
                            });
                        }
                    }
                }
                else {
                    me.smartWizard('destroy');
                    if (form.Fields && form.Fields.length > 0) {
                        for (var i = 0; i < form.Fields.length; i++) {
                            if (form.Fields[i].Name == options.OldID) {
                                ListItems = form.Fields[i].ListItems;
                            }
                        }
                        if (ListItems && ListItems.length > 0) {
                            var divItem = '';
                            divList = [];
                            var item = "<ul>";
                            for (var i = 0; i < ListItems.length; i++) {
                                item = item + "<li><p><a href=\"#" + ListItems[i].Text + "\"><label class=\"stepNumber\">" + i + "</label><span class=\"stepText\">" + ListItems[i].Value + "<br /></span></a></p></li>"
                                divItem = divItem + "<div id=\"" + ListItems[i].Text + "\"></div>";
                                divList.push(ListItems[i].Text);
                            }
                            item = item + "</ul>" + divItem;
                            me.append(item);
                            if ($.browser.msie) {
                                var steps = $("#" + options.Name + " ul > li > p > a");
                                for (var i = 0; i < steps.length; i++) {
                                    var aHerf = $(steps[i]).attr("href");
                                    aHerf = aHerf.substring(aHerf.indexOf("#"), aHerf.length);
                                    $(steps[i]).attr("href", aHerf);
                                }
                            }
                            $("#" + options.Name).smartWizard({
                                onShowStep: function (selStep) {
                                    var selStepID = $(selStep.attr("href")).attr("id");
                                    $(".control").each(function () {
                                        var me = $(this);
                                        if (me.attr("parentControl") == selStepID) {
                                            me.show();
                                        }
                                        else if (me.attr("parentControl") && me.attr("parentControl") != "null") {
                                            me.hide();
                                        }
                                    });
                                }
                            });
                            var stepContainerID = $("#" + options.Name).find(".stepContainer").attr("id");
                            var stepContainerID = "#" + stepContainerID;
                            // $(stepContainerID).find(".content")
                            var allID = "#" + options.Name;
                            var objectID = {};
                            objectID[allID] = '';
                            objectID[stepContainerID] = '';
                            objectID[".content"] = '';
                            $("#" + options.Name).parent().resizable({
                                alsoResize: objectID,
                                handles: "all"
                            });
                        }
                    }
                }
                //me.smartWizard(
                //    {
                //        onShowStep: function (selStep) {
                //            $(".control").each(function () {
                //                if ($(this).attr("controltype") == ControlType.Div) {
                //                    var me = $(this);
                //                    var preleft = parseFloat(me.css("left").substring(0, me.css("left").lastIndexOf("px")));
                //                    var pretop = parseFloat(me.css("top").substring(0, me.css("top").lastIndexOf("px")));
                //                    if ($(this).css("display") == "none") {
                //                        $(".control").each(function () {
                //                            var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                //                            var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                //                            if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                //                                $(this).hide();
                //                            }
                //                        });
                //                    }
                //                    else {
                //                        $(".control").each(function () {
                //                            var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                //                            var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                //                            if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                //                                $(this).show();
                //                            }
                //                        });
                //                    }
                //                }
                //            });
                //        }
                //    });
                //var listDiv = new Array();

                //$(".control").each(function () {
                //    if ($(this).attr("controltype") == ControlType.Div) {
                //        var me = $(this);
                //        var preleft = parseFloat(me.css("left").substring(0, me.css("left").lastIndexOf("px")));
                //        var pretop = parseFloat(me.css("top").substring(0, me.css("top").lastIndexOf("px")));
                //        if ($(this).css("display") == "none") {
                //            $(".control").each(function () {
                //                var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                //                var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                //                //preleft = parseFloat(me.css("left").substring(0, me.css("left").lastIndexOf("px")));
                //                //pretop = parseFloat(me.css("top").substring(0, me.css("top").lastIndexOf("px")));
                //                //var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                //                //var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                //                if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                //                    $(this).hide();
                //                }
                //            });
                //        }
                //        else {
                //            $(".control").each(function () {
                //                var left = parseFloat($(this).css("left").substring(0, $(this).css("left").lastIndexOf("px")));
                //                var top = parseFloat($(this).css("top").substring(0, $(this).css("top").lastIndexOf("px")));
                //                if (left > preleft && top > pretop && left + $(this).width() < preleft + me.width() && top + $(this).height() < pretop + me.height()) {
                //                    $(this).show();
                //                }
                //            });
                //        }
                //    }
                //});

                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Wizard属性', "/FormDesigner/Home/WizardConfigure?ControlType=" + ControlType.Wizard + "&ControlID=" + options.Name, 650, 380, true);
                });
                initContextMenu();
                break;
            case ControlType.Div:
                //$(".control").each(function () {
                //    if ($(this).position().left > preleft && $(this).position().top > pretop && $(this).position().left + $(this).width() < preleft + me.width() && $(this).position().top + $(this).height() < pretop + me.height()) {
                //        this.style.left = left + $(this).position().left + "px";
                //        this.style.top = top + $(this).position().top + "px";
                //    }
                //});
                if (options.CustomStyle && options.CustomStyle.split(";").length > 0) {
                    for (var i = 0; i < options.CustomStyle.split(";").length; i++) {
                        var cssText = options.CustomStyle.split(";")[i];
                        var cssProperty = cssText.split(":")[0];
                        var cssValue = cssText.split(":")[1];
                        if (cssProperty === "background" || cssProperty === "BACKGROUND") {
                            // options.CustomStyle=options.CustomStyle.replace(cssValue, decodeURIComponent(cssValue));
                            me[0].style.background = decodeURIComponent(cssValue);
                            //alert(me.css("backgroundRepeat"));
                            // me[0].style.background_repeat = me.css("backgroundRepeat");
                        }
                        else {
                            me.css(cssProperty, cssValue);
                        }
                    }
                    if (options.Width.toString().indexOf("%") > 0) {
                        me.css("width", options.Width);
                    }
                    else {
                        me.css("width", options.Width + "px");
                    }
                    if (options.Height.toString().indexOf("%") > 0) {
                        me.css("height", options.Height);
                    }
                    else {
                        me.css("height", options.Height + "px");
                    }
                    me.css("left", options.X + "px");
                    me.css("top", options.Y + "px");
                    options.CustomStyle = me.attr("style");
                    options.CustomStyle = options.CustomStyle.replace(options.CustomStyle.slice(options.CustomStyle.indexOf("background-repeat")), "background-repeat:" + me.css("backgroundRepeat") + ";");
                    //  alert(options.CustomStyle);
                    me.resizable('destroy');
                    me.resizable({
                        handles: "all"
                    });
                }
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Div属性', "/FormDesigner/Home/TextConfigure?ControlType=" + ControlType.Div + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.Grid:
                // getGridDS?getGridDS : null;

                if (typeof (getGridDS) != "undefined") {
                    var dataSource = getGridDS.call(this, options);
                }
                else if (options.DataSource) {
                    getGridInfo(options);
                }
                if (options.CustomStyle) {
                    var load_css = (function () {
                        var sStyle = document.createElement("style");
                        sStyle.setAttribute("type", "text/css");
                        if (sStyle.styleSheet) { //ie
                            sStyle.styleSheet.cssText = options.CustomStyle;
                        } else {
                            var csstext = document.createTextNode(options.CustomStyle);
                            sStyle.appendChild(csstext);
                        }
                        document.getElementsByTagName('head')[0].appendChild(sStyle);
                    })();
                }
                if (options.Width.toString().indexOf("%") > 0) {
                    me.css("width", options.Width);
                    //  options.Width = 1;
                }
                else {
                    if (options.Width.toString().indexOf("%") > 0) {
                        me.css("width", options.Width);
                    }
                    else {
                        me.css("width", options.Width + "px");
                    }

                }
                //  me.css("height", "auto");

                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置' + ControlType.Grid + '属性', "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + ControlType.Grid + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.DataTable:
                if (options.DataSource) {
                    $.post("/FormDesigner/Home/GetTableSource", { DataSource: options.DataSource }, function (retValue) {
                        var ListItems = null;
                        if (form.Fields && form.Fields.length > 0) {
                            for (var i = 0; i < form.Fields.length; i++) {
                                if (form.Fields[i].Name == options.OldID) {
                                    ListItems = form.Fields[i].ListItems;
                                }
                            }
                        }
                        if (options.Required) {
                            var tableItem = "<table id='" + options.Name + "table'><thead><tr>";
                            for (var j = 0; j < ListItems.length; j++) {
                                var headItem = "<th>" + ListItems[j].Text + "</th>";
                                tableItem = tableItem + headItem;
                            }
                            tableItem = tableItem + "</tr></thead><tbody>";
                            for (var m = 0; m < retValue.length; m++) {
                                var trItem = "<tr>";
                                for (var n = 0; n < ListItems.length; n++) {
                                    var tdItem = "<td>" + retValue[m][ListItems[n].Value] + "</td>";
                                    var trItem = trItem + tdItem;
                                }
                                trItem = trItem + "</tr>";
                                tableItem = tableItem + trItem;
                            }
                            tableItem = tableItem + "</tbody></table>";
                            me.empty();
                            me.append(tableItem);
                            me.resizable('destroy');
                            me.resizable({
                                alsoResize: "#" + options.Name + "table",
                                handles: "all"
                            });
                            $(".ui-resizable-handle").css("display", "none");
                            //if (options.Height > retValue.length * 22) {
                            //    $("#" + options.Name + "table").css("width", options.Width + "px");
                            //    $("#" + options.Name + "table").css("height", options.Height + "px");
                            //    me.css("width", options.Width + "px");
                            //    me.css("height", options.Height + "px");
                            //}
                            //else {
                            //    $("#" + options.Name + "table").css("width", options.Width + "px");
                            //    $("#" + options.Name + "table").css("height", retValue.length * 22 + "px");
                            //    me.css("width", options.Width + "px");
                            //    me.css("height", retValue.length * 22 + "px");
                            //}
                            $("#" + options.Name + "table").css("width", options.Width + "px");
                            $("#" + options.Name + "table").css("height", options.Height + "px");
                            me.css("width", $("#" + options.Name + "table").width() + "px");
                            me.css("height", $("#" + options.Name + "table").height() + "px");
                        }
                        else {
                            var tableItem = "<table id='" + options.Name + "table'><tbody>";
                            for (var j = 0; j < ListItems.length; j++) {
                                var trItem = "<tr><td>" + ListItems[j].Text + "</td>";
                                for (var i = 0; i < retValue.length; i++) {
                                    tdValueItem = "<td>" + retValue[i][ListItems[j].Value] + "</td>";
                                    trItem = trItem + tdValueItem;
                                }
                                trItem = trItem + "</tr>";
                                tableItem = tableItem + trItem;
                            }
                            tableItem = tableItem + "</tbody></table>";
                            me.empty();
                            me.append(tableItem);
                            me.resizable('destroy');
                            me.resizable({
                                alsoResize: "#" + options.Name + "table",
                                handles: "all"
                            });
                            $(".ui-resizable-handle").css("display", "none");
                            //$("#" + options.Name + "table").css("width", options.Width + "px");
                            //$("#" + options.Name + "table").css("height", options.Height + "px");
                            me.css("width", $("#" + options.Name + "table").width() + "px");
                            me.css("height", $("#" + options.Name + "table").height() + "px");
                            //}
                            //else {
                            //    $("#" + options.Name + "table").css("width", retValue.length * 22 + "px");
                            //    $("#" + options.Name + "table").css("height", options.Height + "px");
                            //    me.css("width", options.Width + "px");
                            //    me.css("height", retValue.length * 22 + "px");
                            //}
                        }
                    });
                }
                if (options.CustomStyle) {
                    var load_css = (function () {
                        var sStyle = document.createElement("style");
                        sStyle.setAttribute("type", "text/css");
                        if (sStyle.styleSheet) { //ie
                            sStyle.styleSheet.cssText = options.CustomStyle;
                        } else {
                            var csstext = document.createTextNode(options.CustomStyle);
                            sStyle.appendChild(csstext);
                        }
                        document.getElementsByTagName('head')[0].appendChild(sStyle);
                    })();
                }
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置' + ControlType.DataTable + '属性', "/FormDesigner/Home/DataCtrlConfigure?ControlType=" + ControlType.DataTable + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.Tree:
                if (typeof (getTreeDS) != "undefined") {
                    var dataSource = getTreeDS.call(this, options);
                }
                else if (options.DataSource) {
                    getTreeInfo(options);
                }
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置' + ControlType.Tree + '属性', "/FormDesigner/Home/TreeConfigure?ControlType=" + ControlType.Tree + "&ControlID=" + options.Name, 650, 530, true);
                });
                if (options.Width.toString().indexOf("%")>0) {
                   // me.css("width", options.Width);
                    me.parent().css("width", options.Width);
                }
                else {
                    me.css("width", options.Width + "px");
                    me.parent().css("width", options.Width + "px");
                }
                if (options.Height.toString().indexOf("%") > 0) {
                    me.css("height", options.Height);
                    me.parent().css("height", options.Height);
                }
                else {
                    me.css("height", options.Height + "px");
                    me.parent().css("height", options.Height + "px");
                }



                break;
            case ControlType.Chart:
                if (options.DataSource) {
                    //configure.loadScript("
                    $.post("/FormDesigner/Home/InitChart", { DataSource: options.DataSource, ExtendData: options.ExtendData, TableSource: form.DataSource }, function (retValue) {
                        hightChart(options, retValue);
                        me.on("resizestop", function (event, ui) {
                            hightChart(options, retValue);
                        });
                        $(".ui-resizable-handle").css("display", "none");
                    });
                }

                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Chart属性', "/FormDesigner/Home/ChartConfigure?ControlType=" + ControlType.Chart + "&ControlID=" + options.Name, 650, 380, true);
                });
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                break;
            case ControlType.Text:
                if (options.CustomStyle && options.CustomStyle.split(";").length > 0) {
                    for (var i = 0; i < options.CustomStyle.split(";").length; i++) {
                        var cssText = options.CustomStyle.split(";")[i];
                        var cssProperty = cssText.split(":")[0];
                        var cssValue = cssText.split(":")[1];
                        me.css(cssProperty, cssValue);
                    }
                    me.css("width", options.Width + "px");
                    me.css("height", options.Height + "px");
                    me.css("left", options.X + "px");
                    me.css("top", options.Y + "px");
                    options.CustomStyle = me.attr("style");
                    me.html(options.Text);
                    me.resizable('destroy');
                    me.resizable({
                        handles: "all"
                    });
                }
                break;
            case ControlType.Image:
                me.find("img").attr("src", options.DefaultValue);
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("img").css("width", options.Width + "px");
                me.find("img").css("height", options.Height + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Image属性', "/FormDesigner/Home/ConfigureControl?ControlType=" + ControlType.Image + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.TextBox:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置TextBox属性', "/FormDesigner/Home/ConfigureControl?ControlType=" + ControlType.TextBox + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.HiddenInput:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置HiddenInput属性', "/FormDesigner/Home/ConfigureControl?ControlType=" + ControlType.HiddenInput + "&ControlID=" + options.Name, 650, 380, true);
                });
                break;
            case ControlType.SysVariable:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置SysVariable属性', "/FormDesigner/Home/SystemControl?ControlType=" + ControlType.SysVariable + "&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.TextArea:
                me.find("label").text(options.Text);
                me.find("textarea").attr("Value", options.DefaultValue);
                me.find("textarea").attr("DataType", options.DataType);
                me.find("textarea").attr("cols", options.Cols);
                me.find("textarea").attr("rows", options.Rows);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("textarea").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("textarea").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("textarea").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("textarea").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置TextArea属性', "/FormDesigner/Home/ConfigureControl?ControlType=TextArea&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.Button:
                me.find("input").val(options.Text);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find(".waterfall").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find(".waterfall").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Button属性', "/FormDesigner/Home/ButtonConfigure?ControlType=Button&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.DropDown:
                me.find("label").text(options.Text);
                me.find("select").val(options.DefaultValue);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("select").attr("disabled", "disabled");
                }
                else {
                    me.find("select").removeAttr("disabled");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("select").css("width", options.Width - me.find("label").width() + "px");
                me.find("select").css("height", options.Height + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置DropDown属性', "/FormDesigner/Home/ChoiceBox?ControlType=DropDown&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.CheckBox:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                if (options.DefaultValue == 1 || options.DefaultValue == true) {
                    me.find("input").attr("checked", "checked");
                }
                else {
                    me.find("input").removeAttr("checked");
                }
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                //me.css("width", options.Width + "px");
                //me.css("height", options.Height + "px");
                //me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                //me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置CheckBox属性', "/FormDesigner/Home/ConfigureControl?ControlType=CheckBox&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.Radio:
                //me.find("label").text(options.Text);
                //me.find("input").attr("Value", options.DefaultValue);
                if (form.Fields && form.Fields.length > 0) {
                    for (var i = 0; i < form.Fields.length; i++) {
                        if (form.Fields[i].Name == options.OldID) {
                            ListItems = form.Fields[i].ListItems;
                        }
                    }
                    if (ListItems && ListItems.length > 0) {
                        $("#" + options.Name).empty();
                        for (var j = 0; j < ListItems.length; j++) {
                            var item = "<div style='float:left; height:25px;line-height:25px;'><input name=\"" + options.Name + "\"  type='radio' id=\"" + options.Name + "checkbox" + j + "\" value=\"" + ListItems[j].Value + "\"></input></div><div style='height:25px;line-height:25px;'><label>" + ListItems[j].Text + "</label></div>"
                            $("#" + options.Name).append(item);
                        }
                    }
                }
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                //me.css("width", options.Width + "px");
                //me.css("height", options.Height + "px");
                $("#" + options.Name).resizable('destroy');
                $("#" + options.Name).resizable({
                    handles: "all"
                });
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Radio属性', "/FormDesigner/Home/ChoiceBox?ControlType=" + ControlType.Radio + "&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.DatePicker:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置DateTime属性', "/FormDesigner/Home/ConfigureControl?ControlType=DatePicker&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.Email:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Email属性', "/FormDesigner/Home/ConfigureControl?ControlType=Email&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.ChooseBox:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置ChooseBox属性', "/FormDesigner/Home/Choosebox?ControlType=ChooseBox&ControlID=" + options.Name, 650, 380, true);
                });

                break;
            case ControlType.Upload:
                me.find("label").text(options.Text);
                me.find("input").attr("Value", options.DefaultValue);
                me.find("input").attr("DataType", options.DataType);
                if ($.trim(options.AccessPattern) == "ReadOnly") {
                    me.find("input").attr("readonly", options.AccessPattern);
                }
                else {
                    me.find("input").removeAttr("readonly");
                }
                me.css("width", options.Width + "px");
                me.css("height", options.Height + "px");
                me.find("input").css("width", options.Width - me.find("label").width() - 5 + "px");
                me.find("input").css("height", options.Height - 5 + "px");
                $("#" + options.Name).bind("dblclick", function () {
                    window.parent.parent.openDialog("actionDialog2", '配置Upload属性', "/FormDesigner/Home/ConfigureControl?ControlType=Upload&ControlID=" + options.Name, 650, 380, true);
                });
                break;
        }
        formDesigner.editItem(options);
        if (configure == 1)
            alert("保存成功");
        //  }
        // catch (ex) {
        //        alert("保存失败");
        //   }

    }
    function hightChart(options, retValue) {
        if (options.DataType === "pie") {
            var data = retValue.data;
            retValue.data = [];
            for (var i = 0; i < retValue.categories.length; i++) {
                retValue.data.push({ name: retValue.categories[i], y: data[i] });
            }
        }
        chart = new Highcharts.Chart({
            chart: {
                renderTo: options.Name,
                type: options.DataType,
                margin: [50, 50, 100, 80]
            },
            title: {
                text: options.Text
            },
            xAxis: {
                categories:
                    retValue.categories
                ,
                labels: {
                    rotation: -45,
                    align: 'right',
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'y值'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        formatter: function () {
                            var formatterData = Math.round(this.percentage * Math.pow(10, 2)) / Math.pow(10, 2);
                            return '<b>' + this.point.name + '</b>: ' + formatterData + ' %';
                        }
                    }
                }
            },
            tooltip: {
                pointFormat: '<b>{point.percentage}%</b>',
                percentageDecimals: 1,
                formatter: function () {
                    if (this.x) {
                        return '<b>' + this.x + '</b><br/>' + this.y;
                    }
                    else {
                        return '<b>' + this.point.name + '</b><br/>' + this.y;
                    }
                }

            },
            series: [{
                name: 'Population',
                data: retValue.data,
                dataLabels: {
                    enabled: true,
                    //     rotation: -90,
                    //     color: '#FFFFFF',
                    //     align: 'right',
                    //       x: -3,
                    //      y: 10,
                    formatter: function () {
                        if (this.x) {
                            return this.y;
                        }
                        else {
                            var formatterData = Math.round(this.percentage * Math.pow(10, 2)) / Math.pow(10, 2);
                            return '<b>' + this.point.name + '</b>: ' + formatterData + ' %';
                        }
                    },
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            }]
        });
        //    chart = new Highcharts.Chart({
        //        chart: {
        //            renderTo: options.Name,
        //            type: options.DataType
        //        },
        //        title: {
        //            text: options.Text
        //        },
        //        //subtitle: {
        //        //    text: 'Source: WorldClimate.com'
        //        //},
        //        xAxis: {
        //            categories: [
        //'统计'
        //            ]
        //        },
        //        yAxis: {
        //            min: 0,
        //            title: {
        //                text: 'y值'
        //            }
        //        },
        //        legend: {
        //            layout: 'vertical',
        //            backgroundColor: '#FFFFFF',
        //            align: 'left',
        //            verticalAlign: 'top',
        //            x: 100,
        //            y: 70,
        //            floating: true,
        //            shadow: true
        //        },
        //        tooltip: {
        //            formatter: function () {
        //                return '' +
        //                    this.x + ': ' + this.y;
        //            }
        //        },
        //        plotOptions: {
        //            column: {
        //                pointPadding: 0.2,
        //                borderWidth: 0
        //            }
        //        },
        //        series: retValue
        //        //series: [{
        //        //    name: 'cpu',
        //        //    data: [83.6]

        //        //}, {
        //        //    name: '网络',
        //        //    data: [92.3]
        //        //},
        //        //{
        //        //    name: '硬盘',
        //        //    data: [98.5]

        //        //},
        //        //{
        //        //    name: '内存',
        //        //    data: [93.4]

        //        //}]
        //    });
        $("#" + options.Name).resizable('destroy');
        $("#" + options.Name).resizable({
            handles: "all"
        });
    }
    function disFreeAtr(retValue) {
        for (var i = 0; i < retValue.length; i++) {
            if (retValue[i].children.length > 0) {
                disFreeAtr(retValue[i].children);
            }
            else {
                retValue[i].children = undefined;
                retValue[i].state = undefined;
            }
        }
        return
    }
    function selectNode(e, data) {
    }
    function initJSTreeContextMenu(ListItems, ID) {
        var bindObject = {
            text: "显示名称",
            ico: null,
            cls: "attrcontrol",
            callback: "function"
        };
        var contextMenuBind = [];// [{type:"root",bindObject:[bindObject]}];
        for (var i = 0; i < ListItems.length; i++) {
            if (i == 0) {
                contextMenuBind.push({ type: ListItems[i].Value, bindObject: [ListItems[i].Text] });
            }
            else {
                var sametype = true;
                for (var j = 0; j < contextMenuBind.length; j++) {
                    if (contextMenuBind[j].type == ListItems[i].Value) {
                        contextMenuBind[j].bindObject.push(ListItems[i].Text);
                        sametype = false;
                        break;
                    }
                }
                if (sametype) {
                    contextMenuBind.push({ type: ListItems[i].Value, bindObject: [ListItems[i].Text] });
                }
            }

        }
        for (var m = 0; m < contextMenuBind.length; m++) {
            var ulmenu = ' <ul id="ulmenu' + ID + '" class="contextMenu" style="font-size: 12px;">';
            for (var n = 0; n < contextMenuBind[m].bindObject.length; n++) {
                var bind = JSON2.parse(eval("'" + contextMenuBind[m].bindObject[n] + "'"));
                bind = $.extend({}, bindObject, bind);
                var liClass = bind.cls;
                var liCallback = bind.callback;
                var liText = bind.text;
                var limenu = '<li class="' + liClass + '"><a href="#' + liCallback + '">' + liText + '</a></li>';
                ulmenu = ulmenu + limenu;
            }
            ulmenu = ulmenu + '</ul>';
            if ($("#ulmenu" + ID).length < 1) {
                $(document).find("body").append(ulmenu);
            }
            var licontextMenu = $("li[type='" + contextMenuBind[m].type + "']").find("a");
            if ($("li[type='" + contextMenuBind[m].type + "']").find("a").length < 1) {
                licontextMenu = $("li[type='root']").find("a")
            }
            licontextMenu.bind("mousedown", function () {
                if (window.event) //停止事件向下传播
                    window.event.cancelBubble = true;
                else {
                    e.stopPropagation();
                }
            });
            if (licontextMenu.length > 0) {
                licontextMenu.contextMenu({
                    menu: "ulmenu" + ID
                }, function (action, el, pos) {
                    var menuaction = eval(action);
                    if (typeof (menuaction) == "function") {
                        menuaction.call(this, el, pos);
                    }
                })
            }
        }
    };
})(window)