using System;

namespace Gerard.HostServer
{
	public interface ITflService
	{
		NFLPlayer GetNflPlayer(
			string firstName,
			string lastName);

		NFLPlayer GetNflPlayer(
			string playerId);

		bool RecordSigning(
			NFLPlayer p, 
			string teamCode, 
			DateTime when, 
			string how);

		bool EndContract(
			NFLPlayer p, 
			DateTime when, 
			bool isRetirement);

		bool InjurePlayer(
			NFLPlayer p);

		bool IsSameDay(
			NFLPlayer p, 
			DateTime when);
	}
}
