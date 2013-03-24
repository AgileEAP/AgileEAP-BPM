<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ChooseParticipantor.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.ChooseParticipantor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentPlace" runat="server">
    <script language="javascript" type="text/javascript">
        //页面初始化
        $(document).ready(function () {
            $("#chkAll").click(function () {
                if ($("#chkAll").is(":checked")) {
                    $("#selectedParticipantor").find("input[type=checkbox]").attr("checked", true);
                }
                else {
                    $("#selectedParticipantor").find("input[type=checkbox]").attr("checked", false);
                }
            });
        });

        //树节点单击获取部门下的人员
        function onNodeClick(id1, id2) {
            var type = $("#" + id1).attr("tag");
            $.post(getCurrentUrl(), { AjaxAction: "GetOrgOrRoleUsers", AjaxArgument: id1, Type: type }, function (value) {
                var ajaxResult = JSON2.parse(value);
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null && ajaxResult.PromptMsg != "") {
                        var retValue = ajaxResult.RetValue
                        if (retValue) {
                            var strHtml = "";
                            for (var i = 0; i < retValue.length; i++) {
                                strHtml += "<option value=\"" + retValue[i].ID + "\">" + retValue[i].Name + "</option>";
                            }
                            $("#selects").empty();
                            if (strHtml != "")
                                $("#selects").append(strHtml);
                        }
                        if (window.parent)
                            window.parent.closeDialog();
                    }
                }
                else {
                    alert("系统出错，请与管理员联系！");
                }
            });
        }

        //树节点双击选取部门或角色
        function onNodeDblclick(id1, id2) {
            if (id1 == "role") return;
            var me = $("#" + id1);
            var tag = $(me).attr("tag");
            var typeName = { Person: "人员", Role: "角色", Org: "组织" };
            var type = typeName[tag]; //获取类型名称
            var title = $(me).attr("title"); //名称
            createHtml(id1, title, type);
        }

        //为gridview添加一行
        function createHtml(id, name, type) {
            var tbody = $("#selectedParticipantor").find("tbody");
            var index = $(tbody).find("tr").length + 1; //序号
            if ($(tbody).find("tr").find("input[value='" + id + "']").length > 0) //存在的即返回
                return false;
            var strHtml = "<tr style=\"cursor: hand\">"
					        + "<td align=\"left\">"
                                + "<input id=\"radioId\" type=\"checkbox\" value='" + id + "' name=\"radioId\" />"
                            + "</td><td>" + index + "</td>"
                            + "<td>" + name + "</td>"
                            + "<td>" + type + "</td>"
                            + "<td><span ondblclick=\"delItem(this)\" title=\"双击进行操作\">删除</span></td>"
				         + "</tr>";
            $(tbody).prepend(strHtml);
            return true;
        }

        //选取单个人员
        function chooseItem(me) {
            var selectedItem = $("option:selected", me);
            var type = "人员";
            var id = $(selectedItem).val();
            var name = $(selectedItem).text();
            if (createHtml(id, name, type))
                $(selectedItem).remove();
        }

        //删除已选取的单个人员
        function delItem(me) {
            $(me).parent().parent().remove();
            changIndex();
        }

        //改变序号
        function changIndex() {
            var trs = $("#selectedParticipantor").find("tbody").find("tr");
            var i = $(trs).length;
            if (i == 0) return;
            $(trs).each(function () {
                $("td", this).slice(1, 2).text(i);
                i--;
            });
        }

        //选取多个人员
        function chooseItems() {
            var type = "人员";
            var id = "";
            var name = "";
            $("#selects").find("option:selected").each(function () {
                id = $(this).val();
                name = $(this).text();
                if (createHtml(id, name, type))
                    $(this).remove();
            });
        }

        //删除多个已选取的人员
        function delItems() {
            var tbody = $("#selectedParticipantor").find("tbody");
            $(tbody).find("input[checked]").each(function () {
                var me = $(this).parent().parent();
                var tds = $("td", me);
                var id = $(this).val();
                var name = $(tds).slice(2, 3).text();
                if ($(tds).slice(3, 4).text() == "人员")
                    $("#selects").append("<option value=\"" + id + "\">" + name + "</option>")
                $(this).parent().parent().remove();
            });
            $("#chkAll").removeAttr("checked");
            changIndex();
        }

        //获取选择的人员的ID
        function getSelectedItems() {
            var participantors = new Array();
            var selectItems = "";
            $("#selectedParticipantor").find("tbody").find("input").each(function () {
                var me = $(this).parent().parent();
                var tds = $("td", me);
                var id = $(this).val();
                var name = $(tds).slice(2, 3).text();
                var type = $(tds).slice(3, 4).text();
                var item = { id: id, name: name, type: type };

                participantors.push(item);
            });
            return participantors;
        }

        //确定
        function chooseConfirm() {
            var items = getSelectedItems();
            window.returnValue = items;
            window.close();
        }
    </script>
    <div style="width: 40%; float: left; height: 100%;">
        <agile:AjaxTree ID="AjaxTree1" Runat="server" />
    </div>
    <div style="float: right; width: 60%;">
        <div id="resourceContainer" style="height: 50%; margin: 0 auto;">
            待选人员
            <select id="selects" ondblclick="javascript:chooseItem(this)" multiple="multiple"
                style="width: 100%; height: 200px;">
            </select>
        </div>
        <div style="width: 100%; height: 5%; overflow: hidden; float: left;">
            <ul class="commandbar">
                <li class="commanditem_first"></li>
                <li onclick="chooseItems();" onmouseover="CommandItemOver(this)" onmouseout="CommandItemOut(this)"
                    class="commanditem" id="cmdChoose" style="cursor: pointer;">选择</li>
                <li onclick="delItems();" onmouseover="CommandItemOver(this)" onmouseout="CommandItemOut(this)"
                    class="commanditem" id="cmdDel" style="cursor: pointer;">删除</li><li class="commanditem_last">
                    </li>
            </ul>
        </div>
        已选人员
        <table id="selectedParticipantor" style="width: 100%" cellspacing="0" rules="all"
            border="1">
            <thead>
                <tr>
                    <th nowrap="nowrap" scope="col">
                        <input id="chkAll" type="checkbox" name="chkAll" style="margin-left: 1px;" />选择
                    </th>
                    <th nowrap="nowrap" scope="col">
                        序号
                    </th>
                    <th nowrap="nowrap" scope="col">
                        名称
                    </th>
                    <th nowrap="nowrap" scope="col">
                        类型
                    </th>
                    <th nowrap="nowrap" scope="col">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
</asp:Content>
