<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Master/Page.Master"
    CodeBehind="DeptDetail.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.DeptDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <%=HtmlExtensions.RequireScripts("jqueryFormValidator")%>

    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        //校验脚本
        function initValidator() {
            $.formValidator.initConfig({ formid: "aspnetForm", showallerror: true, onerror: function (msg) { promptMessage(msg); } });
            /*非空字段*/
            $("#<%=txtName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【部门名称】不能为空" });
            /*非空字段*/
            $("#<%=txtSortOrder.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【序号】不能为空" });
            $("#<%=txtCode.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【编号】不能为空" });
            $("#<%=cboArea.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【地址】不能为空" });
        }

        //页面初始化
        $(document).ready(function () {
            //初始化校验脚本
            initValidator();
            if ("<%= IsAdmin %>" == "true") {
                $("#<%=txtCode.ClientID %>").attr("readOnly", "true");
            }
        });
        function save(me, argument) {
            var deptDetail = getObjectValue("deptDetail");
            var actionType = $.query.get("ActionFlag");
            var value = JSON2.stringify(deptDetail)
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, ActionType: actionType }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                        parentID = "<%= ParentID %>";
                        var frm = window.parent.frames["ifrTree"];
                        frm.addNewAjaxTreeNode(parentID, ajaxResult.RetValue, $("#<%=txtName.ClientID%>").val(), "child");
                        $("#hidCurrentId").val(ajaxResult.RetValue.ID);
                    }
                }
                alert(message);
            });
        }
    </script>
    <div class="div_block" id="deptDetail">
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtName.ClientID%>" class="label">
                    <em>*</em> 部门名称
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtName" runat="server" CssClass="text"></asp:TextBox>
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
                <label for="<%=txtCode.ClientID%>" class="label">
                    <em>*</em> 编号
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtCode" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=cboArea.ClientID%>" class="label">
                    <em>*</em> 地区
                </label>
            </div>
            <div class="div_row_input">
                <agile:Combox runat="server" ID="cboArea" IsSingle="true" />
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtAddress.ClientID%>" class="label">
                    地址
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtZipCode.ClientID%>" class="label">
                    邮编
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtZipCode" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtGovernor.ClientID%>" class="label">
                    主管
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtGovernor" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtGovernPosition.ClientID%>" class="label">
                    主管岗位
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtGovernPosition" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtManager.ClientID%>" class="label">
                    管理员
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtManager" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtContactMan.ClientID%>" class="label">
                    联系人
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtContactMan" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtContactPhone.ClientID%>" class="label">
                    联系电话
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtContactPhone" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtEmail.ClientID%>" class="label">
                    电子邮件
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtWebURL.ClientID%>" class="label">
                    网站地址
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtWebURL" runat="server" CssClass="text"></asp:TextBox>
            </div>
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
            <div class="div_row_lable">
                <label for="<%=txtCreator.ClientID%>" class="label">
                    创建者
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtCreator" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtCreateTime.ClientID%>" class="label">
                    创建时间
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtCreateTime" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
