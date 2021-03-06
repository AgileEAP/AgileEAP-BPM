﻿
/// <reference path="jquery-1.3.2-vsdoc.js" />

//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] Drag js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    
// [日期]       2009-11-17
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
//辅助函数1
var getCoords = function(el) {
    try {
        var box = el.getBoundingClientRect(),
        doc = el.ownerDocument,
        body = doc.body,
        html = doc.documentElement,
        clientTop = html.clientTop || body.clientTop || 0,
        clientLeft = html.clientLeft || body.clientLeft || 0,
        top = box.top + (self.pageYOffset || html.scrollTop || body.scrollTop) - clientTop,
        left = box.left + (self.pageXOffset || html.scrollLeft || body.scrollLeft) - clientLeft
        return { 'top': top, 'left': left };
    }
    catch (e) {
        return { 'top': (self.pageYOffset || html.scrollTop || body.scrollTop) - clientTop, 'left': (self.pageXOffset || html.scrollLeft || body.scrollLeft) - clientLeft };
    }
};
//辅助函数2
var getStyle = function(el, style) {
    if (! +"\v1") {
        style = style.replace(/\-(\w)/g, function(all, letter) {
            return letter.toUpperCase();
        });
        var value = el.currentStyle[style];
        (value == "auto") && (value = "0px");
        return value;
    } else {
        return document.defaultView.getComputedStyle(el, null).getPropertyValue(style)
    }
}
//============================
var Drag = function(id) {
    var el = document.getElementById(id),
        isQuirk = document.documentMode ? document.documentMode == 5 : document.compatMode && document.compatMode != "CSS1Compat",
        options = arguments[1] || {},
        container = options.container || document.documentElement,
        limit = options.limit,
        lockX = options.lockX,
        lockY = options.lockY,
        ghosting = options.ghosting,
        handle = options.handle,
        revert = options.revert,
        scroll = options.scroll,
        coords = options.coords,
        onStart = options.onStart || function() { },
        onDrag = options.onDrag || function() { },
        onEnd = options.onEnd || function() { },
        marginLeft = parseFloat(getStyle(el, "margin-left")),
        marginRight = parseFloat(getStyle(el, "margin-right")),
        marginTop = parseFloat(getStyle(el, "margin-top")),
        marginBottom = parseFloat(getStyle(el, "margin-bottom")),
        cls,
        _handle,
        _ghost,
        _top,
        _left,
        _html;
    el.lockX = getCoords(el).left;
    el.lockY = getCoords(el).top;
    el.style.position = "absolute";
    if (handle) {
        cls = new RegExp("(^|\\s)" + handle + "(\\s|$)");
        for (var i = 0, l = el.childNodes.length; i < l; i++) {
            var child = el.childNodes[i];
            if (child.nodeType == 1 && cls.test(child.className)) {
                _handle = child;
                break;
            }
        }
    }
    _html = (_handle || el).innerHTML;
    var dragstart = function(e) {
        e = e || window.event;
        el.offset_x = e.clientX - el.offsetLeft;
        el.offset_y = e.clientY - el.offsetTop;
        document.onmouseup = dragend;
        document.onmousemove = drag;
        if (ghosting) {
            _ghost = el.cloneNode(false);
            el.parentNode.insertBefore(_ghost, el.nextSibling);
            if (_handle) {
                _handle = _handle.cloneNode(false);
                _ghost.appendChild(_handle);
            }
            ! +"\v1" ? _ghost.style.filter = "alpha(opacity=50)" : _ghost.style.opacity = 0.5;
        }
        (_ghost || el).style.zIndex = ++Drag.z;
        onStart();
        return false;
    }
    var drag = function(e) {
        e = e || window.event;
        el.style.cursor = "pointer";
        ! +"\v1" ? document.selection.empty() : window.getSelection().removeAllRanges();
        _left = e.clientX - el.offset_x;
        _top = e.clientY - el.offset_y;
        if (scroll) {
            var doc = isQuirk ? document.body : document.documentElement;
            doc = options.container || doc;
            options.container && (options.container.style.overflow = "auto");
            var a = getCoords(el).left + el.offsetWidth;
            var b = doc.clientWidth;
            if (a > b) {
                doc.scrollLeft = a - b;
            }
            var c = getCoords(el).top + el.offsetHeight;
            var d = doc.clientHeight;
            if (c > d) {
                doc.scrollTop = c - d;
            }
        }
        if (limit) {
            var _right = _left + el.offsetWidth,
            _bottom = _top + el.offsetHeight,
            _cCoords = getCoords(container),
            _cLeft = _cCoords.left,
            _cTop = _cCoords.top,
            _cRight = _cLeft + container.clientWidth,
            _cBottom = _cTop + container.clientHeight;
            _left = Math.max(_left, _cLeft);
            _top = Math.max(_top, _cTop);
            if (_right > _cRight) {
                _left = _cRight - el.offsetWidth - marginLeft - marginRight;
            }
            if (_bottom > _cBottom) {
                _top = _cBottom - el.offsetHeight - marginTop - marginBottom;
            }
        }
        lockX && (_left = el.lockX);
        lockY && (_top = el.lockY);
        (_ghost || el).style.left = _left + "px";
        (_ghost || el).style.top = _top + "px";
        coords && ((_handle || _ghost || el).innerHTML = _left + " x " + _top);
        onDrag();
    }

    var dragend = function() {
        document.onmouseup = null;
        document.onmousemove = null;
        _ghost && el.parentNode.removeChild(_ghost);
        el.style.left = (_left || el.lockX) + "px";
        el.style.top = (_top || el.lockY) + "px";
        (_handle || el).innerHTML = _html;
        if (revert) {
            el.style.left = el.lockX + "px";
            el.style.top = el.lockY + "px";
        }
        onEnd();
    }
    Drag.z = 999;
    (_handle || el).onmousedown = dragstart;
}

