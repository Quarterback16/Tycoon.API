using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.Exceptions
{
    
    
    /// <summary>
    ///This is a test class for TypeMismatchExceptionTest and is intended
    ///to contain all TypeMismatchExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TypeMismatchExceptionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        
        /// <summary>
        ///A test for TypeMismatchException Constructor
        ///</summary>
        [TestMethod()]
        public void TypeMismatchExceptionConstructorTest()
        {
            Type expectedType = typeof(string);
            Type actualType = typeof(int);
            var target = new TypeMismatchException(expectedType, actualType);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ActualType
        ///</summary>
        [TestMethod()]
        public void ActualTypeTest()
        {
            Type expectedType = typeof(string);
            Type actualType = typeof(int);
            var target = new TypeMismatchException(expectedType, actualType);
            target.ActualType = expectedType;
            Assert.AreEqual(expectedType, target.ActualType);
        }

        /// <summary>
        ///A test for ExpectedType
        ///</summary>
        [TestMethod()]
        public void ExpectedTypeTest()
        {
            Type expectedType = typeof(string);
            Type actualType = typeof(int);
            var target = new TypeMismatchException(expectedType, actualType); 

            target.ExpectedType = expectedType;
            Assert.AreEqual(expectedType, target.ExpectedType);
        }
    }
}
