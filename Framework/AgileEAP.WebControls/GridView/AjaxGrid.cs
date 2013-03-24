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
    [ToolboxData("<{0}:AjaxGrid runat=server></{0}:AjaxGrid>")]
    public class AjaxGrid : System.Web.UI.WebControls.GridView
    {
        #region Properties

        private static readonly object eventPaging = new object();
        private static readonly object eventPaged = new object();

        private bool enableToolbox = false;
        /// <summary>
        /// 
        /// </summary>
        public bool EnableToolbox
        {
            get
            {
                return enableToolbox;
            }
            set
            {
                enableToolbox = value;
            }
        }

        private int pageIndex = 1;
        /// <summary>
        /// 
        /// </summary>
        public new int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        private int pageSize = 20;
        /// <summary>
        /// 
        /// </summary>
        public new int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long ItemCount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public new int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)ItemCount / (double)PageSize);
            }
        }

        private bool enableRowDbClick = true;
        /// <summary>
        /// 
        /// </summary>
        public bool EnableRowDbClick
        {
            get
            {
                return enableRowDbClick;
            }
            set
            {
                enableRowDbClick = value;
            }
        }
        private GridBarStyle gridBarStyle = GridBarStyle.Bottom;
        /// <summary>
        /// 
        /// </summary>
        public GridBarStyle GridBarStyle
        {
            get
            {
                return gridBarStyle;
            }
            set
            {
                gridBarStyle = value;
            }
        }

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
        #region Icons

        private string iconFirst = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconFirst
        {
            get
            {
                return iconFirst;
            }
            set
            {
                iconFirst = value;
            }
        }
        private string iconCsv = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconCsv
        {
            get
            {
                return iconCsv;
            }
            set
            {
                iconCsv = value;
            }
        }
        private string iconTxt = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconTxt
        {
            get
            {
                return iconTxt;
            }
            set
            {
                iconTxt = value;
            }
        }

        private string iconGo = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconGo
        {
            get
            {
                return iconGo;
            }
            set
            {
                iconGo = value;
            }
        }

        private string iconLast = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconLast
        {
            get
            {
                return iconLast;
            }
            set
            {
                iconLast = value;
            }
        }

        private string iconNext = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconNext
        {
            get
            {
                return iconNext;
            }
            set
            {
                iconNext = value;
            }
        }

        private string iconPrevious = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconPrevious
        {
            get
            {
                return iconPrevious;
            }
            set
            {
                iconPrevious = value;
            }
        }

        private string iconRefresh = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconRefresh
        {
            get
            {
                return iconRefresh;
            }
            set
            {
                iconRefresh = value;
            }
        }

        private string iconExcel = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconExcel
        {
            get
            {
                return iconExcel;
            }
            set
            {
                iconExcel = value;
            }
        }

        private string iconAdd = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconAdd
        {
            get
            {
                return iconAdd;
            }
            set
            {
                iconAdd = value;
            }
        }

        private string iconEdit = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconEdit
        {
            get
            {
                return iconEdit;
            }
            set
            {
                iconEdit = value;
            }
        }

        private string iconLoading = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconLoading
        {
            get
            {
                return iconLoading;
            }
            set
            {
                iconLoading = value;
            }
        }


        /// <summary>
        /// 增删条件按钮
        /// </summary>
        private string iconCondition = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconCondition
        {
            get
            {
                return iconCondition;
            }
            set
            {
                iconCondition = value;
            }
        }

        /// <summary>
        /// 增删显示列按钮
        /// </summary>
        private string iconDisplay = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IconDisplay
        {
            get
            {
                return iconDisplay;
            }
            set
            {
                iconDisplay = value;
            }
        }

        #endregion

        #endregion

        #region Render

        /// <summary>
        /// 
        /// </summary>
        private void InitResource()
        {
            if (Page == null)
            {
                if (HttpContext.Current != null && HttpContext.Current.Handler is Page)
                {
                    Page = HttpContext.Current.Handler as Page;
                }
            }
            Type type = this.GetType();
            ClientScriptManager clientManager = Page.ClientScript;
            if (!clientManager.IsClientScriptIncludeRegistered(type, "AjaxGridJs"))
            {
                string url = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.ajaxgrid.js");
                clientManager.RegisterClientScriptInclude(type, "AjaxGridJs", url);
            }

            if (string.IsNullOrEmpty(iconFirst))
            {
                iconFirst = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.first.gif");
                iconGo = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.go.png");
                iconLast = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.last.gif");
                iconNext = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.next.gif");
                iconPrevious = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.previous.gif");
                iconExcel = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.xls.png");
                iconAdd = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.add.gif");
                iconEdit = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.edit.gif");
                iconLoading = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.onLoad.gif");
                iconCondition = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.condition.gif");
                iconDisplay = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.display.gif");
                iconCsv = clientManager.GetWebResourceUrl(type, "AgileEAP.WebControls.GridView.Resources.csv.gif");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        private void RenderToolbar(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "grid_toolbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, iconFirst);
            writer.AddAttribute("style", "cursor:pointer;");
            writer.AddAttribute("title", "首页");
            writer.AddAttribute("onclick", string.Format("ajaxSearchGrid('{0}','{1}')", ID, GridAction.First));
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute("style", "cursor:pointer;");
            writer.AddAttribute("title", "上一页");
            writer.AddAttribute("onclick", string.Format("ajaxSearchGrid('{0}','{1}')", ID, GridAction.Previous));
            writer.AddAttribute(HtmlTextWriterAttribute.Src, iconPrevious);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "txtpageindex");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PageIndex.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute("onclick", string.Format("ajaxSearchGrid('{0}','{1}')", ID, GridAction.Next));
            writer.AddAttribute("style", "cursor:pointer;");
            writer.AddAttribute("title", "下一页");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, iconNext);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute("onclick", string.Format("ajaxSearchGrid('{0}','{1}')", ID, GridAction.Last));
            writer.AddAttribute("style", "cursor:pointer;");
            writer.AddAttribute("title", "末页");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, iconLast);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute("style", "margin:0px 3px 1px 2px;width:200px");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.Write(string.Format("共<span id='pageCount'>{0}</span>页  <span id='recordCount'>{1}</span>条记录  ", PageCount, ItemCount));
            writer.Write("每页");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "txtpagesize");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, PageSize.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.Write("条");
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute("onclick", string.Format("ajaxSearchGrid('{0}','{1}')", ID, GridAction.Go));
            writer.AddAttribute("title", "跳转");
            writer.AddAttribute("style", "cursor:pointer;");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, iconGo);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            if (EnableToolbox)
            {
                #region Toolbox
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute("onclick", "setFilterFields('" + ID + "')");
                writer.AddAttribute("title", "增删查询条件");
                writer.AddAttribute("style", "cursor:pointer;");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, iconCondition);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();


                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute("onclick", "setDisplayFields('" + ID + "')");
                writer.AddAttribute("title", "增删显示列");
                writer.AddAttribute("style", "cursor:pointer;");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, iconDisplay);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute("onclick", string.Format("exportExcel('{0}','{1}')", ID, GridAction.ExportExcel));
                writer.AddAttribute("title", "导出到Excel");
                writer.AddAttribute("style", "cursor:pointer;");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, iconExcel);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute("onclick", string.Format("exportCsv('{0}','{1}')", ID, GridAction.ExportCsv));
                writer.AddAttribute("title", "导出到Csv");
                writer.AddAttribute("style", "cursor:pointer;");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, iconCsv);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();

                #endregion
            }

            writer.RenderEndTag();//end ul
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
        /// 是否Ajax查询
        /// </summary>
        public bool IsAjaxSort
        {
            get
            {
                string ajaxAction = HttpContext.Current.Request.Form["AjaxAction"];
                return !string.IsNullOrEmpty(ajaxAction) && ajaxAction.EndsWith("AjaxSearch");
            }
        }


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

                row.Attributes.Add("style", "cursor: hand");
                row.Attributes.Add("onmouseover", "rowOver(this);");
                row.Attributes.Add("onclick", string.Format("rowClick(this,'{0}')", ID));
                if (EnableRowDbClick)
                {
                    row.Attributes.Add("ondblclick", string.Format("rowDbClick(this,'{0}')", ID));
                }
                row.Attributes.Add("onmouseout", "rowOut(this);");
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int i = 0;
                foreach (DataControlField dataControlField in Columns)
                {
                    if (i++ > 0 && !string.IsNullOrEmpty(dataControlField.SortExpression))
                    {
                        TableCell tc = e.Row.Cells[Columns.IndexOf(dataControlField)];
                        tc.Attributes.Add("style", "cursor: pointer");
                        tc.Attributes.Add("field", dataControlField.SortExpression);
                        tc.Attributes.Add("onclick", string.Format("onGridSort('{0}','{1}')", ID, dataControlField.SortExpression));

                        if (i == 2 && !IsAjaxSort)
                        {
                            tc.Text = string.Format("<span>▲</span>{0}", dataControlField.HeaderText);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitResource();
        }
        /// <summary>
        /// 输出空表
        /// </summary>
        /// <param name="writer"></param>
        private void RenderEmptyGridView(HtmlTextWriter writer)
        {
            //工具栏输出到头部
            if (gridBarStyle == WebControls.GridBarStyle.Top)
            {
                RenderToolbar(writer);
            }

            //输出内容
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "gridview");
            writer.AddAttribute(HtmlTextWriterAttribute.Rules, "all");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (DataControlField col in Columns)
            {
                if (col.Visible)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    writer.Write(col.HeaderText);
                    writer.RenderEndTag();
                }
            }
            writer.RenderEndTag();
            writer.RenderEndTag();//end table
            writer.RenderEndTag();//end div

            //工具栏输出到底部
            if (gridBarStyle == WebControls.GridBarStyle.Bottom)
            {
                RenderToolbar(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            string css = Page.ClientScript.GetWebResourceUrl(this.GetType(), "AgileEAP.WebControls.GridView.Resources.ajaxgrid.css");
            writer.Write(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", css));

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID + "_container");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajax_grid_view");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if ((ItemCount <= 0 || Rows.Count == 0) && DesignMode == false)
            {
                RenderEmptyGridView(writer);
                writer.RenderEndTag();
                return;
            }

            //工具栏输出到头部
            if (gridBarStyle == WebControls.GridBarStyle.Top)
            {
                RenderToolbar(writer);
            }

            //输出内容
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);
            base.Render(writer);

            //工具栏输出到底部
            if (gridBarStyle == WebControls.GridBarStyle.Bottom)
            {
                RenderToolbar(writer);
            }

            writer.RenderEndTag();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string AjaxRender()
        {
            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            using (Html32TextWriter writer = new Html32TextWriter(sw))
            {
                //输出内容
                RenderBeginTag(writer);

                if (Rows == null)
                {
                    RenderEmptyGridView(writer);
                    writer.RenderEndTag();
                    return sb.ToString();
                }

                if (HeaderRow != null)
                {
                    HeaderRow.RenderControl(writer);
                }

                if (Rows != null)
                {
                    foreach (GridViewRow row in Rows)
                    {
                        row.RenderControl(writer);
                    }
                }

                if (FooterRow != null)
                {
                    FooterRow.RenderControl(writer);
                }

                RenderEndTag(writer);
            }

            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GridAction
    {
        /// <summary>
        /// 
        /// </summary>
        First = 0,

        /// <summary>
        /// 
        /// </summary>
        Next = 1,

        /// <summary>
        /// 
        /// </summary>
        Current = 2,

        /// <summary>
        /// 
        /// </summary>
        Previous = 3,

        /// <summary>
        /// 
        /// </summary>
        Last = 4,

        /// <summary>
        /// 
        /// </summary>
        Go = 5,

        /// <summary>
        /// 
        /// </summary>
        EditModel = 6,

        /// <summary>
        /// 
        /// </summary>
        ExportExcel = 7,

        /// <summary>
        /// 
        /// </summary>
        Query = 8,

        /// <summary>
        /// 
        /// </summary>
        Refresh = 9,

        /// <summary>
        /// 
        /// </summary>
        ExportCsv = 10,

        /// <summary>
        /// 
        /// </summary>
        ExportTxt = 11
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GridBarStyle
    {
        /// <summary>
        /// 顶部
        /// </summary>
        Top = 1,

        /// <summary>
        /// 底部
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// 不显示
        /// </summary>
        None = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public class PageArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int Count
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NewPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        public PageArgs(int count, int pageIndex, int pageSize, int pageCount)
        {
            Count = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            PageCount = pageCount;
        }
    }
}
