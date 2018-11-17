using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    [TestClass]
    public class RouteRegistrationServiceTest
    {
        private Mock<IConfigurationManager> mockConfigurationManager;

        [TestInitialize]
        public void TestInitialize()
        {
            mockConfigurationManager = new Mock<IConfigurationManager>();

            var appSettings = new NameValueCollection();

            appSettings.Add("ShortRoutes", Boolean.TrueString);

            mockConfigurationManager.Setup(m => m.AppSettings).Returns(appSettings);
        }

        [TestMethod]
        public void TestShorten()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
          Assert.IsTrue( service.shorten("test")=="test");
          Assert.IsTrue(service.shorten("Participantregistration") == "Prt");
          Assert.IsTrue(service.usedShortValues.Contains("Prt"));
          Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
          Assert.IsTrue(service.usedShortValues.Contains("test"));
          Assert.IsTrue(service.shortenedNames.ContainsKey("test"));
        }

        [TestMethod]
        public void TestShortenRepeat()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            Assert.IsTrue(service.shorten("Participantregistration") == "Prt");
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shorten("Participantregistration") == "Prt");
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
        }


        [TestMethod]
        public void TestShortenCaseSensitive()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            Assert.IsTrue(service.shorten("Participantregistration") == "Prt");
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shorten("partICIPAntregiSTratiON") == "Prt");
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
        }



        [TestMethod]
        public void TestShortenSimilarNames()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            Assert.IsTrue(service.shorten("Participantregistration") == "Prt");
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(string.Equals(  service.shorten("partICIPAntregiSTratiONing") , "Prt0",StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(service.usedShortValues.Contains("Prt0"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("partICIPAntregiSTratiONing"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestShortenArgumentNull()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            service.shorten(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestShortenArgumentEmpty()
        {
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            service.shorten("");
        }

        [TestMethod]
        public void TestMapRoute()
        {
            RouteTable.Routes.Clear();
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            AreaRegistrationContext context = new AreaRegistrationContext("test",RouteTable.Routes);
            service.MapRoute(context, "Example_ParticipantRegistration",
                "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
                new {jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example"});
            Assert.IsTrue(service.routes.Count == 1 );
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
        }

        [TestMethod]
        public void TestMapRouteRepeat()
        {
            RouteTable.Routes.Clear();
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            AreaRegistrationContext context = new AreaRegistrationContext("test", RouteTable.Routes);
            service.MapRoute(context, "Example_ParticipantRegistration",
                "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
                new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            service.MapRoute(context, "Example_ParticipantRegistration_2",
                "ParticipantRegistration/{controller}/Testing/{jobseekerId}/{contractId}",
                new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Testing" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            Assert.IsTrue(service.routes.Count == 2);
        }


        [TestMethod]
        public void TestMapRouteCaseSensitive()
        {
            RouteTable.Routes.Clear();
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            int hardcodedRenames = service.shortenedNames.Count;
            AreaRegistrationContext context = new AreaRegistrationContext("test", RouteTable.Routes);

            service.MapRoute(context, "Example_ParticipantRegistration",
    "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
    new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            service.MapRoute(context, "Example_ParticipantRegistration_2",
                "partICIPAntregiSTratiON/{controller}/Testing/{jobseekerId}/{contractId}",
                new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Testing" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            Assert.IsTrue(service.shortenedNames.Count == 1 + hardcodedRenames);
            Assert.IsTrue(service.routes.Count == 2 );
        }

        [TestMethod]
        public void TestMapRouteSimilarNames()
        {
            RouteTable.Routes.Clear();
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            int hardcodedRenames = service.shortenedNames.Count;
            AreaRegistrationContext context = new AreaRegistrationContext("test", RouteTable.Routes);

            service.MapRoute(context, "Example_ParticipantRegistration",
    "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
    new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            service.MapRoute(context, "Example_partICIPAntregiSTratiONing_2",
                "partICIPAntregiSTratiONing/{controller}/Testing/{jobseekerId}/{contractId}",
                new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Testing" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt0"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("partICIPAntregiSTratiONing"));
            Assert.IsTrue(service.shortenedNames.Count == 2 + hardcodedRenames);
            Assert.IsTrue(service.routes.Count == 2);
        }


        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestMapRouteDuplicate()
        {
            RouteTable.Routes.Clear();
            RouteRegistrationService service = new RouteRegistrationService(mockConfigurationManager.Object);
            AreaRegistrationContext context = new AreaRegistrationContext("test", RouteTable.Routes);

            service.MapRoute(context, "Example_ParticipantRegistration",
    "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
    new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" });
            Assert.IsTrue(service.usedShortValues.Contains("Prt"));
            Assert.IsTrue(service.shortenedNames.ContainsKey("ParticipantRegistration"));
            service.MapRoute(context, "Example_ParticipantRegistration",
    "ParticipantRegistration/{controller}/History/{jobseekerId}/{contractId}",
    new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" });
        }


    }
}
