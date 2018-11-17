using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Helpers
{
    /// <summary>
    ///This is a test class for OriginalGridHelperTest and is intended
    ///to contain all OriginalGridHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GridHelperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private class TestController : Controller
        {
            public ActionResult Index()
            {
                return View();
            }
        }

        private class ParentModel
        {
            [SelectionType(SelectionType.None)]
            public IEnumerable<GridModel> DisplayGrid { get; set; }

            [Selector(TargetProperty = "SingleSelectGrid")]
            public int SelectedID { get; set; }

            [SelectionType(SelectionType.Single)]
            public IEnumerable<GridModel> SingleSelectGrid { get; set; }

            [SelectionType(SelectionType.Multiple)]
            public IEnumerable<GridModel> MultipleSelectGrid { get; set; }
        }

        [Link("Link", Action = "MyAction", Controller = "MyController", Area = "MyArea", Parameters = new[] { "ID", "Data" })]
        [Link("Link", ActionForDependencyType.Hidden, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
        [Link("Link", ActionForDependencyType.Hidden, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
        [Link("Link", ActionForDependencyType.Visible, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
        [Link("Link", ActionForDependencyType.Visible, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
        [Link("Link", ActionForDependencyType.Enabled, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
        [Link("Link", ActionForDependencyType.Enabled, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
        [Link("Link", ActionForDependencyType.Disabled, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
        [Link("Link", ActionForDependencyType.Disabled, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
        private class GridModel
        {
            [Key]
            public int ID { get; set; }

            public bool PositiveDependentProperty { get; set; }

            public bool NegativeDependentProperty { get; set; }

            public string Data { get; set; }

            [Link("Link", Action = "MyAction", Controller = "MyController", Area = "MyArea", Parameters = new[] { "ID", "Data" })]
            public string DataParameters { get; set; }

            [Link("Link", ActionForDependencyType.Hidden, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
            public string DataHiddenPositive { get; set; }

            [Link("Link", ActionForDependencyType.Hidden, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
            public string DataHiddenNegative { get; set; }

            [Link("Link", ActionForDependencyType.Visible, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
            public string DataVisiblePositive { get; set; }

            [Link("Link", ActionForDependencyType.Visible, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
            public string DataVisibleNegative { get; set; }

            [Link("Link", ActionForDependencyType.Enabled, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
            public string DataEnabledPositive { get; set; }

            [Link("Link", ActionForDependencyType.Enabled, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
            public string DataEnabledNegative { get; set; }

            [Link("Link", ActionForDependencyType.Disabled, "PositiveDependentProperty", ComparisonType.EqualTo, false)]
            public string DataDisabledPositive { get; set; }

            [Link("Link", ActionForDependencyType.Disabled, "NegativeDependentProperty", ComparisonType.NotEqualTo, false)]
            public string DataDisabledNegative { get; set; }

            [Hidden]
            public string HiddenData { get; set; }

            [Selector]
            public bool Selected { get; set; }
        }

        private ParentModel GetModel()
        {
            var gridModels = new List<GridModel>();

            for (int i = 0; i < 5; i++)
            {
                var gridModel = new GridModel();

                gridModel.ID = i;
                gridModel.Data = "data";
                gridModel.DataDisabledNegative = "data";
                gridModel.DataDisabledPositive = "data";
                gridModel.DataEnabledNegative = "data";
                gridModel.DataEnabledPositive = "data";
                gridModel.DataVisibleNegative = "data";
                gridModel.DataVisiblePositive = "data";
                gridModel.DataHiddenNegative = "data";
                gridModel.DataHiddenPositive = "data";
                gridModel.HiddenData = "data";

                gridModels.Add(gridModel);
            }

            var model = new ParentModel();

            model.DisplayGrid = gridModels;
            model.SingleSelectGrid = gridModels;
            model.MultipleSelectGrid = gridModels;

            return model;
        }

        private HttpContextBase GetFakeAuthenticatedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var policy = new Mock<HttpCachePolicyBase>();

            context.Setup(ctx => ctx.User).Returns(mockClaimsPrincipal.Object);
            context.Setup(ctx => ctx.Items).Returns(new Dictionary<object, object>());
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Response.Cache).Returns(policy.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }

        public HtmlHelper<dynamic> CreateHtmlHelper(ViewDataDictionary vd)
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute("CreateHtmlHelper", "{area}/{controller}/{action}", new { area = UrlParameter.Optional, controller = "Default", action = "Index", id = UrlParameter.Optional });

            var httpContext = GetFakeAuthenticatedHttpContext();

            var routeData = new RouteData(RouteTable.Routes.First(), new Mock<IRouteHandler>().Object);

            routeData.DataTokens.Add("action", "MyAction");
            routeData.DataTokens.Add("controller", "MyController");
            routeData.DataTokens.Add("area", "MyArea");

            routeData.Values["action"] = "MyAction";
            routeData.Values["controller"] = "MyController";
            routeData.Values["area"] = "MyArea";

            Mock<ViewContext> mockViewContext = new Mock<ViewContext>(
                new ControllerContext(
                    new Mock<HttpContextBase>().Object,
                    routeData,
                    new Mock<ControllerBase>().Object),
                new Mock<IView>().Object,
                vd,
                new TempDataDictionary(),
                new Mock<TextWriter>().Object);

            var mockViewDataContainer = new Mock<IViewDataContainer>();

            mockViewDataContainer.Setup(v => v.ViewData).Returns(vd);

            mockViewContext.Setup(m => m.HttpContext).Returns(httpContext);
            mockViewContext.Setup(m => m.ViewData).Returns(vd);
            mockViewContext.Setup(m => m.RouteData).Returns(routeData);

            return new HtmlHelper<dynamic>(mockViewContext.Object, mockViewDataContainer.Object);
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();

            // Setup principal to return identity
            mockClaimsPrincipal.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup identity to default as authenticated
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");
            mockClaimsIdentity.Setup(m => m.IsAuthenticated).Returns(true);

            // Setup claims identity in User Service
            mockUserService.Setup(m => m.DateTime).Returns(DateTime.Now);
            mockUserService.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();
        }

        /// <summary>
        /// A test for OriginalGridHelper Constructor
        ///</summary>
        [TestMethod]
        public void OriginalGridHelper_Constructor_NotNull()
        {
            var target = new GridHelper();

            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetKeyMetadata
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetKeyMetadataTest_Metadata_Null()
        {
            GridHelper.GetKeyMetadata(null);
            Assert.Fail("ArgumentNullException expected");
        }

        /// <summary>
        ///A test for GetKeyMetadata
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SelectorException))]
        public void GetKeyMetadataTest_Metadata()
        {
            var modelMetadataproperties = new List<ModelMetadata>();
            GridHelper.GetKeyMetadata(modelMetadataproperties);
            Assert.Fail("SelectorException expected");
        }


        /// <summary>
        /// Testing expected exceptions on multiple keys
        /// </summary>
        private class OneKeyViewModel
        {
            [Key]
            public int Id1 { get; set; }

            public int Id2 { get; set; }
        }

        /// <summary>
        ///A test for GetKeyMetadata
        ///</summary>
        [TestMethod()]
        public void GetKeyMetadataTest_Metadata_Key()
        {
            var model = new OneKeyViewModel();
            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());
            var p = GridHelper.GetKeyMetadata(metadata);
            Assert.IsNotNull(p);
            Assert.IsTrue(p.AdditionalValues.Any());
        }


        /// <summary>
        /// Testing expected exceptions on multiple keys
        /// </summary>
        private class MultiKeyViewModel
        {
            [Key]
            public int Id1 { get; set; }

            [Key]
            public int Id2 { get; set; }

            [Selector]
            public bool Selected { get; set; }
        }

        /// <summary>
        ///A test for GetKeyMetadata with multiple keys
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SelectorException))]
        public void GetKeyMetadataTest_Metadata_MultipleKeys()
        {
            var model = new MultiKeyViewModel();
            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());
            GridHelper.GetKeyMetadata(metadata);
        }


        /// <summary>
        ///A test for GetSelectionType
        ///</summary>
        [TestMethod()]
        public void GetSelectionTypeTest()
        {
            var model = new GridModel();
            var parentModel = new ParentModel();
            var modelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());
            var parentModelPropertyMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(parentModel, parentModel.GetType());

            var modelPropertyMetadata = parentModelPropertyMetadata as List<ModelMetadata> ?? parentModelPropertyMetadata.ToList();
            var d = modelPropertyMetadata.First(p => p.PropertyName == "DisplayGrid");
            var modelMetadatas = modelMetadata as List<ModelMetadata> ?? modelMetadata.ToList();
            SelectionType actual = GridHelper.GetSelectionType(modelMetadatas.FirstOrDefault(), d);
            Assert.AreEqual(SelectionType.None, actual);

            var s = modelPropertyMetadata.First(p => p.PropertyName == "SingleSelectGrid");
            actual = GridHelper.GetSelectionType(modelMetadatas.FirstOrDefault(), s);
            Assert.AreEqual(SelectionType.Single, actual);

            var m = modelPropertyMetadata.First(p => p.PropertyName == "MultipleSelectGrid");
            actual = GridHelper.GetSelectionType(modelMetadatas.FirstOrDefault(), m);
            Assert.AreEqual(SelectionType.Multiple, actual);
        }



        /// <summary>
        ///A test for GetSelectorMetadata
        ///</summary>
        [TestMethod()]
        public void GetSelectorMetadataTest()
        {
            var model = new GridModel();
            var parentModel = new ParentModel();
            var modelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());
            var parentModelPropertyMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(parentModel, parentModel.GetType());

            var parentModelPropertiesMetadata = parentModelPropertyMetadata as List<ModelMetadata> ?? parentModelPropertyMetadata.ToList();
            var modelPropertiesMetadata = modelMetadata as List<ModelMetadata> ?? modelMetadata.ToList();
            ModelMetadata actual = GridHelper.GetSelectorMetadata(modelPropertiesMetadata.FirstOrDefault(), modelPropertiesMetadata, parentModelPropertiesMetadata.FirstOrDefault(), parentModelPropertiesMetadata);
            Assert.IsNull(actual);

            var s = parentModelPropertiesMetadata.First(p => p.PropertyName == "SingleSelectGrid");
            ModelMetadata actual1 = GridHelper.GetSelectorMetadata(modelPropertiesMetadata.FirstOrDefault(), modelPropertiesMetadata, s, parentModelPropertiesMetadata);
            Assert.IsNotNull(actual1);
            Assert.IsTrue(actual1.ContainerType == typeof(ParentModel));

            var m = parentModelPropertiesMetadata.First(p => p.PropertyName == "MultipleSelectGrid");
            ModelMetadata actual2 = GridHelper.GetSelectorMetadata(modelPropertiesMetadata.FirstOrDefault(), modelPropertiesMetadata, m, parentModelPropertiesMetadata);
            Assert.IsNotNull(actual2);
            Assert.IsTrue(actual2.ContainerType == typeof(GridModel));
        }

        private class ParentMultiKeyModel
        {
            [Selector(TargetProperty = "SingleSelectGrid")]
            public int SelectedID { get; set; }

            [SelectionType(SelectionType.Single)]
            public IEnumerable<MultiKeyViewModel> SingleSelectGrid { get; set; }

            [SelectionType(SelectionType.Multiple)]
            public IEnumerable<MultiKeyViewModel> MultipleSelectGrid { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(SelectorException))]
        public void GetSelectorMetadata_MultipleKeyWithSingleSelect_ExceptionExpected()
        {
            var parentModel = new ParentMultiKeyModel();
            parentModel.SingleSelectGrid = new[] { new MultiKeyViewModel() };
            
            var parentModelPropertiesMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(parentModel, parentModel.GetType()).ToList();
            
            var parentModelPropertyMetadata = parentModelPropertiesMetadata.FirstOrDefault(m => m.PropertyName == "SingleSelectGrid");

            foreach (var model in parentModel.SingleSelectGrid)
            {
                var modelCopy = model;
                Func<object> modelAccessor = () => modelCopy;

                var modelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(modelAccessor, model.GetType());
                var modelPropertiesMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());

                GridHelper.GetSelectorMetadata(modelMetadata, modelPropertiesMetadata, parentModelPropertyMetadata, parentModelPropertiesMetadata);
            }

            
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(SelectorException))]
        public void GetSelectorMetadata_MultipleKeyWithMultipleSelect_ExceptionExpected()
        {
            var parentModel = new ParentMultiKeyModel();
            parentModel.MultipleSelectGrid = new[] { new MultiKeyViewModel() };

            var parentModelPropertiesMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(parentModel, parentModel.GetType()).ToList();

            var parentModelPropertyMetadata = parentModelPropertiesMetadata.FirstOrDefault(m => m.PropertyName == "MultipleSelectGrid");

            foreach (var model in parentModel.MultipleSelectGrid)
            {
                var modelCopy = model;
                Func<object> modelAccessor = () => modelCopy;

                var modelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(modelAccessor, model.GetType());
                var modelPropertiesMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType());

                GridHelper.GetSelectorMetadata(modelMetadata, modelPropertiesMetadata, parentModelPropertyMetadata, parentModelPropertiesMetadata);
            }


            Assert.Fail();
        }
    }
}