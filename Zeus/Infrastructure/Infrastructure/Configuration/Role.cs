using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines configuration element for Role used by <see cref="RolesCollection"/>.
    /// </summary>
    public class Role : ConfigurationElement
    {

        /// <summary>
        /// Name of role.
        /// </summary>
        [ConfigurationProperty("role", IsKey = true, IsRequired = true)]
        public string Name {
            get
            {
                return this["role"] as string;
            }
            set
            {
                this["role"] = value;
            }
        }
    }



    /// <summary>
    /// Defines configuration element for User used by <see cref="UsersCollection"/>.
    /// </summary>
    public class User : ConfigurationElement
    {

        /// <summary>
        /// Name of user.
        /// </summary>
        [ConfigurationProperty("user", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return this["user"] as string;
            }
            set
            {
                this["user"] = value;
            }
        }
    }


    /// <summary>
    /// Defines configuration element for Contract used by <see cref="ContractsCollection"/>.
    /// </summary>
    public class Contract : ConfigurationElement
    {

        /// <summary>
        /// Name of Contract.
        /// </summary>
        [ConfigurationProperty("contract", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return this["contract"] as string;
            }
            set
            {
                this["contract"] = value;
            }
        }
    }


    /// <summary>
    /// Defines configuration element for Contract used by <see cref="OrgCodesCollection"/>.
    /// </summary>
    public class OrgCode : ConfigurationElement
    {

        /// <summary>
        /// Name of OrgCode.
        /// </summary>
        [ConfigurationProperty("code", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return this["code"] as string;
            }
            set
            {
                this["code"] = value;
            }
        }
    }

     
}
