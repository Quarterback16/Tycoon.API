using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Bulletin model.
    /// </summary>
    public class BulletinModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Alias("title")]
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
        [Alias("Html1")]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        [Alias("Html2")]
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
        /// 
        /// </summary>
        public string ExtensionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExtensionData { get; set; }

        /// <summary>
        /// Gets or sets the bulletin contracts.
        /// </summary>
        /// <value>
        /// The bulletin contracts.
        /// </value>
        public string[] BulletinContracts { get; set; }
    }
}