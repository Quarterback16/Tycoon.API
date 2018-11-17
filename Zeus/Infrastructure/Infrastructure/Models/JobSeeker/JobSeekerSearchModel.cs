using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Models.JobSeeker
{
    /// <summary>
    /// Model representing Job Seeker search.
    /// </summary>
    public class JobSeekerSearchModel
    {
        // Jobseeker specific attributes for search.

        /// <summary>
        /// JobSeeker ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Salutation.
        /// </summary>
        public string Salutation { get; set; }

        /// <summary>
        /// First Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Middle Name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public string DOB { get; set; }

        /// <summary>
        /// Prefered Name.
        /// </summary>
        public string PreferedName { get; set; }

        /// <summary>
        /// Email Address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public string Status { get; set; }
    }
}
