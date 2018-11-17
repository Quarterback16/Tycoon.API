using System;

namespace ProgramAssuranceTool.Models
{
	public class IntegrityError
	{

		public string Column { get; set; }
		public string Table { get; set; }
		public int Key { get; set; }
		public string Message { get; set; }
		public string ErrorCategory { get; set; }
		public DateTime Detected { get; set; }

		public override string ToString()
		{
			return string.Format( "Key:{0} {1}", Key, Message );
		}
	}
}