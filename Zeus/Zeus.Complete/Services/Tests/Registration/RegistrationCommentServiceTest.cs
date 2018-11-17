using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.GenericComments.Contracts.DataContracts;
using Employment.Esc.GenericComments.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Service.Interfaces.Common;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class RegistrationCommentServiceTest
    {
        //Process of method execuation in unit test project
        // [AssemblyInitialize]->[ClassInitialize]->[TestInitialize]->[TestMethod]->[TestCleanUp]->[ClassCleanUp]->[AssemblyCleanUp]

        //[AssemblyInitialize] - is executed before any of the unit test method in any of the classes in the assembly.
        //[AssemblyCleanUp] - is executed after any of the unit test method in any of the classes in the assembly.

        // [ClassInitialize] and [ClassCleanUp] - both method are static - ClassInitialize take TestContext as input parameter
        // Class Initialize is first method which is executed in Class then run all the test cases and then ClassCleanUp is execute
        // ClassInitialize - Initialize all the resource which is intended to used across all unit test with in Testclass

        //Use TestInitialize to run code before running each test
        // [TestInitialize] And [TestCleanUp] -  only one method has this attribute - It is run before and after the each unit test method

        // Asserting the facts
        // Assert, CollectionAssert, StringAssert
        // Assert - compare two input values, Many methods with several overloads
        // CollectionAssert - compare two collections, Check item in Collection
        // StringAssert - compare strings

        //TestContext
        //Run-time information - populated by unitest framework at runtime
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IGenericComments> mockCommentWcf;
        private Mock<IAdwService> adwService;

        private RegistrationService SystemUnderTest()
        {
            return new RegistrationService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, adwService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new RegistrationCommentMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }


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
            mockCommentWcf = new Mock<IGenericComments>();
            adwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IGenericComments>("GenericComments.svc")).Returns(mockCommentWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new RegistrationService(null, null, null, null);
        }


        #region List

        /// <summary>
        /// Test get runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetAllComments_Valid()
        {
            //Arrange
            long JobSeekerID = 12345678;
            var inModel = new JobseekerModel { JobSeekerId = JobSeekerID };
            var request = MappingEngine.Map<ListRequest>(inModel);
            var list = new List<Employment.Esc.GenericComments.Contracts.DataContracts.Comment>();
            list.Add(new Employment.Esc.GenericComments.Contracts.DataContracts.Comment { CommentID = JobSeekerID, CommentType = "JSK" });
            var response = new ListResponse { Comments = list.ToArray(), ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);

            mockMappingEngine.Setup(m => m.Map<ListRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.List(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);

            //Act
            var result = SystemUnderTest().GetAllComments(inModel);

            //Assert
            Assert.IsTrue(result.Comments.Count() > 0);
            Assert.IsTrue(result.Comments.ElementAt(0).CommentID == JobSeekerID);
            mockMappingEngine.Verify(m => m.Map<ListRequest>(inModel), Times.Once());
            mockCommentWcf.Verify(m => m.List(request), Times.Once());
            mockMappingEngine.Verify(m => m.Map<JobseekerModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAllComments_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            long JobSeekerID = 12345678;
            var inModel = new JobseekerModel { JobSeekerId = JobSeekerID };
            var request = MappingEngine.Map<ListRequest>(inModel);
            var list = new List<Employment.Esc.GenericComments.Contracts.DataContracts.Comment>();
            list.Add(new Employment.Esc.GenericComments.Contracts.DataContracts.Comment { CommentID = JobSeekerID, CommentType = "JSK" });
            var response = new ListResponse { Comments = list.ToArray(), ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);

            mockMappingEngine.Setup(m => m.Map<ListRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.List(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);

            //Act
            SystemUnderTest().GetAllComments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAllComments_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            long JobSeekerID = 12345678;
            var inModel = new JobseekerModel { JobSeekerId = JobSeekerID };
            var request = MappingEngine.Map<ListRequest>(inModel);
            var list = new List<Employment.Esc.GenericComments.Contracts.DataContracts.Comment>();
            list.Add(new Employment.Esc.GenericComments.Contracts.DataContracts.Comment { CommentID = JobSeekerID, CommentType = "JSK" });
            var response = new ListResponse { Comments = list.ToArray(), ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);

            mockMappingEngine.Setup(m => m.Map<ListRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.List(request)).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);
            //Act
            SystemUnderTest().GetAllComments(inModel);

            // mockCommentWcf.Setup(m => m.List(It.IsAny<ListRequest>())).Throws<FaultException>();
            // SystemUnderTest().GetAll(new CommentModel());
        }

        #endregion

        #region Add

        /// <summary>
        /// Creates the comment for jobseeker.
        /// </summary>
        [TestMethod]
        public void CreateComment_Valid()
        {
            //Arrange
            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP" };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<AddRequest>(inModel);
            var response = new AddResponse();

            mockMappingEngine.Setup(m => m.Map<AddRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.Add(request)).Returns(response);

            //Act
            SystemUnderTest().CreateComment(inModel);

            //Assert
            mockMappingEngine.Verify(m => m.Map<AddRequest>(inModel), Times.Once());
            mockCommentWcf.Verify(m => m.Add(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CreateComment_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            //Arrange
            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP" };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<AddRequest>(inModel);
            var response = new AddResponse();
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockMappingEngine.Setup(m => m.Map<AddRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.Add(request)).Throws(exception);

            //Act
            SystemUnderTest().CreateComment(inModel);

        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void CreateComment_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP" };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<AddRequest>(inModel);
            var response = new AddResponse();

            mockMappingEngine.Setup(m => m.Map<AddRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.Add(request)).Throws(exception);

            //Act
            SystemUnderTest().CreateComment(inModel);
        }


        /// <summary>
        /// Gets the comment details successful test.
        /// </summary>
        [TestMethod]
        public void Get_Comment_Details()
        {
            //Arrange
            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP", SequenceNumber = 1 };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<GetDetailsRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<GetDetailsRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.GetDetails(request)).Returns(new GetDetailsResponse());
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<GetDetailsResponse>())).Returns(new JobseekerModel());

            //Act
            SystemUnderTest().GetComment(inModel);

            //Assert     
            mockMappingEngine.Verify(m => m.Map<GetDetailsRequest>(inModel), Times.Once());
            mockCommentWcf.Verify(m => m.GetDetails(It.IsAny<GetDetailsRequest>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_Comment_Details_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            //Arrange
            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP", SequenceNumber = 1 };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<GetDetailsRequest>(inModel);
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockMappingEngine.Setup(m => m.Map<GetDetailsRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.GetDetails(request)).Throws(exception);

            //Act
            SystemUnderTest().GetComment(inModel);

        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_Comment_Details_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            long JobSeekerID = 12345678;
            var comment = new CommentModel { CommentID = JobSeekerID, Text = "This is fun creating a testing generic Comment", Topic = "APP", SequenceNumber = 1 };
            var inModel = new JobseekerModel { CurrentComment = comment };
            var request = MappingEngine.Map<GetDetailsRequest>(inModel);

            mockMappingEngine.Setup(m => m.Map<GetDetailsRequest>(inModel)).Returns(request);
            mockCommentWcf.Setup(m => m.GetDetails(request)).Throws(exception);

            //Act
            SystemUnderTest().GetComment(inModel);
        }

        /// <summary>
        /// Update_comments this instance.
        /// </summary>
        [TestMethod]
        public void Update_comment()
        {
            //Arrange
            mockMappingEngine.Setup(m => m.Map<UpdateRequest>(It.IsAny<JobseekerModel>())).Returns(new UpdateRequest());
            mockCommentWcf.Setup(m => m.Update(It.IsAny<UpdateRequest>())).Returns(new UpdateResponse());
            
            //Act
            SystemUnderTest().UpdateComment(It.IsAny<JobseekerModel>());

            //Assert
            mockMappingEngine.Verify(m => m.Map<UpdateRequest>(It.IsAny<JobseekerModel>()), Times.Once());
            mockCommentWcf.Verify(m => m.Update(It.IsAny<UpdateRequest>()), Times.Once());

        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Update_comment_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            mockMappingEngine.Setup(m => m.Map<UpdateRequest>(It.IsAny<JobseekerModel>())).Returns(new UpdateRequest());
            mockCommentWcf.Setup(m => m.Update(It.IsAny<UpdateRequest>())).Throws(exception);

            //Act
            SystemUnderTest().UpdateComment(It.IsAny<JobseekerModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Update_comment_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            mockMappingEngine.Setup(m => m.Map<UpdateRequest>(It.IsAny<JobseekerModel>())).Returns(new UpdateRequest());
            mockCommentWcf.Setup(m => m.Update(It.IsAny<UpdateRequest>())).Throws(exception);

            //Act
            SystemUnderTest().UpdateComment(It.IsAny<JobseekerModel>());
        }

        #endregion
    }
}
