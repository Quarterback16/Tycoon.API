using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Report.
    /// </summary>
    /// <summary>
    /// Defines the methods and properties that are required for an <see cref="ObjectViewModel{T}" /> used in a <see cref="DynamicViewModel" />.
    /// </summary>
    public interface IObjectViewModel<T>
    {
        /// <summary>
        /// The property name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A value.
        /// </summary>
        T Value { get; set; }
    }
}
