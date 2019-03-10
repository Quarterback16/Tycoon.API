namespace Gerard.HostServer
{
	public interface ILog
	{
		void Info(string message);
		void Trace(string message);
		void Debug(string message);
		void Error(string message);
		void Warning(string message);
	}
}
