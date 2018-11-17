using System;

namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{
    /// <summary>
    /// Example model class.
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public class DataAccessCaGenModel
    {
        public String SiteName { get; set; }

        public String SiteAddress { get; set; }

        public String LocationName { get; set; }

        public String AddressLine1 { get; set; }

        public String AddressLine2 { get; set; }

        public String AddressLine3 { get; set; }

        public String LocalityName { get; set; }

        public String State { get; set; }

        public String PostCode { get; set; }

        public string Contracts { get; set; }
    }
}