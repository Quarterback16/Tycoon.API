using Employment.Web.Mvc.Service.Interfaces.Referrals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Referrals
{
    /// <summary>
    ///This is a test class for ReferralHistoryItemTypeExtensionTest and is intended
    ///to contain all ReferralHistoryItemTypeExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReferralHistoryItemTypeExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for ConvertFlag
        ///</summary>
        [TestMethod()]
        public void ConvertFlagTest()
        {
            var actual = ReferralHistoryItemTypeExtension.ConvertFlag("A");
            Assert.AreEqual(ReferralHistoryItemType.ApprovedActivities, actual);
            
            actual = ReferralHistoryItemTypeExtension.ConvertFlag("E");
            Assert.AreEqual(ReferralHistoryItemType.Exemptions, actual);

            actual = ReferralHistoryItemTypeExtension.ConvertFlag(string.Empty);
            Assert.AreEqual(ReferralHistoryItemType.All, actual);
        }

        /// <summary>
        ///A test for GetFlag
        ///</summary>
        [TestMethod()]
        public void GetFlagTest()
        {
            const ReferralHistoryItemType referralHistorySearchType = new ReferralHistoryItemType();
            string actual = referralHistorySearchType.GetFlag();
            Assert.AreEqual(string.Empty, actual);

            actual = ReferralHistoryItemType.All.GetFlag();
            Assert.AreEqual(string.Empty, actual);

            actual = ReferralHistoryItemType.ApprovedActivities.GetFlag();
            Assert.AreEqual("A", actual);

            actual = ReferralHistoryItemType.Exemptions.GetFlag();
            Assert.AreEqual("E", actual);
        }
    }
}