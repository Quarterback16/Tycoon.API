namespace Gerard.HostServer
{
	public class PlayerRetirementStrategy : IProcessTransaction
	{
		private readonly ITflService TflService;

		public PlayerRetirementStrategy(ITflService tflService)
		{
			TflService = tflService;
		}

		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			logger.Trace("Processing Retirement");

			var p = TflService.GetNflPlayer(
				transactionEvent.PlayerId);

			return TflService.EndContract(
				p,
				transactionEvent.EventDate ?? transactionEvent.ExaminationDateTime,
				isRetirement: true);
		}
	}
}