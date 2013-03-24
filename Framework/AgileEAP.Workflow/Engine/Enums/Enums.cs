using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileEAP.Core;

namespace AgileEAP.Workflow.Enums
{
    /// <summary>
    /// 工作流状态
    /// </summary>
    [Remark("工作流状态")]
    public enum ProcessStatus
    {
        /// <summary>
        /// 候选版本，还未发布
        /// </summary>
        [Remark("未发布")]
        Candidate = 0,

        /// <summary>
        /// 已经发布
        /// </summary>
        [Remark("已发布")]
        Release = 1,

        /// <summary>
        /// 终止
        /// </summary>
        [Remark("终止")]
        Terminated = 2
    }

    /// <summary>
    /// 流程实例状态
    /// </summary>
    public enum ProcessInstStatus
    {
        /// <summary>
        /// 未启动
        /// </summary>
        [Remark("未启动")]
        NoStart = 1,

        /// <summary>
        /// 运行
        /// </summary>
        [Remark("运行")]
        Running = 2,

        /// <summary>
        /// 挂起
        /// </summary>
        [Remark("挂起")]
        Suspended = 3,

        /// <summary>
        /// 完成
        /// </summary>
        [Remark("完成")]
        Completed = 4,

        /// <summary>
        /// 终止
        /// </summary>
        [Remark("终止")]
        Terminated = 5,

        /// <summary>
        /// 取消
        /// </summary>
        [Remark("取消")]
        Canceled = 6
    }

    /// <summary>
    /// 活动实例状态
    /// </summary>
    public enum ActivityInstStatus
    {
        /// <summary>
        /// 未启动
        /// </summary>
        [Remark("未启动")]
        NoStart = 1,

        /// <summary>
        /// 运行
        /// </summary>
        [Remark("运行")]
        Running = 2,

        /// <summary>
        /// 挂起
        /// </summary>
        [Remark("挂起")]
        Suspended = 3,

        /// <summary>
        /// 完成
        /// </summary>
        [Remark("完成")]
        Compeleted = 4,

        /// <summary>
        /// 终止
        /// </summary>
        [Remark("终止")]
        Terminated = 5,

        /// <summary>
        /// 取消
        /// </summary>
        [Remark("取消")]
        Canceled = 6,

        /// <summary>
        /// 待激活
        /// </summary>
        [Remark("待激活")]
        WaitActivate = 7,

        /// <summary>
        /// 异常
        /// </summary>
        [Remark("异常")]
        Error = 8
    }

    /// <summary>
    /// 工作项状态
    /// </summary>
    public enum WorkItemStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [Remark("待执行")]
        WaitExecute = 1,

        /// <summary>
        /// 停止
        /// </summary>
        [Remark("停止")]
        Stopped = 2,

        /// <summary>
        /// 执行
        /// </summary>
        [Remark("执行")]
        Executing = 3,

        /// <summary>
        /// 挂起
        /// </summary>
        [Remark("挂起")]
        Suspended = 4,

        /// <summary>
        /// 完成
        /// </summary>
        [Remark("完成")]
        Compeleted = 5,

        /// <summary>
        /// 终止
        /// </summary>
        [Remark("终止")]
        Terminated = 6,

        /// <summary>
        /// 取消
        /// </summary>
        [Remark("取消")]
        Canceled = 7,

        /// <summary>
        /// 出错
        /// </summary>
        [Remark("出错")]
        Error = 8
    }

    /// <summary>
    /// 工作项业务状态
    /// </summary>
    public enum WorkItemBizStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Remark("正常")]
        Common = 1,

        /// <summary>
        /// 代理
        /// </summary>
        [Remark("代理")]
        Agent = 2,

        /// <summary>
        /// 代办
        /// </summary>
        [Remark("代办")]
        Delegate = 3,

        /// <summary>
        /// 协办
        /// </summary>
        [Remark("协办")]
        Help = 4,

        /// <summary>
        /// 待确认
        /// </summary>
        [Remark("待确认")]
        WaitConfirm = 5,

        /// <summary>
        /// 修改
        /// </summary>
        [Remark("修改")]
        ReDo = 6
    }

    /// <summary>
    /// 代办类型
    /// </summary>
    [Remark("代办类型")]
    public enum DelegateType
    {
        /// <summary>
        /// 主办
        /// </summary>
        [Remark("主办")]
        Sponsor = 0,

        /// <summary>
        /// 代办
        /// </summary>
        [Remark("代办")]
        Delegate = 1,

        /// <summary>
        /// 协办
        /// </summary>
        [Remark("协办")]
        Assistant = 2
    }

    /// <summary>
    /// 参与类型-领取（GET）、执行(EXE)、曾经领取(OGET)、曾经执行(OEXE)、执行完成(PEXE)
    /// </summary>
    public enum PartiInType
    {
        /// <summary>
        /// 领取
        /// </summary>
        [Remark("领取")]
        Get = 1,

        /// <summary>
        /// 执行
        /// </summary>
        [Remark("执行")]
        Exe = 2,

        /// <summary>
        /// 曾经领取
        /// </summary>
        [Remark("曾经领取")]
        OGet = 3,

        /// <summary>
        /// 曾经执行
        /// </summary>
        [Remark("曾经执行")]
        OExe = 4,

        /// <summary>
        /// 执行完成
        /// </summary>
        [Remark("执行完成")]
        PExe = 5
    }

    /// <summary>
    /// 流程操作
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// 认领
        /// </summary>
        [Remark("认领")]
        Claim = 1,

        /// <summary>
        /// 启动
        /// </summary>
        [Remark("启动")]
        Start = 2,

        /// <summary>
        /// 停止
        /// </summary>
        [Remark("停止")]
        Stop = 3,

        /// <summary>
        /// 发布
        /// </summary>
        [Remark("发布")]
        Release = 4,

        /// <summary>
        /// 挂起
        /// </summary>
        [Remark("挂起")]
        Suspend = 5,

        /// <summary>
        /// 恢复
        /// </summary>
        [Remark("恢复")]
        Resume = 6,

        /// <summary>
        /// 跳转
        /// </summary>
        [Remark("跳转")]
        Skip = 7,

        /// <summary>
        /// 委托
        /// </summary>
        [Remark("委托")]
        Delegate = 8,

        /// <summary>
        /// 前进（提交）
        ///  [Remark("结束活动")]</summary>
        [Remark("提交")]
        Forward = 9,

        /// <summary>
        /// 完成
        /// </summary>
        [Remark("完成")]
        Complete = 10,

        /// <summary>
        /// 出错
        /// </summary>
        [Remark("出错")]
        Error = 11
    }

}
