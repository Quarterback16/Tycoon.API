using Employment.Web.Mvc.Area.Admin.Controllers;
using Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Area.Admin.Tests.Controllers
{
    /// <summary>
    /// Unit Tests for <see cref="AdwLookupController"/>.
    /// </summary>

    [TestClass]
    public class AdwLookupControllerTest
    {

        private Mock<IAdwService> mockAdwService;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwAdminService> mockAdwAdminService;


        private AdwLookupController SystemUnderTest()
        {
            return new AdwLookupController(mockAdwAdminService.Object, mockAdwService.Object, mockUserService.Object);
        }


        /// <summary>
        /// Runs before each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialise()
        {
            mockUserService = new Mock<IUserService>();
            mockAdwAdminService = new Mock<IAdwAdminService>();
            mockAdwService = new Mock<IAdwService>();

            mockUserService.Setup(x => x.DateTime).Returns(DateTime.Now);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwLookupController_ConstructorWithNullAdwAdminService()
        {
            var instance = new AdwLookupController(null, mockAdwService.Object, mockUserService.Object);
        }


        [TestMethod]
        public void AdwLookupControllerIndex()
        {
            var controller = SystemUnderTest();

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result, "View Result must not be null.");

        }

        #region List Code Type

        [TestMethod]
        public void ListCodeType_GetPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new CodeTypeModel{CodeType ="ABCD", ShortDescription = "Short Description", LongDescription = "LongDescription"};
            IList<CodeTypeModel> models = new List<CodeTypeModel>(){codeModel};
            mockAdwAdminService.Setup(m => m.GetListCodeTypes(It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListCodeType(startswith: "A", listtype: "A") as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if(result!= null)
            {
                var model = result.Model as ListCodeTypeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
                Assert.AreEqual(codeModel.CodeType, models.FirstOrDefault().CodeType);
            }

        }

        [TestMethod]
        public void ListCodeType_GetWithoutPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new CodeTypeModel { CodeType = "ABCD", ShortDescription = "Short Description", LongDescription = "LongDescription" };
            IList<CodeTypeModel> models = new List<CodeTypeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodeTypes(It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListCodeType(startswith: null, listtype: null) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListCodeTypeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
            }

        }


        [TestMethod]
        public void ListCodeType_PostRedirectedToGet()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListCodeTypeViewModel() { StartFromTableType = "A", ListType = "A", MaxRows = 1, ExactLookup = true };
            var codeModel = new CodeTypeModel { CodeType = "ABCD", ShortDescription = "Short Description", LongDescription = "LongDescription" };
            IList<CodeTypeModel> models = new List<CodeTypeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodeTypes(It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListCodeType(viewModel) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result, "Redirect To Route Result must not be null.");
            if (result != null)
            {
                Assert.AreEqual("ListCodeType", result.RouteValues["action"]);
            }

        }

        [TestMethod]
        public void ListCodeType_Post()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListCodeTypeViewModel() { };
            var codeModel = new CodeTypeModel { CodeType = "ABCD", ShortDescription = "Short Description", LongDescription = "LongDescription" };
            IList<CodeTypeModel> models = new List<CodeTypeModel>() { codeModel };
            
            controller.ModelState.AddModelError("", "SomeError");

            // Act
            var result = controller.ListCodeType(viewModel) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");


        }

#endregion


        #region List Code

        [TestMethod]
        public void ListCode_GetPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new CodeModel { Code = "ABCD", ShortDescription = "Short Description", Description = "LongDescription" };
            IList<CodeModel> models = new List<CodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(models);


            // Act
            var result = controller.ListCode(tabletype: "A", startcode: "A", listtype: "A") as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListCodeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
                Assert.AreEqual(codeModel.Code, models.FirstOrDefault().Code);
            }

        }

        [TestMethod]
        public void ListCode_GetWithoutPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new CodeModel { Code = "ABCD", ShortDescription = "Short Description", Description = "LongDescription" };
            IList<CodeModel> models = new List<CodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(models);

            // Act
            var result = controller.ListCode(tabletype: null, startcode: null, listtype: null) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListCodeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
            }

        }


        [TestMethod]
        public void ListCode_PostRedirectedToGet()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListCodeViewModel() { StartFromTableType = "A", ListType = "A", MaxRows = 1, ExactLookup = true };
            var codeModel = new CodeModel { Code = "ABCD", ShortDescription = "Short Description", Description = "LongDescription" };
            IList<CodeModel> models = new List<CodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(models);

            // Act
            var result = controller.ListCode(viewModel) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result, "Redirect To Route Result must not be null.");
            if (result != null)
            {
                Assert.AreEqual("ListCode", result.RouteValues["action"]);
            }

        }

        [TestMethod]
        public void ListCode_Post()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListCodeViewModel() { };
            var codeModel = new CodeModel { Code = "ABCD", ShortDescription = "Short Description", Description = "LongDescription" };
             
            controller.ModelState.AddModelError("", "SomeError");

            // Act
            var result = controller.ListCode(viewModel) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");


        }


        #endregion


        #region List Related Code Type

        [TestMethod]
        public void ListRelatedCodeType_GetPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new RelatedCodeTypeModel { SubType = "ABCD", SubDescription = "subordinate Description", DomDescription = "LongDescription" };
            IList<RelatedCodeTypeModel> models = new List<RelatedCodeTypeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetRelatedListCodeTypes(It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListRelatedCodeType(starttype: "A", listtype: "A") as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListRelatedCodeTypeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
                Assert.AreEqual(model.StartRelatedTableType, "A");
                Assert.IsTrue(model.Results.ToList().Count == 1);
            }

        }

        [TestMethod]
        public void ListRelatedCodeType_GetWithoutPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new RelatedCodeTypeModel { SubType = "ABCD", SubDescription = "subordinate Description", DomDescription = "LongDescription" };
            IList<RelatedCodeTypeModel> models = new List<RelatedCodeTypeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetRelatedListCodeTypes(It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);

            // Act
            var result = controller.ListRelatedCodeType(starttype: null, listtype: null) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListRelatedCodeTypeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
            }

        }


        [TestMethod]
        public void ListRelatedCodeType_PostRedirectedToGet()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListRelatedCodeTypeViewModel() { StartRelatedTableType = "A", ListType = "A", MaxRows = 1, ExactLookup = true };
            var codeModel = new CodeModel { Code = "ABCD", ShortDescription = "Short Description", Description = "LongDescription" };
            IList<CodeModel> models = new List<CodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetListCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(models);

            // Act
            var result = controller.ListRelatedCodeType(viewModel) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result, "Redirect To Route Result must not be null.");
            if (result != null)
            {
                Assert.AreEqual("ListRelatedCodeType", result.RouteValues["action"]);
            }

        }

        [TestMethod]
        public void ListRelatedCodeType_Post()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListRelatedCodeTypeViewModel() { };
             
            controller.ModelState.AddModelError("", "SomeError");

            // Act
            var result = controller.ListRelatedCodeType(viewModel) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");


        }


        #endregion


        #region List Related Code 

        [TestMethod]
        public void ListRelatedCode_GetPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new RelatedCodeModel { RelatedCode = "ABCD", DominantDescription = "dominant Description", SubordinateDescription = "Sub Description" };
            IList<RelatedCodeModel> models = new List<RelatedCodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListRelatedCode(relatedtable: "Related", startcode: "A", listtype: "A", searchtable: "search") as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListRelatedCodeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
                Assert.AreEqual(model.StartFromTableType, "A");
                Assert.IsTrue(model.Results.ToList().Count == 1);
            }

        }

        [TestMethod]
        public void ListRelatedCode_GetWithoutPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new RelatedCodeModel { RelatedCode = "ABCD", DominantDescription = "dominant Description", SubordinateDescription = "Sub Description" };
            IList<RelatedCodeModel> models = new List<RelatedCodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);

            // Act
            var result = controller.ListRelatedCode(null,null, null, null) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListRelatedCodeViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
            }

        }


        [TestMethod]
        public void ListRelatedCode_PostRedirectedToGet()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListRelatedCodeViewModel() { StartFromTableType = "A", ListType = "A", MaxRows = 1, ExactLookup = true };
            var codeModel = new RelatedCodeModel { RelatedCode = "ABCD", DominantDescription = "dominant Description", SubordinateDescription = "Sub Description" };
            IList<RelatedCodeModel> models = new List<RelatedCodeModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListRelatedCode(viewModel) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result, "Redirect To Route Result must not be null.");
            if (result != null)
            {
                Assert.AreEqual("ListRelatedCode", result.RouteValues["action"]);
            }

        }

        [TestMethod]
        public void ListRelatedCode_Post()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListRelatedCodeViewModel() { };

            controller.ModelState.AddModelError("", "SomeError");

            // Act
            var result = controller.ListRelatedCode(viewModel) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");


        }


        #endregion


        #region List Property

        [TestMethod]
        public void ListProperty_GetPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new PropertyModel { Code = "ABCD", CodeType = "code type", PropertyType = "Property type" };
            IList<PropertyModel> models = new List<PropertyModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetPropertyList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);


            // Act
            var result = controller.ListProperty(startfrom: "A", startcode: "ABCD", startproperty: "Related", listtype: "A") as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListPropertyViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
                Assert.AreEqual(model.StartFromTableType, "A");
                Assert.IsTrue(model.Results.ToList().Count == 1);
            }

        }

        [TestMethod]
        public void ListProperty_GetWithoutPostRedirect()
        {
            // Arrange
            var controller = SystemUnderTest();

            var codeModel = new PropertyModel { Code = "ABCD", CodeType = "code type", PropertyType = "Property type" };
            IList<PropertyModel> models = new List<PropertyModel>() { codeModel };
            mockAdwAdminService.Setup(m => m.GetPropertyList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<char>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(models);

            // Act
            var result = controller.ListProperty(null, null, null, null) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");
            if (result != null)
            {
                var model = result.Model as ListPropertyViewModel;
                Assert.IsNotNull(model, "Model must not be null.");
            }

        }


        [TestMethod]
        public void ListProperty_PostRedirectedToGet()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListPropertyViewModel() { StartFromTableType = "A", ListType = "A", MaxRows = 1, ExactLookup = true };
            
            // Act
            var result = controller.ListProperty(viewModel) as RedirectToRouteResult;


            // Assert
            Assert.IsNotNull(result, "Redirect To Route Result must not be null.");
            if (result != null)
            {
                Assert.AreEqual("ListProperty", result.RouteValues["action"]);
            }

        }

        [TestMethod]
        public void ListProperty_Post()
        {
            // Arrange
            var controller = SystemUnderTest();

            var viewModel = new ListPropertyViewModel() { };

            controller.ModelState.AddModelError("", "SomeError");

            // Act
            var result = controller.ListProperty(viewModel) as ViewResult;


            // Assert
            Assert.IsNotNull(result, "View Result must not be null.");


        }


        #endregion

    }
}
