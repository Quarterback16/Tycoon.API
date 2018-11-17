using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Employment.Esc.Payments.Contracts.DataContracts;
using Employment.Esc.Payments.Contracts.FaultContracts;
using Employment.Esc.Payments.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Service.Implementation.Payments;
using Employment.Web.Mvc.Service.Interfaces.Payments.Overrides;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Web.Mvc.Service.Interfaces.Payments;
using Moq;
using IPayments = Employment.Esc.Payments.Contracts.ServiceContracts.IPayments;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    /// <summary>
    ///This is a test class for GeneralPaymentsServiceTest and is intended
    ///to contain all GeneralPaymentsServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class PaymentsServiceTest
    {
        private PaymentsService SystemUnderTest()
        {
            return new PaymentsService(mockOverridesService.Object, mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
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

        private static Mock<IContainerProvider> mockContainerProvider;
        private static Mock<IClient> mockClient;
        private static Mock<IMappingEngine> mockMappingEngine;
        private static Mock<ICacheService> mockCacheService;
        private static Mock<IUserService> mockUserService;
        private static Mock<IPayments> mockGeneralPaymentsWcf;
        private static Mock<IPaymentsHistory> mockPaymentHistoryWcf;
        private static Mock<IOverridesService> mockOverridesService;
        private static Mock<IPaymentsSitePaymentsAcquitals> mockAcquittalsService;

        //Use TestInitialize to run code before running each test
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            mockClient = new Mock<IClient>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            mockOverridesService = new Mock<IOverridesService>();
            mockGeneralPaymentsWcf = new Mock<IPayments>();
            mockPaymentHistoryWcf = new Mock<IPaymentsHistory>();
            mockAcquittalsService = new Mock<IPaymentsSitePaymentsAcquitals>();
            mockClient.Setup(m => m.Create<IPayments>("Payments.svc")).Returns(mockGeneralPaymentsWcf.Object);
            mockClient.Setup(m => m.Create<IPaymentsHistory>("PaymentsHistory.svc")).Returns(mockPaymentHistoryWcf.Object);
            mockClient.Setup(m => m.Create<IPaymentsSitePaymentsAcquitals>("PaymentsSitePaymentsAcquitals.svc")).Returns(mockAcquittalsService.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorCalledWithNullArgumentsThrowsArgumentNullException()
        {
            new PaymentsService(null, null, null, null);
        }

        #region General payments

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GeneralPaymentsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { ContractId = "0205885C" };
            var request = MappingEngine.Map<claimgeneralGetRequest>(inModel);
            var response = new claimgeneralGetResponse { OutGroup4Claim = new List<outGroup4Claim> { new outGroup4Claim { OutGrp4ClaimId = 12345678 } }.ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralGetRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralGetEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListGeneralPayments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ServiceValidationException))]
        public void GeneralPaymentsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault{Details =new List<ValidationDetail>{new ValidationDetail{Key = "Key", Message = "Message"}}});

            var inModel = new PaymentsModel {ContractId = "0205885C"};
            var request = MappingEngine.Map<claimgeneralGetRequest>(inModel);
            var response = new claimgeneralGetResponse { OutGroup4Claim = new List<outGroup4Claim> { new outGroup4Claim { OutGrp4ClaimId = 12345678 } }.ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralGetRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralGetEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListGeneralPayments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ServiceValidationException))]
        public void GeneralPaymentsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault {Message = "Exception"});

            var inModel = new PaymentsModel { ContractId = "0205885C" };
            var request = MappingEngine.Map<claimgeneralGetRequest>(inModel);
            var response = new claimgeneralGetResponse { OutGroup4Claim = new List<outGroup4Claim> { new outGroup4Claim { OutGrp4ClaimId = 12345678 } }.ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralGetRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralGetEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListGeneralPayments(inModel);
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GeneralPaymentsValidResults()
        {
            var inModel = new PaymentsModel { ContractId = "0205885C" };
            var request = MappingEngine.Map<claimgeneralGetRequest>(inModel);
            var response = new claimgeneralGetResponse { OutGroup4Claim = new List<outGroup4Claim> { new outGroup4Claim { OutGrp4ClaimId = 12345678 } }.ToArray() };
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralGetRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralGetEXECUTE(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListGeneralPayments(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(result.ListOfPayments.First().ClaimId == outModel.ListOfPayments.First().ClaimId);
            mockMappingEngine.Verify(m => m.Map<claimgeneralGetRequest>(inModel), Times.Once());
            mockGeneralPaymentsWcf.Verify(m => m.claimgeneralGetEXECUTE(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }
        #endregion

        #region Withdraw General Payment
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void WithdrawPaymentValidResults()
        {
            var inModel = new PaymentsModel { PaymentId = 439457997 };
            var request = MappingEngine.Map<claimgeneralWithdrawRequest>(inModel);
            var response = new claimgeneralWithdrawResponse();
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralWithdrawRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralWithdrawEXECUTE(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().WithdrawGeneralPayment(inModel);

            Assert.IsTrue(result.PaymentId == outModel.PaymentId);
            mockMappingEngine.Verify(m => m.Map<claimgeneralWithdrawRequest>(inModel), Times.Once());
            mockGeneralPaymentsWcf.Verify(m => m.claimgeneralWithdrawEXECUTE(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{TDetail}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { PaymentId = 439457997 };
            var request = MappingEngine.Map<claimgeneralWithdrawRequest>(inModel);
            var response = new claimgeneralWithdrawResponse();
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralWithdrawRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralWithdrawEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().WithdrawGeneralPayment(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("fault reason"), new FaultCode("fault code"));

            var inModel = new PaymentsModel { PaymentId = 439457997 };
            var request = MappingEngine.Map<claimgeneralWithdrawRequest>(inModel);
            var response = new claimgeneralWithdrawResponse();
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralWithdrawRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralWithdrawEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().WithdrawGeneralPayment(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void WithdrawPaymentThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Service validation exception" });

            var inModel = new PaymentsModel { PaymentId = 439457997 };
            var request = MappingEngine.Map<claimgeneralWithdrawRequest>(inModel);
            var response = new claimgeneralWithdrawResponse();
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<claimgeneralWithdrawRequest>(inModel)).Returns(request);
            mockGeneralPaymentsWcf.Setup(m => m.claimgeneralWithdrawEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().WithdrawGeneralPayment(inModel);
        }
        #endregion Payment History

        #region Payment details

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void PaymentDetailsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { PaymentId = 439457997, PaymentSeqNo = 0 };
            var request = MappingEngine.Map<ClmPaymentDtlGetRequest>(inModel);
            var response = new ClmPaymentDtlGetResponse { OutHistoryGroup = new List<OutHistoryGroup>().ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmPaymentDtlGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetails(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().GetPaymentDetails(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void PaymentDetailsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { PaymentId = 439457997, PaymentSeqNo = 0 };
            var request = MappingEngine.Map<ClmPaymentDtlGetRequest>(inModel);
            var response = new ClmPaymentDtlGetResponse { OutHistoryGroup = new List<OutHistoryGroup>().ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmPaymentDtlGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetails(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().GetPaymentDetails(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void PaymentDetailsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { PaymentId = 439457997, PaymentSeqNo = 0 };
            var request = MappingEngine.Map<ClmPaymentDtlGetRequest>(inModel);
            var response = new ClmPaymentDtlGetResponse { OutHistoryGroup = new List<OutHistoryGroup>().ToArray() };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmPaymentDtlGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetails(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().GetPaymentDetails(inModel);
        }

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void PaymentDetailsValidResults()
        {
            var inModel = new PaymentsModel { PaymentId = 439457997, PaymentSeqNo = 0 };
            var request = MappingEngine.Map<ClmPaymentDtlGetRequest>(inModel);
            var response = new ClmPaymentDtlGetResponse { OutHistoryGroup = new List<OutHistoryGroup> { new OutHistoryGroup { PchAmount = 12345678 } }.ToArray() };
            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmPaymentDtlGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetails(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().GetPaymentDetails(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(result.ListOfPayments.First().StatusCode == outModel.ListOfPayments.First().StatusCode );
            mockMappingEngine.Verify(m => m.Map<ClmPaymentDtlGetRequest>(inModel), Times.Once());
            mockPaymentHistoryWcf.Verify(m => m.GetPaymentDetails(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }
        #endregion

        #region Acquittals
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void AcquittalsValidResults()
        {
            var inModel = new PaymentsModel { PaymentId = 1234567890 };
            var request = MappingEngine.Map<ClmAcqReadRequest>(inModel);
            var response = new ClmAcqReadResponse
                               {
                                   OutGroupAcquittalList = new List<OutGroupAcquittalList>
                                           {
                                               new OutGroupAcquittalList
                                                   {
                                                       AcqAcquitalAmount = 110.99,
                                                       AcqAquittalDate = new DateTime(2012, 10, 10)
                                                   }
                                           }.ToArray()
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmAcqReadRequest>(inModel)).Returns(request);
            mockAcquittalsService.Setup(m => m.ClmAcqReadExecute(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListOffsets(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            mockMappingEngine.Verify(m => m.Map<ClmAcqReadRequest>(inModel), Times.Once());
            mockAcquittalsService.Verify(m => m.ClmAcqReadExecute(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AcquittalsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { PaymentId = 1234567890 };
            var request = MappingEngine.Map<ClmAcqReadRequest>(inModel);
            var response = new ClmAcqReadResponse
            {
                OutGroupAcquittalList = new List<OutGroupAcquittalList>
                                           {
                                               new OutGroupAcquittalList
                                                   {
                                                       AcqAcquitalAmount = 110.99,
                                                       AcqAquittalDate = new DateTime(2012, 10, 10)
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmAcqReadRequest>(inModel)).Returns(request);
            mockAcquittalsService.Setup(m => m.ClmAcqReadExecute(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListOffsets(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AcquittalsWcfThrowsFaultPaymentsFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { PaymentId = 1234567890 };
            var request = MappingEngine.Map<ClmAcqReadRequest>(inModel);
            var response = new ClmAcqReadResponse
            {
                OutGroupAcquittalList = new List<OutGroupAcquittalList>
                                           {
                                               new OutGroupAcquittalList
                                                   {
                                                       AcqAcquitalAmount = 110.99,
                                                       AcqAquittalDate = new DateTime(2012, 10, 10)
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmAcqReadRequest>(inModel)).Returns(request);
            mockAcquittalsService.Setup(m => m.ClmAcqReadExecute(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListOffsets(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AcquittalsWcfThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { PaymentId = 1234567890 };
            var request = MappingEngine.Map<ClmAcqReadRequest>(inModel);
            var response = new ClmAcqReadResponse
            {
                OutGroupAcquittalList = new List<OutGroupAcquittalList>
                                           {
                                               new OutGroupAcquittalList
                                                   {
                                                       AcqAcquitalAmount = 110.99,
                                                       AcqAquittalDate = new DateTime(2012, 10, 10)
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmAcqReadRequest>(inModel)).Returns(request);
            mockAcquittalsService.Setup(m => m.ClmAcqReadExecute(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListOffsets(inModel);
        }
        #endregion Acquittals

        #region Comments
        ///// <summary>
        ///// Test find runs successfully and returns expected results on valid use.
        ///// </summary>
        //[TestMethod]
        //public void CommentsValidResults()
        //{
        //    var inModel = new PaymentsModel { PaymentId = 4615912790, PaymentSeqNo = 1 };
        //    var request = MappingEngine.Map<ClmcommentGetRequest>(inModel);
        //    var response = new ClmcommentGetResponse
        //    {
        //        OutClmStatusItem = new List<OutClmStatusItem>
        //                                   {
        //                                       new OutClmStatusItem
        //                                           {
        //                                               CreationUserId = "EY2500",
        //                                               YpyStatusCode = "RECF"
        //                                           }
        //                                   }.ToArray()
        //    };

        //    var outModel = MappingEngine.Map<PaymentsModel>(response);

        //    mockMappingEngine.Setup(m => m.Map<ClmcommentGetRequest>(inModel)).Returns(request);
        //    mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetailsComments(request)).Returns(response);
        //    mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

        //    var result = SystemUnderTest().ListComments(inModel);

        //    Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
        //    mockMappingEngine.Verify(m => m.Map<ClmcommentGetRequest>(inModel), Times.Once());
        //    mockPaymentHistoryWcf.Verify(m => m.GetPaymentDetailsComments(request), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        //}

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CommentsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { PaymentId = 4615912790, PaymentSeqNo = 1 };
            var request = MappingEngine.Map<ClmcommentGetRequest>(inModel);
            var response = new ClmcommentGetResponse
            {
                OutClmStatusItem = new List<OutClmStatusItem>
                                           {
                                               new OutClmStatusItem
                                                   {
                                                       CreationUserId = "EY2500",
                                                       YpyStatusCode = "RECF"
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmcommentGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetailsComments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListComments(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CommentsWcfThrowsFaultPaymentsFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { PaymentId = 4615912790, PaymentSeqNo = 1 };
            var request = MappingEngine.Map<ClmcommentGetRequest>(inModel);
            var response = new ClmcommentGetResponse
            {
                OutClmStatusItem = new List<OutClmStatusItem>
                                           {
                                               new OutClmStatusItem
                                                   {
                                                       CreationUserId = "EY2500",
                                                       YpyStatusCode = "RECF"
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmcommentGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetailsComments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListComments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CommentsWcfThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { PaymentId = 4615912790, PaymentSeqNo = 1 };
            var request = MappingEngine.Map<ClmcommentGetRequest>(inModel);
            var response = new ClmcommentGetResponse
            {
                OutClmStatusItem = new List<OutClmStatusItem>
                                           {
                                               new OutClmStatusItem
                                                   {
                                                       CreationUserId = "EY2500",
                                                       YpyStatusCode = "RECF"
                                                   }
                                           }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmcommentGetRequest>(inModel)).Returns(request);
            mockPaymentHistoryWcf.Setup(m => m.GetPaymentDetailsComments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListComments(inModel);
        }
        #endregion Comments
    }
}
