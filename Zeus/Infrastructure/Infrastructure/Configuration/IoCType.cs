using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration element for an IoC type used by <see cref="IoCTypes" />.
    /// </summary>
    public class IoCType : ConfigurationElement
    {
        /// <summary>
        /// Container.
        /// </summary>
        [ConfigurationProperty("container", IsKey = true, IsRequired = true)]
        public string Container
        {
            get
            {
                return (string)this["container"];
            }
            set
            {
                this["container"] = value;
            }
        }

        /// <summary>
        /// Type.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }



        /// <summary>
        /// MapTo.
        /// </summary>
        [ConfigurationProperty("mapTo")]
        public string MapTo
        {
            get
            {
                return (string)this["mapTo"];
            }
            set
            {
                this["mapTo"] = value;
            }
        }
        /// <summary>
        /// MapTo.
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// MapTo.
        /// </summary>
        [ConfigurationProperty("lifetime")]
        public string Lifetime
        {
            get
            {
                return (string)this["lifetime"];
            }
            set
            {
                this["lifetime"] = value;
            }
        }

        /// <summary>
        /// RegisterAll.
        /// </summary>
        [ConfigurationProperty("registerAll")]
        public string RegisterAll
        {
            get
            {
                return (string)this["registerAll"];
            }
            set
            {
                this["registerAll"] = value;
            }
        }
    }
}
