using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Zeus.Types
{
    public class Constants
    {
    }

    public class GroupRow
    {
        public string GroupName { get; set; }

        public GroupRowType GroupRowType { get; set; }

        public bool RenderedEntiredRow { get; set; }
    }
}