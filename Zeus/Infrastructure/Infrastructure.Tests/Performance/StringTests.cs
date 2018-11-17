using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Claim = System.Security.Claims.Claim;

namespace Employment.Web.Mvc.Infrastructure.Tests.Performance {

    //These tests require you to turn on the 'Optimize code' flag on the project build settings.

    /// <summary>
    /// Test performance of common string operations.
    /// </summary>
    [TestClass]
    public class StringTests 
    {
        /// <summary>
        /// Test chunk of html assembling code.
        /// </summary>
        [TestMethod]
        public void StringBuilderTest() {
            string rowLinks = "";
            List<LinkAttribute> links = new List<LinkAttribute>();
            for (int i = 20; i > 0; i--) {
                links.Add(new LinkAttribute("test" + i) { Action = "test", Order = i });
            }

            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 20000; i++) {
                StringBuilder sb = new StringBuilder();
                foreach (LinkAttribute link in links.OrderBy(b => b.Order)) {
                    sb.Append("<span class=\"button\">" + link.Name + "</span>");
                }
                rowLinks = "<div class=\"ffFix\">" + sb + "</div>";
            }
            stopwatch.Stop();

            //optimised code
            rowLinks = "";
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 20000; i++) {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"ffFix\">");
                foreach (LinkAttribute link in links.OrderBy(b => b.Order)) {
                    sb.Append("<span class=\"button\">");
                    sb.Append(link.Name);
                    sb.Append("</span>");
                }
                sb.Append("</div>");
                rowLinks = sb.ToString();
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed not optimised:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }


        /// <summary>
        /// Test StringComparison.Ordinal vs culture sensitive one.
        /// </summary>
        [TestMethod]
        public void StringCompareOrdinalEqualsTest()
        {
            var test1 = "y";
            var test2 = "Y";
            var test3 = "n";
            var test4 = "pants";
            var test5 = "";
            string test6 = null;

            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 200000; i++)
            {
                bool test = string.Compare(test1, "Y", true, CultureInfo.InvariantCulture) == 0;
                 test = string.Compare(test2, "Y", true, CultureInfo.InvariantCulture) == 0;
                 test = string.Compare(test3, "Y", true, CultureInfo.InvariantCulture) == 0;
                 //test = string.Compare(test4, "Y", true, CultureInfo.InvariantCulture) == 0;
                 test = string.Compare(test5, "Y", true, CultureInfo.InvariantCulture) == 0;
                 test = string.Compare(test6, "Y", true, CultureInfo.InvariantCulture) == 0;
            }
            stopwatch.Stop();

            //optimised code
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 200000; i++)
            {
                bool test = string.Compare(test1, "Y", StringComparison.OrdinalIgnoreCase) == 0;
                test = string.Compare(test2, "Y", StringComparison.OrdinalIgnoreCase) == 0;
                test = string.Compare(test3, "Y", StringComparison.OrdinalIgnoreCase) == 0;
                test = string.Compare(test4, "Y", StringComparison.OrdinalIgnoreCase) == 0;
                test = string.Compare(test5, "Y", StringComparison.OrdinalIgnoreCase) == 0;
                test = string.Compare(test6, "Y", StringComparison.OrdinalIgnoreCase) == 0;
            }
            stopwatch2.Stop();

            //optimised code
            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            for (int i = 0; i < 200000; i++)
            {
                bool test = string.Equals(test1, "Y", StringComparison.OrdinalIgnoreCase) ;
                test = string.Equals(test2, "Y", StringComparison.OrdinalIgnoreCase);
                test = string.Equals(test3, "Y", StringComparison.OrdinalIgnoreCase);
                test = string.Equals(test4, "Y", StringComparison.OrdinalIgnoreCase);
                test = string.Equals(test5, "Y", StringComparison.OrdinalIgnoreCase);
                test = string.Equals(test6, "Y", StringComparison.OrdinalIgnoreCase);
            }
            stopwatch3.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised2:{0}", stopwatch3.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }





        /// <summary>
        /// Test StringComparison.Ordinal vs culture sensitive one.
        /// </summary>
        [TestMethod]
        public void StringCompareOrdinalTest() {
            string s1 = "this is a test";
            string s2 = "pants";

            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 200000; i++) {
                bool test = s1.StartsWith(s2);
            }
            stopwatch.Stop();

            //optimised code
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 200000; i++) {
                bool test = s1.StartsWith(s2, StringComparison.Ordinal);
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }



        /// <summary>
        /// Test string.ToLower/string.Contains vs IndexOf 
        /// </summary>
        [TestMethod]
        public void StringContainsIndexOf() {
            string test = "this is a Test";

            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++) {
                var x = test.ToLower().Contains("test");
                //Assert.IsTrue(x);
            }
            stopwatch.Stop();

            //optimised code
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 100000; i++) {
                var x = test.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0;
                //Assert.IsTrue(x);
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }




        /// <summary>
        /// Test string.Contains vs IndexOf 
        /// </summary>
        [TestMethod]
        public void IndexOfTest() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string s1 = "<span>this is a test of inner html stripping</span>";
            string s2 = "<span>another test. of stuff</span>";
            for (int i = 0; i < 100000; i++) {
                bool x = s1.Contains(".");
                bool y = s2.Contains(".");
            }
            stopwatch.Stop();

            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 100000; i++) {
                bool x = s1.IndexOf('.') >= 0;
                bool y = s2.IndexOf('.') >= 0;
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed modified:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }




        /// <summary>
        /// Test string.Contains vs IndexOf 
        /// </summary>
        [TestMethod]
        public void OrdinalCurrentCultureTest() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string s1 = "<span>this is a test of inner html stripping</span>";
            string s2 = "<span>another test. of stuff</span>";
            for (int i = 0; i < 100000; i++) {
                bool x = s1.IndexOf("test", StringComparison.CurrentCulture) >= 0;
                bool y = s2.IndexOf(".", StringComparison.CurrentCulture) >= 0;
            }
            stopwatch.Stop();

            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 100000; i++) {
                bool x = s1.IndexOf("test", StringComparison.Ordinal) >= 0;
                bool y = s2.IndexOf(".", StringComparison.Ordinal) >= 0;
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed modified:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }


        private static string ToQueryString(IDictionary<string, object> dictionary, bool lowerCaseKey)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return null;
            }

            string query = string.Empty;

            dictionary.ForEach(d =>
            {
                query += string.Format("{0}={1}&", lowerCaseKey ? d.Key.ToLower() : d.Key, d.Value);
            });

            return query.TrimEnd('&');
        }
        //modified version
        private static string ToQueryString2(IDictionary<string, object> dictionary, bool lowerCaseKey)
        {
            if (dictionary == null || dictionary.Count<=0)
            {
                return null;
            }
            var query = new StringBuilder();
            if (lowerCaseKey)
            {
                dictionary.ForEach(d => query.AppendFormat("{0}={1}&",  d.Key.ToLower(), d.Value));
            }
            else
            {
                dictionary.ForEach(d => query.AppendFormat("{0}={1}&", d.Key, d.Value));
            }
            if (query.Length > 0 && query[query.Length - 1] == '&')
            {
                query.Remove(query.Length - 1, 1);
            }

            return query.ToString();
        }


        /// <summary>
        /// Test String.Format vs +.
        /// </summary>
        [TestMethod]
        public void ToQueryStringTest()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
           dictionary.Add("test1","test2");
            dictionary.Add("TEST3", "Test4");
            dictionary.Add("Test5", "test6");

            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 200000; i++)
            {
                var test = ToQueryString(dictionary,  false);
                var test2 = ToQueryString(dictionary, true);
            }
            stopwatch.Stop();

            //optimised code
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 200000; i++)
            {
                var test = ToQueryString2(dictionary, false);
                var test2 = ToQueryString2(dictionary, true);
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }


        private static bool IsInRole(ClaimsIdentity identity, IEnumerable<string> roles)
        {
            return roles != null && roles.Intersect(identity.GetRoles()).Any();
        }
        private static bool IsInRole2(ClaimsIdentity identity, IEnumerable<string> roles)
        {
            if (roles == null || roles.Count()==0)
                return false;

            var rolesHash = identity.GetRolesHash();
            foreach (var role in roles)
            {
                if (rolesHash.Contains(role)) 
                    return true;
            }
            return false;
        }
        private List<Claim> GetClaims(ClaimsIdentity subject)
        {
            var c = new List<Claim>();
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/orgcontract", "AAA"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "XX999999"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "First"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "Last"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/org", "Org"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/baserole", "BBB"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "CCC"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "SPC"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "SPN"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "AJS"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "DIA"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "DIV"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/reportingrole", "DDD"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/defaultsite", "SITE"));

            return c;
        }
        private ClaimsIdentity SystemUnderTest()
        {
            var identity = new Mock<ClaimsIdentity>();
            var subject = new Mock<ClaimsIdentity>();
            var c = GetClaims(subject.Object);

            identity.Setup(i => i.Claims).Returns(c);

            return identity.Object;
        }

        /// <summary>
        /// Test IsInRole speed.
        /// </summary>
        [TestMethod]
        public void IsInRoleTest()
        {
            var roles = new List<string>();
            roles.Add("JPO");
            roles.Add("SPN");
            roles.Add("SPC");
            roles.Add("AJS");
            roles.Add("GGG");
            roles.Add("HHG");
            roles.Add("DDG");
            roles.Add("EER");
            roles.Add("CCV");

            var roles2 = new List<string>();
            roles2.Add("JPO");
            roles2.Add("SPT");

            var roles3 = new List<string>();

            var claims = SystemUnderTest();

            Assert.IsTrue(IsInRole(claims, roles));
            Assert.IsTrue(IsInRole2(claims, roles));
            Assert.IsFalse(IsInRole(claims, roles2));
            Assert.IsFalse(IsInRole2(claims, roles2));
            Assert.IsFalse(IsInRole(claims, roles3));
            Assert.IsFalse(IsInRole2(claims, roles3));

            
            //original code
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 20000; i++)
            {
                var test = IsInRole(claims, roles);
                var test2 = IsInRole(claims, roles2);
                var test3 = IsInRole(claims, roles3);
            }
            stopwatch.Stop();

            //optimised code
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 20000; i++)
            {
                var test = IsInRole2(claims, roles);
                var test2 = IsInRole2(claims, roles2);
                var test3 = IsInRole2(claims, roles3);
            }
            stopwatch2.Stop();

            Trace.WriteLine(string.Format("Elapsed original:{0}", stopwatch.ElapsedMilliseconds));
            Trace.WriteLine(string.Format("Elapsed optimised:{0}", stopwatch2.ElapsedMilliseconds));

            Assert.IsTrue(stopwatch2.ElapsedMilliseconds < stopwatch.ElapsedMilliseconds);
        }


    }
}
