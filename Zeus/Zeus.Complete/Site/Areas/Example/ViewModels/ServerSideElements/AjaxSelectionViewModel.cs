using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{
    [DisplayName("Ajax selection example")]
    [Group("Basic ajax", Order = 1)]
    [Group("Large data set", Order = 2)]
    [Group("Occupations", Order = 3)]
    [Button("Submit", "Basic ajax")]
    [Button("Clear", "Basic ajax", Clear = true)]
    [Button("Submit", "Large data set")]
    [Button("Clear", "Large data set", Clear = true)]
    [Button("Submit", "Occupations")]
    [Button("Clear", "Occupations", Clear = true)]
    public class AjaxSelectionViewModel
    {
        [Display(GroupName = "Basic ajax", Order = 3)]
        public ContentViewModel ContentForServerSideSingle
        {
            get
            {
                var content = new ContentViewModel()
                    .AddLineBreak()
                    .AddPreformatted(@"        /// An example of a select list that allows one selection and operates on the server side (i.e. it loads its choices via an AJAX call)
        /// Note: it is the developers responsiblilty to provide the AJAX action to return choices.
        [Display(Name = ""Single AJAX Selection"", GroupName = ""Basic ajax"", Order = 4)]
        [Bindable]
        [AjaxSelection(""GetAutoCompleteItems"")]
        public SelectList SingleServerSideSelection { get; set; }""

        [AjaxOnly]
        public JsonResult GetAutoCompleteItems(string text, int page)
        {
            var result = AdwService.GetListCodes(""AZ1"").ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }");

                return content;
            }
        }

        [Display(Name = "Single AJAX Selection", GroupName = "Basic ajax", Order = 4)]
        [Bindable]
        [AjaxSelection("GetAutoCompleteItems")]
        public SelectList SingleServerSideSelection { get; set; }

        [Display(GroupName = "Basic ajax", Order = 7)]
        public ContentViewModel ContentForServerSideMulti
        {
            get
            {
                var content = new ContentViewModel()
                    .AddLineBreak()
                    .AddPreformatted(@"        /// An example of a select list that allows multiple selections and operates on the server side (i.e. it loads its choices via an AJAX call)
        /// Note: it is the developers responsiblilty to provide the AJAX action to return choices.
        [Display(Name = ""Multi select AJAX"", GroupName = ""Basic ajax"", Order = 8)]
        [Bindable]
        [AjaxSelection(""GetAutoCompleteItems"")]
        public MultiSelectList MultiServerSideSelection { get; set; }""

        [AjaxOnly]
        public JsonResult GetAutoCompleteItems(string text, int page)
        {
            var result = AdwService.GetListCodes(""AZ1"").ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }");

                return content;
            }
        }

        [Display(Name = "Multi select AJAX", GroupName = "Basic ajax", Order = 8)]
        [Bindable]
        [AjaxSelection("GetAutoCompleteItems")]
        public MultiSelectList MultiServerSideSelection { get; set; }


        [Display(GroupName = "Large data set", Order = 1)]
        public ContentViewModel ContentForDummyID
        {
            get
            {
                var content = new ContentViewModel()
                    .AddLineBreak()
                    .AddPreformatted(@"        /// An example of a large data set being returned from the server side via AJAX
        /// It only loads additional choices as are necessary
        [Display(Name = ""Dummy ID"", GroupName = ""Large data set"", Order = 2)]
        [Bindable]
        [AjaxSelection(""GetDummyID"")]
        public SelectList DummyID { get; set; }

        [AjaxOnly]
        public JsonResult GetDummyID(string text, int page)
        {
            var result = DummyService.FindAll(string.Empty).ToSelectList(m => m.DummyID.ToString(), m => string.Format(""{0} - {1}"", m.DummyID.ToString(), m.Name));

            return AjaxSelectionView(text, page, result);
        }");

                return content;
            }
        }

        /// <summary>
        /// Dummy ID.
        /// </summary>
        [Display(Name = "Dummy ID", GroupName = "Large data set", Order = 2)]
        [Bindable]
        [AjaxSelection("GetDummyID")]
        public SelectList DummyID { get; set; }

        [Display(GroupName = "Occupations", Order = 1)]
        public ContentViewModel ContentForOccupations
        {
            get
            {
                var content = new ContentViewModel()
                    .AddLineBreak()
                    .AddParagraph("An example of the [AjaxSelection] attribute with hierarchical dependencies. Note that this exmaple uses ADW data purely to demonstrate as ordinally you would use the [AdwSelection] attribute when working with ADW data.")
                    .AddParagraph("View Model properties")
                    .AddPreformatted(@"        [Display(GroupName = ""Occupations"", Order = 1, Name = ""Occupation Level 1"")]
        [AjaxSelection(""GetOccupationLevel1"")]
        [Bindable]
        public SelectList OccupationLevel1 { get; set; }

        [Display(GroupName = ""Occupations"", Order = 2, Name = ""Occupation Level 2"")]
        [AjaxSelection(""GetOccupationLevel2"", Parameters = new[] { ""OccupationLevel1"" })]
        [Bindable]
        public SelectList OccupationLevel2 { get; set; }

        [Display(GroupName = ""Occupations"", Order = 3, Name = ""Occupation Level 3"")]
        [AjaxSelection(""GetOccupationLevel3"", Parameters = new[] { ""OccupationLevel2"" })]
        [Bindable]
        public SelectList OccupationLevel3 { get; set; }
        ")
            .AddParagraph("As the OccupationLevel1 property is the root of the hierarchy, it is initially populated in the Controller Action.")
            .AddPreformatted(@"        public ActionResult AjaxSelection()
        {
            var model = new AjaxSelectionViewModel();

            model.OccupationLevel1 = AdwService.GetListCodes(""AZ1"").ToSelectList(m => m.Code, m => m.Description);

            return View(model);
        }")
            .AddParagraph("Which uses the following SERVER side AJAX methods for client-side population.")
            .AddPreformatted(@"        [AjaxOnly]
        public JsonResult GetOccupationLevel1(string text, int page)
        {
            var result = AdwService.GetListCodes(""AZ1"").ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        [AjaxOnly]
        public JsonResult GetOccupationLevel2(string text, int page, string occupationLevel1)
        {
            var result = AdwService.GetRelatedCodes(""AZ12"", occupationLevel1, true).ToCodeModelList().ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }

        [AjaxOnly]
        public JsonResult GetOccupationLevel3(string text, int page, string occupationLevel2)
        {
            var result = AdwService.GetRelatedCodes(""AZ24"", occupationLevel2, true).ToCodeModelList().ToSelectList(m => m.Code, m => m.Description);

            return AjaxSelectionView(text, page, result);
        }");

                return content;
            }
        }

        /// <summary>
        /// Occupation Level 1.
        /// </summary>
        [Display(GroupName = "Occupations", Order = 1, Name = "Occupation Level 1")]
        [AjaxSelection("GetOccupationLevel1")]
        [Bindable]
        public SelectList OccupationLevel1 { get; set; }

        /// <summary>
        /// Occupation Level 2.
        /// </summary>
        [Display(GroupName = "Occupations", Order = 2, Name = "Occupation Level 2")]
        [AjaxSelection("GetOccupationLevel2", Parameters = new[] { "OccupationLevel1" })]
        [Bindable]
        public SelectList OccupationLevel2 { get; set; }

        /// <summary>
        /// Occupation Level 3.
        /// </summary>
        [Display(GroupName = "Occupations", Order = 3, Name = "Occupation Level 3")]
        [AjaxSelection("GetOccupationLevel3", Parameters = new[] { "OccupationLevel2" })]
        [Bindable]
        public SelectList OccupationLevel3 { get; set; }
    }
}