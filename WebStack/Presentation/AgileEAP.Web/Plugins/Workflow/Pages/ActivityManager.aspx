<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityManager.aspx.cs"
    MasterPageFile="../Master/Page.Master" Inherits="AgileEAP.Plugin.Workflow.ActivityManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <script type="text/javascript" language="javascript">
        dialogSetting.detailWidth = 800;
        dialogSetting.detailHeight = 480;

        function del(id) {
            if (id || id == "") {
                var rad = $("#divContent").find(":checked ").first();
                id = rad.val();
            }

            $.post(getCurrentUrl(), { AjaxAction: "Delete", AjaxArgument: id, ProcessDefID: $.query.get("ProcessDefID") }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("操作成功！");
                    rad.parent().parent().remove();
                    $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) - 1);
                }
                else {
                    alert("操作失败");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function chooseConfirm() {
            var activityName = $("#divContent").find(":checked ").first().val();
            $.post(getCurrentUrl(), { AjaxAction: "GetActivityForm", AjaxArgument: activityName, ProcessDefID: $.query.get("ProcessDefID") }, function (result) {
                if (result != "") {
                    window.returnValue = JSON2.parse(result); ;
                }
                window.close();
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
                <asp:TemplateField HeaderText="活动ID">
                    <ItemTemplate>
                        <%#Eval("ID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动名称">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="分支模式">
                    <ItemTemplate>
                        <%#Eval("SplitType").ToSafeString().Cast<AgileEAP.Workflow.Enums.SplitType>().GetRemark()%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动类型">
                    <ItemTemplate>
                        <%#Eval("ActivityType").ToSafeString().Cast<AgileEAP.Workflow.Enums.ActivityType>().GetRemark()%>
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
                    <agile:ChooseBox ID="filterCategoryID" OpenUrl="CategoryIDTree.aspx" DialogTitle="选择所属分类"
                        runat="server"></agile:ChooseBox>
                </dd>
            </dl>
        </fieldset>
    </div>
</asp:Content>
