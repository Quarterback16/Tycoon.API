using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Serialization;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions to serialize an object as a base 64 encoded string and deserialize a base 64 encoded string as an object.
    /// </summary>
    public static class SerializeExtension
    {
        /// <summary>
        /// Serializes an object as a base 64 encoded string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The object serialized as a base 64 encoded string.</returns>
        public static string Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return obj.Serialize<object>();
        }

        /// <summary>
        /// Serializes an object as a base 64 encoded string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The object serialized as a base 64 encoded string.</returns>
        public static string Serialize<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                using (var compressionStream = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    var formatter = new BinaryFormatter();
                    var surrogateSelector = new SurrogateSelector();

                    // Add the serialization surrogate to allow SelectListItem to be serialized
                    surrogateSelector.AddSurrogate(typeof(SelectListItem), new StreamingContext(StreamingContextStates.All), new SelectListItemSerializationSurrogate());
                    formatter.SurrogateSelector = surrogateSelector;
                    
                    // Serialize object to stream
                    formatter.Serialize(compressionStream, obj);

                    compressionStream.Flush();
                }

                stream.Position = 0;

                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        /// Deserializes a base 64 encoded string as an object.
        /// </summary>
        /// <param name="data">The base 64 encoded string data to deserialize.</param>
        /// <returns>The base 64 encoded string data deserialized as an object.</returns>
        public static object Deserialize(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            return data.Deserialize<object>();
        }

        /// <summary>
        /// Deserializes a base 64 encoded string as an object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize as.</typeparam>
        /// <param name="data">The base 64 encoded string data to deserialize.</param>
        /// <returns>The base 64 encoded string data deserialized as an object of the specified type.</returns>
        public static T Deserialize<T>(this string data) where T : class
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            return Convert.FromBase64String(data).Deserialize<T>();
        }

        /// <summary>
        /// Deserializes a byte array as an object.
        /// </summary>
        /// <param name="data">The byte array data to deserialize.</param>
        /// <returns>The byte array data deserialized as an object.</returns>
        public static object Deserialize(this byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            return data.Deserialize<object>();
        }

        /// <summary>
        /// Deserializes a byte array as an object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize as.</typeparam>
        /// <param name="data">The byte array data to deserialize.</param>
        /// <returns>The byte array data deserialized as an object of the specified type.</returns>
        public static T Deserialize<T>(this byte[] data) where T : class
        {
            if (data == null)
            {
                return null;
            }

            using (var stream = new MemoryStream(data))
            {
                using (var compressionStream = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    var formatter = new BinaryFormatter();
                    var surrogateSelector = new SurrogateSelector();

                    // Add the serialization surrogate to allow SelectListItem to be deserialized
                    surrogateSelector.AddSurrogate(typeof(SelectListItem), new StreamingContext(StreamingContextStates.All), new SelectListItemSerializationSurrogate());
                    formatter.SurrogateSelector = surrogateSelector;

                    // Deserialize stream as an object of the specified type
                    return formatter.Deserialize(compressionStream) as T;
                }
            }
        }
    }
}