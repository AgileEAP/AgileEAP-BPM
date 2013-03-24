using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AgileEAP.Core.Extensions
{
    public static class TypeExtension
    {
        public static PropertyInfo GetPropertyIgnoreCase(this Type type, string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo item in properties)
            {
                if (item.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return null;
        }

        public static bool ExistsProperty(this Type type, string propertyName)
        {
            return type.GetPropertyIgnoreCase(propertyName) != null;
        }
    }
}
