using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employment.Esc.ActivityManagement.Contracts.DataContracts;
using Employment.Web.Mvc.Service.Interfaces.Activity;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Employment.Esc.Shared.Contracts.Execution;

namespace Employment.Web.Mvc.Service.Tests.Activity
{
    internal static class ActivityTestDataHelper
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

        #region activity dummy data
        internal static ActivityListModel CreateDummyActivityListModel()
        {

            var data = new ActivityListModel
            {
                SearchType = "ACTIVITYTYPE",
                ActivityType = "AETV",
                SubType = "C4",
                ESACode = "4ACQ",
                Status = "CL",
                //FromPostCode = "2121",
                StartActivityId = 123123,
                StartActivityStartDate = DateTime.Today
            };

            return data;
        }
        internal static ACTList CreateDummyACTList(int i)
        {
            var data = new ACTList()
            {
                ActivityId = 12340 + i,
                ActivityType = "AETV",
                SubType = "C4",
                OrgCode = "SALV",
                ContractType = "Certificate IV in Training and Assessment",
                StartDate = DateTime.Today.AddDays(-i),
                EndDate = DateTime.Today.AddDays(i),
                TitleText = "Certificate IV in Training and Assessment" + i,
                MultiLocation = "N",
                MyOrganisationOnly = "Y",
                GroupedBasedActivity = "N",
                Status = "CL"
            };

            return data;
        }

        internal static ACTDetailsExResponse CreateDummyACTDetailsExResponse()
        {

            var data = new ACTDetailsExResponse
            {
                ActivityName = "Certificate IV in Training and Assessment",
                ActivityDescription = " this is test",
                ActivityType = "AETV",
                SubType = "C4",
                OrgCode = "SALV",
                SiteCode = "FV60",
                StartDate = DateTime.Today.AddDays(-10),
                EndDate = DateTime.Today.AddDays(10),
                Status = "CL",
                ContractId = "123123123",
                RelatedIdentifier = "33311A",
                RiskAssessmentDate = DateTime.Today.AddDays(-2),
                MyOrganisationOnly = "Y",
                ExternallyHosted = "N",
                InnovationFund = "N",
                GroupedBasedActivity = "N",
                PoliceCheck = "Some",
                PlacementAvailableDate = DateTime.Today.AddDays(11),
                IntegrityControlNumber = 123
            };

            //Adw Code: AZI
            IndustryCodes[] industryCodes = {
                                               new IndustryCodes(){ IndustryCode="A"}, 
                                               new IndustryCodes(){ IndustryCode="B"}, 
                                               new IndustryCodes(){ IndustryCode="C"}
                                           };


            RelatedActivityId[] relatedActivityIds = {
                                               new RelatedActivityId(){ RelatedActId=1}, 
                                               new RelatedActivityId(){ RelatedActId=2}, 
                                               new RelatedActivityId(){ RelatedActId=3}, 
                                           };
            data.IndustryCode = industryCodes;
            data.RelatedActivityIds = relatedActivityIds;
            //Location
            data.LocMoreFlag = "";
            data.LocSeqNum = 12321;
            data.LocOrganisationName = "Host 123";
            data.LocLocationCode = "4NOS";
            data.LocStatusCode = "OP";
            data.LocAddressLine1 = "Line 1";
            data.LocAddressLine2 = "Line 2";
            data.LocAddressLine3 = "Line 3";
            data.LocAddressLocation = "10204396";
            data.LocPostCode = "2000";
            data.LocStateCode = "NSW";
            data.LocTitle = "MR";
            data.LocFirstName = "Joe";
            data.LocLastName = "Hou";
            data.LocPhoneNumber = "Ph98888888";
            data.LocFaxNumber = "Fax 022342";
            data.LocEmailAddress = "asd@com.au";
            data.LocICN = 1;
            //Host
            data.HostMoreFlag = "";
            data.HostId = 12321;
            data.HostLinkSeqNum = 11232;
            data.HostStatusCode = "OP";
            data.HostTypeCode = "INT";
            data.HostOrganisationName = "Host org";
            data.HostLinkICN =1;

            return data;
        }
        #endregion

        #region Activity location dummy data
        internal static LocationDetails CreateDummyACTLocDetails(int i)
        {
            var data = new LocationDetails()
            {
                HostName = "Test 123" + i.ToString(),
                LocationSeqNumber = i,
                AddressLine1 = "line1",
                AddressLine2 = "line2",
                AddressLine3 = "line3",
                AddressLocation = "AL12345",
                ContactName = "Joe Hou",
                EmailAddress = "joe.hou@deewr.gov.au",
                ESA = "4NOS",
                FirstName = "Joe",
                LastName = "Hou",
                LocationCode = "2345",
                StatusCode = "OP"
            };

            return data;
        }

        internal static ACTGetLocDetailsResponse CreateDummyACTGetLocDetailsResponse()
        {
            var data = new ACTGetLocDetailsResponse()
            {
                //Host link info
                HostID=123,
                ActHostSeqNum=1, //hostLinkSeqNum
                HostName = "Test123",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                AddressLine3 = "Line3",
                AddressLocation = "12312",
                PostCode = "2000",
                StateCode = "NSW",
                Title = "MISS",
                FirstName = "Joe",
                LastName = "Hou",
                ContactNumber = "98881111",
                EmailAddress = "joe.hou@deewr.gov.au",
                LocationCode="4ALS", //ESA
                INTControlNumber = 12,
            };
           
            return data;
        }

        internal static ActivityLocationModel CreateDummyActivityLocationModel()
        {

            var data = new ActivityLocationModel()
            {
                ActivityId = 123,
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                FirstName = "Joe",
                LastName = "Hou",
                IntegrityControlNumber = 1, 
                HostName = "Host123",
                Title = "T1",
                ContactNumber = "98888888",
                PostCode = "2000",
                State = "NSW",
                StatusCode = "OP",
                Suburb = "10204396"
            };

            return data;
        }

        #endregion

        #region Activity host dummy data
        internal static ACTHostList CreateDummyACTHostList(int i)
        {
            var data = new ACTHostList()
            {
                HostID=i,
                OrganisationName = "Training Co.",
                HostTypeCode = "INT",
                StatusCode = "OP",
                HostSeqNum = 1, //Host link sequence num
                HostIntControlNumber = 123 //Host link ICN
            };

            return data;
        }

        internal static ACTGetHostDetailsResponse CreateDummyACTGetHostDetailsResponse()
        {
            var data = new ACTGetHostDetailsResponse()
            {
                OrganisationName = "Test123",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                AddressLine3 = "Line3",
                Suburb = "12312",
                PostCode = "2000",
                StateCode = "NSW",
                Title = "MISS",
                FirstName = "Joe",
                LastName = "Hou",
                ContactNumber = "98881111",
                EmailAddress = "joe.hou@deewr.gov.au",
                HostTypeCode = "INT",
                SocialEnterpriseindicator = "Y",
                OwningOrganisation = "MISN",
                IntegrityControlNumber = 12,
            };

            return data;
        }

        //internal static ActivityLocationModel CreateDummyActivityLocationModel()
        //{

        //    var data = new ActivityLocationModel()
        //    {
        //        ActivityId = 123,
        //        AddressLine1 = "Line 1",
        //        AddressLine2 = "Line 2",
        //        AddressLine3 = "Line 3",
        //        FirstName = "Joe",
        //        LastName = "Hou",
        //        IntegrityControlNumber = 1,
        //        HostName = "Host123",
        //        Title = "T1",
        //        ContactNumber = "98888888",
        //        PostCode = "2000",
        //        State = "NSW",
        //        StatusCode = "OP",
        //        Suburb = "10204396"
        //    };

        //    return data;
        //}

        #endregion
    }
}
