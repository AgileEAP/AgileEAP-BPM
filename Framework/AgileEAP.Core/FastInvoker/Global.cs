using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AgileEAP.Core.FastInvoker
{

    /// <summary>
    /// The Fast Invoke Handler delegate
    /// </summary>
    /// <param name="target">The Target object</param>
    /// <param name="paramters">The Paramter list</param>
    /// <returns>The Target MethodName returning data</returns>
    public delegate object FastInvokeHandler(object target, params object[] paramters);

    /// <summary>
    /// The Fast Invoke Handler delegate
    /// </summary>
    /// <typeparam name="ReturnType">Return data type</typeparam>
    /// <param name="target">The Target object</param>
    /// <param name="paramters">The Paramter list</param>
    /// <returns>The Target MethodName returning data</returns>
    public delegate ReturnType FastInvokeHandler<ReturnType>(object target, params object[] paramters);

    /// <summary>
    /// The Fast get field invoke Handler delegate
    /// </summary>
    /// <typeparam name="TargetObjectType">The Target object type</typeparam>
    /// <typeparam name="FieldType">The field type</typeparam>
    /// <param name="obj">The Target object</param>
    /// <returns>The field return data</returns>
    public delegate FieldType FieldFastGetInvokeHandler<TargetType, FieldType>(TargetType obj);

    /// <summary>
    /// The Fast set field invoke Handler delegate
    /// </summary>
    /// <typeparam name="TargetObjectType">The Target object type</typeparam>
    /// <typeparam name="FieldType">The field type</typeparam>
    /// <param name="obj">The Target object</param>
    /// <param name="value">The field set data</param>
    public delegate void FieldFastSetInvokeHandler<TargetType, FieldType>(TargetType obj, FieldType value);

}
