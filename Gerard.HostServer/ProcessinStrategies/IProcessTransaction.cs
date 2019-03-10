namespace Gerard.HostServer
{
	public interface IProcessTransaction
	{
		bool Process(
			ExaminationEvent examinationEvent,
			ILog logger);
	}
}