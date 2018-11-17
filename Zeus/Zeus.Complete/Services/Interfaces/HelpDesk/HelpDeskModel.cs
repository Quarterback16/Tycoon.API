using System;

namespace Employment.Web.Mvc.Service.Interfaces.HelpDesk
{
    /// <summary>
    /// Help desk model class
    /// </summary>
    public class HelpDeskModel
    {
        /// <summary>
        /// Application
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Organisation
        /// </summary>
        public string Organisation { get; set; }
        /// <summary>
        /// Site
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }
}