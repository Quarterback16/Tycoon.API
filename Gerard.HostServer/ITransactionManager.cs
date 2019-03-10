namespace Gerard.HostServer
{
	public interface ITransactionManager
	{
		bool ProcessEvent(ExaminationEvent transactionEvent);
	}
}