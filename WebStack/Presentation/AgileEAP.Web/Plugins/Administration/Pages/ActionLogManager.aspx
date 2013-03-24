<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ActionLogManager.aspx.cs" Inherits="AgileEAP.Administration.ActionLogManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" PageSize="20" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="日志类型">
                    <ItemTemplate>
                        <%# RemarkAttribute.GetEnumRemark((LogType)Enum.Parse(typeof(LogType), Eval("LogType").ToString()))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作信息" >
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%#Eval("Message")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作结果">
                    <ItemTemplate>
                        <%# Eval("Result").ToSafeString()=="0"?"失败":"成功"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作员">
                    <ItemTemplate>
                        <%#Eval("UserName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作机器IP">
                    <ItemTemplate>
                        <%#Eval("ClientIP")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作时间">
                    <ItemTemplate>
                        <%#Eval("CreateTime") %>
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
                <dt class="rowlable">操作员名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterUserName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">日志类型</dt>
                <dd class="rowinput">
                    <agile:ComboBox ID="filterLogType" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                        runat="server">
                        <asp:ListItem Text="全部" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="登陆" Value="1"></asp:ListItem>
                        <asp:ListItem Text="操作" Value="0"></asp:ListItem>
                        <asp:ListItem Text="接口" Value="2"></asp:ListItem>
                    </agile:ComboBox>
                </dd>
                <dt class="rowlable">操作机器IP</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterClientIP" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">应用名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterAppModule" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">操作信息</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterMessage" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">操作结果</dt>
                <dd class="rowinput">
                    <agile:ComboBox ID="filterResult" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                        runat="server">
                        <asp:ListItem Text="全部" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="成功" Value="1"></asp:ListItem>
                        <asp:ListItem Text="失败" Value="0"></asp:ListItem>
                    </agile:ComboBox>
                </dd>
                <dt class="rowlable">操作时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterCreateTime" runat="server"></agile:DatePicker>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
