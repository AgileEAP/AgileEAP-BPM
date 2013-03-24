/// <reference path="jquery-vsdoc.js" />

(function ($) {
    $.fn.processbar = function (options) {
        var defaults = {
            barImage: 'process.gif',
            message: '正在处理中',
            width: 120,
            height: 12,
            pos: 0,
            dir: 2,
            len: 0,
            location: document
        };
        var config = $.extend({}, defaults, options);
        return this.each(function () {
            $this = $(this);
            $this.show();
            // $this.html("<div id=\"divParent\"><div id=\"divChild\"><script language=JavaScript type=text/javascript>var t_id = setInterval(animate,20);var pos=0;var dir=2;var len=0;function animate(){var elem = document.getElementById('progress');if(elem != null) {if (pos==0) len += dir;if (len>32 || pos>79) pos += dir;if (pos>79) len -= dir;if (pos>79 && len==0) pos=0;elem.style.left = pos;elem.style.width = len;}}function remove_loading() {this.clearInterval(t_id);var targelem = document.getElementById('loader_container');targelem.style.display='none';targelem.style.visibility='hidden';var divObj = document.getElementById('divChild');divObj.parentNode.removeChild(divObj);}window.onload = remove_loading;</script><style>#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}</style><div id=loader_container><div id=loader><div align=center>" + config.message + "...</div><div id=loader_bg><div id=progress> </div></div></div></div></div></div>");
            $this.html("<div id=\"divParent\"><div id=\"divChild\"><style>#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}</style><div id=loader_container><div id=loader><div align=center>" + config.message + "...</div><div id=loader_bg><div id=progress> </div></div></div></div></div></div>");
            var t_id = setInterval(animate, 20);
            //            var e = document.getElementById('progress');
            //            $this.show();
        });
        function getWindowHight() {
            var windowHeight = getWindowHeight() - 28;
            return windowHeight;
        }
        function animate() {
            var elem = $("#progress");
            if (config.location != document) {
                elem = $("#progress", config.location);
            }
            if (elem != null) {
                if (config.pos == 0)
                    config.len += config.dir;
                if (config.len > 32 || config.pos > 79)
                    config.pos += config.dir;
                if (config.pos > 79)
                    config.len -= config.dir;
                if (config.pos > 79 && config.len == 0)
                    config.pos = 0;
                elem.css("left", config.pos);
                elem.css("width", config.len);
            }
        }
        function remove_loading() {
            this.clearInterval(t_id);
            var targelem = document.getElementById('loader_container');
            targelem.style.display = 'none';
            targelem.style.visibility = 'hidden';
            var divObj = document.getElementById('divChild');
            divObj.parentNode.removeChild(divObj);
        }
    };

    $.fn.complete = function () {
        return this.each(function () {
            $this = $(this);
            $this.hide();
        });
    };
    $.extend({
        processbar: $.fn.processbar
    });
})(jQuery);