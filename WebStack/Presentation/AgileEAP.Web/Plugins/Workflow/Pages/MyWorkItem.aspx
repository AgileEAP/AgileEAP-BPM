<%@ Page Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="MyWorkItem.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.MyWorkItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function rowClick(me, id) {
            if (currentRow) {
                currentRow.className = "gridview_row";
            }
            me.className = "rowcurrent";
            currentRow = me;

            var input = $(me).find("input").first();
            input.attr("checked", true);
            var processInstID = input.attr("processinstid");
            var activity = input.attr("activity");
            var status = input.attr("status");
            var url = "";
            if (activity == "申请虚拟机" && status=="1") {
                url = '<%=RootPath %>Workflow/MyWorkItemDetail.aspx?processInstID=' + processInstID + '&workItemID=' + id + '&Entry=ApplyVirtualMachine';
            }
            else {
                url = '<%=RootPath %>Workflow/MyWorkItemDetail.aspx?processInstID=' + processInstID + '&workItemID=' + id + '&Entry=' + $.query.get("Entry");
               // url = '../../FormDesigner/Form/InitForm?processInstID=' + processInstID + '&workItemID=' + id + '&Entry=' + $.query.get("Entry");
            }
            window.parent.openOperateDialog('办理', url, 900, 600, 1, 0, 30);
        }
    </script>
    <div id="divContent" class="div_content">
        <agile:pagedgridview id="gvList" runat="server" pageindex="1" cssclass="gridview"
            datakeynames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' activity="<%#Eval("Name")%>" name="radioId" status="<%# Eval("CurrentState")%>" processinstid='<%#Eval("ProcessInstID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="工作项名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="所属流程">
                    <ItemTemplate>
                        <%#Eval("ProcessInstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="当前状态">
                    <ItemTemplate>
                        <%# Eval("CurrentState").ToSafeString().Cast<AgileEAP.Workflow.Enums.WorkItemStatus>().GetRemark()%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="提交人">
                    <ItemTemplate>
                        <%#Eval("CreatorName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="提交时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("StartTime")).ToString("yyyy-MM-dd HH:mm:ss")%>
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
                <dt class="rowlable">工作项名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">所属流程</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterProcessName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">提交人</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterCreatorName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">当前状态</dt>
                <dd class="rowinput">
                    <agile:combobox id="filterCurrentState" dropdownstyle="DropDownList" appenddatabounditems="true"
                        runat="server">
                        <asp:ListItem Text="全部" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="待执行" Value="1"></asp:ListItem>
                        <asp:ListItem Text="停止" Value="2"></asp:ListItem>
                        <asp:ListItem Text="执行" Value="3"></asp:ListItem>
                        <asp:ListItem Text="挂起" Value="4"></asp:ListItem>
                        <asp:ListItem Text="完成" Value="5"></asp:ListItem>
                        <asp:ListItem Text="终止" Value="6"></asp:ListItem>
                        <asp:ListItem Text="取消" Value="7"></asp:ListItem>
                        <asp:ListItem Text="出错" Value="8"></asp:ListItem>
                    </agile:combobox>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
