using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrelloApi;

namespace TrelloApi.Tests
{
   [TestClass]
   public class ControllerTests
   {
      [TestMethod]
      public void TestMethod1()
      {
         var mockRepository = new Mock<IBoardRepository>();
         TrelloController controller = new TrelloController();

      }
   }
}
