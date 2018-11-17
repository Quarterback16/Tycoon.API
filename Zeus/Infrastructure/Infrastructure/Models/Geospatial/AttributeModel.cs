namespace Employment.Web.Mvc.Infrastructure.Models.Geospatial
{
    /// <summary>
    /// The model class for geospatial attributes associated with an <see cref="AddressModel"/>.
    /// </summary>
    public class AttributeModel
    {
        /// <summary>
        /// The attribute name example: SA1, SA2, SA3, SA4, RJCP.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The code for the attribute.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The value for the attribute.
        /// </summary>
        public string Value { get; set; }
    }
}
