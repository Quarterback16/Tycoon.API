namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                {
                    return string.Format("{0}, {1}", LastName, FirstName);
                }
                
                if (!string.IsNullOrEmpty(FirstName))
                {
                    return FirstName;
                }

                if (!string.IsNullOrEmpty(LastName))
                {
                    return LastName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Organisation code.
        /// </summary>
        public string OrganisationCode { get; set; }

        /// <summary>
        /// Site code.
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public string Role { get; set; }
    }
}
