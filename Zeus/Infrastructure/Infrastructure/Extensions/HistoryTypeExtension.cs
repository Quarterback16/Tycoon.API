using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    public static class HistoryTypeExtension
    {
        public static string GetIconClass(this HistoryType type)
        {
            string result = null;
            switch (type)
            {
                case HistoryType.Activity:
                    result = "fa-trophy";
                    break;
                case HistoryType.Contract:
                    result = "fa-calendar";
                    break;
                case HistoryType.Employer:
                    result = "fa-users";
                    break;
                case HistoryType.JobSeeker:
                    result = "fa-user";
                    break;
                case HistoryType.Vacancy:
                    result = "fa-flag";
                    break;
                default:
                    result = "fa-history";
                    break;
            }
            return result;
        }
    }
}
