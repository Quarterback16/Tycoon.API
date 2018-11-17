using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Adw code model.
    /// </summary>
    public class CodeModel
    {
        /// <summary>
        /// Adw code
        /// </summary>
        [Alias("code")]
        public string Code { get; set; }

        /// <summary>
        /// Long description
        /// </summary>
        [Alias("long_desc")]
        public string Description { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        [Alias("short_desc")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Currency Start date time
        /// </summary>
        [Alias("currency_strt_dt")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Currency end date time
        /// </summary>
        [Alias("currency_end_dt")]
        public DateTime? EndDate { get; set; }

        [Alias("StartDateInt")]
        public long? CurrencyStart { get; set; }

        [Alias("EndDateInt")]
        public long? CurrencyEnd { get; set; }

        /// <summary>
        /// Returns text for the relevant <see cref="AdwDisplayType"/>.
        /// </summary>
        /// <returns>Text for the current instance using the specified <paramref name="displayType"/>.</returns>
        public string GetDisplayText(AdwDisplayType displayType)
        {
            string text = Description;

            switch (displayType)
            {
                case AdwDisplayType.Description: text = Description; break;
                case AdwDisplayType.ShortDescription: text = ShortDescription; break;
                case AdwDisplayType.Code: text = Code; break;
                case AdwDisplayType.CodeAndDescription: text = string.Format("{0} ({1})", Description, Code); break;
                case AdwDisplayType.CodeAndShortDescription: text = string.Format("{0} ({1})", ShortDescription, Code); break;
            }

            return text;
        }
    }
}
