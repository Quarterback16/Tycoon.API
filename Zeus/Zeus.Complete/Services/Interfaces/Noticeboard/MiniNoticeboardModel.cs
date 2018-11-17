using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    public class MiniNoticeboardModel
    {
        public string Description { get; set; }

        public int MessageCounts { get; set; }

        public string MessageGroupLevel1Code { get; set; }
    }
}
