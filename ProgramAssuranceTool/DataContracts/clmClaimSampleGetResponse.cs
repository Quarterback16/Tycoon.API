using System.Runtime.Serialization;
using Employment.Esc.Shared.Contracts;

namespace ProgramAssuranceTool.DataContracts
{
	/// <summary>
	///   The response object for the Program Assurance Tool Claim Sample Service
	/// </summary>
	[DataContract( Namespace = "http://employment.esc.contracts/2011/03", Name = "pauClaimSampleGetResponse" )]
	public class clmClaimSampleGetResponse : IExtensibleDataObject
	{
		[DataMember( Order = 1 )]
		[FieldAlias( "outGroupClaimSample" )]
		public pauClaimSampleItem[] Claims { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}
}