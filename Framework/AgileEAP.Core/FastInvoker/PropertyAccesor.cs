using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AgileEAP.Core.FastInvoker
{
    /// <summary>
    /// Fast Property Accessor class, This class provide fast dynamic Property accessor.
    /// </summary>
    /// <typeparam name="TargetObjectType">The target object type</typeparam>
    /// <typeparam name="PropertyType">The Property type</typeparam>
    public class PropertyAccessor<TargetType, PropertyType>
    {
        #region Veriables

        protected FastInvokeHandler getMethodHandler;
        protected FastInvokeHandler setMethodHandler;

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region ClassConstructor

        /// <summary>
        /// Initialize the Property accessor
        /// </summary>
        /// <param name="Property">Property Name</param>
        public PropertyAccessor(string Property)
        {
            getMethodHandler = BasePropertyAccessor.GetPropertyInvoker(typeof(TargetType), Property);
            setMethodHandler = BasePropertyAccessor.SetPropertyInvoker(typeof(TargetType), typeof(TargetType).GetProperty(Property));
        }

        /// <summary>
        /// Initialize the Property accessor
        /// </summary>
        /// <param name="Property">Property information</param>
        public PropertyAccessor(PropertyInfo Property)
        {
            getMethodHandler = BasePropertyAccessor.GetPropertyInvoker(typeof(TargetType), Property.Name);
            setMethodHandler = BasePropertyAccessor.SetPropertyInvoker(typeof(TargetType), Property);
        }

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region Functions

        /// <summary>
        /// Get the property value
        /// </summary>
        /// <param name="TargetObjectType">The Target object that hold the field</param>
        /// <param name="Paramters">the Target property Paramters</param>
        /// <returns>the property value</returns>
        public PropertyType Get(TargetType TargetObject, params object[] Paramters)
        {
            return (PropertyType) getMethodHandler(TargetObject, Paramters);
        }

        /// <summary>
        /// Set the property value
        /// </summary>
        /// <param name="TargetObjectType">The Target object that hold the field</param>
        /// <param name="Paramters">the Target property Paramters</param>
        /// <returns>the property value</returns>
        public void Set(TargetType TargetObject, params object[] Paramters)
        {
            setMethodHandler(TargetObject, Paramters);
        }
        
        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        /// <summary>
        /// return string represent for the PropertyAccessor object.
        /// </summary>
        public override string ToString()
        {
            return "Property Invoker : " + getMethodHandler.Method.Name.Substring(4);
        }
    }
}
