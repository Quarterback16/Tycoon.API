using System.Diagnostics.CodeAnalysis;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Tests.ViewModels
{
    /// <summary>
    ///This is a test class for ContentTest and is intended
    ///to contain all ContentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Content Constructor
        ///</summary>
        [TestMethod()]
        public void ContentConstructorTest()
        {
            string text = string.Empty;
            var target = new Content(ContentType.Title, text);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Text
        ///</summary>
        [TestMethod()]
        public void TextTest()
        {
            string text = string.Empty; 
            var target = new Content(ContentType.Title, text); 
            string expected = string.Empty;
            target.Text = expected;
            string actual = target.Text;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        public void TypeTest()
        {
            string text = string.Empty; 
            var target = new Content(ContentType.Title, text);
            Assert.AreEqual(ContentType.Title, target.Type);
        }
    }
}