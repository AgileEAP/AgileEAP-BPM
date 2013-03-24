using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace AgileEAP.Core.FastInvoker
{
    /// <summary>
    /// Base Field Accessor class that provide Fast Dynamic Accessor to field using field Name.
    /// Like the ordinary Reflection, but it very fast.
    /// </summary>
    static class BaseFieldAccessor
    {
        /// <summary>
        /// Get the field invoker for set the field data.
        /// </summary>
        /// <typeparam name="TargetObjectType">The type of the object that hold the field</typeparam>
        /// <typeparam name="FieldType">The field type</typeparam>
        /// <param name="FieldName">The name of the field</param>
        /// <returns>The Field Set invoker</returns>
        public static FieldFastSetInvokeHandler<TargetType, FieldType> SetFieldInvoker<TargetType, FieldType>(string FieldName)
        {
            return SetFieldInvoker<TargetType, FieldType>(typeof(TargetType).GetField(FieldName));
        }

        /// <summary>
        /// Get the field invoker for get the field data.
        /// </summary>
        /// <typeparam name="TargetObjectType">The type of the object that hold the field</typeparam>
        /// <typeparam name="FieldType">The field type</typeparam>
        /// <param name="FieldName">The name of the field</param>
        /// <returns>The Field Get invoker</returns>
        public static FieldFastGetInvokeHandler<TargetType, FieldType> GetFieldInvoker<TargetType, FieldType>(string FieldName)
        {
            return GetFieldInvoker<TargetType, FieldType>(typeof(TargetType).GetField(FieldName));
        }

        /// <summary>
        /// Get the field invoker for set the field data.
        /// </summary>
        /// <typeparam name="TargetObjectType">The type of the object that hold the field</typeparam>
        /// <typeparam name="FieldType">The field type</typeparam>
        /// <param name="Field">The field information</param>
        /// <returns>The Field Set invoker</returns>
        public static FieldFastSetInvokeHandler<TargetType, FieldType> SetFieldInvoker<TargetType, FieldType>(FieldInfo Field)
        {
            Type objectType = typeof(TargetType);

            if (Field != null)
            {
                // Member is a Field...

                DynamicMethod dm = new DynamicMethod("Set" + Field.Name, null, new Type[] { objectType, typeof(FieldType) }, objectType);
                ILGenerator il = dm.GetILGenerator();

                // Load the instance of the object (argument 0) onto the stack
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);

                // Load the value of the object's field (fi) onto the stack
                il.Emit(OpCodes.Stfld, Field);

                // return the value on the top of the stack
                il.Emit(OpCodes.Ret);

                return (FieldFastSetInvokeHandler<TargetType, FieldType>)dm.CreateDelegate(typeof(FieldFastSetInvokeHandler<TargetType, FieldType>));
            }
            else
                throw new Exception(String.Format("Member: '{0}' is not a Field of Type: '{1}'", Field.Name, objectType.Name));
        }

        /// <summary>
        /// Get the field invoker for get the field data.
        /// </summary>
        /// <typeparam name="TargetObjectType">The type of the object that hold the field</typeparam>
        /// <typeparam name="FieldType">The field type</typeparam>
        /// <param name="Field">The field information</param>
        /// <returns>The Field Get invoker</returns>
        public static FieldFastGetInvokeHandler<TargetType, FieldType> GetFieldInvoker<TargetType, FieldType>(FieldInfo Field)
        {
            Type objectType = typeof(TargetType);

            if (Field != null)
            {
                // Member is a Field...

                DynamicMethod dm = new DynamicMethod("Get" + Field.Name, typeof(FieldType), new Type[] { objectType }, objectType);
                ILGenerator il = dm.GetILGenerator();

                // Load the instance of the object (argument 0) onto the stack
                il.Emit(OpCodes.Ldarg_0);
                // Load the value of the object's field (fi) onto the stack
                il.Emit(OpCodes.Ldfld, Field);
                // return the value on the top of the stack
                il.Emit(OpCodes.Ret);

                return (FieldFastGetInvokeHandler<TargetType, FieldType>)dm.CreateDelegate(typeof(FieldFastGetInvokeHandler<TargetType, FieldType>));
            }
            else
                throw new Exception(String.Format("Member: '{0}' is not a Field of Type: '{1}'", Field.Name, objectType.Name));
        }
    }
}
