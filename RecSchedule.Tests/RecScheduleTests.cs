using RecSchedule.Domain;
using Xunit;

namespace RecSchedule.Tests
{
	public class RecScheduleTests
	{
		[Fact]
		public void DefaultRecActivityDescription_IsFree()
		{
			var cut = new RecActivity();
			Assert.True(cut.Description == "free");
		}

		[Fact]
		public void DefaultRecSessionType_IsCasual()
		{
			var cut = new RecSession();
			Assert.True(cut.SessionType == SessionType.Casual);
		}

	}
}
