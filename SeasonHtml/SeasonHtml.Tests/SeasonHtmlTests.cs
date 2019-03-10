using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeasonHtml.Tests
{
	[TestClass]
    public class SeasonHtmlTests
    {
		private Season _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new Season(2019);
		}

		[TestMethod]
		public void Season_Renders_Html()
		{
			_sut.Render();
		}
    }
}
