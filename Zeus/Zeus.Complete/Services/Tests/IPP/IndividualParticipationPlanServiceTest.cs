using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Employment.Esc.EmploymentPathwayPlan.Contracts.DataContracts;
using Employment.Esc.EmploymentPathwayPlan.Contracts.FaultContracts;
using Employment.Esc.EmploymentPathwayPlan.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Service.Implementation.IndividualParticipationPlan;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Web.Mvc.Service.Interfaces.IndividualParticipationPlan;
using Moq;
using IEmploymentPathwayPlan = Employment.Esc.EmploymentPathwayPlan.Contracts.ServiceContracts.IEmploymentPathwayPlan;

namespace Employment.Web.Mvc.Service.Tests.IPP
{  
    /// <summary>
    /// Summary description for IndividualParticipationPlanServiceTest
    /// </summary>
    [TestClass]
    public class IndividualParticipationPlanServiceTest
    {
        private IndividualParticipationPlanService SystemUnderTest()
        {
            return new IndividualParticipationPlanService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new IndividualParticipationPlanMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }
                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IEmploymentPathwayPlan> mockIPPWcf;

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
            mockIPPWcf = new Mock<IEmploymentPathwayPlan>();
            mockClient.Setup(m => m.Create<IEmploymentPathwayPlan>("EmploymentPathwayPlan.svc")).Returns(mockIPPWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCalledWithNullArgumentsThrowsArgumentNullException()
        {
            new IndividualParticipationPlanService(null, null, null);
        }

        #region GetPlanDetails


        /// <summary>
        /// Test find runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetPlanDetailsValidResults()
        {

            var inModel = new IPPModel { JobseekerId = 12345678 , CurrentContractType = "RJCP"};
            var request = MappingEngine.Map<PlanGetRequest>(inModel);
            var response = new PlanGetResponse
            {
                ActivityList = new List<PlanActivity>
                                           {
                                               new PlanActivity {ActivityCode = "EM53", ActivityCategoryCode = "AC02", ActivitySeqNum = 5}
                                           },
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };

            var outModel = MappingEngine.Map<IPPModel>(response);
            var outList = MappingEngine.Map<IEnumerable<PlanItemModel>>(response.ActivityList);

            mockMappingEngine.Setup(m => m.Map<PlanGetRequest>(inModel)).Returns(request);
            mockIPPWcf.Setup(m => m.GetAll( It.IsAny<PlanGetRequest>())).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IPPModel>(response)).Returns(outModel);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<PlanItemModel>>(response.ActivityList)).Returns(outList);

            var result = SystemUnderTest().GetAll(inModel.JobseekerId);

            Assert.IsTrue(result.ActivityList.Count() == response.ActivityList.Count());
            Assert.IsTrue(result.ActivityList.First().PlanItemSeqNum == response.ActivityList.First().ActivitySeqNum);
            mockIPPWcf.Verify(m => m.GetAll(It.IsAny<PlanGetRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IPPModel>(response), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<PlanItemModel>>(response.ActivityList), Times.Once());
        }

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetPlanDetailsThrowsFaultExceptionThrowsServiceValidationException()
        //{
        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

        //    var inModel = new IPPModel { JobseekerId = 12345678 };
        //    var request = MappingEngine.Map<PlanGetRequest>(inModel);
        //    var response = new PlanGetResponse
        //                       {
        //                           ActivityList =
        //                               new List<PlanActivity>
        //                                   {
        //                                       new PlanActivity
        //                                           {ActivityCode = "", ActivityCategoryCode = "", ActivitySeqNum = 0}
        //                                   }
        //                       };

        //    var outModel = MappingEngine.Map<IPPModel>(response);

        //    mockMappingEngine.Setup(m => m.Map<PlanGetRequest>(inModel)).Returns(request);
        //    mockIPPWcf.Setup(m => m.GetAll(request)).Throws(exception);
        //    mockMappingEngine.Setup(m => m.Map<IPPModel>(response)).Returns(outModel);

        //    SystemUnderTest().GetAll();
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetPlanDetailsWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        //{
        //    var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });


        //    var inModel = new IPPModel { JobseekerId = 12345678 };
        //    var request = MappingEngine.Map<PlanGetRequest>(inModel);
        //    var response = new PlanGetResponse
        //    {
        //        ActivityList =
        //            new List<PlanActivity>
        //                                   {
        //                                       new PlanActivity
        //                                           {ActivityCode = "", ActivityCategoryCode = "", ActivitySeqNum = 0}
        //                                   }
        //    };

        //    var outModel = MappingEngine.Map<IPPModel>(response);

        //    mockMappingEngine.Setup(m => m.Map<PlanGetRequest>(inModel)).Returns(request);
        //    mockIPPWcf.Setup(m => m.GetAll(request)).Throws(exception);
        //    mockMappingEngine.Setup(m => m.Map<IPPModel>(response)).Returns(outModel);

        //    SystemUnderTest().GetAll();
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetPlanDetailsThrowsFaultExceptionThrowsPaymentsFault()
        //{
        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));


        //    var inModel = new IPPModel { JobseekerId = 12345678 };
        //    var request = MappingEngine.Map<PlanGetRequest>(inModel);
        //    var response = new PlanGetResponse
        //    {
        //        ActivityList =
        //            new List<PlanActivity>
        //                                   {
        //                                       new PlanActivity
        //                                           {ActivityCode = "", ActivityCategoryCode = "", ActivitySeqNum = 0}
        //                                   }
        //    };

        //    var outModel = MappingEngine.Map<IPPModel>(response);

        //    mockMappingEngine.Setup(m => m.Map<PlanGetRequest>(inModel)).Returns(request);
        //    mockIPPWcf.Setup(m => m.GetAll(request)).Throws(exception);
        //    mockMappingEngine.Setup(m => m.Map<IPPModel>(response)).Returns(outModel);

        //    SystemUnderTest().GetAll();
        //}

        #endregion
    }
}
