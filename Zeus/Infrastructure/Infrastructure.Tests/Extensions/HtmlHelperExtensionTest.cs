using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for HtmlHelperExtensionTest and is intended
    ///to contain all HtmlHelperExtensionTest Unit Tests
    ///</summary>
    [TestClass]
    public class HtmlHelperExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IUserService> mockUserService;
        private Mock<IHistoryService> mockHistoryService;
        private Mock<IConfigurationManager> mockConfigurationManager;

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var adwMapper = new AdwMapper();
        //            adwMapper.Map(Mapper.Configuration);

        //            var stringMapper = new StringMapper();
        //            stringMapper.Map(Mapper.Configuration);

        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        //private HtmlHelper html;
        private HtmlHelper<TestModel> html = null;

        private IPageable<HistoryModel> history;
        private ViewDataDictionary viewData;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            
            mockUserService = new Mock<IUserService>();
            mockHistoryService = new Mock<IHistoryService>();
            mockConfigurationManager = new Mock<IConfigurationManager>();

            history = new Pageable<HistoryModel>();
           
            history.Metadata = new PageMetadata()
            {
                ModelType = typeof(HistoryModel),
                PageNumber = 1,
                PageSize = 10,
                Total = 2
            };

            history.Add(new HistoryModel()
            {
                Values = new RouteValueDictionary { { "id", "1" } },
                HistoryType = HistoryType.JobSeeker,
                DisplayName = "JobSeeker 1",
                DateAccessed = DateTime.Now,
                Username = "XX1234",
                IsPinned = true
            });

            history.Add(new HistoryModel()
            {
                Values = new RouteValueDictionary {{"id", "2"}},
                HistoryType = HistoryType.JobSeeker,
                DisplayName = "JobSeeker 2",
                DateAccessed = DateTime.Now.AddMinutes(1),
                Username = "XX1234",
                IsPinned = false
            });



            mockHistoryService.Setup(m => m.Get(HistoryType.JobSeeker)).Returns(history);
            mockUserService.Setup(m => m.History).Returns(mockHistoryService.Object);

            mockConfigurationManager.Setup(m => m.GetSection<HistorySection>(It.IsAny<string>()))
                .Returns(new HistorySection
                             {
                             });


            mockAdwService = new Mock<IAdwService>();
            mockAdwService.Setup(g => g.GetListCodeDescription("ART", "TEST")).Returns("Description");
            mockAdwService.Setup(r=>r.GetRelatedCodeDescription("relatedCodeType", "searchCode", "code", true)).Returns("RelatedDescription");
            mockAdwService.Setup(a => a.GetListCodes("codeType", "startingCode"))
                .Returns(new List<CodeModel> { new CodeModel {Code="1",Description = "Description1"},
                                                 new CodeModel {Code="2",Description = "Description2"} });

            mockAdwService.Setup(a => a.GetListCodes("codeType", string.Empty))
                .Returns(new List<CodeModel> { new CodeModel {Code="1",Description = "Description1"},
                                                 new CodeModel {Code="2",Description = "Description2"} });

            mockAdwService.Setup(r => r.GetRelatedCodes("relatedCodeType", "searchCode", false))
                            .Returns(new List<RelatedCodeModel> { new RelatedCodeModel {Dominant = true, SubordinateCode = "1",SubordinateDescription = "Description1"},
                                                             new RelatedCodeModel {Dominant = true, SubordinateCode = "2",SubordinateDescription = "Description2"} });

            mockAdwService.Setup(r => r.GetRelatedCodes("relatedCodeType", "searchCode", true))
                .Returns(new List<RelatedCodeModel> { new RelatedCodeModel {Dominant = true, SubordinateCode = "1",SubordinateDescription = "Description1"},
                                                             new RelatedCodeModel {Dominant = true, SubordinateCode = "2",SubordinateDescription = "Description2"} });

            
            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            mockContainerProvider.Setup(m => m.GetService<IHistoryService>()).Returns(mockHistoryService.Object);
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            mockContainerProvider.Setup(m => m.GetService<IConfigurationManager>()).Returns(mockConfigurationManager.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var viewDataContainer = new Mock<IViewDataContainer>();

            viewData = new ViewDataDictionary();

            viewDataContainer.Setup(s => s.ViewData).Returns(viewData);
            
            var viewContext = new ViewContext();            

            var server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(c => c.ApplyAppPathModifier(It.IsAny<string>())).Returns("/ApplicationPath");
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.Setup(r => r.UserHostAddress).Returns("127.0.0.1");
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s.SessionID).Returns(Guid.NewGuid().ToString());

            var context                = new Mock<HttpContextBase>();
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Server).Returns(server.Object);
            context.SetupGet(c => c.Session).Returns(session.Object);
            context.SetupGet(c => c.Request.ApplicationPath).Returns("/ApplicationPath");
            context.Setup(i=>i.Items["Employment.Web.Mvc.Infrastructure.Helpers.AssetHelperInstance"]).Returns(null);

            viewContext.HttpContext = context.Object;

            html = new HtmlHelper<TestModel>(viewContext, viewDataContainer.Object);                        
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver does not have the IAdwService registered.
        /// </summary>
        //[TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AdwSelection_NoAdwServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            //var model = new AdwSelectionAttributeTest.Model() { SingleListCode = "Code" };

            //model.IsValid("SingleListCode");
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        //[TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AdwSelection_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var viewDataContainer = new Mock<IViewDataContainer>().Object;
            var viewContext = new ViewContext();

            HtmlHelper html = new HtmlHelper<TestModel>(viewContext, viewDataContainer);


            //var model = new AdwSelectionAttributeTest.Model() { SingleListCode = "Code" };

            //html.

            //model.IsValid("SingleListCode");
        }

        /// <summary>
        ///A test for AdwGetCodeDescription
        ///</summary>
        [TestMethod]
        public void AdwGetCodeDescriptionTest()
        {
            const string codeType = "ART";
            const string code = "TEST";
            
            string actual = html.AdwGetCodeDescription(codeType, code);
            
            Assert.AreEqual("Description", actual);
        }

        /// <summary>
        ///A test for AdwGetRelatedCodeDescription
        ///</summary>
        [TestMethod]
        public void AdwGetRelatedCodeDescriptionTest()
        {
            var actual = html.AdwGetRelatedCodeDescription("relatedCodeType", "searchCode", "code", true);
            Assert.AreEqual("RelatedDescription", actual);
        }


        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest()
        {
            var actual = html.AdwListCode("codeType", "selectedCodes", "startingCode", true, SelectionType.Single);
            Assert.IsNotNull(actual);
            Assert.AreEqual(3,actual.Count);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest1()
        {
            var actual = html.AdwListCode("codeType", "selectedCodes", "startingCode");
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("1",actual[0].Value);
            Assert.AreEqual("2", actual[1].Value);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest2()
        {
            IEnumerable<string> selectedCodes = new[] { "2" };
            var actual = html.AdwListCode("codeType", selectedCodes);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("1", actual[0].Value);
            Assert.AreEqual("2", actual[1].Value);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsTrue(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest3()
        {
            IEnumerable<string> selectedCodes = new[] { "2" };
            var startingCode = string.Empty;
            var actual = html.AdwListCode("codeType", selectedCodes, startingCode, false, SelectionType.Single);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("1", actual[0].Value);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsTrue(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest4()
        {
            string selectedCode = string.Empty;
            string startingCode = string.Empty;

            var actual = html.AdwListCode("codeType", selectedCode, startingCode);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("1", actual[0].Value);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsFalse(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest5()
        {
            string selectedCode = string.Empty;

            var actual = html.AdwListCode("codeType", selectedCode);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("1", actual[0].Value);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsFalse(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListCode
        ///</summary>
        [TestMethod]
        public void AdwListCodeTest6()
        {
            var actual = html.AdwListCode("codeType");
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count==2);
            Assert.AreEqual("1", actual[0].Value);
            Assert.AreEqual("2", actual[1].Value);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsFalse(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode", false, false);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count==2);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest1()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode", false, "2");
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
            Assert.IsFalse(actual[0].Selected);
            Assert.IsTrue(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest2()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode", false);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest3()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode", "2");
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest4()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode");
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest5()
        {
            var actual = html.AdwListRelatedCode("relatedCodeType", "searchCode", false, "", false, SelectionType.Single);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest6()
        {
            const string relatedCodeType = "relatedCodeType";
            const string searchCode = "searchCode";
            IEnumerable<string> selectedCodes = new []{"1"};
        
            var actual = html.AdwListRelatedCode(relatedCodeType, searchCode, false, selectedCodes);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual[0].Selected);
            Assert.IsFalse(actual[1].Selected);
        }

        /// <summary>
        ///A test for AdwListRelatedCode
        ///</summary>
        [TestMethod]
        public void AdwListRelatedCodeTest7()
        {
            const string relatedCodeType = "relatedCodeType";
            const string searchCode = "searchCode";
            IEnumerable<string> selectedCodes = new [] { "1" };

            var actual = html.AdwListRelatedCode(relatedCodeType, searchCode, false, selectedCodes, true, SelectionType.Single);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count==3);
            Assert.IsTrue(actual[1].Selected);
            Assert.IsFalse(actual[2].Selected);
        }

        /// <summary>
        ///A test for Button
        ///</summary>
        [TestMethod]
        public void ButtonTest()
        {
            const string name = "Name";
            const string value = "1";
            const string defaultResetButtonValue = "Reset";
            const bool reset = true;
            const bool primary = false;
            const bool clear = false;
            object htmlAttributes = new { Test = "true" }; 

            MvcHtmlString actual = html.Button(name, value, false, reset, clear, primary, false, false, htmlAttributes, null, false);
            Assert.IsNotNull(actual);
            
            // <input Test="true" class="button reset" name="submitType" type="reset" value="Reset" />
            var s = actual.ToHtmlString();
            Assert.IsTrue(s.Contains("input"));
            Assert.IsTrue(s.Contains(string.Format("value=\"{0}\"",defaultResetButtonValue)));  // if button is Reset, then value will be overriden to 'Reset' to avoid inconsistencies in naming.
            Assert.IsTrue(s.Contains("Test=\"true\""));
        }

        /// <summary>
        /// A Test for Clear button
        /// </summary>
        [TestMethod]
        public void ButtonTestClear()
        {
            const string name = "Name";
            const string defaultClearButtonValue = "Clear";
            const string value = "2";
            const bool reset = false;
            const bool clear = true;
            const bool primary = false;
            object htmlAttributes = new {Test = "true"};

            MvcHtmlString actualMvcString = html.Button(name, value, false, reset, clear, primary, false, false, htmlAttributes,
                                                        null, false);
            Assert.IsNotNull(actualMvcString);

            // <input Test = "true" class = "button reset" name = "submitType" type="button" value = "Clear"
            var htmlString = actualMvcString.ToHtmlString();
            Assert.IsTrue(htmlString.Contains("button"));
            Assert.IsTrue(htmlString.Contains(defaultClearButtonValue));
            Assert.IsTrue(htmlString.Contains("Test=\"true\""));
        }

        /// <summary>
        ///A test for Button
        ///</summary>
        [TestMethod]
        public void ButtonTest1()
        {
            const string name = "Name";
            const string value = "1";

            MvcHtmlString actual = html.Button(name, value);
            Assert.IsNotNull(actual);
            //<button class="1 button" name="submitType" type="submit" value="1">Name</button>
            var s = actual.ToHtmlString();
            Assert.IsTrue(s.Contains("button"));
            Assert.IsTrue(s.Contains("value=\"1\""));
            Assert.IsTrue(s.Contains(">Name<"));
        }

        /// <summary>
        ///A test for Button
        ///</summary>
        [TestMethod]
        public void ButtonTest2()
        {
            const string name = "Name";

            MvcHtmlString actual = html.Button(name);
            Assert.IsNotNull(actual);
            //<button class="1 button" name="submitType" type="submit">Name</button>
            var s = actual.ToHtmlString();
            Assert.IsTrue(s.Contains("button"));
            Assert.IsTrue(s.Contains("type=\"submit\""));
            Assert.IsTrue(s.Contains(">Name<"));
        }

        /// <summary>
        ///A test for Button
        ///</summary>
        [TestMethod]
        public void ButtonTest3()
        {
            var button = new ButtonAttribute("Name","foo");
            
            var actual = html.Button(button);
            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString(); //<button class="name button" name="submitType" type="submit" value="Name">Name</button>
            Assert.IsTrue(s.Contains("button"));
            Assert.IsTrue(s.Contains("type=\"submit\""));
        }

        /// <summary>
        /// Test button is rendered when user has the required role.
        /// </summary>
        [TestMethod]
        public void ButtonRolesTest_Valid()
        {
            mockUserService.Setup(m => m.IsInRole(It.IsAny<IEnumerable<string>>())).Returns(true);
            var button = new ButtonAttribute("Name","foo") { Roles = new[] { "FOO" } };

            var result = html.Button(button);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.ToString()));
        }

        /// <summary>
        /// Test button is not rendered when user does not have the required role.
        /// </summary>
        [TestMethod]
        public void ButtonRolesTest_Invalid()
        {
            mockUserService.Setup(m => m.IsInRole(It.IsAny<IEnumerable<string>>())).Returns(false);
            var button = new ButtonAttribute("Name","foo") {Roles = new[] {"FOO"}};

            var result = html.Button(button);

            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ToString()));
        }

        /// <summary>
        ///A test for EnumDisplayFor
        ///</summary>
        [TestMethod]
        public void EnumDisplayForTest()
        {
            Enum defaultValue = TestEnum.Two;
            var actual = html.ZeusEnumDisplayFor(m=>m.Enum, defaultValue);
            Assert.IsNotNull(actual);
            Assert.AreEqual("2",actual.ToString());
        }

        /// <summary>
        ///A test for EnumDisplayFor
        ///</summary>
        [TestMethod]
        public void EnumDisplayForTest1()
        {
            var model = new TestModel {Value = "Test", Enum = TestEnum.Four};

            html.ViewData.Model = model;

            var actual = html.ZeusEnumDisplayFor(m => m.Enum);
            Assert.IsNotNull(actual);
            Assert.AreEqual("4", actual.ToString());
        }


        /// <summary>
        /// A test for EnumDropDownListFor
        /// </summary>
        [TestMethod]
        public void EnumDropDownListForTest()
        {
            var model = new TestModel { Value = "Test", Enum = TestEnum.Four };
            html.ViewData.Model = model;

            html.ViewContext.ViewData = new ViewDataDictionary(model) {{"3", new[] {"3"}}};
            html.ViewData.ModelMetadata.AdditionalValues["Attributes"] = new List<DisplayAttribute> { new DisplayAttribute() };

            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("attribute", "test");

            var actual = html.ZeusEnumDropDownListFor(m => m.Enum, TestEnum.Three, htmlAttributes);

//TestEnum drop down list is:-
//<select attribute="test" name="">
//<option value="One">1</option>
//<option value="Two">2</option>
//<option value="Three">3</option>
//<option selected="selected" value="Four">4</option>
//</select>

            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString();
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Assert.IsTrue(s.Contains("attribute=\"test\""));
            Assert.IsTrue(s.Contains("option value=\"One\""));
            Assert.IsTrue(s.Contains("option value=\"Two\""));
            Assert.IsTrue(s.Contains("option value=\"Three\""));
            Assert.IsTrue(s.Contains("<option selected=\"selected\" value=\"Four\">4</option>"));
        }

       
       /// <summary>
        ///A test for EnumDropDownListFor
        ///</summary>
        [TestMethod]
        public void EnumDropDownListForTest1()
        {
            var model = new TestModel { Value = "Test", Enum = TestEnum.Four };
            html.ViewData.Model = model;

            html.ViewContext.ViewData = new ViewDataDictionary(model) { { "3", new[] { "3" } } };
            html.ViewData.ModelMetadata.AdditionalValues["Attributes"] = new List<DisplayAttribute> { new DisplayAttribute() };

            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("attribute", "test");

            var actual = html.ZeusEnumDropDownListFor(m => m.Enum, htmlAttributes);

            //TestEnum drop down list is:-
            //<select attribute="test" name="">
            //<option value="One">1</option>
            //<option value="Two">2</option>
            //<option value="Three">3</option>
            //<option selected="selected" value="Four">4</option>
            //</select>

            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString();
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Assert.IsTrue(s.Contains("attribute=\"test\""));
            Assert.IsTrue(s.Contains("option value=\"One\""));
            Assert.IsTrue(s.Contains("option value=\"Two\""));
            Assert.IsTrue(s.Contains("option value=\"Three\""));
            Assert.IsTrue(s.Contains("<option selected=\"selected\" value=\"Four\">4</option>"));
        }

        /// <summary>
        ///A test for EnumDropDownListFor
        ///</summary>
        [TestMethod]
        public void EnumDropDownListForTest2()
        {
            var model = new TestModel { Value = "Test", Enum = TestEnum.Four };
            html.ViewData.Model = model;

            html.ViewContext.ViewData = new ViewDataDictionary(model) { { "3", new[] { "3" } } };
            html.ViewData.ModelMetadata.AdditionalValues["Attributes"] = new List<DisplayAttribute> { new DisplayAttribute() };


            var actual = html.ZeusEnumDropDownListFor(m => m.Enum, new { attribute = "test" });

            //TestEnum drop down list is:-
            //<select attribute="test" name="">
            //<option value="One">1</option>
            //<option value="Two">2</option>
            //<option value="Three">3</option>
            //<option selected="selected" value="Four">4</option>
            //</select>

            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString();
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Assert.IsTrue(s.Contains("attribute=\"test\""));
            Assert.IsTrue(s.Contains("option value=\"One\""));
            Assert.IsTrue(s.Contains("option value=\"Two\""));
            Assert.IsTrue(s.Contains("option value=\"Three\""));
            Assert.IsTrue(s.Contains("<option selected=\"selected\" value=\"Four\">4</option>"));
        }


        /// <summary>
        ///A test for EnumDropDownListFor
        ///</summary>
        [TestMethod]
        public void EnumDropDownListForTest4Helper()
        {
            var model = new TestModel { Value = "Test", Enum = TestEnum.Four };
            html.ViewData.Model = model;
            html.ViewData.ModelMetadata.AdditionalValues["Attributes"] = new List<DisplayAttribute> { new DisplayAttribute() };

            html.ViewContext.ViewData = new ViewDataDictionary(model) { { "3", new[] { "3" } } };

            var actual = html.ZeusEnumDropDownListFor(m => m.Enum);

            //TestEnum drop down list is:-
            //<select name="">
            //<option value="One">1</option>
            //<option value="Two">2</option>
            //<option value="Three">3</option>
            //<option selected="selected" value="Four">4</option>
            //</select>

            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString();
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Assert.IsTrue(s.Contains("option value=\"One\""));
            Assert.IsTrue(s.Contains("option value=\"Two\""));
            Assert.IsTrue(s.Contains("option value=\"Three\""));
            Assert.IsTrue(s.Contains("<option selected=\"selected\" value=\"Four\">4</option>"));
        }


        /// <summary>
        ///A test for GetHtmlAttributes
        ///</summary>
        //[TestMethod]
        //public void GetHtmlAttributesTest()
        //{
        //    var model = new TestModel { Value = "Test", Enum = TestEnum.Four };
        //    //html.ViewData.Model = model;

        //    System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();
        //    var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value");

        //    html.ViewContext.ViewData = new ViewDataDictionary(model);
        //    //html.ViewContext.ViewData.Model = model;
        //    html.ViewContext.ViewData.ModelMetadata = metadata;


        //    string readOnlyAttribute = string.Empty; 
        //    var a = HtmlHelperExtension.GetHtmlAttributes(html,readOnlyAttribute);
           
        //    Assert.IsNotNull(a);
        //}

        /// <summary>
        ///A test for GetHtmlAttributes
        ///</summary>
        [TestMethod]
        public void GetHtmlAttributesTest1()
        {
            //HtmlHelper html = null; // TODO: Initialize to an appropriate value
            //IDictionary<string, object> expected = null; // TODO: Initialize to an appropriate value
            //IDictionary<string, object> actual;
            //actual = HtmlHelperExtension.GetHtmlAttributes(html);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHtmlFieldPrefixUpOneLevel
        ///</summary>
        [TestMethod]
        public void GetHtmlFieldPrefixUpOneLevelTest()
        {
            string actual = html.GetHtmlFieldPrefixUpOneLevel();
            Assert.IsTrue(string.IsNullOrEmpty(actual));
        }

        /// <summary>
        /// Test rendering of history
        /// </summary>
        [TestMethod]
        public void ShowHistory()
        {
            viewData.Add(HistoryAttribute.ViewDataKey, HistoryType.JobSeeker);

            var actual = html.ShowHistory(HistoryType.JobSeeker);

            Assert.IsNotNull(actual);
            var s = actual.ToHtmlString();
            Assert.IsFalse(string.IsNullOrEmpty(s));
            Assert.IsTrue(s.Contains("div class=\"history\""));
            Assert.IsTrue(s.Contains(string.Format("<h2>Recently Accessed <span class=\"readers\">{0} Records</span></h2>",HistoryType.JobSeeker.GetDescription().ToUpperFirst() ) ));
            Assert.IsTrue(s.Contains("id=\"history\""));
            Assert.IsTrue(s.Contains("<li class=\"pinned\" " + HtmlDataType.ObjectValues + "=\"id=1\">"));
            Assert.IsTrue(s.Contains("<li " + HtmlDataType.ObjectValues + "=\"id=2\">"));
        }

        /// <summary>
        ///A test for ShowMenu
        ///</summary>
        //[TestMethod]
        //public void ShowMenuTest()
        //{
        //    var actual = html.ShowMenu(false);
        //    Assert.IsFalse(string.IsNullOrEmpty(actual.ToHtmlString()));
        //}

        /// <summary>
        ///A test for ShowMenu
        ///</summary>
        //[TestMethod]
        //public void ShowMenuTest1()
        //{
        //    HtmlHelper html = null; // TODO: Initialize to an appropriate value
        //    MvcHtmlString expected = null; // TODO: Initialize to an appropriate value
        //    MvcHtmlString actual;
        //    actual = HtmlHelperExtension.ShowMenu(html);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        /// Test result from Environment for 'WKS'.
        /// </summary>
        [TestMethod]
        public void EnvironmentWks()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("WKS");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroWks"));
            Assert.IsTrue(result.Contains("Local workstation"));
        }

        /// <summary>
        /// Test result from Environment for 'DEV'.
        /// </summary>
        [TestMethod]
        public void EnvironmentDev()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("DEV");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroDev"));
            Assert.IsTrue(result.Contains("Development environment"));
        }

        /// <summary>
        /// Test result from Environment for 'DEVFIX'.
        /// </summary>
        [TestMethod]
        public void EnvironmentDevFix()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("DEVFIX");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroDevFix"));
            Assert.IsTrue(result.Contains("Development fix environment"));
        }

        /// <summary>
        /// Test result from Environment for 'TEST'.
        /// </summary>
        [TestMethod]
        public void EnvironmentTest()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("TEST");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroTest"));
            Assert.IsTrue(result.Contains("Test environment"));
        }

        /// <summary>
        /// Test result from Environment for 'TESTFIX'.
        /// </summary>
        [TestMethod]
        public void EnvironmentTestFix()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("TESTFIX");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroTestFix"));
            Assert.IsTrue(result.Contains("Test fix environment"));
        }

        /// <summary>
        /// Test result from Environment for 'PREPROD'.
        /// </summary>
        [TestMethod]
        public void EnvironmentPreProd()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("PREPROD");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroPreProd"));
            Assert.IsTrue(result.Contains("Pre-Production environment"));
        }

        /// <summary>
        /// Test result from Environment for 'TRAIN'.
        /// </summary>
        [TestMethod]
        public void EnvironmentTrain()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("TRAIN");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("enviroTrain"));
            Assert.IsTrue(result.Contains("Training environment"));
        }

        /// <summary>
        /// Test result from Environment for 'PROD'.
        /// </summary>
        [TestMethod]
        public void EnvironmentProd()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("PROD");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        /// <summary>
        /// Test result from Environment for an unknown environment.
        /// </summary>
        [TestMethod]
        public void EnvironmentUnknown()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("FOO");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        /// <summary>
        /// Test result from Environment for no environment.
        /// </summary>
        [TestMethod]
        public void EnvironmentNone()
        {
            mockConfigurationManager.Setup(m => m.AppSettings.Get("Environment")).Returns("");


            HtmlHelper html = null;
            var result = HtmlHelperExtension.Environment(html).ToString();

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }
    }
}