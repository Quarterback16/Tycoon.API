using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.MarkdownLite;
using System.IO;

namespace MarkdownService
{
    [Route("/")]
    public class MdlController : Controller
    {
        private readonly IMarkdownEngine engine;

        public MdlController(
            IMarkdownEngine engine)
        {
            this.engine = engine;
        }

        [HttpPost]
        public IActionResult Convert()
        {
            var reader = new StreamReader(Request.Body);
            var markdown = reader.ReadToEnd();
            var result = engine.Markup(markdown);
            return Content(result);
        }
    }
}