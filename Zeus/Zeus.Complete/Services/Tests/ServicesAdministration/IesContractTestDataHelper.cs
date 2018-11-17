using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employment.Esc.IESContracts.Contracts.DataContracts;
using Employment.Web.Mvc.Service.Interfaces.ServicesAdministration;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Employment.Esc.Shared.Contracts.Execution;

namespace Employment.Web.Mvc.Service.Tests.ServicesAdministration
{
    internal static class IesContractTestDataHelper
    {
        #region common exception

        internal static ExecutionResult CreateDummyFailedExecutionResult()
        {
            var executionResult = new ExecutionResult() { Status = ExecuteStatus.Failed };
            executionResult.ExecuteMessages.Add(new ExecutionMessage(ExecutionMessageType.Error, "Access denied"));

            return executionResult;
        }


        internal static FaultException CreateDummyFaultException()
        {

            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            return exception;

        }

        internal static FaultException<ValidationFault> CreateDummyFaultExceptionValidationFault()
        {

            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            
            return exception;

        }

        #endregion

        #region contract dummy data

        internal static IesContractListModel CreateDummyContractListModel()
        {

            var data = new IesContractListModel
            {
                AccountManagingOffice = String.Empty,
                ActivityId = 0,
                ContractEndDateFrom = DateTime.MinValue,
                ContractEndDateTo = DateTime.MinValue,
                ContractRound = "4",
                ContractStartDateFrom = DateTime.MinValue,
                ContractStartDateTo = DateTime.MinValue,
                ContractStatus = String.Empty,
                ContractType = "SSC",
                OrganisationCode = "MSN",
                ProviderId=0,
                ContractId = String.Empty
                
            };

            return data;
        }

        internal static OutContractSearchGroup CreateDummyContract(int i)
        {
            var data = new  OutContractSearchGroup()
            {
                AccountManagingOffice="AccountManagingOffice",
                ContractEndDate = DateTime.Today.AddYears(1),
                ContractId="12345678" + i.ToString(),
                ContractIdDescription = "test contract",
                ContractStatus = "ACT",
                ContractType="SSC",
                OrganisationCode="MSN",
                RegionArea="A40",
                Specialisation="S1"
            };

            return data;
        }

        internal static ReadContractResponse CreateDummyContractDetails(string contractId)
        {
            var data = new ReadContractResponse()
            {
                AccountId = 100,                
                AccountManagingOffice = "AccountManagingOffice",
                ApprovedAmount = 100d,
                BisAgreementId = "BisAgreementId",
                ContactEmail = "ContactEmail@deerw.gov.au",
                ContactFax = "88888888",
                ContactMobile = "04444444",
                ContactName = "ContactName",
                ContactPhone = "99999999",
                ContractDescriptionLine1 = "ContractDescriptionLine1",
                ContractDescriptionLine2 = "ContractDescriptionLine2",
                ContractDescriptionLine3 = "ContractDescriptionLine3",
                ContractEndDate = DateTime.Today.AddYears(1),                
                ContractId = contractId,
                ContractIdDescription = "test contract",
                ContractRound = "4",
                ContractStartDate = DateTime.Today.AddYears(1),                 
                ContractStatus = "ACT",                
                ContractType = "SSC",
                CreationDate = DateTime.Today,
                CreationTime = DateTime.Now,
                CreationUserId = "CL0360",
                FeeForServiceFlag = "N",
                FundsSpentToDate = 0,
                IccOfficeCode = "IccOfficeCode",
                InitialApprovedCost = 100d,
                IntegrityControlNumber = 1,
                MarketShare = 0.21d,
                MarketSharePercentage = 21d,
                NonContinueCode = "NonContinueCode",
                NovationPrefix = "NovationPrefix",
                OrganisationCode = "MSN",
                PraSecondaryAcctId = 100,
                ProviderDescription = "Mission Australia",
                ProviderId = 31404009,
                RegionalClassificationCode = "RegionalClassificationCode",
                RegionArea = "A40",
                RemoteInd = "N",
                Specialisation = "S1",
                StreamType = "StreamType",
                UnapprovedAmount = 200d,
                SqlTimestamp = DateTime.MinValue,
                StatusDate = DateTime.MinValue,
                UpdateDate = DateTime.Now,
                UpdateTime = DateTime.Now,
                UpdateUserId = "CL0360",
                UstkFlag = "N"
            };

            return data;
        }

        internal static GetCountsResponse CreateDummyCountsModel()
        {
            var data = new GetCountsResponse
            {
                ContractTotalReferred = 1,
                ContractTotalServiced = 1,
                GroupTotalReferred = 1,
                ContractTotalTransferred = 1
            };

            return data;
        }

        internal static ReadOutletCountResponse CreateDummyOutletCountsModel()
        {
            var data = new ReadOutletCountResponse
                           {
                               OutletTotalReferred = 1, 
                               OutletTotalServiced = 2, 
                               OutletTotalTransferred = 3
                           };

            return data;
        }

        internal static ServiceListResponse CreateDummyServiceListModel()
        {
            var data = new ServiceListResponse
                           {
                               LastServiceCode = "1",
                               LastServiceSeqNum = 1,
                               MoreDataFlag = "Y",
                               OutOutletServices = new OutOutletService[1]
                           };

            data.OutOutletServices[0] = new OutOutletService
                                            {
                                                Code = "1",
                                                CreatedBy = "MD2824",
                                                CreatedDate = DateTime.Today,
                                                CreatedTime = DateTime.Now,
                                                EndDate = DateTime.Today,
                                                IntegrityControlNumber = 1,
                                                ServiceSequenceNumber = 1,
                                                StartDate = DateTime.Now,
                                                Status = "PND",
                                                UpdatedBy = "MD2824",
                                                UpdatedDate = DateTime.Today,
                                                UpdatedTime = DateTime.Now
                                            };

            return data;
        }

        internal static IesOutletListModel CreateDummyOutletListModel()
        {

            var data = new IesOutletListModel
            {
                AccountManagingOffice = "AccountManagingOffice",
                ActivityId = 100,
                ContractEndDateFrom = DateTime.Now,
                ContractEndDateTo = DateTime.Now,
                ContractRound = "4",
                ContractStartDateFrom = DateTime.Now,
                ContractStartDateTo = DateTime.Now,
                ContractStatus = "Test",
                ContractType = "SSC",
                OrganisationCode = "MSN",
                ProviderId = 0,
                ContractId = "ContractId",
                HasMoreRecords = false,
                NextContractId = String.Empty,
                PostCode = "PostCode",
                RegionAreaGroup = new IesRegionArea[] { new IesRegionArea { RegionArea = "area 1" } },
                ServiceTypeGroup = new IesServiceType[]{ new IesServiceType{ ServiceType = "service type 1"}},
                SiteCode = "BH10",
                Specialisation = "specilisation",
                SupervisingOffice = "supervising office"

            };

            return data;
        }

        internal static OutOutletSearchGroup CreateDummyOutlet(int i)
        {
            var data = new OutOutletSearchGroup()
            {
                ContractID = "contractid",
                Description = "desc",
                EndDate = DateTime.Now,
                SequenceNumber = 1,
                Services = "services",
                SiteCode = "BH10",
                Status = "status"
            };

            return data;
        }

        internal static SiteGetResponse CreateDummySiteDetails(string i)
        {
            var data = new SiteGetResponse
            {
                SiteCode = i,
                SiteCodeDescription = "desc",
                AddressLine1 = "address1",
                AddressLine2 = "address2",
                AddressLine3 = "address3",
                Locality = "locality",
                State = "state",
                Postcode = "postcode",
                AreaCode = "areacode",
                PhoneNumber = "phonenumber",
                FaxNumber = "fax",
                FreeCallNumber = "freecall",
                Email = "email@email.com",
                ContactPerson = "cntct person",
                OfficeTypeCode = "officetypecode",
                OfficeTypeDescription = "desc",
                OrganisationCode = "orgcode",
                OrganisationDescription = "orgdesc",
                ManagingSiteCode = "mngsitecde",
                ManagingSiteDescription = "mandesc",
                SiteSPIndicatorCode = "sitespind",
                AddressDisocsCd = "addrsdiscoc", 
                AddressType = "addressype",
                AddressOfficeType = "addressofficetype",
                AddressLocationNm = "addresslocationnm"
            };

            return data;
        }

        internal static ServiceListResponse CreateDummyServiceList(string i)
        {
            var data = new ServiceListResponse
                           {
                               LastServiceCode = "1", 
                               MoreDataFlag = "N", 
                               OutOutletServices = new OutOutletService[1]
                           };

            data.OutOutletServices[0] = CreateDummyService(i);

            return data;
        }

        internal static OutOutletService CreateDummyService(string i)
        {
            var data = new OutOutletService
                           {
                               Code = i,
                               CreatedBy = "Michael",
                               CreatedDate = new DateTime(),
                               CreatedTime = new DateTime(),
                               EndDate = new DateTime(),
                               StartDate = new DateTime(),
                               Status = "APPROVED",
                               UpdatedBy = "Michael",
                               UpdatedDate = new DateTime(),
                               UpdatedTime = new DateTime()
                           };

            return data;
        }

        #endregion

        public static RegionListResponse CreateDummyRegionListResponse(string i)
        {
            var data = new RegionListResponse
                           {
                               ExecutionResult = new ExecutionResult(),
                               LastRegionCode = "QEW",
                               LastRegionType = "DERP",
                               MoreFlag = "N",
                               OutOutletRegions = new OutOutletRegion[1]
                           };

            data.OutOutletRegions[0] = new OutOutletRegion { AdditionalInformation = "qwer", AssessorAvailFl = "wwe" };

            return data;
        }

        public static OutOutletRegion CreateDummyOutOutletRegion(string i)
        {
            var data = new OutOutletRegion
                           {
                               AdditionalInformation = "12321",
                               AssessorAvailFl = "12344",
                               CapacityLimitCnt = 2,
                               ContactName = "Kohm",
                               ContactPhone = "0288888888",
                               CreationDate = DateTime.Today,
                               CreationTime = DateTime.Now,
                               CreationUserId = "MD2824",
                               CurrentAssmntCnt = 1,
                               IntegrityControlNumber = 1233,
                               PartRegnCoverFl = "OK",
                               RegionCode = "12333322",
                               RegionDescription = "RegDescp",
                               RegionType = "RegType",
                               UpdateDate = DateTime.Today,
                               UpdateTime = DateTime.Now,
                               UpdateUserId = "MD2824"
                           };


            return data;
        }
    }
}
