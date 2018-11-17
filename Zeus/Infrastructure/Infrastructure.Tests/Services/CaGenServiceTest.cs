using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Services;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Claim = System.Security.Claims.Claim;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{

    /// <summary>
    /// Test the CaGenService class.
    /// </summary>
    [TestClass]
    public class CaGenServiceTest
    {
        // copy communication assembly in
        // add config settings for cics to run
        // execute a basic jnmadmin wrapper? maybe not every time
        private List<System.Security.Claims.Claim> GetClaims(ClaimsIdentity subject)
        {
            var c = new List<System.Security.Claims.Claim>();
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/orgcontract", "AAA"));
            c.Add(new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "XX999999"));
            c.Add(new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "First"));
            c.Add(new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "Last"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/org", "Org"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/baserole", "BBB"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "CCC"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/reportingrole", "DDD"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/defaultsite", "SITE"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/usertype", "UserType_DEWR"));
            c.Add(new System.Security.Claims.Claim("http://deewr.gov.au/es/2011/03/claims/selfregistered", "false"));
            c.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthenticationMethod, "false"));
            return c;
        }
        private ClaimsIdentity SystemUnderTest()
        {
            var identity = new Mock<ClaimsIdentity>();
            var c = GetClaims(identity.Object);

            identity.Setup(i => i.Claims).Returns(c);
            identity.Setup(i => i.Name).Returns("xx0000");

            return identity.Object;
        }
        [ExpectedException(typeof(SerializationException))]
        [TestMethod]
        public void Execute()
        {
            var identity = SystemUnderTest();
            Thread.CurrentPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>(){identity} );
            CaGenService service = new CaGenService();
            service.Execute<TestContract, TestContractResponse>(new TestContract(), "1234", "test");
        }
    }

    [DataContract]
    public class TestContract
    {
        [DataMember]
        public string Test { get; set; }
    }
    [DataContract]
    public class TestContractResponse : IResponseWithExecutionResult
    {
        [DataMember]
        public string Test { get; set; }

        [DataMember]
        public ExecutionResult ExecutionResult { get; set; }
    }
}
