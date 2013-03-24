#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using System.Security.Permissions;

namespace AgileEAP.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("PageSize")]
    [ToolboxData("<{0}:PagedGridView runat=server></{0}:PagedGridView>")]
    public class PagedGridView : System.Web.UI.WebControls.GridView, IPostBackDataHandler, IPostBackEventHandler
    {
        #region 字段
        private static readonly object EventPageChanging = new object();
        private static readonly object EventPageChanged = new object();
        private string inputPageIndex;
        private string inputPageSize = "10";

        #endregion

        #region Pager属性
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool IncludeRowDoubleClick
        {
            get
            {
                object obj = ViewState["IncludeRowDoubleClick"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["IncludeRowDoubleClick"] = value;
            }
        }

        #endregion

        #region Navigation button
        /// <summary>
        /// 首页按钮文本
        /// </summary>
        [Browsable(true), Themeable(false), Description("首页按钮文本"), Category("Pager"), DefaultValue("&lt;&lt;")]
        public string FirstPageText
        {
            get
            {
                object obj = ViewState["FirstPageText"];
                return (obj == null) ? "&lt;&lt;" : (string)obj;
            }
            set { ViewState["FirstPageText"] = value; }
        }

        /// <summary>
        /// 上一页按钮文本
        /// </summary>
        [Browsable(true), Themeable(false), Description("上一页按钮文本"), Category("Pager"), DefaultValue("&lt;")]
        public string PrevPageText
        {
            get
            {
                object obj = ViewState["PrevPageText"];
                return (obj == null) ? "&lt;" : (string)obj;
            }
            set { ViewState["PrevPageText"] = value; }
        }

        /// <summary>
        /// 当前页文本
        /// </summary>
        [Browsable(true), Themeable(false), Description("当前页文本"), Category("Pager"), DefaultValue("1")]
        public string CurrentPageText
        {
            get
            {
                object obj = ViewState["CurrentPageText"];
                return (obj == null) ? "1" : (string)obj;
            }
            set
            {
                ViewState["CurrentPageText"] = value;
            }
        }

        /// <summary>
        /// 下一页文本
        /// </summary>
        [Browsable(true), Themeable(false), Description("下一页文本"), Category("Pager"), DefaultValue("&gt;")]
        public string NextPageText
        {
            get
            {
                object obj = ViewState["NextPageText"];
                return (obj == null) ? "&gt;" : (string)obj;
            }
            set { ViewState["NextPageText"] = value; }
        }

        /// <summary>
        /// 最后一页文本
        /// </summary>
        [Browsable(true), Themeable(false), Description("最后一页文本"), Category("Pager"), DefaultValue("&gt;&gt;")]
        public string LastPageText
        {
            get
            {
                object obj = ViewState["LastPageText"];
                return (obj == null) ? "&gt;&gt;" : (string)obj;
            }
            set { ViewState["LastPageText"] = value; }
        }

        #endregion

        #region Paging

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public override bool AllowPaging
        {
            get
            {
                return base.AllowPaging;
            }
            set
            {
                base.AllowPaging = false;
            }
        }

        /// <summary>
        /// 是否分页
        /// </summary>
        [DefaultValue(true)]
        public bool AllowPager
        {
            get
            {
                return ViewState["ItemCount"] != null;
            }
        }


        /// <summary>
        /// 当前页码
        /// </summary>
        [ReadOnly(true), Browsable(false), Description("当前页码"), Category("Pager"), DefaultValue(1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPageIndex
        {
            get
            {
                object cpage = ViewState["CurrentPageIndex"];
                int pindex = (cpage == null) ? 1 : (int)cpage;
                if (pindex > PageCount && PageCount > 0)
                    return PageCount;
                else if (pindex < 1)
                    return 1;
                return pindex;
            }
            set
            {
                int cpage = value;
                if (cpage < 1)
                    cpage = 1;
                else if (cpage > this.PageCount)
                    cpage = this.PageCount;
                ViewState["CurrentPageIndex"] = cpage;
            }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public new int PageIndex
        {
            get
            {
                return CurrentPageIndex;
            }
            set
            {
                CurrentPageIndex = value;
            }
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        [Browsable(false), Description("记录总数"), Category("Pager"), DefaultValue(0)]
        public long ItemCount
        {
            get
            {
                object obj = ViewState["ItemCount"];
                return (obj == null) ? 0 : (long)obj;
            }
            set { ViewState["ItemCount"] = value; }
        }

        /// <summary>
        /// 页面大小
        /// </summary>
        [Browsable(true), Description("页面大小"), Category("Paging"), DefaultValue(16)]
        public new int PageSize
        {
            get
            {
                object obj = ViewState["PageSize"];
                return (obj == null) ? 20 : (int)obj;
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }

        /// <summary>
        /// 页面数
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int PageCount
        {
            get
            {
                if (ItemCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)ItemCount / (double)PageSize);
            }
        }
        #endregion

        /// <summary>
        /// 是否自动生成列
        /// </summary>
        public override bool AutoGenerateColumns
        {
            get
            {
                return false;
            }
        }



        #region PagedGridView Control Designer
        /// <summary>
        /// PagedGridView设计器类
        /// </summary>
        public class PagedGridViewDesigner : System.Web.UI.Design.ControlDesigner
        {

            private PagedGridView designGridView;
            /// <summary>
            /// 获取编辑内容
            /// </summary>
            /// <param name="region"></param>
            /// <returns></returns>
            public override string GetEditableDesignerRegionContent(System.Web.UI.Design.EditableDesignerRegion region)
            {
                region.Selectable = false;
                return null;
            }

            /// <summary>
            /// 生成设计时html
            /// </summary>
            /// <returns></returns>
            public override string GetDesignTimeHtml()
            {

                designGridView = (PagedGridView)Component;
                designGridView.ItemCount = 250;
                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(sw);
                designGridView.RenderControl(writer);
                return sw.ToString();
            }

            /// <summary>
            /// 生成错误时，html
            /// </summary>
            /// <param name="e"></param>
            /// <returns></returns>
            protected override string GetErrorDesignTimeHtml(Exception e)
            {
                string errorstr = "Error creating PagedGridView：" + e.Message;
                return CreatePlaceHolderDesignTimeHtml(errorstr);
            }
        }
        #endregion

        #region 自定义参数类型
        /// <summary>
        /// 分页参数
        /// </summary>
        public sealed class PageChangingEventArgs : CancelEventArgs
        {
            private readonly int _newpageindex;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="newPageIndex"></param>
            public PageChangingEventArgs(int newPageIndex)
            {
                this._newpageindex = newPageIndex;
            }

            /// <summary>
            /// 新页
            /// </summary>
            public int NewPageIndex
            {
                get
                {
                    return _newpageindex;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public enum Navigation : byte
        {
            /// <summary>
            /// 首页
            /// </summary>
            First,

            /// <summary>
            /// 上一页
            /// </summary>
            Previous,

            /// <summary>
            /// 当前页
            /// </summary>
            Current,

            /// <summary>
            /// 下一页
            /// </summary>
            Next,

            /// <summary>
            /// 最后一页
            /// </summary>
            Last
        }

        #endregion

        #region 自定义方法

        #region  Render Helper

        /// <summary>
        /// 输出空白
        /// </summary>
        /// <param name="writer"></param>
        private void WriteSpacingStyle(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, "5px");
        }

        /// <summary>
        ///  生成页面事件
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        private string GetPageIndexPostBackEvent(int PageIndex)
        {
            if (PageIndex <= 0)
                PageIndex = 1;
            if (PageIndex > PageCount)
                PageIndex = PageCount;

            if (Page != null)
                return Page.ClientScript.GetPostBackClientHyperlink(this, PageIndex.ToString());

            if (HttpContext.Current.Handler is Page)
            {
                return ((Page)HttpContext.Current.Handler).ClientScript.GetPostBackClientHyperlink(this, PageIndex.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 添加提示符
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pageIndex"></param>
        private void AddToolTip(HtmlTextWriter writer, int pageIndex)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format("第{0}页", pageIndex));
        }

        /// <summary>
        /// 输出导航按钮
        /// </summary>
        /// <param name="output">输出对象</param>
        /// <param name="PageText">页面文本</param>
        /// <param name="PageIndex">页码</param>
        private void RenderNavigationButton(HtmlTextWriter output, string PageText, int PageIndex)
        {
            WriteSpacingStyle(output);
            if (PageCount <= 1)
                output.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
            output.AddAttribute(HtmlTextWriterAttribute.Href, GetPageIndexPostBackEvent(PageIndex));
            AddToolTip(output, PageIndex);
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(PageText);
            output.RenderEndTag();
        }

        /// <summary>
        /// 输出导航按钮
        /// </summary>
        /// <param name="output">输出对象</param>
        /// <param name="pageText">页面文本</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="nav">导航操作</param>
        private void RenderNavigationButton(HtmlTextWriter output, string pageText, int pageIndex, Navigation nav)
        {
            WriteSpacingStyle(output);
            if (nav == Navigation.Current)
            {
                string inputClientID = this.UniqueID + "intputPageIndex";
                if (PageCount <= 1)
                    output.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");

                output.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                output.AddAttribute(HtmlTextWriterAttribute.Value, pageIndex.ToString());
                output.AddAttribute(HtmlTextWriterAttribute.Name, inputClientID);
                output.AddAttribute(HtmlTextWriterAttribute.Id, inputClientID);
                output.AddAttribute("onkeypress", string.Format("if (event.keyCode == 13) __doPostBack(\'{0}\',\'\');", this.UniqueID));
                output.RenderBeginTag(HtmlTextWriterTag.Input);
                output.RenderEndTag();

                return;
            }
            if (nav == Navigation.First || nav == Navigation.Previous)
            {
                if (PageCount <= 1 || (CurrentPageIndex <= 1))
                    output.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Href, GetPageIndexPostBackEvent(pageIndex));
            }
            else if (nav == Navigation.Last || nav == Navigation.Next)
            {
                if (PageCount <= 1 || CurrentPageIndex >= PageCount)
                    output.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Href, GetPageIndexPostBackEvent(pageIndex));
            }

            AddToolTip(output, pageIndex);
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(pageText);
            output.RenderEndTag();
        }

        /// <summary>
        /// 创建导航按钮
        /// </summary>
        /// <param name="output"></param>
        /// <param name="nav"></param>
        private void CreateNavigationButton(HtmlTextWriter output, Navigation nav)
        {
            switch (nav)
            {
                case Navigation.First:
                    RenderNavigationButton(output, FirstPageText, 1, Navigation.First); break;
                case Navigation.Previous:
                    RenderNavigationButton(output, PrevPageText, CurrentPageIndex - 1, Navigation.Previous); break;
                case Navigation.Current:
                    CurrentPageText = CurrentPageIndex.ToString();
                    RenderNavigationButton(output, CurrentPageText, CurrentPageIndex, Navigation.Current); break;
                case Navigation.Next:
                    RenderNavigationButton(output, NextPageText, CurrentPageIndex + 1, Navigation.Next); break;
                case Navigation.Last:
                    RenderNavigationButton(output, LastPageText, PageCount, Navigation.Last); break;
            }
        }

        /// <summary>
        /// 输出左自定义文本
        /// </summary>
        /// <param name="output">输出对象</param>
        private void RenderPagerLeft(HtmlTextWriter output)
        {
            WriteSpacingStyle(output);
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write("共" + PageCount + "页 ");
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ID + "_ItemCount");
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write(ItemCount);
            output.RenderEndTag();
            output.Write(" 条记录 ");


        }

        /// <summary>
        /// 输出右边内容
        /// </summary>
        /// <param name="output"></param>
        private void RenderPagerRight(HtmlTextWriter output)
        {
            string inputClientID = this.UniqueID + "inputPageSize";
            WriteSpacingStyle(output);
            WriteSpacingStyle(output);

            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write("每页");
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            output.AddAttribute(HtmlTextWriterAttribute.Value, PageSize.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Name, inputClientID);
            output.AddAttribute(HtmlTextWriterAttribute.Id, inputClientID);
            //output.AddAttribute(HtmlTextWriterAttribute.Onchange, string.Format("javascript:setTimeout('doPostBack(\'{0}\',\'\')', 0)", inputClientID));
            output.AddAttribute("onkeypress", string.Format("if (event.keyCode == 13) __doPostBack(\'{0}\',\'\');", this.UniqueID));
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write("条记录");
            output.RenderEndTag();
        }
        private void RenderPager(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Class, "gridview_footer");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderPagerLeft(output);
            CreateNavigationButton(output, Navigation.First);
            CreateNavigationButton(output, Navigation.Previous);
            CreateNavigationButton(output, Navigation.Current);
            CreateNavigationButton(output, Navigation.Next);
            CreateNavigationButton(output, Navigation.Last);
            RenderPagerRight(output);
            output.RenderEndTag();
        }
        #endregion

        #region 自定义事件方法
        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="e">分页参数</param>
        protected virtual void OnPageChanging(PageChangingEventArgs e)
        {
            PageChangingEventHandler handler = (PageChangingEventHandler)base.Events[EventPageChanging];
            if (handler != null)
            {
                handler(this, e);
                if (!e.Cancel)
                {
                    CurrentPageIndex = e.NewPageIndex;
                    OnPageChanged(new PagingArgs(ItemCount, e.NewPageIndex, PageSize, PageCount));
                }
            }
            else
            {

                CurrentPageIndex = e.NewPageIndex;
                OnPageChanged(new PagingArgs(ItemCount, e.NewPageIndex, PageSize, PageCount));
            }
        }

        /// <summary>
        /// 分页处理后
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageChanged(PagingArgs e)
        {
            PagintEventHandler handler = (PagintEventHandler)base.Events[EventPageChanged];
            if (handler != null)
                handler(this, e);

            // OnPagerDataBind();
        }

        #endregion
        #endregion

        #region 自定义事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PageChangingEventHandler(object sender, PageChangingEventArgs e);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PagintEventHandler(object sender, PagingArgs e);
        /// <summary>
        /// 分页事件
        /// </summary>
        public event PageChangingEventHandler PageChanging
        {
            add
            {
                base.Events.AddHandler(EventPageChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPageChanging, value);
            }
        }

        /// <summary>
        ///  分页后事件
        /// </summary>
        public event PagintEventHandler PageChanged
        {
            add
            {
                base.Events.AddHandler(EventPageChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPageChanged, value);
            }
        }

        #endregion

        #region Interface Implementation

        #region IPostBackDataHandler

        /// <summary>
        /// 加载回发数据
        /// </summary>
        /// <param name="pkey"></param>
        /// <param name="pcol"></param>
        /// <returns></returns>
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            string str = pcol[this.UniqueID + "intputPageIndex"];
            //  Page.Response.Write(str+":LoadPostData<br>");           
            if (str != null && str.Trim().Length > 0)
            {
                try
                {
                    int pindex = int.Parse(str);
                    if (pindex > 0 && pindex <= PageCount)
                    {
                        inputPageIndex = str;
                        Page.RegisterRequiresRaiseEvent(this);
                    }
                }
                catch
                { }
            }
            return false;
        }

        /// <summary>
        /// 回发处理
        /// </summary>
        public virtual void RaisePostDataChangedEvent() { }

        #endregion

        #region IPostBackEventHandler
        /// <summary>
        /// 回发处理事件
        /// </summary>
        /// <param name="postArgs"></param>
        public new void RaisePostBackEvent(string postArgs)
        {
            base.RaisePostBackEvent(postArgs);

            int pageIndex = 1;
            if (!int.TryParse(postArgs, out pageIndex))
            {
                inputPageIndex = Page.Request.Form[this.UniqueID + "intputPageIndex"];
                if (!int.TryParse(inputPageIndex, out pageIndex))
                    pageIndex = CurrentPageIndex;
            }

            inputPageSize = Page.Request.Form[this.UniqueID + "inputPageSize"];
            int pageSize = 10;
            int.TryParse(inputPageSize, out pageSize);

            if (pageIndex < 1) pageIndex = 1;

            int pageCount = (int)(Math.Ceiling(ItemCount / (double)pageSize));
            if (pageIndex > pageCount) pageIndex = pageCount;

            if (pageIndex != CurrentPageIndex || PageSize != pageSize)
            {
                CurrentPageIndex = pageIndex;
                PageSize = pageSize;
            }

            PageChangingEventArgs pagingArgs = new PageChangingEventArgs(pageIndex);
            OnPageChanging(pagingArgs);
        }

        #endregion

        #endregion

        /// <summary>
        /// 绑定处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {

            base.OnRowDataBound(e);

            int rowIndex = e.Row.RowIndex;

            if (rowIndex > -1)
            {
                GridViewRow row = e.Row;
                string id = string.Empty;
                try
                { id = DataKeys[rowIndex]["ID"].ToString(); }
                catch
                { }

                row.Attributes.Add("style", "cursor: hand");
                row.Attributes.Add("onmouseover", "rowOver(this);");
                row.Attributes.Add("onclick", string.Format("rowClick(this,'{0}')", id));
                if (IncludeRowDoubleClick)
                {
                    row.Attributes.Add("ondblclick", string.Format("rowDbClick(this,'{0}')", id));
                }
                row.Attributes.Add("onmouseout", "rowOut(this);");
            }
        }

        /// <summary>
        ///  预呈现处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Rows.Count > 0)
            {
                UseAccessibleHeader = true;
                HeaderRow.TableSection = TableRowSection.TableHeader;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitWebResource();
        }

        private void InitWebResource()
        {
            if (Page == null) return;

            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "GridViewJs"))
            {

                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.gridview.js");
                clientManager.RegisterClientScriptInclude(type, "GridViewJs", url);

            }
        }

        /// <summary>
        /// 输出空表
        /// </summary>
        /// <param name="output"></param>
        private void RenderEmptyGridView(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Class, "gridview");
            output.AddAttribute(HtmlTextWriterAttribute.Rules, "all");
            output.RenderBeginTag(HtmlTextWriterTag.Table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (DataControlField col in Columns)
            {
                if (col.Visible)
                {
                    output.RenderBeginTag(HtmlTextWriterTag.Th);
                    output.Write(col.HeaderText);
                    output.RenderEndTag();
                }
            }
            output.RenderEndTag();
            output.RenderEndTag();
        }

        /// <summary>
        /// 输出表格内容
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Class, "page_container_gridview");
            output.AddAttribute(HtmlTextWriterAttribute.Id, ID + "_container");
            output.RenderBeginTag(HtmlTextWriterTag.Div);

            if (((ItemCount <= 0 && AllowPager) || Rows.Count == 0) && DesignMode == false)
            {
                RenderEmptyGridView(output);
                output.RenderEndTag();
                return;
            }

            base.Render(output);

            output.RenderEndTag();

            if (AllowPager)
            {
                RenderPager(output);
            }
        }

        /// <summary>
        /// ajax输出
        /// </summary>
        /// <returns></returns>
        public string AjaxRender()
        {
            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            using (Html32TextWriter hw = new Html32TextWriter(sw))
            {
                RenderBeginTag(hw);
                HeaderRow.RenderControl(hw);
                foreach (GridViewRow row in Rows)
                {
                    row.RenderControl(hw);
                }
                FooterRow.RenderControl(hw);
                RenderEndTag(hw);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class PagingArgs : EventArgs
    {
        /// <summary>
        /// 数据记录总数
        /// </summary>
        public long ItemCount
        { get; set; }

        /// <summary>
        /// 当前页面
        /// </summary>
        public int PageIndex
        { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        { get; set; }

        /// <summary>
        /// 总共页数
        /// </summary>
        public int PageCount
        { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemCount">记录总数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageCount">页数</param>
        public PagingArgs(long itemCount, int pageIndex, int pageSize, int pageCount)
        {
            ItemCount = itemCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
            PageCount = pageCount;
        }
    }


}
