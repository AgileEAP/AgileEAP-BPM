/// <reference path="jquery-1.3.2-vsdoc.js" />

//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] CommandBar.js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2009-9-3
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================

function formValidatorIsValid(doValid) {
    try {
        if (!doValid) return true;

        return $.formValidator.pageIsValid("1");
    } catch (e) {
    }

    return true;
}

function CommandItemOver(control) {
    if (control.className == "commanditem")
        control.className = "commanditem_over";
}

function CommandItemOut(control) {
    if (control.className == "commanditem_over")
        control.className = "commanditem";
}

// CommandBar
//var CommandBar = function(id) {
//    var el = document.getElementById(id),
//        isQuirk = document.documentMode ? document.documentMode == 5 : document.compatMode && document.compatMode != "CSS1Compat",
//        options = arguments[1] || {},
//        container = options.container || document.documentElement,
//        limit = options.limit,
//        lockX = options.lockX,
//        lockY = options.lockY,
//        ghosting = options.ghosting,
//        handle = options.handle,
//        revert = options.revert,
//        scroll = options.scroll,
//        coords = options.coords,
//        onStart = options.onStart || function() { },
//        onDrag = options.onDrag || function() { },
