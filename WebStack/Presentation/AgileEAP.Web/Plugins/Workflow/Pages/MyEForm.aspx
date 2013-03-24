<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyEForm.aspx.cs" MasterPageFile="../Master/Page.Master"
    Inherits="AgileEAP.Plugin.Workflow.MyEForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
    
     #eFormView
     {
      width:100%;
      height:100%;
      text-align:center;
     }
    #eFormHeader
    {
        width:100%;
        height:20px;
        font-weight: bold;
    }
     #eForm
     {
        width:80%;
        height:90%;
        margin:auto;
     }
     .eForm_lable
     {
       text-align:right;
     }
     .eForm_input
     {
     text-align:left;
     }
     .eForm_input_combox
     {
         text-align:left;
         position:absolute;
         width:46%;
     }

     .eForm_chooseBox
     {
        background: white url("/Plugins/Workflow/Content/Themes/<%=Skin %>/Images/chooseBox.png") no-repeat right;
        border: 1px solid #F0F0F0;
        cursor: pointer;
     }
     .eForm_textbox_combox
     {
        background: white url("/Plugins/Workflow/Content/Themes/<%=Skin %>/Images/comboxIcon.gif") no-repeat right;
        border: 1px solid #F0F0F0;
        cursor: pointer;
     }
    .cbList
    {
        border: 1px solid #ccc;
        display:none;
        position: relative !important;
        position:absolute;
        float: none;
        background-color: #ffffff;
        width: 65%;
        overflow:auto;
        font-size: 12px;
        z-index: 1001;
        padding: 0px;
         margin:0px !important;
         margin: 20px 0px 0px -160px;
        height:auto!important;
        min-height:70px;
        height:70px;
    }
    #cbItems
    {
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size: 12px;
        width: 65%;
        padding: 0px;
        margin: 0px;   
    }
    #cbItems ul
    {
        list-style: none;
        margin: 0px;
        padding: 0px;
        border: none;
        line-height: 15px;
        list-style-position:inside;
    }
    #cbItems ul li
    {
        margin: 0px;
        padding: 0px;
    }
    #cbItems ul li a
    {
      /*  display: block; border-bottom: 1px dashed #C39C4E;*/
        padding: 2px 0px 2px 5px;
        text-decoration: none;
        color: #666666;
        width: 110px;
        font-size: 12px;
    }
    #cbItems ul li a:hover, #cbItems ul li a:focus
    {
        color: #000000;
        background-color: #eeeeee;
    }

     input 
     {
     	padding: .1em .3ex;
        width:65%;
        border: 1px solid #F0F0F0;
     }
        
input.text {
width:65%;
}

    input[type="checkbox"] {
         width:15px;
    }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script language="javascript" type="text/javascript">

        function getSkin() {
            var skin = "<%=Skin.ToLower()%>";
            return skin;
        }

        function getFormValue(container) {
            var objValue = new Object();
            var inputs = container && container != "" ? $(":input", $("#" + container)) : $(":input");
            inputs.each(function () {
                var values = this.id.split("_");
                var property = values[values.length - 1];

                if (property == "SortOrder") {
                    objValue[property] = parseInt($(this).val());
                }
                else if ($(this).attr("tag") == "choosebox") {
                    if (endWith(property, "data")) {
                        objValue[property.substr(0, property.length - 4)] = $(this).val();
                    }
                }
                else if ($(this).attr("tag") == "combox") {
                    if (endWith(property, "data")) {
                        var enumValue = parseInt($(this).val());
                        if (!enumValue)
                            enumValue = $(this).val();
                        objValue[property.substr(0, property.length - 4)] = enumValue;
                    }
                }
                else if (property != "ParentID") {
                    if ($(this).attr("type") == "checkbox") {
                        objValue[property] = $(this).attr("checked") == "checked" ? 1 : 0;
                    }
                    else {
                        objValue[property] = $(this).val();
                    }
                }
            });
            return objValue;
        }

        function submit() {
            $("input[isRequired='True']", "eForm").each(function (i) {
                if ($.trim(this.value) == "") {
                    alert("必填字段不允许为空！");
                    this.focus();
                    return;
                }
            });

            var value = JSON2.stringify(getFormValue("eForm"))
            $.post(getCurrentUrl(), { AjaxAction: "Submit", AjaxArgument: value, DataSource: $("#hidDataSource").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("提交成功！");
                    $("#hidCurrentId").val(ajaxResult.RetValue);
                    if ($.query.get("Entry") != "StartProcess") {
                        window.parent.parent.$("#actionDialog").dialog("close");
                        window.parent.parent.frames["ifrContentPage"].location.reload();
                    } else {
                        window.parent.$("#actionDialog").dialog("close");
                    }
                }
                else {
                    alert("提交失败，请检查输入是否有误！");
                }
            });
        }

        function openChooseBoxDialog(title, url, width, height) {
            url = url + "&Entry=choose";
            window.parent.parent.openDialog("actionDialog2", title, url, 800, 480, true, 0, 70);
        }

        function cancel() {
            if ($.query.get("Entry") != "StartProcess") {
                window.parent.parent.$("#actionDialog").dialog("close");
            }
            else {
                window.parent.$("#actionDialog").dialog("close");
            }
        }

        function save() {
            $("input[isRequired='True']", "eForm").each(function (i) {
                if ($.trim(this.value) == "") {
                    alert("必填字段不允许为空！");
                    this.focus();
                    return;
                }
            });

            var value = JSON2.stringify(getFormValue("eForm"))
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, DataSource: $("#hidDataSource").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("保存成功！");
                    $("#hidCurrentId").val(ajaxResult.RetValue);
                    if ($.query.get("Entry") != "StartProcess") {
                        window.parent.parent.$("#actionDialog").dialog("close");
                        window.parent.parent.frames["ifrContentPage"].location.reload();
                    } else {
                        window.parent.$("#actionDialog").dialog("close");
                    }
                }
                else {
                    alert("保存失败，请检查输入是否有误！");
                }
            });
        }

        function rollback() {
            var isOK = confirm("你确定要退回该工作项吗？");
            if (!isOK) return;

            var value = JSON2.stringify(getFormValue("eForm"))
            $.post(getCurrentUrl(), { AjaxAction: "Rollback", AjaxArgument: value, DataSource: $("#hidDataSource").val() }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("退回成功！");
                    $("#hidCurrentId").val(ajaxResult.RetValue);
                    if ($.query.get("Entry") != "StartProcess") {
                        window.parent.parent.$("#actionDialog").dialog("close");
                        window.parent.parent.frames["ifrContentPage"].location.reload();
                    } else {
                        window.parent.$("#actionDialog").dialog("close");
                    }
                }
                else {
                    alert("退回失败，请检查输入是否有误！");
                }
            });
        }
    </script>
    <asp:PlaceHolder ID="eForm" runat="server"></asp:PlaceHolder>
    <agile:HidTextBox ID="hidDataSource" runat="server" />
</asp:Content>
