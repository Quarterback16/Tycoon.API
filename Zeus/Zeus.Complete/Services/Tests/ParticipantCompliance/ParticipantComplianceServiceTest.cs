
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Employment.Esc.ComplianceReports.Contracts.DataContracts;
using Employment.Esc.ComplianceReports.Contracts.DataContracts.CcaInterventions;
using Employment.Esc.ComplianceReports.Contracts.DataContracts.Evidences;
using Employment.Esc.ComplianceReports.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.ParticipantCompliance;
using Employment.Web.Mvc.Service.Interfaces.ParticipantCompliance;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.ParticipantCompliance
{
    /// <summary>
    /// Unit tests for <see cref="JCAService" />.
    /// </summary>
    [TestClass]
    public class ParticipantComplianceServiceTest
    {
        private ParticipantComplianceService SystemUnderTest()
        {
            return new ParticipantComplianceService( mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, mockAdwService.Object);
        }

        private IMappingEngine mappingEngine;
        protected IMappingEngine MappingEngine
        {
            get
            {
                if ( mappingEngine == null)
                {
                    var mapper = new ParticipantComplianceMapper();
                    mapper.Map( Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;
        private Mock<Employment.Esc.ComplianceReports.Contracts.ServiceContracts.IComplianceReportsRetrieval> mockComplianceRetrievalWcf;

        //Use Test Initialize to run code before running each test
        [TestInitialize]
        public void TestInitialize( )
        {
            mockClient = new Mock<IClient>( );
            mockMappingEngine = new Mock<IMappingEngine>( );
            mockCacheService = new Mock<ICacheService>( );
            mockUserService = new Mock<IUserService>( );
            mockUserService.SetupGet( m => m.Username).Returns( "PC0437"); 
            mockComplianceRetrievalWcf = new Mock<Employment.Esc.ComplianceReports.Contracts.ServiceContracts.IComplianceReportsRetrieval>();
            mockAdwService = new Mock<IAdwService>( );
            mockClient.Setup(m => m.Create<Employment.Esc.ComplianceReports.Contracts.ServiceContracts.IComplianceReportsRetrieval>("ComplianceReportsRetrieval.svc")).Returns(mockComplianceRetrievalWcf.Object);
        }

        #region Constructor Tests

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new ParticipantComplianceService(null, null, null, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments1_ThrowsArgumentNullException()
        {
            new ParticipantComplianceService(mockClient.Object, null, null, null);
        }

         /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments2_ThrowsArgumentNullException()
        {
            new ParticipantComplianceService(null, mockMappingEngine.Object, null, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments3_ThrowsArgumentNullException()
        {
            new ParticipantComplianceService(null, null, mockCacheService.Object, null);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments5_ThrowsArgumentNullException( )
        {
            new ParticipantComplianceService( null, null, null, mockAdwService.Object);
        }

         /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithValidMockObjects_Return_True( )
        {
            var complianceser = new ParticipantComplianceService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, mockAdwService.Object);
            Assert.IsNotNull( complianceser);
            Assert.IsInstanceOfType( complianceser, typeof( Employment.Web.Mvc.Service.Implementation.ParticipantCompliance.ParticipantComplianceService));
        }

        #endregion Constructor Tests

        #region Search 

        /// <summary>
        /// Test get runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetSearch_Valid( )
        {
            var inModel = new ComplianceReportsListModel { DateFrom = new DateTime( 2013, 02, 02), DateTo = new DateTime( 2013, 04, 10), CtyContractType = "RJCP", RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<SEARCHLISTRequest>( inModel);            
            var response = new SEARCHLISTResponse 
                            {
                                 BreachSeqLast = 0,
                                 CtyContractTypeLast = "RJCP",
                                 Flag = "N",
                                 JobseekerIdLast = 0,
                                 RecommendingSiteCodeLast = "QM60",
                                 RecommendedDateLast = new DateTime( 2013, 03, 01),
                                 SearchListItems = getMockSearhItems( 10).ToArray( ),
                                 ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } 
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>( response);
            mockMappingEngine.Setup( m => m.Map<SEARCHLISTRequest>( inModel)).Returns( request);
            mockComplianceRetrievalWcf.Setup( m => m.Compliancesearch( request)).Returns( response);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).Search( inModel, false);
            mockMappingEngine.Verify( m => m.Map<SEARCHLISTRequest>( inModel), Times.Once( ));
            mockComplianceRetrievalWcf.Verify( m => m.Compliancesearch( request), Times.Once( ));
            mockMappingEngine.Verify( m => m.Map<IEnumerable<ComplianceReportsListItemModel>>( response.SearchListItems), Times.AtLeastOnce( ));
        }

         /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSearch_WcfThrowsFaultExceptionValidationFault_TMandatoryDateFromMissing( )
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { DateTo = new DateTime(2013, 04, 10), CtyContractType = "RJCP", RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<SEARCHLISTRequest>(inModel);
            var response = new SEARCHLISTResponse
                            {
                                BreachSeqLast = 0,
                                CtyContractTypeLast = "RJCP",
                                Flag = "N",
                                JobseekerIdLast = 0,
                                RecommendingSiteCodeLast = "QM60",
                                RecommendedDateLast = new DateTime(2013, 03, 01),
                                SearchListItems = getMockSearhItems(10).ToArray(),
                                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup( m => m.Map<SEARCHLISTRequest>( inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).Search( inModel, false);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSearch_WcfThrowsFaultExceptionValidationFault_TMandatoryDateToMissing( )
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { DateFrom = new DateTime(2013, 04, 10), CtyContractType = "RJCP", RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<SEARCHLISTRequest>(inModel);
            var response = new SEARCHLISTResponse
                            {
                                BreachSeqLast = 0,
                                CtyContractTypeLast = "RJCP",
                                Flag = "N",
                                JobseekerIdLast = 0,
                                RecommendingSiteCodeLast = "QM60",
                                RecommendedDateLast = new DateTime(2013, 03, 01),
                                SearchListItems = getMockSearhItems(10).ToArray(),
                                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup( m => m.Map<SEARCHLISTRequest>( inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).Search( inModel, false);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSearch_WcfThrowsFaultExceptionValidationFault_TMandatorySiteMissing( )
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { DateFrom = new DateTime( 2013, 04, 10), DateTo = new DateTime( 2013, 06, 02), CtyContractType = "RJCP" };
            var request = MappingEngine.Map<SEARCHLISTRequest>(inModel);
            var response = new SEARCHLISTResponse
                            {
                                BreachSeqLast = 0,
                                CtyContractTypeLast = "RJCP",
                                Flag = "N",
                                JobseekerIdLast = 0,
                                RecommendingSiteCodeLast = "QM60",
                                RecommendedDateLast = new DateTime(2013, 03, 01),
                                SearchListItems = getMockSearhItems(10).ToArray(),
                                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup( m => m.Map<SEARCHLISTRequest>( inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).Search( inModel, false);
        }
    
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSearch_WcfThrowsFaultExceptionValidationFault_TMandatoryCTYTypeMissing( )
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { DateFrom = new DateTime( 2013, 04, 10), DateTo = new DateTime( 2013, 06, 02), RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<SEARCHLISTRequest>(inModel);
            var response = new SEARCHLISTResponse
                            {
                                BreachSeqLast = 0,
                                CtyContractTypeLast = "RJCP",
                                Flag = "N",
                                JobseekerIdLast = 0,
                                RecommendingSiteCodeLast = "QM60",
                                RecommendedDateLast = new DateTime(2013, 03, 01),
                                SearchListItems = getMockSearhItems(10).ToArray(),
                                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup( m => m.Map<SEARCHLISTRequest>( inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).Search( inModel, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<SearchListItem> getMockSearhItems( int numbers)
        {
            var searchItems = new List<SearchListItem>( );
            for ( var counter = 0; counter < numbers; counter++)
            {
                searchItems.Add( new SearchListItem
                                    {
                                        AbsenceNoticeFlag = "Y",
                                        DisengagementFlag = "Y",
                                        JobseekerId = 9647456609 + counter
                                    });
            }

            return searchItems;
        }

        #endregion Search

        #region History 

        /// <summary>
        /// Test get runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetHistrory_Valid( )
        {
            var inModel = new ComplianceReportsListModel { JobseekerId = 106232809, CtyContractType = "RJCP", RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<HISTORYRequest>( inModel);            
            var response = new HISTORYResponse 
                            {
                                FirstGivenName = "First Name",
                                LastBreachSeq = 0,
                                LastRecommendedDate = DateTime.MinValue,
                                NextExists ="N",
                                SecondGivenName = "",
                                Surname ="Surname",
                                TitleOfJobseeker ="Title",
                                HistoryItems = getMockHistoryItems( 10).ToArray( ),
                                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } 
                            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>( response);
            mockMappingEngine.Setup( m => m.Map<HISTORYRequest>( inModel)).Returns( request);
            mockComplianceRetrievalWcf.Setup( m => m.Historylist( request)).Returns( response);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).History( inModel, false);
            mockMappingEngine.Verify( m => m.Map<HISTORYRequest>( inModel), Times.Once( ));
            mockComplianceRetrievalWcf.Verify( m => m.Historylist( request), Times.Once( ));
            mockMappingEngine.Verify( m => m.Map<IEnumerable<ComplianceReportsListItemModel>>( response.HistoryItems), Times.AtLeastOnce( ));
        }

         /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetHistrory_WcfThrowsFaultExceptionValidationFault_MandatoryJskidMissing( )
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { CtyContractType = "RJCP", RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<HISTORYRequest>(inModel);
            var response = new HISTORYResponse
            {
                FirstGivenName = "First Name",
                LastBreachSeq = 0,
                LastRecommendedDate = DateTime.MinValue,
                NextExists = "N",
                SecondGivenName = "",
                Surname = "Surname",
                TitleOfJobseeker = "Title",
                HistoryItems = getMockHistoryItems(10).ToArray(),
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup(m => m.Map<HISTORYRequest>(inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).History( inModel, false);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetHistrory_WcfThrowsFaultExceptionValidationFault_TMandatorySiteMissing()
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { JobseekerId = 106232809, CtyContractType = "RJCP" };
            var request = MappingEngine.Map<HISTORYRequest>(inModel);
            var response = new HISTORYResponse
            {
                FirstGivenName = "First Name",
                LastBreachSeq = 0,
                LastRecommendedDate = DateTime.MinValue,
                NextExists = "N",
                SecondGivenName = "",
                Surname = "Surname",
                TitleOfJobseeker = "Title",
                HistoryItems = getMockHistoryItems(10).ToArray(),
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup(m => m.Map<HISTORYRequest>(inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).History( inModel, false);
        }
    
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetHistrory_WcfThrowsFaultExceptionValidationFault_TMandatoryCTYTypeMissing()
        {
            var exception = new FaultException<ValidationFault>( new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var inModel = new ComplianceReportsListModel { JobseekerId = 106232809, RecommendingSiteCode = "QM60" };
            var request = MappingEngine.Map<HISTORYRequest>(inModel);
            var response = new HISTORYResponse
            {
                FirstGivenName = "First Name",
                LastBreachSeq = 0,
                LastRecommendedDate = DateTime.MinValue,
                NextExists = "N",
                SecondGivenName = "",
                Surname = "Surname",
                TitleOfJobseeker = "Title",
                HistoryItems = getMockHistoryItems(10).ToArray(),
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outModel = MappingEngine.Map<ComplianceReportsListModel>(response);
            mockMappingEngine.Setup(m => m.Map<HISTORYRequest>(inModel)).Throws( exception);
            mockMappingEngine.Setup ( m => m.Map<ComplianceReportsListModel>( response)).Returns( outModel);
            var result = SystemUnderTest( ).History( inModel, false);      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<HistoryItem> getMockHistoryItems( int numbers)
        {
            var historyItems = new List<HistoryItem>( );
            for ( var counter = 0; counter < numbers; counter++)
            {
                historyItems.Add( new HistoryItem
                                    {
                                        AbsenceNoticeFlag = "Y",
                                        DisengagementFlag = "Y",
                                        StatusCodeBreach = "D",
                                        BryBcyInitialBreachType ="AAR"
                                    });
            }

            return historyItems;
        }

        #endregion History
    }
}