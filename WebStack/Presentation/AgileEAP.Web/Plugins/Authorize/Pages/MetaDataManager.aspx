<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="MetaDataManager.aspx.cs" Inherits="AgileEAP.Plugin.Authorize.MetaDataManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">
        function onTreeNodeChecked(me, id, mode) {
            if (mode == "Single") {//单选
                $("#ajaxTree_Div_Id").find(":checkbox").attr("checked", false);
                $("#ajaxTreeCurrentNodeId").val(id)
                $(me).attr("checked", true);
            }
            else if (mode == "RelatedMultiple") {//级联多选
                $("ul:first", $("#" + id)).find(":checkbox").attr("checked", me.checked);
                parentChecked(me);
            }
            else if (mode == "Multiple") {//多选
                if (me.checked) {
                    $("ul:first", $("#" + id)).find(":checkbox").attr("checked", false);//取消所有子节点
                    parentUnChecked(me);
                }
            }
            else if (mode == "LeafMultiple") {
                var children = $("ul:first", $("#" + id)).find(":checkbox").attr("checked", me.checked);
                if (children[0]) {
                    parentChecked(me);
                }
            }
        }

        /*父节点取消选择*/
        function parentUnChecked(curCheckbox) {
            var parentNode = $(curCheckbox).parent().parent().parent();  //getParentByTagName(ele, 3);
            if (parentNode[0]) {
                var parentCheckBox = $("#" + "chk_" + parentNode.attr("id"));
                if (parentCheckBox[0]) {
                    parentCheckBox.attr("checked", false);
                    parentUnChecked(parentCheckBox);//递归
                }
            }
        }

        function save() {
            var resources = getCheckedNodes();

            var dataIDs = "";
            try {
                $("input:checked", $("#dataContainer")).each(function (i) {
                    dataIDs += $(this).attr("title") + ",";
                });

            } catch (e) { }

            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: JSON2.stringify(resources), DataIDs: dataIDs }, function (value) {
                var ajaxResult = JSON2.parse(value);
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null && ajaxResult.PromptMsg != "") {
                        alert(ajaxResult.PromptMsg);

                        if (window.parent)
                            window.parent.closeDialog();
                    }
                }
                else {
                    alert("系统出错，请与管理员联系！");
                }
            });
        }
    </script>
    <div class="div_block">
        <agile:AjaxTree ID="AjaxTree1" Runat="server" />
    </div>
</asp:Content>
