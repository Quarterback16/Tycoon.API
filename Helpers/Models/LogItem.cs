using System;
using System.Xml;

namespace Helpers.Models
{
    public class LogItem
    {
        public string LogDir { get; set; }
        public string Filespec { get; set; }
        public DateTime MailDate { get; set; }

        public string Recipients { get; set; }

        public string Subject { get; set; }

        public LogItem(XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                switch (n.Name)
                {
                    case "log-dir":
                        LogDir = n.InnerText;
                        break;

                    case "filespec":
                        Filespec = n.InnerText;
                        break;

                    case "datemailed":
                        MailDate = DateTime.Parse(n.InnerText);
                        break;

                    case "recipients":
                        Recipients = n.InnerText;
                        break;

                    case "subject":
                        Subject = n.InnerText;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}