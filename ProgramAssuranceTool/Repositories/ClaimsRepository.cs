using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Esc.Framework.DataTransport.Cics;
using ProgramAssuranceTool.DataContracts;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Claims;

namespace ProgramAssuranceTool.Repositories
{
	public class ClaimsRepository : IClaimsRepository
	{
		/// <summary>
		/// Gets a bunch of random claims based on the input parameters.
		/// </summary>
		/// <param name="claimType">Type of the claim.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="siteCode">The site code.</param>
		/// <param name="fromDate">From date.</param>
		/// <param name="toDate">To date.</param>
		/// <param name="maxClaimsToExtract"></param>
		/// <param name="includeSpecialClaims"></param>
		/// <returns>
		/// A view of the sample
		/// </returns>
		public ClaimSampleViewModel GetClaimSample( 
			string claimType, string orgCode, string esaCode, string siteCode,
			DateTime? fromDate, DateTime? toDate,
			int? maxClaimsToExtract, bool includeSpecialClaims )
		{
			//  MF has a problem with nulls
			if (string.IsNullOrEmpty( claimType )) claimType = " ";

			var errorMessage = "P81 Error occurs while loading claim sample";

			CicsExecutionResult result;
			var request = new clmClaimSampleGetRequest
			{
				ClaimTypeArray = new pauClaimType[] { new pauClaimType{ ClaimType = claimType }  },
				ClaimOrgCode = orgCode,
				ClaimEsaCode = esaCode,
				ClaimSiteCode = siteCode,
				ClaimFromDate = fromDate,
				ClaimToDate = toDate,
				MaximumSampleSize = maxClaimsToExtract == null ? 20 : (int) maxClaimsToExtract,
				AutoSpecialClaimFlag = includeSpecialClaims ? " " : "N" ,
				ManualSpecialClaimFlag = includeSpecialClaims ? " " : "N",
			};

			try
			{
				var response =
					CicsClient.Execute<clmClaimSampleGetRequest, clmClaimSampleGetResponse>( request,
						"ZEIS0P81", "EXECUTE", out result );

				if (result.MessageType == CicsExecuteStatus.Error)
				{
					return new ClaimSampleViewModel
						{
							ErrorMessage = result.Message,
							ErrorCode = result.ErrorCode,
							ExitStatusNumber = result.ExitStatusNumber
						};
				}

				if (response != null && response.Claims.Length > 0)
				{
					var list = ProcessResponse( response );
					return new ClaimSampleViewModel
						{
							ErrorMessage = string.Empty, 
							ErrorCode = string.Empty,
							ExitStatusNumber = string.Empty,
							Claims = list
						};
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				if (errorMessage.StartsWith( "Unable to communicate with CICS" ))
				{
					//  Try one more time
					var response =
					CicsClient.Execute<clmClaimSampleGetRequest, clmClaimSampleGetResponse>( request,
						"ZEIS0P81", "EXECUTE", out result );
					if ( response != null && response.Claims.Length > 0 )
					{
						var list = ProcessResponse( response );
						return new ClaimSampleViewModel
							{
								ErrorMessage = string.Empty,
								ErrorCode = string.Empty,
								ExitStatusNumber = string.Empty,
								Claims = list
							};
					}
				}
				throw;
			}

			return new ClaimSampleViewModel { ErrorMessage = errorMessage };
		}

		private static List<PatClaim> ProcessResponse( clmClaimSampleGetResponse response )
		{
			var list = response.Claims.Select( c => new PatClaim
				{
					ClaimTypeDescription = c.ClaimTypeDescription,
					ContractTypeDescription = c.ContractTypeDescription,
					SiteDescription = c.SiteDescription,
					EsaDescription = c.EsaDescription,
					OrgDescription = c.OrgDescription,
					ClaimId = c.ClaimId,
					ClaimSequenceNumber = c.ClaimSequenceNumber,
					ClaimType = c.ClaimType,
					ClaimAmount = c.ClaimAmount,
					SiteCode = c.SiteCode,
					SupervisingSiteCode = c.SupervisingSiteCode,
					OrgCode = c.OrgCode,
					ActivityId = c.ActivityId,
					ClaimCreationDate = c.ClaimCreationDate,
					StateCode = c.StateCode,
					ManagedBy = c.ManagedBy,
					ContractId = c.ContractId,
					ContractType = c.ContractType,
					ContractTitle = c.ContractTitle,
					EsaCode = c.EsaCode,
					JobseekerId = c.JobseekerId,
					GivenName = c.GivenName,
					Surname = c.Surname,
					StatusCode = c.ClaimStatusCode,
					StatusCodeDescription = c.ClaimStatusDescription,
					AutoSpecialClaimFlag = c.AutoSpecialClaimFlag.Equals( "Y" ),
					ManSpecialClaimFlag = c.ManSpecialClaimFlag.Equals( "Y" )
				} ).ToList();
			return list;
		}

		/// <summary>
		/// Gets related data for a claim.
		/// </summary>
		/// <param name="claimId">The claim identifier.</param>
		/// <param name="claimSequenceNumber">The claim sequence number.</param>
		/// <returns>related data</returns>
		public ZEIS0P82ViewModel GetRelatedData( long claimId, int claimSequenceNumber )
		{
			CicsExecutionResult result;

			var response = CicsClient.Execute<ZEIS0P82GetRequest, ZEIS0P82GetResponse>
				(
					new ZEIS0P82GetRequest { ClaimId = claimId, ClaimSequenceNumber = claimSequenceNumber },
					"ZEIS0P82", "EXECUTE", out result
				);

			if ( result.MessageType == CicsExecuteStatus.Error )
			{
				return new ZEIS0P82ViewModel { ErrorMessage = result.Message };
			}

			if ( response != null && response.ClaimId > 0 )
			{
				return new ZEIS0P82ViewModel
				{
					ClaimId = response.ClaimId,
					ClaimSequenceNumber = response.ClaimSeqNumber,
					ClaimStatusCode = response.ClaimStatusCode,
					ClaimStartDate = response.ClaimStartDate,
					ClaimEndDate = response.ClaimEndDate,
					Stream = response.Stream,
					PlacementDate = response.PlacementDate,
					RefPlacementComDate = response.RefPlacementComDate,
					RefPlacementEndDate = response.RefPlacementEndDate,
					ReferralDate = response.ReferralDate,
					RefOutcomeCode = response.RefOutcomeCode,
					SiteSequenceNumber = response.SiteSequenceNumber,
					SiteCode = response.SiteCode,
					SiteSpecialistType = response.SiteSpecialistType,
					EmployerName = response.EmployerName,
					JobseekerId = response.JobseekerId,
					LMRCode = response.LMRCode,
					RegStatusCode = response.RegStatusCode,
					JobId = response.JobId,
					JobTitleText = response.JobTitleText,
					JobSiteCd = response.JobSiteCd,
					JobCreationDate = response.JobCreationDate,
					JobHoursTxt = response.JobHoursTxt,
					JobDescription = response.JobDescription,
					JobTenureCd = response.JobTenureCd,
					RefOutcomeDescription = response.RefOutcomeDescription,
					JobTenureDescription = response.JobTenureDescription
				};
			}

			return new ZEIS0P82ViewModel { ErrorMessage = "P82 Error occurs while loading claim details" };
		}
	}
}