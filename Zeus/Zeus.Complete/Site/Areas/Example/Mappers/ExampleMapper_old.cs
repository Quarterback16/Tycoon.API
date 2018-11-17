using System.Collections.Generic;
using Employment.Web.Mvc.Area.Example.Models;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels;
using Employment.Web.Mvc.Area.Example.ViewModels.Grid;
using Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid;
using Employment.Web.Mvc.Area.Example.ViewModels.Workflow;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using System.Security.Claims;
using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using System;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;

namespace Employment.Web.Mvc.Area.Example.Mappers
{
    
    //public static class ExampleMapper
    //{
        /*
          
These don't have to be done as extension methods - they could also be put into a Helper class as static methods for those who don't like extension methods.


        */


        //public static JobSeekerModelList ToJobseekerModelList(this JobSeekerSearchModel model)
        //{
        //    if (model == null)
        //        return null;

            

        //    var modelToReturn = new JobSeekerModelList
        //    {
        //        FirstName = model.FirstName,
        //        Gender = model.Gender,                
        //        LastName = model.LastName,
        //        MiddleName = model.MiddleName,
        //        PreferedName = model.PreferedName,
        //        Salutation = model.Salutation,
        //        Type = model.Type
        //    };

        //    DateTime date;
        //    if(DateTime.TryParse(model.DOB, out date))
        //    {
        //        modelToReturn.DOB = date;
        //    }

        //    long id;
        //    if(Int64.TryParse(model.Id, out id))
        //    {
        //        modelToReturn.Id = id;
        //    }

        //    return modelToReturn;
        //}



        //public static JobSeekerSearchModel ToJobSeekerSearchModel(this JobseekerSearchViewModel model)
        //{
        //    if (model == null)
        //        return null;



        //    var modelToReturn = new JobSeekerSearchModel
        //    {
        //        DOB = model.DOB.ToString(),
        //        Id = model.Id  + "",
        //        FirstName = model.FirstName,
        //        Gender = model.Gender,
        //        LastName = model.LastName,
        //        MiddleName = model.MiddleName,
        //        PreferedName = model.PreferedName,
        //        Salutation = model.Salutation,
        //        Type = model.Type,
        //        Email = model.Email,
        //        Status = model.Status
        //    };


        //    return modelToReturn;
        //}

        /// <summary>
        /// Maps View Model to Page Metadata.
        /// </summary>
        //public static DummiesAllViewModel ToDummiesAllViewModel(this DummiesAllPageMetadata src)
        //{
        //    var dest = new DummiesAllViewModel();
        //    dest.StartsWith = src.StartsWith;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the dummies all page metadata.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static DummiesAllPageMetadata ToDummiesAllPageMetadata(this DummiesAllViewModel src) {
        //    var dest = new DummiesAllPageMetadata();
        //    dest.StartsWith = src.StartsWith;
        //    return dest;
        //}


        /// <summary>
        /// Convert to the sorting view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static SortingViewModel ToSortingViewModel(this SortingGridMetadata src) {
        //    var dest = new SortingViewModel();
        //    dest.Name = src.Name;
        //    return dest;
        //}
        /// <summary>
        /// Convert to the sort model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static SortModel ToSortModel(this SortingGridMetadata src) {
        //    var dest = new SortModel();
        //    return dest;
        //}

        /// <summary>
        /// Convert to the sorting grid metadata.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static SortingGridMetadata ToSortingGridMetadata(this SortingViewModel src) {
        //    var dest = new SortingGridMetadata();
        //    dest.Name = src.Name;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the grid sorting view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static GridSortingViewModel ToGridSortingViewModel(this SortModel src) {
        //    var dest = new GridSortingViewModel();
        //    dest.Address = src.Address;
        //    dest.Date = src.Date;
        //    dest.Duration = src.Duration;
        //    dest.HtmlContent = src.HtmlContent;
        //    dest.ImageUrl = src.ImageUrl;
        //    dest.LargeTextArea = src.LargeTextArea;
        //    dest.Password = src.Password;
        //    dest.PhoneNumber = src.PhoneNumber;
        //    dest.SortingID = src.SortingID;
        //    dest.Time = src.Time;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the grid sorting view model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        //public static Pageable<GridSortingViewModel> ToGridSortingViewModelList(this IEnumerable<SortModel> src, Pageable<GridSortingViewModel> dest) {
        //    if (src != null)
        //    {
        //        foreach (var s in src)
        //        {
        //            dest.Add(s.ToGridSortingViewModel());
        //        }
        //    }

        //    return dest;
        //}


        /// <summary>
        /// Convert to the dummies mainframe page metadata.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static DummiesMainframePageMetadata ToDummiesMainframePageMetadata(this DummiesMainframeViewModel src) {
        //    var dest = new DummiesMainframePageMetadata();
        //    dest.StartsWith = src.StartsWith;
        //    return dest;
        //}
        /// <summary>
        /// Convert to the dummies mainframe page metadata.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        //public static DummiesMainframePageMetadata ToDummiesMainframePageMetadata(this DummiesModel src, DummiesMainframePageMetadata dest) {
        //    dest.NextSequenceID = src.NextSequenceID;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the dummies mainframe page metadata.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static DummiesMainframePageMetadata ToDummiesMainframePageMetadata(this DummiesModel src) {
        //    var dest = new DummiesMainframePageMetadata();
        //    dest.NextSequenceID = src.NextSequenceID;
        //    return dest;
        //}


        /// <summary>
        /// Convert to the step3 view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static Step3ViewModel ToStep3ViewModel(this Step1ViewModel src) {
        //    var dest = new Step3ViewModel();
        //    dest.Name = src.Name;
        //    dest.OccupationLevel1 = src.OccupationLevel1;
        //    dest.OccupationLevel2 = src.OccupationLevel2;
        //    dest.OccupationLevel3 = src.OccupationLevel3;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the step3 view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static Step3ViewModel ToStep3ViewModel(this Step2ViewModel src) {
        //    var dest = new Step3ViewModel();
        //    dest.EmailAddress = src.EmailAddress;
        //    dest.ShowEmailAddress = !string.IsNullOrEmpty(src.EmailAddress);
        //    return dest;
        //}


        /// <summary>
        /// Convert to the step3 view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static Step3ViewModel ToStep3ViewModel(this Step2AlternativeViewModel src) {
        //    var dest = new Step3ViewModel();
        //    dest.PostalAddress = src.PostalAddress;
        //    dest.State = src.State;
        //    dest.Suburb = src.Suburb;
        //    dest.Postcode = src.Postcode;
        //    dest.ShowPostalAddress = !string.IsNullOrEmpty(src.PostalAddress);
        //    return dest;
        //}

        /// <summary>
        /// Convert to the dummy view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static DummyViewModel ToDummyViewModel(this DummyModel src) {
        //    var dest = new DummyViewModel();
        //    dest.Currency = src.Currency;
        //    dest.Date = src.Date;
        //    if (src.DateTime != null)
        //    {
        //        dest.DateTime = src.DateTime.Value;
        //    }
        //    dest.Decimal1 = src.Decimal1;
        //    dest.Description = src.Description;
        //    dest.DummyID = src.DummyID;
        //    dest.EmailAddress = src.EmailAddress;
        //    dest.Name = src.Name;
        //    dest.Time = src.Time;
        //    dest.Url = src.Url;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the claim view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static ClaimViewModel ToClaimViewModel(this Claim src) {
        //    var dest = new ClaimViewModel();
        //    dest.ClaimType = src.Type;
        //    dest.HashKey = src.GetHashCode().ToString();
        //    dest.Value = src.Value;
        //    return dest;
        //}
        /// <summary>
        /// Convert to the claim with buttons view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static ClaimWithButtonsViewModel ToClaimWithButtonsViewModel(this Claim src) {
        //    var dest = new ClaimWithButtonsViewModel();
        //    dest.ClaimType = src.Type;
        //    dest.HashKey = src.GetHashCode().ToString();
        //    dest.Value = src.Value;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the button edit view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static ButtonEditViewModel ToButtonEditViewModel(this Claim src) {
        //    var dest = new ButtonEditViewModel();
        //    dest.ClaimType = src.Type;
        //    dest.HashKey = src.GetHashCode().ToString();
        //    dest.Value = src.Value;
        //    return dest;
        //}


        /// <summary>
        /// Convert to the button edit view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static ButtonEditViewModel ToButtonEditViewModel(this ClaimWithButtonsViewModel src) {
        //    var dest = new ButtonEditViewModel();
        //    dest.ClaimType = src.ClaimType;
        //    dest.HashKey = src.HashKey;
        //    dest.Value = src.Value;
        //    return dest;
        //}

        /// <summary>
        /// Convert to the claim with buttons view model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static IEnumerable<ClaimWithButtonsViewModel> ToClaimWithButtonsViewModelList(this IEnumerable<Claim> src) {
        //    var dest = new List<ClaimWithButtonsViewModel>();
        //    if (src != null) {
        //        foreach (var s in src) {
        //            dest.Add(s.ToClaimWithButtonsViewModel());
        //        }
        //    }
        //    return dest as IEnumerable<ClaimWithButtonsViewModel>;
        //}
        /// <summary>
        /// Convert to the claim view model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        //public static IEnumerable<ClaimViewModel> ToClaimViewModelList(this IEnumerable<Claim> src) {
        //    var dest = new List<ClaimViewModel>();
        //    if (src != null)
        //    {
        //        foreach (var s in src)
        //        {
        //            dest.Add(s.ToClaimViewModel());
        //        }
        //    }
        //    return dest as IEnumerable<ClaimViewModel>;
        //}

        /// <summary>
        /// Convert to the pageable dummy view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        //public static Pageable<DummyViewModel> ToPageableDummyViewModel(this IEnumerable<DummyModel> src,Pageable<DummyViewModel> dest) {
        //    if (src != null) {
        //        foreach (var s in src) {
        //            dest.Add(s.ToDummyViewModel());
        //        }
        //    }
        //    return dest ;
        //}



    //}
}