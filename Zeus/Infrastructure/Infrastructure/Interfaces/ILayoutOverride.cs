using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Layout Override.
    /// </summary>
    public interface ILayoutOverride
    {
        /// <summary>
        /// Which layout types should be hidden.
        /// </summary>
        IEnumerable<LayoutType> Hidden { get; set; }
    }
}
