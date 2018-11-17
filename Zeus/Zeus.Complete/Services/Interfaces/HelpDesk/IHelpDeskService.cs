namespace Employment.Web.Mvc.Service.Interfaces.HelpDesk
{
    /// <summary>
    /// Help Desk Service Interface
    /// </summary>
    public interface IHelpDeskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int Id of the created request</returns>
        int Create(HelpDeskModel model);
    }
}
