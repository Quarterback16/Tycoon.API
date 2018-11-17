using System;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    /// <summary>
    /// Privides tests for the registration mapper service
    /// </summary>
    [TestClass]
    public class JobseekerModelTest
    {
        private JobseekerModel SystemUnderTest()
        {
            return new JobseekerModel();
        }


        /// <summary>
        /// Tests the get 
        /// </summary>
        [TestMethod]
        public void TestGetAge()
        {
            var jsModel = SystemUnderTest();
            jsModel.DateOfBirth = DateTime.Now.AddYears(-66);
            Assert.AreEqual(jsModel.GetAge(DateTime.Now),66);
        }

    }
}
