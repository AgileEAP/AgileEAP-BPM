<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="RoleManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.RoleManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">


        function add(me, argument) {
            var tblParameter = $("#divContent").find("table").eq(0);
            var rowCount = $("#<%=gvList.ClientID %>").find("tr").length;

            $(":radio").removeAttr("checked");
            var newRow = "<tr ondblclick=\"rowDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                          + "<td  style='width:10%;'><input id='radioId' type='radio' value='' checked='checked' name='radioId' /></td><td  style='width:10%;'><input type='text' style='width:98%'/></td>"
                          + "<td  style='width:20%;'><%=GetAppsDDl() %></td><td  style='width:20%;'><input type='text' style='width:98%'/></td>"
                          + "<td  style='width:20%;'><%=User.LoginName %></td><td style='width:20%;'><%=DateTime.Now.ToShortDateString() %></td>" +
                            "</tr>";
            tblParameter.append(newRow);
            $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) + 1);
        }

        function save(me, argument) {

            var currentRow = $("#divContent").find(":checked ").first().parent().parent();
            var tds = $("td", currentRow);
            tds.each(function (i) {
                if (i > 0) {
                    if (i == 2) {
                        var appText = $(this).find("select").eq(0);
                        if (appText[0]) {
                            $(this).text(appText.find("option:selected").text());
                            $(this).append("<input type='hidden' value='" + appText.val() + "'/>");
                        }
                    }
                    else {

                        var text = $(this).find("input").eq(0);
                        if (text[0]) {
                            var innerText = $.trim(text.val());
                            $(this).empty();
                            $(this).text(innerText);
                        }
                    }
                }
            });

            var parameter = new Object();
            var values = currentRow.find("td");
            var ID = $("#divContent").find(":checked ").first().val();
            if (values.length > 0) {
                parameter["ID"] = ID;
                parameter["Name"] = $.trim(values.eq(1).text());
                parameter["AppID"] = $.trim(values.eq(2).find("input").eq(0).val());
                parameter["Description"] = $.trim(values.eq(3).text());
                if (parameter["Name"] == "") {
                    alert("【角色名称】不能为空");
                    return;
                }
                if (parameter["Description"] == "") {
                    alert("【描述】不能为空");
                    return;
                }
            }




            var value = JSON2.stringify(parameter)

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, CurrentId: ID }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                    }
                    $("#divContent").find(":checked ").first().val(ajaxResult.RetValue);
                }

                alert(message);
            });
        }

        function del(me, argument) {
            var rad = $("#divContent").find(":checked ").first(); //.val();
            var id = rad.val();
            if ($.trim(id) == "") {
                rad.parent().parent().remove();
                alert("操作成功");
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
                        rad.parent().parent().remove();
                        $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) - 1);
                    }
                }
                alert(message);
            });
        }


        function rowOut(me) {
            if (oldRow != null) {
                if (currentRow != me) {
                    me.className = "rowout";
                }
                oldRow = me;
            }
        }
        function rowClick(me, id, remember) {
            if (currentRow) {
                currentRow.className = "gridview_row";
            }
            me.className = "rowcurrent";

            if (currentRow && currentRow != me) {
                var tds = $("td", currentRow);
                tds.each(function (i) {

                    if (i == 2) {
                        var appText = $(this).find("select").eq(0);
                        if (appText[0]) {
                            $(this).text(appText.find("option:selected").text());
                            $(this).append("<input type='hidden' value='" + appText.val() + "'/>");
                        }
                    }
                    else {
                        var text = $(this).find("input").eq(0);
                        if (text[0] && text.attr("type") != "radio") {
                            var innerText = $.trim(text.val());
                            $(this).empty();
                            $(this).text(innerText);
                        }
                    }
                });
                // $(currentRow).find("input:checked").first().attr("checked", false);
            }
            currentRow = me;
            // $(currentRow).find("input:checked").first().attr("checked", true);
            $("input:radio", me).attr("checked", true);
        }

        cacheArray = new Array();
        function rowDbClick(me, id, remember) {
            $("td", me).each(function (i) {
                var flag = $(this).find("span");
                if (!flag[0] && i > 0 && i < 4) {
                    var innerText = $.trim($(this).text());

                    if (i == 2) {
                        var text = $(this).find("input").eq(0);
                        $(this).text("");
                        $(this).append("<%=GetAppsDDl() %>");
                        var appText = $(this).find("select").eq(0);
                        appText.val(text.val());
                    }
                    else {
                        $(this).text("");

                        $(this).append("<input type='text' style='width:98%' class='gridview_row' value='" + innerText + "' />");
                        cacheArray[i] = innerText;
                    }
                }
            });
        }

        function onAppSelect(me) {
            var appText = $(me);
            var text = $(me).parent().find("input").eq(0);
            if (text[0]) {
                text.val(appText.val());
            }
            else {
                $(me).parent().append("<input type='hidden' value='" + appText.val() + "'/>");
            }
        }

        function setPrivilege(me, argument) {
            var rad = $("#divContent").find(":checked ").first().val();
            if (rad == null || rad == "") {
                alert("请先选择设置对象");
                return false;
            }
            window.parent.openOperateDialog("设置权限", "AuthorizeCenter/RolePrivilegeManager.aspx?RoleID=" + rad, 500, 500);
        }

        function setPrivilegeMeta(me, argument) {
            var rad = $("#divContent").find(":checked ").first().val();
            if (rad == null || rad == "") {
                alert("请先选择设置对象");
                return false;
            }
            window.parent.openOperateDialog("设置数据权限", argument+"?RoleID=" + rad, 500, 500);
            //window.parent.openOperateDialog("设置数据权限", "AuthorizeCenter/MetaDataPermission.aspx?RoleID=" + rad, 500, 500);
        }


        function setVMMeta(me, argument) {
            var rad = $("#divContent").find(":checked ").first().val();
            if (rad == null || rad == "") {
                alert("请先选择设置对象");
                return false;
            }
            window.parent.openOperateDialog("设置虚拟机分配权限", argument+"?RoleID=" + rad, 500, 500);
           // window.parent.openOperateDialog("设置虚拟机分配权限", "AuthorizeCenter/MyVMPermission.aspx?RoleID=" + rad, 500, 500);
        }
        //关闭窗口
        function closeDialog(fieldTypeId) {
            $("#actionDialog").dialog("close");
        }

    </script>
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="角色名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用ID">
                    <ItemTemplate>
                        <input type="hidden" value='<%#Eval("AppID") %>' />
                        <%# GetAppNameById(Eval("AppID"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="描述">
                    <ItemTemplate>
                        <%#Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建者">
                    <ItemTemplate>
                        <%#GetUserNameByUserId((string)Eval("Creator"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("CreateTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </agile:PagedGridView>
    </div>
    <div id="filterDialog" title="查询">
        <p id="validateTips">
        </p>
        <fieldset>
            <dl>
                <dt class="rowlable">角色名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">应用ID</dt>
                <dd class="rowinput">
                    <agile:ChooseBox ID="filterAppID" OpenUrl="AppIDTree.aspx" DialogTitle="选择应用ID"
                        runat="server"></agile:ChooseBox>
                </dd>
                <dt class="rowlable">描述</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterDescription" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">归属组织</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterOwnerOrg" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">创建者</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterCreator" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">创建时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterCreateTime" runat="server"></agile:DatePicker>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
