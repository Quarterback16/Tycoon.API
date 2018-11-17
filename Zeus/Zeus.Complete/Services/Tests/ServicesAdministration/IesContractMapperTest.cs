using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.ServicesAdministration;
using Employment.Web.Mvc.Service.Interfaces.ServicesAdministration;
using Employment.Esc.IESContracts.Contracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Employment.Web.Mvc.Service.Tests.ServicesAdministration
{
    /// <summary>
    ///This is a test class for ActivityMapperTest and is intended
    ///to contain all ActivityMapperTest Unit Tests
    ///</summary>
    [TestClass]
    public class IesContractMapperTest
    {
        public IesContractMapper SystemUnderTest()
        {
            return new IesContractMapper();
        }

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

        /// <summary>
        ///A test for Mappping configuration
        ///</summary>
        [TestMethod]
        public void IsMappingValid()
        {
            SystemUnderTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
        
        #region Tests related to Contract
        [TestMethod]
        public void ContractListModelToContractSearchRequestMapper_MappingTest_Valid()
        {
            
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyContractListModel();

            //2. Exec
            var dest = MappingEngine.Map<ContractSearchRequest>(source);

            //3. Verification
            Assert.AreEqual(source.AccountManagingOffice, dest.AccountManagingOffice);
            Assert.AreEqual(source.ActivityId, dest.ActivityId);
            Assert.AreEqual(source.ContractEndDateFrom, dest.ContractEndDateFrom);
            Assert.AreEqual(source.ContractEndDateTo, dest.ContractEndDateTo);
            Assert.AreEqual(source.ContractRound, dest.ContractRound);
            Assert.AreEqual(source.ContractStartDateFrom, dest.ContractStartDateFrom);
            Assert.AreEqual(source.ContractStartDateTo, dest.ContractStartDateTo);
            Assert.AreEqual(source.ContractStatus, dest.ContractStatus);
            Assert.AreEqual(source.ContractType, dest.ContractType);
            Assert.AreEqual(source.OrganisationCode, dest.Organisation);
            Assert.AreEqual(source.ProviderId, dest.ProviderId);
        }

        [TestMethod]
        public void ContractSearchResponseToContractListModel_MappingTest_Valid()
        {
            //1. setup data
            List<OutContractSearchGroup> contractList = new List<OutContractSearchGroup>();

            for (int i = 1; i < 10; i++)
            {
                contractList.Add(IesContractTestDataHelper.CreateDummyContract(i));
            }

            var source = new ContractSearchResponse
            {
                MoreFlag = "Y",
                NextContractId = "123456789A",
                OutContractGroup = contractList.ToArray(),
            };

            //2. Exec
            var dest = MappingEngine.Map<IesContractListModel>(source);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, dest.HasMoreRecords);
            Assert.AreEqual(source.NextContractId, dest.NextContractId);
            //Verify response list
            Assert.AreEqual(source.OutContractGroup.Length, dest.Results.Count());
        }

        [TestMethod]
        public void ContractGroupToIesContractModel_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyContract(8);

            //2. Exec
            var dest = MappingEngine.Map<IesContractModel>(source);

            //3. Verification
            Assert.AreEqual(source.AccountManagingOffice, dest.AccountManagingOffice);
            Assert.AreEqual(source.ContractEndDate, dest.ContractEndDate);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.ContractIdDescription, dest.ContractDescription);
            Assert.AreEqual(source.ContractStatus, dest.ContractStatus);
            Assert.AreEqual(source.ContractType, dest.ContractType);
            Assert.AreEqual(source.OrganisationCode, dest.OrganisationCode);
            Assert.AreEqual(source.RegionArea, dest.RegionArea);
            Assert.AreEqual(source.Specialisation, dest.Specialisation);
        }

        [TestMethod]
        public void ReadContractResponseToContractGetModelMapper_MappingTest_Valid()
        {

            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyContractDetails("0204987H");

            //2. Exec
            var dest = MappingEngine.Map<IesContractGetModel>(source);

            //3. Verification
            Assert.AreEqual(source.AccountId, dest.AccountId);
            Assert.AreEqual(source.AccountManagingOffice, dest.AccountManagingOffice);
            Assert.AreEqual(source.ApprovedAmount, dest.ApprovedAmount);
            Assert.AreEqual(source.BisAgreementId, dest.BisAgreementId);
            Assert.AreEqual(source.ContactEmail, dest.ContactEmail);
            Assert.AreEqual(source.ContactFax, dest.ContactFax);
            Assert.AreEqual(source.ContactMobile, dest.ContactMobile);
            Assert.AreEqual(source.ContactName, dest.ContactName);
            Assert.AreEqual(source.ContactPhone, dest.ContactPhone);
            Assert.AreEqual(source.ContractDescriptionLine1, dest.ContractDescriptionLine1);
            Assert.AreEqual(source.ContractDescriptionLine2, dest.ContractDescriptionLine2);
            Assert.AreEqual(source.ContractDescriptionLine3, dest.ContractDescriptionLine3);
            Assert.AreEqual(source.ContractEndDate, dest.ContractEndDate);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.ContractIdDescription, dest.ContractDescription);
            Assert.AreEqual(source.ContractRound, dest.ContractRound);
            Assert.AreEqual(source.ContractStartDate, dest.ContractStartDate);
            Assert.AreEqual(source.ContractStatus, dest.ContractStatus);
            Assert.AreEqual(source.ContractType, dest.ContractType);
            Assert.AreEqual(source.CreationDate, dest.CreationDate);
            Assert.AreEqual(source.CreationTime, dest.CreationTime);
            Assert.AreEqual(source.CreationUserId, dest.CreationUserId);
            Assert.AreEqual(source.FeeForServiceFlag, dest.FeeForServiceFlag);
            Assert.AreEqual(source.FundsSpentToDate, dest.FundsSpentToDate);
            Assert.AreEqual(source.IccOfficeCode, dest.IccOfficeCode);
            Assert.AreEqual(source.InitialApprovedCost, dest.InitialApprovedCost);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.MarketShare, dest.MarketShare);
            Assert.AreEqual(source.MarketSharePercentage, dest.MarketSharePercentage);
            Assert.AreEqual(source.NonContinueCode, dest.NonContinueCode);
            Assert.AreEqual(source.NovationPrefix, dest.NovationPrefix);
            Assert.AreEqual(source.OrganisationCode, dest.OrganisationCode);
            Assert.AreEqual(source.PraSecondaryAcctId, dest.SecondaryAccountId);
            Assert.AreEqual(source.ProviderDescription, dest.ProviderDescription);
            Assert.AreEqual(source.ProviderId, dest.ProviderId);
            Assert.AreEqual(source.RegionalClassificationCode, dest.RegionalClassificationCode);
            Assert.AreEqual(source.RegionArea, dest.RegionArea);
            Assert.AreEqual(source.RemoteInd, dest.RemoteInd);
            Assert.AreEqual(source.Specialisation, dest.Specialisation);
            Assert.AreEqual(source.SqlTimestamp, dest.SqlTimestamp);
            Assert.AreEqual(source.StatusDate, dest.StatusDate);
            Assert.AreEqual(source.StreamType, dest.StreamType);
            Assert.AreEqual(source.UnapprovedAmount, dest.UnapprovedAmount);
            Assert.AreEqual(source.UpdateDate, dest.UpdateDate);
            Assert.AreEqual(source.UpdateTime, dest.UpdateTime);
            Assert.AreEqual(source.UpdateUserId, dest.UpdateUserId);
            Assert.AreEqual(source.UstkFlag, dest.UstkFlag);
        }

        [TestMethod]
        public void IesCountsGetToIesContractCountsGetModel_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyCountsModel();

            //2. Exec
            var dest = MappingEngine.Map<IesContractCountsGetModel>(source);

            Assert.AreEqual(source.ContractTotalReferred, dest.ContractTotalReferred);
            Assert.AreEqual(source.ContractTotalServiced, dest.ContractTotalServiced);
            Assert.AreEqual(source.ContractTotalTransferred, dest.ContractTotalTransferred);
            Assert.AreEqual(source.GroupTotalReferred, dest.GroupTotalReferred);
        }

        [TestMethod]
        public void ReadAccountResponseToIesContractAccountGetModel_MappingTest_Valid()
        {
            //1. setup data
            ReadAccountResponse source = new ReadAccountResponse
            {
                AccountId = 123,
                AccountName = "Account Name",
                AccountNumber = "Account Number",
                AccountType = "A",
                BsbNumber = "123456",
                CommentsLine1 = "line 1",
                CommentsLine2 = "line 2",
                InstitutionBranch = "North Ryde",
                InstitutionName = "NAB",
                InstitutionType = "Institution Type",
                IntegrityControlNumber = 1,
                PassAccountId = "paid",
                PassAccountSeqNum = 1,
                ProviderId = 123456789,
                ProviderName = "Mission",
                Status = "P",
                TaxFileNumber = 123456,
                UpdateDate = DateTime.Today,
                UpdateTime = DateTime.Now,
                UpdateUserId = "UserId"
            };

            //2. Exec
            var dest = MappingEngine.Map<IesContractAccountGetModel>(source);

            Assert.AreEqual(source.AccountId, dest.AccountId);
            Assert.AreEqual(source.AccountName, dest.AccountName);
            Assert.AreEqual(source.AccountNumber, dest.AccountNumber);
            Assert.AreEqual(source.AccountType, dest.AccountType);
            Assert.AreEqual(source.BsbNumber, dest.BsbNumber);
            Assert.AreEqual(source.CommentsLine1, dest.CommentsLine1);
            Assert.AreEqual(source.CommentsLine2, dest.CommentsLine2);
            Assert.AreEqual(source.InstitutionBranch, dest.InstitutionBranch);
            Assert.AreEqual(source.InstitutionName, dest.InstitutionName);
            Assert.AreEqual(source.InstitutionType, dest.InstitutionType);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.PassAccountId, dest.PassAccountId);
            Assert.AreEqual(source.PassAccountSeqNum, dest.PassAccountSeqNum);
            Assert.AreEqual(source.ProviderId, dest.ProviderId);
            Assert.AreEqual(source.ProviderName, dest.ProviderName);
            Assert.AreEqual(source.Status, dest.Status);
            Assert.AreEqual(source.SupersededProviderId, dest.SupersededProviderId);
            Assert.AreEqual(source.TaxFileNumber, dest.TaxFileNumber);
            Assert.AreEqual(source.UpdateDate, dest.UpdateDate);
            Assert.AreEqual(source.UpdateTime, dest.UpdateTime);
            Assert.AreEqual(source.UpdateUserId, dest.UpdateUserId);
            Assert.AreEqual(source.ValidEndDate, dest.ValidEndDate);
            Assert.AreEqual(source.ValidStartDate, dest.ValidStartDate);
        }
        #endregion

        #region Tests related to Outlet
        [TestMethod]
        public void OutletListModelToOutletSearchRequestMapper_MappingTest_Valid()
        {

            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyOutletListModel();

            //2. Exec
            var dest = MappingEngine.Map<OutletSearchRequest>(source);

            //3. Verification
            Assert.AreEqual(source.AccountManagingOffice, dest.AccountManagingOffice);
            Assert.AreEqual(source.ActivityId, dest.ActivityId);
            Assert.AreEqual(source.ContractEndDateFrom, dest.ContractEndDateFrom);
            Assert.AreEqual(source.ContractEndDateTo, dest.ContractEndDateTo);
            Assert.AreEqual(source.ContractRound, dest.ContractRound);
            Assert.AreEqual(source.ContractStartDateFrom, dest.ContractStartDateFrom);
            Assert.AreEqual(source.ContractStartDateTo, dest.ContractStartDateTo);
            Assert.AreEqual(source.ContractStatus, dest.ContractStatus);
            Assert.AreEqual(source.ContractType, dest.ContractType);
            Assert.AreEqual(source.OrganisationCode, dest.Organisation);
            Assert.AreEqual(source.ProviderId, dest.ProviderId);
        }

        [TestMethod]
        public void OutletSearchResponseToOutlettListModel_MappingTest_Valid()
        {
            //1. setup data
            List<OutOutletSearchGroup> outletList = new List<OutOutletSearchGroup>();

            for (int i = 1; i < 10; i++)
            {
                outletList.Add(IesContractTestDataHelper.CreateDummyOutlet(i));
            }

            var source = new OutletSearchResponse
            {
                MoreFlag = "Y",
                NextContractId = "123456789A",
                OutOutletGroup = outletList.ToArray(),
                NextSequenceNumber = 1
            };

            //2. Exec
            var dest = MappingEngine.Map<IesOutletListModel>(source);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, dest.HasMoreRecords);
            Assert.AreEqual(source.NextContractId, dest.NextContractId);
            //Verify response list
            Assert.AreEqual(source.OutOutletGroup.Length, dest.Results.Count());
        }

        [TestMethod]
        public void OutletGroupToIesOutletModel_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyOutlet(8);

            //2. Exec
            var dest = MappingEngine.Map<IesOutletModel>(source);

            //3. Verification
            Assert.AreEqual(source.ContractID, dest.ContractId);
            Assert.AreEqual(source.Description, dest.Description);
            Assert.AreEqual(source.EndDate, dest.OutletEndDate);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.Services, dest.Services);
            Assert.AreEqual(source.SiteCode, dest.SiteCode);
            Assert.AreEqual(source.Status, dest.OutletStatus);            
        }

        [TestMethod]
        public void ServiceListResponseToIesServiceListModel_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyServiceList("1");

            //2. Exec
            var dest = MappingEngine.Map<IesServiceListModel>(source);

            //3. Verification
            Assert.AreEqual(source.OutOutletServices.Count(), dest.Services.Count());
            Assert.AreEqual(source.MoreDataFlag == "N", dest.MoreDataFlag == false);
            Assert.AreEqual(source.LastServiceCode, dest.LastServiceCode);
        }

        [TestMethod]
        public void OutOutletServiceToIesServiceModel_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyService("1");

            //2. Exec
            var dest = MappingEngine.Map<IesServiceModel>(source);

            //3. Verification
            Assert.AreEqual(source.Code, dest.Code);
            Assert.AreEqual(source.CreatedBy, dest.CreatedBy);
            Assert.AreEqual(source.CreatedDate, dest.CreatedDate);
            Assert.AreEqual(source.CreatedTime, dest.CreatedTime);
            Assert.AreEqual(source.EndDate, dest.EndDate);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.ServiceSequenceNumber, dest.ServiceSequenceNumber);
            Assert.AreEqual(source.StartDate, dest.StartDate);
            Assert.AreEqual(source.Status, dest.Status);
            Assert.AreEqual(source.UpdatedBy, dest.UpdatedBy);
            Assert.AreEqual(source.UpdatedDate, dest.UpdatedDate);
            Assert.AreEqual(source.UpdatedTime, dest.UpdatedTime);
        }

        [TestMethod]
        public void IesOutletUpdateSSCRequestModelToOutletUpdateSSCRequest_MappingTest_Valid()
        {
            //1. setup data
            IesOutletUpdateSSCRequestModel source = new IesOutletUpdateSSCRequestModel
            {
                CheckRelatedOutletFlag = "y",
                ContactName = "ContactName",
                ContractId = "0123456789H",
                EmailAddress = "email@email.com",
                EndDate = DateTime.Now,
                FaxNumber = "88888888",
                IntegrityControlNumber = 100,
                MobileNumber = "04000004",
                PhoneNumber = "99999999",
                ProviderText = "ProviderText",
                RelatedOutlets = new IesOutletRelatedInModel[] { new IesOutletRelatedInModel { ContractId = "121212128H",IntegrityControlNumber= 200,SequenceNumber = 1} },
                SequenceNumber = 1,
                StartDate = DateTime.Now.AddYears(-1),
                SupervisingOffice = "SupervisingOffice",
                SuspendClaimsFromDate = DateTime.Now.AddDays(-1),
                SuspendEPFFromDate = DateTime.Now.AddDays(-2),
                SuspendNewCaseFromDate = DateTime.Now.AddDays(-3),
                SuspendRefsFromDate = DateTime.Now.AddDays(-4),
                SuspendRelatedEntityFromDate = DateTime.Now.AddDays(-5),
                SuspendReturnFromDate = DateTime.Now.AddDays(-6),
                SuspendTransportFromDate = DateTime.Now.AddDays(-7)
            };
           

            //2. Exec
            var dest = MappingEngine.Map<OutletUpdateSSCRequest>(source);

            //3. Verification
            Assert.AreEqual(source.CheckRelatedOutletFlag, dest.CheckRelatedOutletFlag);
            Assert.AreEqual(source.ContactName, dest.ContactName);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.EmailAddress, dest.EmailAddress);
            Assert.AreEqual(source.EndDate, dest.EndDate);
            Assert.AreEqual(source.FaxNumber, dest.FaxNumber);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.MobileNumber, dest.MobileNumber);
            Assert.AreEqual(source.PhoneNumber, dest.PhoneNumber);
            Assert.AreEqual(source.ProviderText, dest.ProviderText);
            Assert.AreEqual(source.RelatedOutlets.Length, dest.RelatedOutlets.Length);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.StartDate, dest.StartDate);
            Assert.AreEqual(source.SupervisingOffice, dest.SupervisingOffice);
            Assert.AreEqual(source.SuspendClaimsFromDate, dest.SuspendClaimsFromDate);
            Assert.AreEqual(source.SuspendEPFFromDate, dest.SuspendEPFFromDate);
            Assert.AreEqual(source.SuspendNewCaseFromDate, dest.SuspendNewCaseFromDate);
            Assert.AreEqual(source.SuspendRefsFromDate, dest.SuspendRefsFromDate);
            Assert.AreEqual(source.SuspendRelatedEntityFromDate, dest.SuspendRelatedEntityFromDate);
            Assert.AreEqual(source.SuspendReturnFromDate, dest.SuspendReturnFromDate);
            Assert.AreEqual(source.SuspendTransportFromDate, dest.SuspendTransportFromDate);
        }
        [TestMethod]
        public void IesOutletUpdateNEISRequestModelToOutletUpdateNEISRequest_MappingTest_Valid()
        {
            //1. setup data
            IesOutletUpdateNEISRequestModel source = new IesOutletUpdateNEISRequestModel
            {
                CheckRelatedOutletFlag = "y",
                ContactName = "ContactName",
                ContractId = "0123456789H",
                EmailAddress = "email@email.com",
                EndDate = DateTime.Now,
                FaxNumber = "88888888",
                IntegrityControlNumber = 100,
                MobileNumber = "04000004",
                PhoneNumber = "99999999",
                ProviderText = "ProviderText",
                RelatedOutlets = new IesOutletRelatedInModel[] { new IesOutletRelatedInModel { ContractId = "121212128H", IntegrityControlNumber = 200, SequenceNumber = 1 } },
                SequenceNumber = 1,
                StartDate = DateTime.Now.AddYears(-1),
                SupervisingOffice = "SupervisingOffice",
                SuspendRefsFromDate = DateTime.Now.AddDays(-4)
            };


            //2. Exec
            var dest = MappingEngine.Map<OutletUpdateNEISRequest>(source);

            //3. Verification
            Assert.AreEqual(source.CheckRelatedOutletFlag, dest.CheckRelatedOutletFlag);
            Assert.AreEqual(source.ContactName, dest.ContactName);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.EmailAddress, dest.EmailAddress);
            Assert.AreEqual(source.EndDate, dest.EndDate);
            Assert.AreEqual(source.FaxNumber, dest.FaxNumber);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.MobileNumber, dest.MobileNumber);
            Assert.AreEqual(source.PhoneNumber, dest.PhoneNumber);
            Assert.AreEqual(source.ProviderText, dest.ProviderText);
            Assert.AreEqual(source.RelatedOutlets.Length, dest.RelatedOutlets.Length);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.StartDate, dest.StartDate);
            Assert.AreEqual(source.SupervisingOffice, dest.SupervisingOffice);
            Assert.AreEqual(source.SuspendRefsFromDate, dest.SuspendRefsFromDate);
        }

        [TestMethod]
        public void IesOutletRelatedInModelToRelatedOutletIn_MappingTest_Valid()
        {
            //1. setup data
            IesOutletRelatedInModel source = new IesOutletRelatedInModel { ContractId = "121212128H", IntegrityControlNumber = 200, SequenceNumber = 1 };


            //2. Exec
            var dest = MappingEngine.Map<RelatedOutletIn>(source);

            //3. Verification
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
        }
        [TestMethod]
        public void OutletUpdateSSCResponseToIesOutletUpdateResponseModel_MappingTest_Valid()
        {
            //1. setup data
            OutletUpdateSSCResponse source = new OutletUpdateSSCResponse
            {
                IntegrityControlNumber = 123,
                RelatedOutlets = new RelatedOutletOut[] { new RelatedOutletOut { ContractDescription = "ContractDescription", ContractId = "121212128H", EndDate = DateTime.Now.AddYears(1), IntegrityControlNumber = 200, SequenceNumber = 1, StartDate = DateTime.Now, SupervisingOffice = "SupervisingOffice"} },
                SequenceNumber = 1,
                UpdateDate = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "UpdateUserId"
            };


            //2. Exec
            var dest = MappingEngine.Map<IesOutletUpdateResponseModel>(source);

            //3. Verification
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.RelatedOutlets.Length, dest.RelatedOutlets.Length);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.UpdateDate, dest.UpdateDate);
            Assert.AreEqual(source.UpdateTime, dest.UpdateTime);
            Assert.AreEqual(source.UpdateUserId, dest.UpdateUserId);
        }
        [TestMethod]
        public void OutletUpdateNEISResponseToIesOutletUpdateResponseModel_MappingTest_Valid()
        {
            //1. setup data
            OutletUpdateNEISResponse source = new OutletUpdateNEISResponse
            {
                IntegrityControlNumber = 123,
                RelatedOutlets = new RelatedOutletOut[] { new RelatedOutletOut { ContractDescription = "ContractDescription", ContractId = "121212128H", EndDate = DateTime.Now.AddYears(1), IntegrityControlNumber = 200, SequenceNumber = 1, StartDate = DateTime.Now, SupervisingOffice = "SupervisingOffice" } },
                SequenceNumber = 1,
                UpdateDate = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "UpdateUserId"
            };


            //2. Exec
            var dest = MappingEngine.Map<IesOutletUpdateResponseModel>(source);

            //3. Verification
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.RelatedOutlets.Length, dest.RelatedOutlets.Length);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.UpdateDate, dest.UpdateDate);
            Assert.AreEqual(source.UpdateTime, dest.UpdateTime);
            Assert.AreEqual(source.UpdateUserId, dest.UpdateUserId);
        }

        [TestMethod]
        public void RelatedOutletOutModelToIesOutletRelatedOutModel_MappingTest_Valid()
        {
            //1. setup data
            RelatedOutletOut source = new RelatedOutletOut { ContractId = "121212128H", ContractDescription="tset", EndDate=DateTime.Now.AddYears(1), IntegrityControlNumber = 200, SequenceNumber = 1, StartDate=DateTime.Now, SupervisingOffice="SupervisingOffice" };


            //2. Exec
            var dest = MappingEngine.Map<IesOutletRelatedOutModel>(source);

            //3. Verification
            Assert.AreEqual(source.ContractDescription, dest.ContractDescription);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.EndDate, dest.EndDate);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
            Assert.AreEqual(source.StartDate, dest.StartDate);
            Assert.AreEqual(source.SupervisingOffice, dest.SupervisingOffice);
        }

        [TestMethod]
        public void RegionListResponseToIesOutletRegionsGetModel_MappingTest_Valid()
        {
            //TODO: Finish writing unit test

            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyRegionListResponse("0ESJD2");

            //2. exec
            var dest = MappingEngine.Map<IesOutletRegionsListModel>(source);

            //3. verify
            Assert.AreEqual(source.ExecutionResult, dest.ExecutionResult);
            //Assert.AreEqual(source.LastRegionCode, dest.LastRegionCode);
            //Assert.AreEqual(source.LastRegionType, dest.LastRegionType);
            Assert.AreEqual(source.MoreFlag, dest.MoreFlag);
            Assert.AreEqual(source.OutOutletRegions.Count(), dest.OutletRegions.Count);
        }

        [TestMethod]
        public void OutOutletRegionToOutletRegion_MappingTest_Valid()
        {
            //TODO: Finish writing unit test

            //1. setup data
            var source = IesContractTestDataHelper.CreateDummyOutOutletRegion("0ESJD2");

            //2. exec
            var dest = MappingEngine.Map<IesOutletRegion>(source);

            //3. verify
            Assert.AreEqual(source.AdditionalInformation, dest.AdditionalInformation);
            Assert.AreEqual(source.AssessorAvailFl, dest.AvailableFlag);
            Assert.AreEqual(source.ContactName, dest.ContactName);
            Assert.AreEqual(source.ContactPhone, dest.ContactPhone);
            Assert.AreEqual(source.CreationDate, dest.CreationDate);
            Assert.AreEqual(source.CreationTime, dest.CreationTime);
            Assert.AreEqual(source.CreationUserId, dest.CreationUserId);
            Assert.AreEqual(source.CurrentAssmntCnt, dest.CurrentAssessmentCount);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.PartRegnCoverFl, dest.PartialCoverageFlag);
            Assert.AreEqual(source.RegionCode, dest.RegionCode);
            Assert.AreEqual(source.RegionDescription, dest.RegionDescription);
            Assert.AreEqual(source.RegionType, dest.RegionType);
            Assert.AreEqual(source.UpdateDate, dest.UpdateDate);
            Assert.AreEqual(source.UpdateTime, dest.UpdateTime);
            Assert.AreEqual(source.UpdateUserId, dest.UpdateUserId);
        }

        [TestMethod]
        public void OutletRegionUpdateModelToRegionUpdateRequestMapper_MappingTest_Valid()
        {

            //1. setup data
            var source = new IesOutletRegionUpdateModel
                {
                    AdditionalInformation = "AdditionalInformation",
                    AssessorAvailFl = "Y",
                    CapacityLimitCnt = 99,
                    ContactName = "ContactName",
                    ContactPhone = "ContactPhone",
                    ContractId = "0204342H",
                    IntegrityControlNumber = 3,
                    PartRegnCoverFl = "Y",
                    RegionCode = "A010",
                    RegionType = "ESC4",
                    SequenceNumber = 2
                };

            //2. Exec
            var dest = MappingEngine.Map<RegionUpdateRequest>(source);

            //3. Verification
            Assert.AreEqual(source.AdditionalInformation, dest.AdditionalInformation);
            Assert.AreEqual(source.AssessorAvailFl, dest.AssessorAvailFl);
            Assert.AreEqual(source.CapacityLimitCnt, dest.CapacityLimitCnt);
            Assert.AreEqual(source.ContactName, dest.ContactName);
            Assert.AreEqual(source.ContactPhone, dest.ContactPhone);
            Assert.AreEqual(source.ContractId, dest.ContractId);
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
            Assert.AreEqual(source.PartRegnCoverFl, dest.PartRegnCoverFl);
            Assert.AreEqual(source.RegionCode, dest.RegionCode);
            Assert.AreEqual(source.RegionType, dest.RegionType);
            Assert.AreEqual(source.SequenceNumber, dest.SequenceNumber);
        }
        #endregion

        #region Tests related to Site

        [TestMethod]
        public void SiteGetResponseToIesSiteGetModelMapper_MappingTest_Valid()
        {
            //1. setup data
            var source = IesContractTestDataHelper.CreateDummySiteDetails("0ESJD2");

            //2. exec
            var dest = MappingEngine.Map<IesSiteGetModel>(source);

            //3. verify
            Assert.AreEqual(source.SiteCode, dest.SiteCode);
            Assert.AreEqual(source.SiteCodeDescription, dest.SiteCodeDescription);
            Assert.AreEqual(source.AddressLine1, dest.AddressLine1);
            Assert.AreEqual(source.AddressLine2, dest.AddressLine2);
            Assert.AreEqual(source.AddressLine3, dest.AddressLine3);
            Assert.AreEqual(source.Locality, dest.Locality);
            Assert.AreEqual(source.State, dest.State);
            Assert.AreEqual(source.Postcode, dest.Postcode);
            Assert.AreEqual(source.AreaCode, dest.AreaCode);
            Assert.AreEqual(source.PhoneNumber, dest.PhoneNumber);
            Assert.AreEqual(source.FaxNumber, dest.FaxNumber);
            Assert.AreEqual(source.FreeCallNumber, dest.FreeCallNumber);
            Assert.AreEqual(source.Email, dest.Email);
            Assert.AreEqual(source.ContactPerson, dest.ContactPerson);
            Assert.AreEqual(source.OfficeTypeCode, dest.OfficeTypeCode);
            Assert.AreEqual(source.OfficeTypeDescription, dest.OfficeTypeDescription);
            Assert.AreEqual(source.OrganisationCode, dest.OrganisationCode);
            Assert.AreEqual(source.OrganisationDescription, dest.OrganisationDescription);
            Assert.AreEqual(source.ManagingSiteCode, dest.ManagingSiteCode);
            Assert.AreEqual(source.ManagingSiteDescription, dest.ManagingSiteDescription);
            Assert.AreEqual(source.SiteSPIndicatorCode, dest.SiteSpIndicatorCode);
            Assert.AreEqual(source.AddressDisocsCd, dest.AddressDisocsCd);
            Assert.AreEqual(source.AddressType, dest.AddressType);
            Assert.AreEqual(source.AddressOfficeType, dest.AddressOfficeType);
            Assert.AreEqual(source.AddressLocationNm, dest.AddressLocationNm);
        }

        #endregion
    }
}
