namespace Employment.Web.Mvc.Infrastructure.Types.Geospatial
{
    /// <summary>
    /// 
    /// </summary>
    public enum AddressReliability
    {
        /// <summary>
        /// The accuracy of the Address is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// There is very high confidence in the accuracy of the Address.
        /// </summary>
        VeryHigh,
        /// <summary>
        /// There is high confidence in the accuracy of the Address.
        /// </summary>
        High,
        /// <summary>
        /// There is medium confidence in the accuracy of the Address.
        /// </summary>
        Medium,
        /// <summary>
        /// There is low confidence in the accuracy of the Address.
        /// </summary>
        Low,
        /// <summary>
        /// There is no confidence in the accuracy of the Address.
        /// </summary>
        None
    }
}
