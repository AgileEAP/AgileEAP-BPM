<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Master/Page.Master"
    CodeBehind="UserDetail.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.UserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="Stylesheet" type="text/css" href="../Skins/Default/Styles/tabs.css" />--%>
    <%=HtmlExtensions.RequireStyles("tab_default") %>
    <%=HtmlExtensions.RequireScripts("tab") %>
    <%=HtmlExtensions.RequireScripts("jqueryFormValidator")%>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript">
        //校验脚本
        function initValidator() {
            $.formValidator.initConfig({ formid: "aspnetForm", showallerror: true, onerror: function (msg) { promptMessage(msg); } });
            /*非空字段*/
            $("#<%=txtCode.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【员工编号】不能为空" });
            $("#<%=txtLoginName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【登录名】不能为空" });
            /*非空字段*/
            $("#<%=txtUserName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【姓名】不能为空" });
            /*非空字段*/
            /* $("#<%=txtPassword.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【密码】不能为空" });*/
            /*非空字段*/
            $("#<%=txtMajorOrgID.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【主机构】不能为空" });
            $("#<%=txtMajorOrgID.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【主机构】不能为空" });

            /*日期*/
            $("#<%=dtpExpireTime.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【密码失效时间】格式错误" });
            /*日期*/
            $("#<%=dtpLastLogin.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【最近登录时间】格式错误" });
            /*日期*/
            $("#<%=dtpStartTime.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【有效开始日期】格式错误" });
            /*日期*/
            $("#<%=dtpEndTime.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【有效截止日期】格式错误" });
            /*邮箱*/
            $("#<%=txtEmail.ClientID%>").formValidator().inputValidator().regexValidator({ regexp: "^([\\w-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([\\w-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$", onerror: "你输入的邮箱格式不正确" });
            /*手机号码 */
            $("#<%=txtPhone.ClientID%>").formValidator().inputValidator().regexValidator({ regexp: "mobile", datatype: "enum", onerror: "【联系电话】格式不正确" });

        }
        //页面初始化
        $(document).ready(function () {
            //初始化校验脚本
            initValidator();
            resizeWindow();
        });

        function save(me, argument) {
            var operator = getObjectValue("main_bg2");
            var value = JSON2.stringify(operator);
            var currentID = $.query.get("CurrentId");
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, CurrentID: currentID, OrgID: operator.MajorOrgID }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        if (message == "")
                            message = "操作成功！";
                        $("#hidCurrentId").val(ajaxResult.RetValue.ID);
                    }
                }
                alert(message);
                if (ajaxResult.Result == 1) {
                    window.parent.closeDialog(true);
                    window.parent.frames["ifrContentPage"].frames[1].location.reload();
                    //window.parent.closeDialog(true);
                    //window.parent.location.reload();
                }
            });
        }

        $(window).resize(function () {
            resizeWindow();
        });

        //function selectVMOptionTab(i) {
        //    switch (i) {
        //        case 1:
        //            document.getElementById("divBaeInfo").className = "tabBg1";
        //            document.getElementById("divExtendInfo").className = "tabBg6";
        //            $("#tabBaseInfo").show();
        //            $("#tabExtendInfo").hide();
        //            $("#hidSelected").val(1);
        //            break;
        //        case 2:
        //            document.getElementById("divBaeInfo").className = "tabBg4";
        //            document.getElementById("divExtendInfo").className = "tabBg7";
        //            $("#tabBaseInfo").hide();
        //            $("#tabExtendInfo").show();
        //            $("#hidSelected").val(2);
        //            break;
        //    }
        //}


        function resizeWindow() {
            var windowHeight = getWindowHeight();
            if (windowHeight == 0) return;
            document.getElementById("main_bg2").style.width = document.body.offsetWidth - 8 + "px";
            document.getElementById("main_bg2").style.height = windowHeight - 54 + "px";
            document.getElementById("tabBaseInfo").style.height = document.getElementById("main_bg2").style.height;
            document.getElementById("tabBaseInfo").style.width = document.getElementById("main_bg2").style.width;
            document.getElementById("tabExtendInfo").style.height = document.getElementById("main_bg2").style.height;
            document.getElementById("tabExtendInfo").style.width = document.getElementById("main_bg2").style.width;
        }
    </script>
    <style type="text/css">
        #ctl00_navigation {
            display: none;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <ul id="tabcontainer">
        <li id="BaseInfo" class="tabBg1" onclick="switchTab('BaseInfo');">基本</li>
        <li id="ExtendInfo" class="tabBg2" onclick="switchTab('ExtendInfo');">扩展</li>
    </ul>
    <div id="main_bg">
        <div id="main_bg2">
            <div class="div_block" id="tabBaseInfo">
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=cboUserType.ClientID%>" class="label">
                            <em>*</em>用户类型
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ComboBox ID="cboUserType" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                            runat="server">
                            <asp:ListItem Text="一般" Value="2"></asp:ListItem>
                            <asp:ListItem Text="分公司管理员" Value="1"></asp:ListItem>
                        </agile:ComboBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtCode.ClientID%>" class="label">
                            <em>*</em>员工编号
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtCode" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtLoginName.ClientID%>" class="label">
                            <em>*</em>登录名
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtLoginName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtUserName.ClientID%>" class="label">
                            <em>*</em>姓名
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtPassword.ClientID%>" class="label">
                            <em>*</em>密码
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="text" TextMode="Password"></asp:TextBox>
                        <asp:HiddenField ID="txtOldPassword" runat="server"></asp:HiddenField>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=dtpExpireTime.ClientID%>" class="label">
                            密码失效时间
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpExpireTime" runat="server"></agile:DatePicker>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=cboAuthMode.ClientID%>" class="label">
                            认证方式
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ComboBox ID="cboAuthMode" DropDownStyle="DropDownList" runat="server">
                            <asp:ListItem Text="本地密码认证" Value="1"></asp:ListItem>
                            <asp:ListItem Text="LDAP认证" Value="2"></asp:ListItem>
                        </agile:ComboBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=cboStatus.ClientID%>" class="label">
                            <em>*</em>用户状态
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ComboBox ID="cboStatus" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                            runat="server">
                            <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                            <asp:ListItem Text="挂起" Value="2"></asp:ListItem>
                            <asp:ListItem Text="注销" Value="3"></asp:ListItem>
                            <asp:ListItem Text="锁定" Value="4"></asp:ListItem>
                        </agile:ComboBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=cboSkin.ClientID%>" class="label">
                            <em>*</em>菜单风格
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ComboBox ID="cboSkin" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                            runat="server">
                            <asp:ListItem Text="默认" Value="Default"></asp:ListItem>
                        </agile:ComboBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=dtpLastLogin.ClientID%>" class="label">
                            最近登录时间
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpLastLogin" runat="server"></agile:DatePicker>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=dtpStartTime.ClientID%>" class="label">
                            有效开始日期
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpStartTime" runat="server"></agile:DatePicker>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=dtpEndTime.ClientID%>" class="label">
                            有效截止日期
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpEndTime" runat="server"></agile:DatePicker>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtMACCode.ClientID%>" class="label">
                            绑定MAC
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtMACCode" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtClientIP.ClientID%>" class="label">
                            绑定IP地址
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtClientIP" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtEmail.ClientID%>" class="label">
                            邮箱地址
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtPhone.ClientID%>" class="label">
                            联系电话
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="div_block" id="tabExtendInfo">
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=dtpBirthday.ClientID%>" class="label">
                            出生日期
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpBirthday" runat="server"></agile:DatePicker>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=dtpWorkFromDate.ClientID%>" class="label">
                            参加工作时间
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpWorkFromDate" runat="server"></agile:DatePicker>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtNation.ClientID%>" class="label">
                            民族
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtNation" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtBirthplace.ClientID%>" class="label">
                            出生地
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtBirthplace" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtNativeplace.ClientID%>" class="label">
                            籍贯
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtNativeplace" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtPoliticsStatus.ClientID%>" class="label">
                            政治面貌
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPoliticsStatus" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtIndustrialGrade.ClientID%>" class="label">
                            专业技术职务
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtIndustrialGrade" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtSpeciality.ClientID%>" class="label">
                            特长
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtSpeciality" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtPositionName.ClientID%>" class="label">
                            岗位名称
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPositionName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtPosition.ClientID%>" class="label">
                            基本岗位
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPosition" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtPostGrade.ClientID%>" class="label">
                            岗位等级
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPostGrade" runat="server" CssClass="text"></asp:TextBox>
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
                        <label for="<%=dtpInDate.ClientID%>" class="label">
                            入职日期
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpInDate" runat="server"></agile:DatePicker>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=dtpOutDate.ClientID%>" class="label">
                            离职日期
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:DatePicker ID="dtpOutDate" runat="server"></agile:DatePicker>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtFax.ClientID%>" class="label">
                            传真号码
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtFax" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtMobile.ClientID%>" class="label">
                            手机号码
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtMSN.ClientID%>" class="label">
                            MSN号码
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=txtOfficePhone.ClientID%>" class="label">
                            办公电话
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtOfficePhone" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtAddress.ClientID%>" class="label">
                            住址
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        <label for="<%=chbDirector.ClientID%>" class="label">
                            直接主管
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ChooseBox ID="chbDirector" OpenUrl="OperatorTree.aspx" DialogTitle="选择直接主管"
                            runat="server"></agile:ChooseBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtMajorOrgID.ClientID%>" class="label">
                            <em>*</em>主机构
                        </label>
                    </div>
                    <div class="div_row_input">
                        <agile:ChooseBox ID="chbMajorOrgID" OpenUrl="OrgTree.aspx?Entry=Choose" DialogWidth="600"
                            DialogTitle="选择主机构" runat="server">
                        </agile:ChooseBox>
                    </div>
                    <!--
                        <div class="div_row_input">
                            <asp:TextBox ID="txtMajorOrgID" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                     -->
                    <div class="div_row_lable">
                        <label for="<%=txtPhoto.ClientID%>" class="label">
                            照片
                        </label>
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtPhoto" runat="server" CssClass="text" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
