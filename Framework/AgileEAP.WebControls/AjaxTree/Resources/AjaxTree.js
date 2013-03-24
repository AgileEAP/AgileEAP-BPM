/// <reference path="jquery-1.3.2-vsdoc.js" />
/// <reference path="AjaxTreeDrag.js" />
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


var ajaxTreeSetting = new Object();
ajaxTreeSetting.formid = "form1";
ajaxTreeSetting.ajaxLoading = false;
var ajaxTreeDrag = null;

function onNodeToggle(id) {
    if (id == "") return;

    var me = $("#" + id);
    var children = $("ul:first", me);
    if (children[0]) {
        children.toggle();
        var ico = $("img", me).eq(0)
        var icoSrc = ico.attr("src");

        if (icoSrc == ajaxTreeSetting.ajaxTree_minus) {
            ico.attr("src", ajaxTreeSetting.ajaxTree_plus);
        }
        else {
            ico.attr("src", ajaxTreeSetting.ajaxTree_minus);
        }
    }
}

$(document).ready(function () {
    var form = $("#" + ajaxTreeSetting.formid);

    if (!form[0])
        form = $(document.forms[0]);

    form.submit(function () {
        getCheckedNodeIds();
    });

    var ids = $("#ajaxTreeSelectNodeIds").val();
    if (ids && ids != "") {
        var idArray = ids.split(',');
        for (var i = 0; i < idArray.length; i++) {
            $("#chk_" + idArray[i]).attr("checked", true);
        }
    }

    var container = $("#ajaxTree_Div_Id");
    var currentId = $("#ajaxTreeCurrentNodeId").val();
    if (currentId == "") {
        currentId = container.find("li:first").attr("id");
        $("#ajaxTreeCurrentNodeId").val(currentId);
    }

    $("#" + currentId).find("a:first").addClass("ajaxTree-tree-activenode");

    //初始化树拖动功能
    if (ajaxTreeSetting.enableDrag) ajaxTreeDrag = new Drag("ajaxTree_Div_Id");
});

function getCheckedNodeIds() {
    var selectNodes = "";
    $("input:checked", $("#ajaxTree_Div_Id")).each(function () {
        selectNodes += this.id.replace("chk_", "") + ",";
    });
    $("#ajaxTreeSelectNodeIds").val(selectNodes);

    return selectNodes;
}

function getCurrentNode() {
    var currentNode = new Object();
    try {
        var id = $("#ajaxTreeCurrentNodeId").val();
        var node = $("#" + id);
        currentNode.id = id;
        currentNode.text = node.attr("text");
        currentNode.tooltip = node.attr("title");
        currentNode.value = node.attr("bindvalue");
        currentNode.tag = node.attr("tag");

        $("#ajaxTreeCurrentNode").val(JSON.stringify(currentNode));
    } catch (e) { }

    return currentNode;
}

function getCheckedNodes() {
    var selectNodes = new Array();
    try {
        $("input:checked", $("#ajaxTree_Div_Id")).each(function (i) {
            var id = this.id.replace("chk_", "");
            var node = $("#" + id);
            var item = new Object();
            item.id = id;
            item.text = node.attr("text");
            item.tooltip = node.attr("title");
            item.value = node.attr("bindvalue");
            item.tag = node.attr("tag");
            selectNodes[i] = item;
        });

        $("#ajaxTreeSelectNodes").val(JSON.stringify(selectNodes));
    } catch (e) { }

    return selectNodes;
}

function onTreeNodeChecked(me, id, mode) {

    if (mode == "Single") {//单选
        $("#ajaxTree_Div_Id").find(":checkbox").attr("checked", false);
        $("#ajaxTreeCurrentNodeId").val(id)
        $(me).attr("checked", true);
    }
    else if (mode == "RelatedMultiple") {//级联多选
        $("ul:first", $("#" + id)).find(":checkbox").attr("checked", me.checked);
        parentChecked(me);
    }
    else if (mode == "Multiple") {//多选
    }
    else if (mode == "LeafMultiple") {
        var children = $("ul:first", $("#" + id)).find(":checkbox").attr("checked", me.checked);
        if (children[0]) {
            parentChecked(me);
        }
    }
}

function parentChecked(curCheckbox) {
    var parentNode = $(curCheckbox).parent().parent().parent();  //getParentByTagName(ele, 3);
    var curNodeChecked = curCheckbox.checked;
    if (parentNode[0]) {
        var parentCheckBoxId = "chk_" + parentNode.attr("id");
        var parentCheckBox = $("#" + "chk_" + parentNode.attr("id"));
        if (parentCheckBox[0]) {
            if (curNodeChecked) {
                parentCheckBox.attr("checked", true);
            }
            else {
                var brotherContainer = $(curCheckbox).parent().parent();
                var hasChecked = false;
                brotherContainer.find(":checkbox").each(function () {
                    if (this.checked) {
                        parentCheckBox.attr("checked", true);
                        hasChecked = true;
                        return;
                    }
                });

                if (!hasChecked)
                    parentCheckBox.attr("checked", false);
            }
            parentChecked(parentCheckBox);
        }
    }
}

function getCurrentUrl() {
    return document.URL.split('#')[0];
}

function onAjaxLoad(id) {
    var isLast = $("#" + id).attr("isLast");
    var onloadMessage = "<img src=\"" + ajaxTreeSetting.ajaxTree_onLoad + "\" />正在加载…";
    $("#ajaxTreePromptMessage").append(onloadMessage);
    $("#ajaxTreePromptMessage").show();
    ajaxTreeSetting.ajaxLoading = true;

    $.post(getCurrentUrl(), { AjaxAction: "AjaxExpand", AjaxArgument: id, IsLastNode: isLast }, function (value) {
        $("#" + id).append(value);
        $("#" + id).attr("nodeState", "toggleNode");
        $("#ajaxTreePromptMessage").html("");
        $("#ajaxTreePromptMessage").hide();
        ajaxTreeSetting.ajaxLoading = false;

        var me = $("#" + id);
        var children = $("ul:first", me);
        if (children[0]) {
            var ico = $("img", me).eq(0)
            var icoSrc = ico.attr("src");

            if (icoSrc == ajaxTreeSetting.ajaxTree_minus) {
                ico.attr("src", ajaxTreeSetting.ajaxTree_plus);
            }
            else {
                ico.attr("src", ajaxTreeSetting.ajaxTree_minus);
            }
        }
    });
}

function onNodeIcoClick(id) {
    var state = $("#" + id).attr("nodeState");

    if (state == "ajaxLoad") {
        if (ajaxTreeSetting.ajaxLoading == true) { alert("正在加载，请等待！"); return false; }
        onAjaxLoad(id);
    }
    else {
        onNodeToggle(id);
    }
}


function expandNode(id, depth) {
    if (id == "") return;

    var expandDepth = depth == undefined ? 1 : depth;
    var state = $("#" + id).attr("nodeState");
    if (state == "ajaxLoad") {
        if (ajaxTreeSetting.ajaxLoading == true) { alert("正在加载，请等待！"); return false; }
        onAjaxLoad(id);
    }

    var me = $("#" + id);
    var ul = me.children("ul:first");
    if (ul[0]) {
        if (expandDepth > 0) {
            me.children("img:first").attr("src", ajaxTreeSetting.ajaxTree_minus);
            ul.show();

            ul.children("li").each(function () {
                expandNode($(this).attr("id"), expandDepth - 1);
            });
        }
    }
}

function addAjaxTreeNode() {
    var curNodeId = $("#ajaxTreeCurrentNodeId").val();
    var newNodeId = "newNode" + Math.random().toString().substr(2);
    var item = "<li id=\"" + newNodeId + "\" class=\"line-bottom\" title=\"\" bindvalue=\"" + newNodeId + "\" virtualNodeCount=\"0\" postType=\"AjaxPost\">"
                     + "<img src=" + ajaxTreeSetting.ajaxTree_bottom_line + " style=\"border-width: 0px;\" />"
                     + "<a href=\"javascript:void(0)\" target=\"_self\">子结点</a>"
                     + "</li>";

    var curNode = $("#" + curNodeId);
    var children;
    var brother;
    //添加子结点
    if (curNode[0]) {
        children = $("ul:first", curNode);
        brother = $("li:last", children);
        if (brother.attr("id")) {
            brother = $("li:last", children);
            if (brother[0]) brother.find("img").attr("src", ajaxTreeSetting.ajaxTree_middle_line);
            children.append(item);
        }
        else {
            item = "<ul>" + item + "</ul>";
            curNode.append(item);
        }
    }
    else {//添加根结点
        curNode = $("#ajaxTree_Div_Id");
        children = $("ul:first", curNode);
        brother = $("li:last", children);
        if (brother[0]) {
            var virtualNodeCount = brother.attr("virtualNodeCount");
            if (virtualNodeCount != "" && parseInt(virtualNodeCount) > 0) {
                $("ul:first", children).attr("style", "background: url(" + ajaxTreeSetting.ajaxTree_vertical_line + ") repeat-y 0px 0px;");
            }
            else {
                brother.find("img").attr("src", ajaxTreeSetting.ajaxTree_middle_line);
            }
        }
        children.append(item);
    }

    return newNodeId;
}

function activeCurrentNode(id) {
    $(".ajaxTree-tree-activenode").removeClass("ajaxTree-tree-activenode");

    $("#" + id).find("a:first").addClass("ajaxTree-tree-activenode");
    $("#ajaxTreeCurrentNodeId").val(id);
}

//NoPost = 0, AjaxPost = 1,Post = 2,None=3
function onNodeClick(id, value) {
    activeCurrentNode(id);

    if (!startWith(id, "newNode"))
        $("#ajaxTreeCurrentNodeId").val(id);

    var postType = $("#" + id).attr("postType");
    var action = "AjaxShowNode";
    var argument = id;
    if (postType == "Post") {
        try {
            __doPostBack(id, value);
        } catch (e) { }
    }
    else if (postType != "None") {
        try {
            eval("ajaxTreeNodeAction('" + id + "','" + value + "')");
        }
        catch (e) { }
    }

    return true;
}

function activeNode(node) {
    node.find("a:first").addClass("ajaxTree-tree-activenode");

    var parentNode = node;
    while (parentNode[0] && parentNode.parent()[0] && parentNode.attr("id") != "ajaxTree_Div_Id") {
        if (parentNode.attr("tagName") == "LI") {
            if (node !== parentNode) {
                var ico = $("img:first", parentNode).eq(0)
                ico.attr("src", ajaxTreeSetting.ajaxTree_minus);
            }
            parentNode = parentNode.parent();
            parentNode.show();
        }
        parentNode = parentNode.parent();
    }
}

//搜索
//value：搜索值
//mode：搜索模式,0搜索文本，1搜索值，2搜索文本和值
function findTreeNode(value, mode) {
    if (value == "") {
        alert("请输入查找关键字！");
        return;
    }

    $("#ajaxTree_Div_Id").find("a").removeClass("ajaxTree-tree-activenode");

    var searchMode = mode || 0;
    var isFind = false;
    $("#ajaxTree_Div_Id").find("li").each(function (i) {
        var node = $(this);
        var item = new Object();
        item.id = node.attr("id");
        item.text = node.attr("text");
        item.tooltip = node.attr("title");
        item.value = node.attr("bindvalue");

        if (searchMode == 0) {
            if (item.text && item.text.indexOf(value) >= 0) {
                activeNode(node);
                isFind = true;
            }
        } else if (searchMode == 1) {
            if (item.value && item.value.indexOf(value) >= 0) {
                activeNode(node);
                isFind = true;
            }
        }
        else {
            if ((item.value && item.value.indexOf(value) >= 0) || (item.text && item.text.indexOf(value) >= 0)) {
                activeNode(node);
                isFind = true;
            }
        }
    });

    if (!isFind) alert("没有相关结点！");
}

