using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    [Serializable]
    public class PropertyViewModel
    {

        /// <summary>
        /// Adw property type code.
        /// </summary>
        [Bindable]
        [Key]
        public string PropertyType { get; set; }

        /// <summary>
        /// Adw code type.
        /// </summary>
        [Bindable]
        public string CodeType { get; set; }

        /// <summary>
        /// Adw code.
        /// </summary>
        [Bindable]
        public string Code { get; set; }

        /// <summary>
        /// Adw value.
        /// </summary>
        [Bindable]
        public string Value { get; set; }

        /// <summary>
        /// Currency start date time.
        /// </summary>
        [Bindable]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Currency end date time.
        /// </summary>
        [Bindable]
        public DateTime? EndDate { get; set; }

    }
}