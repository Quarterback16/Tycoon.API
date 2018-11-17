using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="byte"/>.
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// Compress text.
        /// </summary>
        /// <param name="text">The text to compress.</param>
        /// <param name="encoding">The encoding of the text (default is UTF-8).</param>
        /// <returns>The compressed byte array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text" /> is null or empty.</exception>
        public static byte[] Compress(this string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            encoding = encoding ?? Encoding.UTF8;

            return encoding.GetBytes(text).Compress(); 
        }

        /// <summary>
        /// Compress an uncompressed byte array.
        /// </summary>
        /// <param name="uncompressedBytes">The uncompressed byte array.</param>
        /// <returns>The compressed byte array.</returns>
        public static byte[] Compress(this byte[] uncompressedBytes)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    using (var uncompressedStream = new MemoryStream(uncompressedBytes))
                    {
                        uncompressedStream.CopyTo(zipStream);
                    }
                }

                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Uncompress a compressed byte array.
        /// </summary>
        /// <param name="compressedBytes">The compressed byte array.</param>
        /// <returns>The uncompressed byte array.</returns>
        public static byte[] Decompress(this byte[] compressedBytes)
        {
            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var uncompressedStream = new MemoryStream())
                    {
                        zipStream.CopyTo(uncompressedStream);

                        return uncompressedStream.ToArray();
                    }
                }
            }
        }
    }
}
