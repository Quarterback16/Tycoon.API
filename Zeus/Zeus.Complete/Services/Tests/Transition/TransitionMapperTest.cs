using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Transition;
using Employment.Web.Mvc.Service.Interfaces.Transition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Transition
{
    /// <summary>
    /// Unit tests for <see cref="TransitionMapper" />.
    /// </summary>
    [TestClass]
    public class TransitionMapperTest
    {
        private TransitionMapper SystemUnderTest()
        {
            return new TransitionMapper();
        }

        /// <summary>
        /// Test map is valid.
        /// </summary>
        [TestMethod]
        public void TransitionMapperValid()
        {
            SystemUnderTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}

