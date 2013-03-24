function SplitType() {
    this.XOR = "XOR",
       this.OR = "OR",
       this.AND = "AND"
}
function JoinType() {
    this.XOR = "XOR",
      this.OR = "OR",
      this.AND = "AND"
}
function WorkItemNumStrategy() {
    this.ParticipantNumber = "ParticipantNumber";
    this.OperatorNumber = "OperatorNumber";
}
function FinishRule() {
    this.FinishAll = "FinishAll";
    this.SpecifyNum = "SpecifyNum";
    this.SpecifyPercent = "SpecifyPercent";
}
function ParticipantorType() {
    this.Person = "Person";
    this.Role = "Role";
    this.Org = "Org";
}
function ParticipantType() {
    this.Participantor = "Participantor";
    this.ProcessStarter = "ProcessStarter";
    this.SpecialActivity = "SpecialActivity";
    this.RelevantData = "RelevantData";
    this.CustomRegular = "CustomRegular";
    this.ProcessExecutor = "ProcessExecutor";
    this.RelateRegular = "RelateRegular";
}
function FreeRangeStrategy() {
    this.FreeWithinProcess = "FreeWithinProcess";
    this.FreeWithinActivities = "FreeWithinActivities";
    this.FreeWithinNextActivites = "FreeWithinNextActivites";
}
function TimeLimitStrategy() {
    this.LimitTime = "LimitTime";
    this.RelevantLimitTime = "RelevantLimitTime"
}
function RemindStrategy() {
    this.RemindLimtTime = "RemindLimtTime";
    this.RemindRelevantLimitTime = "RemindRelevantLimitTime";
}
function InvokePattern() {
    this.Synchronous = "Synchronous";
    this.Asynchronous = "Asynchronous";
}
function TriggerEventType() {
    ActivityBeforeCreate = 1;
    ActivityBeforeStart = 2;
    ActivityAfterStart = 3;
    ActivityAfterOverTime = 4;
    ActivityAfterTerminate = 5;
    ActivityCompleted = 6;
    ActivityBeforeRemind = 7;
    WorkItemBeforeCreate = 21;
    WorkItemAtferCreate = 22;
    WorkItemExecuting = 23;
    WorkItemCompleted = 24;
    WorkItemError = 25;
    WorkItemCanncel = 26;
    WorkItemOverTime = 27;
    WorkItemSuspended = 28;
}
function ActionPattern() {
    this.Method = "Method";
    this.WebService = "WebService";
    this.BusinessOperation = "BusinessOperation";
}
function ParameterDirection() {
    this.In = "In";
    this.Out = "Out";
    this.Ref = "Ref";
}
function ActivateRuleType() {
    this.DirectRunning = "DirectRunning";
    this.WaitActivate = "WaitActivate";
    this.AutoSkip = "AutoSkip";
}
function ResetParticipant() {
    this.FirstParticipantor = 1;// "FirstParticipantor";
    this.LastParticipantor = 2;// "LastParticipantor";
}

function removeTab(remove) {
    var tabs = $('#tabcontainer').find("li");
    tabs.each(function (i) {
        if (i > 0) {
            $("#tab" + this.id).hide();
            if (remove) $(this).remove();
        }
        else {
            this.className = "tabBg7";
        }
    });
}
function initStartActivity(activity) {
    removeTab(true);
    $("#row_joinType").remove();
    $("#row_propertyandproxy").remove();
    $("#row_priority").remove();
    $("#row_businessSet").remove();
    $("#row_isSpecifyURL").remove();
    $("#row_urltype").remove();
    $("#row_CustomizeURL").remove();
    $("#row_description").css("clear", "both");
}
function initManualActivity(activity) {
    if (activity.Participant && activity.Participant.ParticipantType) {
        switch (activity.Participant.ParticipantType) {
            case "Participantor":
                $("#rblOrganization").attr("checked", "checked");
                $("#txtParticipantRule").attr("disabled", "disabled");
                $("#txtspecialActivity").attr("disabled", "disabled");
                $("#txtParticipantRule").val("");
                $("#txtspecialActivity").val("");
                break;
            case "ProcessStarter":
                $("#rblProcessStarter").attr("checked", "checked");
                $("#txtParticipantRule").attr("disabled", "disabled");
                $("#txtspecialActivity").attr("disabled", "disabled");
                $("#txtParticipantRule").val("");
                $("#txtspecialActivity").val(""); break;
            case "ProcessExecutor":
                $("#rblSpecialActivity").attr("checked", "checked");
                $("#txtspecialActivity").val(activity.Participant.ParticipantValue);
                $("#txtParticipantRule").val("");
                $("#txtParticipantRule").attr("disabled", "disabled");
                $("#txtspecialActivity").removeAttr("disabled");
                $("#orgorrole").attr("disabled", "disabled");
                break;
            case "CustomRegular":
                $("#rblParticipantRule").attr("checked", "checked");
                $("#txtParticipantRule").val(activity.Participant.ParticipantValue);
                $("#txtspecialActivity").val("");
                $("#txtspecialActivity").attr("disabled", "disabled");
                $("#txtParticipantRule").removeAttr("disabled");
                $("#orgorrole").attr("disabled", "disabled");
              //  $scope.Participant().ParticipantType = Type.CustomRegular;
                break;
        }
    }
    else {
        activity.Participant.ParticipantType = "Participantor";
        $("#txtParticipantRule").attr("disabled", "disabled");
        $("#txtspecialActivity").attr("disabled", "disabled");
        $("#txtParticipantRule").val("");
        $("#txtspecialActivity").val("");
    }
    if ($("#chbIsTimeLimitSet").attr("Checked")) {
        $("#txtLimitTimeHour").removeAttr("disabled");
        $("#txtLimitTimeMinute").removeAttr("disabled");
        $("#txtRelevantData").removeAttr("disabled");
        $("#chbIsSendMessageForOvertime").removeAttr("disabled");
        $("#txtRemindLimtTimeHour").removeAttr("disabled");
        $("#txtRemindLimtTimeMinute").removeAttr("disabled");
        $("#txtRemindRelevantData").removeAttr("disabled");
        $("#chbisSendMessageForRemind").removeAttr("disabled");
        $("#rabRelevantLimitTime").removeAttr("disabled");
        $("#rabTimeLimitStrategy").removeAttr("disabled");
        $("#rabRemindLimtTime").removeAttr("disabled");
        $("#rabRemindRelevantLimitTime").removeAttr("disabled");
    }
    else {
        $("#txtLimitTimeHour").attr("disabled", "disabled");
        $("#txtLimitTimeMinute").attr("disabled", "disabled");
        $("#txtRelevantData").attr("disabled", "disabled");
        $("#chbIsSendMessageForOvertime").attr("disabled", "disabled");
        $("#txtRemindLimtTimeHour").attr("disabled", "disabled");
        $("#txtRemindLimtTimeMinute").attr("disabled", "disabled");
        $("#txtRemindRelevantData").attr("disabled", "disabled");
        $("#chbisSendMessageForRemind").attr("disabled", "disabled");
        $("#rabRelevantLimitTime").attr("disabled", "disabled");
        $("#rabTimeLimitStrategy").attr("disabled", "disabled");
        $("#rabRemindLimtTime").attr("disabled", "disabled");
        $("#rabRemindRelevantLimitTime").attr("disabled", "disabled");

    }
    if (activity.CustomURL) {
        if (activity.CustomURL.URLType == "CustomURL") {
            $("#rbCustomizeURL").attr("checked", "checked");
            $("#txtSpecifyURL").removeAttr("disabled");
            activity.CustomURL.URLType = "CustomURL";
        }
        else {
            $("#rbDefaultURL").attr("checked", "checked");
            $("#txtSpecifyURL").attr("disabled", "disabled");
            activity.CustomURL.URLType = "DefaultURL";
        }
    }
    var activateRuleType = new ActivateRuleType();
    var resetParticipant = new ResetParticipant();
    if (activity.ActivateRule) {
        if (activity.ActivateRule.ActivateRuleType == activateRuleType.DirectRunning) {
            $("#rblDirectRunning").attr("Checked", "checked");
        }
        else if (activity.ActivateRule.ActivateRuleType == activateRuleType.WaitActivate) {
            $("#rblDisenabled").attr("Checked", "checked");
        }
        else if (activity.ActivateRule.ActivateRuleType == activateRuleType.AutoSkip)//可选规则
        {
            $("#rblAutoAfter").attr("Checked", "checked");
        }
        if (activity.ResetParticipant == resetParticipant.FirstParticipantor) {
            $("#rbFirstParticipantor").attr("Checked", "checked");
        }
        else if (activity.ResetParticipant == resetParticipant.LastParticipantor)  //重新启动规则
        {
            $("#rbLastParticipantor").attr("Checked", "checked");
        }
    }
    if (activity.FreeFlowRule) {
        var freeRangeStrategy = new FreeRangeStrategy();
        if (activity.FreeFlowRule.FreeRangeStrategy == freeRangeStrategy.FreeWithinProcess) {
            $("#rblFreeWithinProcess").attr("Checked", "checked");
        }
        else if (activity.FreeFlowRule.FreeRangeStrategy == freeRangeStrategy.FreeWithinActivities) {
            $("#rblFreeWithinActivities").attr("Checked", "checked");
        }
        else if (activity.FreeFlowRule.FreeRangeStrategy == freeRangeStrategy.FreeWithinNextActivites) {
            $("#rblFreeWithinNextActivites").attr("Checked", "checked");
        }; //自由范围设置策略
    }

    if ($("#chbIsFreeActivity").attr("checked")) {
        $(".FreeActivity").find("input").removeAttr("disabled");
    }
    else {
        $(".FreeActivity").find("input").attr("disabled", "disabled");
    }
    if ($("#chbIsMulWIValid").attr("checked")) {
        $(".MulWIValidConfigure").find("input:radio").removeAttr("disabled");
        $(".MulWIValidConfigure").find("input:checkbox").removeAttr("disabled");
    }
    else {
        $(".MulWIValidConfigure").find("input").attr("disabled", "disabled");
    }
    if (activity.MultiWorkItem && activity.MultiWorkItem.WorkItemNumStrategy) {
        var workItemNumStrategy = new WorkItemNumStrategy();
        var finishRule = new FinishRule();
        if (activity.MultiWorkItem.WorkItemNumStrategy == workItemNumStrategy.ParticipantNumber) {
            $("#rblParticipantNumber").attr("checked", "checked");
        }
        else {
            $("#rblOperatorNumber").attr("checked", "checked");
        }
        if (activity.MultiWorkItem.IsSequentialExecute)//顺序执行工作项
        {
            $("#rabYIsSequentialExecute").attr("checked", "checked");
        }
        else {
            $("#rabNIsSequentialExecute").attr("checked", "checked");
        }
        if (activity.MultiWorkItem.FinishRule == finishRule.FinishAll)  //完成规则设定
        {
            $("#rblFinishAll").attr("checked", "checked");
        }
        else if (activity.MultiWorkItem.FinishRule == finishRule.SpecifyNum) {
            $("#rblSpecifyNum").attr("checked", "checked");
        }
        else {
            $("#rblSpecifyPercent").attr("checked", "checked");
        }
        if (activity.MultiWorkItem.IsAutoCancel)//顺序执行工作项
        {
            $("#rabYIsAutoCancel").attr("checked", "checked");
        }
        else {
            $("#rabNIsAutoCancel").attr("checked", "checked");
        }
    }
    if (activity.TimeLimit.TimeLimitInfo) {
        var timeLimitStrategy = new TimeLimitStrategy();
        if (activity.TimeLimit.TimeLimitInfo.TimeLimitStrategy == timeLimitStrategy.LimitTime) {
            $("#rabTimeLimitStrategy").attr("checked", "checked");
        }
        else {
            $("#rabRelevantLimitTime").attr("checked", "checked");
        }
    }
    if (activity.TimeLimit.RemindInfo) {
        var remindStrategy = new RemindStrategy();
        if (activity.TimeLimit.RemindInfo.RemindStrategy == remindStrategy.RemindLimtTime) {
            $("#rabRemindLimtTime").attr("checked", "checked");
        }
        else {
            $("#rabRemindRelevantLimitTime").attr("checked", "checked");
        }
    }
}
function initRouterActivity() {
    removeTab(true);
    $("#row_propertyandproxy").remove();
    $("#row_priority").remove();
    $("#row_businessSet").remove();
    $("#row_isSpecifyURL").remove();
    $("#row_urltype").remove();
    $("#row_CustomizeURL").remove();
}
function initSubflowActivity(activity) {
    initManualActivity(activity);
}
function initAutoActivity(activity) {
    $("#participantor").remove();
    $("#tabparticipantor").hide();
    $("#task").remove();
    $("#tabtask").hide();
    $("#limit").remove();
    $("#tablimit").hide();
    $("#task").remove();
    $("#tabtask").hide();
    $("#event").remove();
    $("#tabevent").hide();
    $("#freeActivity").remove();
    $("#tabfreeActivity").hide();
    $("#row_propertyandproxy").remove();
    $("#row_priority").remove();
}
function initEndActivity(activity) {
    removeTab(true);
    $("#row_splitType").remove();
    $("#row_propertyandproxy").remove();
    $("#row_priority").remove();
    $("#row_businessSet").remove();
    $("#row_isSpecifyURL").remove();
    $("#row_urltype").remove();
    $("#row_CustomizeURL").remove();
    $("#row_joinType").css("clear", "both");
    $("#row_description").css("clear", "both");
}
function initProcessActivity(activity) {
    initManualActivity(activity);
}
function initProcessDefine($scope) {
    $scope.processDefine = window.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.processDefine;
    $scope.Ativity = function () {
        var Ativity = "";
        var processDefID = $.query.get("ProcessDefID");
        var activityID = $.query.get("ActivityID");
        angular.forEach($scope.processDefine.Activities, function (Activitie) {
            if (Activitie.ID == activityID) {//&&processDefID == $scope.processDefine.ID) {
                Ativity= Activitie;
               if (Activitie.NewID==null) {
                    Activitie.NewID = activityID;
                }
                if (!Activitie.SplitType) {
                    var splitType = new SplitType();
                    Activitie.SplitType = splitType.XOR;
                }
                if (!Activitie.JoinType) {
                    var joinType = new JoinType();
                    Activitie.JoinType = joinType.AND;
                }
                if (Activitie.MultiWorkItem) {
                    if (!Activitie.MultiWorkItem.FinishRquiredNum) {
                        Activitie.MultiWorkItem.FinishRquiredNum = 0;
                    }
                    if (!Activitie.MultiWorkItem.FinishRequiredPercent) {
                        Activitie.MultiWorkItem.FinishRequiredPercent = 0;
                    }
                }
                    window.parent.$("#actionDialog").find("#bg_div_iframe")[0].contentWindow.editActivityName(Activitie.ID, Activitie.Name);
            }
        });
        return Ativity;
    };
    $scope.Participant = function () {
        var Participant = new Object();
        if ($scope.Ativity().Participant) {
            Participant = $scope.Ativity().Participant;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().Participant = Participant;
            }
        }
        return Participant;
    };
    $scope.CustomURL = function () {
        var CustomURL = new Object();
        if ($scope.Ativity().CustomURL) {
            CustomURL = $scope.Ativity().CustomURL;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().CustomURL = CustomURL;
            }
        }
        return CustomURL;
    }
    $scope.businessSet = function (URL) {
        if (URL == "rbCustomizeURL") {
            $("#rbCustomizeURL").attr("checked", "checked");
            $("#txtSpecifyURL").removeAttr("disabled");
            //  $("#rbDefaultURL").attr("disabled", "disabled");
            $scope.CustomURL().URLType = "CustomURL";
            
        }
        else {
            $("#rbDefaultURL").attr("checked", "checked");
            $("#txtSpecifyURL").attr("disabled", "disabled");
            $scope.CustomURL().URLType = "DefaultURL";
        }
    }
    $scope.addParticipant = function () {
        var participantors = openOperateDialog("添加参与者", "../../Workflow/ChooseParticipantor.aspx", 980, 700, true, 1);
        if (participantors == null) return;
        if (participantors.length != 0) {
            var tbody = $("tbody", "#gvOrgizationOrRole_container");
            var index = $("tr", tbody).length;
            var count = participantors.length;
            for (var i = 0; i < participantors.length; i++) {
                if ($("tr", tbody).find("td:contains('" + participantors[i].id + "')").length > 0)//存在的不添加
                    continue;
                var participantorType = new ParticipantorType();
                switch (participantors[i].type) {
                    case "人员": participantors[i].type = participantorType.Person; break;
                    case "角色": participantors[i].type = participantorType.Role; break;
                    case "组织": participantors[i].type = participantorType.Org; break;
                }
                if (!$scope.Participant().Participantors) {
                    var Participantors = new Array();
                    Participantors.push({ ID: participantors[i].id, Name: participantors[i].name, SortOrder: count + index - i, ParticipantorType: participantors[i].type });
                    $scope.Ativity().Participant = new Object();
                    $scope.Ativity().Participant.Participantors = Participantors;
                }
                else {
                    $scope.Ativity().Participant.Participantors.push({ ID: participantors[i].id, Name: participantors[i].name, SortOrder: count + index - i, ParticipantorType: participantors[i].type });
                }
            }
        }
    };
    $scope.delParticipantor = function () {
        if (!confirm("是否确定删除记录？")) {
            return false;
        }
        var table = $("#gvOrgizationOrRole_container");
        var rad = table.find(":checked ").first();
        //alert($(rad).val());
        angular.forEach($scope.Participant().Participantors, function (Participantor) {
            if ($(rad).val() == Participantor.ID) {
                var position = $scope.Ativity().Participant.Participantors.indexOf(Participantor);
                // alert("xuan");
                $scope.Ativity().Participant.Participantors.splice(position, 1);
            }
        });

    };
    $scope.chooseParticipantorType = function (ParticipantorType) {
        var Type = new ParticipantType();
        if ("rblParticipantRule" == ParticipantorType) {
            $("#txtParticipantRule").val($scope.Participant().ParticipantValue);
            $("#txtspecialActivity").val("");
            $("#txtspecialActivity").attr("disabled", "disabled");
            $("#txtParticipantRule").removeAttr("disabled");
            $("#orgorrole").attr("disabled", "disabled");
            $scope.Participant().ParticipantType = Type.CustomRegular;
        }
        else if ("rblSpecialActivity" == ParticipantorType) {
            $("#txtspecialActivity").val($scope.Participant().ParticipantValue);
            $("#txtParticipantRule").val("");
            $("#txtParticipantRule").attr("disabled", "disabled");
            $("#txtspecialActivity").removeAttr("disabled");
            $("#orgorrole").attr("disabled", "disabled");
            $scope.Participant().ParticipantType = Type.ProcessExecutor;
        }
        else {
            if ("rblProcessStarter" == ParticipantorType) {

                $scope.Participant().ParticipantType = Type.ProcessExecutor;
            }
            else {
                $scope.Participant().ParticipantType = Type.Participantor;
            }
            $("#txtParticipantRule").attr("disabled", "disabled");
            $("#txtspecialActivity").attr("disabled", "disabled");
            $("#txtParticipantRule").val("");
            $("#txtspecialActivity").val("");
        }
    };
    $scope.TimeLimit = function () {
        var TimeLimit = new Object();
        // alert($scope.Ativity().Participant);
        if ($scope.Ativity().TimeLimit) {
            TimeLimit = $scope.Ativity().TimeLimit;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {

                $scope.Ativity().TimeLimit = TimeLimit;
            }
        }
        return TimeLimit;
    };
    $scope.SetLimit = function () {
        if ($("#chbIsTimeLimitSet").attr("Checked")) {
            $("#txtLimitTimeHour").removeAttr("disabled");
            $("#txtLimitTimeMinute").removeAttr("disabled");
            $("#txtRelevantData").removeAttr("disabled");
            $("#chbIsSendMessageForOvertime").removeAttr("disabled");
            $("#txtRemindLimtTimeHour").removeAttr("disabled");
            $("#txtRemindLimtTimeMinute").removeAttr("disabled");
            $("#txtRemindRelevantData").removeAttr("disabled");
            $("#chbisSendMessageForRemind").removeAttr("disabled");
            $("#rabRelevantLimitTime").removeAttr("disabled");
            $("#rabTimeLimitStrategy").removeAttr("disabled");
            $("#rabRemindLimtTime").removeAttr("disabled");
            $("#rabRemindRelevantLimitTime").removeAttr("disabled");
        }
        else {
            $("#txtLimitTimeHour").attr("disabled", "disabled");
            $("#txtLimitTimeMinute").attr("disabled", "disabled");
            $("#txtRelevantData").attr("disabled", "disabled");
            $("#chbIsSendMessageForOvertime").attr("disabled", "disabled");
            $("#txtRemindLimtTimeHour").attr("disabled", "disabled");
            $("#txtRemindLimtTimeMinute").attr("disabled", "disabled");
            $("#txtRemindRelevantData").attr("disabled", "disabled");
            $("#chbisSendMessageForRemind").attr("disabled", "disabled");
            $("#rabRelevantLimitTime").attr("disabled", "disabled");
            $("#rabTimeLimitStrategy").attr("disabled", "disabled");
            $("#rabRemindLimtTime").attr("disabled", "disabled");
            $("#rabRemindRelevantLimitTime").attr("disabled", "disabled");

        }
    };
    $scope.chooseLimit = function (id) {
        var timeLimitStrategy = new TimeLimitStrategy();
        if (id == "rabTimeLimitStrategy") {
            $("#txtRelevantData").attr("disabled", "disabled");
            $("#txtLimitTimeHour").removeAttr("disabled");
            $("#txtLimitTimeMinute").removeAttr("disabled");
            if (!$scope.TimeLimit().TimeLimitInfo) {
                $scope.TimeLimit().TimeLimitInfo = new Object();
            }
            $scope.TimeLimit().TimeLimitInfo.TimeLimitStrategy = timeLimitStrategy.LimitTime;
        }
        else {
            $("#txtLimitTimeHour").attr("disabled", "disabled");
            $("#txtLimitTimeMinute").attr("disabled", "disabled");
            //alert("1");
            $("#txtRelevantData").removeAttr("disabled");
            if (!$scope.TimeLimit().TimeLimitInfo) {
                $scope.TimeLimit().TimeLimitInfo = new Object();
            }
            $scope.TimeLimit().TimeLimitInfo.TimeLimitStrategy = timeLimitStrategy.RelevantLimitTime;
        }
    };
    $scope.chooseRemind = function (id) {
        var remindStrategy = new RemindStrategy();
        if (id == "rabRemindLimtTime") {
            $("#txtRemindRelevantData").attr("disabled", "disabled");
            $("#txtRemindLimtTimeMinute").removeAttr("disabled");
            $("#txtRemindLimtTimeHour").removeAttr("disabled");
            if (!$scope.TimeLimit().RemindInfo) {
                $scope.TimeLimit().RemindInfo = new Object();
            }
            $scope.TimeLimit().RemindInfo.RemindStrategy = remindStrategy.RemindLimtTime;
        }
        else {
            $("#txtRemindLimtTimeHour").attr("disabled", "disabled");
            $("#txtRemindLimtTimeMinute").attr("disabled", "disabled");
            $("#txtRemindRelevantData").removeAttr("disabled");
            if (!$scope.TimeLimit().RemindInfo) {
                $scope.TimeLimit().RemindInfo = new Object();
            }
            $scope.TimeLimit().RemindInfo.RemindStrategy = remindStrategy.RemindRelevantLimitTime;
        }
    };
    $scope.TriggerEvents = function () {
        var TriggerEvents = $scope.Ativity().TriggerEvents;
        return TriggerEvents;
    };
    $scope.addTriggerEvent = function () {
        //var tbody = $("tbody", "#tblTriggerEvent");
        if (!$scope.TriggerEvents()) {
            $scope.Ativity().TriggerEvents = new Array();
        }
        //alert($scope.TriggerEvents());
        // $scope.TriggerEvents().push({ ID: new Date().getTime()});
        $scope.TriggerEvents().push({ ID: new Date().getTime(), EventAction: "", InvokePattern: "Synchronous", TriggerEventType: "ActivityBeforeCreate" });
    };
    $scope.delTriggerEvent = function () {
        if (!confirm("是否确定删除记录？")) {
            return false;
        }
        var table = $("#tblTriggerEvent");
        var rad = table.find("input:checked ").first();
        // alert($(rad).val());
        angular.forEach($scope.TriggerEvents(), function (TriggerEvent) {
            if ($(rad).val() == TriggerEvent.ID) {
                var position = $scope.TriggerEvents().indexOf(TriggerEvent);
                // alert("xuan");
                $scope.TriggerEvents().splice(position, 1);
            }
        });
    };
    $scope.RollBack = function () {
        var RollBack = new Object();
        // alert($scope.Ativity().Participant);
        if ($scope.Ativity().RollBack) {
            RollBack = $scope.Ativity().RollBack;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {

                $scope.Ativity().RollBack = RollBack;
            }
        }
        return RollBack;
    };
    $scope.ActivateRule = function () {
        var ActivateRule = new Object();
        if ($scope.Ativity().ActivateRule) {
            ActivateRule = $scope.Ativity().ActivateRule;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().ActivateRule = ActivateRule;
            }
        }
        return ActivateRule;
    };
    $scope.ActivateRuleType = function (id) {
        var activateRuleType = new ActivateRuleType();
        if (id == "rblDirectRunning") {
            $scope.ActivateRule().ActivateRuleType = activateRuleType.DirectRunning;
        }
        else if (id == "rblDisenabled") {
            $scope.ActivateRule().ActivateRuleType = activateRuleType.WaitActivate;
        }
        else {
            $scope.ActivateRule().ActivateRuleType = activateRuleType.AutoSkip;
        }
    };
    $scope.ResetParticipant = function (id) {
        var resetParticipant = new ResetParticipant();
        if (id == "rbFirstParticipantor") {
            $scope.Ativity().ResetParticipant = resetParticipant.FirstParticipantor;
        } else {
            $scope.Ativity().ResetParticipant = resetParticipant.LastParticipantor;
        }
    }
    $scope.ResetURL = function () {
        var ResetURL = new Object();
        if ($scope.Ativity().ResetURL) {
            ResetURL = $scope.Ativity().ResetURL;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().ResetURL = ResetURL;
            }
        }
        return ResetURL;
    };

    $scope.FreeFlowRule = function () {
        var FreeFlowRule = new Object();
        if ($scope.Ativity().FreeFlowRule) {
            FreeFlowRule = $scope.Ativity().FreeFlowRule;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().FreeFlowRule = FreeFlowRule;
            }
        }
        return FreeFlowRule;
    };
    $scope.addFreeRange = function () {
        //var tbody = $("tbody", "#tblTriggerEvent");
        if (!$scope.FreeFlowRule().FreeRangeActivities) {
            $scope.Ativity().FreeFlowRule.FreeRangeActivities = new Array();
        }
        //  alert($scope.FreeFlowRule().FreeRangeActivities);
        // $scope.TriggerEvents().push({ ID: new Date().getTime()});
        $scope.FreeFlowRule().FreeRangeActivities.push({ ID: new Date().getTime() });
    };
    $scope.delFreeRange = function () {
        if (!confirm("是否确定删除记录？")) {
            return false;
        }
        var table = $("#tblFreeRange");
        var rad = table.find("input:checked ").first();
        angular.forEach($scope.FreeFlowRule().FreeRangeActivities, function (FreeRangeActivity) {
            if ($(rad).val() == FreeRangeActivity.ID) {
                var position = $scope.FreeFlowRule().FreeRangeActivities.indexOf(FreeRangeActivity);
                // alert("xuan");
                $scope.FreeFlowRule().FreeRangeActivities.splice(position, 1);
            }
        });
    };
    $scope.chooseFree = function () {
        if ($("#chbIsFreeActivity").attr("checked")) {
            //$(".FreeActivity").find("input:radio").removeAttr("disabled");
            //$(".FreeActivity").find("input:checkbox").removeAttr("disabled");
            $(".FreeActivity").find("input").removeAttr("disabled");
        }
        else {
            //$(".FreeActivity").find("input:radio").attr("disabled", "disabled");
            //$(".FreeActivity").find("input:checkbox").attr("disabled", "disabled");
            $(".FreeActivity").find("input").attr("disabled", "disabled");
        }
    };
    $scope.choosescopeFree = function (id) {
        var freeRangeStrategy = new FreeRangeStrategy();
        if (id == "rblFreeWithinProcess") {
            $scope.FreeFlowRule().FreeRangeStrategy = freeRangeStrategy.FreeWithinProcess;
        }
        else if (id == "rblFreeWithinActivities") {
            $scope.FreeFlowRule().FreeRangeStrategy = freeRangeStrategy.FreeWithinActivities;
        }
        else {
            $scope.FreeFlowRule().FreeRangeStrategy = freeRangeStrategy.FreeWithinNextActivites;
        }
    };
    $scope.Form = function () {
        var Form = new Object();
        if ($scope.Ativity().Form) {
            Form = $scope.Ativity().Form;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().Form = Form;
            }
        }
        return Form;
    };
    $scope.FormDesigner = function () {
       // var form = window.parent.parent.parent.showDialog("actionDialog3", "表单设计器", "/FormDesigner/Home/FormDesigner", 1250, 700);
      // var form = openOperateDialog("表单设计器", "/FormDesigner/Home/FormDesigner?ActivityID=" + $scope.Ativity().ID+"&&ProcessDefID=" + $.query.get("ProcessDefID"), 1250, 700, true, 1);
        var form = openDialog2({ title: "表单设计器", url: "/FormDesigner/Home/FormDesigner?ActivityID=" + $scope.Ativity().ID + "&&ProcessDefID=" + $.query.get("ProcessDefID"), dialogType: 1, showModal: true,height:screen.height-80,width:screen.width-10,windowStyle: { style: "dialogLeft:0;dialogTop:-100;edge: Raised; center: Yes; resizable: Yes; status: no;scrollbars=no;", auguments: window } });
        //alert(formResult);
       //alert(form);
        if (!form || !form.Fields || form.Fields.length == 0) return;
        $scope.Ativity().Form = form;
    };
    $scope.executeOperate = function (field) {
        if (field.indexOf("up$#") > 0) {
            FieldName = field.substring(0, field.indexOf("up$#"));
        }
        else {
            FieldName = field.substring(0, field.indexOf("down$#"));
        }
        angular.forEach($scope.Form().Fields, function (Field) {
            var i = 0;
            if (Field.Name == FieldName) {
                var position = $scope.Form().Fields.indexOf(Field);
                if (position > 0 && position < $scope.Form().Fields.length) {
                    $scope.Form().Fields.splice(position, 1);
                    if (field.indexOf("up$#") > 0) {
                        $scope.Form().Fields.splice(position - 1, 0, Field);
                        i = 1;
                    }
                    else {
                        $scope.Form().Fields.splice(position + 1, 0, Field);
                        i = 1;
                    }
                }
                else if (position == 0 && field.indexOf("down$#") > 0) {
                    $scope.Form().Fields.splice(position, 1);
                    $scope.Form().Fields.splice(position + 1, 0, Field);
                    i = 1;
                }

            }
            if (i == 1) {
                FieldName = null;
            }
        });


    };
    $scope.chooseParameter = function () {
        var form = openOperateDialog("选择表单来源", "/Workflow/ActivityManager.aspx?Entry=ChooseActivity&ProcessDefID=" + $.query.get("ProcessDefID"), 550, 300, true, 1);
        if (!form || !form.Fields || form.Fields.length == 0) return;
        //for (var i = 0; i < form.Fields.length; i++) {
        //    $scope.Form().Fields.push({X:form.Fields[i],Y:form.Field[i].Y,Width:form.Field[i].Width,Height:form.Field[i].Height, Name: form.Fields[i].Name, Required: form.Fields[i].Required, SortOrder: form.Fields[i].SortOrder, Text: form.Fields[i].Text, DefaultValue: form.Fields[i].DefaultValue, DataType: form.Fields[i].DataType, ControlType: form.Fields[i].ControlType, AccessPattern: form.Fields[i].AccessPattern });
        //}
        $scope.Ativity().Form = form;
        $scope.Form().DataSource = form.DataSource;
    };
    $scope.addParameter = function () {
        if (!$scope.Form().Fields) {
            $scope.Ativity().Form.Fields = new Array();
        }
        $scope.Form().Fields.push({ Name: new Date().getTime(), DataType: "Integer", ControlType: "TextBox", AccessPattern: "Write" });
    };
    $scope.delParameter = function () {
        var table = $("#tblParameter");
        var rad = table.find("input[type='radio']:checked").first();
        if (table.find("input:checked")[0] == undefined) {
            alert("请先选择要删除的记录");
            return false;
        }
        if (!confirm("是否确定删除记录？")) {
            return false;
        }
        angular.forEach($scope.Form().Fields, function (Field) {
            if ($(rad).val() == Field.Name) {
                var position = $scope.Form().Fields.indexOf(Field);
                $scope.Form().Fields.splice(position, 1);
            }
        });
    };
    $scope.configureField = function (Field) {
        var controlType = $.trim(Field.ControlType);
        if (controlType == "ChooseBox") {
            var field = openOperateDialog("设置" + controlType + "属性", "/Workflow/FormControlConfigure.aspx?type=ChooseBox&url=" + Field.URL, 550, 300, true, 1);
            Field.URL = field.url;
        }
        else if (controlType == "TextBox") {
            var rows = Field.Rows || 1;
            var cols = Field.Cols || 1;
            var field = openOperateDialog("设置" + controlType + "属性", "/Workflow/FormControlConfigure.aspx?type=TextBox&rows=" + rows + "&cols=" + cols, 550, 300, true, 1);
            Field.Cols = field.cols;
            Field.Rows = field.rows;
        }
        else if (controlType == "SingleCombox" || controlType == "MultiCombox") {
            var datasource = openOperateDialog("设置" + controlType + "属性", "/Plugins/Administration/Pages/ChooseDict.aspx", 550, 300, true, 1);
            if (datasource)
                Field.DataSource = datasource;
        }
    };
    $scope.MultiWorkItem = function () {
        var MultiWorkItem = new Object();
        if ($scope.Ativity().MultiWorkItem) {
            MultiWorkItem = $scope.Ativity().MultiWorkItem;
        }
        else {
            if ($scope.Ativity().ActivityType == "ManualActivity" || $scope.Ativity().ActivityType == 2) {
                $scope.Ativity().MultiWorkItem = MultiWorkItem;

            }
        }
        return MultiWorkItem;
    };
    $scope.enableMulwi = function () {
        if ($("#chbIsMulWIValid").attr("checked")) {
            $(".MulWIValidConfigure").find("input").removeAttr("disabled");
        }
        else {
            $(".MulWIValidConfigure").find("input").attr("disabled", "disabled");
        }
    };
    $scope.WorkItemNum = function (id) {
        var workItemNumStrategy = new WorkItemNumStrategy();
        if (id == "rblParticipantNumber") {
            $scope.MultiWorkItem().WorkItemNumStrategy = workItemNumStrategy.ParticipantNumber;
        }
        else {
            $scope.MultiWorkItem().WorkItemNumStrategy = workItemNumStrategy.OperatorNumber;
        }
    };
    $scope.FinishRuleType = function (id) {
        var finishRule = new FinishRule();
        if (id == "rblSpecifyNum") {
            $scope.MultiWorkItem().FinishRule = finishRule.SpecifyNum;
            $("#txtFinishRquiredNum").removeAttr("disabled");
            $("#txtFinishRequiredPercent").attr("disabled", "disabled");
        }
        else if (id == "rblFinishAll") {
            $scope.MultiWorkItem().FinishRule = finishRule.FinishAll;
            $("#txtFinishRquiredNum").attr("disabled", "disabled");
            $("#txtFinishRequiredPercent").attr("disabled", "disabled");
        } else {
            $scope.MultiWorkItem().FinishRule = finishRule.SpecifyPercent;
            $("#txtFinishRquiredNum").attr("disabled", "disabled");
            $("#txtFinishRequiredPercent").removeAttr("disabled");
        }
    };
    $scope.SequentialExecute = function (id) {
        if (id == "rabYIsSequentialExecute") {
            $scope.MultiWorkItem().IsSequentialExecute = true;
        }
        else {
            $scope.MultiWorkItem().IsSequentialExecute = false;
        }
    };
    $scope.AutoCancel = function (id) {
        if (id == "rabYIsAutoCancel") {
            $scope.MultiWorkItem().IsAutoCancel = true;
        }
        else {
            $scope.MultiWorkItem().IsAutoCancel = false;
        }
    };
}
