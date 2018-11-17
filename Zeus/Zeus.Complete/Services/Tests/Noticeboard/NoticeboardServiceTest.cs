using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Noticeboard.Contracts.DataContracts;
using Employment.Esc.Noticeboard.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Noticeboard;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Noticeboard
{
    /// <summary>
    /// Unit tests for <see cref="NoticeboardService" />.
    /// </summary>
    [TestClass]
    public class NoticeboardServiceTest
    {
        private NoticeboardService SystemUnderTest()
        {
            return new NoticeboardService(mockAdwService.Object,mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new NoticeboardMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<INoticeboardMessages> mockNoticeboardWcf;

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
            mockNoticeboardWcf = new Mock<INoticeboardMessages>();

            mockClient.Setup(m => m.Create<INoticeboardMessages>("NoticeboardMessages.svc")).Returns(mockNoticeboardWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new NoticeboardService(null,null, null, null);
        }

        //#region GetSummary tests

        ///// <summary>
        ///// Test GetSummary runs successfully and returns expected results on valid use.
        ///// </summary>
        //[TestMethod]
        //public void GetSummary_Valid()
        //{
        //    var siteCode = "HA10";

        //    var inModel = new SummaryModel { SearchCriteriaSiteCode = siteCode };
        //    var request = MappingEngine.Map<MessageCountsGetRequest>(inModel);
        //    var response = new MessageCountsGetResponse
        //    {
        //        ManagedByUsers = new List<string> { "AA0001", "BB0001" },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } },
        //        SiteInfo = new SiteInformation()
        //    };
        //    var outModel = MappingEngine.Map<SummaryModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageCountsGetRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<SummaryModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageCountsGet(request)).Returns(response);

        //    var result = SystemUnderTest().GetSummary(inModel);

        //    Assert.IsTrue(result.MessageCounts.Count() == outModel.MessageCounts.Count());
        //    Assert.IsTrue(result.MessageCounts.First().Key == outModel.MessageCounts.First().Key);
        //    Assert.IsTrue(result.ManagedByUsers.Count() == outModel.ManagedByUsers.Count());
        //    Assert.IsTrue(result.ManagedByUsers.First() == outModel.ManagedByUsers.First());
        //    mockMappingEngine.Verify(m => m.Map<MessageCountsGetRequest>(inModel), Times.Once());
        //    mockNoticeboardWcf.Verify(m => m.MessageCountsGet(request), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<SummaryModel>(response), Times.Once());
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetSummary_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
        //    var inModel = new SummaryModel { SearchCriteriaSiteCode = siteCode };
        //    var request = MappingEngine.Map<MessageCountsGetRequest>(inModel);
        //    var response = new MessageCountsGetResponse
        //    {
        //        ManagedByUsers = new List<string> { "AA0001", "BB0001" },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<SummaryModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageCountsGetRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<SummaryModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageCountsGet(request)).Throws(exception);

        //    SystemUnderTest().GetSummary(inModel);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetSummary_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
        //    var inModel = new SummaryModel { SearchCriteriaSiteCode = siteCode };
        //    var request = MappingEngine.Map<MessageCountsGetRequest>(inModel);
        //    var response = new MessageCountsGetResponse
        //    {
        //        ManagedByUsers = new List<string> { "AA0001", "BB0001" },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<SummaryModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageCountsGetRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<SummaryModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageCountsGet(request)).Throws(exception);

        //    SystemUnderTest().GetSummary(inModel);
        //}

        //#endregion

        //#region GetAppointmentList tests

        ///// <summary>
        ///// Test GetList runs successfully and returns expected results on valid use for message types NARDUE, NAATDY which call MessageAppointmentsList wcf method.
        ///// </summary>
        //[TestMethod]
        //public void GetAppointmentList_Valid()
        //{
        //    var siteCode = "HA10";

        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NARDUE" } };
        //    var request = MappingEngine.Map<MessageAppointmentsListRequest>(inModel);
        //    var response = new MessageAppointmentsListResponse
        //    {
        //        MessageAppointments = new List<MessageAppointmentData> { new MessageAppointmentData { AppointmentId = 1, JobseekerId = 2 }},
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageAppointmentsListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageAppointmentsList(request)).Returns(response);

        //    var result = SystemUnderTest().GetList(inModel);

        //    Assert.IsTrue(result.AppointmentsList.Count() == outModel.AppointmentsList.Count());
        //    Assert.IsTrue(result.AppointmentsList.First().AppointmentID == outModel.AppointmentsList.First().AppointmentID);
        //    Assert.IsNull(result.MessagesList);
        //    Assert.IsTrue(result.MessageCounts.Count() == outModel.MessageCounts.Count());
        //    Assert.IsTrue(result.MessageCounts.First().Key == outModel.MessageCounts.First().Key);
        //    mockMappingEngine.Verify(m => m.Map<MessageAppointmentsListRequest>(inModel), Times.Once());
        //    mockNoticeboardWcf.Verify(m => m.MessageAppointmentsList(request), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<ListModel>(response), Times.Once());
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetAppointmentList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NARDUE" } };
        //    var request = MappingEngine.Map<MessageAppointmentsListRequest>(inModel);
        //    var response = new MessageAppointmentsListResponse
        //    {
        //        MessageAppointments = new List<MessageAppointmentData> { new MessageAppointmentData { AppointmentId = 1, JobseekerId = 2 } },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageAppointmentsListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageAppointmentsList(request)).Throws(exception);

        //    SystemUnderTest().GetList(inModel);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetAppointmentList_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NARDUE" } };
        //    var request = MappingEngine.Map<MessageAppointmentsListRequest>(inModel);
        //    var response = new MessageAppointmentsListResponse
        //    {
        //        MessageAppointments = new List<MessageAppointmentData> { new MessageAppointmentData { AppointmentId = 1, JobseekerId = 2 } },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessageAppointmentsListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessageAppointmentsList(request)).Throws(exception);

        //    SystemUnderTest().GetList(inModel);
        //}

        //#endregion

        //#region GetMessageList tests

        ///// <summary>
        ///// Test GetList runs successfully and returns expected results on valid use for message types NOT NARDUE, NAATDY which call MessagesList wcf method.
        ///// </summary>
        //[TestMethod]
        //public void GetMessageList_Valid()
        //{
        //    var siteCode = "HA10";

        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NNNN" } };
        //    var request = MappingEngine.Map<MessagesListRequest>(inModel);
        //    var response = new MessagesListResponse
        //    {
        //        Messages = new List<MessageData> { new MessageData { MessageId = 1, JobseekerId = 2, MessageContent = "content1,content2" } },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessagesListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessagesList(request)).Returns(response);

        //    var result = SystemUnderTest().GetList(inModel);

        //    Assert.IsTrue(result.MessagesList.Count() == outModel.MessagesList.Count());
        //    Assert.IsTrue(result.MessagesList.First().MessageID == outModel.MessagesList.First().MessageID);
        //    Assert.IsTrue(result.MessagesList.First().MessageContentParts[0] == "content1");
        //    Assert.IsTrue(result.MessagesList.First().MessageContentParts[1] == "content2");
        //    Assert.IsNull(result.AppointmentsList);
        //    Assert.IsTrue(result.MessageCounts.Count() == outModel.MessageCounts.Count());
        //    Assert.IsTrue(result.MessageCounts.First().Key == outModel.MessageCounts.First().Key);
        //    mockMappingEngine.Verify(m => m.Map<MessagesListRequest>(inModel), Times.Once());
        //    mockNoticeboardWcf.Verify(m => m.MessagesList(request), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<ListModel>(response), Times.Once());
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetMessageList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NNNN" } };
        //    var request = MappingEngine.Map<MessagesListRequest>(inModel);
        //    var response = new MessagesListResponse
        //    {
        //        Messages = new List<MessageData> { new MessageData { MessageId = 1, JobseekerId = 2 } },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessagesListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessagesList(request)).Throws(exception);

        //    SystemUnderTest().GetList(inModel);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetMessageList_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var siteCode = "HA10";

        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
        //    var inModel = new ListModel { SearchCriteriaSiteCode = siteCode, SearchCriteriaMessageTypes = new List<string> { "NNNN" } };
        //    var request = MappingEngine.Map<MessagesListRequest>(inModel);
        //    var response = new MessagesListResponse
        //    {
        //        Messages = new List<MessageData> { new MessageData { MessageId = 1, JobseekerId = 2 } },
        //        MessageCounts = new List<MessageCountData> { new MessageCountData { Count = 3, MessageType = "N" } }
        //    };
        //    var outModel = MappingEngine.Map<ListModel>(response);

        //    mockUserService.Setup(m => m.SiteCode).Returns(siteCode);
        //    mockMappingEngine.Setup(m => m.Map<MessagesListRequest>(inModel)).Returns(request);
        //    mockMappingEngine.Setup(m => m.Map<ListModel>(response)).Returns(outModel);
        //    mockNoticeboardWcf.Setup(m => m.MessagesList(request)).Throws(exception);

        //    SystemUnderTest().GetList(inModel);
        //}

        //#endregion
    }
}