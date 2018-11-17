namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the registration type of a type mapping.
    /// </summary>
    public enum RegistrationType
    {
        /// <summary>
        /// The type mapping will be registered normally and will be created by the container provider.
        /// </summary>
        Register,

        /// <summary>
        /// The type mapping will be registered as a singleton instance and will be created by the specified type converter.
        /// </summary>
        Instance,

        /// <summary>
        /// The type will be added as an extension to the container provider.
        /// </summary>
        Extension,

        /// <summary>
        /// The default type is Registration.
        /// </summary>
        Default = Register
    }
}
