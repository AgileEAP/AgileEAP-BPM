<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Master/Page.Master"
    CodeBehind="OrgUserList.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.OrgUserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript">
        function ShowDetail(me) {
            var tr = $(me).parent();
            var id = $("#id", tr).val();
            if (id == "" || id == null) {
                alert("请选择用户！");
                return false;
            }
            window.parent.parent.openOperateDialog("查看详细信息", "AuthorizeCenter/UserDetail.aspx?CurrentID=" + id + "&m=" + Math.random(), 600, 450);
        }

        //关闭窗口
        function closeDialog(fieldTypeId) {
            $("#actionDialog").dialog("close");
        }

        function rowDbClick(me, id, remember) {
            var id = $("#id", me).val();
            if (id == "" || id == null) {
                alert("请选择用户！");
                return false;
            }
            window.parent.openOperateDialog("查看详细信息", "AuthorizeCenter/UserDetail.aspx?CurrentId=" + id + "&Entry=View&m=" + Math.random(), 600, 450);
        }

        function add(me, cmd) {
            window.parent.parent.openOperateDialog("新增用户", "AuthorizeCenter/UserDetail.aspx?ActionFlag=Add&OrgID=" + $.query.get("orgid"), 700, 500);
        }

        function update(me, cmd) {
            var id = $(":checked ").first().attr("value");
            if (id == "" || id == null) {
                alert("请选择用户！");
                return false;
            }
            window.parent.parent.openOperateDialog("更新用户", "AuthorizeCenter/UserDetail.aspx?ActionFlag=Update&CurrentId=" + id, 700, 500);
        }


        function setRole(me, argument) {
            var id = $("#divContent").find(":checked ").first().val();
            if (id == "" || id == null) {
                alert("请选择用户！");
                return false;
            }
            window.parent.parent.openOperateDialog("设置用户角色", "AuthorizeCenter/ObjectRoleManager.aspx?UserID=" + id + "&m=" + Math.random(), 500, 500);
        }

        function setSpecialPrivilege(me, argument) {
            var id = $("#divContent").find(":checked ").first().val();
            if (id == "" || id == null) {
                alert("请选择用户！");
                return false;
            }
            window.parent.parent.openOperateDialog("设置用户特殊权限", "AuthorizeCenter/SpecialPrivilegeManager.aspx?UserID=" + id + "&m=" + Math.random(), 500, 500);
        }

        function activation(me, argument) {
            var rad = $("#divContent").find(":checked ").first(); //.val();
            var id = rad.val();
            if ($.trim(id) == "") {
                alert("请选择用户");
                return;
            }

            $.post(getCurrentUrl(), { AjaxAction: "Activation", AjaxArgument: id }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                    }
                }
                alert(message);
                if (ajaxResult.Result == 1) {
                    window.parent.location.reload();
                }
            });
        }
        function freeze(me, argument) {
            var rad = $("#divContent").find(":checked ").first(); //.val();
            var id = rad.val();
            if ($.trim(id) == "") {
                alert("请选择用户");
                return;
            }

            $.post(getCurrentUrl(), { AjaxAction: "Frezze", AjaxArgument: id }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                    }
                }
                alert(message);
                if (ajaxResult.Result == 1) {
                    window.parent.location.reload();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
        <agile:pagedgridview id="gvList" runat="server" pageindex="1" cssclass="gridview"
            datakeynames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="id" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="登陆名">
                    <ItemTemplate>
                        <%#Eval("LoginName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="人员姓名">
                    <ItemTemplate>
                        <%#Eval("UserName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <%#Eval("Status").ToSafeString().Cast<UserStatus>().GetRemark()%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("CreateTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </agile:pagedgridview>
    </div>
    <div id="filterDialog" title="查询">
        <p id="validateTips">
        </p>
        <fieldset>
            <dl>
                <dt class="rowlable">登陆名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterLoginName" runat="server"></asp:TextBox>
                </dd>
                <dt class="rowlable">姓名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterUserName" runat="server"></asp:TextBox>
                </dd>
                <dt class="rowlable">状态</dt>
                <dd class="rowinput">
                    <agile:combobox id="filterStatus" dropdownstyle="DropDownList" appenddatabounditems="true"
                        runat="server">
                        <asp:ListItem Text="--请选择--" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="冻结" Value="2"></asp:ListItem>
                    </agile:combobox>
                </dd>
            </dl>
        </fieldset>
    </div>
    <script type="text/javascript">
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
        }</script>
</asp:Content>
