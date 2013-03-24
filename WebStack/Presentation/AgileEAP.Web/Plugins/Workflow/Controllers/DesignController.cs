using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Infrastructure;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Core.Extensions;

using System.Web.Mvc;

using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using System.ComponentModel;

using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Engine;
using AgileEAP.Workflow.Enums;


namespace AgileEAP.Plugin.Workflow.Controllers
{
    public class DesignController : BaseController
    {
        WorkflowEngine wfEngine = new WorkflowEngine();
        public DesignController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }

        public string ProcessDefID
        {
            get
            {
                return Request.QueryString["ProcessDefID"];
            }
        }
        //public string CurrentId
        //{
        //    get
        //    {
        //        return Request.QueryString["CurrentId"];
        //    }
        //}
        //public Activity ActivityInfo
        //{
        //    get
        //    {
        //        ProcessDefine processDefine = wfEngine.GetProcessDefine(ProcessDefID);
        //        return processDefine.Activities.FirstOrDefault(a => a.ID == CurrentId);
        //    }
        //}
        public ActionResult DrawWorkflow()
        {
            return View();
        }
        public ActionResult ActivityDetail()
        {
            // InitUI();
            return View();
        }
        public void InitUI()
        {
            string processDefID = Request.QueryString["ProcessDefID"];
            string ActivityID = Request.QueryString["ActivityID"];
            string activityType = Request.QueryString["ActivityType"];
            if (activityType.ToInt() == (short)ActivityType.ManualActivity || activityType == ActivityType.ManualActivity.ToSafeString())
            {
                ProcessDefine processDefine = repository.Query<ProcessDefine>().Where(p => p.ID == processDefID).FirstOrDefault();
                if (processDefine != null)
                {
                    var ActivityInfo = processDefine.Activities.FirstOrDefault(a => a.ID == ActivityID);
                    ManualActivity manualActivity = ActivityInfo as ManualActivity;
                    //基本
                    //ViewData["manualActivity"] = manualActivity;
                    //txtID.Text = manualActivity.ID;
                    //txtName.Text = manualActivity.Name;
                    //cboJoinType.BindCombox(JoinType.AND);
                    //cboJoinType.SelectedValue = ((int)(object)manualActivity.JoinType).ToSafeString();
                    //cboSplitType.BindCombox(SplitType.AND);
                    //cboSplitType.SelectedValue = ((int)(object)manualActivity.SplitType).ToSafeString();
                    //chbAllowAgent.Checked = manualActivity.AllowAgent;
                    //chbIsSplitTransaction.Checked = manualActivity.IsSplitTransaction; //分割事务
                    //txtDescription.Text = manualActivity.Description; //描述
                    //if (manualActivity.CustomURL.URLType == URLType.DefaultURL)
                    //{
                    //    rbDefaultURL.Checked = true;
                    //}
                    //else if (manualActivity.CustomURL.URLType == URLType.CustomURL)
                    //{
                    //    rbCustomizeURL.Checked = true;
                    //    txtSpecifyURL.Text = manualActivity.CustomURL.SpecifyURL;
                    //}
                    //else if (manualActivity.CustomURL.URLType == URLType.ManualProcess)
                    //{
                    //    rbTask.Checked = true;
                    //}


                    //参与者
                    ViewData["Participant"] = manualActivity.Participant.Participantors;
                    // gvOrgizationOrRole.DataSource = manualActivity.Participant.Participantors;
                    //gvOrgizationOrRole.DataBind();
                    //if (manualActivity.Participant.ParticipantType == ParticipantType.Participantor)
                    //{
                    //    rblOrganization.Checked = true;
                    //    ckbIsAllowAppointParticipants.Checked = manualActivity.Participant.AllowAppointParticipants;
                    //}
                    //else if (manualActivity.Participant.ParticipantType == ParticipantType.ProcessStarter)
                    //{
                    //    rblProcessStarter.Checked = true;
                    //}
                    //else if (manualActivity.Participant.ParticipantType == ParticipantType.ProcessExecutor)
                    //{
                    //    rblSpecialActivity.Checked = true;
                    //    txtspecialActivity.Text = manualActivity.Participant.ParticipantValue;
                    //}
                    //else if (manualActivity.Participant.ParticipantType == ParticipantType.CustomRegular)
                    //{
                    //    rblParticipantRule.Checked = true;
                    //    txtParticipantRule.Text = manualActivity.Participant.ParticipantValue;
                    //}
                    //else if (manualActivity.Participant.ParticipantType == ParticipantType.RelevantData)
                    //{
                    //    rblRelevantData.Checked = true;
                    //    txtspecialPath.Text = manualActivity.Participant.ParticipantValue;
                    //}
                    //else if (manualActivity.Participant.ParticipantType == ParticipantType.RelateRegular)
                    //{
                    //    rblRegular.Checked = true;
                    //    txtRegularApp.Text = manualActivity.Participant.ParticipantValue;
                    //}


                    //表单数据    
                    //List<string> formList = manualActivity.Form.ToSafeString();
                    ViewData["Form"] = repository.GetDomain<eForm>(manualActivity.eForm).Content;
                    //gvformList.DataSource = manualActivity.Form.Fields;
                    //gvformList.DataBind();
                    //txtDataSource.Text = manualActivity.Form.DataSource;
                    //txtTitle.Text = manualActivity.Form.Title;

                    //时间限制
                    //ViewData["TimeLimit"] = manualActivity.TimeLimit;
                    //chbIsTimeLimitSet.Checked = manualActivity.TimeLimit.IsTimeLimitSet;
                    //cboCalendarType.BindCombox(CalendarType.CDefault);
                    //cboCalendarType.SelectedValue = ((short)manualActivity.TimeLimit.CalendarSet.Type).ToSafeString();

                    //if (manualActivity.TimeLimit.TimeLimitInfo.TimeLimitStrategy == TimeLimitStrategy.LimitTime)
                    //{
                    //    rabTimeLimitStrategy.Checked = true;
                    //    txtLimitTimeHour.Text = manualActivity.TimeLimit.TimeLimitInfo.LimitTimeHour.ToSafeString();
                    //    txtLimitTimeMinute.Text = manualActivity.TimeLimit.TimeLimitInfo.LimitTimeMinute.ToSafeString();
                    //}
                    //else
                    //{
                    //    rabRelevantLimitTime.Checked = true;
                    //    txtRelevantData.Text = manualActivity.TimeLimit.TimeLimitInfo.RelevantData;
                    //}
                    //chbIsSendMessageForOvertime.Checked = manualActivity.TimeLimit.TimeLimitInfo.IsSendMessageForOvertime;
                    //if (manualActivity.TimeLimit.RemindInfo.RemindStrategy == RemindStrategy.RemindLimtTime)
                    //{
                    //    rabRemindLimtTime.Checked = true;
                    //    txtRemindLimtTimeHour.Text = manualActivity.TimeLimit.RemindInfo.RemindLimtTimeHour.ToSafeString();
                    //    txtRemindLimtTimeMinute.Text = manualActivity.TimeLimit.RemindInfo.RemindLimtTimeMinute.ToSafeString();
                    //}
                    //else
                    //{
                    //    rabRelevantLimitTime.Checked = true;
                    //    txtRemindRelevantData.Text = manualActivity.TimeLimit.RemindInfo.RemindRelevantData;
                    //}
                    //chbisSendMessageForRemind.Checked = manualActivity.TimeLimit.RemindInfo.IsSendMessageForRemind;

                    //多工作项
                    // ViewData["MultiWorkItem"] = manualActivity.MultiWorkItem;
                    //chbIsMulWIValid.Checked = manualActivity.MultiWorkItem.IsMulWIValid; //启动多工作项设置
                    //manualActivity.MultiWorkItem.WorkItemNumStrategy = rblParticipantNumber.Checked ? WorkItemNumStrategy.ParticipantNumber : WorkItemNumStrategy.OperatorNumber; // 多工作项分配策略
                    //if (manualActivity.MultiWorkItem.IsSequentialExecute)//顺序执行工作项
                    //{
                    //    rabYIsSequentialExecute.Checked = true;
                    //}
                    //else
                    //{
                    //    rabNIsSequentialExecute.Checked = true;
                    //}

                    //if (manualActivity.MultiWorkItem.FinishRule == FinishRule.FinishAll)  //完成规则设定
                    //{
                    //    rblFinishAll.Checked = true;
                    //}
                    //else if (manualActivity.MultiWorkItem.FinishRule == FinishRule.SpecifyNum)
                    //{
                    //    rblSpecifyNum.Checked = true;
                    //    txtFinishRquiredNum.Text = manualActivity.MultiWorkItem.FinishRquiredNum.ToSafeString();//要求完成个数
                    //}
                    //else
                    //{
                    //    rblSpecifyPercent.Checked = true;
                    //    txtFinishRequiredPercent.Text = manualActivity.MultiWorkItem.FinishRequiredPercent.ToSafeString();//要求完成百分比
                    //}
                    //if (manualActivity.MultiWorkItem.IsAutoCancel)//自动终止未完成工作项
                    //{
                    //    rabYIsAutoCancel.Checked = true;
                    //}
                    //else
                    //{
                    //    rabNIsAutoCancel.Checked = true;
                    //}

                    //触发事件
                    ViewData["TriggerEvents"] = manualActivity.TriggerEvents;
                    //if (manualActivity.TriggerEvents != null)
                    //{
                    //    rptTriggerEvent.DataSource = manualActivity.TriggerEvents;
                    //    rptTriggerEvent.DataBind();
                    //}

                    //回退
                    ViewData["RollBack"] = manualActivity.RollBack.Parameters;
                    //cboRollbackType.BindCombox(ActionPattern.Method);
                    //cboRollbackType.SelectedValue = ((int)(object)manualActivity.RollBack.ActionPattern).ToSafeString(); //类型
                    //txtRollbackAction.Text = manualActivity.RollBack.ApplicationUri; //动作
                    // rptRollback.DataSource =ManualActivity manualActivity.RollBack.Parameters;
                    //rptRollback.DataBind();

                    //自由流
                    ViewData["FreeFlowRule"] = manualActivity.FreeFlowRule.FreeRangeActivities;
                    //chbIsFreeActivity.Checked = manualActivity.FreeFlowRule.IsFreeActivity; //设置该活动为自由活动
                    //if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinProcess)
                    //{
                    //    rblFreeWithinProcess.Checked = true;
                    //}
                    //else if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinActivities)
                    //{
                    //    rblFreeWithinActivities.Checked = true;
                    //}
                    //else if (manualActivity.FreeFlowRule.FreeRangeStrategy == FreeRangeStrategy.FreeWithinNextActivites)
                    //{
                    //    rblFreeWithinNextActivites.Checked = true;
                    //}; 
                    //自由范围设置策略
                    //ViewData["FreeFlowRule"] = manualActivity.FreeFlowRule;
                    //rptFreeRange.DataSource = manualActivity.FreeFlowRule.FreeRangeActivities;
                    //rptFreeRange.DataBind();

                    //chbIsOnlyLimitedManualActivity.Checked = manualActivity.FreeFlowRule.IsOnlyLimitedManualActivity; //流向的目标活动仅限于人工活动

                    //启动策略
                    // ViewData["ActivateRule"] = manualActivity.ActivateRule;
                    //if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.DirectRunning)
                    //{
                    //    rblDirectRunning.Checked = true;
                    //}
                    //else if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.WaitActivate)
                    //{
                    //    rblDisenabled.Checked = true;
                    //}
                    //else if (manualActivity.ActivateRule.ActivateRuleType == ActivateRuleType.AutoSkip)//可选规则
                    //{
                    //    rblAutoAfter.Checked = true;
                    //}
                    //txtActivateRuleApp.Text = manualActivity.ActivateRule.ActivateRuleApp; //规则逻辑
                    //重新设置Participant
                    // ViewData["ResetParticipant"] = manualActivity.ResetParticipant;
                    //if (manualActivity.ResetParticipant == ResetParticipant.FirstParticipantor)
                    //{
                    //    rbFirstParticipantor.Checked = true;
                    //}
                    //else if (manualActivity.ResetParticipant == ResetParticipant.LastParticipantor)  //重新启动规则
                    //{
                    //    rbLastParticipantor.Checked = true;
                    //}
                    //重新设置URL
                    // ViewData["ResetURL"] = manualActivity.ResetURL;
                    //chbIsSpecifyURL.Checked = manualActivity.ResetURL.IsSpecifyURL; 
                    //cboURLType.BindCombox(URLType.DefaultURL);
                    //cboURLType.SelectedValue = ((int)(object)manualActivity.ResetURL.URLType).ToSafeString(); //URL类型
                    //txt2SpecifyURL.Text = manualActivity.ResetURL.SpecifyURL;
                }
                else
                {
                    ViewData["Participant"] = new List<Participantor>();
                    ViewData["Form"] = new List<FormField>();
                    ViewData["TriggerEvents"] = new List<TriggerEvent>();
                    ViewData["RollBack"] = new List<Parameter>();
                    ViewData["FreeFlowRule"] = new List<Activity>();
                }
            }


        }
        public JsonResult GetProcessInfo()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };

            string processDefID = Request.Form["ProcessDefID"];
            if (!string.IsNullOrEmpty(processDefID))
            {
                ProcessDef processDef = repository.Query<ProcessDef>().Where(p => p.ID == processDefID).FirstOrDefault();
                //System.Xml.Linq.XElement xElem = System.Xml.Linq.XElement.Parse(processDef.Content);
                //IList<string> typeNames=xElem.Element("activities").Elements("activity").Select(e =>  e.Element("activityType").Value).ToList();
                //string typeName = typeNames[0];
                //if (!typeName.StartsWith("AgileEAP.Workflow.Definition"))
                //    typeName = string.Format("AgileEAP.Workflow.Definition.{0}", typeName);
                //System.Reflection.Assembly asmb = System.Reflection.Assembly.LoadFrom("AgileEAP.Workflow.dll");
                //Type type = asmb.GetType(typeName);
                //Activity activity = Activator.CreateInstance(type, new object[] { this, xElem }) as Activity;
                ProcessDefine processDefine = new ProcessDefine(processDef.Content, false);
                string processInstID = Request.Form["ProcessInstID"];
                if (!string.IsNullOrEmpty(processInstID))
                {
                    IList<TransControl> transList = new List<TransControl>();
                    try
                    {
                        IList<ActivityInst> activityInsts = repository.Query<ActivityInst>().Where(a => a.ProcessInstID == processInstID).OrderBy(a => a.CreateTime).ToList();
                        if (activityInsts != null && activityInsts.Count > 0)
                        {
                            transList = repository.Query<TransControl>().Where(t => t.ProcessInstID == processInstID).OrderByDescending(t => t.TransTime).ToList();
                        }
                        var process = new { processDefID = processDefID, processInstID = processInstID, processDefine = processDefine, activityInsts = activityInsts, transList = transList };
                        ajaxResult.RetValue = process;
                        ajaxResult.Result = DoResult.Success;
                        ajaxResult.PromptMsg = "操作成功";
                    }
                    catch (Exception ex)
                    {
                        log.Error("获取流程" + processInstID + "异常,原因:" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        var process = new { processDefID = processDefID, processInstID = processInstID, processDefine = processDefine };
                        ajaxResult.RetValue = process;
                        ajaxResult.Result = DoResult.Success;
                        ajaxResult.PromptMsg = "操作成功";
                    }
                    catch (Exception ex)
                    {
                        log.Error("获取流程" + processInstID + "异常,原因:" + ex.Message);
                    }
                }
            }

            return Json(ajaxResult, new Newtonsoft.Json.Converters.StringEnumConverter());
        }
        public ActionResult ConnectionDetail()
        {
            return View();
        }
    }
}
