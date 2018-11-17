using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Employment.Esc.Payments.Contracts.DataContracts;
using Employment.Esc.Payments.Contracts.FaultContracts;
using Employment.Esc.Payments.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Service.Implementation.Payments;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Web.Mvc.Service.Interfaces.Payments;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    /// <summary>
    ///This is a test class for RateReductionsServiceTest and is intended
    ///to contain all RateReductionsServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class RateReductionsServiceTest
    {
        private RateReductionsService SystemUnderTest()
        {
            return new RateReductionsService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new RateReductionsMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }
                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IPaymentsReductions> mockRateReductionsWcf;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            mockRateReductionsWcf = new Mock<IPaymentsReductions>();
            mockClient.Setup(m => m.Create<IPaymentsReductions>("paymentsReductions.svc")).Returns(mockRateReductionsWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorCalledWithNullArgumentsThrowsArgumentNullException()
        {
            new RateReductionsService(null, null, null);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ServiceValidationException))]
        public void JrrrResultsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault{Details =new List<ValidationDetail>{new ValidationDetail{Key = "Key", Message = "Message"}}});

            var inModel = new RateReductionModel {SiteCode = "ABQZ", ContractType = "RJCP"};
            var request = MappingEngine.Map<JrrrResultsListRequest>(inModel);
            var response = new JrrrResultsListResponse { JrrrGroup = new List<JrrrGroup> {new JrrrGroup() {JobseekerId = 8141313709}}.ToArray()};

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JrrrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJrrrResults(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            SystemUnderTest().ListJrrrResults(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ServiceValidationException))]
        public void JrrrResultsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault {Message = "Exception"});

            var inModel = new RateReductionModel {SiteCode = "ABQZ", ContractType = "RJCP"};
            var request = MappingEngine.Map<JrrrResultsListRequest>(inModel);
            var response = new JrrrResultsListResponse { JrrrGroup = new List<JrrrGroup> { new JrrrGroup() { JobseekerId = 8141313709 } }.ToArray() };

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JrrrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJrrrResults(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            SystemUnderTest().ListJrrrResults(inModel);
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void JrrrResultsValidResults()
        {
            var inModel = new RateReductionModel { SiteCode = "ABQZ", ContractType = "RJCP", CreationUserId = ""};
            var request = MappingEngine.Map<JrrrResultsListRequest>(inModel);
            var response = new JrrrResultsListResponse()
            {
                ExecutionStatus = PaymentsExecutionStatus.Success,
                ExecutionMessage = string.Empty,
                MoreFlag = "N",
                JrrrGroup = new List<JrrrGroup> { new JrrrGroup() { JobseekerId = 8141313709 } }.ToArray()
            };

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JrrrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJrrrResults(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListJrrrResults(inModel);

            Assert.IsTrue(result.ListOfResults.Count() == outModel.ListOfResults.Count());
            Assert.IsTrue(result.ListOfResults.First().ReductionRateValue == outModel.ListOfResults.First().ReductionRateValue);
            mockMappingEngine.Verify(m => m.Map<JrrrResultsListRequest>(inModel), Times.Once());
            mockRateReductionsWcf.Verify(m => m.ListJrrrResults(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<RateReductionModel>(response), Times.Once());
        }


        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void JehrResultsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault{Details =new List<ValidationDetail>{new ValidationDetail{Key = "Key", Message = "Message"}}});

            var inModel = new RateReductionModel { SiteCode = "ABQZ", ContractType = "RJCP" };
            var request = MappingEngine.Map<JehrResultsListRequest>(inModel);
            var response = new JrrrResultsListResponse { JrrrGroup = new List<JrrrGroup> { new JrrrGroup() { JobseekerId = 8141313709 } }.ToArray() };

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JehrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJehrResults(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            SystemUnderTest().ListJehrResults(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void JehrResultsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new RateReductionModel { SiteCode = "ABQZ", ContractType = "RJCP" };
            var request = MappingEngine.Map<JehrResultsListRequest>(inModel);
            var response = new JehrResultsListResponse()
            {
                JehrResultsGroup =
                    new List<JehrResultsGroup> { new JehrResultsGroup() { JobseekerId = 8141313709 } }.ToArray()
            };

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JehrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJehrResults(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            SystemUnderTest().ListJehrResults(inModel);
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void JehrResultsValidResults()
        {
            var inModel = new RateReductionModel { SiteCode = "ABQZ", ContractType = "RJCP", CreationUserId = "" };
            var request = MappingEngine.Map<JehrResultsListRequest>(inModel);
            var response = new JehrResultsListResponse()
            {
                ExecutionStatus = PaymentsExecutionStatus.Success,
                ExecutionMessage = string.Empty,
                Moreflag = "N",
                JehrResultsGroup = new List<JehrResultsGroup> { new JehrResultsGroup() { JobseekerId = 8141313709 } }.ToArray()
            };

            var outModel = MappingEngine.Map<RateReductionModel>(response);

            mockMappingEngine.Setup(m => m.Map<JehrResultsListRequest>(inModel)).Returns(request);
            mockRateReductionsWcf.Setup(m => m.ListJehrResults(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<RateReductionModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListJehrResults(inModel);

            Assert.IsTrue(result.ListOfResults.Count() == outModel.ListOfResults.Count());
            Assert.IsTrue(result.ListOfResults.First().ReductionRateValue == outModel.ListOfResults.First().ReductionRateValue);
            mockMappingEngine.Verify(m => m.Map<JehrResultsListRequest>(inModel), Times.Once());
            mockRateReductionsWcf.Verify(m => m.ListJehrResults(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<RateReductionModel>(response), Times.Once());
        }
    }
}
