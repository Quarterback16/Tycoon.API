using System.Web;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="HttpPostedFileBase"/>.
    /// </summary>
    public static class HttpPostedFileBaseExtension
    {
        /// <summary>
        /// Get the byte array data of a http posted file.
        /// </summary>
        /// <param name="file">The http posted file.</param>
        /// <returns>The file data as a byte array.</returns>
        public static byte[] GetBytes(this HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return null;
            }

            var data = new byte[file.ContentLength];

            file.InputStream.Read(data, 0, file.ContentLength);
            file.InputStream.Position = 0;

            return data;
        }
    }
}
