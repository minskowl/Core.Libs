using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Savchin.ComponentModel;
using Savchin.Logging;

namespace Savchin.Xml
{
    /// <summary>
    /// Implements helper methods for serialization and deserialization objects
    /// </summary>
    public static class XmlSerialaizerHelper
    {

        private static readonly Hashtable Cache = new Hashtable();
        private static ILogger _logger;

        /// <summary>
        /// Inits the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        public static void Init(params Type[] types)
        {
            var serializers = XmlSerializer.FromTypes(types);
            lock (Cache)
                for (int i = 0; i < types.Length; i++)
                {
                    Cache[types[i]] = serializers[i];
                }

        }

        /// <summary>
        /// Adds the serializer.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serializer">The serializer.</param>
        public static void AddSerializer(Type key, XmlSerializer serializer)
        {
            lock (Cache)
                Cache[key] = serializer;
        }

        /// <summary>
        /// Clones the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T CloneByXml<T>(this T data)
        {
            try
            {
                var type = typeof(T);
                return (T)data.ToXml(type).ToObject(type);
            }
            catch (Exception ex)
            {
                Log(ex);
                return default(T);
            }

        }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static XmlSerializer GetSerializer(this Type type)
        {

            if (type == null)
                return null;

            XmlSerializer res;
            lock (Cache)
            {
                res = Cache[type] as XmlSerializer;
                if (res != null)
                    return res;

                res = new XmlSerializer(type);
                Cache[type] = res;
            }

            Debug.WriteLine($"information: register type in XmlSerialaizerHelper [{type.FullName}]");

            return res;
        }

        /// <summary>
        /// Serialize data to the XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToXml<T>(this T data)
        {
            return data.ToXml(typeof(T));
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="writer">The writer.</param>
        /// <exception cref="SerializationException"></exception>
        public static void ToXml<T>(this T data, XmlWriter writer)
        {
            var type = typeof(T);
            try
            {
                GetSerializer(type).Serialize(writer, data);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"Error during serialization {type}", ex);
            }
        }

        /// <summary>
        /// Toes the XML.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ToXml(this object data, Type type)
        {
            try
            {
                using (var stringWriter = new Utf8StringWriter())
                {
                    GetSerializer(type).Serialize(stringWriter, data);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException($"Error during serialization {type}", ex);
            }
        }


        /// <summary>
        /// Toes the object.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <param name="typeName">Name of the type.</param>
        public static object ToObject(this string xml, string typeName)
        {
            try
            {
                if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(xml))
                    return null;

                var type = Type.GetType(typeName);
                return type == null ? null : xml.ToObject(type);
            }
            catch (Exception ex)
            {
                Log(ex);
                return null;
            }
        }

        /// <summary>
        /// Deserialize data from the XML string.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public static T ToObject<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return default(T);
            try
            {
                return (T)xml.ToObject(typeof(T));
            }
            catch (Exception ex)
            {
                Log(ex);
                return default(T);
            }
        }
        /// <summary>
        /// To the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T ToObject<T>(this Stream stream)
        {
            try
            {
                return (T)typeof(T).GetSerializer().Deserialize(stream);
            }
            catch (InvalidOperationException ex)
            {
                Log(ex);
                return default(T);
            }
        }

        /// <summary>
        /// To the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T ToObject<T>(this XmlReader reader)
        {
            try
            {
                return (T)typeof(T).GetSerializer().Deserialize(reader);
            }
            catch (Exception ex)
            {
                Log(ex);
                return default(T);
            }
        }

        private static object ToObject(this string xml, Type type)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            using (var stringReader = new StringReader(xml))
            {
                return type.GetSerializer().Deserialize(stringReader);
            }
        }


        private static void Log(Exception ex)
        {
            if (_logger == null)
                _logger = ServiceLocator.GetInstance<ILogger>();

            var text = ex.ToString();
            _logger?.Warning(text);
            Debug.WriteLine(text);
        }

        /// <summary>
        /// StringWriter with UTF-8 encoding
        /// </summary>
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
