<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Master/Page.Master"
    CodeBehind="ActivityDetail.aspx.cs" Inherits="AgileEAP.Plugin.Workflow.ActivityDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%=HtmlExtensions.RequireStyles("tab_default") %>
    <%=HtmlExtensions.RequireScripts("tab") %>
    <%if (false)
      { %>
    <script src="../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
    <%}%>
    <style type="text/css">
        /*<tr>*/
        .div_row
        {
            width: 100%;
            height: 25px;
        }
        #singleComboxContainer
        {
        	position:absolute;
        }
        /*<td> 标题  用作说明*/
        .div_row_lable
        {
            width: 15%;
            text-align: left;
            float: left;
            vertical-align: text-bottom;
        }
        
        /*<td>  内容 用作用户输入*/
        .div_row_input
        {
            text-align: left;
            float: left;
        }
        .div_row_left
        {
            text-align: left;
            width: 100%;
            height: 25px;
        }
        .btn_choose_form
        {
            cursor: pointer;
            background: url("/Plugins/Workflow/Content/Themes/<%=Skin %>/Images/chooseActivityForm.png") center no-repeat;
        }
        .btn_form_control_config
        {
            cursor: pointer;
            background: url("/Plugins/Workflow/Content/Themes/<%=Skin %>/Images/formControlConfig.gif") center no-repeat;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <script type="text/javascript" language="javascript">

        //校验脚本
        function validator() {
            //$("#<%=txtFinishRequiredPercent.ClientID%>").formValidator({ forcevalid: true, triggerevent: "change", onshow: "请输入的百分比（0-100之间）", onfocus: "只能输入0-100之间的数字" }).inputValidator({ min: 0, max: 100, type: "value", onerrormin: "你输入的值必须大于等于0", onerror: "年龄必须在0-100之间，请确认" });
            //             $("#<%=txtSpecifyURL.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【调用URL】不能为空！" });  
            //             $("#<%=txtID.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【活动ID】不能为空！" });  
            //             $("#<%=txtName.ClientID%>").formValidator().inputValidator({ min: 1, onerror: "【活动名称】不能为空！" });  
        }
        //初始化界面值
        function initUI() {
            var tabs = $('#tabcontainer').find("li");
            var typeName = "<%=TypeName %>";

            var removeTab = function (remove) {
                tabs.each(function (i) {
                    if (i > 0) {
                        $("#tab" + this.id).hide();
                        if (remove) $(this).remove();
                    }
                    else if (remove) {
                        this.className = "tabBg7";
                    }
                });
            };

            if (typeName == "StartActivity") {
                removeTab(true);
                $("#<%=lable_joinType.ClientID %>").hide();
                $("#<%=input_joinType.ClientID %>").hide()
                $("#<%=row_propertyandproxy.ClientID %>").hide();
                $("#row_businessSet").hide();
                $("#<%=row_isSpecifyURL.ClientID %>").hide();
                $("#<%=row_urltype.ClientID %>").hide();
                $("#<%=row_CustomizeURL.ClientID %>").hide();
            } else if (typeName == "EndActivity") {
                removeTab(true);
                $("#<%=lable_splitType.ClientID %>").hide();
                $("#<%=input_splitType.ClientID %>").hide()
                $("#<%=row_propertyandproxy.ClientID %>").hide();
                $("#row_businessSet").hide();
                $("#<%=row_isSpecifyURL.ClientID %>").hide();
                $("#<%=row_urltype.ClientID %>").hide();
                $("#<%=row_CustomizeURL.ClientID %>").hide();
            }
            else if (typeName == "AutoActivity") {
                removeTab(true);
                $("#<%=row_propertyandproxy.ClientID %>").hide();
             }
            else {
                removeTab(false);
            }
        }

        function remindlimittime() {
            if ($("#<%=rabRemindLimtTime.ClientID %>").attr("checked")) {
                $("#<%=txtRemindLimtTimeHour.ClientID %>").attr({ disabled: '' });
                $("#<%=txtRemindLimtTimeMinute.ClientID %>").attr({ disabled: '' });
            }
            else {
                $("#<%=txtRemindLimtTimeHour.ClientID %>").attr({ disabled: 'disabled' });
                $("#<%=txtRemindLimtTimeMinute.ClientID %>").attr({ disabled: 'disabled' });
            }
        }

        function enableFreeActivity(check) {
            if ($("#" + check).attr("checked")) {
                $(":input", $("#freeactivity")).attr({ disabled: '' });
            } else {
                $(":input", $("#freeactivity")).attr({ disabled: 'disabled' });
            }
        }

        function enableStrategy(check) {
            if ($("#" + check).attr("checked")) {
                $(":input", $("#strategy")).attr({ disabled: '' });
            } else {
                $(":input", $("#strategy")).attr({ disabled: 'disabled' });
            }
        }

        function enableMulwi(check) {
            if ($("#" + check).attr("checked")) {
                $(":input", $("#mulwi")).attr({ disabled: '' });
            } else {
                $(":input", $("#mulwi")).attr({ disabled: 'disabled' });
            }
        }
        function limittime() {
            if ($("#<%=rabTimeLimitStrategy.ClientID %>").attr("checked")) {
                $("#<%=txtLimitTimeHour.ClientID %>").attr({ disabled: '' });
                $("#<%=txtLimitTimeMinute.ClientID %>").attr({ disabled: '' });
            }
            else {
                $("#<%=txtLimitTimeHour.ClientID %>").attr({ disabled: 'disabled' });
                $("#<%=txtLimitTimeMinute.ClientID %>").attr({ disabled: 'disabled' });
            }
        }
        function specifyurl(check) {
            var typeName = "<%=TypeName %>";
            if ($("#" + check).attr("checked")) {
                $("#<%=txtSpecifyURL.ClientID %>").attr({ disabled: '' }).unFormValidator(false);
                $("#em_specifyurl").show();
            } else {
                $("#<%=txtSpecifyURL.ClientID %>").attr({ disabled: 'disabled' }).unFormValidator(true);
                $("#em_specifyurl").hide();

            }
        }
        function sharer() {
            $(":input", $("#orgorrole")).attr({ disabled: 'disabled' });
            $("#<%=ckbIsAllowAppointParticipants.ClientID %>").attr({ disabled: 'disabled' });
            $("#<%=txtspecialActivity.ClientID %>").attr({ disabled: 'disabled' });
            $("#<%=txtspecialPath.ClientID %>").attr({ disabled: 'disabled' });
            $("#<%=txtRegularApp.ClientID %>").attr({ disabled: 'disabled' });
            if ($("#<%=rblOrganization.ClientID %>").attr("checked")) {
                $(":input", $("#orgorrole")).attr({ disabled: '' });
                $("#<%=ckbIsAllowAppointParticipants.ClientID %>").attr({ disabled: '' });
            }
            if ($("#<%=rblSpecialActivity.ClientID %>").attr("checked")) {
                $("#<%=txtspecialActivity.ClientID %>").attr({ disabled: '' });
            }
            if ($("#<%=rblRelevantData.ClientID %>").attr("checked")) {
                $("#<%=txtspecialPath.ClientID %>").attr({ disabled: '' });
            }
            if ($("#<%=rblRegular.ClientID %>").attr("checked")) {
                $("#<%=txtRegularApp.ClientID %>").attr({ disabled: '' });
            }
        }
        //添加参与者 change by【hgq】
        function addParticipantor() {
            var participantors = openOperateDialog("添加参与者", "ChooseParticipantor.aspx", 980, 700, true, 1);
            if (participantors == null) return;
            if (participantors.length != 0) {
                var table = $("table", "#gvOrgizationOrRole_container");
                var tbody = $("tbody", table);
                if ($("thead", table).length == 0) {//没有数据时
                    $(tbody).before("<thead></thead>");
                    $("thead", table).append($(tbody).html());
                    $(table).attr({ "cellspacing": "0", "border": "1", "id": "ctl00_contentPlace_gvList" });
                    $(table).css("border-collapse", "collapse");
                    $(tbody).empty();
                }
                var index = $("tr", tbody).length;
                var count = participantors.length;
                var strHtml = ""
                for (var i = 0; i < participantors.length; i++) {
                    if ($("tr", tbody).find("td:contains('" + participantors[i].id + "')").length > 0)//存在的不添加
                        continue;
                    strHtml += "<tr onmouseover=\"rowOver(this);\" onmouseout=\"rowOut(this);\" style=\"cursor: hand\">"
                            + "<td ><input id='radioId' type='radio' value=''  name='radioId' /></td>"
					        + "<td>" + (count + index - i) + "</td><td>" + participantors[i].id + "</td><td>" + participantors[i].name
                            + "</td><td>" + participantors[i].type + "</td></tr>";
                }
                $(tbody).prepend(strHtml);
                changIndex(table);
            }
        }
        //改变序号 add by [hgq]
        function changIndex(table) {
            var trs = $("tr", table);
            $(trs).each(function (i) {
                $("td", this).slice(1, 2).text(i);
            });
        }

        $(document).ready(function () {
            initUI();
        });
        function addParameter(me, argument) {
            var tblParameter = $("#tblParameter");
            var rowCount = $("#tblParameter").find("tr").length;

            $(":radio", tblParameter).removeAttr("checked");
            var newRow = "<tr ondblclick=\"rowDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                            + "<td  style='width:5%;'><input id='radioId' type='radio' value='' checked='checked' name='radioId' /></td>"
                            + "<td  style='width:15%;'><input type='text' style='width:98%'/></td>"
                            + "<td  style='width:15%;'><input type='text' style='width:98%'/></td>"
                            + "<td  style='width:12%;'><select style='width:98%'><option>整数</option><option>浮点数</option><option>日期</option><option selected='selected'>字符串</option></select></td>"
                            + "<td  style='width:13%;'><select style='width:98%'><option selected='selected'>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>单选项列表</option><option>多选项列表</option><option>复选框</option><option>选择框</option><option>选择树</option></select></td>"
                            + "<td  style='width:5%;' flag='checkbox'><input type='checkbox'></input></td>"
                            + "<td  style='width:15%;'><input type='text' style='width:98%'/></td>"
                            + "<td  style='width:10%;'><select style='width:98%'><option selected='selected'>只读</option><option>读写</option></select></td>"
                            + "<td  style='width:10%' flag='operate' ><span class='btn_up' title='上移' onclick=\"executeOperate(this,'up')\">&nbsp;&nbsp;&nbsp;</span> ｜ <span class='btn_down' title='下移' onclick=\"executeOperate(this,'down')\">&nbsp;&nbsp;&nbsp;</span>  ｜ <span class=\"btn_form_control_config\" title='配置' onclick=\"configureField(this)\">&nbsp;&nbsp;&nbsp;&nbsp;</span></td>"
                            + "</tr>";
            tblParameter.append(newRow);
        }

        function chooseParameter() {
            var form = openOperateDialog("选择表单来源", "<%=RootPath %>Workflow/ActivityManager.aspx?Entry=ChooseActivity&ProcessDefID=" + $.query.get("ProcessDefID"), 550, 300, true, 1);
            if (!form || !form.Fields || form.Fields.length == 0) return;

            $("#<%=txtDataSource.ClientID %>").val(form.DataSource);
            //$("#<%=txtTitle.ClientID %>").val(form.Title);
            var fields = form.Fields;
            var table = $("#tblParameter");
            var tbody = table.find("tbody");
            tbody.empty();
            var strHtml = ""
            for (var i = 0; i < fields.length; i++) {
                strHtml += "<tr ondblclick=\"rowDbClick(this,'',true)\" onmouseout=\"rowOut(this);\" onmouseover=\"rowOver(this);\" onclick=\"rowClick(this,'',true)\" style=\"cursor: pointer\">"
                               + "<td style=\"text-align: center\"><input id=\"radioId\" type=\"radio\" value='' name=\"radioId\" /></td>"
                               + "<td>" + fields[i].Text + "</td>"
                               + "<td>" + fields[i].Name + "</td>"
                               + "<td>" + fields[i].DataType + "</td>"
                               + "<td>" + fields[i].ControlType + "</td>"
                               + "<td>" + fields[i].Required + "</td>"
                               + "<td>" + fields[i].DefaultValue + "</td>"
                               + "<td>" + fields[i].AccessPattern + "</td>"
                               + "<td flag=\"operate\">"
                               + "         <span class=\"btn_up\" title='上移' onclick=\"executeOperate(this,'up')\">&nbsp;&nbsp;&nbsp;</span>"
                               + "      ｜ <span class=\"btn_down\" title='下移' onclick=\"executeOperate(this,'down')\">&nbsp;&nbsp;&nbsp;</span>"
                               + "</td></tr>";
            }
            $(tbody).append(strHtml);
        }

        function addTriggerEvent(me, argument) {
            var tblTriggerEvent = $("#tblTriggerEvent");
            var rowCount = $("#tblTriggerEvent").find("tr").length;

            $(":radio", tblTriggerEvent).removeAttr("checked");
            var newRow = "<tr ondblclick=\"rowEventDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                            + "<td  style='width:10%;'><input id='radioId' type='radio' value='' checked='checked' name='radioId' /></td>"
                            + "<td  style='width:30%;'><select style='width:98%'><option>活动创建前</option><option selected='selected'>活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select></td>"
                           + "<td  style='width:40%;'><input type='text' style='width:98%'/></td>"
                           + "<td  style='width:20%;'><select style='width:98%'><option selected='selected'>同步</option><option>异步</option></select></td></tr>";
            tblTriggerEvent.append(newRow);
        }
        function addFreeRange(me, argument) {
            var tblFreeRange = $("#tblFreeRange");
            var rowCount = $("#tblFreeRange").find("tr").length;

            $(":radio", tblFreeRange).removeAttr("checked");
            var newRow = "<tr ondblclick=\"rowFreeRangeDbClick(this,'',true)\" onmouseout='rowOut(this);' onclick=\"rowClick(this,'',true)\" style='cursor: hand'>"
                            + "<td  style='width:10%;text-align:center;'><input id='radioId' type='radio' value='' checked='checked' name='radioId' /></td>"
                            + "<td  style='width:45%;'><input type='text' style='width:98%'/></td>"
                           + "<td  style='width:45%;'><input type='text' style='width:98%'/></td></tr>";
            tblFreeRange.append(newRow);
        }
        var cacheArray = new Array();
        function rowFreeRangeDbClick(me, id, remember) {
            if ($("input[type!=radio]", me).eq(0)[0]) return;
            $("td", me).each(function (i) {
                var flag = $(this).attr("flag");
                if (i > 0) {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' value='" + innerText + "' style='width:98%;'/>");
                    cacheArray[i] = innerText;
                }
            });
        }
        function rowOut(me) {
            if (oldRow != null) {
                if (currentRow != me) {
                    me.className = "rowout";
                }
                oldRow = me;
            }
        }
        function delFreeRange(me) {
            if (!confirm("是否确定删除记录？")) {
                return false;
            }
            var rad = $("#tblFreeRange").find(":checked ").first(); //.val();
            rad.parent().parent().remove();
            return;
        }
        function delTriggerEvent(me) {
            if (!confirm("是否确定删除记录？")) {
                return false;
            }
            var rad = $("#tblTriggerEvent").find(":checked ").first(); //.val();
            var id = rad.val();
            rad.parent().parent().remove();
            return;
        }
        function delParticipantor(me) {
            if (!confirm("是否确定删除记录？")) {
                return false;
            }
            var table = $("table", "#gvOrgizationOrRole_container");
            var rad = table.find(":checked ").first(); //.val();
            rad.parent().parent().remove();
            changIndex(table);
            return;
        }
        function delParameter(me) {
            if (!confirm("是否确定删除记录？")) {
                return false;
            }
            var rad = $("#tblParameter").find(":checked ").first(); //.val();
            rad.parent().parent().remove();
            return;
        }

        var cacheArray = new Array();
        function rowEventDbClick(me, id, remember) {
            if ($("input[type!=radio]", me).eq(0)[0]) return;
            $("td", me).each(function (i) {
                var flag = $(this).attr("flag");
                if (i == 2) {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' value='" + innerText + "' style='width:98%;'/>");
                    cacheArray[i] = innerText;
                }
                if (i == 3)//调用方式
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "同步") {
                        $(this).append("<select style='width:98%'><option selected='selected'>同步</option><option>异步</option></select> ");
                    }
                    else {
                        $(this).append("<select style='width:98%'><option>同步</option><option selected='selected'>异步</option></select> ");
                    }
                    cacheArray[i] = innerText;
                }
                else if (i == 1)//触发时机
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "活动创建前") {
                        $(this).append("<select style='width:98%'><option selected='selected'>活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动启动前") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option selected='selected'>活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动启动后") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option selected='selected'>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动超时后") {
                        $(this).append("<select style='width:98%'><option>活动创建前</option><option >活动启动前</option><option>活动启动后</option><option  selected='selected'>活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动终止后") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option selected='selected'>活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动完成后") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option selected='selected'>活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "活动提醒前") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option selected='selected'>活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项创建前") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option selected='selected'>工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项创建后") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option selected='selected'>工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项执行时") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option selected='selected'>工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项完成后") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option selected='selected'>工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项执行出错时") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option selected='selected'>工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项取消") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option selected='selected'>工作项取消</option><option >工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项超时") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option selected='selected'>工作项超时</option><option >工作项挂起</option></select>");
                    }
                    else if (innerText == "工作项挂起") {
                        $(this).append("<select style='width:98%'><option >活动创建前</option><option >活动启动前</option><option>活动启动后</option><option >活动超时后</option><option >活动终止后</option><option >活动完成后</option><option >活动提醒前</option><option >工作项创建前</option><option >工作项创建后</option><option >工作项执行时</option><option >工作项完成后</option><option >工作项执行出错时</option><option >工作项取消</option><option >工作项超时</option><option selected='selected'>工作项挂起</option></select>");
                    }
                    cacheArray[i] = innerText;
                }
            });
        }

        function rowClick(me, id, remember) {
            if (currentRow) {
                currentRow.className = "gridview_row";
            }
            me.className = "rowcurrent";

            if (currentRow && currentRow != me) {
                $(currentRow).find("td").each(function (i) {
                    var input = $(this).find(":input").eq(0);
                    if (input.attr("type") == "checkbox") {
                        var value = input.attr("checked") == "checked" ? "是" : "否";
                        //$(this).empty();
                        $(this).text(value);
                    }
                    else if (input[0] && input.attr("type") != "radio") {
                        var value = input.val();
                        //$(this).empty();
                        $(this).text(value);
                    }
                });
            }
            currentRow = me;
            $("input:radio", me).attr("checked", true);
        }
        function swapRow(srcRow, destRow) {
            if (srcRow[0] && destRow[0]) {
                var destCells = destRow.find("td[flag!='operate']");
                srcRow.find("td[flag!='operate']").each(function (i) {
                    if (i > 0) {
                        var tmp = $.trim($(this).text());
                        $(this).text($.trim(destCells.eq(i).text()));
                        destCells.eq(i).text(tmp);
                    }
                });
            }
        }

        function executeOperate(me, operate) {
            var currentRow = $(me).parent().parent();
            if (operate == "delete") {
                currentRow.remove();
            } else if (operate == "up") {
                var prev = currentRow.prev();
                swapRow(prev, currentRow);
            } else if (operate == "down") {
                var next = currentRow.next();
                swapRow(next, currentRow);
            }

            return false;
        }

        function configureField(me) {
            var parentNode = $(me).parent().parent();
            var controlNode = parentNode.find("td").eq(4);
            var controlType = $.trim(controlNode.text());
            if (controlType == "选择框") {
                var field = openOperateDialog("设置" + controlType + "属性", "<%=RootPath %>Workflow/FormControlConfigure.aspx?type=ChooseBox&url=" + controlNode.attr("url"), 550, 300, true, 1);
                controlNode.attr("url", field.url);
            }
            else if (controlType == "文本") {
                var rows = controlNode.attr("rows") || 1;
                var cols = controlNode.attr("cols") || 1;
                var field = openOperateDialog("设置" + controlType + "属性", "<%=RootPath %>Workflow/FormControlConfigure.aspx?type=TextBox&rows=" + rows + "&cols=" + cols, 550, 300, true, 1);
                controlNode.attr("rows", field.rows || 1);
                controlNode.attr("cols", field.cols || 1);
            }
            else if (controlType == "单选项列表" || controlType == "多选项列表") {
                var datasource = openOperateDialog("设置" + controlType + "属性", "<%=RootPath %>Infrastructure/ChooseDict.aspx", 550, 300, true, 1);
                if (datasource)
                    controlNode.attr("datasource", datasource);
            }
        }

        var cacheArray = new Array();
        function rowDbClick(me, id, remember) {
            if ($("input[type!=radio]", me).eq(0)[0]) return;

            $("td", me).each(function (i) {
                var flag = $(this).attr("flag");
                if ((i > 0 && i <= 2 && flag != "checkbox" && flag != "operate") || i == 6) {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    $(this).append("<input type='text' value='" + innerText + "' style='width:98%;'/>");
                    cacheArray[i] = innerText;
                }
                if (i == 3)//类型
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "整数") {
                        $(this).append("<select style='width:98%' selected='selected'><option>整数</option><option>浮点数</option><option>日期</option><option>字符串</option></select> ");
                    }
                    else if (innerText == "浮点数") {
                        $(this).append("<select style='width:98%'><option>整数</option><option selected='selected'>浮点数</option><option>日期</option><option>字符串</option></select> ");
                    }
                    else if (innerText == "日期") {
                        $(this).append("<select style='width:98%'><option>整数</option><option>浮点数</option><option selected='selected'>日期</option><option>字符串</option></select> ");
                    }
                    else if (innerText == "字符串") {
                        $(this).append("<select style='width:98%'><option>整数</option><option>浮点数</option><option selected='selected'>日期</option><option selected='selected'>字符串</option></select> ");
                    }
                    cacheArray[i] = innerText;
                }
                else if (i == 4)//控件
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "文本") {
                        $(this).append("<select style='width:98%'><option selected='selected'>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "日期控件") {
                        $(this).append("<select style='width:98%'><option>文本</option selected='selected'><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "月份控件") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option selected='selected'>月份控件</option><option>月份范围控件</option><option>年份控件</option><option>多选项列表</option><option>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "年份控件") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option selected='selected'>年份控件</option><option>多选项列表</option><option>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "多选项列表") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option selected='selected'>多选项列表</option><option>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "复选框") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option  selected='selected'>复选框</option><option>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "单选项列表") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option >复选框</option><option selected='selected'>单选项列表</option><option>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "选择框") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option >复选框</option><option>单选项列表</option><option selected='selected'>选择框</option><option>选择树</option></select>");
                    }
                    else if (innerText == "选择树") {
                        $(this).append("<select style='width:98%'><option>文本</option><option>日期控件</option><option>月份控件</option><option>年份控件</option><option>多选项列表</option><option >复选框</option><option>单选项列表</option><option >选择框</option><option selected='selected'>选择树</option></select>");
                    }
                    cacheArray[i] = innerText;
                }
                else if (i == 5)//必填
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "是") {
                        $(this).append("<input type='checkbox' checked='checked'></input>");
                    }
                    else {
                        $(this).append("<input type='checkbox'></input>");
                    }
                    cacheArray[i] = innerText;
                }
                else if (i == 7)//访问方式
                {
                    var innerText = $.trim($(this).text());
                    $(this).text("");
                    if (innerText == "只读") {
                        $(this).append("<select style='width:98%'><option selected='selected'>只读</option><option>读写</option></select>");
                    }
                    else {
                        $(this).append("<select style='width:98%'><option>只读</option><option selected='selected'>读写</option></select>");
                    }
                    cacheArray[i] = innerText;
                }
            });
        }
        function save(me, argument) {
            //if (!$.formValidator.pageIsValid("1")) return false;
            var activityDetail = getObjectValue("main_bg2");
            //参与者
            var tblParticipantor = $("table", "#gvOrgizationOrRole_container");
            var participantorItems = new Array();
            $("tr", tblParticipantor).each(function (i) {
                if (i > 0) {
                    var item = new Object();
                    var tds = $(this).find("td");
                    if (tds.length > 0) {
                        item["SortOrder"] = $.trim(tds.eq(1).text());
                        item["ID"] = $.trim(tds.eq(2).text());
                        item["Name"] = $.trim(tds.eq(3).text());
                        participantorType = $.trim(tds.eq(4).text());
                        if (participantorType == "角色") {
                            item["ParticipantorType"] = "Role";
                        }
                        else if (participantorType == "人员") {
                            item["ParticipantorType"] = "Person";
                        }
                        else if (participantorType == "组织") {
                            item["ParticipantorType"] = "Org";
                        }
                        participantorItems[i - 1] = item;
                    }
                }
            });
            activityDetail.Participant = new Object();
            activityDetail.Participant.Participantors = participantorItems;

            if ($("#<%=rblOrganization.ClientID %>").attr("checked")) {
                activityDetail.Participant.AllowAppointParticipants = $("#<%=ckbIsAllowAppointParticipants.ClientID %>").attr("checked");
                activityDetail.Participant.ParticipantType = "Participantor";
            }
            else if ($("#<%=rblProcessStarter.ClientID %>").attr("checked")) {
                activityDetail.Participant.ParticipantType = "ProcessStarter";
            }
            else if ($("#<%=rblSpecialActivity.ClientID %>").attr("checked")) {
                activityDetail.Participant.ParticipantValue = $.trim($("#<%=txtspecialActivity.ClientID %>").val());
                activityDetail.Participant.ParticipantType = "ProcessExecutor";
            }
            else if ($("#<%=rblParticipantRule.ClientID %>").attr("checked")) {
                activityDetail.Participant.ParticipantValue = $.trim($("#<%=txtParticipantRule.ClientID %>").val());
                activityDetail.Participant.ParticipantType = "CustomRegular";
            }
            else if ($("#<%=rblRelevantData.ClientID %>").attr("checked")) {
                activityDetail.Participant.ParticipantValue = $.trim($("#<%=txtspecialPath.ClientID %>").val());
                activityDetail.Participant.ParticipantType = "RelevantData";
            }
            else if ($("#<%=rblRegular.ClientID %>").attr("checked")) {
                activityDetail.Participant.ParticipantValue = $.trim($("#<%=txtRegularApp.ClientID %>").val());
                activityDetail.Participant.ParticipantType = "RelateRegular";
            }

            //表单的参数配置
            var tblParameter = $("#tblParameter");
            var parameterItems = new Array();
            $("tr", tblParameter).each(function (i) {
                if (i > 0) {
                    var item = new Object();
                    var tds = $(this).find("td");
                    tds.each(function (i) {
                        var input = $(this).find(":input").eq(0);
                        if (input.attr("type") == "checkbox") {
                            var value = input.attr("checked") == "checked" ? "是" : "否";
                            $(this).text(value);
                        }
                        else if (input[0] && input.attr("type") != "radio") {
                            $(this).text(input.val());
                        }
                    });
                    if (tds.length > 0) {

                        item["Text"] = $.trim(tds.eq(1).text());
                        item["Name"] = $.trim(tds.eq(2).text());
                        var dataType = $.trim(tds.eq(3).text());
                        if (dataType == "整数")
                            item["DataType"] = 1;
                        else if (dataType == "浮点数")
                            item["DataType"] = 2;
                        else if (dataType == "日期")
                            item["DataType"] = 3;
                        else
                            item["DataType"] = 4;
                        var controlType = $.trim(tds.eq(4).text());
                        if (controlType == "文本") {
                            item["ControlType"] = 1;
                            item["Rows"] = parseInt(tds.eq(4).attr("rows")) || 1;
                            item["Cols"] = parseInt(tds.eq(4).attr("cols")) || 1;
                        }
                        else if (controlType == "日期控件")
                            item["ControlType"] = 2;
                        else if (controlType == "日期范围控件")
                            item["ControlType"] = 3;
                        else if (controlType == "月份控件")
                            item["ControlType"] = 4;
                        else if (controlType == "月份范围控件")
                            item["ControlType"] = 5;
                        else if (controlType == "年份控件")
                            item["ControlType"] = 6;
                        else if (controlType == "单选项列表") {
                            item["ControlType"] = 7;
                            item["DataSource"] = tds.eq(4).attr("datasource");
                        }
                        else if (controlType == "多选项列表") {
                            item["ControlType"] = 8;
                            item["DataSource"] = tds.eq(4).attr("datasource");
                        }
                        else if (controlType == "复选框")
                            item["ControlType"] = 9;
                        else if (controlType == "单选")
                            item["ControlType"] = 10;
                        else if (controlType == "选择框") {
                            item["ControlType"] = 11;
                            item["URL"] = $.trim(tds.eq(4).attr("url"));
                        }
                        else if (controlType == "选择树")
                            item["ControlType"] = 12;
                        else if (controlType == "文本范围")
                            item["ControlType"] = 13;
                        item["Required"] = $.trim(tds.eq(5).text()) == "是" ? true : false;
                        item["DefaultValue"] = $.trim(tds.eq(6).text());
                        item["AccessPattern"] = $.trim(tds.eq(7).text()) == "只读" ? 1 : 2;
                        parameterItems[i - 1] = item;
                    }
                }
            });
            activityDetail.Form = new Object();
            activityDetail.Form.Fields = parameterItems;
            activityDetail.Form.DataSource = $.trim($("#<%=txtDataSource.ClientID %>").val());
            activityDetail.Form.Title = $.trim($("#<%=txtTitle.ClientID %>").val());

            //基本
            activityDetail.AllowAgent = $("#<%=chbAllowAgent.ClientID %>").attr("checked");
            activityDetail.IsSplitTransaction = $("#<%=chbIsSplitTransaction.ClientID %>").attr("checked");
            activityDetail.CustomURL = new Object();
            activityDetail.CustomURL.ManualProcess = new Object();
            if ($("#<%=rbCustomizeURL.ClientID %>").attr("checked")) {
                activityDetail.CustomURL.SpecifyURL = $.trim($("#<%=txtSpecifyURL.ClientID %>").val());
                activityDetail.CustomURL.URLType = "CustomURL";
            }
            else if ($("#<%=rbTask.ClientID %>").attr("checked")) {
                activityDetail.CustomURL.ManualProcess.ApplicationUri = "";
                activityDetail.CustomURL.URLType = "ManualProcess";
            }
            else {
                activityDetail.CustomURL.URLType = "DefaultURL";
            }



            //时间限制
            activityDetail.TimeLimit = new Object();
            activityDetail.TimeLimit.TimeLimitInfo = new Object();
            activityDetail.TimeLimit.RemindInfo = new Object();
            activityDetail.TimeLimit.IsTimeLimitSet = $("#<%=chbIsTimeLimitSet.ClientID %>").attr("checked");
            if ($("#<%=rabTimeLimitStrategy.ClientID %>").attr("checked")) {
                activityDetail.TimeLimit.TimeLimitInfo.LimitTimeHour = parseInt($.trim($("#<%=txtLimitTimeHour.ClientID %>").val()));
                activityDetail.TimeLimit.TimeLimitInfo.LimitTimeMinute = parseInt($.trim($("#<%=txtLimitTimeMinute.ClientID %>").val()));
                activityDetail.TimeLimit.TimeLimitInfo.TimeLimitStrategy = "LimitTime";
            }
            else if ($("#<%=rabRelevantLimitTime.ClientID %>").attr("checked")) {
                activityDetail.TimeLimit.TimeLimitInfo.RelevantData = $.trim($("#<%=txtRelevantData.ClientID %>").val());
                activityDetail.TimeLimit.TimeLimitInfo.TimeLimitStrategy = "RelevantLimitTime";
            }
            activityDetail.TimeLimit.TimeLimitInfo.IsSendMessageForOvertime = $("#<%=chbIsSendMessageForOvertime.ClientID %>").attr("checked");
            if ($("#<%=rabRemindLimtTime.ClientID %>").attr("checked")) {
                activityDetail.TimeLimit.RemindInfo.RemindLimtTimeHour = parseInt($.trim($("#<%=txtRemindLimtTimeHour.ClientID %>").val()));
                activityDetail.TimeLimit.RemindInfo.RemindLimtTimeMinute = parseInt($.trim($("#<%=txtRemindLimtTimeMinute.ClientID %>").val()));
                activityDetail.TimeLimit.RemindInfo.RemindStrategy = "RemindLimtTime";
            }
            else if ($("#<%=rabRemindRelevantLimitTime.ClientID %>").attr("checked")) {
                activityDetail.TimeLimit.RemindInfo.RemindRelevantData = $.trim($("#<%=txtRemindRelevantData.ClientID %>").val());
                activityDetail.TimeLimit.RemindInfo.RemindStrategy = "RemindRelevantLimitTime";
            }
            activityDetail.TimeLimit.RemindInfo.IsSendMessageForRemind = $("#<%=chbisSendMessageForRemind.ClientID %>").attr("checked");

            //多工作项
            activityDetail.MultiWorkItem = new Object();
            activityDetail.MultiWorkItem.IsMulWIValid = $("#<%=chbIsMulWIValid.ClientID %>").attr("checked");
            if ($("#<%=rblParticipantNumber.ClientID %>").attr("checked")) {
                activityDetail.MultiWorkItem.WorkItemNumStrategy = "ParticipantNumber";
            }
            else {
                activityDetail.MultiWorkItem.WorkItemNumStrategy = "OperatorNumber";
            }
            activityDetail.MultiWorkItem.IsSequentialExecute = $("#<%=rabYIsSequentialExecute.ClientID %>").attr("checked");

            if ($("#<%=rblFinishAll.ClientID %>").attr("checked")) {
                activityDetail.MultiWorkItem.FinishRule = "FinishAll";
            }
            else if ($("#<%=rblSpecifyNum.ClientID %>").attr("checked")) {
                activityDetail.MultiWorkItem.FinishRquiredNum = parseInt($.trim($("#<%=txtFinishRquiredNum.ClientID %>").val()));
                activityDetail.MultiWorkItem.FinishRule = "SpecifyNum";
            }
            else {
                activityDetail.MultiWorkItem.FinishRule = "SpecifyPercent";
                activityDetail.MultiWorkItem.FinishRequiredPercent = parseFloat($.trim($("#<%=txtFinishRequiredPercent.ClientID %>").val()));
            }
            activityDetail.MultiWorkItem.IsAutoCancel = $("#<%=rabYIsAutoCancel.ClientID %>").attr("checked");

            //触发事件
            var tblTriggerEvent = $("#tblTriggerEvent");
            var eventItems = new Array();
            $("tr", tblTriggerEvent).each(function (i) {
                if (i > 0) {
                    var item = new Object();
                    var tds = $(this).find("td");
                    tds.each(function (i) {
                        var text = $(":input", $(this)).eq(0);
                        if (text[0] && text.attr("type") != "radio") {
                            var innerText = text.val();
                            $(this).empty();
                            $(this).text(innerText);
                        }
                    });
                    if (tds.length > 0) {
                        item["ID"] = $.trim($(this).attr("id"));
                        TriggerEventType = $.trim(tds.eq(1).text());
                        if (TriggerEventType == "活动创建前")
                            item["TriggerEventType"] = 1;
                        else if (TriggerEventType == "活动启动前")
                            item["TriggerEventType"] = 2;
                        else if (TriggerEventType == "活动启动后")
                            item["TriggerEventType"] = 3;
                        else if (TriggerEventType == "活动超时后")
                            item["TriggerEventType"] = 4;
                        else if (TriggerEventType == "活动终止后")
                            item["TriggerEventType"] = 5;
                        else if (TriggerEventType == "活动完成后")
                            item["TriggerEventType"] = 6;
                        else if (TriggerEventType == "活动提醒前")
                            item["TriggerEventType"] = 7;
                        else if (TriggerEventType == "工作项创建前")
                            item["TriggerEventType"] = 21;
                        else if (TriggerEventType == "工作项创建后")
                            item["TriggerEventType"] = 22;
                        else if (TriggerEventType == "工作项执行时")
                            item["TriggerEventType"] = 23;
                        else if (TriggerEventType == "工作项完成后")
                            item["TriggerEventType"] = 24;
                        else if (TriggerEventType == "工作项执行出错时")
                            item["TriggerEventType"] = 25;
                        else if (TriggerEventType == "工作项取消")
                            item["TriggerEventType"] = 26;
                        else if (TriggerEventType == "工作项超时")
                            item["TriggerEventType"] = 27;
                        else if (TriggerEventType == "工作项挂起")
                            item["TriggerEventType"] = 28;
                        item["EventAction"] = $.trim(tds.eq(2).text());
                        var InvokePattern = $.trim(tds.eq(3).text());
                        if (InvokePattern == "同步")
                            item["InvokePattern"] = 1;
                        else
                            item["InvokePattern"] = 2;
                        eventItems[i - 1] = item;

                    }
                }
            });
            activityDetail.TriggerEvents = eventItems;
            //回退
            activityDetail.RollBack = new Object();
            activityDetail.RollBack.ActionPattern = activityDetail.RollbackType;
            activityDetail.RollBack.ApplicationUri = $.trim($("#<%=txtRollbackAction.ClientID %>").val());

            //自由流
            var tblFreeRange = $("#tblFreeRange");
            var freeRangeItems = new Array();
            $("tr", tblFreeRange).each(function (i) {
                if (i > 0) {
                    var item = new Object();
                    var tds = $(this).find("td");
                    tds.each(function (i) {
                        var text = $(":input", $(this)).eq(0);
                        if (text[0] && text.attr("type") != "radio") {
                            var innerText = text.val();
                            $(this).empty();
                            $(this).text(innerText);
                        }
                    });
                    if (tds.length > 0) {
                        item["ID"] = $.trim(tds.eq(1).text());
                        item["Name"] = $.trim(tds.eq(2).text());
                        freeRangeItems[i - 1] = item;

                    }
                }
            });
            activityDetail.FreeFlowRule = new Object();
            activityDetail.FreeFlowRule.FreeRangeActivities = freeRangeItems;
            activityDetail.FreeFlowRule.IsFreeActivity = $("#<%=chbIsFreeActivity.ClientID %>").attr("checked");
            if ($("#<%=rblFreeWithinProcess.ClientID %>").attr("checked")) {
                activityDetail.FreeFlowRule.FreeRangeStrategy = "FreeWithinProcess";
            }
            else if ($("#<%=rblFreeWithinActivities.ClientID %>").attr("checked")) {
                activityDetail.FreeFlowRule.FreeRangeStrategy = "FreeWithinActivities";
            }
            else {
                activityDetail.FreeFlowRule.FreeRangeStrategy = "FreeWithinNextActivites";
            }
            activityDetail.FreeFlowRule.IsOnlyLimitedManualActivity = $("#<%=chbIsOnlyLimitedManualActivity.ClientID %>").attr("checked");

            //启动策略
            activityDetail.ActivateRule = new Object();
            activityDetail.ResetURL = new Object();
            if ($("#<%=rblDirectRunning.ClientID %>").attr("checked")) {
                activityDetail.ActivateRule.ActivateRuleType = "DirectRunning";
            }
            else if ($("#<%=rblDisenabled.ClientID %>").attr("checked")) {
                activityDetail.ActivateRule.ActivateRuleType = "WaitActivate";
            }
            else {
                activityDetail.ActivateRule.ActivateRuleType = "AutoSkip";
            }
            activityDetail.ActivateRule.ActivateRuleApp = $.trim($("#<%=txtActivateRuleApp.ClientID %>").val());
            activityDetail.ResetParticipant = $("#<%=rbFirstParticipantor.ClientID %>").attr("checked") ? "FirstParticipantor" : "LastParticipantor";
            activityDetail.ResetURL.IsSpecifyURL = $("#<%=chbIsSpecifyURL.ClientID %>").attr("checked");
            activityDetail.ResetURL.URLType = activityDetail.URLType;
            activityDetail.ResetURL.SpecifyURL = $.trim($("#<%=txt2SpecifyURL.ClientID %>").val());

            var value = JSON2.stringify(activityDetail);
            $.post(getCurrentUrl(), { AjaxAction: "Save", AjaxArgument: value, ProcessDefID: $.query.get("ProcessDefID"), ActivityID: $.query.get("CurrentId") }, function (result) {
                var ajaxResult = JSON2.parse(result);
                if (ajaxResult && ajaxResult.Result == 1) {
                    alert("操作成功！");
                    $("#hidCurrentId").val(ajaxResult.RetValue);
                    window.parent.closeDialog(1, 1);
                }
                else {
                    alert("操作失败");
                }
            });

        }
    </script>
    <ul id="tabcontainer">
        <li id="basic" class="tabBg1" onclick="switchTab('basic');">基本</li>
        <li id="participantor" class="tabBg2" onclick="switchTab('participantor');">参与者</li>
        <li id="form" class="tabBg3" onclick="switchTab('form');">表单</li>
        <li id="limit" class="tabBg3" onclick="switchTab('limit');">限制</li>
        <li id="task" class="tabBg3" onclick="switchTab('task');">任务</li>
        <li id="event" class="tabBg3" onclick="switchTab('event');">事件</li>
        <li id="rollback" class="tabBg3" onclick="switchTab('rollback');">回退</li>
        <li id="freeActivity" class="tabBg3" onclick="switchTab('freeActivity');">自由流</li>
        <li id="activateRule" class="tabBg5" onclick="switchTab('activateRule');">策略</li>
    </ul>
    <div id="main_bg" style="height: 395px">
        <div id="main_bg2" style="height: 390px">
            <div id="tabbasic">
                <%--基本  --%>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtID.ClientID %>" class="label">
                            活动 ID
                        </label>
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtID" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <label for="<%=txtName.ClientID %>">
                            活动名称
                        </label>
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable" id="lable_joinType" runat="server">
                        <label for="<%=cboJoinType.ClientID %>">
                            聚合模式
                        </label>
                    </div>
                    <div class="div_row_input" id="input_joinType" runat="server">
                        <agile:Combox ID="cboJoinType" IsSingle="true" runat="server"></agile:Combox>
                    </div>
                    <div class="div_row_lable" id="lable_splitType" runat="server">
                        <label for="<%=cboSplitType.ClientID %>">
                            分支模式
                        </label>
                    </div>
                    <div class="div_row_input" id="input_splitType" runat="server">
                        <agile:Combox ID="cboSplitType" IsSingle="true" runat="server"></agile:Combox>
                    </div>
                </div>
                <div class="div_row" runat="server" id="row_propertyandproxy">
                    <div class="div_row_lable" id="lable_allowproxy" runat="server">
                        <label for="<%=chbAllowAgent.ClientID %>" class="label">
                            允许代理
                        </label>
                    </div>
                    <div class="div_row_input" id="input_allowproxy" runat="server">
                        <asp:CheckBox ID="chbAllowAgent" runat="server" />
                    </div>
                    <div class="div_row_lable" id="lable_priority" runat="server">
                        <label for="<%=chbIsSplitTransaction.ClientID %>" class="label">
                            分割事务
                        </label>
                    </div>
                    <div class="div_row_input" id="input_priority" runat="server">
                        <asp:CheckBox ID="chbIsSplitTransaction" runat="server" />
                    </div>
                </div>
                <div class="div_row" style="height: 120px;">
                    <div class="div_row_lable">
                        <label for="<%=txtDescription.ClientID %>">
                            描 述
                        </label>
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" Height="100px" runat="server"
                            Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row_left" id="row_businessSet">
                    业务设置
                </div>
                <div class="div_row" id="row_isSpecifyURL" runat="server">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rbDefaultURL" Text="默认URL" GroupName="BusinessSetting" Checked="true"
                            runat="server" />
                    </div>
                </div>
                <div class="div_row" id="row_urltype" runat="server">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rbTask" Text="人工任务" GroupName="BusinessSetting" runat="server" />
                    </div>
                </div>
                <div class="div_row" id="row_CustomizeURL" runat="server">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rbCustomizeURL" Text="自定义URL" GroupName="BusinessSetting" runat="server" /></div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox runat="server" ID="txtSpecifyURL" Width="100%" /></div>
                </div>
            </div>
            <div id="tabparticipantor">
                <%--参与者  --%>
                <div class="div_row">
                    <div class="div_row_lable" style="width: 20%">
                        <asp:RadioButton ID="rblOrganization" Text="从参与者列表获得" GroupName="ParticipantType"
                            onclick="sharer()" runat="server" />
                    </div>
                    <div class="div_row_input" style="text-align: right; width: 75%">
                        <span onclick="addParticipantor();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<span class='btn_delete' onclick="delParticipantor(this)" title="删除">&nbsp;&nbsp;&nbsp;</span>
                    </div>
                </div>
                <div class="div_row" id="orgorrole" style="height: 200px; overflow: auto;">
                    <agile:PagedGridView ID="gvOrgizationOrRole" CssClass="gridview" IncludeRowDoubleClick="false"
                        runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="选择">
                                <ItemTemplate>
                                    <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Eval("SortOrder")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="参与者ID">
                                <ItemTemplate>
                                    <%#Eval("ID")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("Name")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="类型">
                                <ItemTemplate>
                                    <%#Eval("ParticipantorType").ToSafeString().Cast<AgileEAP.Workflow.Enums.ParticipantorType>().GetRemark()%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </agile:PagedGridView>
                </div>
                <div class="div_row_left">
                    <asp:CheckBox ID="ckbIsAllowAppointParticipants" Text="允许前驱活动根据如上列表指派活动参与者" runat="server" />
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblProcessStarter" onclick="sharer()" Text="流程启动者" GroupName="ParticipantType"
                        runat="server" />
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rblSpecialActivity" onclick="sharer()" Text="活动执行者" GroupName="ParticipantType"
                            runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtspecialActivity" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rblParticipantRule" onclick="sharer()" Text="参与者规则" GroupName="ParticipantType"
                            runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtParticipantRule" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rblRelevantData" onclick="sharer()" Text="相关数据" GroupName="ParticipantType"
                            runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtspecialPath" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rblRegular" Text="规则逻辑" onclick="sharer()" GroupName="ParticipantType"
                            runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtRegularApp" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="tabform">
                <%--表单  --%>
                <div class="div_row">
                    <div class="div_row_lable">
                        业务表单名</div>
                    <div class="div_row_input">
                        <asp:TextBox runat="server" ID="txtDataSource"></asp:TextBox>
                    </div>
                    <div class="div_row_lable">
                        标题</div>
                    <div class="div_row_input">
                        <asp:TextBox runat="server" ID="txtTitle"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable" style="width: 30%">
                        参数配置</div>
                    <div class="div_row_input" style="width: 65%; text-align: right">
                        <%--                        <ul style="list-type: none">
                            <li onclick="chooseParameter();" class='btn_add' title='选择'>选择</li>
                            <li onclick="addParameter();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
                            <li onclick="delParameter(this)" class='btn_delete' title="删除">&nbsp;&nbsp;&nbsp;</li>
                        </ul>--%>
                        <span onclick="chooseParameter();" class='btn_choose_form' title='选择'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp</span>
                        &nbsp;&nbsp;<span onclick="addParameter();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<span class='btn_delete' onclick="delParameter(this)" title="删除">&nbsp;&nbsp;&nbsp;</span>
                    </div>
                </div>
                <div id="div_specialcontrol" class="div_row" style="height: 400px; overflow: auto;">
                    <table id="tblParameter" style="width: 100%; display: inline-table;">
                        <thead>
                            <tr>
                                <th style="width: 5%; text-align: center">
                                    选择
                                </th>
                                <th style="width: 15%">
                                    显示名称
                                </th>
                                <th style="width: 15%">
                                    名称
                                </th>
                                <th style="width: 12%">
                                    类型
                                </th>
                                <th style="width: 13%">
                                    控件
                                </th>
                                <th style="width: 5%">
                                    必填
                                </th>
                                <th style="width: 15%">
                                    默认值
                                </th>
                                <th style="width: 10%">
                                    访问方式
                                </th>
                                <th style="width: 10%">
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="gvformList">
                            <ItemTemplate>
                                <tr ondblclick="rowDbClick(this,'',true)" onmouseout="rowOut(this);" onmouseover="rowOver(this);"
                                    onclick="rowClick(this,'',true)" style="cursor: pointer">
                                    <td style="text-align: center">
                                        <input id="radioId" type="radio" value='' name="radioId" />
                                    </td>
                                    <td>
                                        <%#Eval("Text")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("DataType").ToSafeString().Cast<AgileEAP.Workflow.Enums.DataType>().GetRemark()%>
                                    </td>
                                    <td url="<%#Eval("URL") %>" rows="<%#Eval("Rows") %>" cols="<%#Eval("Cols") %>" datasource="<%#Eval("DataSource")%>">
                                        <%#Eval("ControlType").ToSafeString().Cast<AgileEAP.Workflow.Enums.ControlType>().GetRemark()%>
                                    </td>
                                    <td>
                                        <%#Eval("Required").ToSafeString().EqualIgnoreCase("False") ? "否":"是"%>
                                    </td>
                                    <td>
                                        <%#Eval("DefaultValue")%>
                                    </td>
                                    <td>
                                        <%#Eval("AccessPattern").ToSafeString().Cast<AgileEAP.Workflow.Enums.AccessPattern>().GetRemark()%>
                                    </td>
                                    <td flag="operate">
                                        <span class="btn_up" title='上移' onclick="executeOperate(this,'up')">&nbsp;&nbsp;&nbsp;</span>
                                        ｜ <span class="btn_down" title='下移' onclick="executeOperate(this,'down')">&nbsp;&nbsp;&nbsp;</span>
                                        ｜ <span class="btn_form_control_config" title='配置' onclick="configureField(this)">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div id="tablimit">
                <%--时间限制  --%>
                <div class="div_row_left">
                    <asp:CheckBox ID="chbIsTimeLimitSet" Text="启用时间限制" onclick="enableStrategy(this.id)"
                        runat="server" />
                </div>
                <div class="div_row_left">
                    日历设置
                </div>
                <div class="div_row" id="strategy">
                    <div class="div_row_lable">
                        选择日历：
                    </div>
                    <div class="div_row_input">
                        <agile:Combox runat="server" ID="cboCalendarType" IsSingle="true" />
                    </div>
                </div>
                <div class="div_row_left">
                    时间限制策略
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rabTimeLimitStrategy" onclick="limittime()" GroupName="TimeLimit"
                            Text="时限" runat="server" />
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtLimitTimeHour" runat="server" Width="40px" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"></asp:TextBox>小时
                        <asp:TextBox ID="txtLimitTimeMinute" runat="server" Width="40px" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"></asp:TextBox>分钟
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rabRelevantLimitTime" onclick="limittime()" GroupName="TimeLimit"
                            Text="相关数据" runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtRelevantData" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row" style="width: 100%; text-align: left">
                    <asp:CheckBox ID="chbIsSendMessageForOvertime" Text="启用邮件通知" runat="server" />
                </div>
                <div class="div_row_left">
                    超时预警策略
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rabRemindLimtTime" onclick="remindlimittime()" GroupName="RemindLimtTime"
                            Text="提前" runat="server" />
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtRemindLimtTimeHour" runat="server" Width="40px" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"
                            CssClass="littletext textright"></asp:TextBox>小时
                        <asp:TextBox ID="txtRemindLimtTimeMinute" runat="server" Width="40px" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"
                            CssClass="littletext textright"></asp:TextBox>分钟
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        <asp:RadioButton ID="rabRemindRelevantLimitTime" onclick="remindlimittime()" GroupName="RemindLimtTime"
                            Text="相关数据" runat="server" />
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtRemindRelevantData" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row_left">
                    <asp:CheckBox ID="chbisSendMessageForRemind" Text="启用邮件通知" runat="server" />
                </div>
            </div>
            <div id="tabtask">
                <%--多工作项  --%>
                <div class="div_row_left">
                    <asp:CheckBox ID="chbIsMulWIValid" Text="启动多工作项设置" onclick="enableMulwi(this.id)"
                        runat="server" />
                </div>
                <div class="div_row_left">
                    多工作项分配策略
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblParticipantNumber" runat="server" GroupName="WorkitemNumStrategy"
                        Text="按参与者设置个数领取工作项" />
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblOperatorNumber" runat="server" GroupName="WorkitemNumStrategy"
                        Text="按操作员个数领取工作项" />
                </div>
                <div class="div_row">
                    <div class="div_row_lable" style="width: 20%">
                        顺序执行工作项:</div>
                    <div class="div_row_input" style="margin-left: 12px">
                        <asp:RadioButton ID="rabYIsSequentialExecute" runat="server" GroupName="SequentialExecute"
                            Text="是" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rabNIsSequentialExecute" runat="server" GroupName="SequentialExecute"
                            Text="否" />
                    </div>
                </div>
                <div class="div_row_left">
                    完成规则设定
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblFinishAll" runat="server" GroupName="FinishRule" Text="全部完成" />
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblSpecifyNum" runat="server" GroupName="FinishRule" Text="完成个数" />
                </div>
                <div class="div_row">
                    <div class="div_row_lable" style="margin-left: 60px">
                        要求完成个数</div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtFinishRquiredNum" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblSpecifyPercent" runat="server" GroupName="FinishRule" Text="完成百分比" />
                </div>
                <div class="div_row">
                    <div class="div_row_lable" style="margin-left: 60px">
                        要求完成百分比
                    </div>
                    <div class="div_row_input">
                        <asp:TextBox ID="txtFinishRequiredPercent" onkeyup="value=value.replace(/[^\d]/g,'') "
                            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"
                            runat="server"></asp:TextBox>%
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable" style="width: 20%">
                        自动终止未完成工作项:</div>
                    <div class="div_row_input" style="margin-left: 15px">
                        <asp:RadioButton ID="rabYIsAutoCancel" runat="server" GroupName="AutoCancel" Text="是" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rabNIsAutoCancel" runat="server" GroupName="AutoCancel" Text="否" />
                    </div>
                </div>
            </div>
            <div id="tabevent">
                <%--触发事件  --%>
                <div class="div_row">
                    <div class="div_row_lable" style="width: 30%">
                        事件配置</div>
                    <div class="div_row_input" style="width: 70%; text-align: right">
                        <span onclick="addTriggerEvent();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<span class='btn_delete' onclick="delTriggerEvent(this)" title="删除">&nbsp;&nbsp;&nbsp;</span>
                    </div>
                </div>
                <div class="div_row" style="height: 400px; overflow: auto;">
                    <table id="tblTriggerEvent" style="width: 100%; display: inline-table;">
                        <thead>
                            <tr>
                                <th style="width: 10%; text-align: center">
                                    选择
                                </th>
                                <th style="width: 30%">
                                    触发时机
                                </th>
                                <th style="width: 40%">
                                    事件动作
                                </th>
                                <th style="width: 20%">
                                    调用方式
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptTriggerEvent">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center">
                                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                                    </td>
                                    <td>
                                        <%#Eval("TriggerEventType").ToSafeString().Cast<AgileEAP.Workflow.Enums.TriggerEventType>().GetRemark()%>
                                    </td>
                                    <td>
                                        <%#Eval("EventAction")%>
                                    </td>
                                    <td>
                                        <%#Eval("InvokePattern").ToSafeString().Cast<AgileEAP.Workflow.Enums.InvokePattern>().GetRemark()%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div id="tabrollback">
                <%--回退  --%>
                <div class="div_row">
                    <div class="div_row_lable">
                        类型</div>
                    <div class="div_row_input">
                        <agile:Combox ID="cboRollbackType" runat="server" IsSingle="true"></agile:Combox>
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        动作</div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtRollbackAction" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row_left">
                    参数配置表</div>
                <div class="div_row">
                    <table id="tblRollBack" style="width: 100%; display: inline-table;">
                        <thead>
                            <tr>
                                <th style="width: 40%">
                                    名称
                                </th>
                                <th style="width: 40%">
                                    值
                                </th>
                                <th style="width: 20%">
                                    方向
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptRollback">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <%#Eval("Value")%>
                                    </td>
                                    <td>
                                        <%#Eval("ParameterDirection").ToSafeString().Cast<AgileEAP.Workflow.Enums.ParameterDirection>().GetRemark()%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div id="tabfreeActivity">
                <%--自由流  --%>
                <div class="div_row_left">
                    <asp:CheckBox ID="chbIsFreeActivity" Text="设置该活动为自由活动" onclick="enableFreeActivity(this.id)"
                        runat="server" />
                </div>
                <div class="div_row_left">
                    自由范围设置策略</div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblFreeWithinProcess" runat="server" GroupName="FreeRangeStrategy"
                        Text="在该流程范围内任意自由" /></div>
                <div id="freeactivity" class="div_row">
                    <div class="div_row_lable" style="width: 30%">
                        <asp:RadioButton ID="rblFreeWithinActivities" runat="server" GroupName="FreeRangeStrategy"
                            Text="在指定活动列表范围内自由" />
                    </div>
                    <div class="div_row_input" style="width: 70%; text-align: right">
                        <span onclick="addFreeRange();" class='btn_add' title='添加'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        &nbsp;&nbsp;<span class='btn_delete' onclick="delFreeRange(this)" title="删除">&nbsp;&nbsp;&nbsp;</span>
                    </div>
                </div>
                <div id="freerangetable" class="div_row_left" style="height: 210px; overflow: auto;">
                    <table id="tblFreeRange" style="width: 100%; display: inline-table;">
                        <thead>
                            <tr>
                                <th style="width: 10%; text-align: center">
                                    选择
                                </th>
                                <th style="width: 45%">
                                    活动ID
                                </th>
                                <th style="width: 45%">
                                    活动名称
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptFreeRange">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center">
                                        <input id="radioId" type="radio" value='<%#Eval("ID") %>' name="radioId" />
                                    </td>
                                    <td>
                                        <%#Eval("ID")%>
                                    </td>
                                    <td>
                                        <%#Eval("Name")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblFreeWithinNextActivites" runat="server" GroupName="FreeRangeStrategy"
                        Text="在后继活动范围内自流" />
                </div>
                <div class="div_row_left">
                    自由流设置规则
                </div>
                <div class="div_row_left">
                    <asp:CheckBox ID="chbIsOnlyLimitedManualActivity" Text="流向的目标活动仅限于人工活动" runat="server" />
                </div>
            </div>
            <div id="tabactivateRule">
                <%--启动策略  --%>
                <div class="div_row_left">
                    可选规则</div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblDirectRunning" GroupName="ActivateRule" runat="server" Text="直接运行" /></div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblDisenabled" runat="server" GroupName="ActivateRule" Text="待激活" /></div>
                <div class="div_row_left">
                    <asp:RadioButton ID="rblAutoAfter" GroupName="ActivateRule" runat="server" Text="由规则逻辑值返回确定" /></div>
                <div class="div_row">
                    <div class="div_row_lable">
                        规则逻辑</div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txtActivateRuleApp" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="div_row_left" style="height: 40px">
                    (注：激活规则确定该活动在流程流转至此时将以何种方式激活，<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;激活规则返回值为1或True时均可直接激活，否则为待激活)
                </div>
                <div class="div_row_left">
                    重新启动规则</div>
                <div class="div_row_left" style="margin-left: 20px">
                    <asp:RadioButton ID="rbFirstParticipantor" GroupName="resetActivateRule" runat="server"
                        Text="最初参与者" /><asp:RadioButton ID="rbLastParticipantor" runat="server" GroupName="resetActivateRule"
                            Text="最终参与者" /></div>
                <div class="div_row_left">
                    重新设置URL
                    <asp:CheckBox runat="server" ID="chbIsSpecifyURL" />
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        URL类型
                    </div>
                    <div class="div_row_input">
                        <agile:Combox runat="server" ID="cboURLType" IsSingle="true" />
                    </div>
                </div>
                <div class="div_row">
                    <div class="div_row_lable">
                        调用URL
                    </div>
                    <div class="div_row_input" style="width: 80%">
                        <asp:TextBox ID="txt2SpecifyURL" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnActionState" runat="server" />
    <asp:HiddenField ID="hdnParticipantors" runat="server" />
    <asp:HiddenField ID="hdnFormControlSettings" runat="server" />
    <asp:HiddenField ID="hdnActivities" runat="server" />
</asp:Content>
