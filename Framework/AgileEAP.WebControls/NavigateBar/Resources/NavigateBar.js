/// <reference path="jquery-1.3.2-vsdoc.js" />
/// <reference path="Drag.js" />
//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] NavigateBar.jss脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2009-11-12
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
//function updateNavBar(currentmenuId) {
//    var oldActiveMenuId = getCookie("ActiveMenuId");
//    if (oldActiveMenuId != "")
//        $("#" + oldActiveMenuId).removeClass("currentmenu");
//    $("#" + currentmenuId).addClass("currentmenu");
//    setCookie("ActiveMenuId", currentmenuId);
//    $("#currentNavText").text($("#" + currentmenuId).text());

//    $.post("<%=RootPath %>Ajax/AjaxHandler.ashx", { processClass: "AjaxMenuExplorer", argument: currentmenuId }, function(value) {
//        $("#menu").html(value);
//    });
//}   