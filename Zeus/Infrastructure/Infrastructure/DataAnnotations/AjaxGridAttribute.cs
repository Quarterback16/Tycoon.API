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
    public class AjaxGridAttribute : Attribute
    {
        private string area = string.Empty;
        private string actionName = string.Empty;
        private string controllerName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string AreaName
        {
            get { return this.area; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ActionName
        {
            get { return this.actionName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ControllerName
        {
            get { return this.controllerName; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        public AjaxGridAttribute(string actionName, string controllerName)
        {
            this.actionName = actionName;
            this.controllerName = controllerName;

        }

        public AjaxGridAttribute(string area, string actionName, string controllerName)
        {
            this.area = area;
            this.actionName = actionName;
            this.controllerName = controllerName;

        }




    }
}
