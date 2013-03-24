//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] 用于放置整个站点的公共js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2009-12-20
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
/// <reference path="jquery-vsdoc.js" />
/// <reference path="Cookie.js" />

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//Author     ：trenhui
//Descripton ：GridView相关方法
//Begin      ：
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
$(function () {
    try {
        //初始化当前行
        var id = $("#hidCurrentId").val(); // getCookie("CurrentId");
        initCurrentRow(id);

    } catch (e) { }
});

var currentRow = null;
var oldRow = null;
function rowClick(me, id, remember) {
    if (currentRow) {
        currentRow.className = "gridview_row";
    }

    me.className = "rowcurrent";

    currentRow = me;

    $("input:radio", me).attr("checked", true);
    if (id == '') {
        id = $("input:radio", me).val();
    }
    if (contextStore == "cookie") {
        if (remember)
            setCookie("CurrentId", id);
    } else {
        if (remember)
            $("#hidCurrentId").val(id);
    }

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

function rowDbClick(me, id, remember, url) {
    if (contextStore == "cookie") {
        if (remember)
            setCookie("CurrentId", id);
    } else {
        if (remember)
            $("#hidCurrentId").val(id)
    }

    openActionDialog("actionDialog", "View", "View", "查看", false, 0, 0, url, id);
}

function initCurrentRow(id) {

    try {
        var curObject = $("input[id='radioId'][value='" + id + "']");
        if (curObject[0]) {
            id = curObject.val();
        }
        else {
            curObject = $("input[id='radioId']:first");
            id = curObject.val();

            if (contextStore == "cookie") {
                setCookie("CurrentId", id);
            } else {
                $("#hidCurrentId").val(id)
            }
        }

        if (currentRow) {
            currentRow.className = "gridview_row";
        }

        var me = curObject.parent().parent().get(0);
        me.className = "rowcurrent";

        currentRow = me;

        $("input:radio", me).attr("checked", true);
        if (id == '') {
            id = $("input:radio", me).val();
        }
        if (contextStore == "cookie") {
            if (remember)
                setCookie("CurrentId", id);
        } else {
            if (remember)
                $("#hidCurrentId").val(id);
        }
    }
    catch (e) { }
}
