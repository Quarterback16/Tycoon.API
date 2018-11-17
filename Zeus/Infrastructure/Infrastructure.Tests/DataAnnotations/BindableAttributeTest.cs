using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableAttributeTest
    {
        private IEnumerable<string> roles = new[] {"Role1", "Role2"}; 

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();

            mockUserService.Setup(m => m.Roles).Returns(roles);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver does not have the IUserService registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BindableSelection_NoUserServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var attribute = new BindableAttribute("Role1");

            attribute.IsBindable();
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BindableSelection_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var attribute = new BindableAttribute("Role1");

            attribute.IsBindable();
        }

        /// <summary>
        /// Test property validates if bindable with no roles.
        /// </summary>
        [TestMethod]
        public void BindableSelection_IsBindableWithNoRoles_Validates()
        {
            var attribute = new BindableAttribute();

            Assert.IsTrue(attribute.IsBindable());
        }

        /// <summary>
        /// Test property validates if bindable with roles that user has.
        /// </summary>
        [TestMethod]
        public void BindableSelection_IsBindableWithRoleUserHas_Validates()
        {
            var attribute = new BindableAttribute("Role1");

            Assert.IsTrue(attribute.IsBindable());
        }

        /// <summary>
        /// Test property validates if bindable with roles that user has.
        /// </summary>
        [TestMethod]
        public void BindableSelection_IsBindableWithEmptyRole_Validates()
        {
            var attribute = new BindableAttribute(new string[] {});

            Assert.IsTrue(attribute.IsBindable());
        }

        /// <summary>
        /// Test property fails if bindable with roles that user does not have.
        /// </summary>
        [TestMethod]
        public void BindableSelection_IsNotBindableWithRoleUserDoesNotHave_Fails()
        {
            var attribute = new BindableAttribute("RoleUserDoesNotHave");

            Assert.IsFalse(attribute.IsBindable());
        }
    }
}
