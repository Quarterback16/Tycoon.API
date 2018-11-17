using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Interfaces.Geospatial;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using Employment.Web.Mvc.Zeus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Infrastructure.Interfaces.JobSeeker;

namespace Employment.Web.Mvc.Zeus.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="AjaxController" />.
    /// </summary>
    [TestClass]
    public class AjaxControllerTest
    {
        private AjaxController SystemUnderTest()
        {
            return new AjaxController(mockUserService.Object, mockAdwService.Object, mockAddressService.Object, mockJskSearchSearvice.Object);
        }
        /*
        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var adwMapper = new AdwMapper();
                    adwMapper.Map(Mapper.Configuration);

                    var stringMapper = new StringMapper();
                    stringMapper.Map(Mapper.Configuration);

                    var geospatialMapper = new GeospatialMapper();
                    geospatialMapper.Map(Mapper.Configuration);

                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }
        */
        private Mock<IUserService> mockUserService;
        //private Mock<IMappingEngine> mockMappingEngine;
        private Mock<IAdwService> mockAdwService;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IHistoryService> mockHistoryService;
        private Mock<IAddressService> mockAddressService;
        private Mock<IJobseekerSearchService> mockJskSearchSearvice;

        private IPageable<HistoryModel> history;
        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            //mockMappingEngine = new Mock<IMappingEngine>();
            mockAdwService = new Mock<IAdwService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockHistoryService = new Mock<IHistoryService>();
            mockAddressService = new Mock<IAddressService>();
            mockJskSearchSearvice = new Mock<IJobseekerSearchService>();

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            mockContainerProvider.Setup(m => m.GetService<IHistoryService>()).Returns(mockHistoryService.Object);
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            mockContainerProvider.Setup(m => m.GetService<IAddressService>()).Returns(mockAddressService.Object);
            mockContainerProvider.Setup(m => m.GetService<IJobseekerSearchService>()).Returns(mockJskSearchSearvice.Object);

            DependencyResolver.SetResolver(mockContainerProvider.Object);

            IList<CodeModel> codes = new List<CodeModel> { new CodeModel { Code = "Code", Description = "Description", ShortDescription = "ShortDescription", StartDate = DateTime.Now }, new CodeModel { Code = "Code2", Description = "Text", ShortDescription = "Text", StartDate = DateTime.Now } };
            IList<RelatedCodeModel> relatedCodes = new List<RelatedCodeModel>
                                                       {
                                                           new RelatedCodeModel { Dominant = true, SubordinateCode = "Code", SubordinateDescription = "Description", SubordinateShortDescription = "ShortDescription", StartDate = DateTime.Now },
                                                           new RelatedCodeModel { Dominant = true, SubordinateCode = "Code2", SubordinateDescription = "Text", SubordinateShortDescription = "Text", StartDate = DateTime.Now }
                                                       };

            mockAdwService.Setup(m => m.GetListCodes(It.IsAny<string>())).Returns(codes);
            mockAdwService.Setup(m => m.GetRelatedCodes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(relatedCodes);

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
                Values = new RouteValueDictionary {{"id","1"}},
                HistoryType = HistoryType.JobSeeker,
                DisplayName = "JobSeeker 1",
                DateAccessed = DateTime.Now,
                Username = "XX1234",
                IsPinned = true
            });

            history.Add(new HistoryModel()
            {
                Values = new RouteValueDictionary { { "id", "2" } },
                HistoryType = HistoryType.JobSeeker,
                DisplayName = "JobSeeker 2",
                DateAccessed = DateTime.Now.AddMinutes(1),
                Username = "XX1234",
                IsPinned = false
            });


            mockHistoryService.Setup(m => m.Pin(It.IsAny<HistoryType>(), It.IsAny<IDictionary<string, object>>()));
            mockHistoryService.Setup(m => m.Unpin(It.IsAny<HistoryType>(), It.IsAny<IDictionary<string,object>>()));
            mockHistoryService.Setup(m => m.Get(It.IsAny<HistoryType>())).Returns(history);

            mockUserService.Setup(m => m.History).Returns(mockHistoryService.Object);
        }

        /// <summary>
        /// Test ListCode with valid code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void ListCode_WithValidCode_ReturnsJsonResultWithCodes()
        {
            string code = "Code";
            var controller = SystemUnderTest();

            var result = controller.ListCode(string.Empty, 1, code, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            Assert.IsNotNull(result.Data);
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(d => d.Value == code));
        }

        /// <summary>
        /// Test ListCode with valid code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void ListCode_WithValidCodeAndExcludeValues_ReturnsJsonResultNotContainingExcludeValues()
        {
            string code = "Code";
            var controller = SystemUnderTest();
            var excludeValues = new [] {code};

            var result = controller.ListCode(string.Empty, 1, code, AdwOrderType.Default, AdwDisplayType.Default, excludeValues) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            Assert.IsNotNull(result.Data);
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsNotNull(results);
            Assert.IsFalse(results.Any(d => d.Value == code));
        }

        /// <summary>
        /// Test ListCode with empty code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void ListCode_WithEmptyCode_ReturnsJsonResultWithNoCodes()
        {
            string code = string.Empty;
            var controller = SystemUnderTest();

            var result = controller.ListCode(string.Empty, 1, code, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsFalse(results.Any());
        }

        /// <summary>
        /// Test ListCode with null code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void ListCode_WithNullCode_ReturnsJsonResultWithNoCodes()
        {
            string code = null;
            var controller = SystemUnderTest();

            var result = controller.ListCode(string.Empty, 1, code, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsFalse(results.Any());
        }

        /// <summary>
        /// Test RelatedCode with valid code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithValidCode_ReturnsJsonResultWithCodes()
        {
            string code = "Code";
            string dependentValue = "DependentValue";
            bool dominant = true;

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            Assert.IsNotNull(result.Data);
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(d => d.Value == code));
        }

        /// <summary>
        /// Test RelatedCode with valid code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithValidCodeAndExcludeValues_ReturnsJsonResultWithCodesNotContainingExcludeValues()
        {
            string code = "Code";
            string dependentValue = "DependentValue";
            bool dominant = true;
            string excludeCode = "Code2";
            var excludeValues = new[] { excludeCode };

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, excludeValues) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            Assert.IsNotNull(result.Data);
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(d => d.Value == code));
            Assert.IsFalse(results.Any(d => d.Value == excludeCode));
        }

        /// <summary>
        /// Test RelatedCode with empty code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithEmptyCode_ReturnsJsonResultWithNoCodes()
        {
            string code = string.Empty;
            string dependentValue = "DependentValue";
            bool dominant = true;

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsFalse(results.Any());
        }

        /// <summary>
        /// Test RelatedCode with null code returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithNullCode_ReturnsJsonResultWithNoCodes()
        {
            string code = null;
            string dependentValue = "DependentValue";
            bool dominant = true;

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsFalse(results.Any());
        }

        /// <summary>
        /// Test RelatedCode with empty dependent value returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithEmptyDependentValue_ReturnsJsonResultWithCodes()
        {
            string code = "Code";
            string dependentValue = string.Empty;
            bool dominant = true;

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsTrue(results.Any());
        }

        /// <summary>
        /// Test RelatedCode with null dependent value returns a JSON result.
        /// </summary>
        [TestMethod]
        public void RelatedCode_WithNullDependentValue_ReturnsJsonResultWithCodes()
        {
            string code = "Code";
            string dependentValue = null;
            bool dominant = true;

            var controller = SystemUnderTest();

            var result = controller.RelatedCode(string.Empty, 1, code, dependentValue, dominant, AdwOrderType.Default, AdwDisplayType.Default, null) as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            var results = TypeDescriptor.GetProperties(result.Data).Find("result", true).GetValue(result.Data) as IEnumerable<SelectListItem>;
            Assert.IsTrue(results.Any());
        }

        /// <summary>
        /// Test RelatedCode with null dependent value returns a JSON result.
        /// </summary>
        [TestMethod]
        public void PinHistory()
        {
            var controller = SystemUnderTest();
            var result = controller.PinHistory(HistoryType.JobSeeker, "id=1");
            Assert.IsNotNull(result, "ViewResult should not be null.");
        }

        /// <summary>
        /// Test RelatedCode with null dependent value returns a JSON result.
        /// </summary>
        [TestMethod]
        public void UnpinHistory()
        {
            var controller = SystemUnderTest();
            var result = controller.UnpinHistory(HistoryType.JobSeeker, "id=1");
            Assert.IsNotNull(result, "ViewResult should not be null.");
        }

        /// <summary>
        /// Test RelatedCode with null dependent value returns a JSON result.
        /// </summary>
        [TestMethod]
        public void HistoryNextPage()
        {
            var results = new[] { 
                new HistoryModel { DateAccessed = DateTime.Now, IsPinned = false, Values = new RouteValueDictionary {{"id","123"}},  DisplayName = "Foo", HistoryType = HistoryType.JobSeeker },
                new HistoryModel { DateAccessed = DateTime.Now, IsPinned = false, Values = new RouteValueDictionary {{"id", "456"}},  DisplayName = "Bar", HistoryType = HistoryType.JobSeeker } 
            };
            var metadata = new HistoryPageMetadata
                               {
                                   HistoryType = HistoryType.JobSeeker,
                                   PageNumber = 1,
                                   PageSize = 1,
                                   Total = 2
                               };

            var getResults = new Pageable<HistoryModel>(metadata);

            getResults.AddRange(results);

            //mockMappingEngine.Setup(m => m.Map<IEnumerable<HistoryModel>, IPageable<HistoryModel>>(It.IsAny<IEnumerable<HistoryModel>>(), It.IsAny<IPageable<HistoryModel>>())).Returns(getResults);

            var controller = SystemUnderTest();
            var result = controller.HistoryNextPage(metadata);
            ViewResult viewResult = result as ViewResult;
            IPageable<HistoryModel> model = viewResult.Model as IPageable<HistoryModel>;

            Assert.IsNotNull(viewResult, "ViewResult should not be null.");
            Assert.IsNotNull(model, "model should not be null.");
            Assert.IsTrue(model.Count() == 2);
            Assert.IsTrue(model.Metadata is HistoryPageMetadata);
            Assert.AreEqual(((HistoryPageMetadata)model.Metadata).HistoryType, HistoryType.JobSeeker);
            Assert.AreEqual(model.Metadata.PageNumber, 1);
            Assert.AreEqual(model.Metadata.PageSize, 1);
            Assert.AreEqual(model.Metadata.Total, 2);
        }

        [TestMethod]
        public void AddressSearch()
        {
            var addresses = new[] {new AddressModel {Line1 = "14 Mort Street", Locality = "Braddon", State = "ACT", Postcode = "2601", Reliability = (AddressReliability)1}};
            var addressViewModel = new AjaxAddressViewModel() { Line1 = "14 Mort Street", Locality = "Braddon", State = "ACT", Postcode = "2601", Reliability = (AddressReliability)1 };
            var ajaxAdresses = new List<AjaxAddressViewModel>() { addressViewModel }; // MappingEngine.Map<IEnumerable<AddressModel>, IEnumerable<AjaxAddressViewModel>>(addresses);

            mockAddressService.Setup(s => s.Validate(It.IsAny<string>(), false)).Returns(addresses);
            //mockMappingEngine.Setup(m => m.Map<IEnumerable<AddressModel>, IEnumerable<AjaxAddressViewModel>>(It.IsAny<IEnumerable<AddressModel>>())).Returns(ajaxAdresses);
            
            var controller = SystemUnderTest();
            var result = controller.AddressSearch("test", 0, false);
            var json = result as JsonResult;

            Assert.IsNotNull(result, "JsonResult should not be null.");
            Assert.IsNotNull(json);

            var data = TypeDescriptor.GetProperties(json.Data).Find("result", true).GetValue(json.Data) as IEnumerable<AjaxAddressViewModel>;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
            Assert.IsTrue(data.First().Line1 == ajaxAdresses.First().Line1);
        }
    }
}