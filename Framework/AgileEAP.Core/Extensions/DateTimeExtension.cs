using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 把小于1800/01/01日期转化成1800/01/01
        /// </summary>
        /// <param name="InputDateTime">原日期值</param>
        /// <returns></returns>
        public static DateTime ToSafeDateTime(this  System.DateTime InputDateTime)
        {
            string minDate = "1800/01/01";
            if (InputDateTime < Convert.ToDateTime(minDate))
            {
                InputDateTime = Convert.ToDateTime(minDate);
            }
            return InputDateTime;

        }
    }
}
