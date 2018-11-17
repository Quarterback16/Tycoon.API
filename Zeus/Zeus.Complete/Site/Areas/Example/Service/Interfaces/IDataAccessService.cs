namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{
    /// <summary>
    /// A service to demonstrate using ICaGenService
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public interface IDataAccessService
    {

        /// <summary>
        /// Gets data from sql server.
        /// </summary>
        /// <returns></returns>
        DataAccessSqlServerModel GetSomeSqlServerData();

    }

}