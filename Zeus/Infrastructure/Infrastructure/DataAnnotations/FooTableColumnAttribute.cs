using System;
using System.Collections.Generic;
using System.Text; 

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Provides the ability to specify FooTable class values on a rendered table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FooTableColumnAttribute : Attribute
    {
        private List<DataHideClassType> hideTypes = new List<DataHideClassType>();

        /// <summary>
        /// Set the property as a FooTable column without any FooTable specific tag details
        /// </summary>
        public FooTableColumnAttribute () : this(DataHideClassType.Phone, DataHideClassType.Tablet)
	    {
            this.hideTypes.Add(DataHideClassType.None);
        }

        /// <summary>
        /// Set the property as a FooTable column with the specified data-hide tag details.
        /// The columns will be hidden at the pre-defined widths as per the FooTable documentation.
        /// Multiple values can be set which will cause the DataHideClassString property to return a comma delimited list of values for the data-hide html attribute.
        /// </summary>
        /// <param name="dataHideClassTypes">List of DataHideClassType settings to be assigned to the column</param>
        public FooTableColumnAttribute(params DataHideClassType[] dataHideClassTypes)
        {
            this.hideTypes.AddRange(dataHideClassTypes);

        }

        /// <summary>
        /// Return the value for the data-hide attribute
        /// </summary>
        public string DataHideClassString
        {
            get
            {
                StringBuilder ret = new StringBuilder();
                int count = 0;
                foreach (var hideType in this.hideTypes)
                {
                    count++;
                    switch (hideType)
                    {
                        case DataHideClassType.None:
                            ret.Clear();
                            break;
                        case DataHideClassType.All:
                            ret.AppendFormat("{0}all", 
                                count > 1 ? "," : string.Empty);
                            break;
                        case DataHideClassType.Phone:
                            ret.AppendFormat("{0}phone", 
                                count > 1 ? "," : string.Empty);
                            break;
                        case DataHideClassType.Tablet:
                            ret.AppendFormat("{0}tablet",
                                count > 1 ? "," : string.Empty);
                            break;
                        default:
                            ret.Clear();
                            break;
                    }
                }

                return ret.ToString();
            }
        }
    }

    /// <summary>
    /// Specifies the data-hide values for FooTable columns.
    /// </summary>
    public enum DataHideClassType
    {
        /// <summary>
        /// Sets the data-hide html attribute value to empty string
        /// </summary>
        None,

        /// <summary>
        /// Sets the data-hide html attribute value to 'all'
        /// </summary>
        All,

        /// <summary>
        /// Sets the data-hide html attribute value to 'phone'
        /// </summary>
        Phone,

        /// <summary>
        /// Sets the data-hide html attribute value to 'tablet'
        /// </summary>
        Tablet
    }


}
