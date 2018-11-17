using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Employment.Esc.Payments.Contracts.DataContracts;
using Employment.Esc.Payments.Contracts.DataContracts.Search;
using Employment.Esc.Payments.Contracts.FaultContracts;
using Employment.Esc.Payments.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Service.Implementation.Payments;
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
    ///This is a test class for PaymentsGeneralServiceTest and is intended
    ///to contain all PaymentsGeneralServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class PaymentsListAndLodgeServiceTest
    {
        private PaymentsListAndLodgeService SystemUnderTest()
        {
            return new PaymentsListAndLodgeService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
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
        private Mock<IPayments> mockPaymentsWcf;
        private Mock<IPaymentsOutcomes> mockOutcomeWcf;
        private Mock<IPaymentsHistory> mockPaymentsHistoryWcf;
        private Mock<IPaymentsSitePaymentsAcquitals> mockSitePaymentsAcquittalService;
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
            mockPaymentsWcf = new Mock<IPayments>();
            mockPaymentsHistoryWcf = new Mock<IPaymentsHistory>();
            mockOutcomeWcf = new Mock<IPaymentsOutcomes>();
            mockSitePaymentsAcquittalService = new Mock<IPaymentsSitePaymentsAcquitals>();
            mockServFeesWbpaAndSubsidyPayments = new Mock<IServFeesWbpaAndSubsidyPayments>();
            mockClient.Setup(m => m.Create<IPayments>("Payments.svc")).Returns(mockPaymentsWcf.Object);
            mockClient.Setup(m => m.Create<IPaymentsOutcomes>("PaymentsOutcomes.svc")).Returns(mockOutcomeWcf.Object);
            mockClient.Setup(m => m.Create<IPaymentsSitePaymentsAcquitals>("PaymentsSitePaymentsAcquitals.svc")).Returns(mockSitePaymentsAcquittalService.Object);
            mockClient.Setup(m => m.Create<IServFeesWbpaAndSubsidyPayments>("PaymentsServFeesWbpaAndSubsidyPayments.svc")).Returns(mockServFeesWbpaAndSubsidyPayments.Object);
            mockClient.Setup(m => m.Create<IPaymentsHistory>("PaymentsHistory.svc")).Returns(mockPaymentsHistoryWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCalledWithNullArgumentsThrowsArgumentNullException()
        {
            new PaymentsListAndLodgeService(null, null, null);
        }
        #region Payments search
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void PaymentsSearchGeneralValidResults()
        {
            var inModel = new PaymentsModel {JobseekerId = 1234567890};
            var request = MappingEngine.Map<PaymentsSearchRequest>(inModel);
            var response = new PaymentsSearchResponse
                               {
                                   GroupClaims = new List<GroupClaims> {new GroupClaims {PgcAmount = 110.99, JskrName = "Jsk Name"}}.ToArray()
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentsSearchRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.SearchPayments(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListPayments(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            mockMappingEngine.Verify(m => m.Map<PaymentsSearchRequest>(inModel), Times.Once());
            mockSitePaymentsAcquittalService.Verify(m => m.SearchPayments(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GeneralSearchWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentsSearchRequest>(inModel);
            var response = new PaymentsSearchResponse
                               {
                                   GroupClaims = new List<GroupClaims> {new GroupClaims {PgcAmount = 110.99, JskrName = "Jsk Name"}}.ToArray()
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentsSearchRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.SearchPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListPayments(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GeneralSearchWcfThrowsFaultPaymentsFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentsSearchRequest>(inModel);
            var response = new PaymentsSearchResponse
                               {
                                   GroupClaims = new List<GroupClaims> {new GroupClaims {PgcAmount = 110.99, JskrName = "Jsk Name"}}.ToArray()
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentsSearchRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.SearchPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListPayments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GeneralSearchWcfThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { JobseekerId = 1234567890 };
            var request = MappingEngine.Map<PaymentsSearchRequest>(inModel);
            var response = new PaymentsSearchResponse
            {
                GroupClaims = new List<GroupClaims> { new GroupClaims { PgcAmount = 110.99, JskrName = "Jsk Name" } }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<PaymentsSearchRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.SearchPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListPayments(inModel);
        }
        #endregion Payments search
        #region Site Payments
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SitePaymentsValidResults()
        {
            var inModel = new PaymentsModel {ContractType = "SSC", ContractId = "123456789", SiteCode = "ABCD", ClaimCategory = "SERF"};
            var request = MappingEngine.Map<ClmBulkLstRequest>(inModel);
            request.NextSchDtlRateCd = request.ListType = request.NextClmRqstCmyClaimRequestType = string.Empty;
            var response = new ClmBulkLstResponse
            {
                ExecutionMessage = String.Empty,
                MoreFlag = "N",
                OutSitePaymentList = new List<OutSitePaymentList> { new OutSitePaymentList { AmountRate = 110.99, CmyRateType = "Type 2" } }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.ListSitePayments(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ListSitePayments(inModel);

            Assert.IsTrue(result.ListOfPayments.Count() == outModel.ListOfPayments.Count());
            Assert.IsTrue(Math.Abs(result.ListOfPayments.First().Amount - outModel.ListOfPayments.First().Amount) < double.Epsilon);
            mockMappingEngine.Verify(m => m.Map<ClmBulkLstRequest>(inModel), Times.Once());
            mockSitePaymentsAcquittalService.Verify(m => m.ListSitePayments(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SitePaymentsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel { ContractType = "SSC", ContractId = "123456789", SiteCode = "ABCD", ClaimCategory = "SERF" };
            var request = MappingEngine.Map<ClmBulkLstRequest>(inModel);
            request.NextSchDtlRateCd = request.ListType = request.NextClmRqstCmyClaimRequestType = string.Empty;
            var response = new ClmBulkLstResponse
            {
                ExecutionMessage = string.Empty,
                MoreFlag = "N",
                OutSitePaymentList = new List<OutSitePaymentList> { new OutSitePaymentList { AmountRate = 110.99, CmyRateType = "Type 2" } }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.ListSitePayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListSitePayments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SitePaymentsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel { ContractType = "SSC", ContractId = "123456789", SiteCode = "ABCD", ClaimCategory = "SERF" };
            var request = MappingEngine.Map<ClmBulkLstRequest>(inModel);
            request.NextSchDtlRateCd = request.ListType = request.NextClmRqstCmyClaimRequestType = string.Empty;
            var response = new ClmBulkLstResponse
            {
                ExecutionMessage = string.Empty,
                MoreFlag = "N",
                OutSitePaymentList = new List<OutSitePaymentList> { new OutSitePaymentList { AmountRate = 110.99, CmyRateType = "Type 2" } }.ToArray()
            };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.ListSitePayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListSitePayments(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SitePaymentsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel { ContractType = "SSC", ContractId = "123456789", SiteCode = "ABCD", ClaimCategory = "SERF" };
            var request = MappingEngine.Map<ClmBulkLstRequest>(inModel);
            request.NextSchDtlRateCd = request.ListType = request.NextClmRqstCmyClaimRequestType = string.Empty;
            var response = new ClmBulkLstResponse
                               {
                                   ExecutionMessage = string.Empty,
                                   MoreFlag = "N",
                                   OutSitePaymentList = new List<OutSitePaymentList> {new OutSitePaymentList {AmountRate = 110.99, CmyRateType = "Type 2"}}.ToArray()
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.ListSitePayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ListSitePayments(inModel);
        }
        #endregion Site Payments
        #region TaxInvoice
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void LoadTaxInvoiceValidResults()
        {
            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  PaymentType = paymentType,
                                  ScheduleId = scheduleId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceCSDRequest>(inModel);
            var response = new ClmTaxInvoiceCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   PaymentType = paymentType,
                                   ItemId = 1234,
                                   ScheduleId = scheduleId,
                                   SchDtlSeqNum = csdSeqNo,
                                   UsersInvoiceNumber = invoiceNo,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.DesServiceFeesTaxInvoice(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateTaxInvoice(inModel);

            mockMappingEngine.Verify(m => m.Map<ClmTaxInvoiceCSDRequest>(inModel), Times.Once());
            mockServFeesWbpaAndSubsidyPayments.Verify(m => m.DesServiceFeesTaxInvoice(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadTaxInvoiceWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  PaymentType = paymentType,
                                  ScheduleId = scheduleId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceCSDRequest>(inModel);
            var response = new ClmTaxInvoiceCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   PaymentType = paymentType,
                                   ItemId = 1234,
                                   ScheduleId = scheduleId,
                                   SchDtlSeqNum = csdSeqNo,
                                   UsersInvoiceNumber = invoiceNo,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.DesServiceFeesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateTaxInvoice(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadTaxInvoiceThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  PaymentType = paymentType,
                                  ScheduleId = scheduleId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceCSDRequest>(inModel);
            var response = new ClmTaxInvoiceCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   PaymentType = paymentType,
                                   ItemId = 1234,
                                   ScheduleId = scheduleId,
                                   SchDtlSeqNum = csdSeqNo,
                                   UsersInvoiceNumber = invoiceNo,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.DesServiceFeesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateTaxInvoice(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadTaxInvoiceThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  PaymentType = paymentType,
                                  ScheduleId = scheduleId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceCSDRequest>(inModel);
            var response = new ClmTaxInvoiceCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   PaymentType = paymentType,
                                   ItemId = 1234,
                                   ScheduleId = scheduleId,
                                   SchDtlSeqNum = csdSeqNo,
                                   UsersInvoiceNumber = invoiceNo,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.DesServiceFeesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateTaxInvoice(inModel);
        }
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void LodgeClaimValidResults()
        {
            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var csdIcn = 1111;
            var comments = "Test comments";
            var contractType = "SSC";
            long claimId = 123456456;
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  Comments = comments,
                                  ScheduleDetailIcn = csdIcn,
                                  ContractType = contractType,
                                  PaymentType = paymentType,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmClaimAddCSDRequest>(inModel);
            var response = new ClmClaimAddCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CvcCtyContractTypeCode = contractType,
                                   CsdICN = csdIcn,
                                   PaymentType = paymentType,
                                   UsersInvoiceNumber = invoiceNo,
                                   ClaimId = claimId,
                                   ClaimICN = 1,
                                   ClaimSeqNumber = 1,
                                   SchDtlSeqNum = csdSeqNo,
                                   ScheduleId = scheduleId
                               };

            
            mockMappingEngine.Setup(m => m.Map<ClmClaimAddCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.LodgeDesServiceFees(request)).Returns(response);
            
            var result = SystemUnderTest().LodgeServiceFees(inModel);
            Assert.IsTrue(result == claimId);
            mockMappingEngine.Verify(m => m.Map<ClmClaimAddCSDRequest>(inModel), Times.Once());
            mockServFeesWbpaAndSubsidyPayments.Verify(m => m.LodgeDesServiceFees(request), Times.Once());
            //mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once()); TODO
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeClaimWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var csdIcn = 1111;
            var comments = "Test comments";
            var contractType = "SSC";
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  Comments = comments,
                                  ScheduleDetailIcn = csdIcn,
                                  ContractType = contractType,
                                  PaymentType = paymentType,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmClaimAddCSDRequest>(inModel);
            var response = new ClmClaimAddCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CvcCtyContractTypeCode = contractType,
                                   CsdICN = csdIcn,
                                   PaymentType = paymentType,
                                   UsersInvoiceNumber = invoiceNo,
                                   ClaimId = 123456456,
                                   ClaimICN = 1,
                                   ClaimSeqNumber = 1,
                                   SchDtlSeqNum = csdSeqNo,
                                   ScheduleId = scheduleId
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.LodgeDesServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeServiceFees(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeClaimThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var csdIcn = 1111;
            var comments = "Test comments";
            var contractType = "SSC";
            long claimId = 123456456;
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  Comments = comments,
                                  ScheduleDetailIcn = csdIcn,
                                  ContractType = contractType,
                                  PaymentType = paymentType,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmClaimAddCSDRequest>(inModel);
            var response = new ClmClaimAddCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CvcCtyContractTypeCode = contractType,
                                   CsdICN = csdIcn,
                                   PaymentType = paymentType,
                                   UsersInvoiceNumber = invoiceNo,
                                   ClaimId = claimId,
                                   ClaimICN = 1,
                                   ClaimSeqNumber = 1,
                                   SchDtlSeqNum = csdSeqNo,
                                   ScheduleId = scheduleId
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.LodgeDesServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeServiceFees(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeClaimThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var invoiceNo = "AAAAA";
            var csdIcn = 1111;
            var comments = "Test comments";
            var contractType = "SSC";
            long claimId = 123456456;
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  Comments = comments,
                                  ScheduleDetailIcn = csdIcn,
                                  ContractType = contractType,
                                  PaymentType = paymentType,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  InvoiceNumber = invoiceNo
                              };
            var request = MappingEngine.Map<ClmClaimAddCSDRequest>(inModel);
            var response = new ClmClaimAddCSDResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CvcCtyContractTypeCode = contractType,
                                   CsdICN = csdIcn,
                                   PaymentType = paymentType,
                                   UsersInvoiceNumber = invoiceNo,
                                   ClaimId = claimId,
                                   ClaimICN = 1,
                                   ClaimSeqNumber = 1,
                                   SchDtlSeqNum = csdSeqNo,
                                   ScheduleId = scheduleId
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddCSDRequest>(inModel)).Returns(request);
            mockServFeesWbpaAndSubsidyPayments.Setup(m => m.LodgeDesServiceFees(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeServiceFees(inModel);
        }
        #endregion TaxInvoice
        #region BulkTaxInvoice
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void LoadBulkTaxInvoicesValidResults()
        {
            var csdIcn = 1111;
            var jskId = 1234567890;
            var csdSeqNo = 0987654321;
            var scheduleId = 9517532846;
            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = csdIcn,
                                                                   JobseekerId = jskId,
                                                                   SchDtlSeqNum = csdSeqNo,
                                                                   SchDtlScheduleId = scheduleId
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstTaxInvRequest>(inModel);
            var response = new ClmBulkLstTaxInvResponse
                               {
                                   Amount = 100,
                                   GstAmount = 10,
                                   ExecutionMessage = string.Empty,
                                   ProviderId = 123456,
                                   ProviderName = "AAAAAAAAAAAAAAAA",
                                   AustralianCompanyNumber = "1231231311111"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstTaxInvRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkTaxInvoices(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateBulkInvoices(inModel);

            mockMappingEngine.Verify(m => m.Map<ClmBulkLstTaxInvRequest>(inModel), Times.Once());
            mockSitePaymentsAcquittalService.Verify(m => m.LodgeBulkTaxInvoices(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadBulkTaxInvoicesWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var csdIcn = 1111;
            var jskId = 1234567890;
            var csdSeqNo = 0987654321;
            var scheduleId = 9517532846;
            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = csdIcn,
                                                                   JobseekerId = jskId,
                                                                   SchDtlSeqNum = csdSeqNo,
                                                                   SchDtlScheduleId = scheduleId
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstTaxInvRequest>(inModel);
            var response = new ClmBulkLstTaxInvResponse
                               {
                                   Amount = 100,
                                   GstAmount = 10,
                                   ExecutionMessage = string.Empty,
                                   ProviderId = 123456,
                                   ProviderName = "AAAAAAAAAAAAAAAA",
                                   AustralianCompanyNumber = "1231231311111"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstTaxInvRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkTaxInvoices(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateBulkInvoices(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadBulkTaxInvoicesThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var csdIcn = 1111;
            var jskId = 1234567890;
            var csdSeqNo = 0987654321;
            var scheduleId = 9517532846;
            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = csdIcn,
                                                                   JobseekerId = jskId,
                                                                   SchDtlSeqNum = csdSeqNo,
                                                                   SchDtlScheduleId = scheduleId
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstTaxInvRequest>(inModel);
            var response = new ClmBulkLstTaxInvResponse
                               {
                                   Amount = 100,
                                   GstAmount = 10,
                                   ExecutionMessage = string.Empty,
                                   ProviderId = 123456,
                                   ProviderName = "AAAAAAAAAAAAAAAA",
                                   AustralianCompanyNumber = "1231231311111"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstTaxInvRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkTaxInvoices(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateBulkInvoices(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadBulkTaxInvoicesThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var csdIcn = 1111;
            var jskId = 1234567890;
            var csdSeqNo = 0987654321;
            var scheduleId = 9517532846;
            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = csdIcn,
                                                                   JobseekerId = jskId,
                                                                   SchDtlSeqNum = csdSeqNo,
                                                                   SchDtlScheduleId = scheduleId
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstTaxInvRequest>(inModel);
            var response = new ClmBulkLstTaxInvResponse
                               {
                                   Amount = 100,
                                   GstAmount = 10,
                                   ExecutionMessage = string.Empty,
                                   ProviderId = 123456,
                                   ProviderName = "AAAAAAAAAAAAAAAA",
                                   AustralianCompanyNumber = "1231231311111"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstTaxInvRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkTaxInvoices(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateBulkInvoices(inModel);
        }
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void LodgeBulkClaimsValidResults()
        {
            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  InvoiceNumber = "InvoiceNo",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = 1234,
                                                                   JobseekerId = 1234567890,
                                                                   SchDtlSeqNum = 123554,
                                                                   SchDtlScheduleId = 1245
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstLodgeRequest>(inModel);
            var response = new ClmBulkLstLodgeResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   ClaimId = 0987654321
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstLodgeRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkPayments(request)).Returns(response);
            //mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel.PaymentId);

            var result = SystemUnderTest().LodgeBulkClaims(inModel);

            Assert.IsTrue(result == outModel.PaymentId);
            mockMappingEngine.Verify(m => m.Map<ClmBulkLstLodgeRequest>(inModel), Times.Once());
            mockSitePaymentsAcquittalService.Verify(m => m.LodgeBulkPayments(request), Times.Once());
            //mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeBulkClaimsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  InvoiceNumber = "InvoiceNo",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = 1234,
                                                                   JobseekerId = 1234567890,
                                                                   SchDtlSeqNum = 123554,
                                                                   SchDtlScheduleId = 1245
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstLodgeRequest>(inModel);
            var response = new ClmBulkLstLodgeResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   ClaimId = 0987654321
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstLodgeRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeBulkClaims(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeBulkClaimsThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  InvoiceNumber = "InvoiceNo",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = 1234,
                                                                   JobseekerId = 1234567890,
                                                                   SchDtlSeqNum = 123554,
                                                                   SchDtlScheduleId = 1245
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstLodgeRequest>(inModel);
            var response = new ClmBulkLstLodgeResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   ClaimId = 0987654321
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstLodgeRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeBulkClaims(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeBulkClaimsThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var inModel = new PaymentsModel
                              {
                                  ContractType = "SSC",
                                  ClaimCategory = "SERF",
                                  InvoiceNumber = "InvoiceNo",
                                  ListOfPayments = new List<PaymentsItemModel>
                                                       {
                                                           new PaymentsItemModel
                                                               {
                                                                   ScheduleDetailIcn = 1234,
                                                                   JobseekerId = 1234567890,
                                                                   SchDtlSeqNum = 123554,
                                                                   SchDtlScheduleId = 1245
                                                               }
                                                       }
                              };
            var request = MappingEngine.Map<ClmBulkLstLodgeRequest>(inModel);
            var response = new ClmBulkLstLodgeResponse
                               {
                                   ExecutionMessage = String.Empty,
                                   ClaimId = 0987654321
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmBulkLstLodgeRequest>(inModel)).Returns(request);
            mockSitePaymentsAcquittalService.Setup(m => m.LodgeBulkPayments(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().LodgeBulkClaims(inModel);
        }
        #endregion BulkTaxInvoice
        #region Outcomes Tax Invoice
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        //[TestMethod]
        //public void LoadOutcomesTaxInvoiceValidResults()
        //{
        //    var amount = 110.10;
        //    var paymentType = "ABCD";
        //    var scheduleId = 1234;
        //    var csdSeqNo = 1454;
        //    var jskId = 1234567890;
        //    var snapId = 9874563210;
        //    var inModel = new PaymentsModel
        //                      {
        //                          Amount = amount,
        //                          ContractType = "SSC",
        //                          PaymentType = paymentType,
        //                          ScheduleId = scheduleId,
        //                          ScheduleDetailSeqNo = csdSeqNo,
        //                          JobseekerId = jskId,
        //                          SnapshotId = snapId,
        //                          MfawConfirmed = "N"
        //                      };

        //    var request = MappingEngine.Map<ClmOutcTaxInvoiceRequest>(inModel);

        //    var response = new ClmOutcTaxInvoiceResponse
        //                       {
        //                           ExecutionMessage = String.Empty,
        //                           Amount = amount,
        //                           GstAmount = Math.Round(amount/10, 2),
        //                           CmyRateType = paymentType,
        //                           ProviderId = 123456789,
        //                           ProviderName = "BBBBBBBBBBBBBBB",
        //                       };
            
        //    var outModel = MappingEngine.Map<PaymentsModel>(response);

        //    mockMappingEngine.Setup(m => m.Map<ClmOutcTaxInvoiceRequest>(inModel)).Returns(request);
        //    mockOutcomeWcf.Setup(m => m.clmOutcTaxInvoiceEXECUTE(request)).Returns(response);
        //    mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

        //    SystemUnderTest().ValidateOutcomesInvoice(inModel);

        //    mockMappingEngine.Verify(m => m.Map<ClmOutcTaxInvoiceRequest>(inModel), Times.Once());
        //    mockOutcomeWcf.Verify(m => m.clmOutcTaxInvoiceEXECUTE(request), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        //}

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadOutcomesTaxInvoiceWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var amount = 110.10;
            var paymentType = "ABCD";
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var jskId = 1234567890;
            var snapId = 9874563210;

            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  PaymentType = paymentType,
                                  ScheduleId = scheduleId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  JobseekerId = jskId,
                                  SnapshotId = snapId,
                                  MfawConfirmed = "N"
                              };

            var request = MappingEngine.Map<ClmOutcTaxInvoiceRequest>(inModel);

            var response = new ClmOutcTaxInvoiceResponse
                               {
                                   ExecutionMessage = "Serious error",
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CmyRateType = paymentType,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmOutcTaxInvoiceRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.clmOutcTaxInvoiceEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateOutcomesInvoice(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LoadOutcomesTaxInvoiceThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var amount = 110.10;
            var paymentType = "ABCD";
            
            var inModel = new PaymentsModel
                              {
                                  Amount = amount,
                                  ContractType = "SSC",
                                  
                              };

            var request = MappingEngine.Map<ClmOutcTaxInvoiceRequest>(inModel);

            var response = new ClmOutcTaxInvoiceResponse
                               {
                                   ExecutionMessage = "Serious error",
                                   Amount = amount,
                                   GstAmount = Math.Round(amount/10, 2),
                                   CmyRateType = paymentType,
                                   ProviderId = 123456789,
                                   ProviderName = "BBBBBBBBBBBBBBB",
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmOutcTaxInvoiceRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.clmOutcTaxInvoiceEXECUTE(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateOutcomesInvoice(inModel);
        }
        ///// <summary>
        ///// Test find runs successfully and returns expected results on valid use.
        ///// </summary>
        //[TestMethod]
        //public void LodgeOutcomesClaimValidResults()
        //{
        //    var jskId = 1234567890;
        //    var scheduleId = 1234;
        //    var csdSeqNo = 1454;
        //    var csdIcn = 1111;
        //    long claimId = 123456456;

        //    var inModel = new PaymentsModel
        //                      {
        //                          JobseekerId = jskId,
        //                          ScheduleDetailSeqNo = csdSeqNo,
        //                          ScheduleId = scheduleId,
        //                          MfawConfirmed = "Y",
        //                          ScheduleDetailIcn = csdIcn
        //                      };
        //    var request = MappingEngine.Map<ClmClaimAddOutcRequest>(inModel);

        //    var response = new ClmClaimAddOutcResponse
        //                       {
        //                           ClaimId = claimId
        //                       };


        //    mockMappingEngine.Setup(m => m.Map<ClmClaimAddOutcRequest>(inModel)).Returns(request);
        //    mockOutcomeWcf.Setup(m => m.LodgeOutcomeClaim(request)).Returns(response);

        //    var result = SystemUnderTest().LodgeOutcomeClaim(inModel);
        //    Assert.IsTrue(result == claimId);
        //    mockMappingEngine.Verify(m => m.Map<ClmClaimAddOutcRequest>(inModel), Times.Once());
        //    mockOutcomeWcf.Verify(m => m.LodgeOutcomeClaim(request), Times.Once());
        //}

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeOutcomesClaimWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var jskId = 1234567890;
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var csdIcn = 1111;
            
            var inModel = new PaymentsModel
                              {
                                  JobseekerId = jskId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  MfawConfirmed = "Y",
                                  ScheduleDetailIcn = csdIcn,
                              };
            var request = MappingEngine.Map<ClmClaimAddOutcRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddOutcRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.LodgeOutcomeClaim(request)).Throws(exception);
            
            SystemUnderTest().LodgeOutcomeClaim(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeOutcomesClaimThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var jskId = 1234567890;
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var csdIcn = 1111;
            
            var inModel = new PaymentsModel
                              {
                                  JobseekerId = jskId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  MfawConfirmed = "Y",
                                  ScheduleDetailIcn = csdIcn,
                              };

            var request = MappingEngine.Map<ClmClaimAddOutcRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddOutcRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.LodgeOutcomeClaim(request)).Throws(exception);
            SystemUnderTest().LodgeOutcomeClaim(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void LodgeOutcomesClaimThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<PaymentsFault>(new PaymentsFault { Message = "Exception" });

            var jskId = 1234567890;
            var scheduleId = 1234;
            var csdSeqNo = 1454;
            var csdIcn = 1111;

            var inModel = new PaymentsModel
                              {
                                  JobseekerId = jskId,
                                  ScheduleDetailSeqNo = csdSeqNo,
                                  ScheduleId = scheduleId,
                                  MfawConfirmed = "Y",
                                  ScheduleDetailIcn = csdIcn,
                              };

            var request = MappingEngine.Map<ClmClaimAddOutcRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<ClmClaimAddOutcRequest>(inModel)).Returns(request);
            mockOutcomeWcf.Setup(m => m.LodgeOutcomeClaim(request)).Throws(exception);
            SystemUnderTest().LodgeOutcomeClaim(inModel);
        }
        #endregion Outcomes Tax Invoice
    }
}
