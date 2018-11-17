using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.Models
{
    
    
    /// <summary>
    ///This is a test class for RelatedCodeModelTest and is intended
    ///to contain all RelatedCodeModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RelatedCodeModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

      
        /// <summary>
        ///A test for RelatedCodeModel Constructor
        ///</summary>
        [TestMethod()]
        public void RelatedCodeModelConstructorTest()
        {
            var target = new RelatedCodeModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Dominant
        ///</summary>
        [TestMethod()]
        public void AllRelatedCodeModelTest()
        {
            var target = new RelatedCodeModel();
            Assert.IsNotNull(target);

            target.Dominant = true;
            target.DominantCode = "DomCode";
            target.DominantDescription = "DomDesc";
            target.DominantShortDescription = "DomShortDesc";
            target.EndDate = new DateTime(2012,1,10);
            target.Position = 101;
            target.RelatedCode = "RelCode";
            target.StartDate = new DateTime(2012,1,1);
            target.SubordinateCode = "Subcode";
            target.SubordinateDescription = "subdesc";
            target.SubordinateShortDescription = "subshortdesc";

            Assert.AreEqual(true,target.Dominant);
            Assert.AreEqual("DomCode",target.DominantCode);
            Assert.AreEqual("DomDesc",target.DominantDescription);
            Assert.AreEqual("DomShortDesc",target.DominantShortDescription);
            Assert.AreEqual(new DateTime(2012,1,10),target.EndDate);
            Assert.AreEqual(101,target.Position);
            Assert.AreEqual("RelCode",target.RelatedCode);
            Assert.AreEqual(new DateTime(2012,1,1),target.StartDate);
            Assert.AreEqual("Subcode",target.SubordinateCode);
            Assert.AreEqual("subdesc",target.SubordinateDescription);
            Assert.AreEqual("subshortdesc",target.SubordinateShortDescription);
        }
    }
}