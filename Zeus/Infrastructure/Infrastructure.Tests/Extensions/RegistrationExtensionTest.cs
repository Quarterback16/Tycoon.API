using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="RegistrationExtension" />.
    /// </summary>
    [TestClass]
    public class RegistrationExtensionTest
    {
        /// <summary>
        /// Test register runs successfully.
        /// </summary>
        [TestMethod]
        public void Register_Valid()
        {
            var registration = new Mock<IRegistration>();
            registration.Setup(o => o.Register());
            registration.Object.Register();
            registration.Verify(o => o.Register());
        }

        [TestMethod]
        public void Registrater_Order()
        {
            var registration = new Mock<IRegistration>();
            registration.Setup(o => o.Register());
            registration.Object.Register();

            var order = registration.Object.Order();
            Assert.IsNotNull(order);
            Assert.AreEqual(int.MaxValue,order);
        }

        [TestMethod]
        public void Registrater_Group()
        {
            var registration = new Mock<IRegistration>();
            registration.Setup(o => o.Register());
            registration.Object.Register();

            var group = registration.Object.Group();
            Assert.IsNotNull(group);
            Assert.AreEqual(1, group);
        }
    }
}