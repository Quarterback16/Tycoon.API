using System;
using ProgramAssuranceTool.ViewModels.Claims;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IClaimsRepository
	{
		/// <summary>
		///    Gets a bunch of random claims based on the input parameters.
		/// </summary>
		/// <param name="claimType">Type of the claim.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="siteCode">The site code.</param>
		/// <param name="fromDate">From date.</param>
		/// <param name="toDate">To date.</param>
		/// <param name="maxClaimsToExtract">how many claims to bring back</param>
		/// <param name="includeSpecialClaims">whether to include special claims or not</param>
		/// <returns>A view of the sample</returns>
		ClaimSampleViewModel GetClaimSample( string claimType, string orgCode, string esaCode, string siteCode,
		   DateTime? fromDate, DateTime? toDate,
			int? maxClaimsToExtract, bool includeSpecialClaims );

		/// <summary>
		///   Gets related data for a claim.
		/// </summary>
		/// <param name="claimId">The claim identifier.</param>
		/// <param name="claimSequenceNumber">The claim sequence number.</param>
		/// <returns></returns>
		ZEIS0P82ViewModel GetRelatedData(long claimId, int claimSequenceNumber);
	}
}
