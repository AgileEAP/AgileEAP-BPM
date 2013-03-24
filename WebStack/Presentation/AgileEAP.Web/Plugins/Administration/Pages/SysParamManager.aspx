<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="SysParamManager.aspx.cs" Inherits="AgileEAP.Administration.SysParamManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function add(me, argument) {
            var tblParameter = $("#divContent").find("table").eq(0);
            var rowCount = $("#<%=gvList.ClientID %>").find("tr").length;

            var newRow = "<tr ondblclick=\"rowDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                          + "<td  style='width:20%;'><input type='text' style='width:98%'/></td><td  style='width:35%;'><input type='text' style='width:98%'/></td><td style='width:20%;'><input type='text' style='width:98%'/></td>" +
                            "<td flag='operate'  style='width:15%;'><span class='btn_delete' title='删除' onclick=\"del(this)\">&nbsp;&nbsp;&nbsp;</span> | <span onclick=\"save(this)\"" +
                               " class='btn_save' title='保存'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><input type='hidden' value=''/> </td></tr>";
            tblParameter.append(newRow);
            $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) + 1);
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
                    if (ajaxResult.Result== 1) {
                        if (message == "")
                            message = "操作成功！";
                        $(me).parent().parent().remove();
                        $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) - 1);
                    }
                }
                alert(message);
            });
        }
        function save(me) {
            var currentRow = $(me).parent().parent();
            var tds = $("td", currentRow);
            tds.each(function (i) {
                var text = $(this).find("input").eq(0);
                if (text[0] && text.attr("type") != "hidden") {
                    var innerText = $.trim(text.val());
                    $(this).empty();
                    $(this).text(innerText);
                }
            });
            var id = $(me).parent().find("input").eq(0).val();
            var parameter = new Object();
            var values = currentRow.find("td")
            if (values.length > 0) {
                parameter["Name"] = $.trim(values.eq(0).text());
                parameter["Value"] = $.trim(values.eq(1).text());
                parameter["Description"] = $.trim(values.eq(2).text());
                parameter["ID"] = id;
            }

            if (parameter["Name"] == "") {
                alert("参数名称不能为空");
                return;
            }
            if (parameter["Value"] == "") {
                alert("参数值不能为空");
                return;
            }
            var value = JSON2.stringify(parameter)

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, CurrentId: id }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result== 1) {
                        if (message == "")
                            message = "操作成功！";
                    }
                    $(me).parent().find("input").eq(0).val(ajaxResult.RetValue);
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
                if (!flag[0]) {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' style='width:98%' class='gridview_row' value='" + innerText + "' />");
                    cacheArray[i] = innerText;
                }
            });
        }
    </script>
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="参数名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                    <ItemStyle Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="参数值">
                    <ItemTemplate>
                        <%#Eval("Value")%>
                    </ItemTemplate>
                    <ItemStyle Width="35%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="描述">
                    <ItemTemplate>
                        <%#Eval("Description")%>
                    </ItemTemplate>
                    <ItemStyle Width="30%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <span style=" padding-right: 16px" class='btn_delete' onclick="del(this)" title='删除'></span>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<span onclick="save(this);"
                            class='btn_save' title='保存'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <input type="hidden" value="<%#Eval("ID")%>" />
                    </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateField>
            </Columns>
        </agile:PagedGridView>
    </div>
    <div id="filterDialog" title="查询">
        <p id="validateTips">
        </p>
        <fieldset>
            <dl>
                <dt class="rowlable">参数名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">参数值</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterValue" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">描述</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterDescription" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                
            </dl>
        </fieldset>
    </div>
</asp:Content>
