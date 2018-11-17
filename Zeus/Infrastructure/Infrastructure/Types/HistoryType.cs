using System.ComponentModel;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the history type.
    /// </summary>
    public enum HistoryType
    {
        /// <summary>
        /// Activity history.
        /// </summary>
        [Description("activity")]
        Activity,

        /// <summary>
        /// Contract history.
        /// </summary>
        [Description("contract")]
        Contract,

        /// <summary>
        /// Employer history.
        /// </summary>
        [Description("employer")]
        Employer,

        /// <summary>
        /// Job seeker history.
        /// </summary>
        [Description("job seeker")]
        JobSeeker,

        /// <summary>
        /// Vacancy history.
        /// </summary>
        [Description("vacancy")]
        Vacancy
    }
}
