using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for the Infrastructure <see cref="Employment.Web.Mvc.Infrastructure.Types.MessageType" /> enum
    /// </summary>
    public static class MessageTypeExtension
    {
        public static string GetLabelClasses(this MessageType messageType)
        {
            string result = null;
            switch (messageType)
            {
                case MessageType.Error:
                    result = "label label-danger";
                    break;
                case MessageType.Information:
                    result = "label label-info";
                    break;
                case MessageType.Success:
                    result = "label label-success";
                    break;
                case MessageType.Warning:
                    result = "label label-warning";
                    break;
                default:
                    result = "label label-default";
                    break;
            }
            return result;
        }
    }
}
