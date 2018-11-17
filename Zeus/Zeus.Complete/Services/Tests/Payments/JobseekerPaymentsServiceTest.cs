using Moq;
using System;
using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using System.ServiceModel;
using System.Collections.Generic;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Payments;
using Employment.Esc.Payments.Contracts.DataContracts;
using Employment.Esc.Payments.Contracts.FaultContracts;
using Employment.Esc.Payments.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.Payments;
using Employment.Esc.SystemOverrides.Contracts.FaultContracts;
using Employment.Web.Mvc.Service.Interfaces.Payments.Overrides;
using Employment.Esc.Payments.Contracts.DataContracts.OutcomeSnapshot;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Employment.Esc.Payments.Contracts.DataContracts.JobSeekerPayments.ServiceFee;
using Employment.Esc.Payments.Contracts.DataContracts.JobSeekerPayments.PaymentsHistory;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    [TestClass]
    public class JobseekerPaymentsServiceTest
    {
        private JobseekerPaymentsService SystemUnderTest()
        {
            return new JobseekerPaymentsService(mockOverridesService.Object, mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new PaymentsMapper();
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
        private Mock<IPaymentsOutcomes> mockOutcomeWcf;
        private Mock<IPaymentsHistory> mockPaymentsHistoryWcf;
        private Mock<IPayments> mockPaymentsWcf;
        private Mock<IJobseekerPayments> mockJskPaymentsWcf;
        private Mock<IOverridesService> mockOverridesService;
        private Mock<IServFeesWbpaAndSubsidyPayments> mockServFeesWbpaAndSubsidyPayments;

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
            mockOutcomeWcf = new Mock<IPaymentsOutcomes>();
            mockPaymentsHistoryWcf = new Mock<IPaymentsHistory>();
            mockPaymentsWcf = new Mock<IPayments>();
            mockJskPaymentsWcf = new Mock<IJobseekerPayments>();
            mockOverridesService = new Mock<IOverridesService>();
            mockServFeesWbpaAndSubsidyPayments = new Mock<IServFeesWbpaAndSubsidyPayments>();
            mockClient.Setup(m => m.Create<IPaymentsOutcomes>("PaymentsOutcomes.svc")).Returns(mockOutcomeWcf.Object);
            mockClient.Setup(m => m.Create<IPaymentsHistory>("PaymentsHistory.svc")).Returns(mockPaymentsHistoryWcf.Object);
            mockClient.Setup(m => m.Create<IPayments>("Payments.svc")).Returns(mockPaymentsWcf.Object);
            mockClient.Setup(m => m.Create<IJobseekerPayments>("JobseekerPayments.svc")).Returns(mockJskPaymentsWcf.Object);
            mockClient.Setup(m => m.Create<IServFeesWbpaAndSubsidyPayments>("PaymentsServFeesWbpaAndSubsidyPayments.svc")).Returns(mockServFeesWbpaAndSubsidyPayments.Object);            
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCalledWithNullArgumentsThrowsArgumentNullException()
        {
            new JobseekerPaymentsService(null, null, null, null);
        }
        #region Payment History
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void JobseekerPaymentHistoryValidResults()
        {
            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentHistoryRequest>(inModel);
            var response = new PaymentHistoryResponse
            {
                ListOfPayments = new[] { new ListOfPayments { ClaimId = 0987654321, Amount = 100 } }
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentHistoryRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListClaimedPayments(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().JobseekerPaymentHistory(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            mockMappingEngine.Verify(m => m.Map<PaymentHistoryRequest>(inModel), Times.Once());
            mockJskPaymentsWcf.Verify(m => m.ListClaimedPayments(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void JobseekerPaymentHistoryWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentHistoryRequest>(inModel);
            var response = new PaymentHistoryResponse
                               {
                                   ListOfPayments = new[] {new ListOfPayments {ClaimId = 0987654321, Amount = 100}}
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentHistoryRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListClaimedPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().JobseekerPaymentHistory(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void JobseekerPaymentHistoryThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentHistoryRequest>(inModel);
            var response = new PaymentHistoryResponse
                               {
                                   ListOfPayments = new[] {new ListOfPayments {ClaimId = 0987654321, Amount = 100}}
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentHistoryRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListClaimedPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().JobseekerPaymentHistory(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void JobseekerPaymentHistoryThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentHistoryRequest>(inModel);
            var response = new PaymentHistoryResponse
                               {
                                   ListOfPayments = new[] {new ListOfPayments {ClaimId = 0987654321, Amount = 100}}
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentHistoryRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListClaimedPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().JobseekerPaymentHistory(inModel);
        }
        #endregion Withdraw Payment
        #region Withdraw Payment
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void WithdrawPaymentValidResults()
        {
            var paymentId = 1234567890;
            var inModel = new PaymentsModel { PaymentId = paymentId };
            var request = MappingEngine.Map<ClmWithdrawUesRequest>(inModel);
            request.TextBlock = string.Empty;

            var response = new ClmWithdrawUesResponse
                               {
                                   ClaimId = paymentId
                               };

            mockMappingEngine.Setup(m => m.Map<ClmWithdrawUesRequest>(inModel)).Returns(request);
            mockPaymentsHistoryWcf.Setup(m => m.WithdrawPayment(request)).Returns(response);
            
            SystemUnderTest().WithdrawPayment(inModel);

            mockMappingEngine.Verify(m => m.Map<ClmWithdrawUesRequest>(inModel), Times.Once());
            mockPaymentsHistoryWcf.Verify(m => m.WithdrawPayment(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var paymentId = 1234567890;
            var inModel = new PaymentsModel { PaymentId = paymentId };
            var request = MappingEngine.Map<ClmWithdrawUesRequest>(inModel);
            
            mockMappingEngine.Setup(m => m.Map<ClmWithdrawUesRequest>(inModel)).Returns(request);
            mockPaymentsHistoryWcf.Setup(m => m.WithdrawPayment(request)).Throws(exception);
            
            SystemUnderTest().WithdrawPayment(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var paymentId = 1234567890;
            var inModel = new PaymentsModel { PaymentId = paymentId };
            var request = MappingEngine.Map<ClmWithdrawUesRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmWithdrawUesRequest>(inModel)).Returns(request);
            mockPaymentsHistoryWcf.Setup(m => m.WithdrawPayment(request)).Throws(exception);
            SystemUnderTest().WithdrawPayment(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var paymentId = 1234567890;
            var inModel = new PaymentsModel { PaymentId = paymentId };
            var request = MappingEngine.Map<ClmWithdrawUesRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmWithdrawUesRequest>(inModel)).Returns(request);
            mockPaymentsHistoryWcf.Setup(m => m.WithdrawPayment(request)).Throws(exception);
            SystemUnderTest().WithdrawPayment(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentThrowsFaultExceptionThrowsOverridesFault()
        {
            var exception = new FaultException<SystemOverridesFault>(new SystemOverridesFault { Message = "Exception" });

            var paymentId = 1234567890;
            var inModel = new PaymentsModel { PaymentId = paymentId };
            var request = MappingEngine.Map<ClmWithdrawUesRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmWithdrawUesRequest>(inModel)).Returns(request);
            mockPaymentsHistoryWcf.Setup(m => m.WithdrawPayment(request)).Throws(exception);
            SystemUnderTest().WithdrawPayment(inModel);
        }
        #endregion Payment History
        #region ServiceFees
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ListServiceFeesValidResults()
        {
            var jskId = 1234567890;
            var inModel = new JobseekerPaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ServiceFeesRequest>(inModel);
            var response = new ServiceFeesResponse
                               {
                                   OutReferralGroup = new[] {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   OutServiceClaimGroup = new[] {
                                                                  new OutServiceClaimGroup
                                                                      {
                                                                          ClaimAmount = 110.11,
                                                                          PaymentType = "ABCD",
                                                                          CsdSeqNum = 1234
                                                                      }
                                                              },
                                   OutEventGroup = new[] {
                                               new OutEventGroup
                                                   {
                                                       CseEventCd = "ABCD",
                                                       CseCreationDate = new DateTime(2012, 09, 28),
                                                       CseEventDt = new DateTime(2012, 09, 28)
                                                   }
                                           },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<JobseekerPaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ServiceFeesRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListServiceFees(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<JobseekerPaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListServiceFees(inModel);

            Assert.IsTrue(result.JobseekerId == outModel.JobseekerId);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            Assert.IsTrue(result.ListOfPayments.First().PaymentType == outModel.ListOfPayments.First().PaymentType);
            Assert.IsTrue(result.ListOfPayments.First().SchDtlSeqNum == outModel.ListOfPayments.First().SchDtlSeqNum);

            Assert.IsTrue(result.Referrals.Count() == outModel.Referrals.Count());
            Assert.IsTrue(result.Referrals.First().ContractId == outModel.Referrals.First().ContractId);
            Assert.IsTrue(result.Referrals.First().ReferralSequenceNumber == outModel.Referrals.First().ReferralSequenceNumber);
            Assert.IsTrue(result.Referrals.First().ContractType == outModel.Referrals.First().ContractType);

            Assert.IsTrue(result.JobseekerEvents.Count() == outModel.JobseekerEvents.Count());
            Assert.IsTrue(result.JobseekerEvents.First().EventCode == outModel.JobseekerEvents.First().EventCode);
            Assert.IsTrue(result.JobseekerEvents.First().DateRecorded == outModel.JobseekerEvents.First().DateRecorded);
            Assert.IsTrue(result.JobseekerEvents.First().EventDate == outModel.JobseekerEvents.First().EventDate);

            mockMappingEngine.Verify(m => m.Map<ServiceFeesRequest>(inModel), Times.Once());
            mockJskPaymentsWcf.Verify(m => m.ListServiceFees(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<JobseekerPaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListServiceFeesWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var jskId = 1234567890;
            var inModel = new JobseekerPaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ServiceFeesRequest>(inModel);
            var response = new ServiceFeesResponse                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   OutServiceClaimGroup = new[]
                                                              {
                                                                  new OutServiceClaimGroup
                                                                      {
                                                                          ClaimAmount = 110.11,
                                                                          PaymentType = "ABCD",
                                                                          CsdSeqNum = 1234
                                                                      }
                                                              },
                                   OutEventGroup = new[]
                                                       {
                                                           new OutEventGroup
                                                               {
                                                                   CseEventCd = "ABCD",
                                                                   CseCreationDate = new DateTime(2012, 09, 28),
                                                                   CseEventDt = new DateTime(2012, 09, 28)
                                                               }
                                                       },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<JobseekerPaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ServiceFeesRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerPaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListServiceFees(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListServiceFeesThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var jskId = 1234567890;
            var inModel = new JobseekerPaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ServiceFeesRequest>(inModel);
            var response = new ServiceFeesResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   OutServiceClaimGroup = new[]
                                                              {
                                                                  new OutServiceClaimGroup
                                                                      {
                                                                          ClaimAmount = 110.11,
                                                                          PaymentType = "ABCD",
                                                                          CsdSeqNum = 1234
                                                                      }
                                                              },
                                   OutEventGroup = new[]
                                                       {
                                                           new OutEventGroup
                                                               {
                                                                   CseEventCd = "ABCD",
                                                                   CseCreationDate = new DateTime(2012, 09, 28),
                                                                   CseEventDt = new DateTime(2012, 09, 28)
                                                               }
                                                       },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<JobseekerPaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ServiceFeesRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerPaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListServiceFees(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListServiceFeesThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var jskId = 1234567890;
            var inModel = new JobseekerPaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ServiceFeesRequest>(inModel);
            var response = new ServiceFeesResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   OutServiceClaimGroup = new[]
                                                              {
                                                                  new OutServiceClaimGroup
                                                                      {
                                                                          ClaimAmount = 110.11,
                                                                          PaymentType = "ABCD",
                                                                          CsdSeqNum = 1234
                                                                      }
                                                              },
                                   OutEventGroup = new[]
                                                       {
                                                           new OutEventGroup
                                                               {
                                                                   CseEventCd = "ABCD",
                                                                   CseCreationDate = new DateTime(2012, 09, 28),
                                                                   CseEventDt = new DateTime(2012, 09, 28)
                                                               }
                                                       },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<JobseekerPaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ServiceFeesRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.ListServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerPaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListServiceFees(inModel);
        }
        #endregion ServiceFees
        #region Outcome snapshot
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ListSnapshotValidResults()
        {
            var jskId = 1234567890;
            var inModel = new OutcomesModel {JobseekerId = jskId};
            var request = MappingEngine.Map<SnapshotListRequest>(inModel);
            var response = new SnapshotListResponse
                               {
                                   JobseekerId = jskId,
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      CtyContractType = "ABC",
                                                                      ReferralDate = new DateTime(2012, 10, 10),
                                                                      ReferralSequenceNumber = 123
                                                                  }
                                                          },
                                   SnapGroup = new[]
                                                   {
                                                       new SnapGroup
                                                           {
                                                               OutcomeSnapId = 24531236,
                                                               OutcomeStartDate = new DateTime(2012, 10, 10)
                                                           }
                                                   }
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<SnapshotListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListJskSnapShots(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListSnapshot(inModel);

            Assert.IsTrue(result.ListOfSnapshots.Count() == outModel.ListOfSnapshots.Count());
            Assert.IsTrue((result.ListOfSnapshots.First().OutcomeSnapId == outModel.ListOfSnapshots.First().OutcomeSnapId));
            Assert.IsTrue((result.ListOfSnapshots.First().OutcomeStartDate == outModel.ListOfSnapshots.First().OutcomeStartDate));
            Assert.IsTrue((result.Referrals.First().ReferralDate == outModel.Referrals.First().ReferralDate));
            Assert.IsTrue((result.Referrals.First().ContractType == outModel.Referrals.First().ContractType));
            Assert.IsTrue((result.Referrals.First().ReferralSequenceNumber == outModel.Referrals.First().ReferralSequenceNumber));
            mockMappingEngine.Verify(m => m.Map<SnapshotListRequest>(inModel), Times.Once());
            mockOutcomeWcf.Verify(m => m.ListJskSnapShots(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<OutcomesModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListSnapshotWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            var jskId = 1234567890;

            var inModel = new OutcomesModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<SnapshotListRequest>(inModel);
            var response = new SnapshotListResponse
                               {
                                   JobseekerId = jskId,
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      CtyContractType = "ABC",
                                                                      ReferralDate = new DateTime(2012, 10, 10),
                                                                      ReferralSequenceNumber = 123
                                                                  }
                                                          },
                                   SnapGroup = new[]
                                                   {
                                                       new SnapGroup
                                                           {
                                                               OutcomeSnapId = 24531236,
                                                               OutcomeStartDate = new DateTime(2012, 10, 10)
                                                           }
                                                   }
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<SnapshotListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListJskSnapShots(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListSnapshot(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListSnapshotThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<SnapshotListRequest>(inModel);
            var response = new SnapshotListResponse
                               {
                                   JobseekerId = jskId,
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      CtyContractType = "ABC",
                                                                      ReferralDate = new DateTime(2012, 10, 10),
                                                                      ReferralSequenceNumber = 123
                                                                  }
                                                          },
                                   SnapGroup = new[]
                                                   {
                                                       new SnapGroup
                                                           {
                                                               OutcomeSnapId = 24531236,
                                                               OutcomeStartDate = new DateTime(2012, 10, 10)
                                                           }
                                                   }
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<SnapshotListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListJskSnapShots(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListSnapshot(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListSnapshotThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault {Message = "Exception"});

            var jskId = 1234567890;
            var inModel = new OutcomesModel {JobseekerId = jskId};
            var request = MappingEngine.Map<SnapshotListRequest>(inModel);
            var response = new SnapshotListResponse
                               {
                                   JobseekerId = jskId,
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      CtyContractType = "ABC",
                                                                      ReferralDate = new DateTime(2012, 10, 10),
                                                                      ReferralSequenceNumber = 123
                                                                  }
                                                          },
                                   SnapGroup = new[]
                                                   {
                                                       new SnapGroup
                                                           {
                                                               OutcomeSnapId = 24531236,
                                                               OutcomeStartDate = new DateTime(2012, 10, 10)
                                                           }
                                                   }
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<SnapshotListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListJskSnapShots(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListSnapshot(inModel);
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SnapshotDetailsValidResults()
        {
            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomeSnapGetRequest>(inModel);
            var response = new OutcomeSnapGetResponse
                               {
                                   OutcomeCd = "ABC",
                                   AllowanceType = "XYZ",
                                   CrpRefDate = new DateTime(2012, 10, 10),
                                   UeDays = 112,
                                   InfraNumber = "XXXX1212"
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomeSnapGetRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.GetJskSnapshot(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            var result = SystemUnderTest().SnapshotDetails(inModel);

            Assert.IsTrue(result.JobseekerDetails.OutcomeCode == outModel.JobseekerDetails.OutcomeCode);
            Assert.IsTrue(result.ReferralDate == outModel.ReferralDate);
            Assert.IsTrue(result.InfraNumber == outModel.InfraNumber);
            Assert.IsTrue(result.JobseekerDetails.AllowanceType == outModel.JobseekerDetails.AllowanceType);
            mockMappingEngine.Verify(m => m.Map<OutcomeSnapGetRequest>(inModel), Times.Once());
            mockOutcomeWcf.Verify(m => m.GetJskSnapshot(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<OutcomesModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SnapshotDetailsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail {Key = "Key", Message = "Message"}}});

            var jskId = 1234567890;
            var inModel = new OutcomesModel {JobseekerId = jskId};
            var request = MappingEngine.Map<OutcomeSnapGetRequest>(inModel);
            var response = new OutcomeSnapGetResponse
                               {
                                   OutcomeCd = "ABC",
                                   AllowanceType = "XYZ",
                                   CrpRefDate = new DateTime(2012, 10, 10),
                                   UeDays = 112,
                                   InfraNumber = "XXXX1212"
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomeSnapGetRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.GetJskSnapshot(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            mockMappingEngine.Setup(m => m.Map<OutcomeSnapGetRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.GetJskSnapshot(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().SnapshotDetails(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SnapshotDetailsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomeSnapGetRequest>(inModel);
            var response = new OutcomeSnapGetResponse
                               {
                                   OutcomeCd = "ABC",
                                   AllowanceType = "XYZ",
                                   CrpRefDate = new DateTime(2012, 10, 10),
                                   UeDays = 112,
                                   InfraNumber = "XXXX1212"
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomeSnapGetRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.GetJskSnapshot(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().SnapshotDetails(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SnapshotDetailsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomeSnapGetRequest>(inModel);
            var response = new OutcomeSnapGetResponse
                               {
                                   OutcomeCd = "ABC",
                                   AllowanceType = "XYZ",
                                   CrpRefDate = new DateTime(2012, 10, 10),
                                   UeDays = 112,
                                   InfraNumber = "XXXX1212"
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomeSnapGetRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.GetJskSnapshot(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().SnapshotDetails(inModel);
        }
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void UpdateSnapshotValidResults()
        {
            var jskDetails = new SnapshotJobseekerDetailsModel
                                 {
                                     OutcomeCode = "ABC",
                                     PartialWorkCapacity = "ABC",
                                     ParticipationRequirements = "NONE",
                                     Eligibility = "ELG",
                                     RegistrationStatus = "REG",
                                     AllowanceType = "ALL",
                                 };
            var inModel = new OutcomesModel
                              {
                                  SnapshotId = 1234567890,
                                  InfraNumber = "13323ZXZX",
                                  JobseekerDetails = jskDetails
                              };
            var request = MappingEngine.Map<UpdateSnapshotRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<UpdateSnapshotRequest>(inModel)).Returns(request);
            SystemUnderTest().UpdateSnapshot(inModel);
            mockMappingEngine.Verify(m => m.Map<UpdateSnapshotRequest>(inModel), Times.Once());
            mockJskPaymentsWcf.Verify(m => m.UpdateOutcomeSnapshot(request), Times.Once());
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void UpdateSnapshotValidResultsWithAdditionalDetails()
        {
            var jskDetails = new SnapshotJobseekerDetailsModel
                                 {
                                     OutcomeCode = "ABC",
                                     PartialWorkCapacity = "ABC",
                                     ParticipationRequirements = "NONE",
                                     Eligibility = "ELG",
                                     RegistrationStatus = "REG",
                                     AllowanceType = "ALL",
                                 };
            var additionalDetails = new SnapshotJobseekerDetailsModel
                                 {
                                     OutcomeCode = "ABC",
                                     PartialWorkCapacity = "ABC",
                                     ParticipationRequirements = "NONE",
                                     Eligibility = "ELG",
                                     RegistrationStatus = "REG",
                                     AllowanceType = "ALL",
                                     CarerLowHrs = true,
                                     ParentLowHrs = false
                                 };

            var inModel = new OutcomesModel
                              {
                                  SnapshotId = 1234567890,
                                  InfraNumber = "13323ZXZX",
                                  JobseekerDetails = jskDetails,
                                  JobseekerAdditionalDetails = additionalDetails
                              };
            var request = MappingEngine.Map<UpdateSnapshotRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<UpdateSnapshotRequest>(inModel)).Returns(request);
            SystemUnderTest().UpdateSnapshot(inModel);
            mockMappingEngine.Verify(m => m.Map<UpdateSnapshotRequest>(inModel), Times.Once());
            mockJskPaymentsWcf.Verify(m => m.UpdateOutcomeSnapshot(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateSnapshotWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new OutcomesModel
                              {
                                  SnapshotId = 1234567890,
                                  InfraNumber = "13323ZXZX",
                                  JobseekerDetails = new SnapshotJobseekerDetailsModel
                                                              {
                                                                  OutcomeCode = "ABC",
                                                                  PartialWorkCapacity = "ABC",
                                                                  ParticipationRequirements = "NONE",
                                                                  Eligibility = "ELG",
                                                                  RegistrationStatus = "REG",
                                                                  AllowanceType = "ALL",
                                                              }
                              };
            var request = MappingEngine.Map<UpdateSnapshotRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<UpdateSnapshotRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.UpdateOutcomeSnapshot(request)).Throws(exception);
            SystemUnderTest().UpdateSnapshot(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateSnapshotThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));


            var inModel = new OutcomesModel
                              {
                                  SnapshotId = 1234567890,
                                  InfraNumber = "13323ZXZX",
                                  JobseekerDetails = new SnapshotJobseekerDetailsModel
                                                              {
                                                                  OutcomeCode = "ABC",
                                                                  PartialWorkCapacity = "ABC",
                                                                  ParticipationRequirements = "NONE",
                                                                  Eligibility = "ELG",
                                                                  RegistrationStatus = "REG",
                                                                  AllowanceType = "ALL",
                                                              }
                              };
            var request = MappingEngine.Map<UpdateSnapshotRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<UpdateSnapshotRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.UpdateOutcomeSnapshot(request)).Throws(exception);
            SystemUnderTest().UpdateSnapshot(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateSnapshotThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new OutcomesModel
                              {
                                  SnapshotId = 1234567890,
                                  InfraNumber = "13323ZXZX",
                                  JobseekerDetails = new SnapshotJobseekerDetailsModel
                                                              {
                                                                  OutcomeCode = "ABC",
                                                                  PartialWorkCapacity = "ABC",
                                                                  ParticipationRequirements = "NONE",
                                                                  Eligibility = "ELG",
                                                                  RegistrationStatus = "REG",
                                                                  AllowanceType = "ALL",
                                                              }
                              };
            var request = MappingEngine.Map<UpdateSnapshotRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<UpdateSnapshotRequest>(inModel)).Returns(request);
            mockJskPaymentsWcf.Setup(m => m.UpdateOutcomeSnapshot(request)).Throws(exception);
            SystemUnderTest().UpdateSnapshot(inModel);
        }
        #endregion Outcome snapshot
        #region Assesments And EAF Payments
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ListAssesmentsAndEafPaymentsValidResults()
        {
            var jskId = 1234567890;
            var inModel = new PaymentsModel {JobseekerId = jskId};
            var request = MappingEngine.Map<ClmJskrExtpListRequest>(inModel);
            var response = new ClmJskrExtpListResponse
                               {
                                   JobseekerId = jskId,
                                   OutAvailGroup = new[]
                                                       {
                                                           new OutAvailGroup
                                                               {
                                                                   PaymentType = "DWSW",
                                                                   ItemAmount = 110.12,
                                                                   ItemId = 123456,
                                                                   ItemInvoiceNumber = "1231646"
                                                               }
                                                       }
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmJskrExtpListRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.ListEafPayments(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListAssesmentsAndEafPayments(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            Assert.IsTrue(result.ListOfPayments.First().ItemId == outModel.ListOfPayments.First().ItemId);
            Assert.IsTrue(result.ListOfPayments.First().PaymentType == outModel.ListOfPayments.First().PaymentType);
            Assert.IsTrue(result.ListOfPayments.First().InvoiceNumber == outModel.ListOfPayments.First().InvoiceNumber);
            mockMappingEngine.Verify(m => m.Map<ClmJskrExtpListRequest>(inModel), Times.Once());
            mockPaymentsWcf.Verify(m => m.ListEafPayments(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListAssesmentsAndEafPaymentsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var jskId = 1234567890;
            var inModel = new PaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ClmJskrExtpListRequest>(inModel);
            var response = new ClmJskrExtpListResponse
                               {
                                   JobseekerId = jskId,
                                   OutAvailGroup = new[]
                                                       {
                                                           new OutAvailGroup
                                                               {
                                                                   PaymentType = "DWSW",
                                                                   ItemAmount = 110.12,
                                                                   ItemId = 123456,
                                                                   ItemInvoiceNumber = "1231646"
                                                               }
                                                       }
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmJskrExtpListRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.ListEafPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListAssesmentsAndEafPayments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListAssesmentsAndEafPaymentsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var jskId = 1234567890;
            var inModel = new PaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ClmJskrExtpListRequest>(inModel);
            var response = new ClmJskrExtpListResponse
                               {
                                   JobseekerId = jskId,
                                   OutAvailGroup = new[]
                                                       {
                                                           new OutAvailGroup
                                                               {
                                                                   PaymentType = "DWSW",
                                                                   ItemAmount = 110.12,
                                                                   ItemId = 123456,
                                                                   ItemInvoiceNumber = "1231646"
                                                               }
                                                       }
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmJskrExtpListRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.ListEafPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().JobseekerPaymentHistory(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListAssesmentsAndEafPaymentsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var jskId = 1234567890;
            var inModel = new PaymentsModel { JobseekerId = jskId };
            var request = MappingEngine.Map<ClmJskrExtpListRequest>(inModel);
            var response = new ClmJskrExtpListResponse
                               {
                                   JobseekerId = jskId,
                                   OutAvailGroup = new[]
                                                       {
                                                           new OutAvailGroup
                                                               {
                                                                   PaymentType = "DWSW",
                                                                   ItemAmount = 110.12,
                                                                   ItemId = 123456,
                                                                   ItemInvoiceNumber = "1231646"
                                                               }
                                                       }
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmJskrExtpListRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.ListEafPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().JobseekerPaymentHistory(inModel);
        }
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void UpdateLastClaimDateValidResults()
        {
            var inModel = new PaymentsModel
                              {
                                  FromLastClaimDt = new DateTime(2012, 10, 10),
                                  ScheduleId = 1234656,
                                  ScheduleDetailIcn = 123,
                                  ScheduleDetailSeqNo = 5544
                              };
            var request = MappingEngine.Map<ClmExtpCsdUpdRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmExtpCsdUpdRequest>(inModel)).Returns(request);
            SystemUnderTest().UpdateLastClaimDate(inModel);
            mockMappingEngine.Verify(m => m.Map<ClmExtpCsdUpdRequest>(inModel), Times.Once());
            mockPaymentsWcf.Verify(m => m.UpdateEafPayments(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateLastClaimDateWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel
                              {
                                  FromLastClaimDt = new DateTime(2012, 10, 10),
                                  ScheduleId = 1234656,
                                  ScheduleDetailIcn = 123,
                                  ScheduleDetailSeqNo = 5544
                              };
            var request = MappingEngine.Map<ClmExtpCsdUpdRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmExtpCsdUpdRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.UpdateEafPayments(request)).Throws(exception);
            SystemUnderTest().UpdateLastClaimDate(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateLastClaimDateThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel
                              {
                                  FromLastClaimDt = new DateTime(2012, 10, 10),
                                  ScheduleId = 1234656,
                                  ScheduleDetailIcn = 123,
                                  ScheduleDetailSeqNo = 5544
                              };
            var request = MappingEngine.Map<ClmExtpCsdUpdRequest>(inModel);
            mockMappingEngine.Setup(m => m.Map<ClmExtpCsdUpdRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.UpdateEafPayments(request)).Throws(exception);
            SystemUnderTest().UpdateLastClaimDate(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateLastClaimDateThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel
                              {
                                  FromLastClaimDt = new DateTime(2012, 10, 10),
                                  ScheduleId = 1234656,
                                  ScheduleDetailIcn = 123,
                                  ScheduleDetailSeqNo = 5544
                              };
            var request = MappingEngine.Map<ClmExtpCsdUpdRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<ClmExtpCsdUpdRequest>(inModel)).Returns(request);
            mockPaymentsWcf.Setup(m => m.UpdateEafPayments(request)).Throws(exception);

            SystemUnderTest().UpdateLastClaimDate(inModel);
        }
        #endregion Assesments And EAF Payments
        #region Outcome claims
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ListOutcomeClaimsValidResults()
        {
            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomesListRequest>(inModel);
            var response = new OutcomesListResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   AvailableClaimGroup = new[]
                                                             {
                                                                 new AvailableClaimGroup
                                                                     {
                                                                         AmountRate = 110.11,
                                                                         PaymentType = "ABCD",
                                                                         SchDtlSeqNum = 1234
                                                                     }
                                                             },
                                   JrrrJehrDetailGroup = new[]
                                                             {
                                                                 new JrrrJehrDetailGroup
                                                                     {
                                                                         JehrRequestId = 1234,
                                                                         JehrCreationDate = new DateTime(2013, 07, 01)
                                                                     }
                                                             },
                                   AvailableClaimDetailGroup = new[]
                                                                   {
                                                                       new AvailableClaimDetailGroup
                                                                           {
                                                                               ActivityId = 1234,
                                                                               BusinessName = "ABCD",
                                                                               VacancyId = 1234564
                                                                           }
                                                                   },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomesListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListOutcomeClaims(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListOutcomeClaims(inModel, 0);

            Assert.IsTrue(result.JobseekerId == outModel.JobseekerId);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            Assert.IsTrue(result.ListOfPayments.First().PaymentType == outModel.ListOfPayments.First().PaymentType);
            Assert.IsTrue(result.ListOfPayments.First().SchDtlSeqNum == outModel.ListOfPayments.First().SchDtlSeqNum);

            Assert.IsTrue(result.Referrals.Count() == outModel.Referrals.Count());
            Assert.IsTrue(result.Referrals.First().ContractId == outModel.Referrals.First().ContractId);
            Assert.IsTrue(result.Referrals.First().ReferralSequenceNumber == outModel.Referrals.First().ReferralSequenceNumber);
            Assert.IsTrue(result.Referrals.First().ContractType == outModel.Referrals.First().ContractType);

            mockMappingEngine.Verify(m => m.Map<OutcomesListRequest>(inModel), Times.Once());
            mockOutcomeWcf.Verify(m => m.ListOutcomeClaims(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<OutcomesModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListOutcomeClaimsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomesListRequest>(inModel);
            var response = new OutcomesListResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   AvailableClaimGroup = new[]
                                                             {
                                                                 new AvailableClaimGroup
                                                                     {
                                                                         AmountRate = 110.11,
                                                                         PaymentType = "ABCD",
                                                                         SchDtlSeqNum = 1234
                                                                     }
                                                             },
                                   JrrrJehrDetailGroup = new[]
                                                             {
                                                                 new JrrrJehrDetailGroup
                                                                     {
                                                                         JehrRequestId = 1234,
                                                                         JehrCreationDate = new DateTime(2013, 07, 01)
                                                                     }
                                                             },
                                   AvailableClaimDetailGroup = new[]
                                                                   {
                                                                       new AvailableClaimDetailGroup
                                                                           {
                                                                               ActivityId = 1234,
                                                                               BusinessName = "ABCD",
                                                                               VacancyId = 1234564
                                                                           }
                                                                   },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomesListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListOutcomeClaims(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListOutcomeClaims(inModel, 0);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListOutcomeClaimsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomesListRequest>(inModel);
            var response = new OutcomesListResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   AvailableClaimGroup = new[]
                                                             {
                                                                 new AvailableClaimGroup
                                                                     {
                                                                         AmountRate = 110.11,
                                                                         PaymentType = "ABCD",
                                                                         SchDtlSeqNum = 1234
                                                                     }
                                                             },
                                   JrrrJehrDetailGroup = new[]
                                                             {
                                                                 new JrrrJehrDetailGroup
                                                                     {
                                                                         JehrRequestId = 1234,
                                                                         JehrCreationDate = new DateTime(2013, 07, 01)
                                                                     }
                                                             },
                                   AvailableClaimDetailGroup = new[]
                                                                   {
                                                                       new AvailableClaimDetailGroup
                                                                           {
                                                                               ActivityId = 1234,
                                                                               BusinessName = "ABCD",
                                                                               VacancyId = 1234564
                                                                           }
                                                                   },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomesListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListOutcomeClaims(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListServiceFees(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListOutcomeClaimsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var jskId = 1234567890;
            var inModel = new OutcomesModel { JobseekerId = jskId };
            var request = MappingEngine.Map<OutcomesListRequest>(inModel);
            var response = new OutcomesListResponse
                               {
                                   OutReferralGroup = new[]
                                                          {
                                                              new OutReferralGroup
                                                                  {
                                                                      ContractId = "1234567A",
                                                                      ReferralSequenceNumber = 1234,
                                                                      CtyContractType = "ABC"
                                                                  }
                                                          },
                                   AvailableClaimGroup = new[]
                                                             {
                                                                 new AvailableClaimGroup
                                                                     {
                                                                         AmountRate = 110.11,
                                                                         PaymentType = "ABCD",
                                                                         SchDtlSeqNum = 1234
                                                                     }
                                                             },
                                   JrrrJehrDetailGroup = new[]
                                                             {
                                                                 new JrrrJehrDetailGroup
                                                                     {
                                                                         JehrRequestId = 1234,
                                                                         JehrCreationDate = new DateTime(2013, 07, 01)
                                                                     }
                                                             },
                                   AvailableClaimDetailGroup = new[]
                                                                   {
                                                                       new AvailableClaimDetailGroup
                                                                           {
                                                                               ActivityId = 1234,
                                                                               BusinessName = "ABCD",
                                                                               VacancyId = 1234564
                                                                           }
                                                                   },
                                   JobseekerId = jskId
                               };

            var outModel = MappingEngine.Map<OutcomesModel>(response);

            mockMappingEngine.Setup(m => m.Map<OutcomesListRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.ListOutcomeClaims(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<OutcomesModel>(response)).Returns(outModel);

            SystemUnderTest().ListServiceFees(inModel);
        }
        #endregion Outcome claims
        #region Send JRRR/JEHR Request
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SendJrrrJehrRequestValidResults()
        {
            var inModel = new OutcomesModel
                              {
                                  JobseekerId = 1234567890,
                                  ContractType = "ABCD",
                                  SnapshotId = 1234567890
                              };
            var request = MappingEngine.Map<sendJehrJrrrRequestRequest>(inModel);
            var executionMessage = new List<ExecutionMessage>
                                       {
                                           new ExecutionMessage(ExecutionMessageType.Information,
                                                                "Transaction successful")
                                       };
            var response = new sendJehrJrrrRequestResponse
                               {
                                   ExecutionResult = new ExecutionResult
                                                         {
                                                             ExecuteMessages = executionMessage,
                                                             Status = ExecuteStatus.Success
                                                         }
                               };

            mockMappingEngine.Setup(m => m.Map<sendJehrJrrrRequestRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.sendJehrJrrrRequestEXECUTE(request)).Returns(response);

            SystemUnderTest().SendJrrrOrJehrRequest(inModel);

            mockMappingEngine.Verify(m => m.Map<sendJehrJrrrRequestRequest>(inModel), Times.Once());
            mockOutcomeWcf.Verify(m => m.sendJehrJrrrRequestEXECUTE(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SendJrrrJehrRequestWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail {Key = "Key", Message = "Message"}}});

            var inModel = new OutcomesModel
                              {
                                  JobseekerId = 1234567890
                              };
            var request = MappingEngine.Map<sendJehrJrrrRequestRequest>(inModel);
            
            mockMappingEngine.Setup(m => m.Map<sendJehrJrrrRequestRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.sendJehrJrrrRequestEXECUTE(request)).Throws(exception);
            SystemUnderTest().SendJrrrOrJehrRequest(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SendJrrrJehrRequestThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail {Key = "Key", Message = "Message"}}});

            var inModel = new OutcomesModel
                              {
                                  JobseekerId = 1234567890,
                                  ContractType = "ABCD",
                              };
            var request = MappingEngine.Map<sendJehrJrrrRequestRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<sendJehrJrrrRequestRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.sendJehrJrrrRequestEXECUTE(request)).Throws(exception);
            SystemUnderTest().SendJrrrOrJehrRequest(inModel);
        }
        #endregion Send JRRR/JEHR Request
    }
}
