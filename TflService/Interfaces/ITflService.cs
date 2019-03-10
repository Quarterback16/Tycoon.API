using RosterLib;
using System;

namespace Interfaces
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

		bool SetDob(
			NFLPlayer p,
			DateTime dob);

		bool SetHeight(
			NFLPlayer p,
			int feet,
			int inches);

		bool SetWeight(
			NFLPlayer p,
			int pounds);
	}
}
