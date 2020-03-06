using System;

namespace RecSchedule.Domain
{
	public class RecSession
	{
		public DateTime SessionDate { get; set; }
		public string StartTime { get; set; }
		public SessionType SessionType { get; set; }
		public bool IsDouble { get; set; }
		public RecActivity Activity { get; set; }
	}
}
