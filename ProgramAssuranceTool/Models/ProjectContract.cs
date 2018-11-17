using System;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Models
{
	public class ProjectContract : AuditEntity, IEquatable<ProjectContract>
	{
		private readonly IAdwRepository _adwRepository;

		public ProjectContract( IAdwRepository adwRepository )
		{
			_adwRepository = adwRepository;
		}

		public ProjectContract() : this( new AdwRepository() )
		{
		}

		[Key]
		[Display( Name = "ID" )]
		public int Id { get; set; }

		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		[Display( Name = "Contract Type" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForProjectContracts, true, false )] 
		[Required]
		public string ContractType { get; set; }

		public override string ToString()
		{
			return string.Format( "ID: {0}  Project: {1}  Contract: {2}", Id, ProjectId, ContractType ) ;
		}

		public override bool Equals( object obj )
		{
			if ( obj == null ) return false;
			var objAsProjectContract = obj as ProjectContract;
			return objAsProjectContract != null && Equals( objAsProjectContract );
		}

		public bool Equals( ProjectContract other )
		{
			if ( other == null ) return false;
			return ( ProjectId.Equals( other.ProjectId ) &&
				      ContractType.Equals( other.ContractType )
				);
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public string GetProjectContractDescription()
		{
			var desc = string.Empty;
			if (! string.IsNullOrEmpty( ContractType ))
			{
			   desc = _adwRepository.GetDescription( DataConstants.AdwListCodeForProjectContracts, ContractType );
			   if ( string.IsNullOrEmpty( desc ) )
				   desc = string.Format( "Contract Type :{0} - not found in ADW", ContractType );
			}
			return desc;
		}

		public string GetProjectContractFormatted()
		{
			return string.Format( "{0} - {1}", ContractType, GetProjectContractDescription() );
		}

	}
}