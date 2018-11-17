using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Dummy view model class.
    /// </summary>
    [Serializable]
    public class DummyViewModel
    {
        /// <summary>
        /// Dummy ID.
        /// </summary>
        [Bindable]
        [Key]
        [DescriptionKey]
        [Display(Name = "Dummy ID", Order = 1)]
        [Link("DummiesAll")]
        public long? DummyID { get; set; }

        /// <summary>
        /// Dummy name.
        /// </summary>
        [Bindable]
        [StringLength(100)]
        [Display(Name = "Name", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Dummy description.
        /// </summary>
        [Bindable]
        [StringLength(1000)]
        [Display(Name = "Description", Order = 3)]
        public string Description { get; set; }


        /// <summary>
        /// Dummy Date.
        /// </summary>
        [Bindable] 
        [Display(Name = "Date", Order = 4)]
        [DataType(DataType.Date)]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// Dummy Time.
        /// </summary>
        [Bindable]
        [Display(Name = "Time", Order = 5)]
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }

        /// <summary>
        /// Dummy DateTime.
        /// </summary>
        [Bindable]
        [Display(Name = "DateTime", Order = 5)]
        [DataType(DataType.DateTime)]
        
        public System.DateTime DateTime { get; set; }


        /// <summary>
        /// Dummy Currency.
        /// </summary>
        [Bindable]
        [Display(Name = "Currency", Order = 6)]
        [DataType(DataType.Currency)]
        public double? Currency { get; set; }

        /// <summary>
        /// Dummy EmailAddress.
        /// </summary>
        [Bindable]
        [Display(Name = "EmailAddress", Order = 7)]
        [DataType(DataType.EmailAddress)] 
        public string EmailAddress { get; set; }


        /// <summary>
        /// Example Url.
        /// </summary>
        [Bindable]
        [Display(Name = "Url", Order = 8)]
        [DataType(DataType.Url)]
        [ExcludeSort]
        public string Url { get; set; }


        /// <summary>
        /// Dummy float property.
        /// </summary>
        [Bindable]
        [Display(Name = "Decimal", Order = 9)] 
        public float? Decimal1 { get; set; }


    }
}