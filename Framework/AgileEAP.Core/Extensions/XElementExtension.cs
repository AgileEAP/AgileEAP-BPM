using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AgileEAP.Core.Extensions
{
    /// <summary>
    /// XElementExtension
    /// </summary>
    public static class XElementExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xParent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ChildElementValue(this XElement xParent, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("name is null");

            if (xParent == null) return string.Empty;

            XElement xChild = xParent.Element(name);

            return xChild == null ? string.Empty : xChild.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        public static string GetSafeValue(this XElement xElem)
        {
            return xElem == null ? string.Empty : xElem.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElem"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string AttributeValue(this XElement xElem, string attribute)
        {
            return xElem.AttributeValue(attribute, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElem"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string AttributeValue(this XElement xElem, string attribute, string defaultValue)
        {
            return xElem.Attribute(attribute) == null ? defaultValue : xElem.Attribute(attribute).Value;
        }
    }
}
