using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using AgileEAP.Core.Extensions;

using AgileEAP.Workflow.Enums;
using AgileEAP.Core;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// 简单对象创建类工厂
    /// </summary>
    public sealed class ObejectFactory
    {
        private static ILogger log = LogManager.GetLogger(typeof(ObejectFactory));

        /// <summary>
        /// 创建Activity对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xElem"></param>
        /// <returns></returns>
        public static Activity CreateActivity(ConfigureElement parent, XElement xElem)
        {
            if (xElem == null) return null;

            Activity activity = null;
            try
            {
                string typeName = xElem.Element("activityType").Value;
                if (!typeName.StartsWith("AgileEAP.Workflow.Definition"))
                    typeName = string.Format("AgileEAP.Workflow.Definition.{0}", typeName);
                Type type = Type.GetType(typeName);
                activity = Activator.CreateInstance(type, new object[] { parent, xElem }) as Activity;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return activity;
        }

        /// 创建Activity对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Activity CreateActivity(ConfigureElement parent, string typeName)
        {
            if (parent == null) return null;

            string activityName = typeName;
            if (!activityName.StartsWith("AgileEAP.Workflow.Definition"))
                activityName = string.Format("AgileEAP.Workflow.Definition.{0}", activityName);

            Activity activity = null;
            try
            {
                Type type = Type.GetType(activityName);
                activity = Activator.CreateInstance(type, new object[] { parent, null }) as Activity;
                activity.ActivityType = typeName.Cast<ActivityType>(ActivityType.ManualActivity);
                activity.Name = RemarkAttribute.GetTypeRemark(type);
                activity.ID = typeName + Guid.NewGuid().ToString();
                ((ProcessDefine)parent).Activities.SafeAdd(activity);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return activity;
        }
    }
}
