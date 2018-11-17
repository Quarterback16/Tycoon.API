using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.Models
{
    [Serializable]
    public class JobSeekerModelList
    {
        /// <summary>
        /// JobSeeker ID.
        /// </summary>
        [Bindable]      
        [Id]
        [Key]
        public long? Id { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        [Bindable]        
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }



        /// <summary>
        /// Salutation.
        /// </summary>
        [Bindable]
        public string Salutation { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [Bindable]
        public string Type { get; set; }



        /// <summary>
        /// First Name.
        /// </summary>
        [Bindable]
        public string FirstName { get; set; }


        /// <summary>
        /// Last Name.
        /// </summary>
        [Bindable]
        public string LastName { get; set; }


        /// <summary>
        /// Middle Name.
        /// </summary>
        [Bindable]
        public string MiddleName { get; set; }

        /// <summary>
        /// Middle Name.
        /// </summary>
        [Bindable]
        public string PreferedName { get; set; }


        /// <summary>
        /// Gender.
        /// </summary>
        [Bindable]
        public string Gender { get; set; }



    }
}