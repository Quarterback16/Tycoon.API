using System;

namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{
    /// <summary>
    /// Example model class.
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public class DataAccessSqlServerModel
    {
        public string BulletinTitle { get; set; }

        public int BulletinId { get; set; }

        public DateTime BulletinLiveDate { get; set; }
    }
}