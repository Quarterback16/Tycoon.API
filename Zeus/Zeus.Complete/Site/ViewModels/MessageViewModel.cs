

namespace Employment.Web.Mvc.Zeus.ViewModels
{
    /// <summary>
    /// Defines the view model of a noticeboard message.
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Message type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Message description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Message count.
        /// </summary>
        public int Count { get; set; }
    }
}