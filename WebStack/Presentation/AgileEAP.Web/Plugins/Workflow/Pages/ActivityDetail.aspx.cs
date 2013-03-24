using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

using AgileEAP.Core;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;

using AgileEAP.WebControls;
using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Enums;
using AgileEAP.Core.Utility;

using System.Collections;

namespace AgileEAP.Plugin.Workflow
{
    public partial class ActivityDetail : BasePage
    {
        WorkflowEngine wfEngine = new WorkflowEngine();
        public string ProcessDefID
        {
            get
            {
                return Request.QueryString["ProcessDefID"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitUI();
            }
        }
        public Activity ActivityInfo
        {
            get
            {
                ProcessDefine processDefine = wfEngine.GetProcessDefine(ProcessDefID);
                return processDefine.Activities.FirstOrDefault(a => a.ID == CurrentId);
            }
        }

        public string TypeName
        {
            get
            {
                if (ActivityInfo != null)
                {
                    return ActivityInfo.ActivityType.ToSafeString();
                }
                return "";
            }
        }
        public void InitUI()
        {
            if (ActivityInfo.ActivityType == ActivityType.ManualActivity)
            {
                ManualActivity manualActivity = ActivityInfo as ManualActivity;
                //基本
                txtID.Text = manualActivity.ID;
                txtName.Text = manualActivity.Name;
                cboJoinType.BindCombox(JoinType.AND);
                cboJoinType.SelectedValue = ((int)(object)manualActivity.JoinType).ToSafeString();
                cboSplitType.BindCombox(SplitType.AND);
                cboSplitType.SelectedValue = ((int)(object)manualActivity.SplitType).ToSafeString();
                chbAllowAgent.Checked = manualActivity.AllowAgent;
                chbIsSplitTransaction.Checked = manualActivity.IsSplitTransaction; //分割事务
                txtDescription.Text = manualActivity.Description; //描述
                if (manualActivity.CustomURL.URLType == URLType.DefaultURL)
                {
                    rbDefaultURL.Checked = true;
                }
                else if (manualActivity.CustomURL.URLType == URLType.CustomURL)
                {
                    rbCustomizeURL.Checked = true;
                    txtSpecifyURL.Text = manualActivity.CustomURL.SpecifyURL;
                }
                else if (manualActivity.CustomURL.URLType == URLType.ManualProcess)
                {
                    rbTask.Checked = true;
                }


                //参与者
                gvOrgizationOrRole.DataSource = manualActivity.Participant.Participantors;
                gvOrgizationOrRole.DataBind();
                if (manualActivity.Participant.ParticipantType == ParticipantType.Participantor)
                {
                    rblOrganization.Checked = true;
                    ckbIsAllowAppointParticipants.Checked = manualActivity.Participant.AllowAppointParticipants;
                }
                else if (manualActivity.Participant.ParticipantType == ParticipantType.ProcessStarter)
                {
                    rblProcessStarter.Checked = true;
                }
                else if (manualActivity.Participant.ParticipantType == ParticipantType.ProcessExecutor)
                {
                    rblSpecialActivity.Checked = true;
                    txtspecialActivity.Text = manualActivity.Participant.ParticipantValue;
                }
                else if (manualActivity.Participant.ParticipantType == ParticipantType.CustomRegular)
                {
                    rblParticipantRule.Checked = true;
                    txtParticipantRule.Text = manualActivity.Participant.ParticipantValue;
                }
                else if (manualActivity.Participant.ParticipantType == ParticipantType.RelevantData)
                {
                    rblRelevantData.Checked = true;
                    txtspecialPath.Text = manualActivity.Participant.ParticipantValue;
                }
                else if (manualActivity.Participant.ParticipantType == ParticipantType.RelateRegular)
                {
                    rblRegular.Checked = true;
                    txtRegularApp.Text = manualActivity.Participant.ParticipantValue;
                }


                //表单数据    
                //List<string> formList = manualActivity.Form.ToSafeString();
                gvformList.DataSource = manualActivity.Form.Fields;
                gvformList.DataBind();
                txtDataSource.Text = manualActivity.Form.DataSource;
                txtTitle.Text = manualActivity.Form.Title;

                //时间限制
                chbIsTimeLimitSet.Checked = manualActivity.TimeLimit.IsTimeLimitSet;
                cboCalendarType.BindCombox(CalendarType.CDefault);
                cboCalendarType.SelectedValue = ((short)manualActivity.TimeLimit.CalendarSet.Type).ToSafeString();

                if (manualActivity.TimeLimit.TimeLimitInfo.TimeLimitStrategy == TimeLimitStrategy.LimitTime)
                {
                    rabTimeLimitStrategy.Checked = true;
                    txtLimitTimeHour.Text = manualActivity.TimeLimit.TimeLimitInfo.LimitTimeHour.ToSafeString();
                    txtLimitTimeMinute.Text = manualActivity.TimeLimit.TimeLimitInfo.LimitTimeMinute.ToSafeString();
                }
                else
                {
                    rabRelevantLimitTime.Checked = true;
                    txtRelevantData.Text = manualActivity.TimeLimit.TimeLimitInfo.RelevantData;
                }
                chbIsSendMessageForOvertime.Checked = manualActivity.TimeLimit.TimeLimitInfo.IsSendMessageForOvertime;
                if (manualActivity.TimeLimit.RemindInfo.RemindStrategy == RemindStrategy.RemindLimtTime)
                {
                    rabRemindLimtTime.Checked = true;
                    txtRemindLimtTimeHour.Text = manualActivity.TimeLimit.RemindInfo.RemindLimtTimeHour.ToSafeString();
                    txtRemindLimtTimeMinute.Text = manualActivity.TimeLimit.RemindInfo.RemindLimtTimeMinute.ToSafeString();
                }
                else
                {
                    rabRelevantLimitTime.Checked = true;
                    txtRemindRelevantData.Text = manualActivity.TimeLimit.RemindInfo.RemindRelevantData;
                }
                chbisSendMessageForRemind.Checked = manualActivity.TimeLimit.RemindInfo.IsSendMessageForRemind;

                //多工作项
                chbIsMulWIValid.Checked = manualActivity.MultiWorkItem.IsMulWIValid; //启动多工作项设置
                manualActivity.MultiWorkItem.WorkItemNumStrategy = rblParticipantNumber.Checked ? WorkItemNumStrategy.ParticipantNumber : WorkItemNumStrategy.OperatorNumber; // 多工作项分配策略
                if (manualActivity.MultiWorkItem.IsSequentialExecute)//顺序执行工作项
                {
                    rabYIsSequentialExecute.Checked = true;
                }
                else
                {
                    rabNIsSequentialExecute.Checked = true;
                }

                if (manualActivity.MultiWorkItem.FinishRule == FinishRule.FinishAll)  //完成规则设定
                {
                    rblFinishAll.Checked = true;
                }
                else if (manualActivity.MultiWorkItem.FinishRule == FinishRule.SpecifyNum)
                {
                    rblSpecifyNum.Checked = true;
                    txtFinishRquiredNum.Text = manualActivity.MultiWorkItem.FinishRquiredNum.ToSafeString();//要求完成个数
                }
                else
                {
                    rblSpecifyPercent.Checked = true;
                    txtFinishRequiredPercent.Text = manualActivity.MultiWorkItem.FinishRequiredPercent.ToSafeString();//要求完成百分比
                }
                if (manualActivity.MultiWorkItem.IsAutoCancel)//自动终止未完成工作项
                {
                    rabYIsAutoCancel.Checked = true;
                }
                else
                {
                    rabNIsAutoCancel.Checked = true;
                }

                //触发事件
                if (manualActivity.TriggerEvents != null)
                {
                    rptTriggerEvent.DataSource = manualActivity.TriggerEvents;
                    rptTriggerEvent.DataBind();
                }

                //回退
                cboRollbackType.BindCombox(ActionPattern.Method);
                cboRollbackType.SelectedValue = ((int)(object)manualActivity.RollBack.ActionPattern).ToSafeString(); //类型
                txtRollbackAction.Text = manualActivity.RollBack.ApplicationUri; //动作
                rptRollback.DataSource = manualActivity.RollBack.Parameters;
                rptRollback.DataBind();

                //自由流
                chbIsFreeActivity.Checked = manualActivity.FreeFlowRule.IsFreeActivity; //设置该活动为自由活动
                if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinProcess)
                {
                    rblFreeWithinProcess.Checked = true;
                }
                else if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinActivities)
                {
                    rblFreeWithinActivities.Checked = true;
                }
                else if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinNextActivites)
                {
                    rblFreeWithinNextActivites.Checked = true;
                }; //自由范围设置策略
                rptFreeRange.DataSource = manualActivity.FreeFlowRule.FreeRangeActivities;
                rptFreeRange.DataBind();

                chbIsOnlyLimitedManualActivity.Checked = manualActivity.FreeFlowRule.IsOnlyLimitedManualActivity; //流向的目标活动仅限于人工活动

                //启动策略
                if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.DirectRunning)
                {
                    rblDirectRunning.Checked = true;
                }
                else if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.WaitActivate)
                {
                    rblDisenabled.Checked = true;
                }
                else if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.AutoSkip)//可选规则
                {
                    rblAutoAfter.Checked = true;
                }
                txtActivateRuleApp.Text = manualActivity.ActivateRule.ActivateRuleApp; //规则逻辑

                if (manualActivity.ResetParticipant == ResetParticipant.FirstParticipantor)
                {
                    rbFirstParticipantor.Checked = true;
                }
                else if (manualActivity.ResetParticipant == ResetParticipant.LastParticipantor)  //重新启动规则
                {
                    rbLastParticipantor.Checked = true;
                }

                chbIsSpecifyURL.Checked = manualActivity.ResetURL.IsSpecifyURL; //重新设置URL
                cboURLType.BindCombox(URLType.DefaultURL);
                cboURLType.SelectedValue = ((int)(object)manualActivity.ResetURL.URLType).ToSafeString(); //URL类型
                txt2SpecifyURL.Text = manualActivity.ResetURL.SpecifyURL;

            }
            else if (ActivityInfo.ActivityType == ActivityType.StartActivity)
            {
                StartActivity startActivity = ActivityInfo as StartActivity;
                //基本
                txtID.Text = startActivity.ID;
                txtName.Text = startActivity.Name;
                cboSplitType.BindCombox(SplitType.AND);
                cboSplitType.SelectedValue = ((int)(object)startActivity.SplitType).ToSafeString();
                txtDescription.Text = startActivity.Description; //描述
            }
            else if (ActivityInfo.ActivityType == ActivityType.EndActivity)
            {
                EndActivity endActivity = ActivityInfo as EndActivity;
                //基本
                txtID.Text = endActivity.ID;
                txtName.Text = endActivity.Name;
                cboJoinType.BindCombox(JoinType.AND);
                cboJoinType.SelectedValue = ((int)(object)endActivity.JoinType).ToSafeString();
                txtDescription.Text = endActivity.Description; //描述
            }
            else if (ActivityInfo.ActivityType == ActivityType.AutoActivity)
            {
                AutoActivity autoActivity = ActivityInfo as AutoActivity;
                //基本
                txtID.Text = autoActivity.ID;
                txtName.Text = autoActivity.Name;
                cboJoinType.BindCombox(JoinType.AND);
                cboJoinType.SelectedValue = ((int)(object)autoActivity.JoinType).ToSafeString();

                cboSplitType.BindCombox(SplitType.AND);
                cboSplitType.SelectedValue = ((int)(object)autoActivity.SplitType).ToSafeString();
                txtDescription.Text = autoActivity.Description; //描述
            }
        }
        public void Bind4DropDownList(Enum value, DropDownList dropDownList)
        {
            Dictionary<string, string> values = value.GetRemarks();
            dropDownList.Items.Clear();
            foreach (var item in values)
            {
                System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem(item.Value, item.Key);
                dropDownList.Items.Add(listItem);
            }
        }

        private bool IsExistName()
        {
            bool isTrue = false;
            List<Activity> list = wfEngine.GetProcessDefine(ProcessDefID).Activities.ToList();
            foreach (Activity activity in list)
            {
                if (txtName.Text == activity.Name)
                {
                    isTrue = true;
                    break;
                }
            }
            return isTrue;
        }

        public string Save(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult actionResult = DoResult.Failed;
            string actionMessage = string.Empty;
            if (!string.IsNullOrEmpty(TypeName))
            {
                string processDefID = Request.Form["ProcessDefID"];
                string activityID = Request.Form["ActivityID"];
                try
                {
                    Type type = Assembly.Load("AgileEAP.Workflow").GetType(string.Format("AgileEAP.Workflow.Definition.{0}", TypeName));
                    if (TypeName == ActivityType.StartActivity.ToSafeString())
                    {
                        argument = argument.Replace("JoinType", "");
                    }
                    if (TypeName == ActivityType.EndActivity.ToSafeString())
                    {
                        argument = argument.Replace("SplitType", "");
                    }

                    Activity activity = JsonConvert.DeserializeObject(argument, type) as Activity;
                    activity.ActivityType = TypeName.Cast<ActivityType>(ActivityType.ManualActivity); ;
                    ProcessDefine processDefine = wfEngine.GetProcessDefine(ProcessDefID);
                    ProcessDef processDef = wfEngine.Persistence.Repository.GetDomain<ProcessDef>(ProcessDefID);
                    processDefine.SafeAddActivity(activityID, activity);
                    processDef.Content = processDefine.ToXml();
                    wfEngine.Persistence.Repository.SaveOrUpdate(processDef);

                    actionResult = DoResult.Success;
                    //获取提示信息
                    actionMessage = RemarkAttribute.GetEnumRemark(actionResult);

                    //记录操作日志
                    AddActionLog(processDef, actionResult, string.Format("保存流程{0}活动{1}", processDef.Name, activity.Name));

                    ajaxResult.Result = actionResult;
                    ajaxResult.RetValue = activity.ID;
                    ajaxResult.PromptMsg = actionMessage;
                }
                catch (Exception ex)
                {
                    ajaxResult.Result = DoResult.Failed;
                    AddActionLog<ProcessDef>(string.Format("保存流程{0}活动{1}", processDefID, activityID), ajaxResult.Result);
                    log.Error(actionMessage, ex);
                }

            }
            return JsonConvert.SerializeObject(ajaxResult);

        }
        protected void cboActivityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtID.Text = IdGenerator.NewComb().ToString();
        }
    }
}