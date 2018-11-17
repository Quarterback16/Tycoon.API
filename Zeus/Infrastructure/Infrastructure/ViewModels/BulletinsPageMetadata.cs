using System;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Data used for retrieving next page of <see cref="BulletinsViewModel.Bulletins" />.
    /// </summary>
    [Serializable]
    public class BulletinsPageMetadata : PageMetadata
    {
        /// <summary>
        /// The contract type of the bulletins.
        /// </summary>
        public BulletinType Contract { get; set; }
    }
}
