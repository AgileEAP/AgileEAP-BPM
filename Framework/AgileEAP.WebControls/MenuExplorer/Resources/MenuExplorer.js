/// <reference path="jquery-1.3.2-vsdoc.js" />

//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] AjaxTree js脚本
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

//显示导航菜单
function showMenuExplorer() {
    var container = document.getElementById('menuExplorerContainer');
    var oElems = container.getElementsByTagName('table');
    for (var i = 0; i < oElems.length; i++)
        oElems[i].style.display = '';
}

//初始化导航菜单
function initMenuExplorer() {
    var container = document.getElementById('menuExplorerContainer');
    var oElems = container.getElementsByTagName('table');
    for (var i = 0; i < oElems.length; i++)
        oElems[i].style.display = 'none';
    var menuGroupId = getCookie('CurMenuGroup');
    var currentMenuGroup = document.getElementById(menuGroupId);
    if (currentMenuGroup) currentMenuGroup.style.display = '';
}

if (document.all) {
    window.attachEvent('onload', initMenuExplorer);
}
else { window.addEventListener('load', initMenuExplorer, false); }

function activeMenuGroup(curMenuGroup, curUrl, me) {    
    document.cookie = 'CurMenuGroup=MenuGroup_Header_' + curMenuGroup;
    document.cookie = 'CurUrl=' + curUrl;

    var currentNavText = document.getElementById('currentNavText');
    if (currentNavText) {        
        var activeMenu = document.getElementById(getCookie('ActiveMenuId'));
        if (activeMenu) {            
            var secondMenu = document.getElementById('MenuGroup_Header_' + curMenuGroup);
            if(navigator.appName.indexOf('Explorer')>'-1')
                currentNavText.innerText = activeMenu.innerText + '>' + secondMenu.innerText + '>' + me.innerText;
            else
                currentNavText.textContent = activeMenu.textContent + '>' + secondMenu.textContent + '>' + me.textContent;
        }
    }
}

//显示导航菜单组
function showMenuGroup(subMenu) {
    var body = document.getElementById(subMenu);
    if (body.style.display == 'none') {
        body.style.display = '';
    } else {
        body.style.display = 'none';
    }
    try {
        var container = document.getElementById('menuExplorerContainer');
        var oElems = container.getElementsByTagName('table');
        if (oElems) {
            for (var i = 0; i < oElems.length; i++)
                if (oElems[i] != body) oElems[i].style.display = 'none';
        }
    }
    catch (ex) { }

}

