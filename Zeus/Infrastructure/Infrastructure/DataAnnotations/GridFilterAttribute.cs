using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    public class GridFilterAttribute : Attribute
    {
        private bool showFilterFields = false;
        private List<string> filterFieldNames;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> FilterFieldNames
        {
            get { return this.filterFieldNames; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowFilterFields
        {
            get { return this.showFilterFields; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showFilterFields"></param>
        /// <param name="filterFieldNames"></param>
        public GridFilterAttribute(bool showFilterFields, params string[] filterFieldNames)
        {
            this.showFilterFields = showFilterFields;
            this.filterFieldNames.AddRange(filterFieldNames);

        }

    }
}
