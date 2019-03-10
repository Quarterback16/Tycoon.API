using System;

namespace Gerard.HostServer
{
	public class PlayerSigningStrategy : IProcessTransaction
	{
		private readonly ITflService TflService;

		public PlayerSigningStrategy(
			ITflService tflService)
		{
			TflService = tflService;
		}

		public bool Process(
			ExaminationEvent transactionEvent,
			ILog logger)
		{
			var p = TflService.GetNflPlayer(transactionEvent.PlayerId);
			var eventDate = transactionEvent.EventDate
							 ?? transactionEvent.ExaminationDateTime;

			if (TflService.IsSameDay(p, eventDate))
			{
				logger.Info($@"Signing for {
					p.PlayerName
					} ignored: Same day contract rule");
				return false;
			}

			if (!string.IsNullOrEmpty(p.CurrTeam.TeamCode))
			{
				//  end his previous team contract
				TflService.EndContract(
					p,
					when: eventDate,
					isRetirement: false);
			}

			// start new contract
			return transactionEvent.EventDate != null
				&& TflService.RecordSigning(
					p,
					teamCode: transactionEvent.TeamId,
					when: (DateTime)transactionEvent.EventDate,
					how: "FA");
		}

	}
}