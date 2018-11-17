using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.IESContracts.Contracts.DataContracts;
using Employment.Esc.IESContracts.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Interfaces.JSCI.Types;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;

using Employment.Esc.JSCI.Contracts.DataContracts;
using Employment.Esc.JSCI.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.JSCI;
using Employment.Web.Mvc.Service.Interfaces.JSCI;

namespace Employment.Web.Mvc.Service.Tests.JSCI
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class JsciServiceTest
    {
        private JsciService SystemUnderTest()
        {
            return new JsciService(mockClient.Object, MappingEngine, mockCacheService.Object, mockRegistrationService.Object, mockAdwService.Object);
        }
        private IMappingEngine mappingEngine;
        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new JsciMapper(); 
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private readonly Mock<IClient> mockClient = new Mock<IClient>();
        private readonly Mock<ICacheService> mockCacheService = new Mock<ICacheService>();
        private readonly Mock<IRegistrationService> mockRegistrationService = new Mock<IRegistrationService>();
        private readonly Mock<IAdwService> mockAdwService = new Mock<IAdwService>();

        private readonly Mock<IUserService> mockUserService = new Mock<IUserService>();
        private readonly Mock<IJSCI> mockJsciWcf = new Mock<IJSCI>();
        private readonly Mock<ISite> mockProviderWcf = new Mock<ISite>();
        private readonly Mock<ISessionService> mockSessionService = new Mock<ISessionService>();
        private readonly Mock<IContainerProvider> mockContainerProvider = new Mock<IContainerProvider>();
        //private readonly Mock<IClaimsIdentity> mockIdentity = new Mock<IClaimsIdentity>();
        //private readonly Mock<IHistoryService> mockHistoryService = new Mock<IHistoryService>();
        private readonly long mJobseekerId = 1;
        private readonly ExecutionResult mSuccessResult = new ExecutionResult {Status = ExecuteStatus.Success};
        private readonly JobseekerModel mJobseeker = new JobseekerModel
        {
            JobSeekerId = 1,
            Title = "Mr",
            GivenName = "John",
            Surname = "Smith",
            CRN = "x12345678",
            IsLinkedToOrg = true,
            DateOfBirth = new DateTime(1992, 1, 1),
            CurrentRegistrationDetails = new RegistrationPeriod { Active = true }
        };
        private readonly ValidationFault mFault = new ValidationFault
        {
            Details = new List<ValidationDetail> { new ValidationDetail { Key = "a", Message = "b" } }
        };

        private readonly DateTime now = DateTime.Now;

        private readonly ReadQuestionsResponse BasicReadResponse = new ReadQuestionsResponse
            {
                GroupJsciQuestions = new ReadQuestionDetailsResponse[0],
                JSCIAssessmentStartDate = DateTime.Now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + DateTime.Now.TimeOfDay,
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success },
                CurrentJSCI = "Y"
            };

        private readonly int CriminalAgeLowerBoundary = 23;
        private readonly int CriminalAgeUpperBoundary = 28;
        private readonly int PersonalCharacteristicsAgeBoundary = 45;
        
        /// <summary>
        /// Use TestInitialize to run code before running each test
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            //mockUserService.SetupGet(u => u.Session).Returns(mockSessionService.Object);
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(mJobseeker);
            mockClient.Setup(m => m.Create<IJSCI>("JSCI.svc")).Returns(mockJsciWcf.Object);
            mockClient.Setup(m => m.Create<ISite>("IESSite.svc")).Returns(mockProviderWcf.Object);
            //mockProviderWcf.Setup(m => m.GetSiteDetails(It.IsAny<SiteGetRequest>())).Returns(new SiteGetResponse()); // set if needed

            //mockIdentity.SetupGet(id => id.Name).Returns("JT2554");
            //mockUserService.SetupGet(us => us.Identity).Returns(mockIdentity.Object);

            //mockUserService.SetupGet(u => u.History).Returns(mockHistoryService.Object);
            mockUserService.SetupGet(u => u.DateTime).Returns(now);

            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        #region Constructor tests

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new JsciService(null, null, null, null, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullRegoService_ThrowsArgumentNullException()
        {
            new JsciService(mockClient.Object, MappingEngine, mockCacheService.Object, null, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullAdwService_ThrowsArgumentNullException()
        {
            new JsciService(mockClient.Object, MappingEngine, mockCacheService.Object, mockRegistrationService.Object, null);
        }

        #endregion

        #region ListHistory

        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ListHistory_Valid()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var inModel = new JsciListModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker };
            var response = new ListHistoryResponse
                               {
                                   JSCIBlocks = new[]
                                                    {
                                                        new ListHistoryBlock
                                                            {
                                                                //ArchiveIndicator = "a",
                                                                CreationUserId = "b",
                                                                //DocEvidenceOutcome = "c",
                                                                //JSCIResult = "d",
                                                                //JSCIScore = 2,
                                                                //PriorityDate = now,
                                                                //ProcessCode = "e",
                                                                //ScoreAssessmentDate = now,
                                                                //SecondaryClassificationRequired = "f",
                                                                //SecondaryResultCode = "g",
                                                                //SecondaryResultDate = now,
                                                                //SnaAssessmentRequired = "h",
                                                                //SnaOutcomeDate = now,
                                                                //SnaOutcomeResult = "i",
                                                                //StartDate = now,
                                                                //StartTime = now,
                                                                StatusCode = "A",
                                                                //UpdateUserID = "k",
                                                                //WatAssessmentRequired = "l",
                                                                //WatIndexDate = now
                                                            },
                                                        new ListHistoryBlock
                                                            {
                                                                StatusCode = "A",
                                                                CreationUserId = "b",
                                                                SecondaryClassificationRequired = "Y",
                                                                SnaAssessmentRequired = "N",
                                                                WatAssessmentRequired = "X",
                                                            },
                                                        new ListHistoryBlock
                                                            {
                                                                StatusCode = "A",
                                                                CreationUserId = "b",
                                                                SecondaryClassificationRequired = "R",
                                                                SnaAssessmentRequired = "E",
                                                                WatAssessmentRequired = "W",
                                                            }

                                                    },
                                   ExecutionResult = mSuccessResult
                               };
            //var outModel = MappingEngine.Map<IEnumerable<HistoryItemModel>>(response.JSCIBlocks).ToList();
            mockJsciWcf.Setup(m => m.ListHistory(It.IsAny<ListHistoryRequest>())).Returns(response);

            var result = SystemUnderTest().ListHistory(inModel);

            Assert.AreEqual(mJobseeker, result.Jobseeker);
            Assert.IsTrue(result.History.Count() == response.JSCIBlocks.Length);
            Assert.IsTrue(result.History.First().CreateUserID == response.JSCIBlocks[0].CreationUserId);
            mockJsciWcf.Verify(m => m.ListHistory(It.IsAny<ListHistoryRequest>()), Times.Once());
        }

        /// <summary>
        /// Test IsInRoleValueResolver part of CanViewFromHistoryValueResolver.
        /// </summary>
        [TestMethod]
        public void ListHistory_InactiveJsci()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var inModel = new JsciListModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker };
            var response = new ListHistoryResponse
                               {
                                   JSCIBlocks = new[]
                                                    {
                                                        new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "I"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "X"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "P"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "R"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "D"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "D"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "C"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "V"
                                                            }
                                                        ,new ListHistoryBlock
                                                            {
                                                                CreationUserId = "b",
                                                                StatusCode = "~"
                                                            }
                                                    },
                                   ExecutionResult = mSuccessResult
                               };
            mockJsciWcf.Setup(m => m.ListHistory(It.IsAny<ListHistoryRequest>())).Returns(response);

            var result = SystemUnderTest().ListHistory(inModel);

            Assert.IsTrue(result.History.Count() == response.JSCIBlocks.Length);
            Assert.IsTrue(result.History.First().CreateUserID == response.JSCIBlocks[0].CreationUserId);
            Assert.IsTrue(result.History.First().Status.Equals(JsciStatus.Inactive));
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="ValidationFault" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListHistory_ThrowsValidationFault()
        {
            mockJsciWcf.Setup(w => w.ListHistory(It.IsAny<ListHistoryRequest>())).Throws(
                new FaultException<ValidationFault>(mFault));
            SystemUnderTest().ListHistory(new JsciListModel());
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ListHistory_JobseekerNotAttachedToOrg()
        {
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(new JobseekerModel { JobSeekerId = mJobseekerId, DateOfBirth = new DateTime(1990, 1, 1), CurrentRegistrationDetails = new RegistrationPeriod { Active = true } });
            var inModel = new JsciListModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker };
            SystemUnderTest().ListHistory(inModel);
            mockJsciWcf.Verify(m => m.ListHistory(It.IsAny<ListHistoryRequest>()), Times.Never());
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ListHistory_JobseekerNotFound()
        {
            mockUserService.SetupGet(u => u.Session).Returns(mockSessionService.Object);
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(new JobseekerModel { JobSeekerId = mJobseekerId });
            var inModel = new JsciListModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker };
            SystemUnderTest().ListHistory(inModel);
        }
        
        #endregion

        #region ReadQuestions

        /// <summary>
        /// User is DAD, Jsci is not archived, and is pending. So CopyJca is possible.
        /// </summary>
        [TestMethod]
        public void ReadQuestions_Valid()
        {
            mockUserService.Setup(u => u.IsInRole(It.Is<string[]>(r => r.Contains("DAD")))).Returns(true);
            var inModel = new JsciModel { JobSeekerID = mJobseekerId };
            var response = new ReadQuestionsResponse
            {
                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                ArchiveInd = "",
                JSCIStatusCode = "P",
                ExecutionResult = mSuccessResult,
                GroupJsciQuestions = GetBasicQuestionResponses()
            };

            var expectedModel = MappingEngine.Map<JsciModel>(response);
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);
            var actualModel = SystemUnderTest().ReadQuestions(inModel);

            Assert.IsNotNull(actualModel);
            Assert.AreEqual(expectedModel.AssessmentStartDateTime, actualModel.AssessmentStartDateTime);
            mockJsciWcf.Verify(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>()), Times.Once());
            Assert.IsTrue(actualModel.IsReasonForChangeRequired());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ReadQuestions_FreezeYr12()
        {
            //if SP and
            //jskr has CRN and 
            //jskr is under 21 years and
            //(q HSCH) is Yr 12/13 (r HSCD) 
            //then hide all other options.
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(new JobseekerModel
                                {
                                    JobSeekerId = mJobseekerId,
                                    CRN = "X", //has CRN
                                    DateOfBirth = now.AddYears(20), //under 21
                                    IsLinkedToOrg = true,
                                    CurrentRegistrationDetails = new RegistrationPeriod { Active = true}
                                });

            mockUserService.Setup(u => u.IsInRole(It.Is<string[]>(r => r.Contains("SPS")))).Returns(true); //user is SP
            mockProviderWcf.Setup(m => m.GetSiteDetails(It.IsAny<SiteGetRequest>())).Returns(new SiteGetResponse());

            var inModel = new JsciModel { JobSeekerID = mJobseekerId };
            var response = new ReadQuestionsResponse
            {
                JSCIAssessmentStartDate = now,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                //ArchiveInd = "",
                JSCIStatusCode = "P", //not new
                ExecutionResult = mSuccessResult,
                GroupJsciQuestions = GetBasicQuestionResponses()
            };
            var highestLevelOfSchooling = response.GroupJsciQuestions.First(jq => jq.QuestionCode == QuestionCodes.HSCH);
            highestLevelOfSchooling.ResponseOption = new List<QuestionAnswer> //has response options to remove
                                                         {
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCD, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCA, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCE, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCI, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCJ, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCK, ResponseDescription = "XX"},
                                                             new QuestionAnswer {ResponseCode = ResponseCodes.HSCL, ResponseDescription = "XX"}
                                                         };
            highestLevelOfSchooling.ResponseCode = ResponseCodes.HSCD; //did yr12

            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);

            var result = SystemUnderTest().ReadQuestions(inModel);

            mockJsciWcf.Verify(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>()), Times.Once());

            Assert.IsTrue(result.QuestionResponses[QuestionCodes.HSCH].ResponseOptions.Count == 1, "DR01029075"); //also false EditableIf in vm
            Assert.IsTrue(result.QuestionResponses[QuestionCodes.HSCH].ResponseOptions.First().Key == ResponseCodes.HSCD, "DR01029075");
            Assert.IsTrue(result.QuestionResponses[QuestionCodes.HSCH].ResponseCode == ResponseCodes.HSCD, "DR01029075");
        }

        /// <summary>
        /// IsReasonForChangeRequired() true/false test
        /// </summary>
        [TestMethod]
        public void ReadQuestions_IsReasonForChangeRequired()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            mockProviderWcf.Setup(m => m.GetSiteDetails(It.IsAny<SiteGetRequest>())).Returns(new SiteGetResponse());

            var inModel = new JsciModel { JobSeekerID = mJobseekerId };
            var response = new ReadQuestionsResponse
            {
                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                ArchiveInd = "",
                ExecutionResult = mSuccessResult
            };
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);
            response.JSCIStatusCode = "P"; //pending

            var actualModelTrue = SystemUnderTest().ReadQuestions(inModel);
            mockJsciWcf.Verify(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>()), Times.Once());

            Assert.IsTrue(actualModelTrue.IsReasonForChangeRequired());
            response.JSCIStatusCode = "X"; //incomplete
            var actualModelFalse = SystemUnderTest().ReadQuestions(inModel);
            Assert.IsFalse(actualModelFalse.IsReasonForChangeRequired());
        }

        /// <summary>
        /// User is DAD, Jsci is not archived, and is pending. So CopyJca is possible.
        /// </summary>
        [TestMethod]
        public void ReadQuestions_GetQuestionsTextArchived()
        {
            mockUserService.Setup(u => u.IsInRole(It.Is<string[]>(r => r.Contains("DAD")))).Returns(true);
            var inModel = new JsciModel {JobSeekerID = mJobseekerId };
            var response = new ReadQuestionsResponse
            {
                GroupJsciQuestions = GetBasicQuestionResponses(),
                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                ArchiveInd = "A",
                AssessmentId = 1,
                JSCIStatusCode = "P",
                ExecutionResult = mSuccessResult
            };

            var expectedModel = MappingEngine.Map<JsciModel>(response);
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);

            var actualModel = SystemUnderTest().ReadQuestions(inModel);

            Assert.IsNotNull(actualModel);
            Assert.AreEqual(expectedModel.AssessmentStartDateTime, actualModel.AssessmentStartDateTime);
            mockJsciWcf.Verify(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>()), Times.Once());
            Assert.IsFalse(actualModel.IsReasonForChangeRequired());
        }


        /// <summary>
        /// User is DAD, Jsci is not archived, and is pending. So CopyJca is possible.
        /// </summary>
        [TestMethod]
        public void ReadQuestions_GetQuestionsTextNotArchived()
        {
            mockUserService.Setup(u => u.IsInRole(It.Is<string[]>(r => r.Contains("DAD")))).Returns(true);
            var inModel = new JsciModel { JobSeekerID = mJobseekerId };
            var response = new ReadQuestionsResponse
            {
                JobSeekerID = mJobseekerId,
                GroupJsciQuestions = new[]
                                        {
                                            //to avoid KeyNotFoundException
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.CON1,
                                                    ResponseCode = ResponseCodes.ATAA,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.ATSA,
                                                    ResponseCode = ResponseCodes.ATAA,
                                                    ResponseOption = new List<QuestionAnswer>
                                                                            {
                                                                                new QuestionAnswer { ResponseCode = ResponseCodes.ATAD, ResponseDescription = "aa"}, 
                                                                                new QuestionAnswer { ResponseCode = ResponseCodes.ATAE, ResponseDescription = "bb"}
                                                                            }
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.MLAO,
                                                    ResponseCode = ResponseCodes.ATAA,
                                                },
                                                
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.MLAN,
                                                    ResponseCode = ResponseCodes.MLCV,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.ATSB,
                                                    ResponseCode = ResponseCodes.ATAB,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTH,
                                                    ResponseCode = ResponseCodes.LWTE,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTI,
                                                    ResponseCode = ResponseCodes.LWTB,
                                                },

                                             //JCA 
                                             new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "T",
                                                    QuestionCode = QuestionCodes.DEUB,
                                                    QuestionSetType = "O",
                                                    QuestionText = "ESAt/JCA Report Reference",
                                                    ResponseCode = ResponseCodes.DETL,
                                                    ResponseOption = new List<QuestionAnswer>
                                                                            {
                                                                                new QuestionAnswer { ResponseDescription = "000000002444919", ResponseCode = "000000002444919" },
                                                                            }
                                                },

                                            //checkbox question codes - can't be IsQuestionResponsesArchived
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTK,
                                                    ResponseCode = ResponseCodes.LWTF,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTL,
                                                    ResponseCode = ResponseCodes.LWTC,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTM,
                                                    ResponseCode = ResponseCodes.LWTD,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTH,
                                                    ResponseCode = ResponseCodes.LWTE,
                                                },
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.LWTJ,
                                                    ResponseCode = ResponseCodes.LWTG,
                                                },

                                               // ,//isEarlySchoolLeaver
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.HSCH,
                                                    ResponseCode = ResponseCodes.HSCD,
                                                }
                                                ,
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.CON2, //1.73
                                                    ReasonCode = "a"
                                                }
                                                ,
                                            new ReadQuestionDetailsResponse
                                                {
                                                    ResponseType = "C",
                                                    QuestionCode = QuestionCodes.CON3, //1.42
                                                    ResponseCode = "C1GE",
                                                }
                                        },

                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                JSCIStatusCode = "P",
                ExecutionResult = mSuccessResult
            };

            var expectedModel = MappingEngine.Map<JsciModel>(response);
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);
            mockJsciWcf.Setup(w => w.GetQuestionText(It.IsAny<GetQuestionTextRequest>())).Returns(
                new GetQuestionTextResponse { TextBlock = "JCA REPORT REF:", ExecutionResult = mSuccessResult });

            var actualModel = SystemUnderTest().ReadQuestions(inModel);

            Assert.IsNotNull(actualModel);
            Assert.AreEqual(expectedModel.AssessmentStartDateTime, actualModel.AssessmentStartDateTime);
            mockJsciWcf.Verify(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>()), Times.Once());
            //Assert.IsFalse(actualModel.IsReasonForChangeRequired());
            Assert.IsTrue(actualModel.IsReasonForChangeRequired());
            Assert.AreEqual("JCA REPORT REF:", actualModel.QuestionResponses[QuestionCodes.DEUB].ResponseCode);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ReadQuestions_ThrowsFaultException()
        {
            mockJsciWcf.Setup(w => w.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Throws<FaultException>();
            SystemUnderTest().ReadQuestions(new JsciModel { JobSeekerID = mJobseekerId });
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="ValidationFault" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ReadQuestions_ThrowsValidationFault()
        {
            mockJsciWcf.Setup(w => w.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Throws(new FaultException<ValidationFault>(mFault));
            SystemUnderTest().ReadQuestions(new JsciModel { JobSeekerID = mJobseekerId });
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetQuestionText_ThrowsFaultException()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            BasicReadResponse.GroupJsciQuestions = new[]
                                                       {
                                                           new ReadQuestionDetailsResponse
                                                               {
                                                                   QuestionCode = "MACT",
                                                                   ResponseType = "T",
                                                                   ResponseCode = ResponseCodes.DETL
                                                               }
                                                       };
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(BasicReadResponse);
            mockJsciWcf.Setup(w => w.GetQuestionText(It.IsAny<GetQuestionTextRequest>())).Throws<FaultException>();
            SystemUnderTest().ReadQuestions(new JsciModel { JobSeekerID = mJobseekerId });
            BasicReadResponse.GroupJsciQuestions = new ReadQuestionDetailsResponse[0];
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="ValidationFault" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetQuestionText_ThrowsValidationFault()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var response = new ReadQuestionsResponse
            {
                GroupJsciQuestions = new[] { new ReadQuestionDetailsResponse { ResponseType = "T", ResponseCode = ResponseCodes.DETL}  },
                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                ArchiveInd = "",
                ExecutionResult = mSuccessResult
            };
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);

            mockJsciWcf.Setup(w => w.GetQuestionText(It.IsAny<GetQuestionTextRequest>())).Throws(new FaultException<ValidationFault>(mFault));
            SystemUnderTest().ReadQuestions(new JsciModel { JobSeekerID = mJobseekerId });
        }
        #endregion

        #region Add

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_ThrowsFaultException()
        {
            mockJsciWcf.Setup(w => w.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse());
            mockJsciWcf.Setup(w => w.Add(It.IsAny<AddRequest>())).Throws<FaultException>();
            SystemUnderTest().Add(new JsciModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker, QuestionResponses = new Dictionary<string, QuestionResponseModel>() });
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> thrown on <see cref="ValidationFault" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_ThrowsValidationFault()
        {
            mockJsciWcf.Setup(w => w.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse());
            mockJsciWcf.Setup(w => w.Add(It.IsAny<AddRequest>())).Throws(new FaultException<ValidationFault>(mFault));
            SystemUnderTest().Add(new JsciModel { JobSeekerID = mJobseekerId, Jobseeker = mJobseeker, QuestionResponses = new Dictionary<string, QuestionResponseModel>() });
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Add_Valid()
        {
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(BasicReadResponse);

            var response = new AddResponse
                               {
                                   JobseekerID = mJobseekerId,
                                   ExecutionResult = mSuccessResult
                               };
            mockJsciWcf.Setup(m => m.Add(It.IsAny<AddRequest>())).Returns(response);

            //mockJsciWcf.Setup(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse { ExecutionResult = mSuccessResult });

            //mockJsciWcf.Setup(w => w.GetQuestionText(It.IsAny<GetQuestionTextRequest>())).Returns(
            //    new GetQuestionTextResponse { TextBlock = "", ExecutionResult = mSuccessResult });

            SystemUnderTest().Add(new JsciModel
                                      {
                                          JobSeekerID = mJobseekerId,
                                          Jobseeker = mJobseeker,
                                          ReasonCode = "COC",
                                          QuestionResponses = new Dictionary<string, QuestionResponseModel>
                                                                  {
                                                                       { QuestionCodes.MACT, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.MACT,
                                                                                                     ResponseCode = ResponseCodes.MACA,
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.DIBR, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.DIBR,
                                                                                                     ResponseCode = now.ToShortDateString(),
                                                                                                     ResponseFormat = ResponseFormatType.Date,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.DEUA, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.DEUA,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Text,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.CON2, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.CON2,
                                                                                                     ResponseCode = "",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC", 
                                                                                                     OriginalResponse = ""
                                                                                                 } },
                                                                       { QuestionCodes.CON3, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.CON3,
                                                                                                     ResponseCode = "C1XX",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC",
                                                                                                     OriginalResponse = "C1YY"
                                                                                                 } },
                                                                                                 
                                                                       { QuestionCodes.SC_MLAN, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.SC_MLAN,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.MLAN, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.MLAN,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.SC_LWTH, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.SC_LWTH,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.LWTH, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.LWTH,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.SC_ATSA, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.SC_ATSA,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                       { QuestionCodes.ATSA, new QuestionResponseModel
                                                                                                 {
                                                                                                     QuestionCode = QuestionCodes.ATSA,
                                                                                                     ResponseCode = "blah",
                                                                                                     ResponseFormat = ResponseFormatType.Code,
                                                                                                     ReasonForChangeCode = "COC"
                                                                                                 } },
                                                                      
                                                                  },
                                          PersonalFactors = new JsciJcaData("X", null, DateTime.MinValue),
                                          Disability = new JsciJcaData("X", null, DateTime.MinValue),
                                          SpecialNeeds = new JsciJcaData("X", null, DateTime.MinValue),
                                          IsCurrentJsci = true
                                      });
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_QuestionInError()
        {
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(BasicReadResponse);

            var response = new AddResponse
                                {
                                    JobseekerID = mJobseekerId,
                                    QuestionInError = "XXXX",
                                    ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Failed, ExecuteMessages = new List<ExecutionMessage> { new ExecutionMessage { Text = "question is in error"} } }
                                };
            mockJsciWcf.Setup(m => m.Add(It.IsAny<AddRequest>())).Returns(response);
            mockJsciWcf.Setup(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse { ExecutionResult = mSuccessResult });
            SystemUnderTest().Add(new JsciModel
                                      {
                                          JobSeekerID = mJobseekerId,
                                          Jobseeker = mJobseeker,
                                          IsCurrentJsci = true,
                                          QuestionResponses = new Dictionary<string, QuestionResponseModel>() 
                                      });
        }

        #endregion

        #region CopyWithdraw
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyWithdraw_NoReason()
        {
            SystemUnderTest().CopyWithdraw(new JsciModel(), "", "X");
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CopyWithdraw_0JobseekerId()
        {
            SystemUnderTest().CopyWithdraw(new JsciModel(), "X", null);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CopyWithdraw_NotPending()
        {
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(new ReadQuestionsResponse());
            SystemUnderTest().CopyWithdraw(new JsciModel { JobSeekerID = mJobseekerId }, "X", null);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CopyWithdraw_JcaReferralOpen()
        {
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(new ReadQuestionsResponse { JsaReferralOpenFlag = "Y", JSCIStatusCode = "P" });
            //IsJcaReferralOpen should be true
            SystemUnderTest().CopyWithdraw(new JsciModel { JobSeekerID = mJobseekerId }, "X", null);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CopyWithdraw_JobseekerNotAttachedToOrg()
        {
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(new JobseekerModel { JobSeekerId = mJobseekerId, DateOfBirth = new DateTime(1990, 1, 1), CurrentRegistrationDetails = new RegistrationPeriod { Active = true } });
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(new ReadQuestionsResponse());
            SystemUnderTest().CopyWithdraw(new JsciModel { JobSeekerID = mJobseekerId }, "X", null);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CopyWithdraw_JobseekerNotActive()
        {
            mockRegistrationService.Setup(r => r.ReadJobseeker(It.IsAny<long>())).Returns(new JobseekerModel { JobSeekerId = mJobseekerId, DateOfBirth = new DateTime(1990, 1, 1), CurrentRegistrationDetails = new RegistrationPeriod(), IsLinkedToOrg = true });
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(new ReadQuestionsResponse());
            SystemUnderTest().CopyWithdraw(new JsciModel { JobSeekerID = mJobseekerId }, "X", null);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CopyWithdraw_Valid()
        {
            var response = new ReadQuestionsResponse
            {
                JSCIAssessmentStartDate = now.Date,
                JSCIAssessmentStartTime = DateTime.MinValue + now.TimeOfDay,
                //ArchiveInd = "",
                JSCIStatusCode = "P",
                ExecutionResult = mSuccessResult,
                GroupJsciQuestions = GetBasicQuestionResponses()
            };
            mockJsciWcf.Setup(m => m.ReadQuestions(It.IsAny<ReadQuestionsRequest>())).Returns(response);
            mockJsciWcf.Setup(s => s.CopyWithdraw(It.IsAny<CopyWithdrawRequest>())).Returns(new CopyWithdrawResponse());
            SystemUnderTest().CopyWithdraw(new JsciModel { JobSeekerID = mJobseekerId }, "OTHR", "X");
            mockJsciWcf.Verify(w => w.CopyWithdraw(It.IsAny<CopyWithdrawRequest>()), Times.Once());
            //mockCacheService.Verify(c => c.Remove(It.IsAny<CacheType>(), It.IsAny<string>()), Times.Once());
        }

        #endregion

        #region EditJca
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void EditJca_Valid()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var inModel = new JsciModel { JobSeekerID = mJobseekerId, IsCurrentJsci = true };
            var response = new UpdateJSAResponse
            {
                ExecutionResult = mSuccessResult
            };
            mockJsciWcf.Setup(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>())).Returns(response);
            SystemUnderTest().EditJca(inModel, true, null);
            mockJsciWcf.Verify(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>()), Times.Once());
            mockJsciWcf.Verify(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>()), Times.Never());
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void EditJca_Pending()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var inModel = new JsciModel { JobSeekerID = mJobseekerId, IsCurrentJsci = true, Status = JsciStatus.Pending};
            var response = new UpdateJSAResponse
            {
                ExecutionResult = mSuccessResult
            };
            mockJsciWcf.Setup(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>())).Returns(response);
            mockJsciWcf.Setup(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse());
            SystemUnderTest().EditJca(inModel, true, null);
            mockJsciWcf.Verify(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>()), Times.Once());
            mockJsciWcf.Verify(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>()), Times.Once());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void EditJca_Failed()
        {
            mockUserService.Setup(u => u.IsInRole(It.IsAny<string[]>())).Returns(true);
            var inModel = new JsciModel { JobSeekerID = mJobseekerId, IsCurrentJsci = true };
            var response = new UpdateJSAResponse
            {
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Failed, ExecuteMessages = new List<ExecutionMessage> { new ExecutionMessage { Text = "record could not be found"} } }
            };
            mockJsciWcf.Setup(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>())).Returns(response);
            mockJsciWcf.Setup(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>())).Returns(new CancelUpdateResponse());
            SystemUnderTest().EditJca(inModel, true, null);
            mockJsciWcf.Verify(m => m.UpdateJSA(It.IsAny<UpdateJSARequest>()), Times.Once());
            mockJsciWcf.Verify(m => m.CancelUpdate(It.IsAny<CancelUpdateRequest>()), Times.Once());
        }

        #endregion

        #region model tests
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void JsciModelsAreSerializable()
        {
            new QuestionResponseModel().Serialize();
            new QuestionResponseModel().Serialize();
            new JsciJcaData().Serialize();
            new JsciModel().Serialize();
            new JsciListModel().Serialize();
        }

        /// <summary>
        /// DR01033074 when used with m.IsEarlySchoolLeaver. 
        /// Disable FRED (yes/no to further qualifications question) on load: controller.Add(long) should set vm.IsEditableHaveCompletedQualifications to false.
        /// TODO: test for DR01029075 that selected qualifications is disabled when yr12 selected for highest qualification.
        /// </summary>
        [TestMethod]
        public void JsciModel_DidntDoYr12ButDidEquivalent()
        {
            var model = new JsciModel
            {
                QuestionResponses = new Dictionary<string, QuestionResponseModel>
                    {
                        { QuestionCodes.HSCH, new QuestionResponseModel
                                                    {
                                                        QuestionCode = QuestionCodes.HSCH,
                                                        ResponseFormat = ResponseFormatType.Code
                                                    } },
                        { QuestionCodes.FRED, new QuestionResponseModel
                                                    {
                                                        QuestionCode = QuestionCodes.FRED,
                                                        ResponseFormat = ResponseFormatType.Code
                                                    } }
                    }
            };
            foreach (var questionCode in QuestionCodeGroups.HigherQualificationQuestionCodes)
                model.QuestionResponses.Add(questionCode, new QuestionResponseModel { QuestionCode = questionCode, ResponseFormat = ResponseFormatType.Code });

            //control test
            Assert.IsFalse(model.DidntDoYr12ButDidEquivalent());

            //did yr12
            model.QuestionResponses[QuestionCodes.HSCH].ResponseCode = ResponseCodes.HSCD;
            Assert.IsFalse(model.DidntDoYr12ButDidEquivalent());

            //has further qualifications
            model.QuestionResponses[QuestionCodes.FRED].ResponseCode = ResponseCodes.FREA;
            Assert.IsFalse(model.DidntDoYr12ButDidEquivalent());

            //didn't do yr12
            model.QuestionResponses[QuestionCodes.HSCH].ResponseCode = null;
            Assert.IsFalse(model.DidntDoYr12ButDidEquivalent());

            //but did equivalent qualifiactaion - test each one
            foreach (var questionCode in QuestionCodeGroups.HigherQualificationQuestionCodes)
            {
                foreach (var responseCode in ResponseCodeGroups.HigherQualificationYr12Equivalent)
                {
                    //add this qualification that is equivalent to yr12
                    model.QuestionResponses[questionCode].ResponseCode = responseCode;
                    Assert.IsTrue(model.DidntDoYr12ButDidEquivalent(), "DR01033074");

                    //remove and check it's false again
                    model.QuestionResponses[questionCode].ResponseCode = null;
                    Assert.IsFalse(model.DidntDoYr12ButDidEquivalent());
                }
            }
        }

        //private void AddQuestionResponse(IDictionary<string, QuestionResponseModel> questionResponses, string questionCode, string responseOption

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void JsciModel_IsAustralianBorn()
        {
            var model = new JsciModel { Jobseeker = new JobseekerModel() };
            Assert.IsFalse(model.IsAustralianBorn());

            model.Jobseeker.CountryOfBirth = "AU";
            Assert.IsTrue(model.IsAustralianBorn());

            model.Jobseeker.CountryOfBirth = "XH";
            Assert.IsTrue(model.IsAustralianBorn());

            model.Jobseeker.CountryOfBirth = "XI";
            Assert.IsTrue(model.IsAustralianBorn());
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void JsciModel_IsJcaExists()
        {
            var model = new JsciModel();
            Assert.IsFalse(model.IsJcaExists(now));

            model.AssessmentId = 1;
            Assert.IsFalse(model.IsJcaExists(now));

            model.JcaExpiryDate = now;
            Assert.IsFalse(model.IsJcaExists(now));

            model.JcaExpiryDate = now.AddMilliseconds(-1);
            Assert.IsFalse(model.IsJcaExists(now));

            model.JcaExpiryDate = now.AddMilliseconds(1);
            Assert.IsFalse(model.IsJcaExists(now));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void JsciModel_CriminalConvictionAgeType()
        {
            var model = new JsciModel { Jobseeker = new JobseekerModel() };
            Assert.AreEqual(CriminalConvictionAgeType.Unknown, model.CriminalConvictionAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-CriminalAgeLowerBoundary + 1);
            Assert.AreEqual(CriminalConvictionAgeType.Young, model.CriminalConvictionAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-CriminalAgeLowerBoundary);
            Assert.AreEqual(CriminalConvictionAgeType.Middle, model.CriminalConvictionAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-CriminalAgeUpperBoundary + 1);
            Assert.AreEqual(CriminalConvictionAgeType.Middle, model.CriminalConvictionAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-CriminalAgeUpperBoundary);
            Assert.AreEqual(CriminalConvictionAgeType.Mature, model.CriminalConvictionAgeType(now));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void JsciModel_PersonalCharacteristicsAgeType()
        {
            var model = new JsciModel { Jobseeker = new JobseekerModel() };
            Assert.AreEqual(PersonalCharacteristicsAgeType.Unknown, model.PersonalCharacteristicsAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-PersonalCharacteristicsAgeBoundary + 1);
            Assert.AreEqual(PersonalCharacteristicsAgeType.Young, model.PersonalCharacteristicsAgeType(now));

            model.Jobseeker.DateOfBirth = now.AddYears(-PersonalCharacteristicsAgeBoundary);
            Assert.AreEqual(PersonalCharacteristicsAgeType.Mature, model.PersonalCharacteristicsAgeType(now));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void QuestionResponseModel_ResponseDescription()
        {
            var model = new QuestionResponseModel { ResponseOptions = new Dictionary<string, string>() };
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseFormat = ResponseFormatType.Code;
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseCode = "a";
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseOptions.Add("a", "");
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseCode = "b";
            model.ResponseOptions.Add("b", "c");
            Assert.AreEqual("c", model.ResponseDescription);

            model.ResponseCode = "not a date";
            model.ResponseFormat = ResponseFormatType.Date;
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseCode = DateTime.Today.ToShortDateString();
            model.ResponseFormat = ResponseFormatType.Date;
            Assert.AreEqual(model.ResponseCode, model.ResponseDescription);
            DateTime test;
            Assert.IsTrue(DateTime.TryParse(model.ResponseDescription, out test));

            model.ResponseCode = null;
            model.ResponseFormat = ResponseFormatType.Text;
            Assert.AreEqual("", model.ResponseDescription);

            model.ResponseCode = "some text";
            model.ResponseFormat = ResponseFormatType.Text;
            Assert.AreEqual("some text", model.ResponseDescription);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void QuestionResponseModel_OriginalResponseDescription()
        {
            var model = new QuestionResponseModel();
            Assert.AreEqual("", model.OriginalResponseDescription);
        }

        #endregion

        #region private methods

        private static ReadQuestionDetailsResponse[] GetBasicQuestionResponses()
        {
            return
                new[]
                    {
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "T",
                                QuestionCode = QuestionCodes.DEUB
                            },

                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "D",
                                QuestionCode = QuestionCodes.MACT,
                                ResponseOption =
                                    new List<QuestionAnswer>
                                        {
                                            new QuestionAnswer
                                                {ResponseDescription = "a ", ResponseCode = ResponseCodes.ATAA}
                                        }
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.MACT
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.CON1
                            },

                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.ATSA,
                                ResponseCode = ResponseCodes.ATAD,
                                //ReasonCode = "a",
                                ResponseOption = new List<QuestionAnswer>
                                                     {
                                                         new QuestionAnswer
                                                             {
                                                                 ResponseCode = ResponseCodes.ATAD,
                                                                 ResponseDescription = "aa"
                                                             },
                                                         new QuestionAnswer
                                                             {
                                                                 ResponseCode = ResponseCodes.ATAE,
                                                                 ResponseDescription = "bb"
                                                             }
                                                     }
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.ATSB,
                                ResponseCode = ResponseCodes.LWTA,
                                ResponseOption = new List<QuestionAnswer>
                                                     {
                                                         new QuestionAnswer
                                                             {
                                                                 ResponseCode = ResponseCodes.LWTA,
                                                                 ResponseDescription = "aa"
                                                             }
                                                     }
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTH
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTI
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTJ
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTK
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTL
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTM,
                                ResponseCode = ResponseCodes.ATAE,
                                ResponseOption = new List<QuestionAnswer>
                                                     {
                                                         new QuestionAnswer
                                                             {
                                                                 ResponseCode = ResponseCodes.ATAE,
                                                                 ResponseDescription = "aa"
                                                             }
                                                     }
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.MLAN
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.MLAO
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.LWTI
                            },
                        new ReadQuestionDetailsResponse
                            {
                                ResponseType = "C",
                                QuestionCode = QuestionCodes.HSCH
                            }
                    };
        }
        #endregion
    }
    /// <summary>
    /// Test that obj can be serialized for caching.
    /// </summary>
    internal static class SerialTester
    {
        public static string Serialize<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                using (var compressionStream = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    var formatter = new BinaryFormatter();
                    // Serialize object to stream
                    formatter.Serialize(compressionStream, obj);

                    compressionStream.Flush();
                }

                stream.Position = 0;

                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
