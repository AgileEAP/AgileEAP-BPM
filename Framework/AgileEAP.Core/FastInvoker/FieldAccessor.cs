using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AgileEAP.Core.FastInvoker
{
    /// <summary>
    /// Fast Field Accessor class, This class provide fast dynamic field accessor.
    /// </summary>
    /// <typeparam name="TargetObjectType">The target object type</typeparam>
    /// <typeparam name="FieldType">The Field type</typeparam>
    public class FieldAccessor<TargetType, FieldType>
    {
        #region Veriables

        protected FieldFastGetInvokeHandler<TargetType, FieldType> getMethodHandler;
        protected FieldFastSetInvokeHandler<TargetType, FieldType> setMethodHandler;

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region ClassConstructor

        /// <summary>
        /// Initialize the field accessor
        /// </summary>
        /// <param name="Field">Field Name</param>
        public FieldAccessor(string Field)
        {
            getMethodHandler = BaseFieldAccessor.GetFieldInvoker<TargetType, FieldType>(typeof(TargetType).GetField(Field));
            setMethodHandler = BaseFieldAccessor.SetFieldInvoker<TargetType, FieldType>(typeof(TargetType).GetField(Field));
        }

        /// <summary>
        /// Initialize the field accessor
        /// </summary>
        /// <param name="Field">Field information</param>
        public FieldAccessor(FieldInfo Field)
        {
            getMethodHandler = BaseFieldAccessor.GetFieldInvoker<TargetType, FieldType>(Field);
            setMethodHandler = BaseFieldAccessor.SetFieldInvoker<TargetType, FieldType>(Field);
        }

        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        #region Functions

        /// <summary>
        /// Get the field value
        /// </summary>
        /// <param name="TargetObjectType">The Target object that hold the field</param>
        /// <returns>the field value</returns>
        public FieldType Get(TargetType TargetObject)
        {
            return getMethodHandler(TargetObject);
        }

        /// <summary>
        /// Set the field value
        /// </summary>
        /// <param name="TargetObjectType">The target object that hold the field</param>
        /// <param name="Value">The new field data</param>
        public void Set(TargetType TargetObject, FieldType Value)
        {
            setMethodHandler(TargetObject, Value);
        }
        
        #endregion

        /*//////////////////////////////////////////////////////////////////////////////////////*/

        /// <summary>
        /// return string represent for the FieldAccessor object.
        /// </summary>
        public override string ToString()
        {
            return "Property Invoker : " + getMethodHandler.Method.Name.Substring(4);
        }
    }
}
