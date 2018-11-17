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
    public class ReferenceListAttribute : Attribute
    {
        private string refPropertyName = string.Empty;
        private string valuePropertyName = string.Empty;
        private string displayPropertyName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string RefPropertyName
        {
            get { return this.refPropertyName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ValuePropertyName
        {
            get { return this.valuePropertyName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayPropertyName
        {
            get { return this.displayPropertyName; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPropertyName"></param>
        /// <param name="valuePropertyName"></param>
        /// <param name="displayPropertyName"></param>
        public ReferenceListAttribute(string refPropertyName, string valuePropertyName, string displayPropertyName)
        {
            this.refPropertyName = refPropertyName;
            this.valuePropertyName = valuePropertyName;
            this.displayPropertyName = displayPropertyName;

        }




    }
}
