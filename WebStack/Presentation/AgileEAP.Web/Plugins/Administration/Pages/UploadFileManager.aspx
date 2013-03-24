<%@ Page Title="" Language="C#" MasterPageFile="../Master/Page.Master" AutoEventWireup="true"
    CodeBehind="UploadFileManager.aspx.cs" Inherits="AgileEAP.Administration.UploadFileManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript">
        function save() {
            var catalogName = $.trim($("#<%=txtCatalogName.ClientID%>").val());
            if (catalogName == "") {
                alert("目录名称不能为空");
                return false;
            }

            var path = $.trim($("#<%=txtPath.ClientID %>").val());
            if (path == "") {
                alert("目录路径不能为空");
                return false;
            }

            var catalog = getObjectValue("catalogDetail");
            catalog.Path = $("#<%=txtPath.ClientID %>").val();
            var value = JSON2.stringify(catalog)
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value }, function (result) {
                var ajaxResult = JSON2.parse(result);
                var message = "操作失败";
                if (ajaxResult) {
                    if (ajaxResult.PromptMsg != null)
                        message = ajaxResult.PromptMsg
                    if (ajaxResult.Result== 1) {
                        if (message == "")
                            message = "操作成功！";
                        var parentId = $("#chbParentIDdata").val();
                        if (parentId != "") {
                            var frm = window.parent.frames["ifrTree"];
                            frm.addNewAjaxTreeNode(parentId, ajaxResult.RetValue, $("#<%=txtCatalogName.ClientID%>").val(), "childDict");
                        }
                    }
                    $("#hidCurrentId").val(ajaxResult.RetValue);
                }

                alert(message);
            });
        }


        function del() {
            var rad = $("#divContent").find(":checked ").first();
            var id = rad.val();
            if ($.trim(id) == "") {
                rad.parent().parent().remove();
                //alert("操作成功");
                //return;
            }

            if (confirm("确认删除此文件？")) {
                $.post(getCurrentUrl(), { AjaxAction: "Delete", AjaxArgument: id }, function (result) {
                    var ajaxResult = JSON2.parse(result);
                    var message = "操作失败";
                    if (ajaxResult) {
                        if (ajaxResult.PromptMsg != null)
                            message = ajaxResult.PromptMsg
                        if (ajaxResult.Result== 1) {
                            if (message == "")
                                message = "操作成功！";
                            rad.parent().parent().remove();
                            $("#gvList_ItemCount").text(parseInt($("#gvList_ItemCount").text()) - 1);
                        }
                    }
                    alert(message);
                });
            }
        }

        function autoSetPath() {
            var hdnPath = $("#<%= hdnPath.ClientID%>").val();
            var catalogName =  $("#<%=txtCatalogName.ClientID %>").val();
            if (catalogName != "") {
                autoPath = hdnPath + "\\" + catalogName;
            }
            else {
                autoPath = hdnPath;
            }
            $("#<%=txtPath.ClientID %>").val(autoPath);
        }
    </script>
    <div class="div_block" id="catalogDetail">
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtCatalogName.ClientID%>" class="label">
                    <em>*</em> 目录名称
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtCatalogName" runat="server" CssClass="text" onblur="autoSetPath()"></asp:TextBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtPath.ClientID%>" class="label">
                    <em>*</em> 目录路径
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtPath" runat="server" CssClass="text" ReadOnly="true"></asp:TextBox>
                <asp:HiddenField ID="hdnPath" runat="server" />
                <asp:HiddenField ID="hdnActionType" runat="server" />
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=chbParentID.ClientID%>" class="label">
                    <em>*</em> 上级目录
                </label>
            </div>
            <div class="div_row_input">
                <agile:ChooseBox ID="chbParentID" OpenUrl="CatalogTree.aspx?Entry=Choose" DialogTitle="选择上级目录"
                    runat="server"></agile:ChooseBox>
            </div>
            <div class="div_row_lable">
                <label for="<%=txtSortOrder.ClientID%>" class="label">
                    序号
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
        <div class="div_row">
            <div class="div_row_lable">
                <label for="<%=txtDescription.ClientID%>" class="label">
                    描述
                </label>
            </div>
            <div class="div_row_input">
                <asp:TextBox ID="txtDescription" runat="server" CssClass="text"></asp:TextBox>
            </div>
        </div>
    </div>
    <div id="divContent" class="div_content">
        <agile:PagedGridView ID="gvList" runat="server" PageIndex="1" CssClass="gridview"
            DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="文件名称">
                    <ItemTemplate>
                        <%#Eval("FileName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="唯一名称">
                    <ItemTemplate>
						<%#Eval("UniqueName")%>
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField HeaderText="文件类型">
                    <ItemTemplate>
						<%#Eval("FileType")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="所在目录">
                    <ItemTemplate>
						<%#Eval("Catelog")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="文件路径">
                    <ItemTemplate>
                        <%#Eval("FilePath")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="描述">
                    <ItemTemplate>
						<%#Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="创建者">
                    <ItemTemplate>
                        <%#GetUserNameByUserId((string)Eval("Creator"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="创建时间">
                    <ItemTemplate>
                        <%#((DateTime)Eval("CreateTime")).ToShortDateString() %>
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
                <dt class="rowlable">文件名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterFileName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">唯一名称</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterUniqueName" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">文件类型</dt>
                <dd class="rowinput">
                    <agile:ComboBox ID="filterFileType" DropDownStyle="DropDownList" AppendDataBoundItems="true"
                        runat="server">
                        <asp:ListItem Text="请选择" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="类型1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="类型2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="类型3" Value="3"></asp:ListItem>
                    </agile:ComboBox>
                </dd>
                <dt class="rowlable">所在目录</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterCatelog" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">文件路径</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterPath" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">描述</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterDescription" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">创建者</dt>
                <dd class="rowinput">
                    <asp:TextBox ID="filterCreator" runat="server" CssClass="text"></asp:TextBox>
                </dd>
                <dt class="rowlable">创建时间</dt>
                <dd class="rowinput">
                    <agile:DatePicker ID="filterCreateTime" runat="server"></agile:DatePicker>
                </dd>
            </dl>
        </fieldset>
    </div>
    <script type="text/javascript" language="javascript">
        function openChooseBoxDialog(clientId, id, title, url, width, height) {
            var dlgDetail = openChooseDialog(title, url, width, height);
            if (dlgDetail != null) {
                $("#" + clientId).val(dlgDetail.text);
                $("#" + id + "data").val(dlgDetail.value);
                $("#" + id + "tag").val(dlgDetail.tag);

                $.post(getCurrentUrl(), { AjaxAction: "GetPath", AjaxArgument: dlgDetail.value }, function (result) {
                    var ajaxResult = JSON2.parse(result);
                    var path = "";
                    if ($("#<%=txtCatalogName.ClientID %>").val() != "") {
                        path = ajaxResult.RetValue + "\\" + $("#<%=txtCatalogName.ClientID %>").val();
                    }
                    else {
                        path = ajaxResult.RetValue;
                    }
                    $("#<%=hdnPath.ClientID %>").val(ajaxResult.RetValue)
                    $("#<%=txtPath.ClientID %>").val(path);

                });
                
            }
        } 
    </script>
</asp:Content>
