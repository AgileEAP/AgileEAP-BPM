using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    /// 定义通知接口
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        void Notify();
    }

    /// <summary>
    /// 定义工作分配接口
    /// </summary>
    public interface IWorkAssign
    {
        /// <summary>
        /// 分配工作项
        /// </summary>
        /// <param name="workItemID"></param>
        void AssignWorkItem(string workItemID);
    }

    /// <summary>
    /// 把Engine所需要的服务的接口都定义在一个接口中，便于使用和管理
    /// </summary>
    public interface IEngineService
    {
        IWorkflowPersistence Persistence { get; set; }
        INotification Notification { get; set; }
        IWorkAssign WorkAssign { get; set; }
    }
}
