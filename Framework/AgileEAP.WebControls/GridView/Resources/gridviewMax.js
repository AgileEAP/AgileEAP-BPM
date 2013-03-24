//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] GridView相关js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2011-9-2
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.1
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
/// <reference path="jquery-vsdoc.js" />
/// <reference path="Cookie.js" />

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//Author     ：trenhui
//Descripton ：GridView相关方法
//Begin      ：
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

var currentRow = null;
var oldRow = null;
function rowClick(me, id) {
    if (currentRow) {
        currentRow.className = "gridview_row";
    }
    me.className = "rowcurrent";
    currentRow = me;

    $("input:radio", me).attr("checked", true);
    if (id == '') {
        id = $("input:radio", me).val();
    }

    $("#hidCurrentId").val(id);
    return id;
}

function rowOver(row) {
    if (oldRow == null) {
        oldRow = row;
        row.className = "rowover";
    }
    else {
        oldRow = row;

        if (currentRow != row)
            row.className = "rowover";
    }
}
function rowOut(me) {
    if (oldRow != null) {
        if (currentRow != me)
            me.className = "rowout";

        oldRow = me;
    }
}

function rowDbClick(me, id) {
    $("#hidCurrentId").val(id)
}

function del(me, argument) {
    var gridview = $(".gridview");
    var rad = $("input:checked");
    var id = rad.val();

    if ($.trim(id) == "") {
        rad.parent().parent().remove();
        alert("操作成功");
        return;
    }

    $.post(getCurrentUrl(), { AjaxAction: "Delete", AjaxArgument: id }, function (result) {
        var ajaxResult = JSON2.parse(result);
        var message = "操作失败";
        if (ajaxResult) {
            if (ajaxResult.PromptMsg != null)
                message = ajaxResult.PromptMsg
            if (ajaxResult.ActionResult == 1) {
                if (message == "")
                    message = "操作成功！";
                rad.parent().parent().remove();
                $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) - 1);
                gridview.find("tbody tr:eq(0)").find("input").attr("checked", "true");
            }
        }
        alert(message);
    });
}