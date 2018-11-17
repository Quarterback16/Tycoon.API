using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Adw Code Type Model.
    /// </summary>
    public class CodeTypeModel
    {

        /// <summary>
        /// Code Type.
        /// </summary>
        [Alias("code_type")]
        public string CodeType { get; set; }


        /// <summary>
        /// Short Description.
        /// </summary>
        [Alias("short_desc")]
        public string ShortDescription { get; set; }


        /// <summary>
        /// Long Description.
        /// </summary>
        [Alias("long_desc")]
        public string LongDescription { get; set; }
    }
}
