/// <reference path="jquery-vsdoc.js" />
/// <reference path="Dialog.js" />

function checkVersion(availableVersion, requiredVersion) {
    var compatible = false;
    var available = availableVersion.split(".");
    var required = requiredVersion.split(".");
    try {
        for (var i = 0; i < 4; i++) {
            available[i] = parseInt(available[i], 10);
            required[i] = parseInt(required[i], 10);
        }
    } catch (e) {
    }
    if (available[0] > required[0]
							|| (available[0] == required[0] && (available[1] > required[1] || (available[1] == required[1] && (available[2] > required[2] || (available[2] == required[2] && available[3] >= required[3])))))) {
							    compatible = true;
							}
    return compatible;
}

function isMKSInstalled(type) {
    var compatible = false;
    try {
        try {
            var ua = navigator.userAgent.toLowerCase();
        } catch (e) {
        }
        var engine = "mshtml";
        if (ua.indexOf("khtml") > -1) {
            engine = "khtml";
        } else if (ua.indexOf("gecko") > -1) {
            engine = "gecko";
        }
        var availableVersion = null;
        var mimeType = "application/x-vmware-vmrc;version=2.5.0.309851";
        var mimeArray = mimeType.split(";version=");
        var requiredVersion = mimeArray[1];

        switch (engine) {
            case "mshtml":
                try {
                    var objectName = "";
                    switch (type) {
                        case "HyperV":
                            var o = new ActiveXObject("RDPControls.RdpConnector");
                            compatible = true;
                            break;
                        case "VMWareESX":
                            var o = new ActiveXObject("VMware.web.VMwareRemoteConsole.2.5");
                            var availableVersion = o.Version;
                            o = null;
                            compatible = checkVersion(availableVersion, requiredVersion);
                            break;
                    }

                } catch (e) {
                    compatible = false;
                }
                break;
            case "gecko":
                var mimePrefix = mimeArray[0];
                for (var i = 0; i < navigator.mimeTypes.length; i++) {
                    var curType = navigator.mimeTypes[i].type;
                    if (curType.indexOf(mimePrefix) > -1) {
                        try {
                            var curVersion = curType.split("version=")[1];
                        }
                        catch (e) {
                        }
                        if (availableVersion == null
											|| checkVersion(curVersion,
													availableVersion)) {
													    availableVersion = curVersion;
													}
                    }
                }
                if (availableVersion != null) {
                    compatible = checkVersion(
										availableVersion, requiredVersion);
                } else {
                    compatible = false;
                }
                break;
            case "khtml":
                break;
            default:
                break;
        }
    } catch (e) {
    }
    return compatible;
}

////vmName虚拟机名称，type虚拟机类型
//function openConsole(vmName, type) {

//    openConsole(vmName,type,
//}

//vmName虚拟机名称，type虚拟机类型
function openConsole(vmName, type, serverName, resourcePoolID) {
    //    alert("vmName:" + vmName);
    //    alert("type:" + type);
    var fileName = "";
    var CLSID = "";
    switch (type) {
        case "HyperV":
            fileName = "RDPClient.CAB";
            CLSID = "CLSID:{9C87F514-6DA1-4964-B570-147D3EC956DA}";
            break;
        case "VMWareESX":
            fileName = "vmware-vmrc-win32-x86.exe";
            CLSID = "CLSID:B94C2238-346E-4c5e-9B36-8CC627F35574";
            $(document.body).prepend("<object classid='" + CLSID + "' id='mks' codebase='VMRC/" + fileName + "' style='display:none'></object>");
            break;
    }
    if (type != "XenServer" && !isMKSInstalled(type)) {
        if (!confirm('请先下载插件')) {
            return;
        }
        window.parent.parent.open("VMRC/" + fileName);
        return;
    }

    //loading提示
    var loadbar = $("#loadingBar").processbar({ message: '正在启动控制台...' });
    //-----------------------
    $.post(getCurrentUrl(), { AjaxAction: "GetToken", AjaxArgument: decodeURIComponent(vmName), ServerName: serverName, ResourcePoolID: resourcePoolID }, function (result) {
        try {
            var token = JSON2.parse(result);
            if (token) {
                switch (type) {
                    case "HyperV":
                        var ticket = JSON2.parse(token.Ticket);
                        var page = "VRMS/VMRC.htm?vmIP=" + ticket.VMIP + "&virtualiationPlatform=" + type;
                        window.parent.parent.openOperateDialog('控制台', page, 1024, 650, false);
                        break;
                    case "VMWareESX":
                        var objEl = document.getElementById("mks");
                        objEl.connectWithSession(token.VCenter, token.Ticket, token.VMName, 2);
                        break;
                    case "XenServer":
                        var ticket = JSON2.parse(token.Ticket);
                        var vncURL = encodeURIComponent(ticket.URL);
                        window.parent.parent.openOperateDialog('控制台', "VRMS/VMRC.htm?virtualiationPlatform=" + type + "&xenSession=" + ticket.Session + "&vncURL=" + vncURL, 900, 600, true);
                        break;
                }
            }
        } catch (e) { alert("打开控制台出错"); }

        loadbar.complete();
    });
}


