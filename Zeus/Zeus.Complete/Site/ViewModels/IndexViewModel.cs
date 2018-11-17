using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using ReadOnlyAttribute = System.ComponentModel.ReadOnlyAttribute;

namespace Employment.Web.Mvc.Zeus.ViewModels
{
    [DisplayName("Home")]
    public class IndexViewModel
    {
        /// <summary>
        /// Whether to show the Noticeboard Messages.
        /// </summary>
        [Hidden(ExcludeFromView = true)]
        [ReadOnly(true)]
        public bool ShowNoticeboardMessages { get; set; }

        /// <summary>
        /// Noticeboard messages.
        /// </summary>
        public IEnumerable<MessageViewModel> NoticeboardMessages { get; set; }
    }
}