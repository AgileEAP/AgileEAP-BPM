<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="TransitionManager.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.TransitionManager" %>

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
                <asp:TemplateField HeaderText="源活动名称">
                    <ItemTemplate>
                        <%#Eval("SrcActInstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="源活动定义名称">
                    <ItemTemplate>
                        <%#Eval("SrcActName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="目标活动名称">
                    <ItemTemplate>
                        <%#Eval("DestActInstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="目标活动定义名称">
                    <ItemTemplate>
                        <%#Eval("DestActName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="流程实例名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="迁移时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("TransTime")) %>
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
                <dt class="rowlable">源活动名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterSrcActInstName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">源活动定义名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterSrcActName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">目标活动名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterDestActInstName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">目标活动定义名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterDestActName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">流程实例名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">迁移时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterTransTime" runat="server"></agile:DatePicker>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
