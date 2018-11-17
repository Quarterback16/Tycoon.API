using System.Runtime.Serialization;
using Employment.Esc.Shared.Contracts;

namespace ProgramAssuranceTool.DataContracts
{
	/// <summary>
	///   The request object for service P82 Program Assurance Tool Related Data Service
	/// </summary>
	[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "ZEIS0P82" )]
	public class ZEIS0P82GetRequest : IExtensibleDataObject
	{
		[DataMember( Order = 1, IsRequired = true )]
		[FieldAlias( "inClaimId" )]
		public long ClaimId { get; set; }

		[DataMember( Order = 2, IsRequired = true )]
		[FieldAlias( "inClaimSeqNumber" )]
		public int ClaimSequenceNumber { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}
}