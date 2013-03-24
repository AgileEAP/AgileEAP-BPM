/// <reference path="../jquery-vsdoc.js" />

(function ($) {
    $.fn.startMove = function (options) {
        var defaults = {
            evt: window.event,
            left: "navRegion",
            leftcontent: "navbg",
            leftwidth: 193,
            leftdragwidth:193,
            contentTop: 110,
            ifmove: 0,
            contentwidth: document.body.offsetWidth - 193,
            rightdragwidth:300
        };
        var config = $.extend({}, defaults, options);
        splitMouseStart();
        function splitMouseStart() {
            e = window.event || config.evt;
            var pointx = (e.clientX + $(document).scrollLeft()) || e.offsetX;
            var pointy = (e.clientY + $(document).scrollTop()) || e.offsetY;
            var item = "<div id=\"split_frame\" style=\"background:#fff;opacity: 0;filter:Alpha(Opacity=0);position:absolute;top:80px;\"></div>";
            $("#contentRegion").append(item);
            var windowHeight = getWindowHeight();
            document.getElementById("split_frame").style.width = document.body.offsetWidth - config.leftwidth+"px";
            document.getElementById("split_frame").style.height = windowHeight - config.contentTop+"px";
            document.getElementById("split_frame").style.left = pointx-60 + "px";
            document.onselectstart = function (e) { return false };
            document.onmousemove = null;
            document.onmouseup = splitMouseup;
            document.onmousemove = splitMouseMove;
        }
        function splitMouseMove() {
            config.ifmove = 1;
            e = window.event || arguments[0];
            $(document).attr("style", "cursor: col-resize");
            var windowHeight = getWindowHeight();
            var pointx = (e.clientX + $(document).scrollLeft()) || e.offsetX;
            var pointy = (e.clientY + $(document).scrollTop()) || e.offsetY;

            if (pointx > config.leftdragwidth && document.body.offsetWidth-pointx>config.rightdragwidth) {
                document.getElementById("splitBar").style.height = windowHeight - config.contentTop + "px";
                document.getElementById("splitBar").style.left = pointx + "px";
            }
            else if (pointx <= config.leftdragwidth) {
                document.getElementById("splitBar").style.left = config.leftwidth + "px";
            }
            else {
                document.getElementById("splitBar").style.left = document.body.offsetWidth - config.rightdragwidth + "px";
            }
        }
        function splitMouseup() {
            document.onmousemove = null;
            if (config.ifmove == 1) {
                e = window.event || arguments[0];
                var endX = (e.clientX + $(document).scrollLeft()) || e.offsetX;
                var endY = (e.clientY + $(document).scrollTop()) || e.offsetY;
                if (endX > config.leftwidth && document.body.offsetWidth - endX > config.rightdragwidth) {
                    
                    document.getElementById(config.left).style.width = endX + "px";
                    document.getElementById(config.leftcontent).style.width = endX - 9 + "px";
                  //  document.getElementById("contentRegion").style.width = document.body.offsetWidth - endX-9 + "px";
                }
                else if (endX <= config.leftdragwidth) {
                    document.getElementById(config.left).style.width = config.leftwidth + "px";
                    document.getElementById(config.leftcontent).style.width = config.leftwidth - 9 + "px";
                    // document.getElementById("contentRegion").style.width = document.body.offsetWidth - config.leftwidth-9 + "px";
                }
                else {
                    document.getElementById(config.left).style.width = document.body.offsetWidth - config.rightdragwidth + "px";
                    document.getElementById(config.leftcontent).style.width = document.body.offsetWidth - config.rightdragwidth - 9 + "px";
                }
            }
            $("#split_frame").remove();
            config.ifmove = 0;

        }
    }
    $.extend({
        startMove: $.fn.startMove
    });
})(jQuery);