/// <reference path="jquery-vsdoc.js" />
//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] 用于放置整个站点的弹出层js脚本
//----------------------------------------------------------------------------------------------------
// [作者] 	    trenhui
// [日期]       2009-07-16
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]
// [描    述]
// [更新日期]
// [版 本 号] ver1.0
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
var dialogSetting = { viewPageUrl: null, managerPageUrl: null, actionUrl: null, filterRunat: 1, detailWidth: 650, detailHeight: 370, filterWidth: 400, filterHeight: 400 };
$(function () {
    //初始化查询窗体
    var filterDialog = $("#filterDialog");
    if (filterDialog[0]) {
        filterDialog.dialog({
            bgiframe: true,
            autoOpen: false,
            width: dialogSetting.filterWidth,
            height: dialogSetting.filterHeight,
            maxWidth: dialogSetting.filterWidth,
            maxHeight: dialogSetting.filterHeight,
            minWidth: dialogSetting.filterWidth,
            minHeight: dialogSetting.filterHeight,
            modal: false,
            position: ['center', 30],
            buttons: {
                '取 消': function () {
                    $(this).dialog('close');
                },
                '清 除': function () {
                    $("input[type='text'][name*='filter']").each(function () {
                        $(this).val('');
                    });
                    $("select[name*='filter']").each(function () {
                        $(this).val('-1');
                    });
                },

                '确 定': function () {
                    $(this).dialog('close');
                    filterAction();
                }
            },
            close: function () {
                //clearValues();
            }
        });
    }

    //将弹出层移动到aspnetForm里
    $("div[role=dialog]").appendTo("form#form1");
});

function doPostBack() {
    //document.location.reload();
    //__doPostBack('ctl00$commandBar', 'Search$Search');
}


//默认自定义查询方法有用勿删
function filterAction() {
    __doPostBack('ctl00$commandBar', 'Search$Search');
}

function openFilterDialog(me, argument) {
    $('#filterDialog').dialog('open');
}

//弹出操作对话框：container 对话容器，cmdName执行的命令，argument命令参数，showModal是否模态显示，style弹出样式（0表示弹出div,1表示弹出窗体）
function openActionDialog(url, cmdName, argument, actionName, dialogSetting, showModal, refreshParent) {
    var dialogShowModal = showModal || false;
    var refresh = refreshParent || 1; //1刷新
    var actionDialog = $("#actionDialog");
    if (actionDialog && actionDialog.has("iframe")[0])
        actionDialog.dialog("destroy");
    actionDialog.html('<iframe id="bg_div_iframe" scrolling="no"  width="100%" height="98%" allowTransparency="true" frameborder="0"></iframe>');
    actionDialog.find('#bg_div_iframe').attr('src', url);

    actionDialog.dialog({
        bgiframe: true,
        autoOpen: false,
        width: dialogSetting.detailWidth,
        height: dialogSetting.detailHeight,
        maxWidth: dialogSetting.detailWidth,
        maxHeight: dialogSetting.detailHeight,
        minWidth: dialogSetting.detailWidth,
        minHeight: dialogSetting.detailHeight,
        modal: dialogShowModal,
        position: ['center', 100],
        close: function () {
            if (cmdName != "View" && refresh == 1)
                doPostBack();
        }
    });
    actionDialog.dialog("option", "title", actionName);
    actionDialog.dialog("open");
}

//弹出操作对话框：title窗体标题，url页面地址，width弹出窗宽度，height弹出窗框高度,showModal是否模态显示，style弹出样式（0表示弹出div,1表示弹出窗体）
function openOperateDialog(title, url, width, height, showModal, style, marginTop) {
    return openDialog("actionDialog", title, url, width, height, showModal, style, marginTop);
}

//弹出操作对话框：title窗体标题，url页面地址，width弹出窗宽度，height弹出窗框高度,showModal是否模态显示，style弹出样式（0表示弹出div,1表示弹出窗体）
function openDialog(container, title, url, width, height, showModal, style, marginTop) {
    var dialogContainer = container || "actionDialog";
    var dialogWidth = width || 350;
    var dialogHeight = height || 400;
    var dialogShowModal = showModal || false;
    var dialogStyle = style || 0; //0表示弹出div,1表示弹出窗体
    var dialogMarginTop = marginTop || 100;

    //var appendSign = url.indexOf("?") > 0 ? "&" : "?";
    //url = url + appendSign;//+ "random=" + Math.random();

    if (dialogStyle == 1) {
        if (showModal) {
            var cssDialog = "dialogHeight:" + height + "px; dialogWidth:" + width + "px; edge: Raised; center: Yes; resizable: Yes; status: No;scrollbars=no,";
            return window.showModalDialog(url, window, cssDialog);
        }
        else {
            var cssDialog = 'center: Yes,status=yes,menubar=no,scrollbars=no,resizable=yes,width=' + width + ',height=' + height;
            window.open(url, title, cssDialog);
        }
    }
    else {
        var operateDialog = $("#" + dialogContainer);

        if (operateDialog && operateDialog.has("iframe")[0])
            operateDialog.dialog("destroy");
        operateDialog.html('<iframe id="bg_div_iframe" width="100%" height="98%" allowTransparency="true" frameborder="0"></iframe>');
        operateDialog.find('#bg_div_iframe').attr('src', url);

        operateDialog.dialog({
            bgiframe: true,
            autoOpen: false,
            width: dialogWidth,
            height: dialogHeight,
            position: ['center', dialogMarginTop],
            modal: dialogShowModal
        });
        operateDialog.dialog("option", "title", title);
        operateDialog.dialog("open");
        return true;
    }
}

function getAjaxObject(me, cmdName, argument) {
    return getObjectValue();
}

function commandExecute(me, cmdName, argument, isAjax) {
    if (isAjax) {
        try {
            var actionName = cmdName.substring(0, 1).toLowerCase() + cmdName.substr(1);
            eval(actionName + "(me,'" + argument + "');");
        } catch (e) { }
    }
    else {
        var currentURL = document.URL.split("#")[0].replace("Manager.aspx", "Detail.aspx")
        var currentID = cmdName == "Add" ? "" : "&CurrentId=" + document.getElementById("hidCurrentId").value;
        var url = dialogSetting.actionUrl || currentURL;

        if (cmdName == "View") url = url.replace("Entry=Manager", "Entry=View").replace("Entry=Update", "Entry=View");
        var joinTag = url.indexOf('?') > 0 ? "&" : "?";
        url += joinTag + "ActionFlag=" + cmdName + currentID + "&radom=" + Math.random();

        if (cmdName == "Search")
            openFilterDialog(me, argument);
        else if (cmdName == "Add" || cmdName == "Update" || cmdName == "View") {
            if (window.parent) {
                window.parent.openActionDialog(url, cmdName, argument, me.innerText, dialogSetting);
            } else {
                openActionDialog(url, cmdName, argument, me.innerText, dialogSetting);
            }
        }
        else {
            if (cmdName == "Delete") {
                if (!confirm("是否确定删除记录？")) {
                    return false;
                }
                del($("#hidCurrentId").val());
            }
            var actionName = cmdName.substring(0, 1).toLowerCase() + cmdName.substr(1);
            try {
                eval(actionName + "(me,'" + argument + "');");
            } catch (e) { }
        }
    }
}
function openDialog2(options) {
    var defaults =
        {
            container: "actionDialog",
            position:document,
            width: 350,
            height: 400,
            maxDialog: false,
            minDialog:false,
            showModal: false,
            dialogType: 0, //0表示弹出div,1表示弹出窗体
            marginTop: 100,
            title: "窗口",
            url: null,
            contentUrl: null,
            content: null,
            resizable: true,
            dialogClass: null,
            customdialogSylte: null,
            windowStyle: { style: "edge: Raised; center: Yes; resizable: Yes; status: no;scrollbars=no", auguments: window },
            divContentStyle: { style: "font-size:14px;margin:0 auto;padding:20px;text-align:left", bgiframe: true, autoOpen: false },
            buttons: null
        };
    var config = $.extend({}, defaults, options);
    var frame = config.url ? true : false;
    if (config.dialogType == 1) {
        var cssDialog = "dialogHeight:" + config.height + "px; dialogWidth:" + config.width + "px;" + config.windowStyle.style;
        if (config.showModal) {
            return window.showModalDialog(config.url, config.windowStyle.auguments, cssDialog);
        }
        else {
            return window.showModelessDialog(url, config.windowStyle.auguments, cssDialog);
            // var cssDialog = 'center: Yes,status=yes,menubar=no,scrollbars=no,resizable=yes,width=' + config.width + ',height=' + config.height;
            // window.open(url, title, cssDialog);
        }
    }
    else {
        var operateDialog = $("#" + config.container);
        if (operateDialog.length <= 0) {
            var id = config.container;
            dialogItem = '<div id="' + id + '" style="height: 0px; border: 0px; overflow: hidden; font-size: 13px;">';
            $(config.position).find("body").append(dialogItem);
            operateDialog = $("#" + id);
        }
        if (frame) {
            if (operateDialog && operateDialog.has("iframe")[0]) {
                operateDialog.dialog("destroy");
            }
            operateDialog.html('<iframe id="bg_div_iframe" width="100%" height="100%" allowTransparency="true" frameborder="0"></iframe>');
            operateDialog.find('#bg_div_iframe').attr('src', config.url);
            var $waterfall = addWaterfall("#" + config.container);
            $waterfall.hide();
            operateDialog.dialog({
                bgiframe: true,
                autoOpen: false,
                width: config.width,
                height: config.height,
                resizable:config.resizable,
                position: ['center', config.marginTop],
                modal: config.showModal,
                resizeStart: function (event, ui) {
                    window.console.log("开始调整大小");
                    $waterfall.show();
                },
                resizeStop: function (event, ui) {
                    window.console.log("结束调整大小");
                    $waterfall.hide();
                }
            });
            operateDialog.dialog("option", "title", config.title);
            operateDialog.dialog("open");
            var $headerBar = $("#" + config.container).prev();
            $headerBar.bind('dblclick', function () {
            });
            $headerBar.bind('mousedown', function () {
                $waterfall.show();
            });
            $headerBar.bind('mouseup', function () {
                $waterfall.hide();
            });
            return true;
        }
        else {
            if (operateDialog) {
                operateDialog.empty();
            }
            if (config.contentUrl) {
                $.get(config.contentUrl)
           .done(function (content, nativeStatusText, responses) {
               config.content = content;
              var reg = /<script[^>]*>([sS]*)<\/script>/g;
              var contentscript = content.match(reg);
              var currentscript = document.getElementsByTagName("script");
              var existscript = false;
              for (var i = 0; i < contentscript.length; i++) {
                  for (var j = 0; j < currentscript.length; j++) {
                      if ($(currentscript[j]).attr("src") == $(contentscript[i]).attr("src")) {
                          existscript = true;
                          break;
                      }
                  }
                  if (!existscript) {
                      (function () {
                       //  function async_load() {
                              var po = document.createElement("script");
                              po.type = "text/javascript"; po.async = true; po.src = $(contentscript[i]).attr("src");
                              var s = document.getElementsByTagName("script")[0];
                              s.parentNode.insertBefore(po, s);
                        //  };
                      //    if (window.attachEvent)
                      //        window.attachEvent('onload', async_load);
                     //     else
                     //         window.addEventListener('load', async_load, false);
                      })();
                  }
                  existscript = false;
               }
               operateDialog.html(content);
               operateDialog.dialog({
                   bgiframe: config.divContentStyle.bgiframe,
                   autoOpen: config.divContentStyle.autoOpen,
                   width: config.width,
                   height: config.height,
                   resizable:config.resizable,
                   position: ['center', config.marginTop],
                   modal: config.showModal,
                   close: function( event, ui ) 
                   {
                       operateDialog.dialog("destroy");
                   },
                   buttons: config.buttons
               });
               operateDialog.dialog("option", "title", config.title);
               operateDialog.dialog("open");
           });
            }
            else {
                if (operateDialog && operateDialog.children().length>0) {
                    operateDialog.dialog("destroy");
                }
                operateDialog.html("<div style='" + config.divContentStyle.style + "'>" + config.content + "</div>");
                operateDialog.dialog({
                    bgiframe: config.divContentStyle.bgiframe,
                    autoOpen: config.divContentStyle.autoOpen,
                    option:config.dialogClass,
                    width: config.width,
                    resizable:config.resizable,
                    position: ['center', config.marginTop],
                    modal: config.showModal,
                    buttons: config.buttons
                });
                operateDialog.dialog("option", "title", config.title);
                if (config.customdialogSylte) {
                    $(document).find(".ui-widget-header").attr("style",config.customdialogSylte.head);
                    operateDialog.attr("style", config.customdialogSylte.body);
                    $(document).find(".ui-dialog-buttonpane").attr("style", config.customdialogSylte.footer);
                    $(document).find(".ui-dialog-buttonpane").children().css("float", "none");
                }
                operateDialog.dialog("open");
            }
        }
    }
}
function showDialog(container, title, url, width, height, showModal, style, marginTop, content) {

    var dialogContainer = container || "actionDialog";
    var dialogWidth = width || 350;
    var dialogHeight = height || 400;
    var dialogShowModal = showModal || false;
    var dialogStyle = style || 0; //0表示弹出div,1表示弹出窗体
    var dialogMarginTop = marginTop || 100;
    var frame = url ? true : false;
    var dialogContent = frame ? url : content;
    var operateDialog = $("#" + dialogContainer);
    if (frame) {
        if (operateDialog && operateDialog.has("iframe")[0])
            operateDialog.dialog("destroy");
        operateDialog.html('<iframe id="bg_div_iframe" width="100%" height="100%" allowTransparency="true" frameborder="0"></iframe>');
        operateDialog.find('#bg_div_iframe').attr('src', dialogContent);
        var $waterfall = addWaterfall("#" + dialogContainer);
        $waterfall.hide();
        operateDialog.dialog({
            bgiframe: true,
            autoOpen: false,
            width: dialogWidth,
            height: dialogHeight,
            resizable:true,
            position: ['center', dialogMarginTop],
            modal: dialogShowModal,
            resizeStart: function (event, ui) {
                window.console.log("开始调整大小");
                $waterfall.show();
            },
            resizeStop: function (event, ui) {
                window.console.log("结束调整大小");
                $waterfall.hide();
            }
        });
        operateDialog.dialog("option", "title", title);
        operateDialog.dialog("open");
        var $headerBar = $("#" + dialogContainer).prev();
        $headerBar.bind('dblclick', function () {
            //alert("双击");
            // window.moveTo(0, 0);
            // window.resizeTo(window.screen.availWidth, window.screen.availHeight);
            // $("#" + dialogContainer)
        });
        $headerBar.bind('mousedown', function () {
            //  window.console.info.apply(window.console, ["mousedown"]);
            $waterfall.show();
        });
        $headerBar.bind('mouseup', function () {
            //  window.console.info.apply(window.console, ["mouseup"]);
            $waterfall.hide();
        });

        return true;
    }
    else {
        operateDialog.html("<div style=\"font-size:14px;margin:0 auto;padding:20px;text-align:center\">" + dialogContent + "</div>");
        operateDialog.dialog({
            bgiframe: true,
            autoOpen: false,
            width: dialogWidth,
            height: dialogHeight,
            position: ['center', dialogMarginTop],
            modal: dialogShowModal,
            buttons: {
                '取 消': function () {
                    $(this).dialog('close');
                },
                '确 定': function () {
                    $(this).dialog('close');
                    // filterAction();
                }
            }
        });
        operateDialog.dialog("option", "title", title);
        operateDialog.dialog("open");
    }

}
//增加遮罩层
function addWaterfall(outterDiv) {
    var waterfall = $(document.createElement("div"));
    waterfall.css({
        "height": "100%",
        "width": "100%",
        "filter": "alpha(opacity = 50)",
        "-moz-opacity": "0.1",
        "opacity": "0.1",
        "background-color": "#fff",
        "position": "absolute",
        "left": "0px",
        "top": "30px",
        "display": "none"
    });
    $(waterfall).appendTo(outterDiv);
    $(waterfall).bind('click', function () {
        $(this).hide();
    });
    return $(waterfall);
}