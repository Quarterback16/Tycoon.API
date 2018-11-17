using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;
using Employment.Web.Mvc.Zeus.ViewModels;

namespace Employment.Web.Mvc.Zeus.Mappers
{
    /// <summary>
    /// Represents a mapper that is used to map between the Noticeboard Domain Models and View Models.
    /// </summary>
    public static class NoticeboardMapper 
    {

        /// <summary>
        /// Convert to the bulletin view model list.
        /// </summary>
        /// <param name="srcList">The source list.</param>
        /// <param name="destList">The dest list.</param>
        /// <returns></returns>
        public static Pageable<BulletinViewModel> ToBulletinViewModelList(this IEnumerable<BulletinModel> srcList, Pageable<BulletinViewModel> destList) {
            if (srcList != null) {
                foreach (var src in srcList) {
                    destList.Add(src.ToBulletinViewModel());
                }
            }
            return destList;
        }

        /// <summary>
        /// Convert to the message view model list.
        /// </summary>
        /// <param name="srcList">The source list.</param>
        /// <returns></returns>
        public static IEnumerable<MessageViewModel> ToMessageViewModelList(this IEnumerable<MiniNoticeboardModel> srcList) {
            var destList = new List<MessageViewModel>();
            if (srcList != null) {
                foreach (var src in srcList) {
                    destList.Add(src.ToMessageViewModel());
                }
            }
            return destList as IEnumerable<MessageViewModel>;
        }
        /// <summary>
        /// Map between Noticeboard Domain Models and View Models.
        /// </summary>
        public static MessageViewModel ToMessageViewModel(this MiniNoticeboardModel src)
        {
            var dest = new MessageViewModel();
            dest.Count = src.MessageCounts;
            dest.Type = src.MessageGroupLevel1Code;
            return dest;
        }
    }
}