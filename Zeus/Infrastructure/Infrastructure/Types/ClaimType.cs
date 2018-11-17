namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Defines the available claim types.
    /// </summary>
    public class ClaimType
    {
        /// <summary>
        /// Contracts claim.
        /// </summary>
        public static readonly string Contracts = "http://deewr.gov.au/es/2011/03/claims/orgcontract";

        /// <summary>
        /// Self registered claim.
        /// </summary>
        public static readonly string IsSelfRegistered = "http://deewr.gov.au/es/2011/03/claims/selfregistered";

        /// <summary>
        /// Name claim.
        /// </summary>
        public static readonly string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// Organisation code claim.
        /// </summary>
        public static readonly string Organisation = "http://deewr.gov.au/es/2011/03/claims/org";

        /// <summary>
        /// Nation roles claim.
        /// </summary>
        public static readonly string RolesNation = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// Base roles claim.
        /// </summary>
        public static readonly string RolesBase = "http://deewr.gov.au/es/2011/03/claims/baserole";

        /// <summary>
        /// General roles claim.
        /// </summary>
        public static readonly string RolesGeneral = "http://deewr.gov.au/es/2011/03/claims/generalrole";

        /// <summary>
        /// Reporting roles claim.
        /// </summary>
        public static readonly string RolesReporting = "http://deewr.gov.au/es/2011/03/claims/reportingrole";

        /// <summary>
        /// Site code claim.
        /// </summary>
        public static readonly string Site = "http://deewr.gov.au/es/2011/03/claims/defaultsite";

        /// <summary>
        /// User type claim.
        /// </summary>
        public static readonly string UserType = "http://deewr.gov.au/es/2011/03/claims/usertype";

        /// <summary>
        /// Site codes claim.
        /// </summary>
        public static readonly string Sites = "http://deewr.gov.au/es/2011/03/claims/orgsite";

        /// <summary>
        /// First name claim.
        /// </summary>
        public static readonly string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        /// <summary>
        /// Last name claim.
        /// </summary>
        public static readonly string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        /// <summary>
        /// Email claim.
        /// </summary>
        public static readonly string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        /// <summary>
        /// Last logon claim.
        /// </summary>
        public static readonly string LastLogon = "http://deewr.gov.au/es/2011/03/claims/lastLogonDateTimeStamp";

        /// <summary>
        /// Days to password expiry claim.
        /// </summary>
        public static readonly string DaysToPwdExpiry = "http://deewr.gov.au/ws/2011/03/identity/claims/daystopwdexpiry";

        /// <summary>
        /// Display name.
        /// </summary>
        public static readonly string DisplayName = "http://deewr.gov.au/ws/2011/03/identity/claims/displayname";

        /// <summary>
        /// Authentication method
        /// </summary>
        public static readonly string AuthenticationMethod =
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";
    }
}
