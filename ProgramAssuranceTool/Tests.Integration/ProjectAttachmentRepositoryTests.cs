using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
   public class ProjectAttachmentRepositoryTests
   {
		[TestMethod]
		public void TestAttachmentStorage()
		{
			var sut = new PatService();
			const int attachmentId = 113;
			var fileData = System.IO.File.ReadAllBytes( "c:\\UserData\\Temp\\TommyFFL.clf" );
			sut.StoreAttachment( fileData, attachmentId );

		}

		[TestMethod]
		public void TestAttachmentRetrieval()
		{
			var sut = new PatService();
			byte[] fileData = sut.RetrieveAttachment( 113 );
			System.IO.File.WriteAllBytes( "c:\\UserData\\Temp\\TommyHawks.clf", fileData );
		}
   }
}
