using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents
{
    [Widgets("Example")]
    public class DashboardViewModel : ILayoutOverride
    {
        public DashboardViewModel()
        {
            Hidden = new List<LayoutType>() { LayoutType.RequiredFieldsMessage };
        }

        public IEnumerable<LayoutType> Hidden { get; set; }

    }
}