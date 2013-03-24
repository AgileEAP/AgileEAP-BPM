using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using AgileEAP.Core;

namespace AgileEAP.Workflow.Enums
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// 开始活动
        /// </summary>
        [Remark("开始活动")]
        StartActivity = 1,

        /// <summary>
        /// 人工活动
        /// </summary>
        [Remark("人工活动")]
        ManualActivity = 2,

        /// <summary>
        /// 路由活动
        /// </summary>
        [Remark("路由活动")]
        RouterActivity = 3,

        /// <summary>
        /// 子流程活动
        /// </summary>
        [Remark("子流程活动")]
        SubflowActivity = 4,
        /// <summary>
        /// 自动活动
        /// </summary>
        [Remark("自动活动")]
        AutoActivity = 5,

        /// <summary>
        /// 结束活动
        /// </summary>
        [Remark("结束活动")]
        EndActivity = 6,

        /// <summary>
        /// 处理活动
        /// </summary>
        [Remark("处理活动")]
        ProcessActivity = 7
    }

    /// <summary>
    /// 参与类型
    /// </summary>
    public enum ParticipantType
    {
        /// <summary>
        ///// 参与者
        /// </summary>
        [Remark("参与者")]
        Participantor = 1,

        /// <summary>
        /// 流程启动者
        /// </summary>
        [Remark("流程启动者")]
        ProcessStarter = 2,

        /// <summary>
        /// 特殊活动
        /// </summary>
        [Remark("特殊活动")]
        SpecialActivity = 3,

        /// <summary>
        /// 相关数据
        /// </summary>
        [Remark("相关数据")]
        RelevantData = 4,

        /// <summary>
        /// 自定义规则
        /// </summary>
        [Remark("自定义规则")]
        CustomRegular = 5,

        /// <summary>
        /// 流程执行者
        /// </summary>
        [Remark("流程执行者")]
        ProcessExecutor = 6,

        /// <summary>
        /// 相关规则
        /// </summary>
        [Remark("相关规则")]
        RelateRegular = 7
    }

    /// <summary>
    /// 事物处理方式
    /// </summary>
    [Remark("事物处理方式")]
    public enum TransactionType
    {
        /// <summary>
        /// 联合
        /// </summary>
        [Remark("联合")]
        Join = 1,

        /// <summary>
        /// 挂起
        /// </summary>
        [Remark("挂起")]
        Suspend = 2
    }

    /// <summary>
    /// 响应方式
    /// </summary>
    [Remark("响应方式")]
    public enum ActionPattern
    {
        /// <summary>
        /// 方法
        /// </summary>
        [Remark("方法")]
        Method = 1,

        [Remark("Web服务")]
        WebService = 2,

        /// <summary>
        /// 业务操作
        /// </summary>
        [Remark("业务操作")]
        BusinessOperation = 3
    }

    /// <summary>
    /// 参与者类型
    /// </summary>
    public enum ParticipantorType
    {
        /// <summary>
        /// 人员
        /// </summary>
        [Remark("人员")]
        Person = 1,

        /// <summary>
        /// 角色
        /// </summary>
        [Remark("角色")]
        Role = 2,

        /// <summary>
        /// 组织
        /// </summary>
        [Remark("组织")]
        Org = 3
    }

    /// <summary>
    /// 活动聚合模式
    /// </summary>
    [Remark("聚合模式")]
    public enum JoinType
    {
        /// <summary>
        /// 单一聚合
        /// </summary>
        [Remark("单一聚合")]
        XOR = 1,

        /// <summary>
        /// 多路聚合
        /// </summary>
        [Remark("多路聚合")]
        OR = 2,

        /// <summary>
        /// 全部聚合
        /// </summary>
        [Remark("全部聚合")]
        AND = 3
    }

    /// <summary>
    /// 抑制联合事物方式
    /// </summary>
    [Remark("抑制联合事物方式")]
    public enum SuppressJoinFailure
    {
        /// <summary>
        /// 抑制
        /// </summary>
        [Remark("抑制")]
        Suppress = 1,

        /// <summary>
        /// 不做处理
        /// </summary>
        [Remark("不做处理")]
        None = 2
    }

    /// <summary>
    /// 活动分支模式
    /// </summary>
    [Remark("分支模式")]
    public enum SplitType
    {
        /// <summary>
        /// 单一分支
        /// </summary>
        [Remark("单一分支")]
        XOR = 1,

        /// <summary>
        /// 多路分支
        /// </summary>
        [Remark("多路分支")]
        OR = 2,

        /// <summary>
        /// 全部分支
        /// </summary>
        [Remark("全部分支")]
        AND = 3
    }

    /// <summary>
    /// 优先级类型
    /// </summary>
    [Remark("优先级类型")]
    public enum PriorityType
    {
        /// <summary>
        /// 高
        /// </summary>
        [Remark("高")]
        High = 1,

        /// <summary>
        /// 次高
        /// </summary>
        [Remark("次高")]
        SecondaryHigh = 2,

        /// <summary>
        /// 中
        /// </summary>
        [Remark("中")]
        Middle = 3,

        /// <summary>
        /// 次中
        /// </summary>
        [Remark("次中")]
        SecondaryMiddle = 4,

        /// <summary>
        /// 低
        /// </summary>
        [Remark("低")]
        Low = 5,

        /// <summary>
        /// 次低
        /// </summary>
        [Remark("次低")]
        SecondaryLow = 6
    }

    /// <summary>
    /// 激活规则
    /// </summary>
    [Remark("激活规则")]
    public enum ActivateRuleType
    {
        /// <summary>
        /// 直接运行
        /// </summary>
        [Remark("直接运行")]
        DirectRunning = 1,

        /// <summary>
        /// 待激活
        /// </summary>
        [Remark("待激活")]
        WaitActivate = 2,

        /// <summary>
        /// 自动跳转
        /// </summary>
        [Remark("自动跳转")]
        AutoSkip = 3
    }

    /// <summary>
    /// URL类型,ManualProcess(人工处理),WebURL(web展现)
    /// </summary>
    public enum URLType
    {
        /// <summary>
        /// 默认URL
        /// </summary>
        [Remark("默认URL")]
        DefaultURL = 1,

        /// <summary>
        /// 人工处理
        /// </summary>
        [Remark("人工处理")]
        ManualProcess = 2,

        /// <summary>
        /// Web页面
        /// </summary>
        [Remark("Web页面")]
        CustomURL = 3
    }

    /// <summary>
    /// 调用模式
    /// </summary>
    [Remark("调用模式")]
    public enum InvokePattern
    {
        /// <summary>
        /// 同步
        /// </summary>
        [Remark("同步")]
        Synchronous = 1,

        /// <summary>
        /// 异步
        /// </summary>
        [Remark("异步")]
        Asynchronous = 2
    }

    ///// <summary>
    ///// 数据类型
    ///// </summary>
    //[Remark("数据类型")]
    //public enum DataType
    //{
    //    /// <summary>
    //    /// 整数
    //    /// </summary>
    //    [Remark("整数")]
    //    Integer = 1,

    //    /// <summary>
    //    /// 浮点数
    //    /// </summary>
    //    [Remark("浮点数")]
    //    Float = 2,

    //    /// <summary>
    //    /// 日期
    //    /// </summary>
    //    [Remark("日期")]
    //    DateTime = 3,

    //    /// <summary>
    //    /// 字符串
    //    /// </summary>
    //    [Remark("字符串")]
    //    String = 4,

    //    /// <summary>
    //    /// 布尔型
    //    /// </summary>
    //    [Remark("布尔型")]
    //    Boolean = 5,
    //    [Remark("曲线图")]
    //    line = 6,
    //    [Remark("柱状图")]
    //    column = 7,
    //    [Remark("饼图")]
    //    pie = 8,
    //    [Remark("区域图")]
    //    area = 9
    //}

    ///// <summary>
    ///// 字段控件类型
    ///// </summary>
    //[Remark("字段控件类型")]
    //public enum ControlType
    //{
    //    /// <summary>
    //    /// 文本
    //    /// </summary>
    //    [Remark("文本")]
    //    Text = 0,

    //    [Remark("单项输入框")]
    //    TextBox = 1,

    //    /// <summary>
    //    /// 日期控件
    //    /// </summary>
    //    [Remark("日期控件")]
    //    DatePicker = 2,

    //    ///// <summary>
    //    ///// 日期范围控件
    //    ///// </summary>
    //    //[Remark("日期范围控件")]
    //    //DateRangePicker = 3,

    //    /// <summary>
    //    /// 月份控件
    //    /// </summary>
    //    [Remark("月份控件")]
    //    MonthPicker = 4,

    //    ///// <summary>
    //    ///// 月份范围控件
    //    ///// </summary>
    //    //[Remark("月份范围控件")]
    //    //MonthRangePicker = 5,

    //    /// <summary>
    //    /// 年份控件
    //    /// </summary>
    //    [Remark("年份控件")]
    //    YearPicker = 6,

    //    /// <summary>
    //    /// 单选项列表
    //    /// </summary>
    //    [Remark("单选项列表")]
    //    SingleCombox = 7,

    //    /// <summary>
    //    /// 多选项列表
    //    /// </summary>
    //    [Remark("多选项列表")]
    //    MultiCombox = 8,

    //    /// <summary>
    //    /// 复选框
    //    /// </summary>
    //    [Remark("复选框")]
    //    CheckBox = 9,

    //    ///// <summary>
    //    ///// 单选按钮
    //    ///// </summary>
    //    //[Remark("单选按钮")]
    //    //RadioBox = 10,
    //    [Remark("下拉列表")]
    //    DropDown = 10,
    //    /// <summary>
    //    /// 选择框
    //    /// </summary>
    //    [Remark("选择列表框")]
    //    ChooseBox = 11,

    //    /// <summary>
    //    /// 选择树
    //    /// </summary>
    //    [Remark("选择树")]
    //    ChooseTree = 12,

    //    [Remark("按钮")]

    //    Button = 13,

    //    [Remark("多行输入框")]

    //    TextArea = 14,
    //    [Remark("单选按钮")]
    //    Radio = 15,
    //    [Remark("邮箱输入框")]
    //    Email = 16,
    //    [Remark("选择框")]
    //    Combox = 17,
    //    [Remark("上传按钮")]
    //    Upload = 18,

    //    [Remark("隐藏控件")]
    //    HiddenInput = 19,

    //    [Remark("系统变量")]
    //    SysVariable = 20,
    //      [Remark("容器控件")]
    //    Div = 21,

    //    [Remark("Grid控件")]
    //    Grid = 22,
        
    //    [Remark("向导控件")]
    //    Wizard = 23,
    //    [Remark("表格控件")]
    //    DataTable = 24,
    //    [Remark("图片控件")]
    //    Image = 25,
    //    [Remark("图表控件")]
    //    Chart = 26

    //    ///// <summary>
    //    ///// 文本范围
    //    ///// </summary>
    //    //[Remark("文本范围")]
    //    //TextRangebox = 13
    //}


    //[Remark("系统控件类型")]
    //public enum SystemControlType
    //{
    //    /// <summary>
    //    /// 文本
    //    /// </summary>
    //    [Remark("组织单位名")]
    //    OrgID = 0,

    //    [Remark("用户名")]
    //    UserID = 1,

    //    /// <summary>
    //    /// 日期控件
    //    /// </summary>
    //    [Remark("当前日期")]
    //    CurrentDate = 2,

       
    //}
    /// <summary>
    /// 多工作项分配策略
    /// </summary>
    public enum WorkItemNumStrategy
    {
        /// <summary>
        /// 按参考者设置个数领取工作项
        /// </summary>
        [Remark("按参考者设置个数领取工作项")]
        ParticipantNumber = 1,

        /// <summary>
        /// 按操作员个数分配工作项
        /// </summary>
        [Remark("按操作员个数分配工作项")]
        OperatorNumber = 2
    }

    /// <summary>
    /// 完成规则
    /// </summary>
    [Remark("完成规则")]
    public enum FinishRule
    {
        /// <summary>
        /// 全部完成
        /// </summary>
        [Remark("全部完成")]
        FinishAll = 1,

        /// <summary>
        /// 完成个数
        /// </summary>
        [Remark("完成个数")]
        SpecifyNum = 2,

        /// <summary>
        /// 完成百分比
        /// </summary>
        [Remark("完成百分比")]
        SpecifyPercent = 3
    }

    /// <summary>
    /// 自由范围设置策略
    /// </summary>
    [Remark("自由范围设置策略")]
    public enum FreeRangeStrategy
    {
        /// <summary>
        /// 在该流程范围内自由
        /// </summary>
        [Remark("在该流程范围内自由")]
        FreeWithinProcess = 1,

        /// <summary>
        /// 在指定活动列表范围内自由
        /// </summary>
        [Remark("在指定活动列表范围内自由")]
        FreeWithinActivities = 2,

        /// <summary>
        /// 在后继活动范围内自由
        /// </summary>
        [Remark("在后继活动范围内自由")]
        FreeWithinNextActivites = 3
    }

    /// <summary>
    /// 重启活动参与者
    /// </summary>
    [Remark("重启活动参与者")]
    public enum ResetParticipant
    {
        /// <summary>
        /// 最初参与者
        /// </summary>
        [Remark("最初参与者")]
        FirstParticipantor = 1,

        /// <summary>
        /// 最终参与者
        /// </summary>
        [Remark("最终参与者")]
        LastParticipantor = 2
    }

    ///// <summary>
    ///// 读写方式
    ///// </summary>
    //public enum AccessPattern
    //{
    //    /// <summary>
    //    /// 只读
    //    /// </summary>
    //    [Remark("只读")]
    //    ReadOnly = 1,

    //    /// <summary>
    //    /// 读写
    //    /// </summary>
    //    [Remark("读写")]
    //    Write = 2
    //}

    /// <summary>
    ///  触发时机,创建（create）,启动（start）,结束（terminate）,超时（overtime）,提醒（remind）
    /// </summary>
    [Remark("触发事件")]
    public enum TriggerEventType
    {
        /// <summary>
        /// 活动创建前
        /// </summary>
        [Remark("活动创建前")]
        ActivityBeforeCreate = 1,

        /// <summary>
        /// 活动启动前
        /// </summary>
        [Remark("活动启动前")]
        ActivityBeforeStart = 2,

        /// <summary>
        /// 活动启动后
        /// </summary>
        [Remark("活动启动后")]
        ActivityAfterStart = 3,

        /// <summary>
        /// 活动超时后
        /// </summary>
        [Remark("活动超时后")]
        ActivityAfterOverTime = 4,

        /// <summary>
        /// 活动终止后
        /// </summary>
        [Remark("活动终止后")]
        ActivityAfterTerminate = 5,

        /// <summary>
        /// 活动完成后
        /// </summary>
        [Remark("活动完成后")]
        ActivityCompleted = 6,

        /// <summary>
        /// 活动提醒前
        /// </summary>
        [Remark("活动提醒前")]
        ActivityBeforeRemind = 7,

        /// <summary>
        /// 工作项创建前
        /// </summary>
        [Remark("工作项创建前")]
        WorkItemBeforeCreate = 21,

        /// <summary>
        /// 工作项创建后
        /// </summary>
        [Remark("工作项创建后")]
        WorkItemAtferCreate = 22,

        /// <summary>
        /// 工作项执行前
        /// </summary>
        [Remark("工作项执行时")]
        WorkItemExecuting = 23,

        /// <summary>
        /// 工作项完成后
        /// </summary>
        [Remark("工作项完成后")]
        WorkItemCompleted = 24,

        /// <summary>
        /// 工作项执行出错时
        /// </summary>
        [Remark("工作项执行出错时")]
        WorkItemError = 25,

        /// <summary>
        /// 工作项取消
        /// </summary>
        [Remark("工作项取消")]
        WorkItemCanncel = 26,

        /// <summary>
        /// 工作项超时
        /// </summary>
        [Remark("工作项超时")]
        WorkItemOverTime = 27,

        /// <summary>
        /// 工作项挂起
        /// </summary>
        [Remark("工作项挂起")]
        WorkItemSuspended = 28
    }

    /// <summary>
    /// 参数方向
    /// </summary>
    [Remark("参数方向")]
    public enum ParameterDirection
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        [Remark("输入参数")]
        In = 1,

        /// <summary>
        /// 输出参数
        /// </summary>
        [Remark("输出参数")]
        Out = 2,

        /// <summary>
        /// 输入输出参数
        /// </summary>
        [Remark("输入输出参数")]
        Ref = 3
    }

    /// <summary>
    /// 终结方式
    /// </summary>
    [Remark("终结方式")]
    public enum TerminateType
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Remark("自动")]
        Auto = 1,

        /// <summary>
        /// 人工
        /// </summary>
        [Remark("人工")]
        Manual = 2
    }

    /// <summary>
    /// 异常处理策略
    /// </summary>
    [Remark("异常处理策略")]
    public enum ExceptionStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        [Remark("回滚")]
        Rollback = 1,

        /// <summary>
        /// 
        /// </summary>
        [Remark("忽略")]
        Ignore = 2
    }

    /// <summary>
    /// 提醒策略
    /// </summary>
    [Remark("提醒策略")]
    public enum RemindStrategy
    {
        /// <summary>
        /// 自定义时间限制
        /// </summary>
        [Remark("自定义时间限制")]
        RemindLimtTime = 1,

        /// <summary>
        /// 从相关数据获取时间限制
        /// </summary>
        [Remark("从相关数据获取时间限制")]
        RemindRelevantLimitTime = 2
    }

    /// <summary>
    /// 时间限制策略
    /// </summary>
    [Remark("时间限制策略")]
    public enum TimeLimitStrategy
    {
        /// <summary>
        /// 自定义时间限制
        /// </summary>
        [Remark("自定义时间限制")]
        LimitTime = 1,

        /// <summary>
        /// 从相关数据获取时间限制
        /// </summary>
        [Remark("从相关数据获取时间限制")]
        RelevantLimitTime = 2

    }

    /// <summary>
    /// 提醒类型
    /// </summary>
    [Remark("提醒类型")]
    public enum RemindType
    {
        /// <summary>
        /// Email
        /// </summary>
        [Remark("Email")]
        Email = 1,

        /// <summary>
        /// Email
        /// </summary>
        [Remark("短信")]
        Sms = 2,

        /// <summary>
        /// 电话
        /// </summary>
        [Remark("电话")]
        Phone = 3,

        /// <summary>
        /// 自定义
        /// </summary>
        [Remark("自定义")]
        Custom = 4
    }

    /// <summary>
    /// 日历类型
    /// </summary>
    [Remark("日历类型")]
    public enum CalendarType
    {
        /// <summary>
        /// 默认日历
        /// </summary>
        [Remark("默认日历")]
        CDefault = 1,

        /// <summary>
        /// 24x7日历
        /// </summary>
        [Remark("24x7日历")]
        C24x7 = 2,

        /// <summary>
        /// 流程日历
        /// </summary>
        [Remark("流程日历")]
        CProcess = 3,

        /// <summary>
        /// 参与者日历
        /// </summary>
        [Remark("参与者日历")]
        CParticipant = 4
    }
}
