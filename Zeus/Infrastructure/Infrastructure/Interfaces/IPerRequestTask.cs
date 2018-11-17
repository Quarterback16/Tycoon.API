namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Per Request Task.
    /// </summary>
    public interface IPerRequestTask
    {
        /// <summary>
        /// Start per request task.
        /// </summary>
        void Start();

        /// <summary>
        /// End per request task.
        /// </summary>
        void End();
    }
}
