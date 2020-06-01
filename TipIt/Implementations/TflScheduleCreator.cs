using System;
using System.Data.OleDb;
using TipIt.Interfaces;
using TipIt.Models;

namespace TipIt.Implementations
{
	public class TflScheduleCreator : IGameProcessor
	{
		public OleDbConnection OleDbConn;

		public TflScheduleCreator()
		{
			//  set up minimal TFL Data Librarian functionality
			OleDbConn = new OleDbConnection(
				"Provider=VFPOLEDB.1;Data Source=e:\\tfl\\nfl\\team.dbf");
		}
		public void ProcessGame(
			Game g)
		{
			var season = Season(g.GameDate);
			var week = TflWeek(g.Round);
			var gameDate = g.GameDate;
			var gameHour = GameHour(g.GameDate);
			var awayTeamCode = g.AwayTeam;
			var homeTeamCode = g.HomeTeam;

			InsertGame(
				season,
				week,
				gameDate,
				gameHour,
				awayTeamCode,
				homeTeamCode);
		}

		public string TflWeek(
			int round)
		{
			return $"{round:0#}";
		}

		public string Season(
			DateTime gameDate)
		{
			//TODO
			return "2020";
		}

		public string GameHour(
			DateTime gameDate)
		{
			//TODO
			return "1";
		}

		private void InsertGame(
			string season, 
			string week, 
			DateTime gameDate, 
			string gameHour, 
			string awayTeamCode,
			string homeTeamCode )
		{
			var formatStr =
			   "INSERT INTO SCHED (SEASON, WEEK, GAMENO, GAMEDATE, GAMEHOUR, AWAYTEAM, HOMETEAM)";

			formatStr += "VALUES( '{0}','{1}','{2}','{3}',{{{4:MM/dd/yyyy}}},{5},{6}  )";
			var commandStr = string.Format(
				formatStr, 
				season, 
				week, 
				gameDate,
				gameHour,
				awayTeamCode, 
				homeTeamCode );

			ExecuteNflCommand(
				commandStr);
		}

		private void ExecuteNflCommand(
			string commandStr)
		{
			OleDbConn.Close();
			OleDbConn.Open();
			var cmd = new OleDbCommand(
				commandStr, 
				OleDbConn);
			cmd.ExecuteNonQuery();
			OleDbConn.Close();
		}
	}
}
