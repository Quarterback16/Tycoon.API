using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Infrastructure.Mappers
{
    /// <summary>
    /// Represents a mapper that is used to map between the Bulletin Domain Models and the Bulletin View Models.
    /// </summary>
    public static class BulletinMapper
    {
        /// <summary>
        /// Map between the Bulletin Domain Models and the Bulletin View Models.
        /// </summary>
        public static BulletinViewModel ToBulletinViewModel(this BulletinModel src)
        {
            var dest = new BulletinViewModel();
            dest.BulletinContracts = src.BulletinContracts;
            //dest.ExpiresDate = src.ExpiresDate;
            dest.Html = src.Html;
            dest.HtmlExtended = src.HtmlExtended;
            dest.LiveDate = src.LiveDate;
            dest.PageId = src.PageId;
            dest.Title = src.Title;
            dest.Url = src.Url;
            return dest;
            // Domain Model to View Model
            //mapper.CreateMap<BulletinModel, BulletinViewModel>();
        }
    }
}
