
<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="CacheDataManager.aspx.cs" Inherits="AgileEAP.Administration.CacheDataManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


	<%if (false)
      { %>
		<script   src="../Scripts/jquery-vsdoc.js" 
        type="text/javascript"></script> 
    <%}%>	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
<script type="text/javascript">
    function clearCache() {
        $.post(getCurrentUrl(), { AjaxAction: "ClearCache" }, function (result) {
            var ajaxResult = JSON2.parse(result);
            var message = "清除失败！";
            if (ajaxResult) {
                if (ajaxResult.PromptMsg != null)
                    message = ajaxResult.PromptMsg
            }
            alert(message);
         });
    }
</script>
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview" DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="缓存名">
                    <ItemTemplate>
						<%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="缓存值">
                    <ItemTemplate>
						<%#Eval("Value")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="生效时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("CreateTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="失效时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("ExpireTime")).ToShortDateString() %>
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
				<dt class="rowlable">缓存名</dt>
				<dd class="rowinput">
						 <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
				</dd>
				<dt class="rowlable">缓存值</dt>
				<dd class="rowinput">
						 <asp:TextBox ID="filterValue" runat="server" CssClass="text"></asp:TextBox>
				</dd>
				<dt class="rowlable">生效时间</dt>
				<dd class="rowinput">
						<agile:DatePicker ID="filterCreateTime" runat="server"></agile:DatePicker>
				</dd>
				<dt class="rowlable">失效时间</dt>
				<dd class="rowinput">
						<agile:DatePicker ID="filterExpireTime" runat="server"></agile:DatePicker>
				</dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>


