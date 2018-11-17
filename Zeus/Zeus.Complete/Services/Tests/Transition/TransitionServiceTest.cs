using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Transition.Contracts.DataContracts.Rjcp;
using Employment.Esc.Transition.Contracts.ServiceContracts.Rjcp;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Service.Implementation.Transition;
using Employment.Web.Mvc.Service.Interfaces.Transition;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessFault = Employment.Esc.Transition.Contracts.FaultContracts.BusinessFault;
using Employment.Web.Mvc.Service.Interfaces.Diary;

namespace Employment.Web.Mvc.Service.Tests.Transition
{
    /// <summary>
    /// Summary description for TransitionsServiceTest
    /// </summary>
    public class TransitionServiceTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        protected Mock<IClient> client;
        protected Mock<IMappingEngine> mappings;
        protected Mock<ICacheService> cacheService;
        protected Mock<IUserService> userService;
        protected Mock<IRjcpTransition> wcf;
        protected Mock<IDiaryAppointmentService> appointmentService;
        protected Mock<IDiaryAddressService> diaryAddressService;
        protected List<ExecutionMessage> messagesBeforeMap;
        protected List<ClientExecutionMessage> messagesMapped;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        protected static IMappingEngine MappingEngine { get; set; }

        protected static void InitTransitionMapper()
        {
            if (MappingEngine == null)
            {
                var mapper = new TransitionMapper();
                mapper.Map(Mapper.Configuration);
                MappingEngine = Mapper.Engine;
            }
        }
        /// <summary>
        /// to intercept instance methods calls to itself
        /// </summary>
        /// <returns></returns>
        private Mock<TransitionService> TransitionServiceMock()
        {
            return new Mock<TransitionService>(client.Object, mappings.Object, cacheService.Object, appointmentService.Object, diaryAddressService.Object);
        }

        private TransitionService TransitionService()
        {
            return new TransitionService(client.Object, mappings.Object, cacheService.Object, appointmentService.Object, diaryAddressService.Object);
        }

        public virtual void Initialize()
        {
            mappings = new Mock<IMappingEngine>(MockBehavior.Strict);
            cacheService = new Mock<ICacheService>();
            userService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(userService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            appointmentService = new Mock<IDiaryAppointmentService>();
            diaryAddressService = new Mock<IDiaryAddressService>();
            wcf = new Mock<IRjcpTransition>(MockBehavior.Strict);
            client = new Mock<IClient>();
            client.Setup(c => c.Create<IRjcpTransition>(It.Is<string>(s => s == "Transition.svc"))).Returns(wcf.Object);
            messagesBeforeMap = new List<ExecutionMessage>();
            messagesMapped = new List<ClientExecutionMessage>();
            mappings
                .Setup(m => m.Map<IEnumerable<ClientExecutionMessage>>(It.IsAny<IEnumerable<ExecutionMessage>>()))
                .Returns<IEnumerable<ExecutionMessage>>(items =>
                {
                    var mapped = items.Select(item => MappingEngine.Map<ClientExecutionMessage>(item));
                    messagesMapped.AddRange(mapped);
                    return mapped;
                })
                .Callback((IEnumerable<ExecutionMessage> items) => messagesBeforeMap.AddRange(items));
        }

        public virtual void Cleanup()
        {
        }

        #region GetProviderInformationMethod

        [TestClass]
        public class GetProviderInformationMethod : TransitionServiceTest
        {
            private ProviderInformationRequest searchRequest;
            private ProviderInformationSearchModel searchModel;
            private ProviderInformationModel cachedModel;
            private ProviderInformationResponse searchResponse;
            private ProviderInformationModel responseModel;

            // Use ClassInitialize to run code before running the first test in the class
            [ClassInitialize()]
            public static void ClassInitialize(TestContext testContext) { InitTransitionMapper(); }

            // Use ClassCleanup to run code after all tests in a class have run
            [ClassCleanup()]
            public static void ClassCleanup() { MappingEngine = null; }

            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                if (!TestContext.Properties.Contains("cached")) throw new ArgumentException("cached");
                bool cached = Boolean.Parse((string)TestContext.Properties["cached"]);

                string siteCode = TestContext.Properties.Contains("siteCode")
                                      ? (string)TestContext.Properties["siteCode"]
                                      : String.Empty;

                searchModel = new ProviderInformationSearchModel() { SiteCode = siteCode };
                searchRequest = MappingEngine.Map<ProviderInformationRequest>(searchModel);
                mappings.Setup(m => m.Map<ProviderInformationRequest>(searchModel)).Returns(searchRequest);
                mappings.Setup(m => m.Map<ProviderInformationModel>(It.IsAny<ProviderInformationModel>())).Returns<ProviderInformationModel>(m => m);

                cachedModel = cached ? new ProviderInformationModel() { NominalJobseekerList = new List<NominalJobseekerListItem>() } : null;
                var list = new List<ProviderInformationModel> { cachedModel };
                cacheService.Setup(c => c.TryGet(It.IsAny<KeyModel>(), out list)).Returns(cached);

                //setup wcf call
                if (!cached)
                {
                    searchResponse = new ProviderInformationResponse() { NominalJobseekers = new List<NominalJobseekerItem>().ToArray() };
                    responseModel = MappingEngine.Map<ProviderInformationModel>(searchResponse);
                    mappings.Setup(m => m.Map<ProviderInformationResponse, ProviderInformationModel>(searchResponse)).Returns(responseModel);
                }
            }

            [TestCleanup]
            public override void Cleanup()
            {
                searchRequest = null;
                searchModel = null;
                cachedModel = null;
                searchResponse = null;
                responseModel = null;
            }

            //[TestCategory("Cache Service"), TestMethod]
            //[TestProperty("siteCode", "T074")]
            //[TestProperty("cached", "true")]
            //public void UseCacheIfKeyExists()
            //{
            //    // arrange
            //    // act
            //    var actualModel = TransitionService().GetProviderInformation(searchModel);
            //    // assert
            //    client.Verify(c => c.Create<IRjcpTransition>(It.IsAny<string>()), Times.Never());
            //    Assert.IsNotNull(actualModel);
            //    Assert.AreSame(actualModel, cachedModel);
            //}

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            public void CallWcfIfNoKeyFound()
            {
                // arrange
                int calls = 0;
                wcf.Setup(w => w.GetRjcpProviderInformation(searchRequest)).Returns(searchResponse).Callback(
                    () => calls++).Verifiable();
                // act
                TransitionService().GetProviderInformation(searchModel);
                // assert
                wcf.VerifyAll();
                Assert.IsTrue(calls == 1);
            }

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            public void MapResponseAndReturnModel()
            {
                // arrange
                wcf.Setup(w => w.GetRjcpProviderInformation(searchRequest)).Returns(searchResponse);
                // act
                var actualModel = TransitionService().GetProviderInformation(searchModel);
                // assert
                Assert.IsNotNull(actualModel);
                Assert.AreSame(actualModel, responseModel);
            }

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            public void MapWcfResponseMessages()
            {
                // arrange
                var responseMessages = new[]
                                           {
                                               new ExecutionMessage(ExecutionMessageType.Information, "info1", ""),
                                               new ExecutionMessage(ExecutionMessageType.Information, "info2", ""),
                                               new ExecutionMessage(ExecutionMessageType.Warning, "warn1", "")
                                           };
                searchResponse.ExecuteMessages = responseMessages;
                // act
                // assert
                VerifyResponseMessages
                    (
                        w => w.GetRjcpProviderInformation(searchRequest),
                        ts => ts.GetProviderInformation(searchModel),
                        searchResponse, responseMessages, 2, 1
                    );
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceBusinessException))]
            public void ThrowExceptionOnMainframeError()
            {
                // arrange
                var fault = new BusinessFault(string.Format("{0} {1}", "cicsExecutionResultErrorCode", "cicsExecutionResultMessage"));
                var exception = new FaultException<BusinessFault>(fault);
                wcf.Setup(w => w.GetRjcpProviderInformation(searchRequest)).Throws(exception);

                try
                {
                    // act
                    TransitionService().GetProviderInformation(searchModel);
                }
                catch (Exception ex)
                {
                    // assert
                    Assert.AreSame(fault.Message, ex.Message);
                    throw;
                }
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceValidationException))]
            public void ThrowExceptionOnValidationFault()
            {
                // arrange
                var fault = new ValidationFault(new[] { new ValidationDetail { Message = "test" } });
                var exception = new FaultException<ValidationFault>(fault);
                wcf.Setup(w => w.GetRjcpProviderInformation(searchRequest)).Throws(exception);
                // act
                TransitionService().GetProviderInformation(searchModel);
                // assert
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("siteCode", "T074")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceValidationException))]
            public void ThrowExceptionOnFault()
            {
                // arrange
                wcf.Setup(w => w.GetRjcpProviderInformation(searchRequest)).Throws(new FaultException());
                // act
                TransitionService().GetProviderInformation(searchModel);
                // assert
            }
        }

        #endregion

        #region GetParticipantInformationByIdMethod

        [TestClass]
        public class GetParticipantInformationByIdMethod : TransitionServiceTest
        {
            private long jskrId;
            private TransitionJobseekerDetailsModel cachedModel;
            private TransitionJobseekerDetailsResponse searchResponse;
            private TransitionJobseekerDetailsModel responseModel;

            // Use ClassInitialize to run code before running the first test in the class
            [ClassInitialize()]
            public static void ClassInitialize(TestContext testContext) { InitTransitionMapper(); }

            // Use ClassCleanup to run code after all tests in a class have run
            [ClassCleanup()]
            public static void ClassCleanup() { MappingEngine = null; }

            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                if (!TestContext.Properties.Contains("cached")) throw new ArgumentException("cached");
                bool cached = Boolean.Parse((string)TestContext.Properties["cached"]);

                if (!TestContext.Properties.Contains("jskrId")) throw new ArgumentException("jskrId");
                jskrId = long.Parse((string)TestContext.Properties["jskrId"]);

                cachedModel = cached ? new TransitionJobseekerDetailsModel() : null;
                cacheService.Setup(c => c.TryGet(It.IsAny<KeyModel>(), out cachedModel)).Returns(cached);
                //setup wcf call
                if (!cached)
                {
                    searchResponse = new TransitionJobseekerDetailsResponse();
                    responseModel = MappingEngine.Map<TransitionJobseekerDetailsModel>(searchResponse);
                    mappings.Setup(m => m.Map<TransitionJobseekerDetailsResponse, TransitionJobseekerDetailsModel>(searchResponse)).Returns(responseModel);
                }
            }

            [TestCleanup]
            public override void Cleanup()
            {
                cachedModel = null;
                searchResponse = null;
                responseModel = null;
            }

            //[TestCategory("Cache Service"), TestMethod]
            //[TestProperty("jskrId", "123")]
            //[TestProperty("cached", "true")]
            //public void UseCacheIfKeyExists()
            //{
            //    // arrange
            //    // act
            //    var actualModel = TransitionService().GetParticipantInformation(jskrId);
            //    // assert
            //    client.Verify(c => c.Create<IRjcpTransition>(It.IsAny<string>()), Times.Never());
            //    Assert.IsNotNull(actualModel);
            //    Assert.AreSame(actualModel, cachedModel);
            //}

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            public void CallWcfIfNoKeyFound()
            {
                // arrange
                int calls = 0;
                wcf.Setup(w => w.GetRjcpTransitionJobseekerDetails(It.Is<TransitionJobseekerDetailsRequest>(r => r.JobseekerId == jskrId)))
                    .Returns(searchResponse)
                    .Callback(() => calls++)
                    .Verifiable();
                // act
                TransitionService().GetParticipantInformation(jskrId);
                // assert
                wcf.VerifyAll();
                Assert.IsTrue(calls == 1);
            }

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            public void MapResponseAndReturnModel()
            {
                // arrange
                wcf.Setup(w => w.GetRjcpTransitionJobseekerDetails(It.IsAny<TransitionJobseekerDetailsRequest>())).Returns(searchResponse);
                // act
                var actualModel = TransitionService().GetParticipantInformation(jskrId);
                // assert
                Assert.IsNotNull(actualModel);
                Assert.AreSame(actualModel, responseModel);
            }

            [TestCategory("WCF Response"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            public void MapWcfResponseMessages()
            {
                // arrange
                var responseMessages = new[]
                                           {
                                               new ExecutionMessage(ExecutionMessageType.Information, "info1", ""),
                                               new ExecutionMessage(ExecutionMessageType.Information, "info2", ""),
                                               new ExecutionMessage(ExecutionMessageType.Warning, "warn1", "")
                                           };
                searchResponse.ExecuteMessages = responseMessages;
                // act
                // assert
                VerifyResponseMessages
                    (
                        w => w.GetRjcpTransitionJobseekerDetails(It.IsAny<TransitionJobseekerDetailsRequest>()),
                        ts => ts.GetParticipantInformation(jskrId),
                        searchResponse, responseMessages, 2, 1
                    );
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceBusinessException))]
            public void ThrowExceptionOnMainframeError()
            {
                // arrange
                var fault = new BusinessFault(string.Format("{0} {1}", "cicsExecutionResultErrorCode", "cicsExecutionResultMessage"));
                var exception = new FaultException<BusinessFault>(fault);
                wcf.Setup(w => w.GetRjcpTransitionJobseekerDetails(It.IsAny<TransitionJobseekerDetailsRequest>())).Throws(exception);

                try
                {
                    // act
                    TransitionService().GetParticipantInformation(jskrId);
                }
                catch (Exception ex)
                {
                    // assert
                    Assert.AreSame(fault.Message, ex.Message);
                    throw;
                }
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceValidationException))]
            public void ThrowExceptionOnValidationFault()
            {
                // arrange
                var fault = new ValidationFault(new[] { new ValidationDetail { Message = "test" } });
                var exception = new FaultException<ValidationFault>(fault);
                wcf.Setup(w => w.GetRjcpTransitionJobseekerDetails(It.IsAny<TransitionJobseekerDetailsRequest>())).Throws(exception);
                // act
                TransitionService().GetParticipantInformation(jskrId);
                // assert
            }

            [TestCategory("WCF Fault"), TestMethod]
            [TestProperty("jskrId", "123")]
            [TestProperty("cached", "false")]
            [ExpectedException(typeof(ServiceValidationException))]
            public void ThrowExceptionOnFault()
            {
                // arrange
                wcf.Setup(w => w.GetRjcpTransitionJobseekerDetails(It.IsAny<TransitionJobseekerDetailsRequest>())).Throws(new FaultException());
                // act
                TransitionService().GetParticipantInformation(jskrId);
                // assert
            }
        }

        #endregion

        #region GetParticipantInformationByIdAndSearchMethod

        [TestClass]
        public class GetParticipantInformationByIdAndSearchMethod : TransitionServiceTest
        {
            private ProviderInformationSearchModel searchModel;
            long jskrId;

            // Use ClassInitialize to run code before running the first test in the class
            [ClassInitialize()]
            public static void ClassInitialize(TestContext testContext) { InitTransitionMapper(); }

            // Use ClassCleanup to run code after all tests in a class have run
            [ClassCleanup()]
            public static void ClassCleanup() { MappingEngine = null; }

            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();
                jskrId = 123;
                searchModel = new ProviderInformationSearchModel() { SiteCode = "" };
            }

            [TestCleanup]
            public override void Cleanup()
            {
                searchModel = null;
            }

            //[TestMethod]
            //[ExpectedException(typeof(NoClientResultException))]
            //public void DoNotLoadDetailsForMissedItem()
            //{
            //    //arrange
            //    //setup response for call to itself
            //    var list = new List<NominalJobseekerListItem>()
            //                   {
            //                       new NominalJobseekerListItem() {Id = 111} //jskrId is not in response
            //                   };
            //    var model = new ProviderInformationModel() { NominalJobseekerList = list };
            //    var serviceMock = TransitionServiceMock();
            //    serviceMock.Setup(s => s.GetProviderInformation(searchModel)).Returns(model).Verifiable();
            //    // act
            //    serviceMock.Object.GetParticipantInformation(jskrId);
            //    // assert
            //    serviceMock.VerifyAll();
            //}

            //[TestMethod]
            //public void LoadDetailsForItemInSearchResult()
            //{
            //    //arrange
            //    var list = new List<NominalJobseekerListItem>()
            //                   {
            //                       new NominalJobseekerListItem() {Id = 123} //jskrId is in response
            //                   };
            //    var model = new ProviderInformationModel() { NominalJobseekerList = list };
            //    var detailsModel = new TransitionJobseekerDetailsModel { };
            //    var serviceMock = TransitionServiceMock();
            //    //setup response for call to itself
            //    serviceMock.Setup(s => s.GetProviderInformation(searchModel)).Returns(model).Verifiable();
            //    serviceMock.Setup(s => s.GetParticipantInformation(It.Is<long>(id => id == jskrId))).Returns(detailsModel).Verifiable();
            //    //act
            //    serviceMock.Object.GetParticipantInformation(jskrId);
            //    //assert
            //    serviceMock.VerifyAll();
            //}
        }

        #endregion

        protected void VerifyResponseMessages<TResponse>
            (
            Expression<Func<IRjcpTransition, TResponse>> wcfcall,
            Func<TransitionService, IClientExecution> serviceCall,
            TResponse result, IEnumerable<ExecutionMessage> responseMessages, int infos, int warns
            )
        {
            //arrange
            wcf.Setup(wcfcall).Returns(result);
            // act
            IClientExecution actualModel = serviceCall(TransitionService());
            // assert
            Assert.IsNotNull(actualModel);
            Assert.IsTrue(messagesBeforeMap.Count == 3); //same response messages were sent to map
            foreach (var executionMessage in responseMessages)
            {
                var em = executionMessage;
                Assert.IsTrue(messagesBeforeMap.Exists(mbm => mbm == em));
            }
            //same mapped messages returned
            Assert.IsTrue(actualModel.Warnings.Count() == warns);
            Assert.IsTrue(actualModel.Infos.Count() == infos);
        }
    }
}
