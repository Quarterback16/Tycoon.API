using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Esc.Shared.Contracts.Execution;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for ServiceExtensionTest and is intended
    ///to contain all ServiceExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ToServiceValidationException
        ///</summary>
        [TestMethod]
        public void ToServiceValidationExceptionTest()
        {
            FaultException<ValidationFault> vf = new FaultException<ValidationFault>(new ValidationFault());
            vf.Detail.Add(new ValidationDetail {Key="Key",Message="Message",Tag="Tag" });
            ServiceValidationException actual = ServiceExtension.ToServiceValidationException(vf);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.ContainsKey("Key"));
            Assert.IsTrue(actual.Errors.Count == 1);
        }

        [TestMethod]
        public void ToServiceValidationExceptionTest1()
        {
            FaultException<ValidationFault> vf = new FaultException<ValidationFault>(new ValidationFault());
            vf.Detail.Add(new ValidationDetail { Key = string.Empty, Message = "Message", Tag = "Tag" });
            ServiceValidationException actual = ServiceExtension.ToServiceValidationException(vf);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count == 1);
            Assert.IsTrue(actual.Errors.ContainsKey("0"));
        }


        /// <summary>
        ///A test for ToServiceValidationException
        ///</summary>
        [TestMethod]
        public void ToServiceFaultExceptionTest()
        {
            var vf = new FaultException();
            
            ServiceValidationException actual = ServiceExtension.ToServiceValidationException(vf);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count == 1);
            Assert.AreEqual("Employment.Web.Mvc.Infrastructure.Exceptions.ServiceValidationException: The creator of this fault did not specify a Reason.", actual.ToString());
        }


        /// <summary>
        ///A test for Validate
        ///</summary>
        [TestMethod()]
        public void ValidateTest()
        {
            var response = new TestResponse();
            var e = new ExecutionResult();
            e.Status = ExecuteStatus.Failed;
            e.ExecuteMessages.Add(new ExecutionMessage { Help="Test Help", Text="Test Fail", Id=1, Type=ExecutionMessageType.Error});
            response.ExecutionResult = e;

            try
            {
                ServiceExtension.Validate(response);
            }
            catch (ServiceValidationException ex)
            {
                Assert.IsNotNull(ex);
                Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
                Assert.AreEqual("Test Fail",ex.Message);
                Assert.IsNotNull(ex.Errors);
                return;
            }

            Assert.Fail();
        }

        /// <summary>
        ///A test for Validate
        ///</summary>
        [TestMethod()]
        public void ValidateTest1()
        {
            var response = new TestResponse();
            var e = new ExecutionResult();
            e.Status = ExecuteStatus.Failed;
            e.ExecuteMessages.Add(new ExecutionMessage { Help = string.Empty, Text = "Test Fail", Id = 1, Type = ExecutionMessageType.Error });
            response.ExecutionResult = e;

            try
            {
                ServiceExtension.Validate(response);
            }
            catch (ServiceValidationException ex)
            {
                Assert.IsNotNull(ex);
                Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
                Assert.AreEqual("Test Fail", ex.Message);
                Assert.IsNotNull(ex.Errors);
                return;
            }

            Assert.Fail();
        }


        private class TestResponse : IResponseWithExecutionResult
        {
            public ExecutionResult ExecutionResult { get; set; }
        }
    }
}