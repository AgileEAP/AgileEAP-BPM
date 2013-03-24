using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AgileEAP.Core;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Data;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Security;
using AgileEAP.Core.Caching;
using AgileEAP.MVC;
using AgileEAP.MVC.Controllers;
using AgileEAP.Core.Authentication;
using AgileEAP.MVC.Security;
using AgileEAP.Core.Web;
using AgileEAP.Infrastructure.Domain;

using AgileEAP.Plugin.FormDesigner.Models;
using Kendo.Mvc.UI;

namespace AgileEAP.Plugin.FormDesigner.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWorkContext workContext, IRepository<string> repository)
            : base(workContext, repository)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form_Filter([DataSourceRequest] DataSourceRequest request)
        {
            GridFilter filter = request.GetFilter();
            IList<FormDataModel> data = repository.ExecuteDataTable<eForm>("select ID,Name,AppID,Creator,CreateTime from AB_eForm", filter.Parameters, filter.SortCommand ?? "order by CreateTime desc", filter.PageInfo).ToList<FormDataModel>();
            var result = new DataSourceResult()
            {
                Data = data,
                Total = (int)filter.PageInfo.ItemCount
            };

            return Json(result);
        }

        public ActionResult DeleteForm(string formID)
        {
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = string.Format("删除流程定义{0}", formID);
            try
            {
                eForm form = repository.GetDomain<eForm>(formID);
                if (form != null)
                {
                    repository.Delete<eForm>(form);
                    ajaxResult.Result = DoResult.Success;
                    ajaxResult.RetValue = formID;
                    actionMessage += "成功";
                    AddActionLog<eForm>(form, ajaxResult.Result, actionMessage);
                }
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
                AddActionLog<eForm>(actionMessage, ajaxResult.Result);
            }

            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }


        public ActionResult FormDesigner()
        {
            return View();
        }
        public ActionResult ConfigureControl()
        {
            return View();
        }
        public ActionResult ChoiceBox()
        {
            return View();
        }
        public ActionResult ButtonConfigure()
        {
            return View();
        }
        public ActionResult Choosebox()
        {
            return View();
        }
        public ActionResult SystemControl()
        {
            return View();
        }
        public ActionResult TextConfigure()
        {
            return View();
        }
        public ActionResult WizardConfigure()
        {
            return View();
        }
        public ActionResult ChartConfigure()
        {
            return View();
        }
        public ActionResult TreeConfigure()
        {
            return View();
        }
        public ActionResult DataCtrlConfigure()
        {
            return View();
        }
        public JsonResult GetTable()
        {
            try
            {
                string tableName = Request.Form["TableName"];
                var models = CacheManager.Get<IList<TableInfoModel>>("all_table_schema", () =>
                {
                    IList<TableInfoModel> tableInfoModels = new List<TableInfoModel>();
                    var tables = repository.GetTables();
                    if (tables != null)
                    {
                        foreach (var table in tables)
                        {
                            tableInfoModels.Add(new TableInfoModel()
                            {
                                TableName = table,
                                Columns = repository.GetTableColumns(table)
                            });
                        }
                    }
                    return tableInfoModels;
                });

                return Json(models.Where(o => o.TableName == tableName || string.IsNullOrEmpty(tableName)).ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Json(null);
        }

        public JsonResult GetDataSorceTree(string tableName)
        {
            IList<object> trees = new List<object>();
            IList<string> tables = new List<string>();
            try
            {
                if (tableName != null)
                {
                    tables.Add(tableName);
                }
                else
                {
                    tables = repository.GetTables();
                }
                if (tables != null)
                {
                    foreach (var table in tables)
                    {
                        var parentNode = new
                        {
                            attr = new { id = table, type = "root" },
                            data = table,
                            state = "closed",
                            children = new List<object>()
                        };
                        List<string> columns = repository.GetTableColumns(table).ToList();
                        foreach (string column in columns)
                        {
                            var children = new
                            {
                                attr = new { id = table + "." + column, type = "column" },
                                data = column,
                            };
                            parentNode.children.Add(children);
                        }
                        trees.Add(parentNode);
                    }
                }
                return Json(trees);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Json(null);
        }


        public JsonResult InitChart()
        {
            try
            {
                string dataSource = Request.Form["DataSource"];
                string extendData = Request.Form["ExtendData"];
                string tableSource = Request.Form["TableSource"];
                string[] dataItems = dataSource.Split(new Char[] { ',' });
                string cmdText = string.Empty;
                IList<object> categories = new List<object>();
                IList<object> dataList = new List<object>();
                foreach (string dataItem in dataItems)
                {
                    if (dataItem.Contains("."))
                    {
                        string dataTable = dataItem.Substring(0, dataItem.IndexOf("."));
                        string dataColumn = dataItem.Substring(dataItem.IndexOf(".") + 1);
                        cmdText = string.Format("select {0} from {1} where {2} in(select {2} from {3} where CCode='{4}') ", dataColumn, dataTable, extendData, tableSource, workContext.User.OrgID);
                    }
                    else
                    {
                        cmdText = string.Format("select * from {0} where {1} in(select {1} from {2} where CCode='{3}') ", dataItem, extendData, tableSource, workContext.User.OrgID);
                    }
                    DataTable dt = repository.ExecuteDataTable<AgileEAP.Infrastructure.Domain.eForm>(cmdText);
                    foreach (DataColumn column in dt.Columns)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            dataList.Add(row[column]);
                            categories.Add(column.ColumnName);
                        }
                        // series.Add(new
                        //{
                        //    name = column.ColumnName
                        //   // data = dataList
                        //});

                    }
                }

                return Json(new { categories = categories, data = dataList });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Json(null);
        }


        public JsonResult getFormInfo()
        {
            try
            {

                string eFormID = Request.Form["EFormID"];
                eForm eFormInfo = repository.Query<eForm>().FirstOrDefault(e => e.ID == eFormID);
                if (eFormInfo != null)
                {
                    return Json(eFormInfo);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("获取表单信息出错,出错信息为：{0}", ex));
            }
            return Json(null);
        }

        public JsonResult saveForm()
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            string formInfo = Request.Form["Form"];
            string formName = Request.Form["FormName"];
            string eFormID = string.IsNullOrEmpty(Request.Form["EFormID"]) ? IdGenerator.NewGuid().ToSafeString() : Request.Form["EFormID"];
            string formApp = Request.Form["FormApp"];
            string formDescription = Request.Form["FormDescription"];
            try
            {
                eForm eform = new eForm()
                 {
                     ID = eFormID,
                     Description = formDescription,
                     AppID = formApp,
                     Content = formInfo,
                     CreateTime = DateTime.Now,
                     Creator = workContext.User.ID,
                     Name = formName
                 };
                repository.SaveOrUpdate(eform);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = eform;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                ajaxResult.PromptMsg = "操作失败";
                log.Error(ex);
            }
            return Json(ajaxResult);
        }


        public JsonResult GetTreeInfo()
        {
            string dataSource = Request.Form["DataSource"];
            string nodeImg = Request.Form["NodeImg"];
            if (!string.IsNullOrEmpty(dataSource))
            {
                IList<TreeNodeModel> roots = new List<TreeNodeModel>();
                try
                {
                    IList<TreeNode> nodeList = repository.ExecuteDataTable<eForm>(dataSource).ToList<TreeNode>();
                    // IList<TreeNode> rootLists = nodeList.Where(n => nodeList.FirstOrDefault(l => l.ID == n.ParentID) == null).ToList();
                    foreach (var node in nodeList.Where(n => string.IsNullOrEmpty(n.ParentID) || n.ParentID == "-1").ToArray())
                    {
                        TreeNodeModel parentNode = new TreeNodeModel()
                        {
                            ID = node.ID,
                            attr = new { id = node.ID, href = "#", check = !string.IsNullOrEmpty(node.Checked) ? true : false, type = string.IsNullOrEmpty(node.Type) ? "root" : node.Type },
                            data = new { title = node.Name, icon = "/Plugins/eCloud/Content/Themes/Default/Images/resource.png" },
                            state = "open"
                        };
                        roots.Add(parentNode);
                        AddChildNode(parentNode, nodeList);
                    }
                    return Json(roots);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("执行SQL语句{0}失败，无法获取树形控件的数据源", dataSource), ex);
                }
            }
            return Json(null);
        }
        public void AddChildNode(TreeNodeModel parentNode, IList<TreeNode> nodeList)
        {
            foreach (var node in nodeList.Where(n => n.ParentID == parentNode.ID).ToArray())
            {

                TreeNodeModel childrenNode = new TreeNodeModel()
                {
                    ID = node.ID,
                    attr = new { id = node.ID, href = "#", check = !string.IsNullOrEmpty(node.Checked) ? true : false, type = string.IsNullOrEmpty(node.Type) ? "root" : node.Type },
                    data = node.Name

                };
                parentNode.children.Add(childrenNode);
                AddChildNode(childrenNode, nodeList);
            }
        }


        public JsonResult GetTableSource()
        {
            string dataSource = Request.Form["DataSource"];
            string mysqlDataSource = Request.Form["MySqlDataSource"];
            string oracleSource = Request.Form["OracleSource"];
            string msSQLDataSource = Request.Form["MsSQLDataSource"];
            try
            {
                if (string.IsNullOrEmpty(dataSource))
                {
                    DatabaseType databaseType = UnitOfWork.CurrentDatabaseType;
                    if (databaseType == DatabaseType.MySQL)
                    {
                        dataSource = mysqlDataSource;
                    }
                    else if (databaseType == DatabaseType.MsSQL2008)
                    {
                        dataSource = msSQLDataSource;
                    }
                    else if (databaseType == DatabaseType.Oracle)
                    {
                        dataSource = oracleSource;
                    }
                }
                if (!string.IsNullOrEmpty(dataSource))
                {
                    DataTable dt = repository.ExecuteDataTable<eForm>(dataSource);
                    return Json(dt);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("执行SQL语句{0}失败，无法获取Table控件的数据源", dataSource), ex);
            }
            return Json(null);
        }

        public JsonResult ManageSource()
        {
            string dataSource = Request.Form["DataSource"];
            string mysqlDataSource = Request.Form["MySqlDataSource"];
            string oracleSource = Request.Form["OracleSource"];
            string msSQLDataSource = Request.Form["MsSQLDataSource"];
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = "操作";
            try
            {
                if (string.IsNullOrEmpty(dataSource))
                {
                    DatabaseType databaseType = UnitOfWork.CurrentDatabaseType;
                    if (databaseType == DatabaseType.MySQL)
                    {
                        dataSource = mysqlDataSource;
                    }
                    else if (databaseType == DatabaseType.MsSQL2008)
                    {
                        dataSource = msSQLDataSource;
                    }
                    else if (databaseType == DatabaseType.Oracle)
                    {
                        dataSource = oracleSource;
                    }
                }
                DataTable dt = repository.ExecuteDataTable<eForm>(dataSource);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = dt;
                actionMessage += "成功";
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
            }
            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        public JsonResult GetUserInfo()
        {
            try
            {
                string userID = Request.Form["UserID"];
                if (!string.IsNullOrEmpty(userID))
                {
                    Operator userOperator = repository.GetDomain<Operator>(userID);
                    return Json(userOperator);
                }
                else
                {
                    if (workContext.User != null)
                    {
                        return Json(workContext.User);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取当前用户信息失败", ex);
            }
            return Json(null);
        }

        public JsonResult GetOrgPath()
        {
            string orgPath = string.Empty;
            string userID = Request.Form["UserID"];
            if (string.IsNullOrEmpty(orgPath))
            {
                if (string.IsNullOrEmpty(userID))
                {
                    userID = workContext.User.ID;
                }
                Employee employee = repository.FindOne<Employee>(ParameterBuilder.BuildParameters().SafeAdd("OperatorID", userID));
                string orgCode = repository.GetDomain<Organization>(employee.MajorOrgID).Code;

                var func = ExpressionUtil.MakeRecursion<string, string>(f => r =>
                {
                    Organization org = repository.All<Organization>().FirstOrDefault(o => o.ID.Trim() == r || o.Code.Trim() == r);
                    if (org == null)
                        return string.Empty;

                    if (string.IsNullOrEmpty(org.ParentID) || org.ParentID == "-1")
                        return org.Code;

                    return string.Format("{0}/{1}", f(org.ParentID.Trim()), org.Code);
                });

                orgPath = func(orgCode);
            }

            return Json(orgPath);
        }

        public JsonResult GetCurrentDatabaseType()
        {
            string databaseType = UnitOfWork.CurrentDatabaseType.ToSafeString();
            return Json(databaseType);
        }

        public JsonResult ManageStoredProcedure()
        {
            string dataSource = Request.Form["DataSource"];
            string mysqlDataSource = Request.Form["MySqlDataSource"];
            string oracleSource = Request.Form["OracleSource"];
            string msSQLDataSource = Request.Form["MsSQLDataSource"];
            AjaxResult ajaxResult = new AjaxResult() { Result = DoResult.Failed };
            string actionMessage = "操作";
            try
            {
                if (string.IsNullOrEmpty(dataSource))
                {
                    DatabaseType databaseType = UnitOfWork.CurrentDatabaseType;
                    if (databaseType == DatabaseType.MySQL)
                    {
                        dataSource = mysqlDataSource;
                    }
                    else if (databaseType == DatabaseType.MsSQL2008)
                    {
                        dataSource = msSQLDataSource;
                    }
                    else if (databaseType == DatabaseType.Oracle)
                    {
                        dataSource = oracleSource;
                    }
                }
                DataTable dt = repository.ExecuteDataTable<eForm>(dataSource);
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = dt;
                actionMessage += "成功";
            }
            catch (Exception ex)
            {
                actionMessage += "出错";
                log.Error(actionMessage, ex);
            }
            ajaxResult.PromptMsg = actionMessage;
            return Json(ajaxResult);
        }

        public JsonResult SaveEForm()
        {
            string formValueJson = Request.Form["FormValue"];
            string formDataSource = Request.Form["FormDataSource"];
            string entry = Request.Form["Entry"];
            string updateID = Request.Form["UpdateID"];
            string extend = Request.Form["Extend"];
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Failed,
                PromptMsg = "操作失败"
            };
            try
            {
                Dictionary<string, object> formValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(formValueJson);
                if (!string.IsNullOrEmpty(extend))
                {
                    Dictionary<string, object> eFormExtend = JsonConvert.DeserializeObject<Dictionary<string, object>>(extend);
                    formValues.SafeAdd<string, object>(eFormExtend);
                }
                IList<SaveEFormModel> saveEForms = new List<SaveEFormModel>();
                string dataTable = formDataSource;
                SaveEFormModel formSourceTable = new SaveEFormModel();
                if (!string.IsNullOrEmpty(dataTable))
                {
                    formSourceTable.TableName = dataTable;

                }
                foreach (var formValue in formValues)
                {
                    if (formValue.Key.Contains("_"))
                    {
                        dataTable = formValue.Key.Substring(0, formValue.Key.LastIndexOf("_"));
                        string fieldKey = formValue.Key.Substring(formValue.Key.LastIndexOf("_") + 1);
                        string fieldValue = formValue.Value.ToSafeString();
                        bool sameDataTable = false;
                        foreach (SaveEFormModel saveEForm in saveEForms)
                        {
                            if (dataTable == saveEForm.TableName)
                            {
                                saveEForm.ColumnsName.Add(fieldKey);
                                saveEForm.ColumnsValue.Add(fieldValue);
                                sameDataTable = true;
                                break;
                            }
                        }
                        if (!sameDataTable)
                        {
                            IList<string> cloummsName = new List<string>();
                            IList<string> columnsValue = new List<string>();
                            cloummsName.Add(fieldKey);
                            columnsValue.Add(fieldValue);
                            saveEForms.Add(new SaveEFormModel()
                            {
                                TableName = dataTable,
                                ColumnsName = cloummsName,
                                ColumnsValue = columnsValue
                            });
                        }
                    }
                    else if (!string.IsNullOrEmpty(formValue.Key))
                    {
                        formSourceTable.ColumnsName.Add(formValue.Key);
                        formSourceTable.ColumnsValue.Add(formValue.Value.ToSafeString());
                    }
                }
                saveEForms.Add(formSourceTable);
                foreach (SaveEFormModel eForm in saveEForms)
                {
                    IList<string> tableColumns = repository.GetTableColumns(eForm.TableName);
                    if (tableColumns != null && tableColumns.Count() > 0)
                    {
                        if (entry == "add")
                        {
                            string cmdText = string.Join(",", eForm.ColumnsName);
                            string cmdValue = string.Join("','", eForm.ColumnsValue);
                            //if (!string.IsNullOrEmpty(extend))
                            //{
                            //  //  EFormExtendModel eFormExtend = JsonConvert.DeserializeObject<EFormExtendModel>(extend);
                            //    Dictionary<string, object> eFormExtend = JsonConvert.DeserializeObject<Dictionary<string, object>>(extend);
                            //    if (eFormExtend != null)
                            //    {
                            //     cmdText=cmdText+","+string.Join(",", eFormExtend.ColumnsName);
                            //     cmdValue = cmdValue + "','" + string.Join("','", eFormExtend.ColumnsValue);
                            //    }
                            //}
                            if (!cmdText.Contains(",ID"))
                            {
                                string ID = IdGenerator.NewComb().ToSafeString();
                                repository.ExecuteDataTable<eForm>("insert into " + eForm.TableName + "(ID," + cmdText + ") values ('" + ID + "','" + cmdValue + "')");
                            }
                            else
                            {
                                repository.ExecuteDataTable<eForm>("insert into " + eForm.TableName + "(" + cmdText + ") values ('" + cmdValue + "')");
                            }
                        }
                        else
                        {
                            string cmd = string.Empty;
                            for (int i = 0; i < eForm.ColumnsName.Count(); i++)
                            {
                                if (string.IsNullOrEmpty(cmd))
                                {
                                    cmd = string.Format("{0}='{1}'", eForm.ColumnsName[i], eForm.ColumnsValue[i]);
                                }
                                else
                                {
                                    cmd = cmd + "," + string.Format("{0}='{1}'", eForm.ColumnsName[i], eForm.ColumnsValue[i]);
                                }
                            }
                            if (!string.IsNullOrEmpty(updateID))
                                repository.ExecuteDataTable<eForm>("update  " + eForm.TableName + " set " + cmd + " where ID='" + updateID + "'");
                        }
                    }
                }
                ajaxResult.Result = DoResult.Success;
                ajaxResult.RetValue = null;
                ajaxResult.PromptMsg = "操作成功";
            }
            catch (Exception ex)
            {
                log.Error(string.Format("保存eForm表单失败,eForm表单信息为{0}", formValueJson), ex);
            }
            return Json(ajaxResult);
        }
    }
}
