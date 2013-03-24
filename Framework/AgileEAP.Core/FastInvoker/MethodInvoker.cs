using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AgileEAP.Core.FastInvoker
{
    /// <summary>
    /// Fast method invoker class, This class provide fast dynamic method invoker.
    /// </summary>
    /// <typeparam name="TargetObjectType">The target object type</typeparam>
    public class MethodInvoker<TargetObjectType>
    {
        #region Veriables

        protected FastInvokeHandler methodHandler;

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region ClassConstructor

        /// <summary>
        /// Initialize the method invoker
        /// </summary>
        /// <param name="TargetObjectType">The Target type</param>
        /// <param name="Method">The Target Method information</param>
        public MethodInvoker(string Method)
        {
            methodHandler = BaseMethodInvoker.GetMethodInvoker(typeof(TargetObjectType).GetMethod(Method));
        }

        /// <summary>
        /// Initialize the method invoker
        /// </summary>
        /// <param name="TargetObjectType">The Target type</param>
        /// <param name="Method">The Target Method Name</param>
        public MethodInvoker(MethodInfo Method)
        {
            methodHandler = BaseMethodInvoker.GetMethodInvoker(Method);
        }

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region Functions

        /// <summary>
        /// Invoke the method
        /// </summary>
        /// <param name="TargetObjectType">The Target object that hold the method</param>
        /// <param name="Paramters">The Paramters</param>
        /// <returns>the method return value</returns>
        public object Invoke(TargetObjectType TargetObject, params object[] Paramters)
        {
            return methodHandler.Invoke(TargetObject, Paramters);
        }

        /// <summary>
        /// Asynchronous Invocation for the target method
        /// </summary>
        /// <param name="TargetObjectType">The Target object that hold the method</param>
        /// <param name="Paramters">The Paramters</param>
        /// <param name="Callback">The Callback delegate</param>
        /// <param name="Object">State object</param>
        /// <returns>IAsyncResult Represents the status of an asynchronous operation.</returns>
        public IAsyncResult BeginInvoke(TargetObjectType TargetObject, object[] Paramters, AsyncCallback Callback, object @Object)
        {
            return methodHandler.BeginInvoke(TargetObject, Paramters, Callback, @Object);
        }
        
        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        /// <summary>
        /// return string represent for the MethodInvoker object.
        /// </summary>
        public override string ToString()
        {
            return "Method Invoker : " + methodHandler.Method.Name;
        }
    }
}
