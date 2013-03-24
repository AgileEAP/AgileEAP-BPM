<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="ProcessDefManager.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.ProcessDefManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript" language="javascript">
        dialogSetting.detailWidth = 800;
        dialogSetting.detailHeight = 480;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function publish() {
            $.post(getCurrentUrl(), { AjaxAction: "Publish", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                        tr.find("td")[4].innerText = ajaxResult.RetValue;
                    }
                }
                //$("#hidCurrentId").val(ajaxResult.RetValue);
                alert(message);
            });
        }

        function processChart() {
            var id = $("#hidCurrentId").val();
            window.parent.openOperateDialog("流程图", "<%=RootPath %>Workflow/ProcessChart.aspx?processDefID=" + id, 600, 500);
        }
        function addProcess() {
            var id = $("#hidCurrentId").val();
            window.parent.showDialog("actionDialog", "设计流程图", "/WorkflowDesigner/Workflow/WorkflowDesigner", 1050, 700);
        }
        function jsChart() {
            var id = $("#hidCurrentId").val();
            window.parent.showDialog("actionDialog", "js流程图", "/Workflow/Design/DrawWorkflow?processDefID=" + id, 600, 500);
        }
        function designChart() {
            var id = $("#hidCurrentId").val();
            if (id) {
                window.parent.showDialog("actionDialog", "设计流程图", "/WorkflowDesigner/Workflow/WorkflowDesigner?processDefID=" + id, 1050, 700);
            }
            else {
                alert("请先选择流程，再查看或修改流程图");
            }
        }
        function testButton() {
            window.parent.openDialog2({ contentUrl: "/Workflow/eForm/Index?processDefID=914b4e9d-4db9-4578-b9dc-400bee0d7eb9&&Entry=StartProcess" });
        };
        function formDesigner() {
            var id = $("#hidCurrentId").val();
            window.parent.showDialog("actionDialog", "表单设计器", "/FormDesigner/Home/FormDesigner?processDefID=" + id, 1250, 700);
        }
        function initForm() {
            var id = $("#hidCurrentId").val();
            window.parent.showDialog("actionDialog", "工作台", "/Workflow/eForm/Index?processDefID=" + id + "&&Entry=StartProcess", 950, 600);
        }
        function stop() {
            $.post(getCurrentUrl(), { AjaxAction: "Stop", AjaxArgument: $("#hidCurrentId").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result == 1) {
                        var tr = $("#<%=gvList.ClientID %>").find(":checked ").parent().parent();
                        tr.find("td")[4].innerText = ajaxResult.RetValue;
                    }
                }
                //$("#hidCurrentId").val(ajaxResult.RetValue);
                alert(message);
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
                <asp:TemplateField HeaderText="显示名">
                    <ItemTemplate>
                        <%#Eval("Text")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="流程内容">
                    <ItemTemplate>
						<%#Eval("Content")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="所属分类">
                    <ItemTemplate>
                        <%#GetDictItemText(Eval("CategoryID").ToSafeString(),"ProcessCategory")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="当前状态">
                    <ItemTemplate>
                        <%# RemarkAttribute.GetEnumRemark((AgileEAP.Workflow.Enums.ProcessStatus)Enum.Parse(typeof(AgileEAP.Workflow.Enums.ProcessStatus), Eval("CurrentState").ToString()))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="是否当前版本">
                    <ItemTemplate>
                        <%# Eval("CurrentFlag").ToSafeString()=="1"?"是":"否"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="流程启动者">
                    <ItemTemplate>
                        <%#Eval("Startor")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="是否有活动实例">
                    <ItemTemplate>
                        <%# Eval("IsActive").ToSafeString() == "1" ? "是" : "否"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="版本">
                    <ItemTemplate>
                        <%#Eval("Version")%>
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
                <dt class="rowlable">显示名</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterText" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">流程内容</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterContent" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">所属分类</dt>
                <dd class="rowinput">
                    <agile:ComboBox runat="server" ID="filterCategoryID" DropDownStyle="DropDownList"
                        AppendDataBoundItems="true">
                    </agile:ComboBox>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
