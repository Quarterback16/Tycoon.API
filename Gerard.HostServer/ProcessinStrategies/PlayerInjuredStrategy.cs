namespace Gerard.HostServer
{
	public class PlayerInjuredStrategy : IProcessTransaction
	{
		private readonly ITflService TflService;

		public PlayerInjuredStrategy(ITflService tflService)
		{
			TflService = tflService;
		}

		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			var p = TflService.GetNflPlayer(transactionEvent.PlayerId);

			return TflService.InjurePlayer(p);
		}
	}
}