 
 
 
 
 
 
/*
Generated code. Do not edit manually.
Example Mapper.cs
*/
using Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid;
using Employment.Web.Mvc.Area.Example.ViewModels.Workflow;
using Employment.Web.Mvc.Area.Example.ViewModels.Grid;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using System.Security.Claims;
using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using System;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker; 
using Employment.Web.Mvc.Area.Example.Models;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;


namespace Employment.Web.Mvc.Area.Example.Mappers
{

	
	/// <summary>
    /// Represents a mapper that is used to map between View Models and Domain Models.
    /// </summary>
	public partial class ExampleMapper 
	{

		public static JobSeekerModelList MapToJobSeekerModelList(JobSeekerSearchModel source)
		 {
			var target = new JobSeekerModelList();
			Map(source, target);
			return target;
		}

 
		public static void Map(JobSeekerSearchModel source, JobSeekerModelList target)
		{
			target.Salutation = source.Salutation;
			target.Type = source.Type;
			target.FirstName = source.FirstName;
			target.LastName = source.LastName;
			target.MiddleName = source.MiddleName;
			target.PreferedName = source.PreferedName;
			target.Gender = source.Gender;
 
			target.DOB = ParseToDateTime(source.DOB);
			target.Id = ParseToLong(source.Id);
		} 

		public static JobSeekerSearchModel MapToJobSeekerSearchModel(JobseekerSearchViewModel source)
		 {
			var target = new JobSeekerSearchModel();
			Map(source, target);
			return target;
		}


		public static void Map(JobseekerSearchViewModel source, JobSeekerSearchModel target)
		{
			target.Type = source.Type;
			target.Salutation = source.Salutation;
			target.FirstName = source.FirstName;
			target.MiddleName = source.MiddleName;
			target.LastName = source.LastName;
			target.Gender = source.Gender;
			target.PreferedName = source.PreferedName;
			target.Email = source.Email;
			target.Status = source.Status;

			target.DOB = source.DOB.HasValue ? source.DOB.Value.ToString() : string.Empty;
			target.Id = source.Id.HasValue ? source.Id.Value.ToString() : string.Empty;
		}

		public static DummiesAllViewModel MapToDummiesAllViewModel(DummiesAllPageMetadata source)
		 {
			var target = new DummiesAllViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(DummiesAllPageMetadata source, DummiesAllViewModel target)
		{
			target.StartsWith = source.StartsWith;

		}
		public static DummiesAllPageMetadata MapToDummiesAllPageMetadata(DummiesAllViewModel source)
		 {
			var target = new DummiesAllPageMetadata();
			Map(source, target);
			return target;
		}


		public static void Map(DummiesAllViewModel source, DummiesAllPageMetadata target)
		{
			target.StartsWith = source.StartsWith;

		}

#region SORTING Mapping		
		public static SortingViewModel MapToSortingViewModel(SortingGridMetadata source)
		 {
			var target = new SortingViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(SortingGridMetadata source, SortingViewModel target)
		{
			target.Name = source.Name;

		}
		public static SortModel MapToSortModel(SortModel source)
		 {
			var target = new SortModel();
			Map(source, target);
			return target;
		}


		public static void Map(SortModel source, SortModel target)
		{
			target.SortingID = source.SortingID;
			target.Address = source.Address;
			target.Date = source.Date;
			target.Time = source.Time;
			target.PhoneNumber = source.PhoneNumber;
			target.Duration = source.Duration;
			target.LargeTextArea = source.LargeTextArea;
			target.Password = source.Password;
			target.ImageUrl = source.ImageUrl;
			target.HtmlContent = source.HtmlContent;

		}
		public static SortingGridMetadata MapToSortingGridMetadata(SortingViewModel source)
		 {
			var target = new SortingGridMetadata();
			Map(source, target);
			return target;
		}


		public static void Map(SortingViewModel source, SortingGridMetadata target)
		{
			target.Name = source.Name;

		}
		public static GridSortingViewModel MapToGridSortingViewModel(SortModel source)
		 {
			var target = new GridSortingViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(SortModel source, GridSortingViewModel target)
		{
			target.SortingID = source.SortingID;
			target.Address = source.Address;
			target.Date = source.Date;
			target.Time = source.Time;
			target.PhoneNumber = source.PhoneNumber;
			target.Duration = source.Duration;
			target.LargeTextArea = source.LargeTextArea;
			target.Password = source.Password;
			target.ImageUrl = source.ImageUrl;
			target.HtmlContent = source.HtmlContent;

		}
#endregion

		public static DummiesMainframePageMetadata MapToDummiesMainframePageMetadata(DummiesMainframeViewModel source)
		 {
			var target = new DummiesMainframePageMetadata();
			Map(source, target);
			return target;
		}


		public static void Map(DummiesMainframeViewModel source, DummiesMainframePageMetadata target)
		{
			target.StartsWith = source.StartsWith;

		}
		public static DummiesMainframePageMetadata MapToDummiesMainframePageMetadata(DummiesModel source)
		 {
			var target = new DummiesMainframePageMetadata();
			Map(source, target);
			return target;
		}


		public static void Map(DummiesModel source, DummiesMainframePageMetadata target)
		{
			target.NextSequenceID = source.NextSequenceID;

		}
		 

		public static void Map(DummyModel source, DummyViewModel target)
		{
			target.DummyID = source.DummyID;
			target.Name = source.Name;
			target.Description = source.Description;
			target.Date = source.Date;
			target.Time = source.Time;
			target.Currency = source.Currency;
			target.EmailAddress = source.EmailAddress;
			target.Url = source.Url;
			target.Decimal1 = source.Decimal1;

			target.DateTime = source.DateTime ?? DateTime.MinValue;
		}

		public static ClaimViewModel MapToClaimViewModel(Claim source)
		 {
			var target = new ClaimViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(Claim source, ClaimViewModel target)
		{
			target.Value = source.Value;

		}
		
		public static void Map(Claim source, ClaimWithButtonsViewModel target)
		{
			target.Value = source.Value;

			target.HashKey = source.GetHashCode().ToString();
		}

		public static void Map(ClaimWithButtonsViewModel source, ButtonEditViewModel target)
		{
			target.ClaimType = source.ClaimType;
			target.Value = source.Value;

			target.HashKey = source.GetHashCode().ToString();
		}


		public static void Map(Claim source, ButtonEditViewModel target)
		{
			target.Value = source.Value;

			target.HashKey = source.GetHashCode().ToString();
		}


		public static Step3ViewModel MapToStep3ViewModel(Step1ViewModel source)
		 {
			var target = new Step3ViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(Step1ViewModel source, Step3ViewModel target)
		{
			target.Name = source.Name;
			target.OccupationLevel1 = source.OccupationLevel1;
			target.OccupationLevel2 = source.OccupationLevel2;
			target.OccupationLevel3 = source.OccupationLevel3;

		}
		public static Step3ViewModel MapToStep3ViewModel(Step2ViewModel source)
		 {
			var target = new Step3ViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(Step2ViewModel source, Step3ViewModel target)
		{
			target.EmailAddress = source.EmailAddress;

		}
		public static Step3ViewModel MapToStep3ViewModel(Step2AlternativeViewModel source)
		 {
			var target = new Step3ViewModel();
			Map(source, target);
			return target;
		}


		public static void Map(Step2AlternativeViewModel source, Step3ViewModel target)
		{
			target.PostalAddress = source.PostalAddress;
			target.State = source.State;
			target.Postcode = source.Postcode;
			target.Suburb = source.Suburb;

		}
	
	}



	
}