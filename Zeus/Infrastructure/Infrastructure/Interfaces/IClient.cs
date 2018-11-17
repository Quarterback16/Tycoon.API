namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Client.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Creates a new instance of the service.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <param name="serviceName">Name of service.</param>
        /// <returns>New instance of the service.</returns>
        TService Create<TService>(string serviceName) where TService : class;
    }
}
