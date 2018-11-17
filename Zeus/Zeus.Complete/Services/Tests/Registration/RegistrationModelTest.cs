using System;
using System.Text;
using AutoMapper;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    /// <summary>
    /// Privides tests for the registration mapper service
    /// </summary>
    [TestClass]
    public class RegistrationMapperTest
    {
        private RegistrationMapper SystemUnderTest()
        {
            return new RegistrationMapper();
        }

        /// <summary>
        /// Test map is valid.
        /// </summary>
        [TestMethod]
        public void Map_Valid()
        {
            SystemUnderTest().Map(Mapper.Configuration);
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void DateTimeToNullableDateTimeMapsToNull()
        {
            var sut = new DateTimeToNullableDateTimeValueResolver();

            var source = DateTime.MinValue;
            DateTime? destination = DateTime.MinValue;

            var typeMap = new TypeMap(new TypeInfo(destination.GetType()), new TypeInfo(destination.GetType()), new MemberList());
            var resolutionContext = new ResolutionContext(typeMap, source, destination.GetType(), destination.GetType(), new MappingOperationOptions());

            var result = sut.Resolve(new ResolutionResult(resolutionContext));
            Assert.IsNull(result.Value);

        }

        [TestMethod]
        public void DateTimeToNullableDateTimeMapsToSourceValue()
        {
            var sut = new DateTimeToNullableDateTimeValueResolver();

            var source = DateTime.Now;
            DateTime? destination = DateTime.MinValue;

            var typeMap = new TypeMap(new TypeInfo(destination.GetType()), new TypeInfo(destination.GetType()), new MemberList());
            var resolutionContext = new ResolutionContext(typeMap, source, destination.GetType(), destination.GetType(), new MappingOperationOptions());
            var result = sut.Resolve(new ResolutionResult(resolutionContext));

            Assert.IsTrue(((DateTime?)result.Value).Value.Equals(source));

        }
    }
}
