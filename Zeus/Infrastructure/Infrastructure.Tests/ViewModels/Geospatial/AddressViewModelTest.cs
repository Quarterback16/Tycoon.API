using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.ViewModels.Geospatial
{
    [TestClass]
    public class AddressViewModelTest
    {
        private const string Line1 = "14 Mort Street";
        private const string Locality = "Braddon";
        private const string State = "ACT";
        private const string Postcode = "2601";

        ///// <summary>
        /////A test for Validate
        /////</summary>
        //[TestMethod]
        //public void ValidateRequiredTest()
        //{
        //    var target = new AddressViewModel {Required = true};

        //    var validationContext = new ValidationContext(target, null, null);
        //    IEnumerable<ValidationResult> actual = target.Validate(validationContext);
        //    Assert.IsTrue(actual.Count() == 4);
        //}

        //[TestMethod]
        //public void ValidateAddressTest()
        //{
        //    var target = new AddressViewModel { Required = true, Line1 = Line1, Locality = Locality, State = State, Postcode = Postcode };

        //    var validationContext = new ValidationContext(target, null, null);
        //    IEnumerable<ValidationResult> actual = target.Validate(validationContext);
        //    Assert.IsTrue(actual.Count() == 0);
        //}

        [TestMethod]
        public void SingleLineAddressTest()
        {
            var target = new AddressViewModel { Line1 = Line1, Locality = Locality, State = State, Postcode = Postcode };
            var expected = String.Format("{0} {1} {2} {3}", Line1, Locality, State, Postcode);
            var actual = target.SingleLineAddress;
            Assert.AreEqual(expected, actual);
        }
    }
}
