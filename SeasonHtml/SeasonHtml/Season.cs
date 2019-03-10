using System;
using System.Text;

namespace SeasonHtml
{
	public class Season
	{
		public int Year { get; set; }
		public HtmlFile HtmlFile { get; set; }

		public Season( int year )
		{
			Year = year;
		}

		public void Render()
		{
			//  output the file
			HtmlFile = new HtmlFile(
				FileName(),
				$"{Year} Season reports");
			HtmlFile.AddToHead(Header());
			Body();
			HtmlFile.AddToBody(Footer());
			HtmlFile.Render();
		}

		private string FileName()
		{
			return $"nfl-{Year}.htm";
		}

		private void Body()
		{
			HtmlFile.AddToBody(Heading());
		}

		private string Heading()
		{
			var sb = new StringBuilder();
			AddLine(sb, HtmlLib.DivOpenClass("header row"));
			var link = HtmlLib.Href("..\\tfl-out\\currstnd.txt", Year.ToString());
			AddLine(sb, HtmlLib.H2($"Season {link}"));
			AddLine(sb, HtmlLib.DivClose());
			return sb.ToString();
		}

		private string Header()
		{
			var sb = new StringBuilder();
			AddLine(sb,"  <meta name = 'viewport' content ='width=device-width;initial-scale = 1.0;'maximum-scale = 1.0;/>");
			AddLine(sb,"  <meta name = 'apple-mobile-web-app-capable' content = 'yes' />");
			AddLine(sb,string.Empty);
			AddLine(sb,HtmlLib.Comment("JQuery UI"));
			AddLine(sb,HtmlLib.CssLink("../css/jquery-ui-1.10.0.custom.min.css"));
			AddLine(sb,HtmlLib.CssLink("../css/base.css"));
			AddLine(sb,HtmlLib.CssLink("../css/tablet.css"));
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.Comment("Javascript"));
			AddLine(sb,HtmlLib.JSScriptFile("../js/jquery-1.8.2.min.js"));
			AddLine(sb,HtmlLib.JSScriptFile("../js/jquery-ui-1.10.0.custom.min.js"));
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.StyleOpen());
			AddLine(sb,"  .class1 A:link {text-decoration: yellow;}");
			AddLine(sb,"  .left.col {width: 250px;}");
			AddLine(sb,"  .right.col {left: 250px;right: 0;}");
			AddLine(sb,"  .header.row {height: 75px;	background: #333;}");
			AddLine(sb,"  .body.row {top: 75px;bottom: 50px;background: #000;padding: 1em;}");
			AddLine(sb,"  .footer.row {height: 75px;top: 20px;bottom: 0;background: #333;}");
			AddLine(sb,"  body {color: White;background: #000;font - family: Segoe UI, Helvetica, Arial, Sans-Serif;}");
			AddLine(sb,"  .header, .footer {padding: 0 1em;}");
			AddLine(sb,HtmlLib.StyleClose());
			AddLine(sb, string.Empty);
			AddLine(sb,HtmlLib.ScriptOpen());
			var script = @"
        $(function () {

            $('#myAccordian').accordion({
                header: 'h3',

				activate: false,
                heightStyle: 'fill',
                collapsible: true,
                autoHeight: false
			});
        });";
			AddLine(sb,script);
			AddLine(sb,HtmlLib.ScriptClose());

			return sb.ToString();
		}

		private static void AddLine(StringBuilder sb, string line)
		{
			sb.AppendLine(line);
		}

		private static string Footer()
		{
			return string.Empty;
		}
	}
}
