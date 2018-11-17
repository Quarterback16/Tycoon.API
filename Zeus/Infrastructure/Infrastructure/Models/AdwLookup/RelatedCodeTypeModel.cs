using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Related Code Type Model used in AdwAdminService.
    /// </summary>
    public class RelatedCodeTypeModel
    {
        /// <summary>
        /// Relationship.
        /// </summary>
        [Alias("relation_type_cd")]
        public string Relationship { get; set; }

        /// <summary>
        /// Sub-type.
        /// </summary>
        [Alias("sub_code_type")]
        public string SubType { get; set; }

        /// <summary>
        /// Sub Description.
        /// </summary>
        [Alias("SubDesc")]
        public string SubDescription { get; set; }
        
        /// <summary>
        /// Dom-type.
        /// </summary>
        [Alias("dom_code_type")]
        public string DomType { get; set; }

        /// <summary>
        /// Dom-description.
        /// </summary>
        [Alias("DomDesc")]
        public string DomDescription { get; set; }
    }
}
