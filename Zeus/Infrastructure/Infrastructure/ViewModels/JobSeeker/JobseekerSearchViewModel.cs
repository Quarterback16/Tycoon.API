using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker
{
    /// <summary>
    /// The View Model for <see cref="JobseekerSearchModel"/>.
    /// </summary>
    [Serializable]
    [Group(GroupNames.JobSeekerSearch, Order = 1, GroupType = GroupType.FieldSet)]
    [Group(GroupNames.JobSeekerAdvancedSearch, ActionForDependencyType.Visible, "IsAdvancedSearch", ComparisonType.EqualTo, true, Order = 2)]

    [Button("Search", GroupName = GroupNames.JobSeekerAdvancedSearch)]

    public class JobseekerSearchViewModel
    {
        internal const string AjaxProperty = "SingleLineJobseekerSearch";
        internal const string AjaxPropertyModelMetadataKey = "IsJobseekerSearchViewModelAjaxProperty";
        internal const string SingleLineSearchValidationError = "Please enter the {0}";



        /// <summary>
        /// Gets or Sets whether <see cref="JobseekerSearchViewModel"/> is read-only.
        /// </summary>
        /// <value>
        /// <c>True</c> if read-only otherwise <c>false</c>.
        /// </value>
        [Bindable]
        [Hidden]
        public bool ReadOnly { get; set; }




        /// <summary>
        /// Gets or Sets whether <see cref="JobseekerSearchViewModel"/> is required.
        /// </summary>
        /// <value>
        /// <c>True</c> if required otherwise <c>false</c>.
        /// </value>
        [Bindable]
        [Hidden]
        public bool Required { get; set; }

        /// <summary>
        ///  Gets or sets the full jobseeker record as single line.
        /// </summary>
        [Bindable]
        [Display(GroupName = GroupNames.JobSeekerSearch, Order = 1, Name = "Enter Job Seeker")]
        [Row("1", Types.RowType.Default)]
        [RequiredIfTrue("Required", ErrorMessage = SingleLineSearchValidationError)]
        [ReadOnlyIfTrue("ReadOnly")]

        public string SingleLineJobseekerSearch 
        {
            get
            { 

                if (string.IsNullOrEmpty(FirstName))
                {
                    return string.Format("{0}", Id);
                }

                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", Salutation, FirstName, MiddleName, LastName, Id, Gender, DOB, Type);

            }
            set
            {

            }
        }


        [Display(Name = "Advance Search", GroupName = GroupNames.JobSeekerSearch, Order = 2)]
        [Row("1", Types.RowType.Default)]
        [Bindable]

        public bool IsAdvancedSearch { get; set; }

        // Advanced Fields Jobseeker specific attributes for search.

        /// <summary>
        /// JobSeeker ID.
        /// </summary>
        [Bindable]       
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 1, Name = "Job Seeker ID")]
        [Row("2", Types.RowType.Half)]
        [Id]
        public int? Id { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        [Bindable]
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 2, Name = "DOB")]
        [Row("2", Types.RowType.Half)]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

                

        /// <summary>
        /// Salutation.
        /// </summary>
        [Bindable]
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 3, Name = "Salutation")]
        [AdwSelection(Types.SelectionType.Single, Types.AdwType.ListCode, "EPL")]
        [Row("3", Types.RowType.Half)]
        public string Salutation { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        [Bindable]
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 4, Name = "Type")]
        [Row("3", Types.RowType.Half)]
        [Selection(Types.SelectionType.Single, new string[] {"C" }, new string[] {"C" })]
        public string Type { get; set; }



        /// <summary>
        /// First Name.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 5, Name = "First Name")]
        [Row("4", Types.RowType.Half)]
        public string FirstName { get; set; }


        /// <summary>
        /// Last Name.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 6, Name = "Last Name")]
        [Row("4", Types.RowType.Half)]
        public string LastName { get; set; }

        
        /// <summary>
        /// Middle Name.
        /// </summary>
        [Bindable]

        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 7, Name = "Middle Name")]
        [Row("5", Types.RowType.Half)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Middle Name.
        /// </summary>
        [Bindable]

        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 8, Name = "Prefered Name")]
        [Row("5", Types.RowType.Half)]
        public string PreferedName { get; set; }


        /// <summary>
        /// Gender.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 9, Name = "Gender")]
        [Row("6", Types.RowType.Half)]
        [Selection(Types.SelectionType.Single, new string[] { "F", "M" }, new string[] { "Male", "Female"})]
        public string Gender { get; set; }


        /// <summary>
        /// Status.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 10, Name = "Status")]
        [Row("6", Types.RowType.Half)]
        [Selection(Types.SelectionType.Single, new string[] { "A", "I", "0" }, new string[] { "Active", "Inactive", "Both" })]
        public string Status { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 11, Name = "Email")]
        [Row("7", Types.RowType.Half)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        /// <summary>
        /// Phone.
        /// </summary>
        [Bindable]
        
        [Display(GroupName = GroupNames.JobSeekerAdvancedSearch, Order = 12, Name = "Phone")]
        [Row("7", Types.RowType.Half)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
