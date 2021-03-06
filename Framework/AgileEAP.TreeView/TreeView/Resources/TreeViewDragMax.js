﻿/// <reference path="jquery-1.3.2-vsdoc.js" />

//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] Drag js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    trh
// [日期]       2009-11-17
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
var Drag = function(id) {
    var el = null,
        options = arguments[1] || {},
        onStart = options.onStart || function() { },
        onDrag = options.onDrag || function() { },
        onEnd = options.onEnd || function() { },
        dragNode,
        x,
        y,
        left,
        top;

    var dragstart = function(e) {
        e = e || window.event;
        el = e.target || e.srcElement;
        if (!(el.tagName == "A" && el.getAttribute("dragNode"))) {
            return;
        }
        el = el.parentNode;

        dragNode = document.getElementById("TreeViewDragContainer");
        dragNode.style.position = 'absolute';
        dragNode.style.display = 'none';
        activeCurrentNode($(el).attr("id"));

        var dragIcon = document.createElement("img");
        dragIcon.setAttribute("src", TreeViewSetting.TreeView_drag_enable);
        dragNode.appendChild(dragIcon);

        var ghost = document.createElement("UL");
        ghost.appendChild(el.cloneNode(true));
        dragNode.appendChild(ghost);

        dragNode.style.left = e.pageX || (e.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft));
        dragNode.style.top = e.pageY || (e.clientY + (document.documentElement.scrollTop || document.body.scrollTop));

        onStart();

        document.onmouseup = dragend;
        document.onmousemove = drag;

        return false;
    }
    var drag = function(e) {
        //判断是否是树结点
        if (!(el.tagName == "LI" && el.getAttribute("nodeState"))) {
            return;
        }

        e = e || window.event;
        var target = e.target || e.srcElement;
        if (target.parentNode != el && target.parentNode.tagName == "LI" && target.tagName == "A" && target.getAttribute("dragNode")) {
            target = target.parentNode;
            var id = target.getAttribute("id");
            if ($(el).find("#" + id)[0]) {
                $(dragNode).children("img:first").attr("src", TreeViewSetting.TreeView_drag_disable);
            }
            else {
                $(dragNode).children("img:first").attr("src", TreeViewSetting.TreeView_drag_enable);
            }
        }
        else {
            $(dragNode).children("img:first").attr("src", TreeViewSetting.TreeView_drag_disable);
        }

        autoScroll(e);
        dragNode.style.cursor = "pointer";
        var x = e.pageX || (e.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft));
        var y = e.pageY || (e.clientY + (document.documentElement.scrollTop || document.body.scrollTop));

        dragNode.style.left = x + 2 + 'px';
        dragNode.style.top = y + 2 + 'px';
        dragNode.style.display = 'block';
        dragNode.style.zIndex = Drag.z;

        onDrag();
        return false;
    }

    autoScroll = function(e) {
        var step = 10;
        var wh = window.innerHeight || document.body.clientHeight;
        if (e.clientY < 20) {
            window.scrollBy(0, -step);
        }
        else if (e.clientY > wh - 20) {
            window.scrollBy(0, step);
        }
    }

    var dragend = function(e) {
        document.onmouseup = null;
        document.onmousemove = null;
        e = e || window.event;
        var target = e.target || e.srcElement;
        if (target.parentNode != el && target.parentNode.tagName == "LI" && target.tagName == "A" && target.getAttribute("dragNode")) {
            target = target.parentNode;

            var id = target.getAttribute("id");
            if ($(el).find("#" + id)[0]) {
                $(dragNode).children("img:first").attr("src", TreeViewSetting.TreeView_drag_disable);
                dragNode.innerHTML = "";
                dragNode.style.display = 'none';
                return false;
            }

            var parentNode = $(el).parent();
            var ul = $(target).find("ul:first");
            if (ul[0]) {
                ul[0].appendChild(el);
                refreshParentNodeState(parentNode);
                expandNode(id);
            }
            else {
                var nodeIcon = $(target).children("img:first");
                nodeIcon.attr("src", TreeViewSetting.TreeView_minus);
                // nodeIcon.unbind("click");
                nodeIcon.bind("click", function() {
                    onNodeIcoClick(id);
                });

                var eUL = document.createElement("UL");
                $(el).attr("isLast", "true");
                eUL.appendChild(el);
                target.appendChild(eUL);

                refreshParentNodeState(parentNode);
            }

            //更新树结点状态
            refreshNodeState($(target));
        }

        onEnd();

        dragNode.style.display = 'none';
        dragNode.innerHTML = "";
        return true;
    }

    var refreshParentNodeState = function(parentNode) {
        if (parentNode[0]) {
            var prevNode = parentNode.children("li:last");
            if (prevNode[0]) {
                var ul = prevNode.find("ul:first");
                if (ul[0]) {
                    var display = ul.css("display");
                    ul.removeAttr("style");
                    ul.css("display", display);
                }
                else {
                    prevNode.find("img:first").attr("src", TreeViewSetting.TreeView_bottom_line);
                }
                prevNode.attr("style", "background:url(" + TreeViewSetting.TreeView_bottom_line + ")  white no-repeat;_background: url(" + TreeViewSetting.TreeView_bottom_line + ") white no-repeat 0px 3px;");
                prevNode.attr("isLast", "true");
            }
            else {
                parentNode = parentNode.parent();
                parentNode.children("ul:first").remove();
                var nodeIcon = parentNode.find("img:first");
                nodeIcon.attr("src", parentNode.next("li").size() > 0 ? TreeViewSetting.TreeView_middle_line : TreeViewSetting.TreeView_bottom_line);
                nodeIcon.unbind("click");
                parentNode.removeAttr("style");
            }
        }
    }

    var refreshNodeState = function(node) {
        if (node[0]) {
            var ul = node.children("ul:first");
            if (!ul[0]) {
                var liIcon = node.next("li").size() > 0 ? TreeViewSetting.TreeView_middle_line : TreeViewSetting.TreeView_bottom_line;
                var nodeIcon = node.children("img:first");
                nodeIcon.attr("src", liIcon);
                nodeIcon.unbind("click");
                node.removeAttr("style");

                return;
            }

            var display = ul.css("display");
            if (node.next("li").size() > 0) {
                ul.attr("style", "background:url(" + TreeViewSetting.TreeView_vertical_line + ")  white repeat-y;_background: url(" + TreeViewSetting.TreeView_vertical_line + ") white repeat-y 0px;");
            }
            else {
                ul.removeAttr("style");
            }
            ul.css("display", display);

            var childrenCount = ul.children("li").size();
            if (childrenCount > 0) {
                ul.children("li").each(function() {
                    var li = $(this);
                    if (childrenCount == 1) {
                        if (li.children("ul").size() > 0)
                            li.attr("style", "background:url(" + TreeViewSetting.TreeView_bottom_line + ")  white no-repeat;_background: url(" + TreeViewSetting.TreeView_bottom_line + ") white no-repeat 0px;");
                        else {
                            var nodeIcon = li.children("img:first");
                            nodeIcon.attr("src", TreeViewSetting.TreeView_bottom_line);
                            nodeIcon.unbind("click");
                            li.removeAttr("style");
                        }
                    }
                    else {
                        var liIcon = li.next("li").size() > 0 ? TreeViewSetting.TreeView_middle_line : TreeViewSetting.TreeView_bottom_line;
                        li.attr("style", "background:url(" + liIcon + ")  white no-repeat;_background: url(" + liIcon + ") white no-repeat 0px;");
                    }

                    refreshNodeState(li);
                });
            }
        }
    }

    Drag.z = 999;
    document.onmousedown = dragstart;
}
