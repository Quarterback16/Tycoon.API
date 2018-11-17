using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Models
{
	public class Project : AuditEntity, IValidatableObject
	{
		private readonly IAdwRepository _adwRepository;

		public Project( IAdwRepository adwRepository )
		{
			_adwRepository = adwRepository;
		}

		public Project() : this( new AdwRepository() )
		{
		}

		[Key]
		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		[Display( Name = "Project Type" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForProjectTypes, true, false )]
		[Required]
		[HtmlProperties( CssClass = "form-control" )]
		public string ProjectType { get; set; }

		[Display( Name = "Org Code" )]
		[StringLength( 4 )]
		public string OrgCode { get; set; }

		[Display( Name = "Organisation" )]
		[StringLength( 250 )]
		[Required]
		public string Organisation { get; set; }

		[Display( Name = "Project Name" )]
		[StringLength( 250 )]
		[Required]
		public string ProjectName { get; set; }

		[Display( Name = "Project Coordinator" )]
		[Required]
		public string Coordinator { get; set; }

		public string CoordinatorCode { get; set; }

		[Display( Name = "Comments" )]
		[ScaffoldColumn( false )]
		[DataType( DataType.MultilineText )]
		[StringLength( 2000 )]
		[HtmlProperties( MaxLength = 2000 )]
		[AllowHtml]
		public string Comments { get; set; }


		public bool Resource_NSW_ACT { get; set; }
		public bool Resource_QLD { get; set; }
		public bool Resource_VIC { get; set; }
		public bool Resource_NT { get; set; }
		public bool Resource_WA { get; set; }
		public bool Resource_SA { get; set; }
		public bool Resource_TAS { get; set; }
		[Display( Name = "Resources")]
		public bool Resource_NO { get; set; }

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			if (string.IsNullOrEmpty( _adwRepository.GetDescription(
				DataConstants.AdwListCodeForProjectTypes, ProjectType ) ))
				yield return new ValidationResult( "Project Type is not valid", new[] {"ProjectType"} );

			if (NoneOfTheResourcesHaveBeenTicked() )
				yield return new ValidationResult( "At least one resource must be selected", new[] { "Resource_NO" } );

		}

		public override string ToString()
		{
			return string.Format( "{0}:{1}", ProjectId, ProjectName );
		}

		private bool NoneOfTheResourcesHaveBeenTicked()
		{
			return !Resource_NO && !Resource_NSW_ACT &&
					 !Resource_NT && !Resource_QLD && !Resource_SA &&
					 !Resource_TAS && !Resource_VIC && !Resource_WA;
		}

		public string PatResourceSet()
		{
			return String.Format( "NO:{0} NSW/ACT:{1} QLD:{2} VIC:{3} SA:{4} WA:{5} TAS:{6} NT:{7}",
				Resource_NO ? "Y" : "N",
								Resource_NSW_ACT ? "Y" : "N",
								Resource_QLD ? "Y" : "N",
								Resource_VIC ? "Y" : "N",
								Resource_SA ? "Y" : "N",
								Resource_WA ? "Y" : "N",
								Resource_TAS ? "Y" : "N",
								Resource_NT ? "Y" : "N"
								);
		}

		public string ResourcesSet()
		{
			var list = new List<String>();
			if ( Resource_NO ) list.Add( "National Office" );
			if ( Resource_NSW_ACT ) list.Add( "New South Wales and ACT" );
			if ( Resource_QLD ) list.Add( "Queensland" );
			if ( Resource_VIC ) list.Add( "Victoria" );
			if ( Resource_SA ) list.Add( "South Australia" );
			if ( Resource_WA ) list.Add( "West Australia" );
			if ( Resource_TAS ) list.Add( "Tasmania" );
			if ( Resource_NT ) list.Add( "Northern Territory" );
			return String.Join( ", ", list );
		}

		public string ResourcesSetShort()
		{
			var list = new List<String>();
			if ( Resource_NO ) list.Add( CommonConstants.ResourceCode_NO );
			if ( Resource_NSW_ACT ) list.Add( CommonConstants.ResourceCode_NSWACT );
			if ( Resource_QLD ) list.Add( CommonConstants.ResourceCode_QLD );
			if ( Resource_VIC ) list.Add( CommonConstants.ResourceCode_VIC );
			if ( Resource_SA ) list.Add( CommonConstants.ResourceCode_SA );
			if ( Resource_WA ) list.Add( CommonConstants.ResourceCode_WA );
			if ( Resource_TAS ) list.Add( CommonConstants.ResourceCode_TAS );
			if ( Resource_NT ) list.Add( CommonConstants.ResourceCode_NT );
			return String.Join( ",", list );
		}

		public string GetProjectTypeDescription()
		{
			var projectTypeDesc = _adwRepository.GetDescription( DataConstants.AdwListCodeForProjectTypes, ProjectType );
			if ( string.IsNullOrEmpty( projectTypeDesc ) )
				projectTypeDesc = string.Format( "Project Type :{0} - not found in ADW", ProjectType );
			return projectTypeDesc;
		}

		public bool IsContractMonitoringOrContractSiteVisit()
		{
			if (ProjectType == null) return false;

			return ( ProjectType.Equals( DataConstants.ProjectType_Contract_Monitoring ) ||
			          ProjectType.Equals( DataConstants.ProjectType_Site_Monitoring ) );
		}

		public string StandardProjectName()
		{
			return string.Format( "{0} - {1} - {2}", OrgCode, GetOrgName(), GetProjectTypeDescription() );
		}

		public string GetOrgName()
		{
			return _adwRepository.GetDescription( DataConstants.AdwListCodeForOrgCodes, OrgCode );
		}

		public string GetFormattedOrgName()
		{
			return string.Format( "{0} - {1}", OrgCode, GetOrgName().Trim() );
		}

		public bool IsConverted()
		{
			// is converted if the forst part of the name is the Org Code
			return ( OrgCode.Equals( ProjectName.Substring( 0, 4 ) ) );
		}

		public bool IsValidProjectType()
		{
			var desc = GetProjectTypeDescription();
			return !desc.EndsWith( "is unknown" );
		}

		public bool CanEdit( PatUser user )
		{
			return CanEdit( user.LoginName, user.IsAdministrator() );
		}

		public bool CanEdit( string userId, bool isAdmin )
		{
			var canEdit = false;
			if (isAdmin)
				canEdit = true;
			else
			{
				if (CoordinatorCode == null) CoordinatorCode = Coordinator;
				if ( userId.Equals( CoordinatorCode ) )
					canEdit = true;
			}

			return canEdit;
		}

		public bool CanExport( string userId )
		{
			var canExport = false;
			var user = new PatUser( userId );
			if ( user.IsAdmin() )
				canExport = true;
			else
			{
				if ( CoordinatorCode == null ) CoordinatorCode = Coordinator;
				if ( userId.Equals( CoordinatorCode ) )
					canExport = true;
				else
				{
					if (user.IsInAnyOfTheseGroups( ResourcesSetShort() ))
						canExport = true;
				}
			}
			return canExport;
		}

		public bool CanAddSample( string userId, int nContracts )
		{
			var canAddSample = false;  
			if (IsContractMonitoringOrContractSiteVisit())
			{
				if (nContracts > 0)
				{
					var user = new PatUser( userId );
					if (user.IsAdministrator())
						canAddSample = true;
					else
					{
						if (CoordinatorCode == null) CoordinatorCode = Coordinator;
						if (userId.Equals( CoordinatorCode ))
							canAddSample = true;
						else
						{
							if (user.IsInAnyOfTheseGroups( ResourcesSetShort() ))
								canAddSample = true;
						}
					}
				}
			}
			return canAddSample;
		}

		public bool CanAddUpload( string userId, int nContracts )
		{
			var canAddUpload = false;
			if ( ! IsContractMonitoringOrContractSiteVisit() )
			{
				if ( nContracts > 0 )
				{
					var user = new PatUser( userId );
					if ( user.IsAdministrator() )
						canAddUpload = true;
					else
					{
						if ( CoordinatorCode == null ) CoordinatorCode = Coordinator;
						if ( userId.Equals( CoordinatorCode ) )
							canAddUpload = true;
						else
						{
							if ( user.IsInAnyOfTheseGroups( ResourcesSetShort() ) )
								canAddUpload = true;
						}
					}
				}
			}
			return canAddUpload;
		}


		public bool CanAddAttachment(string userId)
		{
			var canAdd = false;

			var user = new PatUser(userId);
			if (user.IsAdministrator())
				canAdd = true;
			else
			{
				if (CoordinatorCode == null) CoordinatorCode = Coordinator;
				if (userId.Equals(CoordinatorCode) || userId.Equals(CreatedBy))
					canAdd = true;
				else
				{
					if (user.IsInAnyOfTheseGroups(ResourcesSetShort()))
						canAdd = true;
				}
			}
			return canAdd;
		}

		public bool CanEditCheckList(string userId)
		{
			var canAdd = false;

			var user = new PatUser(userId);
			if (user.IsAdministrator())
				canAdd = true;
			else
			{
				if (CoordinatorCode == null) CoordinatorCode = Coordinator;
				if (userId.Equals(CoordinatorCode) || userId.Equals(CreatedBy))
					canAdd = true;
				else
				{
					if (user.IsInAnyOfTheseGroups(ResourcesSetShort()))
						canAdd = true;
				}
			}
			return canAdd;
		}

		public bool CanDelete( bool isAdmin )
		{
			var canDelete = isAdmin;
			return canDelete;
		}

		public string ProjectNameWithId()
		{
			return string.Format( "{0} - {1}", ProjectId, ProjectName );
		}
	}
}