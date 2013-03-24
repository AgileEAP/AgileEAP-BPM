
<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ModuleManager.aspx.cs" Inherits="AgileEAP.Administration.ModuleManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


	<%if (false)
      { %>
		<script   src="../Scripts/jquery-vsdoc.js" 
        type="text/javascript"></script> 
    <%}%>	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview" DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' text='<%#Eval("Text")%>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="模块名">
                    <ItemTemplate>
						<%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="所属应用">
                    <ItemTemplate>
						<%#Eval("AppID")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="模块显示名">
                    <ItemTemplate>
						<%#Eval("Text")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="程序集">
                    <ItemTemplate>
						<%#Eval("Assembly")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="命名空间">
                    <ItemTemplate>
						<%#Eval("Namesapce")%>
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
				<dt class="rowlable">模块名</dt>
				<dd class="rowinput">
						 <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
				</dd>
				<dt class="rowlable">所属应用</dt>
				<dd class="rowinput">
						<agile:ChooseBox ID="filterAppID" OpenUrl="AppManager.aspx?Entry=Choose" DialogTitle="选择所属应用"
						runat="server"></agile:ChooseBox>
				</dd>
				<dt class="rowlable">模块显示名</dt>
				<dd class="rowinput">
						 <asp:TextBox ID="filterText" runat="server" CssClass="text"></asp:TextBox>
				</dd>
				
            </dl>
        </fieldset>
    </div>
</asp:Content>


