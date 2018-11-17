using System.Collections.Specialized;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Models
{
    /// <summary>
    ///This is a test class for MenuModelTest and is intended
    ///to contain all MenuModelTest Unit Tests
    ///</summary>
    [TestClass]
    public class MenuModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for MenuModel Constructor
        ///</summary>
        [TestMethod]
        public void MenuModelConstructorTest()
        {
            var target = new MenuModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod]
        public void EqualsTest()
        {
            var target = new MenuModel();
            var obj = new MenuModel();
            
            Assert.IsTrue(target.Equals(obj));

            Assert.IsFalse(target.Equals(null));

            var parentMenuModel = new MenuModel();
            target.Parent = parentMenuModel;
            obj.Parent = parentMenuModel;

            Assert.IsTrue(target.Equals(obj));
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod]
        public void GetHashCodeTest()
        {
            var target = new MenuModel();
            var obj = new MenuModel();

            target.Name = "Test";
            obj.Name = "Test";

            target.Action = "Action";
            obj.Action = "Action";

            target.Area = "Area";
            obj.Area = "Area";

            target.ParentController = "ParentController";
            obj.ParentController = "ParentController";

            target.ParentAction = "ParentAction";
            obj.ParentAction = "ParentAction";

            Assert.AreEqual(obj.GetHashCode(), target.GetHashCode());

            obj.Name = "Test1";
            Assert.AreNotEqual(obj.GetHashCode(), target.GetHashCode());
        }

        /// <summary>
        ///A test for IsAuthorized
        ///</summary>
        [TestMethod]
        public void IsAuthorizedTest()
        {
            var target = new MenuModel();
            var identity = new Mock<ClaimsIdentity>();
            identity.SetupGet(a => a.IsAuthenticated).Returns(true);

            var actual = target.IsAuthorized(identity.Object);
            Assert.IsFalse(actual);


            var appSettings = new NameValueCollection {{"Environment", "PROD"}};

            var mockConfigurationManager = new Mock<IConfigurationManager>();
            mockConfigurationManager.SetupGet(m => m.AppSettings).Returns(appSettings);

            var mockContainerProvider = new Mock<IContainerProvider>();

            mockContainerProvider.Setup(m => m.GetService<IConfigurationManager>()).Returns(mockConfigurationManager.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            target.ControllerSecurity = new SecurityModel {AllowAny = true, AllowInProduction = true};
            Assert.IsTrue(target.IsAuthorized(identity.Object));

            target.ControllerSecurity = null;
            target.ActionSecurity = new SecurityModel { AllowAny = true, AllowInProduction = true };
            Assert.IsTrue(target.IsAuthorized(identity.Object));
        }

        /// <summary>
        ///A test for Action
        ///</summary>
        [TestMethod]
        public void ActionTest()
        {
            var target = new MenuModel {Action = "Action"};

            var actual = target.Action;
            Assert.AreEqual("Action", actual);
        }

        /// <summary>
        ///A test for ActionSecurity
        ///</summary>
        [TestMethod]
        public void ActionSecurityTest()
        {
            var target = new MenuModel { ActionSecurity = new SecurityModel()};

            var actual = target.ActionSecurity;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for Area
        ///</summary>
        [TestMethod]
        public void AreaTest()
        {
            var target = new MenuModel {Area = "Area"};
            Assert.AreEqual("Area",target.Area);
        }

        /// <summary>
        ///A test for Controller
        ///</summary>
        [TestMethod]
        public void ControllerTest()
        {
            var target = new MenuModel { Controller = "Controller"};
            Assert.AreEqual("Controller", target.Controller);
        }

        /// <summary>
        ///A test for ControllerSecurity
        ///</summary>
        [TestMethod]
        public void ControllerSecurityTest()
        {
            var target = new MenuModel {ControllerSecurity = new SecurityModel() };
            Assert.IsNotNull(target.ControllerSecurity);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new MenuModel {Name = "Name"};
            Assert.AreEqual("Name",target.Name);
        }

        /// <summary>
        ///A test for Order
        ///</summary>
        [TestMethod]
        public void OrderTest()
        {
            var target = new MenuModel {Order = 99};
            Assert.AreEqual(99,target.Order);
        }

        /// <summary>
        ///A test for Parent
        ///</summary>
        [TestMethod]
        public void ParentTest()
        {
            var target = new MenuModel {Parent = new MenuModel()};
            Assert.IsNotNull(target.Parent);
        }

        /// <summary>
        ///A test for ParentAction
        ///</summary>
        [TestMethod]
        public void ParentActionTest()
        {
            var target = new MenuModel {ParentAction = "ParentAction"};
            Assert.AreEqual("ParentAction", target.ParentAction);
        }

        /// <summary>
        ///A test for ParentArea
        ///</summary>
        [TestMethod]
        public void ParentAreaTest()
        {
            var target = new MenuModel { ParentArea = "ParentArea" };
            Assert.AreEqual("ParentArea", target.ParentArea);
        }

        /// <summary>
        ///A test for ParentController
        ///</summary>
        [TestMethod]
        public void ParentControllerTest()
        {
            var target = new MenuModel { ParentController = "ParentController" };
            Assert.AreEqual("ParentController", target.ParentController);
        }

        /// <summary>
        ///A test for Url
        ///</summary>
        [TestMethod]
        public void UrlTest()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(new StringWriter()))
                {
                    User = new GenericPrincipal(new GenericIdentity("username"), new string[0])
                };

            // Setup route
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute("Menu_GetUrl_ReturnsUrl", "{area}/{controller}/{action}", new { area = UrlParameter.Optional, controller = "Default", action = "Index", id = UrlParameter.Optional });

            var target = new MenuModel();
            Assert.IsFalse(string.IsNullOrEmpty(target.Url));
            Assert.AreEqual(@"/",target.Url);
        }
    }
}
