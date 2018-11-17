using System;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.JCAAssessment.Contracts.DataContracts;
using Employment.Esc.JCAAssessment.Contracts.ServiceContracts;
using Employment.Esc.JobseekerCaseDetails.Contracts.DataContracts;
using Employment.Esc.JobseekerCaseDetails.Contracts.ServiceContracts;
using Employment.Esc.Provider.Contracts.DataContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Case;
using Employment.Web.Mvc.Service.Interfaces.Case;
using Microsoft.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Case
{
    /// <summary>
    /// Summary description for CaseSummaryServiceTest
    /// </summary>
    [TestClass]
    public class CaseSummaryServiceTest
    {
        private CaseSummaryService SystemUnderTest()
        {
            return new CaseSummaryService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }
        private IMappingEngine mappingEngine;
        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new CaseSummaryMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IClient> mockClient;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IJobseekerCaseDetails> mockCaseSummaryWcf;
        private Mock<ISessionService> mockSessionService;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClaimsIdentity> mockIdentity;
        private Mock<IJCAAssessment> mockJcaAssessmentWcf; 
        private readonly ExecutionResult mSuccessResult = new ExecutionResult {Status = ExecuteStatus.Success};

        /// <summary>
        /// Use TestInitialize to run code before running each test
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockCaseSummaryWcf = new Mock<IJobseekerCaseDetails>();
            mockJcaAssessmentWcf = new Mock<IJCAAssessment>();
            mockSessionService = new Mock<ISessionService>();
            mockUserService.SetupGet(u => u.Session).Returns(mockSessionService.Object);
            mockClient.Setup(m => m.Create<IJobseekerCaseDetails>("JobseekerCaseDetails.svc")).Returns(mockCaseSummaryWcf.Object);
            mockClient.Setup(m => m.Create<IJCAAssessment>("JCAAssessment.svc")).Returns(mockJcaAssessmentWcf.Object);
            mockIdentity = new Mock<IClaimsIdentity>();
            mockIdentity.SetupGet(id => id.Name).Returns("JT2554");
            mockUserService.SetupGet(us => us.Identity).Returns(mockIdentity.Object);

            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        [TestMethod]
        public void GetCaseSummaryTest()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);

            CaseDetailsRequest request = new CaseDetailsRequest();
            var response = new CaseDetailsRemoteResponseEx
            {
                ActionJobseekerItems = new ActionJobseekerItem[]
                                           {
                                               
                                           },
                AdditionalInformationDetails = new AdditionalInformationDetailRemote()
                {
                    //ClaimFlag = "Y",
                    ClientNotes = "I am a dog!",
                    ManagedBy = "RT2637",
                    SpecialRequirements = "Needs to be fed twice a day"
                },
                AllowanceDetails = new AllowanceDetailRemote()
                                       {
                                           AllowanceEndDate = DateTime.Now, AllowanceRate = "F", AllowanceRateDesc = "Full", AllowanceStartDate = DateTime.Now, AllowanceType = "NSA", AllowanceTypeDesc = "New Start Allowance"
                                       },
                CommencementDetails = new CommencementDetailRemote()
                {

                },
                DiaryResponse = new DiaryAppointmentsRemote()
                                    {
                                        InitialAppointment = new DiaryAppointmentRemote(), NextAppointment = new DiaryAppointmentRemote(), PreviousAppointment = new DiaryAppointmentRemote()
                                    },
                EarningsInformationList = new EarningsResponseItem[]
                                              {
                                                  
                                              },
                FactorsAndEligibilityDetails = new FactorsAndEligibilityDetailRemote()
                {
                    ApprenticeFlag = true,
                    CareerAdviceEligibleFlag = true,
                    ComplianceFlag = true,
                    EslFlag = true
                },
                ExecutionResult = mSuccessResult,
                JcaDetails = new JcaDetailRemote()
                                 {
                                     EffectiveStatusCode = "A",
                                     EffectiveStatusDate = DateTime.Now,
                                     EffectiveStatusDesc = "Buh",
                                     ExpiryDate = DateTime.Now.AddDays(21),
                                     LastEffectiveDate = DateTime.Now,
                                 },
                JsciDetails = new JsciDetailRemote()
                                  {
                                     DisabilityCode     =  "A",
                                     HighLevelEducationCode = "B",
                                     PersonalFactors = "C",
                                     SpecialNeeds = "D",
                                     SpecialNeedsDescription = "F",
                                     StatusCode = "Z",
                                     StatusDescription = "X"
                                  },
                LifecycleDetails = new LifecycleDetailRemote()
                                       {
                                           ActualEndDate = DateTime.Now,
                                           ApprovedApplicationPresent = true,
                                           CommencementDate = DateTime.Now,
                                       },
                                       JobseekerPersonalDetails = new JobseekerPersonalDetailRemote()
                                                                      {
                                                                          Community = "ABC",
                                                                          CommunityDesc = "Buh",
                                                                          ContactNumber = "123456"
                                                                      }

            };

            mockCaseSummaryWcf.Setup(m => m.GetCaseDetailsRemoteEx(It.IsAny<CaseDetailsRequest>())).Returns(response);

            var result = SystemUnderTest().GetCaseSummary(123456);

            Assert.IsNotNull(result.ActionJobseekerList);
            Assert.IsNotNull(result.AdditionalInformationDetails);
            Assert.IsNotNull(result.AllowanceDetails);
            Assert.IsNotNull(result.CommencementDetails);
            Assert.IsNotNull(result.DiaryAppointments);
            Assert.IsNotNull(result.Earnings);
            Assert.IsNotNull(result.ExitReasonList);
            Assert.IsNotNull(result.FactorsAndEligibilityDetails);
            Assert.IsNotNull(result.JcaDetails);
            Assert.IsNotNull(result.JsciDetails);
            Assert.IsNotNull(result.JobseekerPersonalDetails);
            Assert.IsNotNull(result.LifecycleDetails);
        }

        [TestMethod]
        public void UpdatedCaseDetails()
        {
            CaseSummaryModel caseSummaryModel = new CaseSummaryModel()
                                                    {
                                                        Action = "",
                                                        ActionReason = "",
                                                        JobseekerPersonalDetails = new JobseekerPersonalModel()
                                                                                       {
                                                                                            Site = "",
                                                                                            JobseekerICN = 1,
                                                                                           JobseekerId = 12345
                                                                                       },
                                                         LifecycleDetails = new LifecycleModel()
                                                                                {
                                                                                    CommencementDate = DateTime.Now,
                                                                                    PlacementICN = 1,
                                                                                    PlacementSeqNo = 2,
                                                                                    ReferralDate = DateTime.Now,
                                                                                    ReferralICN = 3,
                                                                                    ReferralSeqNo = 1
                                                                                },

                                                         AdditionalInformationDetails = new AdditionalInformationModel()
                                                                                            {

                                                                                                ClientNotes = "Buh",
                                                                                                ManagedBy = "RT2637_D",
                                                                                                SpecialRequirements = "Fuh"
                                                                                            }

                                                    };

            var response = new UpdateDetailsResponse()
            {
                ExecutionResult = mSuccessResult,
            };

            mockCaseSummaryWcf.Setup(m => m.UpdateJobseekerCaseDetailsOnly(It.IsAny<UpdateDetailsRequest>())).Returns(response);

            var result = SystemUnderTest().UpdateCaseDetails(caseSummaryModel);

            Assert.IsNotNull(result);

            
        }

        [TestMethod]
        public void CommenceTest()
        {
            var request = new CaseSummaryModel()
                {
                    Action = "",
                    ActionReason = "",
                    JobseekerPersonalDetails = new JobseekerPersonalModel()
                                                   {
                                                       Site = "QM60",
                                                       JobseekerICN = 1,
                                                       JobseekerId = 12345
                                                   },
                    CommencementDetails = new CommencementModel(),
                    LifecycleDetails = new LifecycleModel(){PlacementStartDate = DateTime.Now}
                };

            var wcfResponse = new UpdateDetailsRemoteResponse()
                                  {
                                      ExecutionResult = mSuccessResult,
                                      RjcpCommencementResponse = new RjcpProviderCommenceJobseekerResponse()
                                                                     {
                                                                            ConfirmationMessage = "",
                                                                            CreditId = 1,
                                                                            CreditSeqNum = 1,
                                                                            CreditIntCtrNum = 1,
                                                                            PlacementSeqNum = 1,
                                                                            PlacementType = "",
                                                                            PlacementStatus = "",
                                                                            CommencementDate = DateTime.Now,
                                                                            PlacementIntCtrNum = 1,
                                                                            IppFoundFlag = "Y",
                                                                            PlacementComFlag  = "Y",
                                                                            HasTaxInvoice = "Y",
                                                                            ScheduleId = 1,
                                                                            SchDtlSeqNum = 2,
                                                                            RateCd = "D",
                                                                            PaymentType = "F",
                                                                            ClaimSchDtlIntCntlNum = 0 ,
                                                                            ExecutionResult = mSuccessResult
                                                                     }
                                  };

            mockCaseSummaryWcf.Setup(m => m.CommenceJobseekerRemote(It.IsAny<UpdateDetailsRemoteRequest>())).Returns(wcfResponse);

            var result = SystemUnderTest().Commence(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEarningsTest()
        {
            var request = new CaseSummaryModel()
            {
                JobseekerPersonalDetails = new JobseekerPersonalModel()
                {
                    Site = "QM60",
                    JobseekerICN = 1,
                    JobseekerId = 12345,
                    CRN = "123456789",
                    Surname = "Gillard"
                }
            };

            var wcfResponse = new GetLastEarningResponse()
            {
                LastEarningDate = new DateTime (2013, 6, 1)
            };

            mockJcaAssessmentWcf.Setup(m => m.GetLastEarningsFromCLINK(It.IsAny<GetLastEarningsRequest>())).Returns(wcfResponse);

            var result = SystemUnderTest().GetEarnings(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateEarningsTest()
        {
            var wcfResponse = new TriggerEarningsResponse()
            {
            };

            mockCaseSummaryWcf.Setup(m => m.TriggerEarningsInformation(It.IsAny<TriggerEarningsRequest>())).Returns(wcfResponse);

            SystemUnderTest().UpdateEarnings(123456789, DateTime.Now);
        }
    }
}
