using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Wrappers
{
    /// <summary>
    /// Represents a wrapper for the <see cref="System.Configuration.ConfigurationManager" /> to make it Unit Testable.
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        private static readonly IConfigurationManager instance = new ConfigurationManagerWrapper();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The current.</value>
        public static IConfigurationManager Current
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Name value collection of Application settings.
        /// </summary>
        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// Connection strings.
        /// </summary>
        /// <param name="name">Unique name of the connection string.</param>
        /// <returns>The connection string.</returns>
        public string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Get a section from the Web Config.
        /// </summary>
        /// <typeparam name="T">Type of section.</typeparam>
        /// <param name="sectionName">Name of section.</param>
        /// <returns>The section as type.</returns>
        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// Open configuration.
        /// </summary>
        /// <returns>Configuration.</returns>
        public System.Configuration.Configuration OpenConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        }

        /// <summary>
        /// Refresh section.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        public void RefreshSection(string sectionName)
        {
            ConfigurationManager.RefreshSection(sectionName);
        }
    }
}
