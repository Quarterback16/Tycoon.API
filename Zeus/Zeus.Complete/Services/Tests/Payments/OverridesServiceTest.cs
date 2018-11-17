using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.SystemOverrides.Contracts.DataContracts;
using Employment.Esc.SystemOverrides.Contracts.DataContracts.Web;
using Employment.Esc.SystemOverrides.Contracts.FaultContracts;
using Employment.Esc.SystemOverrides.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Service.Implementation.Payments;
using Employment.Web.Mvc.Service.Interfaces.Payments;
using Employment.Web.Mvc.Service.Interfaces.Payments.Overrides;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    [TestClass]
    public class OverridesServiceTest
    {
        public TestContext TestContext { get; set; }

        private OverridesService SystemUnderTest()
        {
            return new OverridesService(mockAdwService.Object, mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        
        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new OverridesMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }
                return mappingEngine;
            }
        }

        private static Mock<IContainerProvider> mockContainerProvider;
        private static Mock<IAdwService> mockAdwService;
        private static Mock<IClient> mockClient;
        private static Mock<IMappingEngine> mockMappingEngine;
        private static Mock<ICacheService> mockCacheService;
        private static Mock<IUserService> mockUserService;
        private static Mock<ISystemOverrides> mockOverridesWcf;
        private static Mock<ISystemOverridesWeb> mockOverridesWcfWeb;

        private static readonly string[] departmentRoles = new[] { "DAD" };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            mockAdwService = new Mock<IAdwService>();
            mockClient = new Mock<IClient>();

            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();

            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            mockOverridesWcf = new Mock<ISystemOverrides>();
            mockOverridesWcfWeb = new Mock<ISystemOverridesWeb>();
            mockClient.Setup(m => m.Create<ISystemOverrides>("overrides.svc")).Returns(mockOverridesWcf.Object);
            mockClient.Setup(m => m.Create<ISystemOverridesWeb>("SystemOverrides.svc")).Returns(mockOverridesWcfWeb.Object);
            mockUserService.Setup(x => x.Roles).Returns(departmentRoles);

        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Overrides_Search_Request_Throws_Validation_Error_When_No_Criteria()
        {
            var model = new OverridesSearchCriteria();
            var request = new UESOverrideListRequest(); 
            mockMappingEngine.Setup(x => x.Map<UESOverrideListRequest>(model)).Returns(request);
            SystemUnderTest().Search(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Overrides_Search_Request_Throws_Validation_Error_When_Fault_Exception()
        {
            var exception = new FaultException<SystemOverridesFault>(new SystemOverridesFault());
            var inModel = new OverridesSearchCriteria();
            SetupSearchCriteriaModel(inModel);
            var request = MappingEngine.Map<UESOverrideListRequest>(inModel);
            mockMappingEngine.Setup(x => x.Map<UESOverrideListRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(x => x.ListOverrides(request)).Throws(exception);
            SystemUnderTest().Search(inModel);
        }

        [TestMethod]
        public void Overrides_Search_Request_Valid_Criteria()
        {
            var inModel = new OverridesSearchCriteria();
            SetupSearchCriteriaModel(inModel);
            var request = MappingEngine.Map<UESOverrideListRequest>(inModel);
            var response = new UESOverrideListResponse { OutGroup = new[] { new OutGroup { JobseekerId = 1233445 } } };
            var outModel = MappingEngine.Map<OverridesListModel>(response);

            SetupMockingListOverrides(response, outModel, inModel, request);

            var result = SystemUnderTest().Search(inModel);
            Assert.IsNotNull(result);
            Assert.AreEqual(outModel.Overrides.Count(), result.Overrides.Count());
            Assert.AreEqual(outModel.Overrides.First().JobSeekerId, result.Overrides.First().JobSeekerId);
        }


        private void SetupMockingListOverrides(UESOverrideListResponse response, OverridesListModel outModel,
                                               OverridesSearchCriteria inModel, UESOverrideListRequest request)
        {
            mockMappingEngine.Setup(x => x.Map<UESOverrideListRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(x => x.ListOverrides(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OverridesListModel>(response)).Returns(outModel);
        }

        private void SetupSearchCriteriaModel(OverridesSearchCriteria modelIn)
        {
            modelIn.PlacementType = "RFC";
            modelIn.OrganisationCode = "ORG";
            modelIn.SiteCode = "SITE";
            modelIn.Status = string.Empty;
            modelIn.SpecificTypes = new string[0];
            modelIn.Types = new string[0];
            modelIn.ProgramType = "RJCP";
            modelIn.SupervisingOffice = "BOB";
            modelIn.JobSeekerId = 1233445;
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void OverridesGetRequestThrowsValidationErrorWhenFaultException()
        {
            OverrideModel outModel;
            var exception = new FaultException<SystemOverridesFault>(new SystemOverridesFault());
            var inModel = new OverrideModel();
            inModel.Id = 1234;
            var request = MappingEngine.Map<OscOverrideReadUESRequest>(inModel);
            var key = new KeyModel("OverrideModel").Add(inModel.Id);
            mockCacheService.Setup(x => x.TryGet(key, out outModel)).Returns(false);
            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(x => x.LoadOverride(request)).Throws(exception);

            SystemUnderTest().Get(inModel);
        }

        [TestMethod]
        public void Overrides_Get_Request_Valid_Input()
        {
            OverrideModel cacheModel;
            var inModel = new OverrideModel();
            inModel.Id = 1234;
            var request = MappingEngine.Map<OscOverrideReadUESRequest>(inModel);
            var key = new KeyModel("OverrideModel").Add(inModel.Id);
            mockCacheService.Setup(x => x.TryGet(key, out cacheModel)).Returns(true);
            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(request);

            var response = new OscOverrideReadUESResponse {JobseekerId = 111111};
            var outModel = MappingEngine.Map<OverrideModel>(response);

            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(x => x.LoadOverride(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OverrideModel>(response)).Returns(outModel);

            var result = SystemUnderTest().Get(inModel);
            Assert.IsNotNull(result);
            Assert.AreEqual(outModel.JobSeekerId, result.JobSeekerId);
        }

        [TestMethod]
        public void Overrides_Get_Request_Not_Cached()
        {
            var inModel = new OverrideModel();
            inModel.Id = 1234;
            var request = MappingEngine.Map<OscOverrideReadUESRequest>(inModel);
            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(request);
            var key = new KeyModel("OverrideModel").Add(inModel.Id);
            OverrideModel outModel;
            //outModel.JobSeekerId = 11111;
            mockCacheService.Setup(x => x.TryGet(key, out outModel)).Returns(false);
            

            var response = new OscOverrideReadUESResponse { JobseekerId = 111111, OutStatusHistoryGroup = new OutStatusHistoryGroup[] {new OutStatusHistoryGroup() {TextBlock="Help"}}};
            mockOverridesWcf.Setup(x => x.LoadOverride(request)).Returns(response);
            outModel = MappingEngine.Map<OverrideModel>(response);
            outModel.Comments = MappingEngine.Map<IEnumerable<CommentModel>>(response.OutStatusHistoryGroup);
            mockMappingEngine.Setup(x => x.Map<OverrideModel>(response)).Returns(outModel);
            mockMappingEngine.Setup(x => x.Map<IEnumerable<CommentModel>>(response.OutStatusHistoryGroup)).Returns(outModel.Comments);

            mockCacheService.Setup(x => x.Set(key, outModel));

            var result = SystemUnderTest().Get(inModel);
            Assert.IsNotNull(result);
            Assert.AreEqual(outModel.JobSeekerId, result.JobSeekerId);
            Assert.AreEqual(outModel.Comments.First().Text, result.Comments.First().Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Overrides_Update_Request_Throws_Validation_Error_When_No_Criteria()
        {
            var model = new OverrideModel();
            var request = new OscOverrideUpdateRequest(); 
            mockMappingEngine.Setup(x => x.Map<OscOverrideUpdateRequest>(model)).Returns(request);
            SystemUnderTest().Update(model);
        }

        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]//TODO Fix test
        //public void OverridesUpdateRequestThrowsValidationErrorWhenFaultException()
        //{
        //    OverrideModel outModel;
        //    var exception = new FaultException<SystemOverridesFault>(new SystemOverridesFault());
        //    var inModel = new OverrideModel();
        //    inModel.Id = 1234;
        //    var request = MappingEngine.Map<OscOverrideUpdateRequest>(inModel);
        //    var key = new KeyModel("OverrideModel").Add(inModel.Id);
        //    mockCacheService.Setup(x => x.TryGet(key, out outModel)).Returns(false);
        //    mockMappingEngine.Setup(x => x.Map<OscOverrideUpdateRequest>(inModel)).Returns(request);
        //    mockOverridesWcf.Setup(x => x.UpdateOverride(request)).Throws(exception);

        //    SystemUnderTest().Get(inModel);
        //}

        //[TestMethod]
        //public void OverridesUpdateRequestValidInput()//TODO Fix test
        //{
        //    var outModel = new OverrideModel();
        //    var user = new UserModel() { UserID = "sh0779" };
        //    var inModel = new OverrideModel
        //                      {
        //                          Id = 1234,
        //                          Icn = 1000,
        //                          User = user,
        //                          Status = "Ready",
        //                          SupportingComments = "Comments are here!"
        //                      };

        //    var request = MappingEngine.Map<OscOverrideUpdateRequest>(inModel);
        //    var key = new KeyModel("OverrideModel").Add(inModel.Id);

        //    mockCacheService.Setup(x => x.TryGet(key, out outModel)).Returns(true);
        //    mockMappingEngine.Setup(x => x.Map<OscOverrideUpdateRequest>(inModel)).Returns(request);

        //    var response = new OscOverrideUpdateResponse() { OverrideICN = 1001 };
        //    outModel = MappingEngine.Map<OverrideModel>(response);

        //    mockMappingEngine.Setup(x => x.Map<OscOverrideUpdateRequest>(inModel)).Returns(request);
        //    mockOverridesWcf.Setup(x => x.UpdateOverride(request)).Returns(response);
        //    mockMappingEngine.Setup(m => m.Map(response, new OverrideModel())).Returns(outModel);

        //    var result = SystemUnderTest().Update(inModel);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(outModel.Icn, result.Icn);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void OverridesCreateRequestThrowsValidationErrorWhenNoCriteria()
        //{
        //    var model = new OverrideModel();
        //    var request = MappingEngine.Map<OscOverrideAddUESRequest>(model);
        //    mockMappingEngine.Setup(x => x.Map<OscOverrideAddUESRequest>(model)).Returns(request);
        //    SystemUnderTest().Create(model);
        //}


        [TestMethod]
        public void Overrides_Create_Request_Valid_Input()
        {
            var inModel = new OverrideModel();

            SetupOverrideModelForCreate(inModel);
            var request = MappingEngine.Map<OscOverrideAddRjcpRequest>(inModel);
            mockMappingEngine.Setup(x => x.Map<OscOverrideAddRjcpRequest>(inModel)).Returns(request);
            mockAdwService.Setup(x => x.GetListCodeDescriptionShort("OVA", "SUBT")).Returns("XXX");
            mockAdwService.Setup(x => x.GetListCodeDescriptionShort("ORS", "PEND")).Returns("XXX");
            mockAdwService.Setup(x => x.GetRelatedCodeDescription("SOMT", "TYPE", "SPEC")).Returns("XXX");

            mockUserService.SetupGet(x => x.Username).Returns("SH0779");
            mockUserService.SetupGet(x => x.DateTime).Returns(DateTime.Now);
            var response = new OscOverrideAddRjcpResponse() { OverrideReqId = 100 };
            var outModel = MappingEngine.Map<OverrideModel>(response);
            outModel.Status = "PEND";
            var messageRequest = new MessageRequest();
            
            mockOverridesWcfWeb.Setup(x => x.AddRjcpOverride(request)).Returns(response);
            mockOverridesWcf.Setup(x => x.SendEmail(messageRequest));
            mockMappingEngine.Setup(m => m.Map<OverrideModel>(response)).Returns(outModel);
            mockMappingEngine.Setup(m => m.Map<MessageRequest>(inModel)).Returns(messageRequest);

            var result = SystemUnderTest().Create(inModel);
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Id);
        }

        private void SetupOverrideModelForCreate(OverrideModel model)
        {
            SetupOverrideRequestDetails(model);
            SetupOverrideContactDetails(model);
            SetupOverrideSpecialClaim(model);
            SetupOverrideReferralDetails(model);
        }

        private void SetupOverrideRequestDetails(OverrideModel model)
        {
            model.HasAdvisedCentrelinkIfRequired = true;
            model.HasRequiredDocumentaryEvidence = true;
            model.IsAwareOfAuditingProcedures = true;
            model.IsWorkExperienceOverride = false;
            model.JobSeekerId = 1234565667;
            model.Reason = "NONE";
            model.SpecificType = "SPEC";
            model.Status = "PEND";
            model.Type = "TYPE";
            model.UpdateComments = "Text";
            model.SubmissionDate = DateTime.MinValue;
            model.SupportingComments = "Text";
            model.IsRelatedEntity = true;
            model.IsOwnOrganisation = false;
            model.ApproveRejectReasonCode = "AP01";
        }

        private void SetupOverrideReferralDetails(OverrideModel model)
        {
            model.Referral.ActivityId = 1223445;
            model.Referral.CommencementDate = DateTime.MinValue;
            model.Referral.EmployerId = 99999;
            model.Referral.EmployerName = "Long John Silver";
            model.Referral.OutcomeSnapshotId = 8888;
            model.Referral.PlacementDate = DateTime.MinValue;
            model.Referral.PlacementId = 456456;
            model.Referral.ProgramType = "RJCP";
            model.Referral.ReferralDate = DateTime.MinValue;
            model.Referral.SequenceNumber = 2;
            model.Referral.PlacementType = "S1";
            model.Referral.WorkExperienceStartDate = DateTime.MinValue;
        }

        private void SetupOverrideSpecialClaim(OverrideModel model)
        {
            model.SpecialClaim.ActualPaymentType = "BOOT";
            model.SpecialClaim.RequestedPaymentType = "PEEP";
            model.SpecialClaim.InvoiceAmount = 100.0;
            model.SpecialClaim.DeewrServicesGstAmount = 1.23;
            model.SpecialClaim.InvoiceGstAmount = 10.0;
            model.SpecialClaim.InvoiceId = "MyPaymentDude";
            model.SpecialClaim.NetInputTaxCredits = 11.23;
            model.SpecialClaim.ReimbursementAmount = 0.00;
            model.SpecialClaim.StartDate = DateTime.MinValue;
            model.SpecialClaim.Status = string.Empty;
            model.SpecialClaim.Id = 123;
        }

        private void SetupOverrideContactDetails(OverrideModel model)
        {
            model.Contact.AlternativeEmailAddress = "bob@gmail.com";
            model.Contact.EmailAddress = "bob@hotmail.com";
            model.Contact.FirstName = "bob";
            model.Contact.LastName = "jane";
            model.Contact.PhoneNumber = "0212341234";
            model.Contact.SiteCode = "USRS";
            model.Contact.OrganisationCode = "DEPT";
            model.Contact.UserId = "BJ2000";
            model.Contact.MobileNumber = "0410234234";
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Overrides_Send_Email_Throws_Validation_Error_When_No_Criteria()
        {
            var inModel = new OverrideModel();
            inModel.Id = 1234;
            var getRequest = MappingEngine.Map<OscOverrideReadUESRequest>(inModel);
            var key = new KeyModel("OverrideModel").Add(inModel.Id);
            OverrideModel cacheModel;
            mockCacheService.Setup(x => x.TryGet(key, out cacheModel)).Returns(true);
            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(getRequest);

            var response = new OscOverrideReadUESResponse { JobseekerId = 111111 };
            var outModel = MappingEngine.Map<OverrideModel>(response);

            mockMappingEngine.Setup(x => x.Map<OscOverrideReadUESRequest>(inModel)).Returns(getRequest);
            mockOverridesWcf.Setup(x => x.LoadOverride(getRequest)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<OverrideModel>(response)).Returns(outModel);

            var request = MappingEngine.Map<MessageRequest>(inModel);
            mockMappingEngine.Setup(x => x.Map<MessageRequest>(inModel)).Returns(request);
            mockAdwService.Setup(x=>x.GetListCodeDescriptionShort("OVA", "FORW")).Returns("Forward");
            mockAdwService.Setup(x => x.GetListCodeDescriptionShort("ORS", inModel.Status)).Returns("Approved");
            SystemUnderTest().SendEmail(0, EmailAction.Forward, string.Empty, string.Empty);
        }

        //[TestMethod]
        //public void OverrideSendEmailValidInput()
        //{
        //    var inModel = new OverrideModel();

        //    SetupOverrideModelForCreate(inModel);
        //    var request = MappingEngine.Map<MessageRequest>(inModel);
        //    mockMappingEngine.Setup(x => x.Map<MessageRequest>(inModel)).Returns(request);

        //    var response = new MessageResponse() { successfullysent = true };
        //    //var outModel = MappingEngine.Map<OverrideModel>(response);

        //    mockOverridesWcf.Setup(x => x.SendEmail(request)).Returns(response);
        //    //mockMappingEngine.Setup(m => m.Map<OverrideModel>(response)).Returns(outModel);

        //    SystemUnderTest().SendEmail(inModel,EmailAction.Forward);
        //}
        #region Tax Invoice
        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void TaxInvoiceValidResults()
        {
            var inModel = new OverrideModel
                              {
                                  JobSeekerId = 123456,
                                  Reason = "AAX",
                                  Referral = new ReferralModel {ProgramType = "XGF"},
                                  SupportingComments = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                                  SpecialClaim = new SpecialClaimModel {RequestedPaymentType = "NNC",Id = 123},
                                  Contact = new ContactModel
                                                {
                                                    FirstName = "ABC",
                                                    LastName = "BCF",
                                                    EmailAddress = "abc@xyz.com",
                                                    SiteCode = "ACDA",
                                                    UserId = "ABCD"
                                                },                                  
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceUesOscRequest>(inModel);
            var response = new ClmTaxInvoiceUesOscResponse
                               {
                                   Amount = 100.12,
                                   ProviderId = 123456,
                                   ProviderName = "ABCAC"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceUesOscRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(m => m.GetUesTaxInvoice(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ValidateOverride(inModel);

            Assert.AreEqual(result.Amount, outModel.Amount);
            Assert.AreEqual(result.ProviderId, outModel.ProviderId);
            Assert.AreEqual(result.ProviderName, outModel.ProviderName);
            mockMappingEngine.Verify(m => m.Map<ClmTaxInvoiceUesOscRequest>(inModel), Times.Once());
            mockOverridesWcf.Verify(m => m.GetUesTaxInvoice(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<PaymentsModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void TaxInvoiceWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new OverrideModel
                              {
                                  JobSeekerId = 123456,
                                  Reason = "AAX",
                                  Contract = "XGF",
                                  SupportingComments = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                                  SpecialClaim = new SpecialClaimModel {RequestedPaymentType = "NNC", Id = 123},
                                  Contact = new ContactModel
                                                {
                                                    FirstName = "ABC",
                                                    LastName = "BCF",
                                                    EmailAddress = "abc@xyz.com",
                                                    SiteCode = "ACDA",
                                                    UserId = "ABCD"
                                                }                              
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceUesOscRequest>(inModel);
            var response = new ClmTaxInvoiceUesOscResponse
                               {
                                   Amount = 100.12,
                                   ProviderId = 123456,
                                   ProviderName = "ABCAC"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceUesOscRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(m => m.GetUesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateOverride(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void TaxInvoiceThrowsFaultExceptionThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new OverrideModel
                              {
                                  JobSeekerId = 123456,
                                  Reason = "AAX",
                                  Contract = "XGF",
                                  SupportingComments = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                                  SpecialClaim = new SpecialClaimModel {RequestedPaymentType = "NNC",Id = 123},
                                  Contact = new ContactModel
                                                {
                                                    FirstName = "ABC",
                                                    LastName = "BCF",
                                                    EmailAddress = "abc@xyz.com",
                                                    SiteCode = "ACDA",
                                                    UserId = "ABCD"
                                                }
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceUesOscRequest>(inModel);
            var response = new ClmTaxInvoiceUesOscResponse
                               {
                                   Amount = 100.12,
                                   ProviderId = 123456,
                                   ProviderName = "ABCAC"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceUesOscRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(m => m.GetUesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateOverride(inModel);
        }
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void TaxInvoiceThrowsFaultExceptionThrowsPaymentsFault()
        {
            var exception = new FaultException<SystemOverridesFault>(new SystemOverridesFault { Message = "Exception" });

            var inModel = new OverrideModel
                              {
                                  JobSeekerId = 123456,
                                  Reason = "AAX",
                                  Contract = "XGF",
                                  SupportingComments = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                                  SpecialClaim = new SpecialClaimModel {RequestedPaymentType = "NNC",Id=123},
                                  Contact = new ContactModel
                                                {
                                                    FirstName = "ABC",
                                                    LastName = "BCF",
                                                    EmailAddress = "abc@xyz.com",
                                                    SiteCode = "ACDA",
                                                    UserId = "ABCD"
                                                }
                              };
            var request = MappingEngine.Map<ClmTaxInvoiceUesOscRequest>(inModel);
            var response = new ClmTaxInvoiceUesOscResponse
                               {
                                   Amount = 100.12,
                                   ProviderId = 123456,
                                   ProviderName = "ABCAC"
                               };

            var outModel = MappingEngine.Map<PaymentsModel>(response);

            mockMappingEngine.Setup(m => m.Map<ClmTaxInvoiceUesOscRequest>(inModel)).Returns(request);
            mockOverridesWcf.Setup(m => m.GetUesTaxInvoice(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<PaymentsModel>(response)).Returns(outModel);

            SystemUnderTest().ValidateOverride(inModel);
        }
        #endregion Tax Invoice


        #region Outcome Snapshot List

        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void OverridesOutcomeSnapshotListThrowsValidationErrorWhenNoCriteria()
        //{
        //    var model = new OutcomeSnapshotListModel();
        //    var key = new KeyModel("OverrideModel").Add(inModel.Id);
        //    mockCacheService.Setup(x => x.TryGet(key, out cacheModel)).Returns(true);
        //    var request = MappingEngine.Map<OscSnapListRequest>(model);
        //    mockMappingEngine.Setup(x => x.Map<OscSnapListRequest>(model)).Returns(request);
        //    SystemUnderTest().GetOutcomeSnapshotList(model);
        //}

        #endregion
    }
}
