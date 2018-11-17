using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.DataAccess
{
    [Group("Overview")]
    public class StaticMappingViewModel
    {
        [Display(GroupName = "Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                .AddTitle("Mapping Documentation and Demos")
                .AddParagraph("As you will be aware, we have discontinued the use of AutoMapper. But you have following options to perform mapping.")
                .BeginUnorderedList()
                .AddListItem("Static mapping using T4 Templates")
                .AddListItem("Extension methods")
                .AddListItem("Explicit Conversion")
                .AddListItem("Manual mapping")
                .AddListItem("Combination of above all")
                .EndUnorderedList()

                .AddTitle("Static mapping using T4 Templates")
                .AddSubTitle("Overview")
                .AddParagraph("This method makes use of T4 template that will generate the mappings. Your template can be based on the template shown below. Methods to be used are listed below.")
                .BeginOrderedList()
                .AddListItem("StaticMapper, copy the Employment.TextTemplating.dll to a directory named tools under the ESG_SDK directory (the ESG_SDK directory is the directory identified by the ESG_SDK environment variable). The required assembly is available ")
                .AddExternalLink("file://epfpn010/ESG_SOFTWARE/Developer%20Software/ESG/StaticMapper")
                .AddListItem("Create a Mappers folder inside your project.")
                .AddListItem("Add new item --> Text Template")
                .AddListItem("You do not need to add any assembly reference to your project.")
                .BeginListItem()
                    .AddText("Replace the contents of this new template with the following. Use your Area Name instead of Example and import namespaces and assemblies you need.")
                    .AddPreformatted(@" <#@ template debug='false\' hostspecific=\'false\' language=\'C#\'  #>
<#@ output extension=\'.cs\' #>
<#@ assembly name=\'%ESG_SDK%\Tools\Employment.TextTemplating.dll\' #>
<#@ import namespace=\'Employment.TextTemplating\' #>
<# // Import the assemblies and namespaces for the types you are generating mappings for. #> 
<#@ assembly name=\'System.Core\' #>
<#@ assembly name=\'$(SolutionDir)\Site\bin\Employment.Web.Mvc.Infrastructure.dll\' #>
<#@ assembly name=\'$(SolutionDir)\Site\bin\Employment.Web.Mvc.Area.Example.dll\' #>
<#@ assembly name=\'$(SolutionDir)\Site\bin\Employment.Web.Mvc.Service.Interfaces.dll\' #>
<#@ import namespace=\'System.Linq\' #>
<#@ import namespace=\'System.Text\' #>
<#@ import namespace=\'System.Collections.Generic\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.Models\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.Service.Interfaces\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.ViewModels\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.ViewModels.Grid\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid\' #>
<#@ import namespace=\'Employment.Web.Mvc.Area.Example.ViewModels.Workflow\' #>
<#@ import namespace=\'Employment.Web.Mvc.Infrastructure.Types\' #>
<#@ import namespace=\'Employment.Web.Mvc.Infrastructure.ValueResolvers\' #> 
<#@ import namespace=\'System.Security.Claims\' #> 
<#@ import namespace=\'Employment.Web.Mvc.Infrastructure.Models.JobSeeker\' #> 
<#@ import namespace=\'System\' #> 
<#@ import namespace=\'Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker\'#> 
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
using Employment.Web.Mvc.Area.Example.Service.Interfaces;")
                .EndListItem()
                .EndOrderedList()
                .AddStrongText("Create Mapper instance")
                .AddPreformatted("var jobseekerSearchViewModelToJobSeekerSearchModel = new StaticMapping<JobseekerSearchViewModel, JobSeekerSearchModel>();")
                .AddText("If you wish to exclude any members:")
                .AddPreformatted("jobseekerSearchViewModelToJobSeekerSearchModel.IgnoreMember(\"Id\")")
                .AddStrongText("GenerateMapping() ")
                .AddText("Use this method when you have one-to-one mappings. Do not use this method if you have ignored any members. This method generates two functions: ")
                .BeginUnorderedList()
                .AddListItem("Generated-1. First generated method will simply create a new instance of destination class, then call the second method and returns this destination instance")
                .AddListItem("Generated-2. Second method takes two parameters source instance and target instance, then creates the mapping between them.")
                .EndUnorderedList()
                .AddStrongText("MapNewMethod() ")
                .AddEmphasisText("Must be used in conjuction with BeginMapExistingMethod() and EndMapExistingMethod()")
                .AddText("This method creates the 'Generated-1' listed above.").AddParagraph("")
                .AddStrongText("BeginMapExistingMethod() ")
                .AddEmphasisText("EndMapExistingMethod() must be called in the end.")
                .AddText("This method creates the 'Generated-2' method listed above. It will do the mappings for the fields except for the members that are ignored. It will not close the method, so you can do custom mappings.")
                .AddUnderlinedText("You can add custom mappings at this this point.")
                .AddPreformatted(@"<#= dummyModelToDummyViewModel.BeginMapExistingMethod() #>
			                        target.DateTime = source.DateTime ?? DateTime.MinValue;
		                           <#= dummyModelToDummyViewModel.EndMapExistingMethod() #>")
                .AddStrongText("EndMapExistingMethod() ")
                .AddEmphasisText("Ensure that BeginMapExistingMethod() is called prior to calling this method.")
                .AddText("This method adds the closing tag (}) indicating the end of method.")
                .AddParagraph("More information can be found at:")
                .AddExternalLink("http://esg-technical.construction.enet/guidance/developer/static-mapping.html")
                .AddSubTitle("Sample Template Example")
                .AddUnderlinedText("You can look at ExampleMapper.tt located here: $/ESC4/ESS/WiP/Web/Zeus/Zeus.Complete/Site/Areas/Example/Mappers")
                .AddParagraph(".....examples......")
                .AddText("Sample Template")
                .AddPreformatted(@"
<#@ template debug='false' hostspecific='false' language='C#'  #>
<#@ output extension='.cs' #>
<#@ assembly name='%ESG_SDK%\Tools\Employment.TextTemplating.dll' #>
<#@ import namespace='Employment.TextTemplating' #>
<# // Import the assemblies and namespaces for the types you are generating mappings for. #> 
<#@ assembly name='System.Core' #>
<#@ assembly name='$(SolutionDir)\Site\bin\Employment.Web.Mvc.Infrastructure.dll' #>
<#@ assembly name='$(SolutionDir)\Site\bin\Employment.Web.Mvc.Area.Example.dll' #>
<#@ assembly name='$(SolutionDir)\Site\bin\Employment.Web.Mvc.Service.Interfaces.dll' #>
<#@ import namespace='System.Linq' #>
<#@ import namespace='System.Text' #>
<#@ import namespace='System.Collections.Generic' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.Models' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.Service.Interfaces' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.ViewModels' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.ViewModels.Grid' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid' #>
<#@ import namespace='Employment.Web.Mvc.Area.Example.ViewModels.Workflow' #>
<#@ import namespace='Employment.Web.Mvc.Infrastructure.Types' #>
<#@ import namespace='Employment.Web.Mvc.Infrastructure.ValueResolvers' #> 
<#@ import namespace='System.Security.Claims' #> 
<#@ import namespace='Employment.Web.Mvc.Infrastructure.Models.JobSeeker' #> 
<#@ import namespace='System' #> 
<#@ import namespace='Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker'#> 
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

<#
var jobSeekerSearchModelToJobSeekerModelListMapping = new StaticMapping<JobSeekerSearchModel, JobSeekerModelList>();
jobSeekerSearchModelToJobSeekerModelListMapping.IgnoreMember('DOB');
jobSeekerSearchModelToJobSeekerModelListMapping.IgnoreMember('Id');

var jobseekerSearchViewModelToJobSeekerSearchModel = new StaticMapping<JobseekerSearchViewModel, JobSeekerSearchModel>();
jobseekerSearchViewModelToJobSeekerSearchModel.IgnoreMember('Id');
jobseekerSearchViewModelToJobSeekerSearchModel.IgnoreMember('DOB');

var dummiesAllPageMetadataToDummiesAllViewModel = new StaticMapping<DummiesAllPageMetadata, DummiesAllViewModel>();
var dummiesAllViewModelToDummiesAllPageMetadata = new StaticMapping<DummiesAllViewModel, DummiesAllPageMetadata>();

var sortingGridMetadataToSortingViewModel = new StaticMapping<SortingGridMetadata, SortingViewModel>();
var sortModelToSortModel = new StaticMapping<SortModel, SortModel>();
var sortingViewModelToSortingGridMetadata = new StaticMapping<SortingViewModel, SortingGridMetadata>();
var gridSortingViewModelToGridSortingViewModel = new StaticMapping<SortModel, GridSortingViewModel>();

var dummiesMainframeViewModelToDummiesMainframePageMetadata = new StaticMapping<DummiesMainframeViewModel, DummiesMainframePageMetadata>();
var dummiesModelToDummiesMainframePageMetadata = new StaticMapping<DummiesModel, DummiesMainframePageMetadata>();


var dummyModelToDummyViewModel = new StaticMapping<DummyModel, DummyViewModel>();
dummyModelToDummyViewModel.IgnoreMember('DateTime');
var claimToClaimViewModel = new StaticMapping<Claim, ClaimViewModel>();
var claimToClaimWithButtonsViewModel = new StaticMapping<Claim, ClaimWithButtonsViewModel>();
claimToClaimWithButtonsViewModel.IgnoreMember('HashKey');

var claimWithButtonsViewModelToButtonEditViewModel = new StaticMapping<ClaimWithButtonsViewModel, ButtonEditViewModel>();
claimWithButtonsViewModelToButtonEditViewModel.IgnoreMember('HashKey');

var claimToButtonEditViewModel = new StaticMapping<Claim, ButtonEditViewModel>();
claimToButtonEditViewModel.IgnoreMember('HashKey');

var step1ViewModelToStep3ViewModel = new StaticMapping<Step1ViewModel, Step3ViewModel>();
var step2ViewModelToStep3ViewModel = new StaticMapping<Step2ViewModel, Step3ViewModel>();
var step2AlternativeViewModelToStep3ViewModel = new StaticMapping<Step2AlternativeViewModel, Step3ViewModel>();

 

#>
	
	/// <summary>
    /// Represents a mapper that is used to map between View Models and Domain Models.
    /// </summary>
	public partial class ExampleMapper 
	{

		<#= jobSeekerSearchModelToJobSeekerModelListMapping.MapNewMethod() #> 
		<#= jobSeekerSearchModelToJobSeekerModelListMapping.BeginMapExistingMethod() #> 
			target.DOB = ParseToDateTime(source.DOB);
			target.Id = ParseToLong(source.Id);
		<#= jobSeekerSearchModelToJobSeekerModelListMapping.EndMapExistingMethod() #> 

		<#= jobseekerSearchViewModelToJobSeekerSearchModel.MapNewMethod() #>
		<#= jobseekerSearchViewModelToJobSeekerSearchModel.BeginMapExistingMethod() #>
			target.DOB = source.DOB.HasValue ? source.DOB.Value.ToString() : string.Empty;
			target.Id = source.Id.HasValue ? source.Id.Value.ToString() : string.Empty;
		<#= jobseekerSearchViewModelToJobSeekerSearchModel.EndMapExistingMethod() #>

		<#= dummiesAllPageMetadataToDummiesAllViewModel.GenerateMapping() #>
		<#= dummiesAllViewModelToDummiesAllPageMetadata.GenerateMapping() #>

#region SORTING Mapping		
		<#= sortingGridMetadataToSortingViewModel.GenerateMapping() #>
		<#= sortModelToSortModel.GenerateMapping() #>
		<#= sortingViewModelToSortingGridMetadata.GenerateMapping() #>
		<#= gridSortingViewModelToGridSortingViewModel.GenerateMapping() #>
#endregion

		<#= dummiesMainframeViewModelToDummiesMainframePageMetadata.GenerateMapping() #>
		<#= dummiesModelToDummiesMainframePageMetadata.GenerateMapping() #>
		 

		<#= dummyModelToDummyViewModel.BeginMapExistingMethod() #>
			target.DateTime = source.DateTime ?? DateTime.MinValue;
		<#= dummyModelToDummyViewModel.EndMapExistingMethod() #>

		<#= claimToClaimViewModel.GenerateMapping() #>
		
		<#= claimToClaimWithButtonsViewModel.BeginMapExistingMethod() #>
			target.HashKey = source.GetHashCode().ToString();
		<#= claimToClaimWithButtonsViewModel.EndMapExistingMethod() #>

		<#= claimWithButtonsViewModelToButtonEditViewModel.BeginMapExistingMethod() #>
			target.HashKey = source.GetHashCode().ToString();
		<#= claimWithButtonsViewModelToButtonEditViewModel.EndMapExistingMethod() #>


		<#= claimToButtonEditViewModel.BeginMapExistingMethod() #>
			target.HashKey = source.GetHashCode().ToString();
		<#= claimToButtonEditViewModel.EndMapExistingMethod() #>


		<#= step1ViewModelToStep3ViewModel.GenerateMapping() #>
		<#= step2ViewModelToStep3ViewModel.GenerateMapping() #>
		<#= step2AlternativeViewModelToStep3ViewModel.MapNewMethod() #>
		<#= step2AlternativeViewModelToStep3ViewModel.BeginMapExistingMethod() #>
		<#= step2AlternativeViewModelToStep3ViewModel.EndMapExistingMethod() #>
	
	}



	
}
")

                .AddTitle("Extension methods")
                .AddSubTitle("Overview")
                .AddText("As the name suggests you can create extension methods for mapping.")
                .AddEmphasisText("You can use static mapping simulateneously with extension methods. To do this:")
                .BeginOrderedList()
                    .AddListItem("Create a Partial class and name it the same as the name stated in template.")
                    .AddListItem("Make this class static.")
                    .AddListItem("Now you can add extension methods using 'this' keyword in this class.")
                .EndOrderedList()
                .AddUnderlinedText("Cases where this will be used is when mapping collections. You can map the base types in collection using static mapping template and then create extenion methods for mapping the collections as shown in the example.")
                .AddSubTitle(".....Examples......")
                .AddPreformatted(@" 
public static Pageable<GridSortingViewModel> ToGridSortingViewModelList(this IEnumerable<SortModel> src, Pageable<GridSortingViewModel> dest)
{
    if (src != null)
    {
        foreach (var s in src)
        {
            dest.Add(ExampleMapper.MapToGridSortingViewModel(s));
        }
    }

    return dest;
}")


                .AddTitle("Explicit Conversion")
                .AddSubTitle("Overview")
                .AddText("You can create explicit conversion in the constructor of the target class as follows:")
                .AddSubTitle(".....Examples......")
                .AddPreformatted(@"
public static explicit operator UserContext(Contracts.DataContracts.UserContext dataUserContext)
        {
            var userContext = new UserContext
            {
                ContinueOlderVersion = dataUserContext.ContinueOlderVersion,
                Contracts = dataUserContext.Contracts,
                CurrentContract = dataUserContext.CurrentContract,
                CurrentOrganisation = dataUserContext.CurrentOrganisation,
                CurrentSecurityGroup = dataUserContext.CurrentSecurityGroup,
                CurrentSite = dataUserContext.CurrentSite,
                DateTime = dataUserContext.DateTime,
                DaysToPasswordExpiry = dataUserContext.DaysToPasswordExpiry,
                DefaultOrganisation = dataUserContext.DefaultOrganisation,
                DefaultSecurityGroup = dataUserContext.DefaultSecurityGroup                 
            };
            return userContext;
        }

")

                .AddTitle("Manual mapping")


                .AddTitle("Combination of above all")
                .AddSubTitle("Overview")
                .AddText("As mentioned above, you can create a partial class with the same name and add your custom mapping methods there and also use T4 template for straight-forward mappings.")
                ;
            }
        }

    }
}