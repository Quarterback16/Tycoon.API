using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    /// <summary>
    /// Noticeboard appointment model class.
    /// </summary>
    public class NoticeboardAppointmentModel
    {
        /// <summary>
        /// Appointment ID
        /// </summary>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Jobseeker ID
        /// </summary>
        public long JobseekerID { get; set; }

        /// <summary>
        /// Jobseeker first name
        /// </summary>
        public string JobseekerFirstName { get; set; }

        /// <summary>
        /// Jobseeker last name
        /// </summary>
        public string JobseekerLastName { get; set; }

        /// <summary>
        /// Appointment start date
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Appointment end date
        /// </summary>
        public DateTime EndDateTime { get; set; }
        
        /// <summary>
        /// Breach
        /// </summary>
        public string Breach { get; set; }

        /// <summary>
        /// Consultant ID
        /// </summary>
        public string ConsultantID { get; set; }

        /// <summary>
        /// Consultant name
        /// </summary>
        public string ConsultantText { get; set; }

        /// <summary>
        /// Appointment creation date
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Appointment creating site
        /// </summary>
        public string CreationSiteCode { get; set; }

        /// <summary>
        /// Eligibility code for jobseeker
        /// </summary>
        public string EligibilityCode { get; set; }

        /// <summary>
        /// Interpreter code for jobseeker
        /// </summary>
        public string InterpreterCode { get; set; }

        /// <summary>
        /// Appointment location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Appointment Reason Code
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// Appointment Result Code
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// Appointment Result Reason code
        /// </summary>
        public string ResultReason { get; set; }

        /// <summary>
        /// Session Type Code
        /// </summary>
        public string SessionTypeCode { get; set; }

        /// <summary>
        /// Special needs indicator of jobseeker
        /// </summary>
        public bool SpecialNeedsIndicator { get; set; }

        /// <summary>
        /// Special requirements of jobseeker
        /// </summary>
        public string SpecialRequirements { get; set; }
    }
}
