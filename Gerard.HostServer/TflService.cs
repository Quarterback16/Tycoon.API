using NLog;
using System;
using System.Linq;
using System.Data;

namespace Gerard.HostServer
{
	public class TflService : ITflService
	{
		private readonly ITflDataLibrarian TflDataLibrarian;

		public Logger Logger { get; set; }

		public TflService(
			ITflDataLibrarian librarian,
			Logger logger)
		{
			Logger = logger;
			TflDataLibrarian = librarian;
		}

		/// <summary>
		///   Find a single player
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>s
		/// <returns></returns>
		public NFLPlayer GetNflPlayer(
			string firstName,
			string lastName)
		{
			//Logger.Trace($"GetNflPlayer calling DataLibrarian {firstName} {lastName}");
			var ds = TflDataLibrarian.GetPlayer(firstName, lastName);
			var list = (from DataRow dr in ds.Tables[0].Rows
						select new NFLPlayer(dr, string.Empty)).ToList();
			//Logger.Trace($"GetNflPlayer found {list.Count} records");
			return list.Count > 0 ? list[0] : null;
		}

		public NFLPlayer GetNflPlayer(
			string playerId)
		{
			var ds = TflDataLibrarian.GetPlayer(playerId);
			var list = (from DataRow dr in ds.Tables[0].Rows
						select new NFLPlayer(dr, fantasyLeague: string.Empty))
						.ToList();
			var plyr = list.Count > 0 ? list[0] : null;
			if (plyr == null)
				Logger.Info($"Could not find a player for ID: {playerId}");
			else
				Logger.Trace($"Found {plyr.PlayerName} for ID: {playerId}");

			return plyr;
		}

		public bool RecordSigning(
			NFLPlayer p,
			string teamCode,
			DateTime when,
			string how)
		{
			Logger.Trace($"RecordSigning: {p.PlayerName} signs with {teamCode}");
			try
			{
				TflDataLibrarian.Sign(
					p.PlayerCode,
					teamCode,
					when,
					how);
				TflDataLibrarian.SetCurrentTeam(
					p.PlayerCode,
					teamCode);
				Logger.Info($"{p.PlayerName} now playing for {teamCode}");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error in End Contract");
				return false;
			}
			return true;
		}

		public bool EndContract(
			NFLPlayer p,
			DateTime when,
			bool isRetirement)
		{
			Logger.Trace($@"EndContract on {
				p.PlayerName
				} when: {
				when
				} retirement: {
				isRetirement
				}");
			try
			{
				if (p.TeamCode.Equals("??"))
				{
					Logger.Info($"{p.PlayerName} already ended service");
					return false;
				}
				else
				{
					if (isRetirement)
					{
						TflDataLibrarian.RetirePlayer(
							when, 
							p.PlayerCode);
						Logger.Info($"{p.PlayerName} retired");
					}
					else
					{
						TflDataLibrarian.CloseServitude(
							when,
							p.PlayerCode);
						Logger.Info($"{p.PlayerName} contract with {p.TeamCode} ended");
					}
					TflDataLibrarian.SetCurrentTeam(p.PlayerCode, "??");
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"Error in End Contract {ex.Message}");
				return false;
			}
			return true;
		}

		public bool InjurePlayer(
			NFLPlayer p)
		{
			Logger.Trace($"InjurePlayer: {p.PlayerName}");
			try
			{
				var oldRole = p.PlayerRole;
				if (oldRole == Constants.K_ROLE_INJURED)
					Logger.Info($"{p.PlayerName} already marked as injured");
				else
				{
					TflDataLibrarian.SetRole(
						p.PlayerCode,
						Constants.K_ROLE_INJURED);
					Logger.Info($"{p.PlayerName} marked as injured");
				}
				//TODO:  Possible could remove W2 etc from POSDESC too
			}
			catch (Exception ex)
			{
				Logger.Error(ex,"Error in Injure player");
				return false;
			}
			return true;
		}

		public bool IsSameDay(
			NFLPlayer p, 
			DateTime when)
		{
			var lastContract = TflDataLibrarian.LastContract(p.PlayerCode);
			return when.Date == lastContract.Date;
		}

		public bool UpdateDob(NFLPlayer p, DateTime dob)
		{
			TflDataLibrarian.SetDob(
				playerId: p.PlayerCode,
				dob: dob);
			return true;
		}
	}
}
