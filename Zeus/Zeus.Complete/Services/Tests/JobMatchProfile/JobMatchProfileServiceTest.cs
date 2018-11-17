using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Service.Implementation.JobMatchProfile;
using Employment.Web.Mvc.Service.Interfaces.JobMatchProfile;
using Employment.Esc.JobMatchProfile.Contracts.ServiceContracts;
using Employment.Esc.JobMatchProfile.Contracts.DataContracts;

namespace Employment.Web.Mvc.Service.Tests.JobMatchProfile
{
    /// <summary>
    /// Unit tests for <see cref="EmployerService" />.
    /// </summary>
    [TestClass]
    public class JobMatchProfileServiceTest
    {
        private JobMatchProfileService SystemUnderTest()
        {
            return new JobMatchProfileService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new JobMatchProfileMapper();
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
        private Mock<IJobMatchProfile> mockJobMatchProfileService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockJobMatchProfileService = new Mock<IJobMatchProfile>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            mockClient.Setup(m => m.Create<IJobMatchProfile>("JobMatchProfile.svc")).Returns(mockJobMatchProfileService.Object);
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
            var service = new JobMatchProfileService(null, null, null);
        }

        #region AddProfileLicence
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLicence_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.AddProfileLicence(new LicenceModel());
        }

        [TestMethod]
        public void AddProfileLicence_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.AddLicence(It.Is<AddLicenceRequest>(i => i.JobSeekerID == 1234567890 && i.Licences[0].LicenceCode == "C" && i.Licences[0].StateCode == "NSW")))
                .Returns(new AddLicenceResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new LicenceModel() { LicenceType = "C", LicenceState = "NSW", JobSeekerID = 1234567890 };
            service.AddProfileLicence(model);

            mockJobMatchProfileService.Verify(m => m.AddLicence(It.Is<AddLicenceRequest>(i => i.JobSeekerID == 1234567890 && i.Licences[0].LicenceCode == "C" && i.Licences[0].StateCode == "NSW")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLicence_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.AddLicence(It.IsAny<AddLicenceRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new LicenceModel() { LicenceType = "C", LicenceState = "NSW", JobSeekerID = 1234567890 };
            service.AddProfileLicence(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLicence_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.AddLicence(It.IsAny<AddLicenceRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new LicenceModel() { LicenceType = "C", LicenceState = "NSW", JobSeekerID = 1234567890 };
            service.AddProfileLicence(model);
        } 
        #endregion 

        #region AddProfileLocations
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLocations_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.AddProfileLocations(new ProfileLocationsModel());
        }

        [TestMethod]
        public void AddProfileLocations_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.AddLocation(It.Is<AddLocationRequest>(i => i.JobSeekerID == 1234567890 && i.LocationCodes == "A,B,C")))
                .Returns(new AddLocationResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.AddProfileLocations(model);

            mockJobMatchProfileService.Verify(m => m.AddLocation(It.Is<AddLocationRequest>(i => i.JobSeekerID == 1234567890 && i.LocationCodes == "A,B,C")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLocations_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.AddLocation(It.IsAny<AddLocationRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.AddProfileLocations(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AddProfileLocations_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.AddLocation(It.IsAny<AddLocationRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.AddProfileLocations(model);
        }
        #endregion 

        #region CanEditProfile
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CanEditProfile_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.CanEditProfile(0);
        }

        [TestMethod]
        public void CanEditProfile_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.ValidateOrganisation(It.Is<ValidateOrganisationRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new ValidateOrganisationResponse() { ExecutionResult = SuccessResult(), UpdateAllowed = true });
            var service = SystemUnderTest();
            service.CanEditProfile(1234567890);

            mockJobMatchProfileService.Verify(m => m.ValidateOrganisation(It.Is<ValidateOrganisationRequest>(i => i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CanEditProfile_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.ValidateOrganisation(It.IsAny<ValidateOrganisationRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.CanEditProfile(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CanEditProfile_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.ValidateOrganisation(It.IsAny<ValidateOrganisationRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.CanEditProfile(1234567890);
        }
        #endregion 

        #region CreateOccupationProfile
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CreateOccupationProfile_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.CreateOccupationProfile(new OccupationProfileModel() { Occupations = new[] { new OccupationCategoryModel() { NewOccupationCatergory = "A", OldOccupationCatergory = "A", TimeStamp = 1234567890 } } });
        }

        [TestMethod]
        public void CreateOccupationProfile_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.Add(It.Is<AddRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new AddResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new [] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.CreateOccupationProfile(model);

            mockJobMatchProfileService.Verify(m => m.Add(It.Is<AddRequest>(i => i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CreateOccupationProfile_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Add(It.IsAny<AddRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new[] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.CreateOccupationProfile(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CreateOccupationProfile_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Add(It.IsAny<AddRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new[] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.CreateOccupationProfile(model);
        }
        #endregion 

        #region DeleteOccupationProfile
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void DeleteOccupationProfile_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.DeleteOccupationProfile(new DeleteOccupationProfileModel());
        }

        [TestMethod]
        public void DeleteOccupationProfile_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.Delete(It.Is<DeleteRequest>(i => i.JobSeekerID == 1234567890 && i.JobMatchPreferenceID == 1234567890)))
                .Returns(new DeleteResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new DeleteOccupationProfileModel() { JobSeekerID = 1234567890, OccupationProfileID = 1234567890 };
            service.DeleteOccupationProfile(model);

            mockJobMatchProfileService.Verify(m => m.Delete(It.Is<DeleteRequest>(i => i.JobSeekerID == 1234567890 && i.JobMatchPreferenceID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void DeleteOccupationProfile_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Delete(It.IsAny<DeleteRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new DeleteOccupationProfileModel() { JobSeekerID = 1234567890, OccupationProfileID = 1234567890 };
            service.DeleteOccupationProfile(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void DeleteOccupationProfile_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Delete(It.IsAny<DeleteRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new DeleteOccupationProfileModel() { JobSeekerID = 1234567890, OccupationProfileID = 1234567890 };
            service.DeleteOccupationProfile(model);
        }
        #endregion 

        #region FindVacancies
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindVacancies_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.FindVacancies(new FindVacancyModel());
        }

        [TestMethod]
        public void FindVacancies_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.VacancyJobSeekerFind(It.Is<VacancyJobSeekerFindRequest>(i => i.JobSeekerID == 1234567890 && i.JobMatchPreferenceID == 1234567890 && i.ExpandLocationStep == 0)))
                .Returns(new VacancyJobSeekerFindResponse() { ExecutionResult = SuccessResult(), VacancyJobSeekerList = new [] { new VacancyJobSeekerList() { VacancyID = 1234567890, VacancyTitle = "Job" } } });
            var service = SystemUnderTest();
            var model = new FindVacancyModel() { JobSeekerID = 1234567890, JobMatchProfileID = 1234567890, ExpandLocationStep = 0 };
            service.FindVacancies(model);

            mockJobMatchProfileService.Verify(m => m.VacancyJobSeekerFind(It.Is<VacancyJobSeekerFindRequest>(i => i.JobSeekerID == 1234567890 && i.JobMatchPreferenceID == 1234567890 && i.ExpandLocationStep == 0)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindVacancies_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.VacancyJobSeekerFind(It.IsAny<VacancyJobSeekerFindRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new FindVacancyModel() { JobSeekerID = 1234567890, JobMatchProfileID = 1234567890, ExpandLocationStep = 0 };
            service.FindVacancies(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindVacancies_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.VacancyJobSeekerFind(It.IsAny<VacancyJobSeekerFindRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new FindVacancyModel() { JobSeekerID = 1234567890, JobMatchProfileID = 1234567890, ExpandLocationStep = 0 };
            service.FindVacancies(model);
        }
        #endregion

        #region GetAllProfileDetails
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAllProfileDetails_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.GetAllProfileDetails(0);
        }

        [TestMethod]
        public void GetAllProfileDetails_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.GetDetails(It.Is<GetDetailsRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new GetDetailsResponse() { ExecutionResult = SuccessResult(), JobSeekerID = 1234567890, JobMatchPreferences = new [] { new JobMatchPreference() }, 
                    Licences = new [] { new Licence() { } }, LocationCodes = "A,B,C", Skills = "D,E,F" });
            var service = SystemUnderTest();

            service.GetAllProfileDetails(1234567890);

            mockJobMatchProfileService.Verify(m => m.GetDetails(It.Is<GetDetailsRequest>(i => i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAllProfileDetails_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetDetails(It.IsAny<GetDetailsRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetAllProfileDetails(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAllProfileDetails_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetDetails(It.IsAny<GetDetailsRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetAllProfileDetails(1234567890);
        }
        #endregion

        #region GetJobMatchFilters
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetJobMatchFilters_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.GetJobMatchFilters(0);
        }

        [TestMethod]
        public void GetJobMatchFilters_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.GetFilter(It.Is<GetFilterRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new GetFilterResponse() { ExecutionResult = SuccessResult(), AdwJMFFilterTypes = new[] { "A", "B", "C" } });
            var service = SystemUnderTest();
            service.GetJobMatchFilters(1234567890);

            mockJobMatchProfileService.Verify(m => m.GetFilter(It.Is<GetFilterRequest>(i => i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetJobMatchFilters_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetFilter(It.IsAny<GetFilterRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetJobMatchFilters(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetJobMatchFilters_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetFilter(It.IsAny<GetFilterRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetJobMatchFilters(1234567890);
        }
        #endregion 

        #region GetSkills
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSkills_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.GetSkills(0);
        }

        [TestMethod]
        public void GetSkills_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.GetSkills(It.Is<GetSkillsRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new GetSkillsResponse() { ExecutionResult = SuccessResult(), Skills = "A,B,C"});
            var service = SystemUnderTest();
            service.GetSkills(1234567890);

            mockJobMatchProfileService.Verify(m => m.GetSkills(It.Is<GetSkillsRequest>(i => i.JobSeekerID == 1234567890)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSkills_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetSkills(It.IsAny<GetSkillsRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetSkills(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSkills_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetSkills(It.IsAny<GetSkillsRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetSkills(1234567890);
        }
        #endregion 

        #region RemoveProfileLicence
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLicence_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.RemoveProfileLicence(new LicenceModel());
        }

        [TestMethod]
        public void RemoveProfileLicence_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLicence(It.Is<DeleteLicenceRequest>(i => i.JobSeekerID == 1234567890 && i.DeleteAll == false && i.IntegrityControlNumber == 1 && i.SequenceNumber == 2)))
                .Returns(new DeleteLicenceResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new LicenceModel() { JobSeekerID = 1234567890, IntegrityControlNumber = 1, SequenceNumber = 2 };
            service.RemoveProfileLicence(model);

            mockJobMatchProfileService.Verify(m => m.DeleteLicence(It.Is<DeleteLicenceRequest>(i => i.JobSeekerID == 1234567890 && i.DeleteAll == false && i.IntegrityControlNumber == 1 && i.SequenceNumber == 2)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLicence_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLicence(It.IsAny<DeleteLicenceRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new LicenceModel() { JobSeekerID = 1234567890, IntegrityControlNumber = 1, SequenceNumber = 2 };
            service.RemoveProfileLicence(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLicence_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLicence(It.IsAny<DeleteLicenceRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new LicenceModel() { JobSeekerID = 1234567890, IntegrityControlNumber = 1, SequenceNumber = 2 };
            service.RemoveProfileLicence(model);
        }
        #endregion 

        #region RemoveProfileLocations
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLocations_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.RemoveProfileLocations(new ProfileLocationsModel());
        }

        [TestMethod]
        public void RemoveProfileLocations_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLocation(It.Is<DeleteLocationRequest>(i => i.JobSeekerID == 1234567890 && i.LocationCodes == "A,B,C")))
                .Returns(new DeleteLocationResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.RemoveProfileLocations(model);

            mockJobMatchProfileService.Verify(m => m.DeleteLocation(It.Is<DeleteLocationRequest>(i => i.JobSeekerID == 1234567890 && i.LocationCodes == "A,B,C")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLocations_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLocation(It.IsAny<DeleteLocationRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.RemoveProfileLocations(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void RemoveProfileLocations_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.DeleteLocation(It.IsAny<DeleteLocationRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new ProfileLocationsModel() { JobSeekerID = 1234567890, LocationCodes = "A,B,C" };
            service.RemoveProfileLocations(model);
        }
        #endregion

        #region SaveJobMatchFilters
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveJobMatchFilters_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.SaveJobMatchFilters(new FilterModel());
        }

        [TestMethod]
        public void SaveJobMatchFilters_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.SetFilter(It.Is<SetFilterRequest>(i => i.JobSeekerID == 1234567890 && i.AdwJMFFilterTypes.SequenceEqual(new string[] { "A", "B", "C" }))))
                .Returns(new SetFilterResponse() { ExecutionResult = SuccessResult(), Success = true });
            var service = SystemUnderTest();
            var model = new FilterModel() { JobSeekerID = 1234567890, Filters = new string[] { "A", "B", "C" } };
            service.SaveJobMatchFilters(model);

            mockJobMatchProfileService.Verify(m => m.SetFilter(It.Is<SetFilterRequest>(i => i.JobSeekerID == 1234567890 && i.AdwJMFFilterTypes.SequenceEqual(new string[] { "A", "B", "C" }))), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveJobMatchFilters_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SetFilter(It.IsAny<SetFilterRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new FilterModel() { JobSeekerID = 1234567890, Filters = new string[] { "A", "B", "C" } };
            service.SaveJobMatchFilters(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveJobMatchFilters_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SetFilter(It.IsAny<SetFilterRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new FilterModel() { JobSeekerID = 1234567890, Filters = new string[] { "A", "B", "C" } };
            service.SaveJobMatchFilters(model);
        }
        #endregion

        #region SaveSkills
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveSkills_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.SaveSkills(new JobSeekerSkillsModel());
        }

        [TestMethod]
        public void SaveSkills_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.SaveSkills(It.Is<SaveSkillsRequest>(i => i.JobSeekerID == 1234567890 && i.SkillText == "A,B,C")))
                .Returns(new SaveSkillsResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new JobSeekerSkillsModel() { JobSeekerID = 1234567890, Skills = "A,B,C" };
            service.SaveSkills(model);

            mockJobMatchProfileService.Verify(m => m.SaveSkills(It.Is<SaveSkillsRequest>(i => i.JobSeekerID == 1234567890 && i.SkillText == "A,B,C")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveSkills_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SaveSkills(It.IsAny<SaveSkillsRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new JobSeekerSkillsModel() { JobSeekerID = 1234567890, Skills = "A,B,C" };
            service.SaveSkills(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveSkills_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SaveSkills(It.IsAny<SaveSkillsRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new JobSeekerSkillsModel() { JobSeekerID = 1234567890, Skills = "A,B,C" };
            service.SaveSkills(model);
        }
        #endregion

        #region UpdateOccupationProfileDetails
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateOccupationProfileDetails_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.UpdateOccupationProfileDetails(new OccupationProfileModel() { Occupations = new[] { new OccupationCategoryModel() { NewOccupationCatergory = "A", OldOccupationCatergory = "A", TimeStamp = 1234567890 } } });
        }

        [TestMethod]
        public void UpdateOccupationProfileDetails_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.Update(It.Is<UpdateRequest>(i => i.JobSeekerID == 1234567890 && i.JobHours == "A" && i.JobMatchPreferenceID == 1234567891)))
                .Returns(new UpdateResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new[] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.UpdateOccupationProfileDetails(model);

            mockJobMatchProfileService.Verify(m => m.Update(It.Is<UpdateRequest>(i => i.JobSeekerID == 1234567890 && i.JobHours == "A" && i.JobMatchPreferenceID == 1234567891)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateOccupationProfileDetails_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Update(It.IsAny<UpdateRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new[] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.UpdateOccupationProfileDetails(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void UpdateOccupationProfileDetails_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.Update(It.IsAny<UpdateRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            var model = new OccupationProfileModel()
            {
                CreatedBy = "BB1111",
                HasIndustryAccreditation = false,
                HasRelatedQualifications = false,
                IndustryAccreditation = "",
                JobHours = "A",
                JobSeekerID = 1234567890,
                JobType = "A",
                OccupationProfileID = 1234567891,
                Occupations = new[] { new OccupationCategoryModel()
                    {
                        NewOccupationCatergory = "8414",
                        OldOccupationCatergory = "8414",
                        TimeStamp = 1234567890
                    }
                },
                RelatedQualifications = "",
                ResumeID = 1234567890,
                ResumeTitle = "Test Resume",
                TimeStamp = 1234567890,
                Title = "Plumbing"
            };
            service.UpdateOccupationProfileDetails(model);
        }
        #endregion 


        #region GetVOPStatus
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetVOPStatus_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.GetVOPStatus(0);
        }

        [TestMethod]
        public void GetVOPStatus_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.GetVOPStatus(It.Is<GetVOPStatusRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new GetVOPStatusResponse() { ExecutionResult = SuccessResult(), VOPStatusItem = new VOPStatusItem[] { new VOPStatusItem() { Status = "C" } } });
            var service = SystemUnderTest();
            var model = service.GetVOPStatus(1234567890);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Status, "C");
        }

        [TestMethod]
        public void GetVOPStatus_Successful2()
        {
            mockJobMatchProfileService.Setup(m => m.GetVOPStatus(It.Is<GetVOPStatusRequest>(i => i.JobSeekerID == 1234567890)))
                .Returns(new GetVOPStatusResponse() { ExecutionResult = SuccessResult(), VOPStatusItem = null });
            var service = SystemUnderTest();
            var model = service.GetVOPStatus(1234567890);
            Assert.IsNull(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetVOPStatus_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetVOPStatus(It.IsAny<GetVOPStatusRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetVOPStatus(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetVOPStatus_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.GetVOPStatus(It.IsAny<GetVOPStatusRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetVOPStatus(1234567890);
        }
        #endregion


        #region SetCompletedVOPStatus
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SetCompletedVOPStatus_WithNoJobSeekerId_ThrowsServiceValidationException()
        {
            var service = SystemUnderTest();
            service.SetCompletedVOPStatus(0);
        }

        [TestMethod]
        public void SetCompletedVOPStatus_Successful()
        {
            mockJobMatchProfileService.Setup(m => m.SetVOPStatus(It.Is<SetVOPStatusRequest>(i => i.JobSeekerID == 1234567890 && i.Status == "C")))
                .Returns(new SetVOPStatusResponse() { ExecutionResult = SuccessResult() });
            var service = SystemUnderTest();
            service.SetCompletedVOPStatus(1234567890);
            mockJobMatchProfileService.Verify(m => m.SetVOPStatus(It.Is<SetVOPStatusRequest>(i => i.JobSeekerID == 1234567890 && i.Status == "C")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SetCompletedVOPStatus_FaultException_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SetVOPStatus(It.IsAny<SetVOPStatusRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.SetCompletedVOPStatus(1234567890);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SetCompletedVOPStatus_FaultException2_ThrowsServiceValidationException()
        {
            mockJobMatchProfileService.Setup(m => m.SetVOPStatus(It.IsAny<SetVOPStatusRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.SetCompletedVOPStatus(1234567890);
        }
        #endregion
    }
}
