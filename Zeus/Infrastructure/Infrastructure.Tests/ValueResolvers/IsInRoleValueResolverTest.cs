using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="IsInRoleValueResolver{T}" />.
    ///</summary>
    [TestClass]
    public class IsInRoleValueResolverTest
    {
        private IsInRoleValueResolver SystemUnderTest(IEnumerable<string> roles)
        {
            return new IsInRoleValueResolver(roles);
        }

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;

        // Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test null reference exception when Dependency Resolver does not have the IUserService registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void IsInRoleValueResolver_NoUserServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var sut = SystemUnderTest(new[] { "Role" });

            object source = null;
            bool destination = false;

            destination = sut.Resolve(source);
        }

        /// <summary>
        /// Test null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void IsInRoleValueResolver_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var sut = SystemUnderTest(new[] { "Role" });

            object source = null;
            bool destination = false;

            bool result = sut.Resolve(source);
        }

        /// <summary>
        /// Test the resolver returns true when user is in role.
        ///</summary>
        [TestMethod]
        public void IsInRoleValueResolver_ResolveWithUserInRole_ResolvesDestinationAsTrue()
        {
            // Return that user is in role
            mockUserService.Setup(m => m.IsInRole(It.IsAny<IEnumerable<string>>())).Returns(true);

            var sut = SystemUnderTest(new [] { "Role" });

            object source = null;
            bool destination = false;

            destination = sut.Resolve(source);


            Assert.IsTrue(destination);
        }

        /// <summary>
        /// Test the resolver returns false when user is not in role.
        ///</summary>
        [TestMethod]
        public void IsInRoleValueResolver_ResolveWithUserNotInRole_ResolvesDestinationAsFalse()
        {
            // Return that user is not in role
            mockUserService.Setup(m => m.IsInRole(It.IsAny<IEnumerable<string>>())).Returns(false);

            var sut = SystemUnderTest(new[] { "Role" });

            object source = null;
            bool destination = true;

            destination = sut.Resolve(source);

            Assert.IsFalse(destination);
        }
    }
}
