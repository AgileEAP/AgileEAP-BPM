using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Utility
{
    public static class ExpressionUtil
    {
        /// <summary>
        /// 构造递归Lambda表达式
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult> MakeRecursion<T1, T2, T3, TResult>(Func<Func<T1, T2, T3, TResult>, Func<T1, T2, T3, TResult>> fun)
        {
            return (x, y, z) => fun(MakeRecursion(fun))(x, y, z);
        }



        /// <summary>
        /// 构造递归Lambda表达式
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Func<T1, T2, TResult> MakeRecursion<T1, T2, TResult>(Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> fun)
        {
            return (x, y) => fun(MakeRecursion(fun))(x, y);
        }

        /// <summary>
        /// 构造递归Lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="targetObject"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Func<T, TResult> MakeRecursion<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> fun)
        {
            return x => fun(MakeRecursion(fun))(x);
        }
    }
}
