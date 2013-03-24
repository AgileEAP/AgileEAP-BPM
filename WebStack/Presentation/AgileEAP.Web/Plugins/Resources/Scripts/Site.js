//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] 用于放置整个站点的公共js脚本
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2009-07-16
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]
// [描    述]
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
/// <reference path="jquery-vsdoc.js" />
function refreshParent() {
    if (window.parent) {
        window.parent.location.reload();
        return;
    }
    if (window.opener) {
        window.opener.location.reload();
    }
}

function closeDialog(refresh, contaioner) {
    var doRefresh = refresh || 0;
    var dialogContainer = contaioner || "actionDialog";
    try {
        var actionDialog = $("#" + dialogContainer);
        if (actionDialog && actionDialog.has("iframe")[0]) {
            actionDialog.dialog("close");
        }
        else if (window.parent) {
            window.parent.$("#" + dialogContainer).dialog("close");
        }
        if (refresh) window.parent.frames["ifrContentPage"].location.reload();
    }
    catch (e) { }
}
function getCurrentUrl() {
    return document.URL.split('#')[0];
}

function promptMessage(message) {
    alert(message);
}

function startWith(value, searchValue) {

    if (value.length < searchValue) return false;

    if (searchValue.length > 0) {
        for (var i = 0; i < searchValue.length; i++) {
            if (value.charAt(i) != searchValue.charAt(i))
                return false;
        }
        return true;
    }
    return false;
}

function endWith(value, searchValue) {

    if (value.length < searchValue) return false;

    if (searchValue.length > 0) {
        for (var i = searchValue.length - 1; i > 0; i--) {
            if (value.charAt(value.length - (searchValue.length - i)) != searchValue.charAt(i))
                return false;
        }
        return true;
    }
    return false;
}

function getEvent() {
    if (document.all) {
        return window.event; //如果是ie
    }
    func = getEvent.caller;
    while (func != null) {
        var arg0 = func.arguments[0];
        if (arg0) {
            if ((arg0.constructor == Event || arg0.constructor == MouseEvent) || (typeof (arg0) == "object" && arg0.preventDefault && arg0.stopPropagation)) {
                return arg0;
            }
        }
        func = func.caller;
    }
    return null;
}

function getObjectValue(container) {
    var objValue = new Object();
    var inputs = container && container != "" ? $(":input", $("#" + container)) : $(":input");
    inputs.each(function () {
        var values = this.id.split("_");
        var fullName = values[values.length - 1];
        var property = startWith(fullName, "filter") ? fullName.substr(6) : fullName.substr(3);

        if (property == "SortOrder") {
            objValue[property] = parseInt($(this).val());
        }
        else if ($(this).attr("tag") == "choosebox") {
            if (endWith(property, "data")) {
                objValue[property.substr(0, property.length - 4)] = $(this).val();
            }
        }
        else if ($(this).attr("tag") == "combox") {
            if (endWith(property, "data")) {
                var enumValue = parseInt($(this).val());
                if (!enumValue)
                    enumValue = $(this).val();
                objValue[property.substr(0, property.length - 4)] = enumValue;
            }
        }
        else if (property != "ParentID") {
            if ($(this).attr("type") == "checkbox") {
                objValue[property] = $(this).attr("checked") == true ? 1 : 0;
            }
            else {
                objValue[property] = $(this).val();
            }
        }
    });
    return objValue;
}

//返回选择对象
function chooseConfirm() {
    var item = $(":checked ").first();
    var retValue = new Object();
    retValue.text = item.attr("text");
    retValue.value = item.attr("value");
    retValue.tag = item.attr("tag");
    window.returnValue = retValue;
    window.close();
}

function getCurrentDate() {
    var today = new Date();
    var day = today.getDate();
    var month = today.getMonth() + 1;
    var year = today.getFullYear();
    return date = year + "-" + month + "-" + day;
}

//合并行 add by 【hgq】
jQuery.fn.rowspan = function (colIdx) {
    return this.each(function () {
        var that;
        $('tr', this).each(function (row) {
            var thisRow = $('td:eq(' + colIdx + '),th:eq(' + colIdx + ')', this);
            if (that && thisRow.html() == that.html()) {
                rowspan = $(that).attr("rowspan");
                if (rowspan == undefined) {
                    that.attr("rowspan", 1);
                    rowspan = "1";//that).attr("rowspan");
                }
                rowspan = Number(rowspan) + 1;
                that.attr("rowspan", rowspan);
                thisRow.remove(); ////$(thisRow).hide();
            } else {
                that = thisRow;
            }
            that = (that == null) ? thisRow : that;
        });
    });
}

function getWindowHeight() {
    var windowHeight = 0;
    if (typeof (window.innerHeight) == 'number') {
        windowHeight = window.innerHeight;
    }
    else {
        if (document.documentElement && document.documentElement.clientHeight) {
            windowHeight = document.documentElement.clientHeight;
        }
        else {
            if (document.body && document.body.clientHeight) {
                windowHeight = document.body.clientHeight;
            }
        }
    }
    return windowHeight;
}

function executeCommand(cmd, arg) {
    try {
        var action = cmd.substring(0, 1).toLowerCase() + cmd.substr(1);
        eval(action + "('" + arg + "');");
    } catch (e) { }
}