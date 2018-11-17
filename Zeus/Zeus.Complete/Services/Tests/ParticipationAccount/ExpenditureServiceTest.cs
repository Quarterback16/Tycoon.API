using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Employment.Esc.ParticipationAccount.Contracts.DataContracts.ExpenditureSearch;
using Employment.Esc.ParticipationAccount.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.ParticipationAccount;
using Employment.Web.Mvc.Service.Interfaces.ParticipationAccount;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.ParticipationAccount
{

    [TestClass]
    public class ExpenditureServiceTest
    {
        private Mock<IParticipationAccount> mockExpenditureSearchWcf;
        private Mock<IClient> mockClient;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ParticipationAccountMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        public ExpenditureService SystemUnderTest()
        {
            
            return new ExpenditureService(mockAdwService.Object, mockClient.Object, MappingEngine, mockCacheService.Object);
        }

        /// <summary>
        /// Initialise the mock object before run each test case
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            mockExpenditureSearchWcf = new Mock<IParticipationAccount>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();
            mockCacheService = new Mock<ICacheService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockClient = new Mock<IClient>();
 
            //WCF services
             mockClient.Setup(m => m.Create<IParticipationAccount>("ParticipationAccount.svc")).Returns(mockExpenditureSearchWcf.Object);


        }

        /// <summary>
        /// test to get expenditure search results successfully
        /// </summary>
        [TestMethod]
        public void ListExpenditure_Valid()
        {

            //1. Setup dummy request data
            var request = CreateDummyExpenditureListModel();

            //2. set up dummy response data
            List<SearchItemResponse> expenditureList = new List<SearchItemResponse>();

            for (int i = 1; i < 10; i++)
            {
                expenditureList.Add(CreateDummyExpenditureResponse(i));
            }

            var response = new SearchListResponse
            {
                MoreDataflag = "Y",
                NextExpendiId = 12390,
                ResponseList = expenditureList.ToArray()
            };

            //3. mock expenditure search wcf
            mockExpenditureSearchWcf.Setup(m => m.GetExpenditureList(It.IsAny<SearchRequest>())).Returns(response);

            //4. execute the method
            var result = SystemUnderTest().ListExpenditure(request);


            //5. Verification
            //Verify More parameters
            Assert.AreEqual("Y", result.HasMoreRecords);
            Assert.AreEqual(response.NextExpendiId, result.NextExpendId);
            //Verify response list
            Assert.AreEqual(response.ResponseList.Length, result.ListOfExpenditue.Count());

            //Verify behaviour
            mockExpenditureSearchWcf.Verify(m => m.GetExpenditureList(It.Is<SearchRequest>(r => r.Site == request.SiteCode)), Times.Once());
 
        }


        /// <summary>
        /// Test ToServiceValidationException is thrown when WCF throws FaultException/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ExPenditureList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = CreateDummyFaultException();

            var request = CreateDummyExpenditureListModel();


            mockExpenditureSearchWcf.Setup(m => m.GetExpenditureList(It.IsAny<SearchRequest>())).Throws(exception);
            var result = SystemUnderTest().ListExpenditure(request);
        }

        /// <summary>
        /// create dummy SearchItemResponse object
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private SearchItemResponse CreateDummyExpenditureResponse(int i)
        {
            var data = new SearchItemResponse()
            {
                CategoryCd = "CategoryCode" + i.ToString(),
                SubCategoryCd = "subCategorCd" + i.ToString(),
                ExpendId = 1 + i,
                ExpendSeqNum = 1,
                JobseekerId = 10000000 + i,
                ClaimId = 10000 + i,
                StatusCd = "StatusCode" + i.ToString(),
            };

            return data;
        }

        private ExpenditureListModel CreateDummyExpenditureListModel()
        {
            var data = new ExpenditureListModel
            {
                SequenceNumber = 1,
                ContractId = "Cont0001",
                SiteCode = "QM60",
                Region = "Region"
            };

            return data;
        }

        private ExecutionResult CreateDummyFailedExecutionResult()
        {
            var executionResult = new ExecutionResult() { Status = ExecuteStatus.Failed };
            executionResult.ExecuteMessages.Add(new ExecutionMessage(ExecutionMessageType.Error, "Access denied"));

            return executionResult;
        }


        private FaultException CreateDummyFaultException()
        {

            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            return exception;

        }

        private FaultException<ValidationFault> CreateDummyFaultExceptionValidationFault()
        {

            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            return exception;

        }

    }
}
