<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ProcessInstManager.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.ProcessInstManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function suspend() {
            $.post(getCurrentUrl(), { AjaxAction: "Suspend", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "挂起失败";
                if (ajaxResult && ajaxResult.Result == 1) {
                    var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                    tr.find("td")[2].innerText = ajaxResult.RetValue;
                    message = "挂起成功";
                }
                alert(message);
            });
        }

        function del() {
            $.post(getCurrentUrl(), { AjaxAction: "Delete", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "删除失败";
                if (ajaxResult && ajaxResult.Result == 1) {
                    var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                    tr.remove();
                    message = "删除成功";
                }
                alert(message);
            });
        }

        function resume() {
            $.post(getCurrentUrl(), { AjaxAction: "Resume", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "恢复失败";
                if (ajaxResult && ajaxResult.Result == 1) {
                    var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                    tr.find("td")[2].innerText = ajaxResult.RetValue;
                    message = "恢复成功";
                }
                alert(message);
            });
        }

        function terminate() {
            $.post(getCurrentUrl(), { AjaxAction: "Terminate", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "终止失败";
                if (ajaxResult && ajaxResult.Result == 1) {
                    var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                    tr.find("td")[2].innerText = ajaxResult.RetValue;
                    message = "终止成功";
                }
                alert(message);
            });
        }
        function proessInstChart() {
            var id = $("#hidCurrentId").val();
            $.post(getCurrentUrl(), { AjaxAction: "GetprocessDefID", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    window.parent.openOperateDialog("流程图", "<%=RootPath %>Workflow/Design/DrawWorkflow?ProcessInstID=" + id + "&processDefID=" + ajaxResult.RetValue, 700, 500);
                }
            });
           
        }
    </script>
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="当前状态">
                    <ItemTemplate>
                        <%# RemarkAttribute.GetEnumRemark((AgileEAP.Workflow.Enums.ProcessInstStatus)Enum.Parse(typeof(AgileEAP.Workflow.Enums.ProcessInstStatus), Eval("CurrentState").ToString()))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="限期时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("LimitTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="启动时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("StartTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="结束时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("EndTime")).ToShortDateString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="所属流程版本">
                    <ItemTemplate>
                        <%#Eval("ProcessVersion")%>
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
                <dt class="rowlable">名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">当前状态</dt>
                <dd class="rowinput">
                    <agile:ComboBox ID="filterCurrentState" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                        runat="server">
                        <asp:ListItem Text="请选择" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="未启动" Value="1"></asp:ListItem>
                        <asp:ListItem Text="运行" Value="2"></asp:ListItem>
                        <asp:ListItem Text="挂起" Value="3"></asp:ListItem>
                        <asp:ListItem Text="完成" Value="4"></asp:ListItem>
                        <asp:ListItem Text="终止" Value="5"></asp:ListItem>
                        <asp:ListItem Text="取消" Value="6"></asp:ListItem>
                    </agile:ComboBox>
                </dd>
                <dt class="rowlable">限期时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterLimitTime" runat="server"></agile:DatePicker>
                </dd>
                <dt class="rowlable">启动时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterStartTime" runat="server"></agile:DatePicker>
                </dd>
                <dt class="rowlable">结束时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterEndTime" runat="server"></agile:DatePicker>
                </dd>
                <dt class="rowlable">所属流程版本</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterProcessVersion" runat="server" CssClass="text"></asp:TextBox>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
