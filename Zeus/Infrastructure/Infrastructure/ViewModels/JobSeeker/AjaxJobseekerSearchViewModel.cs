
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker
{
    /// <summary>
    /// Represents Ajax view Model for <see cref="JobseekerSearchViewModel"/>.
    /// </summary>
    public class AjaxJobseekerSearchViewModel
    { 
        /// <summary>
        /// Gets or sets the full jobseeker record as single line.
        /// </summary>
        public string SingleLineJobseekerSearch
        {
            get;
            set;
        }


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

    }
}
