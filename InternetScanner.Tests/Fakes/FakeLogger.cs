using System;

namespace InternetScanner.Tests.Fakes
{
	public class FakeLogger : ILog
	{
		public void Debug(string message)
		{
			Console.WriteLine(message);
		}

		public void Error(string message)
		{
			Console.WriteLine(message);
		}

		public void Info(string message)
		{
			Console.WriteLine(message);
		}

		public void Trace(string message)
		{
			Console.WriteLine(message);
		}

		public void Warning(string message)
		{
			Console.WriteLine(message);
		}
	}
}
