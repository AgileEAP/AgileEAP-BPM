/// <reference path="Jquery/jquery-1.4.2.min.js" />

function correctPNG() {
    for (var i = 0; i < document.images.length; i++) {
        var img = document.images[i]
        var imgName = img.src.toUpperCase();
        if (imgName.substring(imgName.length - 3, imgName.length) == "PNG") {
            var imgID = (img.id) ? "id='" + img.id + "' " : ""
            var imgClass = (img.className) ? "class='" + img.className + "' " : ""
            var imgTitle = (img.title) ? "title='" + img.title + "' " : "title='" + img.alt + "' "
            var imgStyle = "display:inline-block;" + img.style.cssText
            if (img.align == "left") imgStyle = "float:left;" + imgStyle
            if (img.align == "right") imgStyle = "float:right;" + imgStyle
        if (img.parentElement.href) imgStyle = "cursor:pointer;" + imgStyle
            var strNewHTML = "<span " + imgID + imgClass + imgTitle
   + " style=\"" + "width:" + img.width + "px; height:" + img.height + "px;" + imgStyle + ";"
   + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
   + "(src=\'" + img.src + "\', sizingMethod='scale');\"></span>"
            img.outerHTML = strNewHTML
            i = i - 1
        };
    };
};

if (navigator.userAgent.indexOf("MSIE") > -1) {
    window.attachEvent("onload", correctPNG);
    }
//    $(document).ready(function () {
//        correctPNG();
//    });
//function correctPNG() {
//    $("img").each(function (i) {
//        if (this.src.substring(this.src.length - 3, this.src.length) == "PNG" || this.src.substring(this.src.length - 3, this.src.length) == "png") {
//            var imgPNG = document.createElement("img");

//            var imgID = this.id ? "id='" + img.id + "' " : "";

//            var imgClass = (this.className) ? "class='" + this.className + "' " : ""
//            var imgTitle = (this.title) ? "title='" + this.title + "' " : "title='" + this.alt + "' "
//            var imgStyle = "display:inline-block;" + this.style.cssText;
//            if (this.align == "left") imgStyle = "float:left;" + imgStyle
//            if (this.align == "right") imgStyle = "float:right;" + imgStyle
//            //            if (this.parentElement.href) imgStyle = "cursor:pointer;" + imgStyle
//            var imgWidth = this.width;
//            var imagHeight = this.height;
//            $(this).html("<span " + imgID + imgClass + imgTitle
//               + " style=\"" + "width:" + imgWidth + "px; height:" + imagHeight + "px;" + imgStyle + ";"
//               + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
//               + "(src=\'" + this.src + "\', sizingMethod='scale');\"></span>");
//            i = i - 1;
//        }
//    });
//}