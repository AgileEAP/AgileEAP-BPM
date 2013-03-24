<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="TraceLogManager.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.TraceLogManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <%# RemarkAttribute.GetEnumRemark((AgileEAP.Workflow.Enums.Operation)Enum.Parse(typeof(AgileEAP.Workflow.Enums.Operation), Eval("ActionType").ToString()))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作人">
                    <ItemTemplate>
                        <%#Eval("OperatorName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IP地址">
                    <ItemTemplate>
                        <%#Eval("ClientIP")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="流程名称">
                    <ItemTemplate>
                        <%#Eval("ProcessName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="流程实例名称">
                    <ItemTemplate>
                        <%#Eval("ProcessInstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动实例名称">
                    <ItemTemplate>
                        <%#Eval("ActivityInstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="工作项名称 ">
                    <ItemTemplate>
                        <%#Eval("WorkItemName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="消息">
                    <ItemTemplate>
                        <%#Eval("Message")%>
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
                <dt class="rowlable">操作</dt>
                <dd class="rowinput">
                    <agile:ComboBox ID="filterActionType" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                        runat="server">
                        <asp:ListItem Text="请选择" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="认领" Value="1"></asp:ListItem>
                        <asp:ListItem Text="启动" Value="2"></asp:ListItem>
                        <asp:ListItem Text="停止" Value="3"></asp:ListItem>
                        <asp:ListItem Text="发布" Value="4"></asp:ListItem>
                        <asp:ListItem Text="挂起" Value="5"></asp:ListItem>
                        <asp:ListItem Text="恢复" Value="6"></asp:ListItem>
                        <asp:ListItem Text="跳转" Value="7"></asp:ListItem>
                        <asp:ListItem Text="委托" Value="8"></asp:ListItem>
                        <asp:ListItem Text="提交" Value="9"></asp:ListItem>
                        <asp:ListItem Text="完成" Value="10"></asp:ListItem>
                        <asp:ListItem Text="出错" Value="11"></asp:ListItem>
                    </agile:ComboBox>
                </dd>
                <dt class="rowlable">操作人</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterOperatorName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">IP地址</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterClientIP" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">流程名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterProcessName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">流程实例名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterProcessInstName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">活动实例名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterActivityInstName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">工作项名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterWorkItemName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">消息</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterMessage" runat="server" CssClass="text"></asp:TextBox>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
