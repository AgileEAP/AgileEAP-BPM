
<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="AppDetail.aspx.cs" Inherits="AgileEAP.Administration.AppDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%=HtmlExtensions.RequireScripts("jqueryFormValidator")%>

	<%if (false)
      { %>
		<script   src="../Scripts/jquery-vsdoc.js" 
        type="text/javascript"></script> 
    <%}%>	

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">

   <script type="text/javascript" language="javascript">

        //校验脚本
        function initValidator()
        {
			$.formValidator.initConfig({ formid: "aspnetForm", showallerror: true, onerror: function(msg) { promptMessage(msg); } });
            /*非空字段*/
            $("#<%=txtName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【应用名】不能为空" });
            /*非空字段*/
            $("#<%=txtText.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【应用显示名】不能为空" });
			/*日期*/
            $("#<%=dtpUseTime.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【开通时间】格式错误" });
            
			/*默认验证字段*/
			$("#<%=txtAppURL.ClientID%>").formValidator().inputValidator({max:128, onerror: "【访问URL】长度不能超过128" });
			/*默认验证字段*/
			$("#<%=txtDescription.ClientID%>").formValidator().inputValidator({max:256, onerror: "【描述】长度不能超过256" });
            /*非空字段*/
            $("#<%=txtCreator.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【创建者】不能为空" });
			/*日期*/
            $("#<%=dtpCreateTime.ClientID%>").formValidator().inputValidator({ type: "date", onerror: "【创建时间】格式错误" });
		}
		
		//页面初始化
        $(document).ready(function() {
			//初始化校验脚本
      		initValidator();
		 });
			
    </script>

    <div class="div_block">
        <div class="div_row">
            <div class="div_row_lable">
			<label for="<%=txtName.ClientID%>" class="label">
			<em>*</em>
                应用名
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtName" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
			<label for="<%=txtText.ClientID%>" class="label">
			<em>*</em>
                应用显示名
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtText" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
			<label for="<%=cboIsUse.ClientID%>" class="label">
			                是否开通
			</label>
            </div>
            <div class="div_row_input">
					   <agile:ComboBox ID="cboIsUse" DropDownStyle="DropDownList" 
                            AppendDataBoundItems="true" runat="server">
                            <asp:ListItem Text="否" Value="0" ></asp:ListItem>
							<asp:ListItem Text="是" Value="1" Selected="True"></asp:ListItem>
                        </agile:ComboBox>
            </div>
            <div class="div_row_lable">
			<label for="<%=dtpUseTime.ClientID%>" class="label">
			                开通时间
			</label>
            </div>
            <div class="div_row_input">
				<agile:DatePicker ID="dtpUseTime" runat="server"></agile:DatePicker>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
			<label for="<%=txtManageRole.ClientID%>" class="label">
                管理角色
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtManageRole" runat="server" CssClass="text"></asp:TextBox>
            </div>
            <div class="div_row_lable">
			<label for="<%=txtAppURL.ClientID%>" class="label">
			                访问URL
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtAppURL" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
			<label for="<%=txtSortOrder.ClientID%>" class="label">
			                序号
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="text"></asp:TextBox>
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
			<em>*</em>
                创建者
			</label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtCreator" runat="server" CssClass="text" Enabled="false"></asp:TextBox>
            </div>
            <div class="div_row_lable">
			<label for="<%=dtpCreateTime.ClientID%>" class="label">
			                创建时间
			</label>
            </div>
            <div class="div_row_input">
				<agile:DatePicker ID="dtpCreateTime" runat="server"></agile:DatePicker>
            </div>
        </div>
    </div>
</asp:Content>

