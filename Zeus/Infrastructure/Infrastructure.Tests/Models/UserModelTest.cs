using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Models
{
    /// <summary>
    ///This is a test class for UserModelTest and is intended
    ///to contain all UserModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

  /// <summary>
        ///A test for UserModel Constructor
        ///</summary>
        [TestMethod()]
        public void UserModelConstructorTest()
        {
            var target = new UserModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for FirstName
        ///</summary>
        [TestMethod()]
        public void FirstNameTest()
        {
            var target = new UserModel();
            const string expected = "FirstName";
            target.FirstName = expected;
            string actual = target.FirstName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest()
        {
            var target = new UserModel {FirstName = "Fred", LastName = "Flintstone"};
            string actual = target.FullName;
            Assert.IsFalse(string.IsNullOrEmpty(actual));
            Assert.AreEqual("Flintstone, Fred",actual);
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest1()
        {
            var target = new UserModel { FirstName = "Fred" };
            string actual = target.FullName;
            Assert.IsFalse(string.IsNullOrEmpty(actual));
            Assert.AreEqual("Fred", actual);
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest2()
        {
            var target = new UserModel { LastName = "Flintstone" };
            string actual = target.FullName;
            Assert.IsFalse(string.IsNullOrEmpty(actual));
            Assert.AreEqual("Flintstone", actual);
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest3()
        {
            var target = new UserModel();
            string actual = target.FullName;
            Assert.AreEqual(string.Empty, actual);
        }

        /// <summary>
        ///A test for LastName
        ///</summary>
        [TestMethod()]
        public void LastNameTest()
        {
            var target = new UserModel();
            const string expected = "Last";
            target.LastName = expected;
            string actual = target.LastName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrganisationCode
        ///</summary>
        [TestMethod()]
        public void OrganisationCodeTest()
        {
            var target = new UserModel();
            const string expected = "ORG";
            target.OrganisationCode = expected;
            string actual = target.OrganisationCode;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Role
        ///</summary>
        [TestMethod()]
        public void RoleTest()
        {
            var target = new UserModel();
            const string expected = "ROLE";
            target.Role = expected;
            string actual = target.Role;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SiteCode
        ///</summary>
        [TestMethod()]
        public void SiteCodeTest()
        {
            var target = new UserModel();
            const string expected = "SITE";
            target.SiteCode = expected;
            string actual = target.SiteCode;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UserID
        ///</summary>
        [TestMethod()]
        public void UserIDTest()
        {
            var target = new UserModel();
            const string expected = "IH0093";
            target.UserID = expected;
            string actual = target.UserID;
            Assert.AreEqual(expected, actual);
        }
    }
}
