using System;

namespace RosterLib
{
	public class SimplePreReport
	{
		public string Season { get; set; }
		public string Folder { get; set; }
		public string FileOut { get; set; }
		public string Body { get; set; }
		public string ReportHeader { get; set; }
		public string InstanceName { get; set; }
		public string ReportType { get; set; }

		public string RenderHtml()
		{
			FileOut = string.Format("{0}{1}//{2}//{3}.htm", Utility.OutputDirectory(), Season, Folder, InstanceName);
			if (!string.IsNullOrEmpty(InstanceName))
				ReportHeader = $"{InstanceName}";
			else
				ReportHeader = $"{ReportType}";

			var reportHeader = $@"{ReportHeader} as of {
				DateTime.Now.ToString("ddd dd MMM yy HH:MM tt")
				}";
			var h = new HtmlFile(FileOut, reportHeader);
			var html = $"<h3>{reportHeader}</h3>";
			html += $"<pre>{Body}</pre>";
			h.AddToBody(html);
			h.Render();
			return html;
		}
	}
}