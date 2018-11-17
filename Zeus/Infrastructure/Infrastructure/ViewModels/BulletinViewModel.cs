using System;
using System.ComponentModel;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines a bulletin View Model.
    /// </summary>
    [DisplayName("Bulletin")]
    public class BulletinViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string HtmlExtended { get; set; }

        /// <summary>
        /// Gets or sets the live date.
        /// </summary>
        /// <value>
        /// The live date.
        /// </value>
        public DateTime LiveDate { get; set; }

        ///// <summary>
        ///// Gets or sets the expires date.
        ///// </summary>
        ///// <value>
        ///// The expires date.
        ///// </value>
        //public DateTime ExpiresDate { get; set; }

        /// <summary>
        /// Gets or sets the bulletin contracts.
        /// </summary>
        /// <value>
        /// The bulletin contracts.
        /// </value>
        public string[] BulletinContracts { get; set; }
    }
}