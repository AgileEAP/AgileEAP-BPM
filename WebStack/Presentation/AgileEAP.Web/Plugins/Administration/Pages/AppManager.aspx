<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="AppManager.aspx.cs" Inherits="AgileEAP.Administration.AppManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' text=" <%#Eval("Text")%>"
                            name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用名">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="应用显示名">
                    <ItemTemplate>
                        <%#Eval("Text")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="是否开通">
                    <ItemTemplate>
                        <%#Eval("IsUse")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="开通时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("UseTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="管理角色">
                    <ItemTemplate>
                        <%#Eval("ManageRole")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="访问URL">
                    <ItemTemplate>
                        <%#Eval("AppURL")%>
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
                <dt class="rowlable">应用名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">应用显示名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterText" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                
            </dl>
        </fieldset>
    </div>
</asp:Content>
