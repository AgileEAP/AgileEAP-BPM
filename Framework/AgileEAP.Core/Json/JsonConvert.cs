using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft;
using Newtonsoft.Json;

namespace AgileEAP.Core
{
    public class JsonConvert
    {
        /// <summary>
        ///  Deserializes the JSON to the given anonymous type.
        /// </summary>
        /// <typeparam name="T">The anonymous type to deserialize to. This can't be specified traditionally</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>     The deserialized anonymous type from the JSON string.</returns>
        public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType<T>(value, anonymousTypeObject);
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value"> The JSON to deserialize.</param>
        /// <returns>The deserialized object from the Json string.</returns>
        public static object DeserializeObject(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        ///  Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">  The type of the object to deserialize to.</typeparam>
        /// <param name="value"> The JSON to deserialize.</param>
        /// <returns>The deserialized object from the Json string.</returns>
        public static T DeserializeObject<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        //
        // Summary:
        //     Deserializes the JSON to the specified .NET type.
        //
        // Parameters:
        //   value:
        //     The JSON to deserialize.
        //
        //   converters:
        //     Converters to use while deserializing.
        //
        // Type parameters:
        //   T:
        //     The type of the object to deserialize to.
        //
        // Returns:
        //     The deserialized object from the JSON string.
        public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value, converters);
        }
        ///// <summary>
        /////   Deserializes the JSON to the specified .NET type.
        ///// </summary>
        ///// <typeparam name="T"> The type of the object to deserialize to.</typeparam>
        ///// <param name="value">The object to deserialize.</param>
        ///// <param name="settings">The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object.If this is null, default serialization settings will be is used.</param>
        ///// <returns>The deserialized object from the JSON string.</returns>
        //public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value, settings);
        //}

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value"> The JSON to deserialize.</param>
        /// <param name="type"> The System.Type of object being deserialized.</param>
        /// <returns>The deserialized object from the Json string.</returns>
        public static object DeserializeObject(string value, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, type);
        }


        /// <summary>
        /// Deserializes the XmlNode from a JSON string.
        /// </summary>
        /// <param name="value">   The JSON string.</param>
        /// <returns>eserializes the XmlNode from a JSON string.</returns>
        public static System.Xml.XmlNode DeserializeXmlNode(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeXmlNode(value);
        }

        /// <summary>
        /// Deserializes the XmlNode from a JSON string nested in a root elment.
        /// </summary>
        /// <param name="value"> The JSON string.</param>
        /// <param name="rootElementName">The name of the root element to append when deserializing.</param>
        /// <returns>he deserialized XmlNode</returns>
        public static System.Xml.XmlNode DeserializeXmlNode(string value, string rootElementName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeXmlNode(value, rootElementName);
        }

        /// <summary>
        ///  Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value"> The object to serialize.</param>
        /// <returns> Serializes the specified object to a JSON string.</returns>
        public static string SerializeObject(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        //
        // Summary:
        //     Serializes the specified object to a JSON string using a collection of Newtonsoft.Json.JsonConverter.
        //
        // Parameters:
        //   value:
        //     The object to serialize.
        //
        //   settings:
        //     The Newtonsoft.Json.JsonSerializerSettings used to serialize the object.
        //      If this is null, default serialization settings will be is used.
        //
        // Returns:
        //     A JSON string representation of the object.
        public static string SerializeObject(object value, JsonSerializerSettings settings)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, settings);
        }
        //
        // Summary:
        //     Serializes the specified object to a JSON string using a collection of Newtonsoft.Json.JsonConverter.
        //
        // Parameters:
        //   value:
        //     The object to serialize.
        //
        //   converters:
        //     A collection converters used while serializing.
        //
        // Returns:
        //     A JSON string representation of the object.
        public static string SerializeObject(object value, params JsonConverter[] converters)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, converters);
        }
        //
        // Summary:
        //     Serializes the specified object to a JSON string using a collection of Newtonsoft.Json.JsonConverter.
        //
        // Parameters:
        //   value:
        //     The object to serialize.
        //
        //   formatting:
        //     Indicates how the output is formatted.
        //
        //   settings:
        //     The Newtonsoft.Json.JsonSerializerSettings used to serialize the object.
        //      If this is null, default serialization settings will be is used.
        //
        // Returns:
        //     A JSON string representation of the object.
        public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, formatting, settings);
        }

        //
        // Summary:
        //     Serializes the specified object to a JSON string using a collection of Newtonsoft.Json.JsonConverter.
        //
        // Parameters:
        //   value:
        //     The object to serialize.
        //
        //   formatting:
        //     Indicates how the output is formatted.
        //
        //   converters:
        //     A collection converters used while serializing.
        //
        // Returns:
        //     A JSON string representation of the object.
        public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, formatting, converters);
        }

        /// <summary>
        ///  Serializes the XML node to a JSON string.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <returns> A JSON string of the XmlNode.</returns>
        public static string SerializeXmlNode(System.Xml.XmlNode node)
        {
            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(node);
        }
    }
}
