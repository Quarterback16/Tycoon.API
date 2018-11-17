using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{

    /// <summary>
    /// Grid Sorting view model.
    /// </summary>
    [Serializable]
    public class GridSortingViewModel
    {
        /// <summary>
        /// Sorting ID.
        /// </summary>
        [Key]
        [Bindable]
        [DescriptionKey]
        [Display(Name = "Sorting ID", Order = 1)]
        public long? SortingID { get; set; }


        /// <summary>
        /// Address property.
        /// </summary>
        [Bindable]
        [Display(Name = "Address", Order = 2)]
        public string Address { get; set; }

        /// <summary>
        /// Date Value.
        /// </summary>
        [Bindable]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Time Value.
        /// </summary>
        [Bindable]
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        [Bindable]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Duration Property.
        /// </summary>
        [Bindable]
        [DataType(DataType.Duration)]
        public int? Duration { get; set; }

        /// <summary>
        /// Multi-line Text.
        /// </summary>
        [Bindable]
        [DataType(DataType.MultilineText)]
        public string LargeTextArea { get; set; }

        /// <summary>
        /// Password Value.
        /// </summary>
        [Bindable]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Image URL.
        /// </summary>
        [Bindable]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }


        /// <summary>
        /// Html Content.
        /// </summary>
        [Bindable]
        [DataType(DataType.Html)]
        public string HtmlContent { get; set; }

    }
}