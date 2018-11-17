using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Tests.ViewModels
{
    /// <summary>
    ///This is a test class for ContentViewModelTest and is intended
    ///to contain all ContentViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentViewModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ContentViewModel Constructor
        ///</summary>
        [TestMethod()]
        public void ContentViewModelConstructorTest()
        {
            var target = new ContentViewModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for AddParagraph
        ///</summary>
        [TestMethod()]
        public void AddParagraphTest()
        {
            var target = new ContentViewModel();
            const string text = "A test for paragraph {0}";
            
            ContentViewModel actual = target.AddParagraph(text);
            target.AddParagraph("Test");

            Assert.IsTrue( actual.GetContent().Any());
            var p = actual.GetContent().First();
            Assert.AreEqual(ContentType.Paragraph, p.Type);
            Assert.AreEqual(text, p.Text);
            Assert.AreEqual("Test", actual.GetContent().Last().Text);
        }

        /// <summary>
        ///A test for AddPreformatted
        ///</summary>
        [TestMethod()]
        public void AddPreformattedTest()
        {
            var target = new ContentViewModel();
            const string text = "A test for paragraph {0}";

            ContentViewModel actual = target.AddPreformatted(text);
            target.AddPreformatted("Test");
            Assert.IsTrue(actual.GetContent().Any());
            var p = actual.GetContent().First();
            Assert.AreEqual(ContentType.Preformatted, p.Type);
            Assert.AreEqual(text, p.Text);
            Assert.AreEqual("Test",actual.GetContent().Last().Text);
        }

        /// <summary>
        ///A test for AddTitle
        ///</summary>
        [TestMethod()]
        public void AddTitleTest()
        {
            var target = new ContentViewModel();
            const string text = "A Title";
            target.AddTitle(text);

            ContentViewModel actual = target.AddTitle("Title");
            Assert.AreEqual(text, actual.GetContent().First(t => t.Type == ContentType.Title).Text);
            Assert.AreEqual("Title", actual.GetContent().Last(t => t.Type == ContentType.Title).Text);
        }

        /// <summary>
        ///A test for Employment.Web.Mvc.Infrastructure.Interfaces.IFluent.GetType
        ///</summary>
        [TestMethod()]
        public void GetTypeTest()
        {
            IFluent target = new ContentViewModel();
            Type expected = typeof(ContentViewModel);
            Type actual = target.GetType();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetContent
        ///</summary>
        [TestMethod()]
        public void GetContentTest()
        {
            var target = new ContentViewModel();
            target.AddTitle("Title");
            List<Content> actual = target.GetContent();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Title", actual.First(t => t.Type == ContentType.Title).Text);
        }
    }
}