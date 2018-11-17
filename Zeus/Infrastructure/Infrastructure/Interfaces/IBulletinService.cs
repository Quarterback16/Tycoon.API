using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods and properties that are required for a bulletins service.
    /// </summary>
    public interface IBulletinService
    {
        /// <summary>
        /// Get bulletin details for specified ID.
        /// </summary>
        /// <param name="bulletinID">The bulletin ID.</param>
        /// <returns>The bulletin details.</returns>
        BulletinModel Get(int bulletinID);

        /// <summary>
        /// Get all bulletins of the specified type.
        /// </summary>
        /// <param name="bulletinType">The bulletin type.</param>
        /// <param name="limit">The maximum number of bulletins to get.</param>
        /// <returns>Bulletins of the specified type.</returns>
        IList<BulletinModel> List(BulletinType bulletinType, int limit);
    }
}