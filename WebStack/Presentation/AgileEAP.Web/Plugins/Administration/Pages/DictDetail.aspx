<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="DictDetail.aspx.cs" Inherits="AgileEAP.Administration.DictDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=HtmlExtensions.RequireScripts("gridview")%>
    <%=HtmlExtensions.RequireScripts("jqueryFormValidator")%>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">

        //校验脚本
        function initValidator() {
            $.formValidator.initConfig({ formid: "aspnetForm", showallerror: true, onerror: function (msg) { promptMessage(msg); } });
            /*非空字段*/
            $("#<%=txtName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【字典名】不能为空" });
            /*非空字段*/
            $("#<%=txtText.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【字典显示名】不能为空" });
            /*必选项*/
            $("#<%=chbParentID.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【父字典】不能为空" });
            /*默认验证字段*/
            $("#<%=txtDescription.ClientID%>").formValidator().inputValidator({ max: 128, onerror: "【描述】长度不能超过128" });

        }

        //页面初始化
        $(document).ready(function () {
            //初始化校验脚本
            initValidator();
        });
        function addDict() {
            var tblDict = $("#tblDict");
            var rowCount = $("#tblDict").find("tr").length;

            var newRow = "<tr ondblclick=\"rowDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                          + "<td style='width:10%'>" + rowCount + "</td>" + "<td  style='width:20%;'><input type='text' style='width:98%'/></td><td  style='width:25%;'><input type='text' style='width:98%'/></td><td style='width:25%;'><input type='text' style='width:98%'/></td>" +
                            "<td flag='operate' style='width:20%'><span class='btn_delete' title='删除' onclick=\"del(this)\">&nbsp;&nbsp;&nbsp;</span><input type='hidden' value=''/> </td></tr>";
            tblDict.append(newRow);
        }

        function del(me) {
            var id = $(me).parent().find("input").eq(0).val();
            if (id == "") {
                $(me).parent().parent().remove();
                return;
            }
            $.post(getCurrentUrl(), { AjaxAction: "Delete", AjaxArgument: id }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                        $(me).parent().parent().remove();
                    }
                }
                alert(message);
            });
        }

        function executeOperate(me, operate) {
            var currentRow = $(me).parent().parent();
            if (operate == "delete") {
                currentRow.remove();
            } else if (operate == "up") {
                var prev = currentRow.prev();
                swapRow(prev, currentRow);
            } else if (operate == "down") {
                var next = currentRow.next();
                swapRow(next, currentRow);
            }

            return false;
        }

        function rowClick(me, id, remember) {
            if (currentRow) {
                currentRow.className = "gridview_row";
            }
            me.className = "rowcurrent";

            if (currentRow && currentRow != me) {
                var tds = $("td", currentRow);
                tds.each(function (i) {
                    var text = $(this).find("input").eq(0);
                    if (text[0] && text.attr("type") != "hidden") {
                        var innerText = $.trim(text.val());
                        $(this).empty();
                        $(this).text(innerText);
                    }
                });
            }
            currentRow = me;
        }

        cacheArray = new Array();
        function rowDbClick(me, id, remember) {
            if ($("input[type!=hidden]", me).eq(0)[0]) return;
            $("td", me).each(function (i) {
                var flag = $(this).find("span");
                if (i > 0 && !flag[0]) {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' style='width:98%' class='gridview_row' value='" + innerText + "' />");
                    cacheArray[i] = innerText;
                }
            });
        }

        function save(me, argument) {

            if (!$.formValidator.pageIsValid("1")) return false;

            var tblDict = $("#tblDict");
            var dictItems = new Array();


            $("tr", tblDict).each(function (i) {
                if (i > 0) {
                    var dictItem = new Object();
                    var tds = $(this).find("td")
                    if (tds.length > 0) {
                        tds.each(function (i) {
                            var text = $("input", $(this)).eq(0);
                            if (text[0] && text.attr("type") != "hidden") {
                                var innerText = text.val();
                                $(this).empty();
                                $(this).text(innerText);
                            }
                        });
                    }


                    dictItem["SortOrder"] = parseInt($.trim(tds.eq(0).text()));
                    dictItem["Value"] = $.trim(tds.eq(1).text());
                    dictItem["Text"] = $.trim(tds.eq(2).text());
                    dictItem["Description"] = $.trim(tds.eq(3).text());
                    dictItem["ID"] = $.trim(tds.find("input").last().val());

                    dictItems[i - 1] = dictItem;
                }
            });

            var dict = getObjectValue("dictDetail");
            dict.DictItems = dictItems;

            for (var i = 0; i < dictItems.length; i++) {
                if (dictItems[i]["Value"] == "") {
                    alert("字典项值不能为空");
                    return;
                }
                if (dictItems[i]["Text"] == "") {
                    alert("字典项显示值不能为空");
                    return;
                }
            }
            dict.ID = $("#hidCurrentId").val();
            var value = JSON2.stringify(dict);

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, dictId: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                        var parentId = $("#chbParentIDdata").val();
                        if (parentId != "") {
                            var frm = window.parent.frames["ifrTree"];
                            frm.addNewAjaxTreeNode(parentId, ajaxResult.RetValue.ID, $("#<%=txtText.ClientID%>").val(), "childDict");
                        }
                        //$(me).parent().find("input").eq(0).val(ajaxResult.RetValue);
                    }

                    $("#hidCurrentId").val(ajaxResult.RetValue.ID);
                    var j = 0;
                    $("#tblDict").find("tr").each(function (j) {
                        if (j > 0) {
                            $("input", this).val(ajaxResult.RetValue.DictIds[j - 1]);
                        }
                    });
                }

                alert(message);
            });
        }
    </script>
    <div id="dictDetail" class="div_block">
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtName.ClientID%>" class="label">
                    <em>*</em>字典名
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtName" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtText.ClientID%>" class="label">
                    <em>*</em>字典显示名
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtText" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=chbParentID.ClientID%>" class="label">
                    <em>*</em>父字典
                </label>
            </div>
            <div class="div_row_input">
                <agile:choosebox id="chbParentID" openurl="ParentIDTree.aspx" dialogtitle="选择父字典"
                    runat="server"></agile:choosebox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtSortOrder.ClientID%>" class="label">
                    序号
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtDescription.ClientID%>" class="label">
                    描述
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtDescription" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="block_title">
                操作项
            </div>
        </div>
        <div style="float: left; width: 100%; text-align: center;">
            <table id="tblDict" style="width: 100%; display: inline-table;">
                <thead>
                    <tr>
                        <th style="width: 10%">序号 </th>
                        <th style="width: 20%">字典项值 </th>
                        <th style="width: 25%">字典项显示值 </th>
                        <th style="width: 25%">描述 </th>
                        <th style="width: 20%"><span onclick="addDict();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </span></th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptDict" runat="server">
                    <ItemTemplate>
                        <tr ondblclick="rowDbClick(this,'',true)" onmouseout="rowOut(this);" onmouseover="rowOver(this);"
                            onclick="rowClick(this,'',true)" style="cursor: pointer">
                            <td>
                                <%#Eval("SortOrder")%>
                            </td>
                            <td>
                                <%#Eval("Value")%>
                            </td>
                            <td>
                                <%#Eval("Text")%>
                            </td>
                            <td>
                                <%#Eval("Description")%>
                            </td>
                            <td flag="operate"><span class='btn_delete' onclick="del(this)" title="删除">&nbsp;&nbsp;&nbsp;
                            </span>
                                <input type="hidden" value='<%#Eval("ID") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
