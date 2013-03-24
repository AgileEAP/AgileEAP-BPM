using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using AgileEAP.Core;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Data;

using AgileEAP.Core.Service;
using AgileEAP.Core.Security;
using AgileEAP.Core.Extensions;
using AgileEAP.Core.Web;
using AgileEAP.Core.Caching;
using AgileEAP.Core.FastInvoker;

using AgileEAP.WebControls;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;



namespace AgileEAP.Plugin.Authorize
{
    public partial class OrgUserList : BasePage
    {
        AuthorizeService operatorService = new AuthorizeService();
        #region ---界面处理方法---

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvList.PageChanged += new PagedGridView.PagintEventHandler(gvList_PageChanged);
        }

        void gvList_PageChanged(object sender, PagingArgs e)
        {
            ShowList(gvList, new PageInfo(e.PageIndex, e.PageSize, e.ItemCount));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
            }
        }

        #endregion

        #region ---操作处理方法---
        /// <summary>
        /// 转向明细页面
        /// </summary>
        /// <param name="param"></param>
        protected void Redirect(string param)
        {
            var currentIdParam = PageContext.Action == ActionType.Add ? string.Empty : string.Format("&CurrentId={0}", CurrentId);
            Response.Redirect(string.Format("EmployeeDetail.aspx?LastUrl={0}&Runat=1&ActionFlag={1}{2}{3}", Request.Url.PathAndQuery, PageContext.Action, currentIdParam, string.IsNullOrEmpty(param) ? param : "&" + param));
        }

        /// <summary>
        /// 新增
        /// </summary>
        public void Add()
        {
            PageContext.Action = ActionType.Add;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }

        /// <summary>
        /// 查看
        /// </summary>
        public void View()
        {
            PageContext.Action = ActionType.View;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }

        /// <summary>
        /// 修改
        /// </summary>
        public void Update()
        {
            PageContext.Action = ActionType.Update;
            PageContext.PageIndex = gvList.PageIndex;
            SavePageContext(PageContext);

            Redirect(string.Empty);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public void Search()
        {
            ShowList(gvList, new PageInfo(1, gvList.PageSize, gvList.ItemCount));
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            ShowList(gvList, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount));
        }

        /// <summary>
        /// 显示列表信息
        /// </summary>
        /// <param name="gvList">GridView对象</param>
        /// <param name="pageInfo">分页信息</param>
        public void ShowList(PagedGridView gvList, PageInfo pageInfo)
        {
            gvList.AllowPaging = true;
            string orgId = Request.QueryString["orgid"];
            if (!string.IsNullOrWhiteSpace(orgId))
            {
                string filter = GetFilterString();
                IDictionary<string, object> para = GetFilterParameters();

                Organization organization =repository.GetDomain<Organization>(orgId);
                if (organization != null)
                {
                    para.SafeAdd("OrgID", new Condition(string.Format(" OwnerOrg like '{0}%' AND UserType!=0 ", organization.OwnerOrg)));
                }
                DataTable dt = operatorService.GetOperatorByOrg(para, pageInfo, filter);
                gvList.ItemCount = pageInfo.ItemCount;
                gvList.DataSource = dt;
                gvList.DataBind();
            }
        }



        public string IsExist(string argument)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                string dataID = Request.Form["DataID"].Trim();
                IDictionary<string, object> para = new Dictionary<string, object>();
                para.SafeAdd("ID", dataID);
                Operator operatorInfo =repository.FindOne<Operator>(para);
                if (operatorInfo != null)
                {
                    ajaxResult.PromptMsg = "该用户已经存在！";
                }

                ajaxResult.RetValue = string.Empty;
                ajaxResult.Result = DoResult.Success;


            }
            catch (Exception ex)
            {
                ajaxResult.Result = DoResult.Failed;
                ajaxResult.PromptMsg = "获取信息出错，请联系管理员！";
                log.Error(ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }

        public string Delete(string operatorID)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(operatorID))
                {
                    IRepository<string> repository = new Repository<string>();

                    UnitOfWork.ExecuteWithTrans<Operator>(() =>
                        {
                            repository.Delete<Operator>(operatorID);

                            IDictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.SafeAdd("OperatorID", operatorID);
                            Employee employee = repository.FindOne<Employee>(parameters);
                            repository.Delete<Employee>(parameters);

                            parameters.Clear();
                            parameters.SafeAdd("EmployeeID", employee.ID);
                            repository.Delete<EmployeeOrg>(parameters);

                            parameters.Clear();
                            parameters.SafeAdd("ObjectID", operatorID);
                            repository.Delete<ObjectRole>(parameters);
                        });


                    doResult = DoResult.Success;

                    //获取提示信息
                    actionMessage = RemarkAttribute.GetEnumRemark(doResult);

                    ajaxResult.RetValue = CurrentId;
                    ajaxResult.PromptMsg = actionMessage;
                }

                ajaxResult.Result = doResult;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }

        public string Activation(string operatorID)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(operatorID))
                {
                    if (operatorID.Equals(User.LoginName.GetOperID()))
                    {
                        actionMessage = "不可以自我启用";
                    }
                    else
                    {
                        Operator operatorModel =repository.GetDomain<Operator>(operatorID);
                        if (operatorModel != null)
                        {
                            if (operatorModel.Status == (short)UserStatus.Freezon)
                            {
                                operatorModel.Status = (short)UserStatus.Normal;
                                repository.SaveOrUpdate(operatorModel);
                                doResult = DoResult.Success;
                                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                            }
                            else
                            {
                                actionMessage = "该操作员已经处于激活状态";
                            }
                        }
                        else
                        {
                            actionMessage = "没找到该操作员";
                        }
                    }
                    ajaxResult.RetValue = CurrentId;
                    ajaxResult.PromptMsg = actionMessage;
                }

                ajaxResult.Result = doResult;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }

        public string Frezze(string operatorID)
        {
            AjaxResult ajaxResult = new AjaxResult();

            string errorMsg = string.Empty;
            DoResult doResult = DoResult.Failed;
            string actionMessage = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(operatorID))
                {
                    if (operatorID.Equals(User.LoginName.GetOperID()))
                    {
                        actionMessage = "不可以自我冻结";
                    }
                    else
                    {
                        Operator operatorModel = repository.GetDomain<Operator>(operatorID);
                        if (operatorModel != null)
                        {
                            if (operatorModel.Status == (short)UserStatus.Normal)
                            {
                                operatorModel.Status = (short)UserStatus.Freezon;
                                repository.SaveOrUpdate(operatorModel);
                                doResult = DoResult.Success;
                                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                            }
                            else
                            {
                                actionMessage = "该操作员已经处于冻结状态";
                            }
                        }
                        else
                        {
                            actionMessage = "没找到该操作员";
                        }
                    }
                    ajaxResult.RetValue = CurrentId;
                    ajaxResult.PromptMsg = actionMessage;
                }

                ajaxResult.Result = doResult;
            }
            catch (Exception ex)
            {
                actionMessage = RemarkAttribute.GetEnumRemark(doResult);
                log.Error(actionMessage, ex);
            }

            return JsonConvert.SerializeObject(ajaxResult);
        }
        //protected void btnSearch_OnClick(object sender, EventArgs e)
        //{
        //    string loginName = txtLoginName.Text;
        //    string userName = txtUserName.Text;

        //    IDictionary<string, object> para = new Dictionary<string, object>();
        //    if (!string.IsNullOrWhiteSpace(loginName))
        //        para.SafeAdd("LoginName", loginName);
        //    if (!string.IsNullOrWhiteSpace(userName))
        //        para.SafeAdd("UserName", userName);

        //    DataTable dt = operatorService.GetOperatorByOrg(para, new PageInfo(gvList.PageIndex, gvList.PageSize, gvList.ItemCount), GetFilterString());
        //    gvList.DataSource = dt;
        //    gvList.DataBind();


        //}


        #endregion

        #region 辅助函数
        public string GetUserNameByUserId(string userId)
        {
            Operator operatorInfo =repository.GetDomain<Operator>(userId);
            if (operatorInfo != null)
            {
                return operatorInfo.UserName;
            }
            return "";
        }
        public string GetStateByStatus(string sStatus)
        {
            string state = "";
            try
            {
                state = int.Parse(sStatus) == 1 ? "正常" : "冻结";
            }
            catch
            {
                return state;
            }
            return state;
        }
        #endregion 辅助函数

    }
}