using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Dummy view model class.
    /// </summary>
    [Serializable]
    [Button("Submit")]
    public class PRGViewModel
    {
        /// <summary>
        /// Dummy ID.
        /// </summary>
        [Bindable]
        [Editable(false)]
        [VisibleIfNotEmpty]
        [Display(Name = "Dummy ID", Order = 1)]
        public long? DummyID { get; set; }

        /// <summary>
        /// Dummy name.
        /// </summary>
        [Bindable]
        [StringLength(100)]
        [Display(Name = "Name", Order = 2)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Dummy Date.
        /// </summary>
        [Bindable] 
        [Display(Name = "Date", Order = 4)]
        [DataType(DataType.Date)]
        [Required]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// Dummy EmailAddress.
        /// </summary>
        [Bindable]
        [Display(Name = "EmailAddress", Order = 7)]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }
    }
}