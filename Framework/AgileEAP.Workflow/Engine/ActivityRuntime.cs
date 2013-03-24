using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core.Utility;

using AgileEAP.Core.Extensions;
using AgileEAP.Core.Caching;
using AgileEAP.Workflow.Domain;

using AgileEAP.Workflow.Enums;
using AgileEAP.Workflow.Definition;

namespace AgileEAP.Workflow.Engine
{
    /// <summary>
    ///  实现推动和更改具体某个Activity状态
    /// </summary>
    internal class ActivityRuntime
    {
        #region Variables
        private IEngineService engineService;
        private IList<WorkItem> workItems = new List<WorkItem>();
        private bool waiting = false;
        #endregion

        #region Properties
        public IWorkflowPersistence Persistence
        {
            get { return engineService.Persistence; }
        }
        public INotification Notification
        {
            get { return engineService.Notification; }
        }
        public IWorkAssign WorkAssign
        {
            get { return engineService.WorkAssign; }
        }

        public ActivityContext Context
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public ActivityRuntime(IEngineService services, ActivityContext context)
        {
            engineService = services;
            workItems = Persistence.GetWorkItems(context.ActivityInst.ID);
            Context = context;
        }
        #endregion

        public void Run()
        {
            waiting = true;

            while (waiting)
            {
                waiting = false;
                EventPublisher.Publish<ActivityExecutingEvent>(new ActivityExecutingEvent()
                {
                    UUID = string.Format("{0}-{1}-{2}-ActivityExecutingEvent", Context.ProcessDefine.ID, Context.ProcessDefine.Version, Context.Activity.ID),
                    Context = Context
                });

                foreach (var workItem in GetExecutingWorkItems(Context.ActivityInst))
                {
                    var workItemContext = new WorkItemContext()
                        {
                            ProcessDefine = Context.ProcessDefine,
                            ProcessInst = Context.ProcessInst,
                            ActivityInst = Context.ActivityInst,
                            WorkItem = workItem,
                            Parameters = Context.Parameters
                        };

                    WorkItemRuntime workItemRuntime = new WorkItemRuntime(engineService, workItemContext);

                    workItemRuntime.Run(workItem);
                }

                EventPublisher.Publish<ActivityExecutedEvent>(new ActivityExecutedEvent()
                {
                    UUID = string.Format("{0}-{1}-{2}-ActivityExecutedEvent", Context.ProcessDefine.ID, Context.ProcessDefine.Version, Context.Activity.ID),
                    Context = Context
                });
            }
        }

        public IList<WorkItem> GetExecutingWorkItems(ActivityInst activityInst)
        {
            return Persistence.GetWorkItems(activityInst.ID).Where(w => w.CurrentState == (short)WorkItemStatus.Executing).ToList();
        }

        //private void UpdateActiviyInstStatus(ActivityInst activityInst)
        //{
        //    //TODO: 还需要考虑其他条件和状态(Cancelled? Failed?)来判断Workflow的状态
        //    //TODO: 如果是新启动一个Task，还可能需要判断InComingDependency
        //    //TODO: 可能还要判断Dependency是否强制还是Default的

        //    TaskInstance ti = GetTaskInstance(task.TaskInstanceId);

        //    //TODO:
        //    //ExternalRuleCheckResult result = ExternalRuleCheck.ChekTaskStatusConstraint(taskInstanceId);

        //    if (task.NeedToSetToNewStatus)
        //    {
        //        if (ti.TaskStatus != task.NewStatus)
        //        {
        //            UpdateTaskStatus(ti, task.NewStatus, task);

        //            //If new start task, Check and assign work?
        //        }
        //    }
        //    else
        //    {
        //        switch (ti.TaskStatus)
        //        {
        //            case TaskStatus.not_started: //Start a not started Task
        //                UpdateTaskStatus(ti, TaskStatus.ready_to_start, task);
        //                //TODO: AssignWork
        //                break;
        //            case TaskStatus.ready_to_start:
        //                break;
        //            case TaskStatus.In_process:
        //                break;
        //            case TaskStatus.completed:
        //                break;
        //            case TaskStatus.Failed:
        //                break;
        //        }
        //    }
        //}
        //private void GetToCheckTasks()
        //{
        //    //TODO: 这里只是简单地检测当前在处理的Task，如果策略不同，需要修改

        //    foreach (TaskInstance ti in _taskInstances)
        //    {
        //        TaskDefinition taskDefinition = GetTaskDefinition(ti.TaskInstanceId);

        //        if (TaskInNeedToCheckStatus(ti.TaskStatus))
        //        {
        //            //Need to check if all predessors are completed (or no predessors)
        //            if (AllTaskPredecessorsCompleted(ti))
        //            {
        //                _tasksToCheck.Add(new NeedToCheckTask(ti.TaskInstanceId, false, TaskStatus.not_started));
        //            }
        //        }
        //    }
        //}
        //private bool TaskInNeedToCheckStatus(TaskStatus status)
        //{
        //    return status == TaskStatus.not_started
        //        || status == TaskStatus.ready_to_start
        //        || status == TaskStatus.In_process;
        //}
        //private bool AllTaskPredecessorsCompleted(TaskInstance taskInstance)
        //{
        //    IList<TaskInstance> predecessors = GetTaskPredecessors(taskInstance);
        //    bool result = true;

        //    foreach (TaskInstance ti in predecessors)
        //    {
        //        if (ti.TaskStatus != TaskStatus.completed)
        //        {
        //            result = false;
        //            break;
        //        }
        //    }
        //    return result;
        //}
        //private IList<TaskInstance> GetTaskPredecessors(TaskInstance taskInstance)
        //{
        //    IList<TaskInstance> result = new List<TaskInstance>();

        //    IList<TaskRoute> tds = Persistence.GetTaskInRoutes(taskInstance.TaskInstanceId);
        //    foreach (TaskInstance ti in _taskInstances)
        //    {
        //        foreach (TaskRoute td in tds)
        //        {
        //            if (ti.TaskInstanceId == td.TaskFrontId)
        //                result.Add(ti);
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 尽量保证这里是更改Task状态的唯一地方
        ///// </summary>
        ///// <param name="ti"></param>
        ///// <param name="newStatus"></param>
        ///// <param name="task"></param>
        //private void UpdateTaskStatus(TaskInstance ti, TaskStatus newStatus, NeedToCheckTask task)
        //{
        //    Persistence.UpdateTaskStatus(ti.TaskInstanceId, newStatus);
        //    ti.TaskStatus = newStatus;

        //    //
        //    _needToCheckAgain = true;

        //    //TODO: Check and Send out emails
        //}

        //private TaskDefinition GetTaskDefinition(int taskDefinitionId)
        //{
        //    foreach (TaskDefinition td in workItems)
        //    {
        //        if (td.TaskDefinitionId == taskDefinitionId)
        //            return td;
        //    }
        //    return null; //TODO: throw exceptions
        //}
        //private TaskInstance GetTaskInstance(int taskInstanceId)
        //{
        //    foreach (TaskInstance ti in _taskInstances)
        //    {
        //        if (ti.TaskInstanceId == taskInstanceId)
        //            return ti;
        //    }
        //    return null; //TODO: throw exceptions
        //}
    }
}
