using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{

    /// <summary>
    /// Defines security sub-section.
    /// </summary> 
    public class SecuritySection : ConfigurationSection
    {

        /// <summary>
        /// Defines Name property for this section.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired= true)]
        public string Name {
            get 
            {
                return this["name"] as string;
            }
            set 
            {
                this["name"] = value;
            }
        }



        /// <summary>
        /// Defines 'allowInProduction' property for this section.
        /// </summary>
        [ConfigurationProperty("allowInProduction", IsRequired = false, DefaultValue = "false")]
        public string AllowInProduction
        {
            get
            {
                return this["allowInProduction"] as string;
            }
            set
            {
                this["allowInProduction"] = value;
            }
        }


        /// <summary>
        /// Defines Roles section.
        /// </summary>
        [ConfigurationProperty("roles", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RolesCollection), AddItemName = "add")]
        public RolesCollection Roles {
            get
            {
                return (RolesCollection)this["roles"];
            }
            set
            {
                this["roles"] = value;
            }
        }

        /// <summary>
        /// Defines Users section.
        /// </summary>
        [ConfigurationProperty("users", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UsersCollection), AddItemName = "add")]
        public UsersCollection Users
        {
            get
            {
                return (UsersCollection)this["users"];
            }
            set
            {
                this["users"] = value;
            }
        }


        /// <summary>
        /// Defines OrgCodes section.
        /// </summary>
        [ConfigurationProperty("organisationCodes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(OrgCodesCollection), AddItemName = "add")]
        public OrgCodesCollection OrgCodes
        {
            get
            {
                return (OrgCodesCollection)this["organisationCodes"];
            }
            set
            {
                this["organisationCodes"] = value;
            }
        }

        /// <summary>
        /// Defines Contracts section.
        /// </summary>
        [ConfigurationProperty("contracts", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ContractsCollection), AddItemName = "add")]
        public ContractsCollection Contracts
        {
            get
            {
                return (ContractsCollection)this["contracts"];
            }
            set
            {
                this["contracts"] = value;
            }
        }

    }
}
