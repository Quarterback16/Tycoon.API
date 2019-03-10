using System;

namespace Gerard.HostServer
{
	public interface ITflService
	{
		INFLPlayer GetNflPlayer(string firstName, string lastName);

		INFLPlayer GetNflPlayer(string playerId);

		bool RecordSigning(INFLPlayer p, string teamCode, DateTime when, string how);

		bool EndContract(INFLPlayer p, DateTime when, bool isRetirement);

		bool InjurePlayer(INFLPlayer p);

		bool IsSameDay(INFLPlayer p, DateTime when);
	}
}
