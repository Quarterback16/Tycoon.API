using System;

namespace Gerard.HostServer
{
	public class ExaminationEvent
	{
		public string ArticleText { get; set; }

		public DateTime? EventDate { get; set; }

		public bool IsSigning { get; set; }

		public bool IsWaiver { get; set; }

		public bool IsCut { get; set; }

		public bool IsInjury { get; set; }

		public bool IsRetirement { get; set; }

		public string Position { get; set; }

		public string PlayerName { get; set; }

		public string PlayerFirstName { get; set; }

		public string PlayerLastName { get; set; }

		public string PlayerId { get; set; }

		public string TeamName { get; set; }

		public string TeamId { get; set; }

		public string RecommendedAction { get; set; }

		public DateTime ExaminationDateTime { get; set; }

		public ExaminationEvent()
		{
			RecommendedAction = "IGNORE";
		}

		public override string ToString()
		{
			return $"Examination Result : {RecommendedAction} {ExaminationDateTime:u} {ArticleText}";
		}

		public void DumpEvent()
		{
			Console.WriteLine("ArticleText:  {0}", ArticleText);
			Console.WriteLine("Action:       {0}", RecommendedAction);
			Console.WriteLine("IsSigning:    {0}", IsSigning);
			Console.WriteLine("IsWaiver:     {0}", IsWaiver);
			Console.WriteLine("IsCut:        {0}", IsCut);
			Console.WriteLine("IsRetirement: {0}", IsRetirement);
			Console.WriteLine("PlayerId:     {0}", PlayerId);
			Console.WriteLine("TeamId:       {0}", TeamId);
			Console.WriteLine("PlayerName:   {0}", PlayerName);
			Console.WriteLine("TeamName:     {0}", TeamName);
			Console.WriteLine("Position:     {0}", Position);
		}
	}
}
