<%@ page language="C#" masterpagefile="../Master/Page.Master" autoeventwireup="true"
    codebehind="ResourceDetail.aspx.cs" inherits="AgileEAP.Plugin.Authorize.ResourceDetail" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
        <%=HtmlExtensions.RequireScripts("gridview") %>
        <%=HtmlExtensions.RequireScripts("jqueryFormValidator")%>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
        #tblOperate td {
            text-align: left;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">

        //校验脚本
        function initValidator() {
            $.formValidator.initConfig({ formid: "aspnetForm", showallerror: true, onerror: function (msg) { alert(msg); } });
            /*非空字段*/
            $("#<%=txtName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【资源名称】不能为空" });
            /*非空字段*/
            $("#<%=txtText.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【资源显示名】不能为空" });
            $("#<%=txtSortOrder.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【显示序名】不能为空" });
            /*必选项*/
            $("#<%=chbParentID.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【父资源】不能为空" });
            /*默认验证字段*/
            $("#<%=txtEntry.ClientID%>").formValidator().inputValidator({ max: 64, onerror: "【冗余字段】长度不能超过64" });
            /*默认验证字段*/
            $("#<%=txtArgument.ClientID%>").formValidator().inputValidator({ max: 128, onerror: "【冗余字段】长度不能超过128" });
            /*默认验证字段*/
            $("#<%=txtURL.ClientID%>").formValidator().inputValidator({ max: 128, onerror: "【URL长度不能超过128" });
            /*默认验证字段*/
            $("#<%=txtIcon.ClientID%>").formValidator().inputValidator({ max: 128, onerror: "【闭合图标】长度不能超过128" });
            /*默认验证字段*/
            $("#<%=txtExpandIcon.ClientID%>").formValidator().inputValidator({ max: 128, onerror: "【展开图标】长度不能超过128" });
        }

        //页面初始化
        $(document).ready(function () {
            //初始化校验脚本
            initValidator();
        });

        function addOperate() {
            var tblOperate = $("#tblOperate");
            var rowCount = $("#tblOperate").find("tr").length;

            var newRow = "<tr ondblclick=\"rowDbClick(this,'')\" onmouseout='rowOut(this);' onmouseover='rowOver(this);' onclick=\"rowClick(this,'')\" style='cursor: hand' id=''>"
                          + "<td>" + rowCount + "</td><td>操作名</td><td>命令</td><td>参数</td><td flag='combox'>弹出</td>" +
                            "<td flag='combox'>否</td><td flag='operate'><span class='btn_delete' title='删除' onclick=\"executeOperate(this,'delete')\">&nbsp;&nbsp;&nbsp;</span> ｜ <span class='btn_up' title='上移' onclick=\"executeOperate(this,'up')\">&nbsp;&nbsp;&nbsp;</span> ｜ <span class='btn_down' title='下移' onclick=\"executeOperate(this,'down')\">&nbsp;&nbsp;&nbsp;</span></td></tr>";
            tblOperate.append(newRow);
        }

        function swapRow(srcRow, destRow) {
            if (srcRow[0] && destRow[0]) {
                var destCells = destRow.find("td[flag!='operate']");
                srcRow.find("td[flag!='operate']").each(function (i) {
                    if (i > 0) {
                        var tmp = $.trim($(this).text());
                        $(this).text($.trim(destCells.eq(i).text()));
                        destCells.eq(i).text(tmp);
                    }
                });
            }
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

        function rowOut(me) {
            if (oldRow != null) {
                if (currentRow != me) {
                    me.className = "rowout";
                }
                oldRow = me;
            }
        }

        function rowClick(me, id) {
            if (currentRow) {
                currentRow.className = "gridview_row";
            }
            me.className = "rowcurrent";

            if (currentRow && currentRow != me) {
                $(currentRow).find("td").each(function (i) {
                    var text = $(":input", $(this)).eq(0);
                    if (text[0]) {
                        var innerText = text.val();
                        $(this).empty();
                        $(this).text(innerText);
                    }
                });
            }
            currentRow = me;
        }

        function rowDbClick(me, id) {
            if ($(":input", $(me)).eq(0)[0]) return;

            $("td", me).each(function (i) {
                var flag = $(this).attr("flag");
                if (i > 0 && flag != "operate" && flag != "combox") {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' value='" + innerText + "' style='width:100px;'/>");
                }
                if (i == 4)//运行方式
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<select style='width:100px;'><option " + (innerText == "弹出" ? "selected='selected'" : "") + " >弹出</option><option " + (innerText == "Post" ? "selected='selected'" : "") + ">Post</option><option " + (innerText == "Ajax" ? "selected='selected'" : "") + ">Ajax</option></select> ");
                }
                else if (i == 5)//是否验证
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<select style='width:100px;'><option " + (innerText == "是" ? "selected='selected'" : "") + " >是</option><option " + (innerText == "否" ? "selected='selected'" : "") + ">否</option></select>");
                }
            });
        }

        function save(me, argument) {
            if (!$.formValidator.pageIsValid("1")) return false;

            var tblOperate = $("#tblOperate");
            var operateItems = new Array();

            $("tr", tblOperate).each(function (i) {
                if (i > 0) {
                    var item = new Object();
                    var tds = $(this).find("td");
                    tds.each(function (i) {
                        var text = $(":input", $(this)).eq(0);
                        if (text[0]) {
                            var innerText = text.val();
                            $(this).empty();
                            $(this).text(innerText);
                        }
                    });

                    if (tds.length > 0) {
                        item["SortOrder"] = parseInt($.trim(tds.eq(0).text()));
                        item["OperateName"] = $.trim(tds.eq(1).text());
                        item["CommandName"] = $.trim(tds.eq(2).text());
                        item["CommandArgument"] = $.trim(tds.eq(3).text());
                        item["ID"] = $.trim($(this).attr("id"));
                        var runat = $.trim(tds.eq(4).text());
                        if (runat == "Ajax")
                            item["Runat"] = 1;
                        else if (runat == "Post")
                            item["Runat"] = 2;
                        else
                            item["Runat"] = 3;
                        item["IsVerify"] = $.trim(tds.eq(5).text()) == "是" ? 1 : 0;
                        operateItems[i - 1] = item;
                    }
                }
            });

            var resource = getObjectValue("resourceDetail");
            resource.Operates = operateItems;
            var value = JSON2.stringify(resource)

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, CurrentId: $("#hidCurrentId").val() }, function (result) {
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
                            frm.addNewAjaxTreeNode(parentId, ajaxResult.RetValue, $("#<%=txtText.ClientID%>").val(), $("#cboTypedata").val());
                        }
                    }

                    $("#hidCurrentId").val(ajaxResult.RetValue);
                }

                alert(message);
            });
        }
    </script>
    <div id="resourceDetail" class="div_block">
        <div class="div_row">
            <div class="block_title">
                资源明细</div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtName.ClientID%>" class="label">
                    <em>*</em> 资源名称
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtName" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtText.ClientID%>" class="label">
                    <em>*</em> 资源显示名
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtText" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=cboType.ClientID%>" class="label">
                    资源类型
                </label>
            </div>
            <div class="div_row_input">
                <agile:Combox ID="cboType" runat="server"></agile:Combox>
            </div>
            <div class="div_row_lable">
                <label for="<%=chbParentID.ClientID%>" class="label">
                    父资源
                </label>
            </div>
            <div class="div_row_input">
                <agile:ChooseBox ID="chbParentID" OpenUrl="ChooseParentResource.aspx?Entry=Choose"
                    DialogTitle="选择父资源" runat="server">
                </agile:ChooseBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtEntry.ClientID%>" class="label">
                    调用入口
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtEntry" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtArgument.ClientID%>" class="label">
                    URL参数
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtArgument" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtURL.ClientID%>" class="label">
                    URL
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtURL" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtSortOrder.ClientID%>" class="label">
                    <em>*</em>序号
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="text" Text="1"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtIcon.ClientID%>" class="label">
                    闭合图标
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtIcon" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtExpandIcon.ClientID%>" class="label">
                    展开图标
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtExpandIcon" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=cboOpenMode.ClientID%>" class="label">
                    页面打开模式
                </label>
            </div>
            <div class="div_row_input">
                <agile:Combox ID="cboOpenMode" runat="server"></agile:Combox>
            </div>
            <div class="div_row_lable">
                <label for="<%=cboShowNavigation.ClientID%>" class="label">
                    是否显示导航
                </label>
            </div>
            <div class="div_row_input">
                <agile:Combox ID="cboShowNavigation" runat="server"></agile:Combox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=cboShowToolBar.ClientID%>" class="label">
                    是否显示工具栏
                </label>
            </div>
            <div class="div_row_input">
                <agile:Combox ID="cboShowToolBar" runat="server"></agile:Combox>
            </div>
 <%--           <div class="div_row_lable">
                <label for="<%=chbOwnerOrg.ClientID%>" class="label">
                    <em>*</em> 归属组织
                </label>
            </div>
            <div class="div_row_input">
                <agile:ChooseBox ID="chbOwnerOrg" OpenUrl="OrgTree.aspx?Entry=Choose" DialogWidth="600"
                    DialogTitle="选择所属组织" runat="server">
                </agile:ChooseBox>
            </div>--%>
        </div>
   <%--     <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=chbAppID.ClientID%>" class="label">
                    <em>*</em> 所属应用
                </label>
            </div>
            <div class="div_row_input">
                <agile:ChooseBox ID="chbAppID" OpenUrl="../Infrastructure/AppManager.aspx?Entry=Choose"
                    DialogWidth="600" DialogTitle="选择所属应用" runat="server">
                </agile:ChooseBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=chbModuleID.ClientID%>" class="label">
                    <em>*</em> 所属模块
                </label>
            </div>
            <div class="div_row_input">
                <agile:ChooseBox ID="chbModuleID" OpenUrl="../Infrastructure/ModuleManager.aspx?Entry=Choose"
                    DialogWidth="600" DialogTitle="选择所属模块" runat="server">
                </agile:ChooseBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtDescription.ClientID%>" class="label">
                    描述
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtDescription" runat="server" Width="80%" CssClass="text"></asp:TextBox>
            </div>
        </div>--%>
        <div class="div_row">
            <div class="block_title">
                操作项</div>
        </div>
        <div style="float: left; width: 100%; text-align: center;">
            <table id="tblOperate" class="gridview" border="1" style="width: 95%; display: inline-table; word-wrap: normal; word-break: keep-all" >
                <thead>
                    <tr>
                        <th style="width: 5%">
                            序号
                        </th>
                        <th style="width: 20%">
                            名称
                        </th>
                        <th style="width: 20%">
                            命令
                        </th>
                       <th style="width: 25%">
                            参数
                        </th>
                        <th style="width: 5%" flag="combox">
                            运行
                        </th>
                        <th style="width: 5%" flag="combox">
                            验证
                        </th>
                        <th style="width: 20%">
                            <span onclick="addOperate();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptOperate" runat="server">
                    <ItemTemplate>
                        <tr ondblclick="rowDbClick(this,'')" onmouseout="rowOut(this);" onmouseover="rowOver(this);"
                            onclick="rowClick(this,'')" style="cursor: pointer" id='<%#Eval("ID") %>'>
                            <td>
                                <%#Eval("SortOrder")%>
                            </td>
                            <td>
                                <%#Eval("OperateName")%>
                            </td>
                            <td>
                                <%#Eval("CommandName")%>
                            </td>
                            <td>
                                <%#Eval("CommandArgument")%>
                            </td>
                            <td flag="combox">
                                <%# RemarkAttribute.GetEnumRemark((Runat)Enum.Parse(typeof(Runat), Eval("Runat").ToString()))%>
                            </td>

                            <td flag="combox">
                                <%#Eval("IsVerify").ToString()=="1"?"是":"否"%>
                            </td>
                            <td flag="operate">
                                <span class='btn_delete' onclick="executeOperate(this,'delete')">&nbsp;&nbsp;&nbsp;</span>
                                ｜ <span class="btn_up" title='上移' onclick="executeOperate(this,'up')">&nbsp;&nbsp;&nbsp;</span>
                                ｜ <span class="btn_down" title='下移' onclick="executeOperate(this,'down')">&nbsp;&nbsp;&nbsp;</span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:content>
