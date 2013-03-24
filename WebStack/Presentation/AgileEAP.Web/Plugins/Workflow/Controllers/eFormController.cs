using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;
using AgileEAP.Core.Caching;
using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC.Security;

using AgileEAP.Workflow.Domain;
using AgileEAP.Workflow.Definition;
using AgileEAP.Workflow.Engine;
using System.Xml;
using AgileEAP.Workflow.Enums;
using System.Text;
using AgileEAP.Infrastructure.Domain;
using System.IO;


namespace AgileEAP.Plugin.Workflow.Controllers
{
    public class eFormController : BaseController
    {
        private IWorkflowEngine workflowEngine;
        public eFormController(IWorkflowEngine workflowEngine, IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {
            this.workflowEngine = workflowEngine;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public string GetSysVarValue(string varName, string defaultValue)
        {
            return string.Empty;

        }
       /*qlj 2013.1.8改  将表单独立于流程之外*/
        public ActionResult Index()
        {
            //IWorkflowEngine engine = new WorkflowEngine();
            try
            {
                string processDefID = Request.QueryString["processDefID"];
                string entry = Request.QueryString["Entry"];
                string eFormID = Request.QueryString["eFormID"];
                ManualActivity manualActivity = null;
                eForm formInfo = null;
                if (string.IsNullOrEmpty(processDefID) && !string.IsNullOrEmpty(eFormID))
                {
                    formInfo = repository.GetDomain<eForm>(eFormID);
                }
                else
                {
                    if (!string.IsNullOrEmpty(processDefID) && entry.EqualIgnoreCase("StartProcess"))
                    {
                        Activity startActivity = workflowEngine.Persistence.GetStartActivity(processDefID);
                        manualActivity = workflowEngine.Persistence.GetOutActivities(processDefID, startActivity.ID)[0] as ManualActivity;
                    }
                    else
                    {
                        string workItemID = Request.QueryString["workItemID"];
                        WorkItem workItem = repository.GetDomain<WorkItem>(workItemID);
                        ActivityInst activityInst = workflowEngine.Persistence.GetActivityInst(workItem.ActivityInstID);
                        manualActivity = workflowEngine.Persistence.GetActivity(workItem.ProcessID, activityInst.ActivityDefID) as ManualActivity;
                    }
                }
                if (manualActivity != null && manualActivity.eForm != null)
                {
                    formInfo = repository.GetDomain<eForm>(manualActivity.eForm);
                }
                   if (formInfo != null&&!string.IsNullOrEmpty(formInfo.Content))
                   {
                     Form  form=JsonConvert.DeserializeObject<Form>(formInfo.Content);
                       string processInstID = Request.QueryString["processInstID"];
                       ProcessForm processForm = null;
                       IDictionary<string, object> values = new Dictionary<string, object>();
                       if (!string.IsNullOrEmpty(processInstID))
                       {
                           IDictionary<string, object> parameters = new Dictionary<string, object>();
                           parameters.SafeAdd("processInstID", processInstID);
                           processForm = repository.FindOne<ProcessForm>(parameters);
                           if (processForm != null)
                           {
                               parameters.Clear();
                               parameters.Add("ID", processForm.BizID);
                               DataTable dt = repository.ExecuteDataTable<ProcessForm>(string.Format("select * from {0}", processForm.BizTable), parameters);
                               if (dt != null && dt.Rows.Count > 0)
                               {

                                   foreach (DataRow row in dt.Rows)
                                   {
                                       foreach (var field in form.Fields)//manualActivity.Form.Fields)
                                       {

                                           if (field.ControlType == ControlType.SysVariable || field.ControlType == ControlType.HiddenInput)
                                           {
                                               switch (field.DefaultValue.ToInt())
                                               {
                                                   case (short)SystemControlType.OrgID:
                                                       Organization org = repository.Query<Organization>().FirstOrDefault(o => o.Code == workContext.User.OrgID);
                                                       values.SafeAdd(field.Name, org.Name);
                                                       if (!string.IsNullOrEmpty(field.ExtendData))
                                                       {
                                                           values.SafeAdd(field.ExtendData, workContext.User.OrgID);
                                                       }
                                                       break;
                                                   case (short)SystemControlType.UserID:
                                                       values.SafeAdd(field.Name, workContext.User.Name);
                                                       if (!string.IsNullOrEmpty(field.ExtendData))
                                                       {
                                                           values.SafeAdd(field.ExtendData, workContext.User.ID);
                                                       }
                                                       break;
                                                   case (short)SystemControlType.CurrentDate:
                                                       values.SafeAdd(field.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                       break;
                                               }

                                           }
                                           else if (dt.Columns.Contains(field.Name))
                                           {
                                               if (!string.IsNullOrEmpty(row[field.Name].ToSafeString()))
                                               {
                                                   values.SafeAdd(field.Name, row[field.Name]);
                                               }
                                               else
                                               {
                                                   if (!string.IsNullOrEmpty(field.DefaultValue))
                                                       values.SafeAdd(field.Name, field.DefaultValue);
                                               }
                                           }

                                       }
                                   }
                               }
                           }
                       }
                       if (processForm == null)
                       {
                           foreach (var field in form.Fields)//manualActivity.Form.Fields)
                           {
                               if (field.ControlType == ControlType.SysVariable||field.ControlType==ControlType.HiddenInput)
                               {
                                   switch (field.DefaultValue.ToInt())
                                   {
                                       case (short)SystemControlType.OrgID:
                                           Organization org = repository.Query<Organization>().FirstOrDefault(o => o.Code == workContext.User.OrgID);
                                           values.SafeAdd(field.Name, org.Name);
                                           if (!string.IsNullOrEmpty(field.ExtendData))
                                           {
                                               values.SafeAdd(field.ExtendData, workContext.User.OrgID);
                                           }
                                           break;
                                       case (short)SystemControlType.UserID:
                                           values.SafeAdd(field.Name, workContext.User.Name);
                                           if (!string.IsNullOrEmpty(field.ExtendData))
                                           {
                                               values.SafeAdd(field.ExtendData, workContext.User.ID);
                                           }
                                           break;
                                       case (short)SystemControlType.CurrentDate:
                                           values.SafeAdd(field.Name, DateTime.Now.ToSafeDateTime());
                                           break;
                                   }
                               }
                               else if (field.ControlType != ControlType.SysVariable && field.ControlType != ControlType.HiddenInput)
                               {
                                   if (!string.IsNullOrEmpty(field.DefaultValue))
                                       values.SafeAdd(field.Name, field.DefaultValue);
                               }
                               else
                               {
                                   // values.SafeAdd(field.Name, workContext.);
                               }
                           }
                       }
                       ViewData["Values"] = values;
                       ViewData["Form"] = form;//manualActivity.Form;
                   }
                //}
            }
            catch (Exception ex)
            {
                log.Error("初始化表单失败" + ex);
            }
            return View();
        }


        public JsonResult Submit(string json)
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            ProcessForm processForm = null;
            try
            {
                string processDefID = Request.Form["processDefID"];
                string entry = Request.Form["Entry"];
                string processInstID = Request.Form["processInstID"];
                string workItemID = Request.Form["workItemID"];
                UnitOfWork.ExecuteWithTrans<ProcessDef>(() =>
                {
                    if (!string.IsNullOrEmpty(processDefID) && entry.EqualIgnoreCase("StartProcess"))
                    {
                        processInstID = workflowEngine.CreateAProcess(processDefID);// string.Empty; // TODO: Initialize to an appropriate value
                        workflowEngine.StartAProcess(processInstID, null);
                        WorkItem wi = repository.Query<WorkItem>().FirstOrDefault(w => w.ProcessInstID == processInstID);
                        workItemID = wi.ID;
                        AddActionLog(wi, DoResult.Success, string.Format("启动流程实像{0}", processInstID));
                    }

                    Dictionary<string, object> formValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string table = Request.Form["DataSource"];
                    string cmdText = string.Empty;

                    processForm = repository.Query<ProcessForm>().FirstOrDefault(pf => pf.ProcessInstID == processInstID);
                    if (processForm != null)
                    {
                        StringBuilder sbUpdateValues = new StringBuilder();
                        foreach (var item in formValues)
                        {
                            if (sbUpdateValues.Length > 0)
                                sbUpdateValues.Append(",");
                            sbUpdateValues.AppendFormat("{0} = :{0}", item.Key);
                        }
                        cmdText = string.Format("update {0} set {1} where ID ='{2}'", table, sbUpdateValues.ToString(), processForm.BizID);
                    }
                    else
                    {
                        formValues.SafeAdd("ID", IdGenerator.NewComb().ToSafeString());
                        formValues.SafeAdd("Applicant", workContext.User.ID);
                        formValues.SafeAdd("ApplicantName", workContext.User.Name);
                        formValues.SafeAdd("ApplyTime", DateTime.Now);
                        StringBuilder sbFields = new StringBuilder();
                        StringBuilder sbValues = new StringBuilder();
                        foreach (var item in formValues)
                        {
                            sbFields.Append(item.Key).Append(",");
                            sbValues.Append(":").Append(item.Key).Append(",");
                        }

                        processForm = new ProcessForm()
                        {
                            ID = IdGenerator.NewComb().ToString(),
                            BizID = formValues.GetSafeValue<string>("ID"),
                            BizTable = table,
                            CreateTime = DateTime.Now,
                            Creator = workContext.User.ID,
                            ProcessInstID = processInstID,
                            KeyWord = sbValues.ToString()
                        };
                        cmdText = string.Format("insert into {0}({1}) values({2})", table, sbFields.ToString().TrimEnd(','), sbValues.ToString().TrimEnd(','));
                    }

                    workflowEngine.Persistence.Repository.ExecuteSql<WorkItem>(cmdText, formValues);
                    workflowEngine.Persistence.Repository.SaveOrUpdate(processForm);
                    workflowEngine.CompleteWorkItem(workItemID, formValues);
                });
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = processForm.BizID;
                ajaxResult.PromptMsg = string.Format("完成工作流实例{0}工作项{1}", processForm.ProcessInstID, workItemID);
                //记录操作日志
                AddActionLog(processForm, ajaxResult.Result, ajaxResult.PromptMsg);
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                //记录操作日志
                AddActionLog(processForm, ajaxResult.Result, string.Format("完成工作流实例{0}工作项失败", processForm.ProcessInstID));
                log.Error(ex);
            }

            return Json(ajaxResult);

        }

        public JsonResult Rollback(string json)
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            string workItemID = Request.QueryString["workItemID"];
            string actionMessage = string.Format("退回工作流实例工作项{0}", workItemID);
            try
            {
                //回退工作项
                workflowEngine.RollbackWorkItem(workContext.User, workItemID);

                ajaxResult.Result = DoResult.Success;
                ajaxResult.PromptMsg = actionMessage;
            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                log.Error(actionMessage, ex);
            }
            finally
            {
                AddActionLog<WorkItem>(actionMessage, ajaxResult.Result);
            }

            return Json(ajaxResult);
        }


        public JsonResult GetDict()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            string id = Request.Form["DataSource"];
            string Name = Request.Form["Name"];
            try
            {
                Dictionary<string, object> dictItems = new Dictionary<string, object>();
                IList<DictItem> dicts = repository.Query<DictItem>().Where(d => d.DictID == id).ToList();
                if (dicts != null && dicts.Count() > 0)
                {
                    foreach (var dict in dicts)
                    {
                        dictItems.Add(dict.Value, dict.Text);
                    }
                }
                var retValue = new
                {
                    Name = Name,
                    dictItems = dictItems
                };
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = retValue;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                ajaxResult.PromptMsg = "操作失败";
                log.Error(ex);
            }
            return Json(ajaxResult);
        }

        public JsonResult GetFileInfo()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            string bizID = Request.Form["BizID"];
            string Name = Request.Form["Name"];
            try
            {
                IList<string> fileItems = new List<string>();
                IList<UploadFile> fileInfos = repository.Query<UploadFile>().Where(d => d.BizID == bizID).ToList();
                if (fileInfos != null && fileInfos.Count() > 0)
                {
                    foreach (var fileInfo in fileInfos)
                    {
                        fileItems.Add(fileInfo.FilePath);
                    }
                }
                var retValue = new
                {
                    Name = Name,
                    fileItems = fileItems
                };
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = retValue;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                ajaxResult.PromptMsg = "操作失败";
                log.Error(ex);
            }
            return Json(ajaxResult);
        }

        public ContentResult Upload(System.Web.HttpPostedFileBase FileData, string folder)
        {
            string fileName = "";
            if (null != FileData)
            {
                try
                {
                    fileName = Path.GetFileName(FileData.FileName);//获得文件名 
                    string ext = Path.GetExtension(FileData.FileName);//获得文件扩展名 
                    string processInstID = Request.Form["processInstID"];
                    string saveName = Request.Form["name"] + "$" + fileName; //实际保存文件名 
                    string path = System.Configuration.ConfigurationManager.AppSettings["FileDirectory"];
                    string filePath = Request.MapPath(path + "/Workflow/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    FileData.SaveAs(filePath + saveName);
                    UploadFile uploadFile = new UploadFile()
                    {
                        ID = IdGenerator.NewGuid().ToSafeString(),
                        UniqueName = saveName,
                        FileName = fileName,
                        CreateTime = DateTime.Now,
                        FilePath = path + "/Workflow/" + saveName,
                        FileType = 0,
                        Creator = workContext.User.ID,
                        BizID = processInstID
                    };
                    repository.SaveOrUpdate(uploadFile);
                }
                catch (Exception ex)
                {
                    fileName = ex.Message;
                }
            }
            return Content(fileName);
        }

        public void download(string Path, string fileName)
        {
            System.IO.Stream iStream = null;
            byte[] buffer = new Byte[10000];
            int length;
            long dataToRead;
            string filepath = Server.MapPath(Path);
            try
            {
                iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.Read);
                dataToRead = iStream.Length;
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fileName));//System.Text.UTF8Encoding.UTF8.GetBytes(FileName)  
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10000);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
            }
        }

        public JsonResult CreateJSFile()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            try
            {
                string ProcessDef = Request.Form["ProcessDefID"];
                string ActivityID = Request.Form["ActivityID"];
                string Content = Request.Form["jsContent"];
                string path = Request.MapPath("/Plugins/FormDesigner/Scripts/");
                string name = ProcessDef + ActivityID + ".js";
                if (!System.IO.File.Exists(path + name))
                {
                    FileStream fs1 = new FileStream(path + name, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(Content);//要写入的信息。 
                    sw.Close();
                    fs1.Close();
                }
                else
                {
                    FileStream fs = new FileStream(path + name, FileMode.Open, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(Content);//开始写入值
                    sr.Close();
                    fs.Close();
                }
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = path + name;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                ajaxResult.PromptMsg = "操作失败";
                log.Error(ex);
            }
            return Json(ajaxResult);
        }

      

    }
}
