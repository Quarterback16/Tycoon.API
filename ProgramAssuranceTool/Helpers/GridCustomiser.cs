using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels;

namespace ProgramAssuranceTool.Helpers
{
	public class GridCustomiser
	{
		public const string ReviewListSortOrder = "ReviewList-SortOrder";
		public const string ReviewListAscendingOrDescending = "ReviewList-AscOrDesc";
		public const string ReviewListPageSize = "ReviewList-PageSize";
		public const string ReviewListGridWidth = "ReviewList-GridWidth";
		public const string ReviewListMargin = "ReviewList-Margin";
		public const string ReviewListDepth = "ReviewList-Depth";

		protected readonly IUserSettingsRepository UserSettingsRepository;

		public GridCustomiser( IUserSettingsRepository userSettingsRepository )
		{
			UserSettingsRepository = userSettingsRepository;
		}

		/// <summary>
		///   Turn the vm into a list of user settings that can be saved.
		/// </summary>
		/// <param name="vm">view model in</param>
		/// <returns>list of user settings</returns>
		public List<UserSetting> GetUserSettingsFromGridViewModel( CustomiseReviewGridViewModel vm )
		{
			var list = new List<UserSetting>();
			if (! string.IsNullOrEmpty( vm.Col01 ))
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col01} );
			if ( !string.IsNullOrEmpty( vm.Col02 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col02 } );
			if ( !string.IsNullOrEmpty( vm.Col03 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col03 } );
			if ( !string.IsNullOrEmpty( vm.Col04 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col04 } );
			if ( !string.IsNullOrEmpty( vm.Col05 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col05 } );
			if ( !string.IsNullOrEmpty( vm.Col06 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col06 } );
			if ( !string.IsNullOrEmpty( vm.Col07 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col07 } );
			if ( !string.IsNullOrEmpty( vm.Col08 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col08 } );
			if ( !string.IsNullOrEmpty( vm.Col09 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col09 } );
			if ( !string.IsNullOrEmpty( vm.Col10 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col10 } );
			if ( !string.IsNullOrEmpty( vm.Col11 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col11 } );
			if ( !string.IsNullOrEmpty( vm.Col12 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col12 } );
			if ( !string.IsNullOrEmpty( vm.Col13 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col13 } );
			if ( !string.IsNullOrEmpty( vm.Col14 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col14 } );
			if ( !string.IsNullOrEmpty( vm.Col15 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col15 } );
			if ( !string.IsNullOrEmpty( vm.Col16 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col16 } );
			if ( !string.IsNullOrEmpty( vm.Col17 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col17 } );
			if ( !string.IsNullOrEmpty( vm.Col18 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col18 } );
			if ( !string.IsNullOrEmpty( vm.Col19 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col19 } );
			if ( !string.IsNullOrEmpty( vm.Col20 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col20 } );
			if ( !string.IsNullOrEmpty( vm.Col21 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col21 } );
			if ( !string.IsNullOrEmpty( vm.Col22 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col22 } );
			if ( !string.IsNullOrEmpty( vm.Col23 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col23 } );
			if ( !string.IsNullOrEmpty( vm.Col24 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col24 } );
			if ( !string.IsNullOrEmpty( vm.Col25 ) )
				list.Add( new UserSetting { Name = CommonConstants.ReviewListColumn, UserId = vm.UserId, SerialiseAs = "string", Value = vm.Col25 } );
			list.Add( new UserSetting { Name = ReviewListSortOrder, UserId = vm.UserId, SerialiseAs = "string", Value = vm.SortOrder } );
			list.Add( new UserSetting { Name = ReviewListAscendingOrDescending, UserId = vm.UserId, SerialiseAs = "string", Value = vm.AscOrDescending } );
			list.Add( new UserSetting { 
				Name = ReviewListPageSize, 
				UserId = vm.UserId, SerialiseAs = "string", 
				Value = vm.PageSize } );
			list.Add( new UserSetting
			{
				Name = ReviewListGridWidth,
				UserId = vm.UserId,
				SerialiseAs = "integer",
				Value = string.Format( "{0}", vm.GridWidth )
			} );
			list.Add( new UserSetting
			{
				Name = ReviewListMargin,
				UserId = vm.UserId,
				SerialiseAs = "integer",
				Value = string.Format( "{0}", vm.Margin )
			} );
			list.Add( new UserSetting
			{
				Name = ReviewListDepth,
				UserId = vm.UserId,
				SerialiseAs = "integer",
				Value = string.Format( "{0}", vm.Depth )
			} );

			return list;
		}

		public CustomiseReviewGridViewModel GetGridViewModelFromUserSettings( List<UserSetting> list )
		{
			var vm = new CustomiseReviewGridViewModel();
			if ( list.Count == 0 )
			{
				//  get default Customisations
				vm = GetDefaultCustomisation();
			}
			else
			{
				var theSetting = GetColumnSetting( 1, list );
				if ( theSetting.Value != null ) vm.Col01 = theSetting.Value;

				theSetting = GetColumnSetting( 2, list );
				if ( theSetting.Value != null ) vm.Col02 = theSetting.Value;

				theSetting = GetColumnSetting( 3, list );
				if ( theSetting.Value != null ) vm.Col03 = theSetting.Value;

				theSetting = GetColumnSetting( 4, list );
				if ( theSetting.Value != null ) vm.Col04 = theSetting.Value;

				theSetting = GetColumnSetting( 5, list );
				if ( theSetting.Value != null ) vm.Col05 = theSetting.Value;

				theSetting = GetColumnSetting( 6, list );
				if ( theSetting.Value != null ) vm.Col06 = theSetting.Value;

				theSetting = GetColumnSetting( 7, list );
				if ( theSetting.Value != null ) vm.Col07 = theSetting.Value;

				theSetting = GetColumnSetting( 8, list );
				if ( theSetting.Value != null ) vm.Col08 = theSetting.Value;

				theSetting = GetColumnSetting( 9, list );
				if ( theSetting.Value != null ) vm.Col09 = theSetting.Value;

				theSetting = GetColumnSetting( 10, list );
				if ( theSetting.Value != null ) vm.Col10 = theSetting.Value;

				theSetting = GetColumnSetting( 11, list );
				if ( theSetting.Value != null ) vm.Col11 = theSetting.Value;

				theSetting = GetColumnSetting( 12, list );
				if ( theSetting.Value != null ) vm.Col12 = theSetting.Value;

				theSetting = GetColumnSetting( 13, list );
				if ( theSetting.Value != null ) vm.Col13 = theSetting.Value;

				theSetting = GetColumnSetting( 14, list );
				if ( theSetting.Value != null ) vm.Col14 = theSetting.Value;

				theSetting = GetColumnSetting( 15, list );
				if ( theSetting.Value != null ) vm.Col15 = theSetting.Value;

				theSetting = GetColumnSetting( 16, list );
				if ( theSetting.Value != null ) vm.Col16 = theSetting.Value;

				theSetting = GetColumnSetting( 17, list );
				if ( theSetting.Value != null ) vm.Col17 = theSetting.Value;

				theSetting = GetColumnSetting( 18, list );
				if ( theSetting.Value != null ) vm.Col18 = theSetting.Value;

				theSetting = GetColumnSetting( 19, list );
				if ( theSetting.Value != null ) vm.Col19 = theSetting.Value;

				theSetting = GetColumnSetting( 20, list );
				if ( theSetting.Value != null ) vm.Col20 = theSetting.Value;

				theSetting = GetColumnSetting( 21, list );
				if ( theSetting.Value != null ) vm.Col21 = theSetting.Value;

				theSetting = GetColumnSetting( 22, list );
				if ( theSetting.Value != null ) vm.Col22 = theSetting.Value;

				theSetting = GetColumnSetting( 23, list );
				if ( theSetting.Value != null ) vm.Col23 = theSetting.Value;

				theSetting = GetColumnSetting( 24, list );
				if ( theSetting.Value != null ) vm.Col24 = theSetting.Value;

				theSetting = GetColumnSetting( 25, list );
				if ( theSetting.Value != null ) vm.Col25 = theSetting.Value;

				theSetting = GetSetting( ReviewListAscendingOrDescending, list );
				if ( theSetting.Value != null ) vm.AscOrDescending = theSetting.Value;

				theSetting = GetSetting( ReviewListSortOrder, list );
				if ( theSetting.Value != null ) vm.SortOrder = theSetting.Value;

				theSetting = GetSetting( ReviewListPageSize, list );
				if ( theSetting.Value != null ) vm.PageSize = theSetting.Value;

				theSetting = GetSetting( ReviewListGridWidth, list );
				if ( theSetting.Value != null ) vm.GridWidth = Int32.Parse( theSetting.Value );

				theSetting = GetSetting( ReviewListMargin, list );
				if ( theSetting.Value != null ) vm.Margin = Int32.Parse( theSetting.Value );

				theSetting = GetSetting( ReviewListDepth, list );
				if ( theSetting.Value != null ) vm.Depth = Int32.Parse( theSetting.Value );
			}
			return vm;
		}

		private static CustomiseReviewGridViewModel GetDefaultCustomisation()
		{
			return new CustomiseReviewGridViewModel
				{
					Col01 = CommonConstants.ReviewListHeadingAutoSpecialClaim,
					Col02 = CommonConstants.ReviewListHeadingManualSpecialClaim,
					Col03 = CommonConstants.ReviewListHeadingJobSeekerId,
					Col04 = CommonConstants.ReviewListHeadingClaimId,
					Col05 = CommonConstants.ReviewListHeadingJobSeekerGivenName,
					Col06 = CommonConstants.ReviewListHeadingJobSeekerSurname,
					Col07 = CommonConstants.ReviewListHeadingClaimAmount,
					Col08 = CommonConstants.ReviewListHeadingClaimType,
					Col09 = CommonConstants.ReviewListHeadingClaimDescription,
					Col10 = CommonConstants.ReviewListHeadingEmploymentServiceAreaCode,
					Col11 = CommonConstants.ReviewListHeadingEmploymentServiceAreaName,
					Col12 = CommonConstants.ReviewListHeadingOrgCode,
					Col13 = CommonConstants.ReviewListHeadingOrgName,
					Col14 = CommonConstants.ReviewListHeadingSiteCode,
					Col15 = CommonConstants.ReviewListHeadingSiteName,
					Col16 = CommonConstants.ReviewListHeadingStateCode,
					Col17 = CommonConstants.ReviewListHeadingManagedBy,
					Col18 = CommonConstants.ReviewListHeadingContractType,
					Col19 = CommonConstants.ReviewListHeadingContractTypeDescription,
					Col20 = CommonConstants.ReviewListHeadingClaimCreationDate,
					Col21 = CommonConstants.ReviewListHeadingActivityId,
					Col22 = CommonConstants.ReviewListHeadingAssessmentOutcome,
					Col23 = CommonConstants.ReviewListHeadingRecoveryReason,
					Col24 = CommonConstants.ReviewListHeadingAssessmentAction,
					Col25 = CommonConstants.ReviewListHeadingFinalOutcome,
					GridWidth = CommonConstants.GridStandardWidth,
					Margin = 40,
					Depth = CommonConstants.GridStandardHeight,
					PageSize = CommonConstants.GridStandardNoOfRows.ToString( CultureInfo.InvariantCulture ),
					SortOrder = CommonConstants.ReviewListHeadingClaimId,
					AscOrDescending = "Ascending"
				};
		}

		private static UserSetting GetColumnSetting( int colNo, IEnumerable<UserSetting> list )
		{
			var theSetting = new UserSetting();
			var userSettings = list as UserSetting[] ?? list.ToArray();
			if (colNo <= userSettings.Count() )
			{
				var aSetting = userSettings[ colNo - 1 ];
				if (aSetting.Name == CommonConstants.ReviewListColumn)
					theSetting = aSetting;
			}
			return theSetting;
		}

		private static UserSetting GetSetting( string settingRequired, IEnumerable<UserSetting> list )
		{
			var theSetting = new UserSetting();
			foreach ( var userSetting in list
				.Where( userSetting => userSetting.Name.Equals( settingRequired ) ) )
			{
				theSetting = userSetting;
				break;
			}
			return theSetting;
		}

		public List<UserSetting> GetDefaultUserSettings( string userId )
		{
			var vm = GetDefaultCustomisation();
			vm.UserId = userId;
			var list = GetUserSettingsFromGridViewModel( vm );
			return list;
		}

		public CustomiseReviewGridViewModel RefreshCustomisations( CustomiseReviewGridViewModel vm )
		{
			//  convert to list of settings
			var list = GetUserSettingsFromGridViewModel( vm );
			//  process list
			var myDict = new Dictionary<string, UserSetting>();
			foreach ( var userSetting in list
				.Where( userSetting => ! myDict.ContainsKey( userSetting.SettingKey() ) ) )
				myDict.Add( userSetting.SettingKey(), userSetting );
			//  convert dict back to list
			var newList = myDict.Select( item => item.Value ).ToList();
			//  convert list back to view model
			var vmOut = GetGridViewModelFromUserSettings( newList );
			vmOut.UserId = vm.UserId;
			vmOut.UploadId = vm.UploadId;
			return vmOut;
		}
	}
}