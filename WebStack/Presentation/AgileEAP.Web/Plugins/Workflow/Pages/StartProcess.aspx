<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Master/Page.Master"
    CodeBehind="StartProcess.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.StartProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function startProcess() {
            var processDefID = $("input:checked").first().val();
            //var url = '<%=RootPath %>eClient/Order/Detail/custom?processDefID=' + processDefID + '&Entry=StartProcess';
            //window.parent.openOperateDialog('流程', url, 1100, 600, 1, 0, 50);

            //原来的
            //   var url = '<%=RootPath %>Workflow/MyEForm.aspx?processDefID=' + processDefID + '&Entry=StartProcess';
            // window.parent.openOperateDialog('流程', url, 700, 400, 1, 0, 50);
           // window.parent.showDialog("actionDialog", "工作台", "/FormDesigner/Form/InitForm?processDefID=" + id + "&&Entry=StartProcess", 950, 600);

            //$.post(getCurrentUrl(), { AjaxAction: "StartProcessInst", AjaxArgument: processDefID }, function (result) {
            //    var ajaxResult = JSON2.parse(result);
            //    if (ajaxResult && ajaxResult.Result == 1) {
            //        var parameters = ajaxResult.RetValue.split("$");
            //        var url = '<%=RootPath %>Workflow/MyEForm.aspx?processDefID=&' + processDefID + '&processInstID=' + parameters[0] + '&workItemID=' + parameters[1] + '&Entry=StartProcess';
            //        window.parent.openOperateDialog('流程', url, 700, 400, 1, 0, 50);
            //    }
            //    else {
            //        alert("操作失败");
            //    }
            //});
            $.post("/FormDesigner/Form/StartProcessInst", { processDefID: processDefID }, function (value) {
                var ajaxResult = value;
                var message = "操作失败";
                if (ajaxResult && ajaxResult.Result == 1) {
                    var parameters = ajaxResult.RetValue.split("$");
                    // var url = '<%=RootPath %>Workflow/MyEForm.aspx?processDefID=&' + processDefID + '&processInstID=' + parameters[0] + '&workItemID=' + parameters[1] + '&Entry=StartProcess';
                    url = '/FormDesigner/Form/InitForm?processDefID=&' + processDefID + '&processInstID=' + parameters[0] + '&workItemID=' + parameters[1] + '&Entry=WaitProcess';
                    window.parent.openOperateDialog('流程', url, 950, 600, 1, 0, 50);
                   // alert("操作成功");
                }
                else {
                    alert("操作失败");
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
                <asp:TemplateField HeaderText="所属分类">
                    <ItemTemplate>
                        <%#GetDictItemText(Eval("CategoryID").ToSafeString(),"ProcessCategory")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="显示名">
                    <ItemTemplate>
                        <%#Eval("Text")%>
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
</asp:Content>
