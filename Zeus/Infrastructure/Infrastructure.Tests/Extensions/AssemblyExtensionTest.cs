using System.Linq;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for AssemblyExtensionTest and is intended
    ///to contain all AssemblyExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AssemblyExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ConcreteTypes
        ///</summary>
        [TestMethod()]
        public void ConcreteTypesTest()
        {
            object assemblyNull = null;
            IEnumerable<Type> cEmpty = ((Assembly)assemblyNull).ConcreteTypes();
            Assert.IsNotNull(cEmpty);
            Assert.IsFalse(cEmpty.Any());

            Assembly a = Assembly.GetAssembly(this.GetType());
            var c = a.ConcreteTypes();
            Assert.IsTrue(c.Any());
        }

       /// <summary>
        ///A test for GetConcreteTypesImplementing
        ///</summary>
        [TestMethod()]
        public void GetConcreteTypesImplementingTest()
        {
            Assembly assembly = Assembly.GetAssembly(this.GetType());
            IEnumerable<Type> actual = assembly.GetConcreteTypesImplementing<AssemblyExtensionTest>();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains(this.GetType()));
        }

        /// <summary>
        /// A test for GetPublicTypesImplementing
        /// </summary>
        [TestMethod()]
        public void GetPublicTypesImplementingTest()
        {
            Assembly assembly = Assembly.GetAssembly(this.GetType());
            IEnumerable<Type> actual = assembly.GetPublicTypesImplementing<AssemblyExtensionTest>();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains(this.GetType()));
        }


        /// <summary>
        ///A test for PublicTypes
        ///</summary>
        [TestMethod()]
        public void PublicTypesTest()
        {
            object assemblyNull = null;
            IEnumerable<Type> cEmpty = ((Assembly)assemblyNull).PublicTypes();
            Assert.IsNotNull(cEmpty);
            Assert.IsFalse(cEmpty.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var actual = assembly.PublicTypes();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains(this.GetType()));
        }

        [TestMethod]
        public void PublicTypesImplementing_fromAssemblies()
        {
            object assemblyNull = null;
            var n = ((Assembly)assemblyNull).GetPublicTypesImplementing<AssemblyExtensionTest>();
            Assert.IsNotNull(n);
            Assert.IsFalse(n.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var list = new List<Assembly> {assembly};
            var i = list.GetPublicTypesImplementing<AssemblyExtensionTest>();

            Assert.IsNotNull(i);
            Assert.IsTrue(i.Any());
            Assert.IsTrue(i.Contains(this.GetType()));
        }

        [TestMethod]
        public void ConcreteTypesImplementing_fromAssemblies()
        {
            var n = ((IEnumerable<Assembly>)null).ConcreteTypes();
            Assert.IsNotNull(n);
            Assert.IsFalse(n.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var list = new List<Assembly> { assembly };
            var i = list.ConcreteTypes();

            Assert.IsNotNull(i);
            Assert.IsTrue(i.Any());
            Assert.IsTrue(i.Contains(this.GetType()));
        }


        [TestMethod]
        public void GetConcreteTypesImplementing_fromAssemblies()
        {
            var n = ((IEnumerable<Assembly>)null).GetConcreteTypesImplementing<AssemblyExtensionTest>();
            Assert.IsNotNull(n);
            Assert.IsFalse(n.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var list = new List<Assembly> { assembly };
            var i = list.GetConcreteTypesImplementing<AssemblyExtensionTest>();

            Assert.IsNotNull(i);
            Assert.IsTrue(i.Any());
            Assert.IsTrue(i.Contains(this.GetType()));
        }

        [TestMethod]
        public void GetPublicTypesImplementing_fromAssemblies()
        {
            var n = ((IEnumerable<Assembly>)null).GetPublicTypesImplementing<AssemblyExtensionTest>();
            Assert.IsNotNull(n);
            Assert.IsFalse(n.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var list = new List<Assembly> { assembly };
            var i = list.GetPublicTypesImplementing<AssemblyExtensionTest>();

            Assert.IsNotNull(i);
            Assert.IsTrue(i.Any());
            Assert.IsTrue(i.Contains(this.GetType()));
        }

        [TestMethod]
        public void PublicTypes_fromAssemblies()
        {
            var n = ((IEnumerable<Assembly>)null).PublicTypes();
            Assert.IsNotNull(n);
            Assert.IsFalse(n.Any());

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            var list = new List<Assembly> { assembly };
            var i = list.PublicTypes();

            Assert.IsNotNull(i);
            Assert.IsTrue(i.Any());
            Assert.IsTrue(i.Contains(this.GetType()));
        }
    }
}