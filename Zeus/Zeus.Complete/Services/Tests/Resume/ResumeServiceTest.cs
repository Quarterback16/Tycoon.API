using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.ResumeRtf.Contracts.DataContracts;
using Employment.Esc.ResumeRtf.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Service.Implementation.Resume;
using Employment.Web.Mvc.Service.Interfaces.Resume;

namespace Employment.Web.Mvc.Service.Tests.Resume
{
    /// <summary>
    /// Unit tests for <see cref="EmployerService" />.
    /// </summary>
    [TestClass]
    public class ResumeServiceTest
    {
        private ResumeService SystemUnderTest()
        {
            return new ResumeService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ResumeMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IResumeRtf> mockResumeService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockResumeService = new Mock<IResumeRtf>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            mockClient.Setup(m => m.Create<IResumeRtf>("ResumeRtf.svc")).Returns(mockResumeService.Object);
        }

        private ExecutionResult SuccessResult()
        {
            return new ExecutionResult() { Status = ExecuteStatus.Success };
        }

        private FaultException FaultException()
        {
            return new FaultException();
        }

        private FaultException<ValidationFault> ValidationFaultException()
        {
            return new FaultException<ValidationFault>(new ValidationFault(new ValidationDetail[] { new ValidationDetail() { Key = "A", Message = "B", Tag = "C" } }));
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            var service = new ResumeService(null, null, null);
        }

        #region Add
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.Add(new AddResumeModel());
        }

        [TestMethod]
        public void Add_Successful()
        {
            mockResumeService.Setup(m => m.Add(It.Is<AddRequest>(i => i.JobSeekerID == 1234567890 && i.ResumeName == "TEST")))
                .Returns(new AddResponse() { ExecutionResult = SuccessResult(), ResumeID = 1234567890, Timestamp = 1234567890, Updated = DateTime.Now });
            var service = SystemUnderTest();
            var model = new AddResumeModel() { JobSeekerID = 1234567890, CompressedRTF = new byte[1] { 1 }, ResumeName = "TEST" };
            service.Add(model);

            mockResumeService.Verify(m => m.Add(It.Is<AddRequest>(i => i.JobSeekerID == model.JobSeekerID && i.ResumeName == model.ResumeName)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Add(It.IsAny<AddRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new AddResumeModel() { JobSeekerID = 1234567890, CompressedRTF = new byte[1] { 1 }, ResumeName = "TEST" };
            service.Add(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Add_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Add(It.IsAny<AddRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new AddResumeModel() { JobSeekerID = 1234567890, CompressedRTF = new byte[1] { 1 }, ResumeName = "TEST" };
            service.Add(model);
        } 
        #endregion

        #region Delete
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Delete_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.Delete(new DeleteResumeModel());
        }

        [TestMethod]
        public void Delete_Successful()
        {
            mockResumeService.Setup(m => m.Delete(It.Is<DeleteRequest>(i => i.ResumeID == 1234567890 && i.Timestamp == 1234567890)))
                .Returns(SuccessResult());
            var service = SystemUnderTest();
            var model = new DeleteResumeModel() { JobSeekerID = 1234567890, ResumeID = 1234657890, RowVersion = 1234567890 };
            service.Delete(model);

            mockResumeService.Verify(m => m.Delete(It.Is<DeleteRequest>(i => i.ResumeID == 1234657890 && i.Timestamp == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Delete_InvalidResponse_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Delete(It.IsAny<DeleteRequest>())).Returns(new ExecutionResult() { Status = ExecuteStatus.Failed,
                ExecuteMessages = new List<ExecutionMessage>(new[] { new ExecutionMessage(ExecutionMessageType.Error, "Failed"), 
                    new ExecutionMessage(ExecutionMessageType.Error, "") { Help = "" } })
            });

            var service = SystemUnderTest();
            var model = new DeleteResumeModel() { JobSeekerID = 1234567890, ResumeID = 1234657890, RowVersion = 1234567890 };
            service.Delete(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Delete_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Delete(It.IsAny<DeleteRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new DeleteResumeModel() { JobSeekerID = 1234567890, ResumeID = 1234657890, RowVersion = 1234567890 };
            service.Delete(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Delete_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Delete(It.IsAny<DeleteRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new DeleteResumeModel() { JobSeekerID = 1234567890, ResumeID = 1234657890, RowVersion = 1234567890 };
            service.Delete(model);
        }
        #endregion

        #region ExtractKeywords
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ExtractKeywords_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.ExtractKeywords(new ExtractKeywordsModel());
        }

        [TestMethod]
        public void ExtractKeywords_Successful()
        {
            var response = new ParseKeywordsResponse() { ExecutionResult = SuccessResult(), MatchedKeyWords = new string[] { "build", "clean" } };
            mockResumeService.Setup(m => m.ParseKeywords(It.Is<ParseKeywordsRequest>(i => i.ResumeID == 1234567890 && i.JobSeekerID == 1234567890)))
                .Returns(response);
            var service = SystemUnderTest();
            var model = new ExtractKeywordsModel() { JobSeekerID = 1234567890, ResumeID = 1234567890 };
            var result = service.ExtractKeywords(model);

            Assert.IsTrue(result.SequenceEqual(response.MatchedKeyWords));
            mockResumeService.Verify(m => m.ParseKeywords(It.Is<ParseKeywordsRequest>(i => i.ResumeID == 1234567890 && i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ExtractKeywords_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.ParseKeywords(It.IsAny<ParseKeywordsRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new ExtractKeywordsModel() { JobSeekerID = 1234567890, ResumeID = 1234657890 };
            service.ExtractKeywords(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ExtractKeywords_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.ParseKeywords(It.IsAny<ParseKeywordsRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new ExtractKeywordsModel() { JobSeekerID = 1234567890, ResumeID = 1234657890 };
            service.ExtractKeywords(model);
        }
        #endregion

        #region Find
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Find_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.Find(0);
        }

        [TestMethod]
        public void Find_Successful()
        {
            var resumes = new List<Found>();
            resumes.Add(new Found()
            {
                IsDefault = true,
                Modified = DateTime.Now,
                ResumeID = 1234567890,
                ResumeName = "Test Resume",
                Timestamp = 1234567890
            });
            var response = new FindResponse() { ExecutionResult = SuccessResult(), Resumes = resumes.ToArray() };
            mockResumeService.Setup(m => m.Find(It.Is<FindRequest>(r => r.JobSeekerID == 1234567890)))
                .Returns(response);
            var service = SystemUnderTest();

            var result = service.Find(1234567890);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.First().ResumeId, resumes.First().ResumeID);
            Assert.AreEqual(result.First().ResumeTitle, resumes.First().ResumeName);
            Assert.AreEqual(result.First().Timestamp, resumes.First().Timestamp);
            mockResumeService.Verify(m => m.Find(It.Is<FindRequest>(r => r.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Find_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Find(It.IsAny<FindRequest>())).Throws(FaultException());

            var service = SystemUnderTest();

            service.Find(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Find_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.Find(It.IsAny<FindRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();

            service.Find(1234567890);
        }
        #endregion

        #region GenerateAndAddResume

        [TestMethod]
        public void GenerateAndAddResume_Successful()
        {
            var response = new AddResponse() { ExecutionResult = SuccessResult(), ResumeID = 1234567890, Timestamp = 1234567890 };
            mockResumeService.Setup(m => m.GenerateAndAdd(It.IsAny<GenerateAndAddRequest>()))
                .Returns(response);
            var service = SystemUnderTest();
            var model = new ResumeBuilderModel() { JobSeekerID = 1234567890 };
            var eduHistory = new List<EducationHistoryItemModel>();
            eduHistory.Add(new EducationHistoryItemModel()
            {
                DateObtained = "2001",
                ID = 0,
                Institution = "University",
                Qualification = "Degree",
                QualificationDescription = "test"
            });

            var empHistory = new List<EmploymentHistoryItemModel>();
            empHistory.Add(new EmploymentHistoryItemModel()
            {
                ID = 0,
                Employer = "test",
                DateStarted = DateTime.Now.AddYears(-5),
                DateFinished = DateTime.Now.AddYears(-2),
                DutiesDescription = "test duty",
                JobTitle = "tester"
            });

            var referees = new List<RefereesItemModel>();
            referees.Add(new RefereesItemModel()
            {
                ID = 0,
                ContactDetails = "123546456456",
                Organisation = "Org",
                RefereeName = "John Smith",
                Relationship = "Supervisor"
            });

            var licences = new List<LicenceItemModel>();
            licences.Add(new LicenceItemModel()
            {
                ID = 0,
                LicenceType = "C",
                State = "NSW"
            });

            model.EducationHistory = eduHistory;
            model.WorkHistory = empHistory;
            model.Referees = referees;
            model.Skills = new string[] { "skill1", "skill2" };
            model.Licences = licences;
            model.PersonalDetails = new ResumePersonalDetailsModel()
            {
                DateOfBirth = DateTime.Now.AddYears(-20),
                FullName = "Joe Bloggs",
                HomePhone = "1234567890",
                MobilePhone = "1234567891",
                ResumeName = "Test Resume",
                Email = "test@test.com"
            };
            
            service.GenerateAndAddResume(model);

            mockResumeService.Verify(m => m.GenerateAndAdd(It.IsAny<GenerateAndAddRequest>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GenerateAndAddResume_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GenerateAndAdd(It.IsAny<GenerateAndAddRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new ResumeBuilderModel() { JobSeekerID = 1234567890 };
            var eduHistory = new List<EducationHistoryItemModel>();
            eduHistory.Add(new EducationHistoryItemModel()
            {
                DateObtained = "2001",
                ID = 0,
                Institution = "University",
                Qualification = "Degree",
                QualificationDescription = "test"
            });

            var empHistory = new List<EmploymentHistoryItemModel>();
            empHistory.Add(new EmploymentHistoryItemModel()
            {
                ID = 0,
                Employer = "test",
                DateStarted = DateTime.Now.AddYears(-5),
                DateFinished = DateTime.Now.AddYears(-2),
                DutiesDescription = "test duty",
                JobTitle = "tester"
            });

            var referees = new List<RefereesItemModel>();
            referees.Add(new RefereesItemModel()
            {
                ID = 0,
                ContactDetails = "123546456456",
                Organisation = "Org",
                RefereeName = "John Smith",
                Relationship = "Supervisor"
            });

            var licences = new List<LicenceItemModel>();
            licences.Add(new LicenceItemModel()
            {
                ID = 0,
                LicenceType = "C",
                State = "NSW"
            });

            model.EducationHistory = eduHistory;
            model.WorkHistory = empHistory;
            model.Referees = referees;
            model.Skills = new string[] { "skill1", "skill2" };
            model.Licences = licences;
            model.PersonalDetails = new ResumePersonalDetailsModel()
            {
                DateOfBirth = DateTime.Now.AddYears(-20),
                FullName = "Joe Bloggs",
                HomePhone = "1234567890",
                MobilePhone = "1234567891",
                ResumeName = "Test Resume",
                Email = "test@test.com"
            };
            
            service.GenerateAndAddResume(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GenerateAndAddResume_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GenerateAndAdd(It.IsAny<GenerateAndAddRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new ResumeBuilderModel() { JobSeekerID = 1234567890 };
            var eduHistory = new List<EducationHistoryItemModel>();
            eduHistory.Add(new EducationHistoryItemModel()
            {
                DateObtained = "2001",
                ID = 0,
                Institution = "University",
                Qualification = "Degree",
                QualificationDescription = "test"
            });

            var empHistory = new List<EmploymentHistoryItemModel>();
            empHistory.Add(new EmploymentHistoryItemModel()
            {
                ID = 0,
                Employer = "test",
                DateStarted = DateTime.Now.AddYears(-5),
                DateFinished = DateTime.Now.AddYears(-2),
                DutiesDescription = "test duty",
                JobTitle = "tester"
            });

            var referees = new List<RefereesItemModel>();
            referees.Add(new RefereesItemModel()
            {
                ID = 0,
                ContactDetails = "123546456456",
                Organisation = "Org",
                RefereeName = "John Smith",
                Relationship = "Supervisor"
            });

            var licences = new List<LicenceItemModel>();
            licences.Add(new LicenceItemModel()
            {
                ID = 0,
                LicenceType = "C",
                State = "NSW"
            });

            model.EducationHistory = eduHistory;
            model.WorkHistory = empHistory;
            model.Referees = referees;
            model.Skills = new string[] { "skill1", "skill2" };
            model.Licences = licences;
            model.PersonalDetails = new ResumePersonalDetailsModel()
            {
                DateOfBirth = DateTime.Now.AddYears(-20),
                FullName = "Joe Bloggs",
                HomePhone = "1234567890",
                MobilePhone = "1234567891",
                ResumeName = "Test Resume",
                Email = "test@test.com"
            };
            service.GenerateAndAddResume(model);
        }
        #endregion

        #region GetUncompressedResume

        [TestMethod]
        public void GetUncompressedResume_Successful()
        {
            var response = new GetDetailsResponse() { ExecutionResult = SuccessResult(), 
                Resumes = new[] { new Details() 
                    { 
                        CompressedRTF = new byte[1] { 1 },
                        Created = DateTime.Now,
                        CreatedID = "1234567890",
                        IsDefault = true,
                        ResumeID = 1234567890,
                        ResumeName = "Test Resume",
                        RowVersion = 1234567890,
                        RTF = "\\rtf",
                        Timestamp = 1234567890                        
                    } 
                } };
            mockResumeService.Setup(m => m.GetDetailsUncompressed(It.Is<GetDetailsRequest>(i => i.ResumeID == 1234567890)))
                .Returns(response);
            var service = SystemUnderTest();
            var result = service.GetUncompressedResume(1234567890);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RTF, "\\rtf");
            Assert.AreEqual(result.Timestamp, 1234567890);
            mockResumeService.Verify(m => m.GetDetailsUncompressed(It.Is<GetDetailsRequest>(i => i.ResumeID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetUncompressedResume_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GetDetailsUncompressed(It.IsAny<GetDetailsRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetUncompressedResume(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetUncompressedResume_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GetDetailsUncompressed(It.IsAny<GetDetailsRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetUncompressedResume(1234567890);
        }
        #endregion
    }
}
