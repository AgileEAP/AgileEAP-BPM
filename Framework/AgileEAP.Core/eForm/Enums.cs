

namespace AgileEAP.Core
{
    /// <summary>
    /// 数据类型
    /// </summary>
    [Remark("数据类型")]
    public enum DataType
    {
        /// <summary>
        /// 整数
        /// </summary>
        [Remark("整数")]
        Integer = 1,

        /// <summary>
        /// 浮点数
        /// </summary>
        [Remark("浮点数")]
        Float = 2,

        /// <summary>
        /// 日期
        /// </summary>
        [Remark("日期")]
        DateTime = 3,

        /// <summary>
        /// 字符串
        /// </summary>
        [Remark("字符串")]
        String = 4,

        /// <summary>
        /// 布尔型
        /// </summary>
        [Remark("布尔型")]
        Boolean = 5,
        [Remark("曲线图")]
        line = 6,
        [Remark("柱状图")]
        column = 7,
        [Remark("饼图")]
        pie = 8,
        [Remark("区域图")]
        area = 9
    }

    /// <summary>
    /// 字段控件类型
    /// </summary>
    [Remark("字段控件类型")]
    public enum ControlType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Remark("文本")]
        Text = 0,

        [Remark("单项输入框")]
        TextBox = 1,

        /// <summary>
        /// 日期控件
        /// </summary>
        [Remark("日期控件")]
        DatePicker = 2,

        ///// <summary>
        ///// 日期范围控件
        ///// </summary>
        //[Remark("日期范围控件")]
        //DateRangePicker = 3,

        /// <summary>
        /// 月份控件
        /// </summary>
        [Remark("月份控件")]
        MonthPicker = 4,

        ///// <summary>
        ///// 月份范围控件
        ///// </summary>
        //[Remark("月份范围控件")]
        //MonthRangePicker = 5,

        /// <summary>
        /// 年份控件
        /// </summary>
        [Remark("年份控件")]
        YearPicker = 6,

        /// <summary>
        /// 单选项列表
        /// </summary>
        [Remark("单选项列表")]
        SingleCombox = 7,

        /// <summary>
        /// 多选项列表
        /// </summary>
        [Remark("多选项列表")]
        MultiCombox = 8,

        /// <summary>
        /// 复选框
        /// </summary>
        [Remark("复选框")]
        CheckBox = 9,

        ///// <summary>
        ///// 单选按钮
        ///// </summary>
        //[Remark("单选按钮")]
        //RadioBox = 10,
        [Remark("下拉列表")]
        DropDown = 10,
        /// <summary>
        /// 选择框
        /// </summary>
        [Remark("选择列表框")]
        ChooseBox = 11,

        /// <summary>
        /// 选择树
        /// </summary>
        [Remark("选择树")]
        ChooseTree = 12,

        [Remark("按钮")]

        Button = 13,

        [Remark("多行输入框")]

        TextArea = 14,
        [Remark("单选按钮")]
        Radio = 15,
        [Remark("邮箱输入框")]
        Email = 16,
        [Remark("选择框")]
        Combox = 17,
        [Remark("上传按钮")]
        Upload = 18,

        [Remark("隐藏控件")]
        HiddenInput = 19,

        [Remark("系统变量")]
        SysVariable = 20,
        [Remark("容器控件")]
        Div = 21,

        [Remark("Grid控件")]
        Grid = 22,

        [Remark("向导控件")]
        Wizard = 23,
        [Remark("表格控件")]
        DataTable = 24,
        [Remark("图片控件")]
        Image = 25,
        [Remark("图表控件")]
        Chart = 26,
        [Remark("树形控件")]
        Tree = 27,
        [Remark("Tabs控件")]
        Tabs=28
        ///// <summary>
        ///// 文本范围
        ///// </summary>
        //[Remark("文本范围")]
        //TextRangebox = 13
    }


    [Remark("系统控件类型")]
    public enum SystemControlType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Remark("组织单位名")]
        OrgID = 0,

        [Remark("用户名")]
        UserID = 1,

        /// <summary>
        /// 日期控件
        /// </summary>
        [Remark("当前日期")]
        CurrentDate = 2,
    }


    /// <summary>
    /// 读写方式
    /// </summary>
    public enum AccessPattern
    {
        /// <summary>
        /// 只读
        /// </summary>
        [Remark("只读")]
        ReadOnly = 1,

        /// <summary>
        /// 读写
        /// </summary>
        [Remark("读写")]
        Write = 2
    }

}
