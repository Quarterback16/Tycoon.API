using System;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Bulletins extension data
    /// </summary>
    [Serializable]
    public class BulletinExtensionData
    {
        /// <summary>
        /// Gets or sets the contracts.
        /// </summary>
        /// <value>
        /// The contracts.
        /// </value>
        public string[] Contracts { get; set; }
    }
}