using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Mapper.
    /// </summary>
    public interface IQuickfind
    {
        /// <summary>
        /// Quickfind action.
        /// </summary>
        /// <param name="criteria">Quickfind criteria.</param>
        /// <returns>A redirect to an action.</returns>
        [HttpPost]
        RedirectToRouteResult Quickfind(QuickfindCriteria criteria);
    }

    /// <summary>
    /// Quickfind Criteria
    /// </summary>
    public class QuickfindCriteria
    {
        private string data;

        /// <summary>
        /// Quickfind Data
        /// </summary>
        [Bindable]
        public string QuickfindData { get { return !string.IsNullOrEmpty(data) ? data.Trim() : string.Empty; } set { data = value; } }
    }
}
