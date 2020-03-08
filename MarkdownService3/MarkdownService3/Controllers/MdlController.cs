using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.MarkdownLite;

namespace MarkdownService3.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MdlController : ControllerBase
	{
		private readonly IMarkdownEngine _engine;

		public MdlController(
			IMarkdownEngine engine)
		{
			_engine = engine;
		}

		[HttpPost]
		public IActionResult Convert()
		{
			var reader = new StreamReader(
				Request.Body);
			var markdown = reader.ReadToEnd();
			var result = _engine.Markup(
				markdown);
			return Content(result);
		}
	}
}
