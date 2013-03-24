/// <reference path="jquery-1.4.2.min-vsdoc.js" />
//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] ReportGrid.js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2010-6-17
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================

function getFilterValues() {
    var filterValues = new Object();
    var inputs = $("input[id^='filter']");
    inputs.each(function () {
        var property = fullName.substr(6);
        if ($(this).attr("tag") == "choosebox") {
            if (endWith(property, "data")) {
                filterValues[property.substr(0, property.length - 4)] = $(this).val();
            }
        }
        else if ($(this).attr("tag") == "combox") {
            if (endWith(property, "data")) {
                var enumValue = parseInt($(this).val());
                if (!enumValue) {
                    enumValue = $(this).val();
                }
                filterValues[property.substr(0, property.length - 4)] = enumValue;
            }
        }
        else {
            filterValues[property] = $(this).val();
        }
    });
    return filterValues;
}

function getCurrentUrl() {
    return document.URL.split('#')[0];
}

function navigate(action) {
    var filterValues = getFilterValues();
    filterValues.action = action;

    $.post(getCurrentUrl(), { AjaxAction: "AjaxSearch", AjaxArgument: $.parseJSON(filterValues) }, function (value) {
        $("#ajax_grid_view").html(value);
    });
}

//弹出操作对话框：container 对话容器，title窗体标题，url页面地址，width弹出窗宽度，height弹出窗框高度,showModal是否模态显示，style弹出样式（0表示弹出div,1表示弹出窗体）
function openDialog(container, title, url, width, height, showModal, style) {
    var dialogWidth = width || 350;
    var dialogHeitht = height || 400;
    var dialogShowModal = showModal || false;
    var dialogStyle = style || 0; //0表示弹出div,1表示弹出窗体

    var appendSign = url.indexOf("?") > 0 ? "&" : "?";
    url = url + appendSign + "random=" + Math.random();

    if (dialogStyle == 1) {
        if (showModal) {
            var cssDialog = "dialogHeight:" + height + "px; dialogWidth:" + width + "px; edge: Raised; center: Yes; resizable: Yes; status: No;scrollbars=no,";
            return window.showModalDialog(url, title, cssDialog);
        }
        else {
            var cssDialog = 'center: Yes,status=yes,menubar=no,scrollbars=no,resizable=yes,width=' + width + ',height=' + height;
            window.open(url, title, cssDialog);
        }
    }
    else {
        container = "#" + container;

        if ($(container).html() != "")
            $(container).dialog("destroy");
        $(container).html('<iframe id="bg_div_iframe" width="100%" height="98%" allowTransparency="true" frameborder="0"></iframe>');
        $('#bg_div_iframe').attr('src', url);

        $(container).dialog({
            bgiframe: true,
            autoOpen: false,
            width: dialogWidth,
            height: dialogHeitht,
            modal: dialogShowModal
        });
        $(container).dialog("option", "title", title);
        $(container).dialog("open");
        return true;
    }
}

//argument查询方案code,ctrlId=字段id,url打开页面地址,title页面标题,width弹出窗宽度，height弹出窗框高度
function openChooseTreeDialog(argument, ctrlId, url, title, width, height) {
    var appendSign = url.indexOf("?") > 0 ? "&" : "?";
    url = url + appendSign + "argument=" + argument;
    openDialog("actionDialog", title, url, width, height);


}

function openChooseBoxDialog(ctrlId, url, title, width, height) {
    openDialog("actionDialog", title, url, width, height);
}