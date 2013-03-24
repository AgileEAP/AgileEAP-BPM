<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="OrgTree.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.OrgTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%=HtmlExtensions.RequireStyles("jQueryContextMenu_default") %>
    <%=HtmlExtensions.RequireScripts("jQueryContextMenu") %>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript">

        var iconParent = "<%=RootPath %>Plugins/Authorize/Content/Themes//<%=Skin %>/Images/orgtree.gif";
        var iconChild = "<%=RootPath %>Plugins/Authorize/Content/Themes//<%=Skin %>/Images/orgtree.gif";


        function addNewAjaxTreeNode(parentId, nodeId, nodeName, nodeType) {
            if ($("#" + nodeId)[0]) {
                $("#" + nodeId).find("a:first").text(nodeName);
                return;
            }

            var iconImg = ajaxTreeSetting.ajaxTree_node

            switch (nodeType) {
                case "parent":
                    iconImg = iconParent;
                    break;
                case "child":
                    iconImg = iconChild;
                    break;
            }

            var item = "<li id=\"" + nodeId + "\" class=\"line-bottom\" title=\"" + nodeName + "\" bindvalue=\"" + nodeId + "\" virtualNodeCount=\"0\" postType=\"NoPost\" tag=\""
                     + nodeType + "\" nodeState=\"toggleNode\" islast=\"true\" >"
                     + "<img src=" + ajaxTreeSetting.ajaxTree_bottom_line + " style=\"border-width: 0px;\" />"
                     + "<img src=" + iconImg + " style=\"border-width: 0px;\" />"
                     + "<a href=\"OrgUserList.aspx?orgid=" + nodeId + "\" target=\"ifrMain\" onclick=\"onNodeClick('"
                     + nodeId + "','" + nodeId + "')\">" + nodeName + "</a>"
                     + "</li>";

            var curNode = $("#" + parentId);
            var children;
            var brother;
            //添加子结点
            if (curNode[0]) {
                children = $("ul:first", curNode);
                brother = $("li:last", children);

                if (brother.attr("id")) {
                    brother = $("li:last", children);
                    if (brother[0]) {
                        brother.find("img:first").attr("src", ajaxTreeSetting.ajaxTree_middle_line);
                    }
                    children.append(item);
                }
                else {
                    curNode.find("img:first").attr("src", ajaxTreeSetting.ajaxTree_minus);
                    curNode.find("img:first").bind("click", function () {
                        onNodeIcoClick(parentId);
                    });

                    var virtualNodeCount = curNode.attr("virtualNodeCount");
                    if (virtualNodeCount != "" && parseInt(virtualNodeCount) > 0) {
                        item = "<ul style=\"background: url(" + ajaxTreeSetting.ajaxTree_vertical_line + ") repeat-y 0px 0px;\">" + item + "</ul>";
                    } else {
                        if (curNode.first().next("li").size() <= 0) {
                            item = "<ul >" + item + "</ul>";

                        } else {
                            item = "<ul style=\"background: url(" + ajaxTreeSetting.ajaxTree_vertical_line + ") repeat-y 0px 0px;\">" + item + "</ul>";
                        }
                    }

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

            initContextMenu();

            return nodeId;
        }

        function ajaxTreeNodeAction(id, value) {
            $("#ifrDict", window.parent.document.body).attr("src", "DictDetail.aspx?ActionFlag=Update&CurrentId=" + id);
        }

        document.oncontextmenu = function (e) { return false; }

        function initContextMenu() {
            $("li A").contextMenu({
                menu: 'treeContextMenu'
            }, function (action, el, pos) {

                activeCurrentNode(el.context.parentNode.id);

                if (action == "addDept") {
                    $("#ifrMain", window.parent.document.body).attr("src", "DeptDetail.aspx?ActionFlag=Add&ParentId=" + el.context.parentNode.id);
                }
                if (action == "editDept") {
                    $("#ifrMain", window.parent.document.body).attr("src", "DeptDetail.aspx?ActionFlag=Update&CurrentId=" + el.context.parentNode.id);
                }
                if (action == "addUser") {
                    $("#ifrMain", window.parent.document.body).attr("src", "UserDetail.aspx?ActionFlag=Add&OrgID=" + el.context.parentNode.id);
                }
                if (action == "delDept") {
                    $.post("OrgTree.aspx", { AjaxAction: "DeleteTreeNode", AjaxArgument: el.context.parentNode.id }, function (result) {
                        var ajaxResult = JSON2.parse(result);
                        var message = "操作失败";
                        if (ajaxResult) {
                            if (ajaxResult.PromptMsg != null)
                                message = ajaxResult.PromptMsg
                            if (ajaxResult.Result == 1) {
                                if (message == "")
                                    message = "操作成功！";
                                $("#" + el.context.parentNode.id).remove();
                                if (ajaxResult.RetValue != "")
                                    $("#ifrMain", window.parent.document.body).attr("src", "DeptDetail.aspx?ActionFlag=Update&CurrentId=" + ajaxResult.RetValue);
                            }
                        }
                        alert(message);
                    });
                }
            });
        }

        $(document).ready(function () {
            //默认展开一级菜单
            expandNode('0', '1');
            initContextMenu();
        });
    </script>
    <style type="text/css">
        .ajaxTree-container
        {
            position: absolute;
            left: -18px;
            overflow: auto;
            height: 100%;
            width: 110%;
        }
        html, body, form
        {
            margin: 0px;
            height: 100%;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <agile:AjaxTree ID="AjaxTree1" Runat="server" />
    <ul id="treeContextMenu" class="contextMenu" style="font-size: 12px;">
        <li class="copy"><a href="#addDept">新增部门</a></li>
        <li class="edit"><a href="#editDept">编辑部门</a></li>
        <li class="delete"><a href="#delDept">删除部门</a></li>
        <li class="copy"><a href="#addUser">新增人员</a></li>
    </ul>
    <script language="javascript" type="text/javascript">
        //返回选择对象
        function chooseConfirm() {
            var item = getCurrentNode();
            var retValue = new Object();
            retValue.text = item.text;
            retValue.value = item.id;
            retValue.tag = item.tag;
            window.returnValue = retValue;
            window.close();
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

                if (value != "")
                    initContextMenu();
            });
        }
    </script>
</asp:Content>
