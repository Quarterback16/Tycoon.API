using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration section for history.
    /// </summary>
    public class HistorySection : ConfigurationSection
    {
        /// <summary>
        /// Page size of history.
        /// </summary>
        [ConfigurationProperty("pageSize", IsRequired = true, DefaultValue = 10)]
        public int PageSize
        {
            get
            {
                return (int)this["pageSize"];
            }
            set
            {
                this["pageSize"] = value;
            }
        }

    }
}
