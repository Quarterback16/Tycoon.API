using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Models.AdwLookup
{
    /// <summary>
    /// Model to represent Delta Search.
    /// </summary>
    public class DeltaModel
    {
        [Alias("")]
        public string Code { get; set; }

        [Alias("")]
        public string ShortDescription { get; set; }


        [Alias("")]
        public string LongDescription { get; set; }



        [Alias("")]
        public string CurrencyStart { get; set; }


        [Alias("")]
        public string CurrencyEnd { get; set; }


        [Alias("")]
        public DateTime? CurrencyStartDate { get; set; }


        [Alias("")]
        public DateTime? CurrencyEndDate { get; set; }

    }
}
