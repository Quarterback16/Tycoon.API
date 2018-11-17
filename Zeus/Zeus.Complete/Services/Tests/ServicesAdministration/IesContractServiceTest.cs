using System.Web.Mvc;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Services;
using Employment.Web.Mvc.Service.Implementation.ServicesAdministration;
using Employment.Web.Mvc.Service.Interfaces.ServicesAdministration;
using Employment.Esc.IESContracts.Contracts.ServiceContracts;
using Employment.Esc.IESContracts.Contracts.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;

namespace Employment.Web.Mvc.Service.Tests.ServicesAdministration
{
    [TestClass]
    public class IesContractServiceTest
    {
        #region Private variables/initializers

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        /// <summary>
        /// WCF service
        /// </summary>
        private Mock<ISearch> mockContractSearchWcf;
        private Mock<IContract> mockContractGetWcf;
        private Mock<IOutlet> mockOutletUpdateWcf;
        private Mock<ISite> mockSiteGetWcf;
        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new IesContractMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private IesContractService SystemUnderTest()
        {
            return new IesContractService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }

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

            //WCF services
            mockContractSearchWcf = new Mock<ISearch>();
            mockClient.Setup(m => m.Create<ISearch>("IESSearch.svc")).Returns(mockContractSearchWcf.Object);
            mockContractGetWcf = new Mock<IContract>();
            mockClient.Setup(m => m.Create<IContract>("IESContract.svc")).Returns(mockContractGetWcf.Object);
            mockOutletUpdateWcf = new Mock<IOutlet>();
            mockClient.Setup(m => m.Create<IOutlet>("IESOutlet.svc")).Returns(mockOutletUpdateWcf.Object);
            mockSiteGetWcf = new Mock<ISite>();
            mockClient.Setup(m => m.Create<ISite>("IESSite.svc")).Returns(mockSiteGetWcf.Object);

        }

        #endregion

        #region Contract search test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ContractList_Valid()
        {
            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyContractListModel();
            //b.response
            List<OutContractSearchGroup> contractLists = new List<OutContractSearchGroup>();

            for (int i = 1; i < 10; i++)
            {
                contractLists.Add(IesContractTestDataHelper.CreateDummyContract(i));
            }

            var response = new ContractSearchResponse
            {
                MoreFlag = "Y",
                NextContractId = "123456789A",
                OutContractGroup = contractLists.ToArray()
            };

            mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListContracts(request);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, result.HasMoreRecords);
            Assert.AreEqual(response.NextContractId, result.NextContractId);
            //Verify response list
            Assert.AreEqual(response.OutContractGroup.Length, result.Results.Count());

            //Verify behaviour
            mockContractSearchWcf.Verify(m => m.ContractSearch(It.Is<ContractSearchRequest>(r => r.ContractType == request.ContractType)), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test invalid search combination
        /// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_RequestDataError_ThrowsServiceValidationException()
        //{

        //    //1. Setup data
        //    //a.request
        //    var request = new IesContractListModel
        //    {
        //        ContractType = "SSC"
        //    };

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);

        //    //3. Verification
        //}

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ContractList_FailedResponse_ThrowsServiceValidationException()
        {
            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyContractListModel();

            //b.response
            var response = new ContractSearchResponse();
            response.ExecutionResult =IesContractTestDataHelper.CreateDummyFailedExecutionResult();

            mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListContracts(request);

            //3. Verification
        }

       

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ContractList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyContractListModel();

            mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListContracts(request);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ContractList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultException();

             //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyContractListModel();

            mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListContracts(request);
        }
        #endregion
        #endregion

        #region Contract GET test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ContractGet_Valid()
        {
            //1. Setup data
            //a.request
            string contractId = "0204342H";
           // var request = new ReadContractRequest { ContractId = contractId };
            //b.response
           
            var response = IesContractTestDataHelper.CreateDummyContractDetails(contractId);
            
            mockContractGetWcf.Setup(m => m.ReadContract(It.IsAny<ReadContractRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetContract(contractId);

            //3. Verification
            //Verify More parameters
            Assert.IsTrue(result.ContractId.Equals(contractId));
            
            //Verify behaviour
            mockContractGetWcf.Verify(m => m.ReadContract(It.Is<ReadContractRequest>(r => r.ContractId == contractId)), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test invalid search combination
        /// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_RequestDataError_ThrowsServiceValidationException()
        //{

        //    //1. Setup data
        //    //a.request
        //    var request = new IesContractListModel
        //    {
        //        ContractType = "SSC"
        //    };

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);

        //    //3. Verification
        //}

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_FailedResponse_ThrowsServiceValidationException()
        //{
        //    //1. Setup data
        //    //a.request
        //    var request = IesContractTestDataHelper.CreateDummyContractListModel();

        //    //b.response
        //    var response = new ContractSearchResponse();
        //    response.ExecutionResult = IesContractTestDataHelper.CreateDummyFailedExecutionResult();

        //    mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Returns(response);

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);

        //    //3. Verification
        //}



        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    var exception = IesContractTestDataHelper.CreateDummyFaultExceptionValidationFault();

        //    //1. Setup data
        //    //a.request
        //    var request = IesContractTestDataHelper.CreateDummyContractListModel();

        //    mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Throws(exception);

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var exception = IesContractTestDataHelper.CreateDummyFaultException();

        //    //1. Setup data
        //    //a.request
        //    var request = IesContractTestDataHelper.CreateDummyContractListModel();

        //    mockContractSearchWcf.Setup(m => m.ContractSearch(It.IsAny<ContractSearchRequest>())).Throws(exception);

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);
        //}
        #endregion
        #endregion

        #region Contract counts test

        [TestMethod]
        public void IesCountsGet_Valid()
        {
            //1. Setup data
            //a.request
            string contractId = "0204342H";

            //b.response
            var response = IesContractTestDataHelper.CreateDummyCountsModel();

            mockContractGetWcf.Setup(m => m.GetCounts(It.IsAny<GetCountsRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetCounts(contractId);

            //3. Verification
            //Verify More parameters
            Assert.IsTrue(result.ContractTotalReferred.Equals(response.ContractTotalReferred));

            //Verify behaviour
            mockContractGetWcf.Verify(m => m.GetCounts(It.Is<GetCountsRequest>(r => r.ContractId == contractId)), Times.Once());
        }

        #endregion

        #region Contract account test

        [TestMethod]
        public void IesAccountGet_Valid()
        {
            //1. Setup data
            //a.request
            long accountId = 5423360;
            ReadAccountRequest request = new ReadAccountRequest
            {
                AccountId = accountId
            };

            //b.response
            ReadAccountResponse response = new ReadAccountResponse
            {
                AccountId = accountId,
                AccountName = "Mission Australia",
                AccountNumber = "148828",
                AccountType = "P",
                BsbNumber = "032005",
                CommentsLine1 = "esc3 + jpl a/cstep contracts",
                InstitutionBranch = "HAYMARKET BR 671-675 GEORGE ST",
                InstitutionName = "Westpac Banking Corporation",
                InstitutionType = "Banking Institution",
                IntegrityControlNumber = 11
            };
                


            mockContractGetWcf.Setup(m => m.ReadAccount(It.IsAny<ReadAccountRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetAccount(accountId);

            //3. Verification
            //Verify More parameters
            Assert.IsTrue(result.AccountId.Equals(accountId));

            //Verify behaviour
            mockContractGetWcf.Verify(m => m.ReadAccount(It.Is<ReadAccountRequest>(r => r.AccountId==accountId)), Times.Once());
        }

        #endregion
        #region Outlet search test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void OutletList_Valid()
        {
            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyOutletListModel();
            //b.response
            List<OutOutletSearchGroup> outletList = new List<OutOutletSearchGroup>();

            for (int i = 1; i < 10; i++)
            {
                outletList.Add(IesContractTestDataHelper.CreateDummyOutlet(i));
            }

            var response = new OutletSearchResponse
            {
                MoreFlag = "Y",
                NextContractId = "123456789A",
                OutOutletGroup = outletList.ToArray(),
                NextSequenceNumber = 1
            };

            mockContractSearchWcf.Setup(m => m.OutletSearch(It.IsAny<OutletSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListOutlets(request);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, result.HasMoreRecords);
            Assert.AreEqual(response.NextContractId, result.NextContractId);
            Assert.AreEqual(response.NextSequenceNumber, result.NextSequenceNumber);
            //Verify response list
            Assert.AreEqual(response.OutOutletGroup.Length, result.Results.Count());

            //Verify behaviour
            mockContractSearchWcf.Verify(m => m.OutletSearch(It.Is<OutletSearchRequest>(r => r.ContractType == request.ContractType)), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test invalid search combination
        /// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void List_RequestDataError_ThrowsServiceValidationException()
        //{

        //    //1. Setup data
        //    //a.request
        //    var request = new IesContractListModel
        //    {
        //        ContractType = "SSC"
        //    };

        //    //2. exec
        //    var result = SystemUnderTest().ListContracts(request);

        //    //3. Verification
        //}

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletList_FailedResponse_ThrowsServiceValidationException()
        {
            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyOutletListModel();

            //b.response
            var response = new OutletSearchResponse();
            response.ExecutionResult = IesContractTestDataHelper.CreateDummyFailedExecutionResult();

            mockContractSearchWcf.Setup(m => m.OutletSearch(It.IsAny<OutletSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListOutlets(request);

            //3. Verification
        }



        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyOutletListModel();

            mockContractSearchWcf.Setup(m => m.OutletSearch(It.IsAny<OutletSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListOutlets(request);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultException();

            //1. Setup data
            //a.request
            var request = IesContractTestDataHelper.CreateDummyOutletListModel();

            mockContractSearchWcf.Setup(m => m.OutletSearch(It.IsAny<OutletSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListOutlets(request);
        }
        #endregion
        #endregion

        #region Outlet update SSC test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void OutletUpdateSSC_Valid()
        {
            //1. Setup data
            //a.request


            IesOutletUpdateSSCRequestModel request = new IesOutletUpdateSSCRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendClaimsFromDate = DateTime.MinValue,
                SuspendEPFFromDate = DateTime.MinValue,
                SuspendNewCaseFromDate = DateTime.MinValue,
                SuspendRefsFromDate = DateTime.MinValue,
                SuspendRelatedEntityFromDate = DateTime.MinValue,
                SuspendReturnFromDate = DateTime.MinValue,
                SuspendTransportFromDate = DateTime.MinValue
            };
            //b.response
            
            var response = new OutletUpdateSSCResponse
            {
                IntegrityControlNumber = 119,
                SequenceNumber = 1,
                UpdateDate = DateTime.Today,
                UpdateTime = DateTime.Now,
                UpdateUserId = "test"
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletSSC(It.IsAny<OutletUpdateSSCRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().UpdateOutletSSC(request);

            //3. Verification
            Assert.AreEqual(response.IntegrityControlNumber, result.IntegrityControlNumber);
            Assert.AreEqual(response.SequenceNumber, result.SequenceNumber);
            Assert.AreEqual(response.UpdateUserId, result.UpdateUserId);
            Assert.AreEqual(response.UpdateDate, result.UpdateDate);
            Assert.AreEqual(response.UpdateTime, result.UpdateTime);

            //Verify behaviour
            mockOutletUpdateWcf.Verify(m => m.UpdateOutletSSC(It.Is<OutletUpdateSSCRequest>(r => r.ContractId + r.SequenceNumber.ToString() == request.ContractId + request.SequenceNumber.ToString())), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test invalid search combination
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateSSC_RequestDataError_ThrowsServiceValidationException()
        {

            //1. Setup data
            //a.request
            var request = new IesOutletUpdateSSCRequestModel
            {
                ContractId = "test"
            };

            //2. exec
            var result = SystemUnderTest().UpdateOutletSSC(request);

            //3. Verification
        }

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateSSC_FailedResponse_ThrowsServiceValidationException()
        {
            //1. Setup data
            //a.request
            
            IesOutletUpdateSSCRequestModel request = new IesOutletUpdateSSCRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendClaimsFromDate = DateTime.MinValue,
                SuspendEPFFromDate = DateTime.MinValue,
                SuspendNewCaseFromDate = DateTime.MinValue,
                SuspendRefsFromDate = DateTime.MinValue,
                SuspendRelatedEntityFromDate = DateTime.MinValue,
                SuspendReturnFromDate = DateTime.MinValue,
                SuspendTransportFromDate = DateTime.MinValue
            };

            //b.response
            var response = new OutletUpdateSSCResponse();
            response.ExecutionResult = IesContractTestDataHelper.CreateDummyFailedExecutionResult();

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletSSC(It.IsAny<OutletUpdateSSCRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().UpdateOutletSSC(request);

            //3. Verification
        }



        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateSSC_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            IesOutletUpdateSSCRequestModel request = new IesOutletUpdateSSCRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendClaimsFromDate = DateTime.MinValue,
                SuspendEPFFromDate = DateTime.MinValue,
                SuspendNewCaseFromDate = DateTime.MinValue,
                SuspendRefsFromDate = DateTime.MinValue,
                SuspendRelatedEntityFromDate = DateTime.MinValue,
                SuspendReturnFromDate = DateTime.MinValue,
                SuspendTransportFromDate = DateTime.MinValue
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletSSC(It.IsAny<OutletUpdateSSCRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().UpdateOutletSSC(request);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateSSC_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultException();

            //1. Setup data
            //a.request
            IesOutletUpdateSSCRequestModel request = new IesOutletUpdateSSCRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendClaimsFromDate = DateTime.MinValue,
                SuspendEPFFromDate = DateTime.MinValue,
                SuspendNewCaseFromDate = DateTime.MinValue,
                SuspendRefsFromDate = DateTime.MinValue,
                SuspendRelatedEntityFromDate = DateTime.MinValue,
                SuspendReturnFromDate = DateTime.MinValue,
                SuspendTransportFromDate = DateTime.MinValue
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletSSC(It.IsAny<OutletUpdateSSCRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().UpdateOutletSSC(request);
        }
        #endregion
        #endregion

        #region Outlet update NEIS test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void OutletUpdateNEIS_Valid()
        {
            //1. Setup data
            //a.request


            IesOutletUpdateNEISRequestModel request = new IesOutletUpdateNEISRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendRefsFromDate = DateTime.MinValue
            };
            //b.response

            var response = new OutletUpdateNEISResponse
            {
                IntegrityControlNumber = 119,
                SequenceNumber = 1,
                UpdateDate = DateTime.Today,
                UpdateTime = DateTime.Now,
                UpdateUserId = "test"
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletNEIS(It.IsAny<OutletUpdateNEISRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().UpdateOutletNEIS(request);

            //3. Verification
            Assert.AreEqual(response.IntegrityControlNumber, result.IntegrityControlNumber);
            Assert.AreEqual(response.SequenceNumber, result.SequenceNumber);
            Assert.AreEqual(response.UpdateUserId, result.UpdateUserId);
            Assert.AreEqual(response.UpdateDate, result.UpdateDate);
            Assert.AreEqual(response.UpdateTime, result.UpdateTime);

            //Verify behaviour
            mockOutletUpdateWcf.Verify(m => m.UpdateOutletNEIS(It.Is<OutletUpdateNEISRequest>(r => r.ContractId + r.SequenceNumber.ToString() == request.ContractId + request.SequenceNumber.ToString())), Times.Once());
        }

        #region exceptions
        ///// <summary>
        ///// Test invalid search combination
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void OutletUpdateNEIS_RequestDataError_ThrowsServiceValidationException()
        //{

        //    //1. Setup data
        //    //a.request
        //    var request = new IesOutletUpdateNEISRequestModel
        //    {
        //        ContractId = "test"
        //    };

        //    //2. exec
        //    var result = SystemUnderTest().UpdateOutletNEIS(request);

        //    //3. Verification
        //}

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateNEIS_FailedResponse_ThrowsServiceValidationException()
        {
            //1. Setup data
            //a.request

            IesOutletUpdateNEISRequestModel request = new IesOutletUpdateNEISRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendRefsFromDate = DateTime.MinValue
            };

            //b.response
            var response = new OutletUpdateNEISResponse();
            response.ExecutionResult = IesContractTestDataHelper.CreateDummyFailedExecutionResult();

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletNEIS(It.IsAny<OutletUpdateNEISRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().UpdateOutletNEIS(request);

            //3. Verification
        }



        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateNEIS_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            IesOutletUpdateNEISRequestModel request = new IesOutletUpdateNEISRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendRefsFromDate = DateTime.MinValue
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletNEIS(It.IsAny<OutletUpdateNEISRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().UpdateOutletNEIS(request);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OutletUpdateNEIS_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = IesContractTestDataHelper.CreateDummyFaultException();

            //1. Setup data
            //a.request
            IesOutletUpdateNEISRequestModel request = new IesOutletUpdateNEISRequestModel
            {
                CheckRelatedOutletFlag = "N",
                ContactName = "TBA",
                ContractId = "0205881K",
                EmailAddress = null,
                EndDate = new DateTime(2015, 6, 30),
                FaxNumber = null,
                IntegrityControlNumber = 118,
                MobileNumber = null,
                PhoneNumber = "0123456789",
                ProviderText = "abc",
                RelatedOutlets = null,
                SequenceNumber = 1,
                StartDate = new DateTime(2009, 4, 2),
                SupervisingOffice = "QLPU",
                SuspendRefsFromDate = DateTime.MinValue
            };

            mockOutletUpdateWcf.Setup(m => m.UpdateOutletNEIS(It.IsAny<OutletUpdateNEISRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().UpdateOutletNEIS(request);
        }
        #endregion
        #endregion

        #region Outlet counts test

        [TestMethod]
        public void ReadOutletCount_Valid()
        {
            //1. Setup data
            //a.request
            const string contractId = "0204342H";
            const int sequenceNumber = 1;

            //b.response
            var response = IesContractTestDataHelper.CreateDummyOutletCountsModel();

            mockOutletUpdateWcf.Setup(m => m.ReadOutletCount(It.IsAny<ReadOutletCountRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetOutletCounts(contractId, sequenceNumber);

            //3. Verification
            //Verify More parameters
            Assert.IsTrue(result.OutletTotalReferred.Equals(response.OutletTotalReferred));
            Assert.IsTrue(result.OutletTotalServiced.Equals(response.OutletTotalServiced));
            Assert.IsTrue(result.OutletTotalTransferred.Equals(response.OutletTotalTransferred));

            //Verify behaviour
            mockOutletUpdateWcf.Verify(m => m.ReadOutletCount(It.Is<ReadOutletCountRequest>(r => r.ContractId == contractId)), Times.Once());
        }

        #endregion

        #region Outlet services test

        [TestMethod]
        public void GetServiceList_Valid()
        {
            //1. Setup data
            //a.request
            const string contractId = "0204342H";
            const int sequenceNumber = 1;

            //b.response
            var response = IesContractTestDataHelper.CreateDummyServiceListModel();

            mockOutletUpdateWcf.Setup(m => m.ListService(It.IsAny<ServiceListRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListOutletServices(contractId, sequenceNumber);

            //3. Verification
            //Verify More parameters
            Assert.IsTrue(result.LastServiceCode.Equals(response.LastServiceCode));
            Assert.IsTrue(result.LastServiceSeqNum.Equals(response.LastServiceSeqNum));
            Assert.IsTrue(result.MoreDataFlag);
            Assert.IsTrue(result.Services.ElementAt(0).Code.Equals(response.OutOutletServices.ElementAt(0).Code));
            Assert.IsTrue(result.Services.ElementAt(0).CreatedBy.Equals(response.OutOutletServices.ElementAt(0).CreatedBy));
            Assert.IsTrue(result.Services.ElementAt(0).CreatedDate.Equals(response.OutOutletServices.ElementAt(0).CreatedDate));
            Assert.IsTrue(result.Services.ElementAt(0).CreatedTime.Equals(response.OutOutletServices.ElementAt(0).CreatedTime));
            Assert.IsTrue(result.Services.ElementAt(0).EndDate.Equals(response.OutOutletServices.ElementAt(0).EndDate));
            Assert.IsTrue(result.Services.ElementAt(0).IntegrityControlNumber.Equals(response.OutOutletServices.ElementAt(0).IntegrityControlNumber));
            Assert.IsTrue(result.Services.ElementAt(0).ServiceSequenceNumber.Equals(response.OutOutletServices.ElementAt(0).ServiceSequenceNumber));
            Assert.IsTrue(result.Services.ElementAt(0).StartDate.Equals(response.OutOutletServices.ElementAt(0).StartDate));
            Assert.IsTrue(result.Services.ElementAt(0).Status.Equals(response.OutOutletServices.ElementAt(0).Status));
            Assert.IsTrue(result.Services.ElementAt(0).UpdatedBy.Equals(response.OutOutletServices.ElementAt(0).UpdatedBy));
            Assert.IsTrue(result.Services.ElementAt(0).UpdatedDate.Equals(response.OutOutletServices.ElementAt(0).UpdatedDate));
            Assert.IsTrue(result.Services.ElementAt(0).UpdatedTime.Equals(response.OutOutletServices.ElementAt(0).UpdatedTime));

            //Verify behaviour
            mockOutletUpdateWcf.Verify(m => m.ListService(It.Is<ServiceListRequest>(r => r.ContractId == contractId)), Times.Once());
            mockOutletUpdateWcf.Verify(m => m.ListService(It.Is<ServiceListRequest>(r => r.SequenceNumber == sequenceNumber)), Times.Once());
        }

        [TestMethod]
        public void GetRegionsList_Valid()
        {
            ////1. Setup data
            ////a.request
            //const string contractId = "0204342H";
            //const int sequenceNumber = 1;

            ////b.response
            //var response = IesContractTestDataHelper.CreateDummyRegionListResponse(contractId);

            //mockOutletUpdateWcf.Setup(m => m.ListRegion(It.IsAny<RegionListRequest>())).Returns(response);

            ////2. exec
            //var result = SystemUnderTest().ListOutletRegions(contractId, sequenceNumber);

            ////3. Verification
            ////Verify More parameters
            //Assert.IsTrue(result.LastRegionCode.Equals(response.LastRegionCode));
            //Assert.IsTrue(result.ExecutionResult.Equals(response.ExecutionResult));
            //Assert.IsTrue(result.LastRegionType.Equals(response.LastRegionType));
            //Assert.IsTrue(result.MoreFlag.Equals(response.MoreFlag));
            //Assert.IsTrue(result.OutletRegions.Count().Equals(response.OutOutletRegions.Count()));
        }

        #endregion

        #region Site details test

        /// <summary>
        /// Test SiteGet runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SiteGet_Valid()
        {
            //1. setup data
            //a. request

            string siteCode = "0F2@";
            mockUserService.Setup(x => x.OrganisationCode).Returns("1");

            //b. response
            var response = IesContractTestDataHelper.CreateDummySiteDetails(siteCode);
            mockSiteGetWcf.Setup(m => m.GetSiteDetails(It.IsAny<SiteGetRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetSite(siteCode);

            //3. verify
            Assert.IsTrue(result.SiteCode.Equals(siteCode));

            //b. verify behavior
            mockSiteGetWcf.Verify(m => m.GetSiteDetails(It.Is<SiteGetRequest>(r => r.SiteCode == siteCode)), Times.Once());
        }

        #endregion
    }
}
