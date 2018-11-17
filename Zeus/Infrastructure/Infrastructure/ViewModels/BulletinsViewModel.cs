using System.ComponentModel;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines a bulletins View Model.
    /// </summary>
    [DisplayName("Bulletins")]
    public class BulletinsViewModel
    {
        /// <summary>
        /// List of bulletins.
        /// </summary>
        [Paged("BulletinsNextPage")]
        public IPageable<BulletinViewModel> Bulletins { get; set; }
    }
}