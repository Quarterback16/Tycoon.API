using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.Example.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="DefaultController" />.
    /// </summary>
    [TestClass]
    public class DefaultControllerTest
    {
        private DefaultController SystemUnderTest()
        {
            return new DefaultController( mockUserService.Object, mockAdwService.Object);
        }

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var mapper = new ExampleMapper();
        //            mapper.Map(Mapper.Configuration);
        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        //private Mock<IMappingEngine> mockMappingEngine;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
           // mockMappingEngine = new Mock<IMappingEngine>();
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();
        }

        #region Index tests

        /// <summary>
        /// Test index returns a view result.
        /// </summary>
        [TestMethod]
        public void Index_WithGet_ReturnsViewResult()
        {
            var controller = SystemUnderTest();

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result, "ViewResult should not be null.");
        }

        #endregion
	}
}