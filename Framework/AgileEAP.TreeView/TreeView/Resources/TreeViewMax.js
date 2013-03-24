/// <reference path="jquery-1.3.2-vsdoc.js" />
/// <reference path="TreeViewDrag.js" />
//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] TreeView js脚本
// 修改AjaxTree，使支持同一页面支持多颗树            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2011-8-2
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================

var TreeView = function (id) {
    var options = arguments[1] || {},
        selectionMode = options.selectionMode || "RelatedMultiple",
        ajaxLoading = options.ajaxLoading || false,
        enableDrag = options.enableDrag || false,
        treeView_root_line = options.treeView_root_line,
        treeView_folder = options.treeView_folder,
        treeView_node = options.treeView_node,
        treeView_bottom_line = options.treeView_bottom_line,
        treeView_middle_line = options.treeView_middle_line,
        treeView_vertical_line = options.treeView_vertical_line,
        treeView_minus = options.treeView_minus,
        treeView_plus = options.treeView_plus,
        treeView_top_line = options.treeView_top_line,
        onNodeClick = options.nodeClick || "treeViewNodeAction";

    $(document).ready(function () {
        var container = $("#" + id);
        var currentId = $("#" + id + "CurrentNodeId").val();
        if (currentId || currentId == "") {
            currentId = container.find("li:first").attr("id");
            $("#" + "CurrentNodeId").val(currentId);
        }

        $("#" + currentId).find("a:first").addClass("treeView-tree-activenode");

        //绑定图标展开事件
        bindNodeToggleEvent();

        //绑定checkbox选中事件
        bindCheckBoxEvent();

        //绑定结点A单击事件
        bindNodeEvent();
    });

    this.bindTreeNodeEvent = function () {
        //绑定图标展开事件
        bindNodeToggleEvent();

        //绑定checkbox选中事件
        bindCheckBoxEvent();

        //绑定结点A单击事件
        bindNodeEvent();
    }

    //绑定CheckBox事件
    function bindNodeToggleEvent() {
        $("#" + id).find("img[nodeIcoState]").click(function () {
            var state = $(this).attr("nodeIcoState");

            var nodeID = $(this).parent().attr("id");
            if (state == "ajaxLoad") {
                if (ajaxLoading == true) { alert("正在加载，请等待！"); return false; }
                onAjaxLoad(nodeID);
            }
            else {
                onNodeToggle(nodeID);
            }
        });
    }

    //绑定CheckBox事件
    function bindCheckBoxEvent() {
        $("#" + id).find("input[type='checkbox']").click(function () {
            var nodeID = $(this).attr("id").replace("chk_", "");
            if (selectionMode == "Single") {//单选
                $("#" + id).find(":checkbox").attr("checked", false);
                $("#" + id + "CurrentNodeId").val(nodeID)
                $(this).attr("checked", true);
            }
            else if (selectionMode == "RelatedMultiple") {//级联多选
                $("ul:first", $("#" + nodeID)).find(":checkbox").attr("checked", this.checked);
                parentChecked(this);
            }
            else if (selectionMode == "Multiple") {//多选
            }
            else if (selectionMode == "LeafMultiple") {
                var children = $("ul:first", $("#" + nodeID)).find(":checkbox").attr("checked", this.checked);
                if (children[0]) {
                    parentChecked(this);
                }
            }
        });
    }

    //绑定结点事件
    function bindNodeEvent() {
        $("#" + id).find("a").click(function () {
            var parentNode = $(this).parent();
            var nodeID = parentNode.attr("id");
            var value = parentNode.attr("bindvalue");
            activeCurrentNode2(nodeID);

            if (!startWith(nodeID, "newNode"))
                $("#" + id + "CurrentNodeId").val(nodeID);

            var postType = $("#" + nodeID).attr("postType");
            if (postType == "Post") {
                try {
                    __doPostBack(nodeID, value);
                } catch (e) { }
            }
            else if (postType != "None") {
                try {
                    eval(onNodeClick + "('" + nodeID + "','" + value + "')");
                }
                catch (e) {
                    alert("调用函数" + onNodeClick + "('" + nodeID + "','" + value + "')" + "出错");
                }
            }
            return true;
        });
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

    function onNodeToggle(nodeID) {
        if (nodeID == "") return;

        var me = $("#" + nodeID);
        var children = $("ul:first", me);
        if (children[0]) {
            children.toggle();
            var ico = $("img", me).eq(0)
            var icoSrc = ico.attr("src");

            if (icoSrc == treeView_minus) {
                ico.attr("src", treeView_plus);
            }
            else {
                ico.attr("src", treeView_minus);
            }
        }
    }

    var getCheckedNodeIds = function () {
        var selectNodes = "";
        $("input:checked", $("#" + id)).each(function () {
            selectNodes += this.id.replace("chk_", "") + ",";
        });
        $("#" + id + "SelectedNodeIds").val(selectNodes);

        return selectNodes;
    }

    this.getCurrentNode = function () {
        var currentNode = new Object();
        try {
            var nodeID = $("#" + id + "CurrentNodeId").val();
            var node = $("#" + nodeID);
            currentNode.id = nodeID;
            currentNode.text = node.attr("text");
            currentNode.tooltip = node.attr("title");
            currentNode.value = node.attr("bindvalue");
            currentNode.tag = node.attr("tag");
            currentNode.extend = node.attr("extend");

            $("#" + id + "CurrentNode").val(JSON.stringify(currentNode));
        } catch (e) { }

        return currentNode;
    }

    this.getCheckedNodes = function () {
        var selectNodes = new Array();
        try {
            $("input:checked", $("#" + id)).each(function (i) {
                var id = this.id.replace("chk_", "");
                var node = $("#" + id);
                var item = new Object();
                item.id = id;
                item.text = node.attr("text");
                item.tooltip = node.attr("title");
                item.value = node.attr("bindvalue");
                item.tag = node.attr("tag");
                item.extend = node.attr("extend");
                selectNodes[i] = item;
            });

            $("#" + id + "SelectNodes").val(JSON.stringify(selectNodes));
        } catch (e) { }

        return selectNodes;
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

    function onAjaxLoad(nodeID) {
        var isLast = $("#" + nodeID).attr("isLast");
        var onloadMessage = "<img src=\"" + treeView_onLoad + "\" />正在加载…";
        $("#" + id + "PromptMessage").append(onloadMessage);
        $("#" + id + "PromptMessage").show();
        ajaxLoading = true;

        $.post(getCurrentUrl(), { AjaxAction: "AjaxExpand", AjaxArgument: nodeID, IsLastNode: isLast }, function (value) {
            $("#" + nodeID).append(value);
            $("#" + nodeID).attr("nodeState", "toggleNode");
            $("#" + id + "PromptMessage").html("");
            $("#" + id + "PromptMessage").hide();
            ajaxLoading = false;

            var me = $("#" + nodeID);
            var children = $("ul:first", me);
            if (children[0]) {
                var ico = $("img", me).eq(0)
                var icoSrc = ico.attr("src");

                if (icoSrc == treeView_minus) {
                    ico.attr("src", treeView_plus);
                }
                else {
                    ico.attr("src", treeView_minus);
                }
            }
        });
    }

    this.expandNode = function (nodeID, depth) {
        if (nodeID == "") return;

        var expandDepth = depth == undefined ? 1 : depth;
        var state = $("#" + nodeID).attr("nodeState");
        if (state == "ajaxLoad") {
            if (ajaxLoading == true) { alert("正在加载，请等待！"); return false; }
            onAjaxLoad(nodeID);
        }

        var me = $("#" + nodeID);
        var ul = me.children("ul:first");
        if (ul[0]) {
            if (expandDepth > 0) {
                me.children("img:first").attr("src", treeView_minus);
                ul.show();

                ul.children("li").each(function () {
                    expandNode($(this).attr("id"), expandDepth - 1);
                });
            }
        }
    }

    this.addTreeViewNode = function (parentId, nodeId, nodeValue, nodeText, nodeImg, nodeTag, parentNodeImg) {
        if ($("#" + nodeId)[0]) {
            $("#" + nodeId).find("a:first").text(nodeText);
            alert("结点" + nodeId + "已经存在");
            return;
        }

        var iconImg = nodeImg || treeView_node;
        var item = "<li id=\"" + nodeId + "\" class=\"line-bottom\" title=\"" + nodeText + "\" bindvalue=\"" + nodeValue + "\" virtualNodeCount=\"0\" postType=\"NoPost\" tag=\""
                     + nodeTag + "\" nodeState=\"toggleNode\" islast=\"true\" >"
                     + "<img src=" + treeView_bottom_line + " style=\"border-width: 0px;\" />"
                     + "<img src=" + iconImg + " style=\"border-width: 0px;\" />"
                     + "<a href=\"javascript:void(0)\" target=\"_self\" >" + nodeText + "</a>"
                     + "</li>";

        var curNode;
        var children;
        var brother;
        //添加子结点
        if (parentId != "" && parentId != undefined) {
            var curNode = $("#" + parentId);
            children = $("ul:first", curNode);
            brother = $("li:last", children);

            if (brother.attr("id")) {
                brother = $("li:last", children);
                if (brother[0]) {
                    brother.find("img:first").attr("src", treeView_middle_line);
                }
                children.append(item);
            }
            else {
                var nodeImgs = curNode.find("img");
                nodeImgs.eq(0).attr("src", treeView_minus);
                nodeImgs.eq(0).bind("click", function () {
                    onNodeToggle(parentId);
                });

                if (parentNodeImg != "" && parentNodeImg != undefined) {
                    nodeImgs.eq(1).attr("src", parentNodeImg);
                }

                var virtualNodeCount = curNode.attr("virtualNodeCount");
                if (virtualNodeCount != "" && parseInt(virtualNodeCount) > 0) {
                    item = "<ul style=\"background: url(" + treeView_vertical_line + ") repeat-y 0px 0px;\">" + item + "</ul>";
                } else {
                    if (curNode.first().next("li").size() <= 0) {
                        item = "<ul >" + item + "</ul>";

                    } else {
                        item = "<ul style=\"background: url(" + treeView_vertical_line + ") repeat-y 0px 0px;\">" + item + "</ul>";
                    }
                }

                curNode.append(item);
            }
        }
        else {//添加根结点
            curNode = $("#" + id);
            children = $("ul:first", curNode);
            if (children[0]) {
                brother = $("li:last", children);
                if (brother[0]) {
                    var virtualNodeCount = brother.attr("virtualNodeCount");
                    if (virtualNodeCount != "" && parseInt(virtualNodeCount) > 0) {
                        $("ul:first", children).attr("style", "background: url(" + treeView_vertical_line + ") repeat-y 0px 0px;");
                    }
                    else {
                        brother.find("img:first").attr("src", treeView_middle_line);
                    }
                }
                children.append(item);
            }
            else {
                item = "<li id=\"" + nodeId + "\" class=\"line-bottom\" title=\"" + nodeText + "\" bindvalue=\"" + nodeValue + "\" virtualNodeCount=\"0\" postType=\"NoPost\" tag=\""
                     + nodeTag + "\" nodeState=\"toggleNode\" islast=\"true\" >"
                     + "<img src=" + treeView_root_line + " style=\"border-width: 0px;\" />"
                     + "<img src=" + iconImg + " style=\"border-width: 0px;\" />"
                     + "<a href=\"javascript:void(0)\" target=\"_self\" >" + nodeText + "</a>"
                     + "</li>";
                curNode.append("<ul>" + item + "</ul>");
            }
        }

        $("#" + nodeId, curNode).find("a").click(function () {
            var parentNode = $(this).parent();
            var nodeID = parentNode.attr("id");
            var value = parentNode.attr("bindvalue");
            activeCurrentNode2(nodeID);

            var postType = $("#" + nodeID).attr("postType");
            if (postType == "Post") {
                try {
                    __doPostBack(nodeID, value);
                } catch (e) { }
            }
            else if (postType != "None") {
                try {
                    eval(onNodeClick + "('" + nodeID + "','" + value + "')");
                }
                catch (e) {
                    alert("调用函数" + onNodeClick + "('" + nodeID + "','" + value + "')" + "出错");
                }
            }
            return true;
        });

        return nodeId;
    }

    function activeCurrentNode2(nodeID) {
        $("#" + id).find(".treeView-tree-activenode").removeClass("treeView-tree-activenode");

        $("#" + nodeID).find("a:first").addClass("treeView-tree-activenode");
        $("#" + id + "CurrentNodeId").val(nodeID);
    }

    this.activeCurrentNode = function (nodeID) {
        $("#" + id).find(".treeView-tree-activenode").removeClass("treeView-tree-activenode");

        $("#" + nodeID).find("a:first").addClass("treeView-tree-activenode");
        $("#" + id + "CurrentNodeId").val(nodeID);
    }

    function activeNode(node) {
        node.find("a:first").addClass("treeView-tree-activenode");

        var parentNode = node;
        while (parentNode[0] && parentNode.parent()[0] && parentNode.attr("id") != id) {
            if (parentNode.attr("tagName") == "LI") {
                if (node !== parentNode) {
                    var ico = $("img:first", parentNode).eq(0)
                    ico.attr("src", treeView_minus);
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
    this.findTreeNode = function (value, mode) {
        if (value == "") {
            alert("请输入查找关键字！");
            return;
        }

        $("#" + id).find("a").removeClass("treeView-tree-activenode");
        var searchMode = mode || 0;
        var isFind = false;
        $("#" + id).find("li").each(function (i) {
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
}

