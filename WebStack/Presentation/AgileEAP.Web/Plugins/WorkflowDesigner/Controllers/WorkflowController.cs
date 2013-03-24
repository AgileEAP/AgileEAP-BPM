using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;
using AgileEAP.Core.Extensions;

using System.Web.Mvc;

using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using System.ComponentModel;

using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Definition;
using System.Xml;
using AgileEAP.Core.Caching;
namespace AgileEAP.Plugin.WorkflowDesigner.Controllers
{
    public class WorkflowController : BaseController
    {
        public WorkflowController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WorkflowDesigner()
        {
            return View();
        }
        public JsonResult SaveProcessDefine()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };

            try
            {
                string json = Request.Form["processDefContent"];
                ProcessDefine processDefine = JsonConvert.DeserializeObject<ProcessDefine>(json, new ActivityConvert());
                string processDefContent = processDefine.ToXml();
                string name = Request.Form["Name"];
                string text = Request.Form["text"];
                string version = Request.Form["version"];
                string description = Request.Form["description"];
                string startor = Request.Form["startor"];
                string isActive = Request.Form["isActive"];
                string categoryID = Request.Form["categoryID"];
                string currentFlag = Request.Form["currentFlag"];
                string currentStatus = Request.Form["currentStatus"];
                string processDefID = Request.Form["processDefID"];
                if (!string.IsNullOrEmpty(processDefContent))
                {
                    string ID = IdGenerator.NewGuid().ToString();
                    if (!string.IsNullOrEmpty(processDefID))
                    {
                        ID = processDefID;
                    }
                    ProcessDef proDef = repository.Query<ProcessDef>().Where(p => p.ID != processDefID && p.Name == processDefine.ID && p.Version == processDefine.Version).FirstOrDefault();
                    if (proDef != null)
                    {
                        ajaxResult.RetValue = false;
                        ajaxResult.Result = DoResult.Success;
                        ajaxResult.PromptMsg = "操作失败，该流程已经存在，选择该流程修改";
                    }
                    else
                    {
                        ProcessDef processDef = new ProcessDef()
                        {
                            ID = ID,
                            Name = name,
                            Text = text,
                            Version = version,
                            Description = description,
                            CreateTime = DateTime.Now,
                            Creator = workContext.User.LoginName,
                            Startor = startor,
                            IsActive = isActive.ToShort(),
                            CurrentFlag = currentFlag.ToShort(),
                            CurrentState = currentStatus.ToShort(),
                            Content = processDefContent,
                            CategoryID = categoryID,

                        };
                        repository.SaveOrUpdate(processDef);
                        ajaxResult.RetValue = processDef;
                        ajaxResult.Result = DoResult.Success;
                        ajaxResult.PromptMsg = "操作成功";
                        CacheManager.Remove(ID);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("创建工作流失败" + ex.Message);
                ajaxResult.RetValue = false;
                ajaxResult.Result = DoResult.Failed;
                ajaxResult.PromptMsg = "操作失败";
            }
            return Json(ajaxResult);
        }

        public JsonResult GetProcessDefine()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };

            try
            {
                string processDefineID = Request.Form["ProcessDefineID"];
                ProcessDef processDef = repository.Query<ProcessDef>().FirstOrDefault(p => p.ID == processDefineID);
                if (processDef != null)
                {
                    ajaxResult.RetValue = processDef;
                    ajaxResult.Result = DoResult.Success;
                    ajaxResult.PromptMsg = "操作成功";
                }
            }
            catch (Exception ex)
            {
                log.Error("创建工作流失败" + ex.Message);
            }
            return Json(ajaxResult);
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

        public ActionResult ActivityDetail()
        {
            return View();
        }

        public JsonResult GetprocessDefContent()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            try
            {
                string json = Request.Form["processDefContent"];
                ProcessDefine processDefine = JsonConvert.DeserializeObject<ProcessDefine>(json, new ActivityConvert());
                string processDefContent = processDefine.ToXml();
                ajaxResult.RetValue = processDefContent;
                ajaxResult.Result = DoResult.Success;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                log.Error("获取工作流定义失败" + ex.Message);
                ajaxResult.RetValue = false;
                ajaxResult.Result = DoResult.Failed;
                ajaxResult.PromptMsg = "操作失败";
            }
            return Json(ajaxResult);
        }

        public JsonResult DrawProcess()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            string processDefID = Request.Form["processDefID"];
            string processDefContent = Request.Form["processDefContent"];
            ProcessDefine processDefine = new ProcessDefine(processDefContent, false);
            string processInstID = Request.Form["ProcessInstID"];
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

            return Json(ajaxResult, new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
