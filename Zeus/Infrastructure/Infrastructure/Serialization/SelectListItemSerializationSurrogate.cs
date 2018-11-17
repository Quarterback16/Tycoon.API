using System;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Serialization
{
    /// <summary>
    /// Implements a serialization surrogate selector that allows <see cref="SelectListItem" /> to be serialized and deserialized, even though it is not decorated with <see cref="SerializableAttribute" />.
    /// </summary>
    public sealed class SelectListItemSerializationSurrogate : ISerializationSurrogate
    {
        /// <summary>
        /// Method called to serialize a <see cref="SelectListItem" /> object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var selectListItem = (SelectListItem)obj;

            info.AddValue("Selected", selectListItem.Selected);
            info.AddValue("Text", selectListItem.Text);
            info.AddValue("Value", selectListItem.Value);
        }

        /// <summary>
        /// Method called to deserialize a <see cref="SelectListItem" /> object.
        /// </summary>
        /// <param name="obj">The object to populate. </param>
        /// <param name="info">The information to populate the object.</param>
        /// <param name="context">The source from which the object is deserialized.</param>
        /// <param name="selector">The surrogate selector where the search for a compatible surrogate begins.</param>
        /// <returns>The populated deserialized object.</returns>
        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var selectListItem = (SelectListItem)obj;

            selectListItem.Selected = info.GetBoolean("Selected");
            selectListItem.Text = info.GetString("Text");
            selectListItem.Value = info.GetString("Value");

            // Formatters ignore this return value
            return null;
        }
    }
}
