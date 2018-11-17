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
    public class EditableTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public DataType DataType
        {
            get
            {
                return this.dataType;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected DataType dataType = DataType.Custom;

        /// <summary>
        /// 
        /// </summary>
        public bool AllowEdit
        {
            get
            {
                return allowEdit;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected bool allowEdit = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowEdit"></param>
        /// <param name="dataType"></param>
        public EditableTypeAttribute(bool allowEdit, DataType dataType)
        {
            this.allowEdit = allowEdit;
            this.dataType = dataType;

        }

    }
}
