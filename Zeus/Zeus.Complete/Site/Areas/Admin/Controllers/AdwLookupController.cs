using Employment.Web.Mvc.Area.Admin.Mappers;
using Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Employment.Web.Mvc.Area.Admin.Controllers
{
    /// <summary>
    /// Adw Lookup controlller.
    /// </summary>
    [Security(AllowAny = true)]
    public class AdwLookupController : InfrastructureController
    {


        protected readonly IAdwAdminService AdwAdminService;


        /// <summary>
        /// Adw Lookup Controller constructor.
        /// </summary>
        /// <param name="adwAdminService">Adw Admin service.</param>
        /// <param name="adwService">Adw Service.</param>
        /// <param name="userService">User Service.</param>
        public AdwLookupController(IAdwAdminService adwAdminService, IAdwService adwService, IUserService userService)
            : base(userService, adwAdminService)
        {
            if (adwAdminService == null)
            {
                throw new ArgumentNullException("adwAdminService", "Adw Admin Service cannot be null.");
            }

            AdwAdminService = adwAdminService;
        }


        /// <summary>
        /// "Adw Lookup" Index.
        /// </summary>
        /// <returns></returns>
        [Menu("ADW Lookups", Order = 1)]
        public ActionResult Index()
        {
            var model = new AdwLookupIndexViewModel();

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("Adw Lookup").AddParagraph("Select from the following:");




            return View(model);
        }

        /// <summary>
        /// Search List code type.
        /// </summary>
        /// <returns></returns>
        [Menu("ListCodeType", Order = 2, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListCodeType(string startswith, string listtype, bool exactlookup = false, int maxrows = 0)
        {
            PageTitle = "List Code Type";
            var model = new ListCodeTypeViewModel();
            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Code Type").AddParagraph("List all code table types. If an optional start type is entered, the list will start with this table type.");
            
            // TODO: POST - REDIRECT - GET
            if(!string.IsNullOrEmpty(startswith) && listtype!=null)
            {
                model.StartFromTableType = startswith;
                model.ListType = listtype;
                model.MaxRows = maxrows;
                model.ExactLookup = exactlookup;              

                var metadata = model.ToListCodeTypePageMetadata();

                model.Results = GetListCodeType(metadata);

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }

                
            }

            

            return View(model);
        }

        private IPageable<CodeTypeViewModel> GetListCodeType(ListCodeTypePageMetadata metadata)
        {
            var selectedCodeType = metadata.ListType;
            char listTypeChar = '\0';
            if (!string.IsNullOrEmpty(selectedCodeType))
            {
                char.TryParse(selectedCodeType, out listTypeChar);
            }
            var codeModelList = AdwAdminService.GetListCodeTypes(metadata.StartsWith, listTypeChar, metadata.ExactLookup, metadata.MaxRows);

            // Populate grid property with data and page metadata. Note that this mapping takes both source and destination.
            // New list is the pageable instance WITH PAGE METADATA is used as destination object TO KEEP Page METADATA.

            return  codeModelList.ToPageableCodeTypeModelList(new Pageable<CodeTypeViewModel>(metadata));
        }


        [AjaxOnly]
        public ActionResult SearchNextCodeType(ListCodeTypePageMetadata metadata)
        {
            IPageable<CodeTypeViewModel> results = new Pageable<CodeTypeViewModel>();

            if (metadata != null)
            {
                results = GetListCodeType(metadata);                
            }

            return PagedView(results);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListCodeType(ListCodeTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var route = new RouteValueDictionary();
                route.Add("startswith", model.StartFromTableType);
                route.Add("listtype", model.ListType ?? "C");
                route.Add("exactlookup", model.ExactLookup);
                route.Add("maxrows", model.MaxRows);

                //bool currentCodesOnly = false;
                //currentCodesOnly = !string.IsNullOrEmpty(selectedCodeType) && selectedCodeType.StartsWith("c") ? true : false;


                // Following Post Redirect Get pattern
                return RedirectToAction("ListCodeType", route);
            }

            return View(model);
        }

        /// <summary>
        /// List Code.
        /// </summary>
        /// <returns></returns>
        [Menu("ListCode", Order = 3, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListCode(string tabletype, string startcode, string listtype, bool exact = false, int max = 0, bool type = false)
        {
            PageTitle = "List Code";
            var model = new ListCodeViewModel();          

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Code").AddParagraph("List codes for a specified table type. If an optional code type is entered, it will be the starting value for the list or the code used when performing an exact lookup.");

            if (!string.IsNullOrEmpty(tabletype))
	        {
                
                model.TableType = tabletype;
                model.StartFromTableType = startcode;
                model.ListType = listtype;
                model.ExactLookup = exact;
                model.MaxRows = max;
                model.Type = type;
                bool currentCodesOnly = GetCurrentCodeOnlyFromListType(listtype);
                var results = AdwAdminService.GetListCodes(model.TableType, model.StartFromTableType, currentCodesOnly, exact, max, type);
                model.Results = results.ToPageableCodeTypeViewModelList();

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }
	        }

            return View(model);
        }


        private bool GetDominantFromSearchTable(string searchTable)
        {
            bool dominant = true;

            if(!string.IsNullOrEmpty(searchTable))
            {
                switch(searchTable.ToLower())
                {
                    case "d":
                        dominant = true;
                        break;
                    case "s":
                        dominant = false;
                        break;
                    default:
                        break;
                }
            }

            return dominant;
        }


        private bool GetCurrentCodeOnlyFromListType(string listtype)
        {
            bool currentCodesOnly = false;
            if (!string.IsNullOrEmpty(listtype))
            {
                switch (listtype)
                {
                    case "A":
                        currentCodesOnly = false;
                        break;
                    case "C":
                        currentCodesOnly = true;
                        break;
                    case "E":
                        currentCodesOnly = false; // TODO: not sure IF to process this.
                        break;
                }
            }
            return currentCodesOnly;
        }

        /// <summary>
        /// List Code.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListCode(ListCodeViewModel model)
        {
            if(ModelState.IsValid)
            {
                var routeValues = new RouteValueDictionary();
                routeValues.Add("tabletype", model.TableType);
                routeValues.Add("startcode", model.StartFromTableType);
                routeValues.Add("listtype", model.ListType);
                routeValues.Add("exact", model.ExactLookup);
                routeValues.Add("max", model.MaxRows);
                routeValues.Add("type", model.Type);

                return RedirectToAction("ListCode", routeValues);
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Menu("ListRelatedCodeType", Order = 4, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListRelatedCodeType(string starttype, string listtype, bool exact = false, int max = 0)
        {
            PageTitle = "List Related Code Type";

            var model = new ListRelatedCodeTypeViewModel();

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List RelatedCode Type").AddParagraph("List all related code table types. The list will begin with the start related code type if one is entered.");

            if(!string.IsNullOrEmpty(listtype)) // startType can be null: !string.IsNullOrEmpty(starttype) &&
            {
                model.StartRelatedTableType = starttype;
                model.ListType = listtype;
                model.ExactLookup = exact;
                model.MaxRows = max;

                var listTypeChar = '\0';
                char.TryParse(listtype, out listTypeChar);
                

                var results = AdwAdminService.GetRelatedListCodeTypes(model.StartRelatedTableType, listTypeChar, model.ExactLookup, model.MaxRows);

                model.Results = results.ToPagableRelatedCodeTypeViewModel();

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListRelatedCodeType(ListRelatedCodeTypeViewModel model)
        {
            if(ModelState.IsValid)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                
                if(!string.IsNullOrEmpty(model.StartRelatedTableType))
                {
                    routeValues.Add("starttype", model.StartRelatedTableType);
                }
                routeValues.Add("listtype", model.ListType ?? "C");
                routeValues.Add("exact", model.ExactLookup);
                routeValues.Add("max", model.MaxRows);

                return RedirectToAction(actionName: "ListRelatedCodeType", routeValues: routeValues);
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Menu("ListRelatedCode", Order = 5, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListRelatedCode(string relatedtable, string startcode, string listtype,string searchtable, bool exact = false, int max = 0)// string wstype, 
        {
            PageTitle = "List Related Code";

            var model = new ListRelatedCodeViewModel();

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Related Code").AddParagraph("List all related codes, dominant and subordinate, for a specified related table type. A start from dominant or subordinate code can be entered.");

            if(!string.IsNullOrEmpty(relatedtable))
            {
                model.RelatedTableType = relatedtable;
                model.StartFromTableType = startcode;
                model.ListType = listtype;
                //model.WsType = wstype;
                model.SearchTable = searchtable;
                model.ExactLookup = exact;
                model.MaxRows = max;

                var results = AdwAdminService.GetRelatedCodes(model.RelatedTableType, model.StartFromTableType, GetDominantFromSearchTable(model.SearchTable), GetCurrentCodeOnlyFromListType(listtype), model.ExactLookup, model.MaxRows);

                model.Results = results.ToPageableRelatedCodeViewModel();

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }
            }



            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListRelatedCode(ListRelatedCodeViewModel model)
        {
            if(ModelState.IsValid)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues.Add("relatedtable", model.RelatedTableType);
                routeValues.Add("startcode", model.StartFromTableType);
                routeValues.Add("listtype", model.ListType);
                //routeValues.Add("wstype", model.WsType);
                routeValues.Add("searchtable", model.SearchTable);
                routeValues.Add("exact", model.ExactLookup);
                routeValues.Add("max", model.MaxRows);

                return RedirectToAction("ListRelatedCode", routeValues);
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Menu("ListPropertyType", Order = 6, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListPropertyType(string startfrom, string startfromproperty, string listtype, bool exact = false, int max = 0)
        {
            PageTitle = "List Property Type";

            var model = new ListPropertyTypeViewModel();

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Property Type").AddParagraph("List all property types. If an optional start from type is entered, the list will start with this type. If a table type is entered, a starting property type can be entered.");

            if (!string.IsNullOrEmpty(startfrom) || !string.IsNullOrEmpty(startfromproperty) || !string.IsNullOrEmpty(listtype) )
            {
                model.StartFromTableType = startfrom; 
                model.StartFromProperty = startfromproperty;
                model.ExactLookup = exact;
                model.MaxRows = max;
                model.ListType = listtype;
                var listTypeChar = '\0';
                char.TryParse(listtype, out listTypeChar);
                var results = AdwAdminService.GetPropertyTypeList(model.StartFromTableType, model.StartFromProperty, listTypeChar, model.ExactLookup, model.MaxRows);

                model.Results = results.ToPageablePropertyViewModel();

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListPropertyType(ListPropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("startfrom", model.StartFromTableType);
                routeValueDictionary.Add("startfromproperty", model.StartFromProperty);
                routeValueDictionary.Add("listtype", model.ListType);
                routeValueDictionary.Add("exact", model.ExactLookup);
                routeValueDictionary.Add("max", model.MaxRows);

                return RedirectToAction("ListPropertyType", routeValueDictionary);
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Menu("ListProperty", Order = 7, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListProperty(string startfrom, string startcode, string startproperty, string listtype, bool exact = false, int max = 0)
        {
            PageTitle = "List Property";

            var model = new ListPropertyViewModel();

            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Property").AddParagraph("List all codes and property types and values for a table. If a table type is entered, a code can be entered. If a type and code is entered then a property type can be entered.");

            if (!string.IsNullOrEmpty(startfrom) || !string.IsNullOrEmpty(startcode) || !string.IsNullOrEmpty(startproperty) || !string.IsNullOrEmpty(listtype))
            {

                model.StartFromTableType = startfrom;
                model.StartFromCode = startcode;
                model.StartFromProperty = startproperty;
                model.ExactLookup = exact;
                model.MaxRows = max;
                model.ListType = listtype;
                var listTypeChar = '\0';
                char.TryParse(listtype, out listTypeChar);
                var results = AdwAdminService.GetPropertyList(model.StartFromTableType, model.StartFromCode, model.StartFromProperty, listTypeChar, model.ExactLookup, model.MaxRows);
                model.Results = results.ToPageablePropertyViewModel();

                if (model.Results == null || (model.Results != null && !model.Results.Any()))
                {
                    AddInformationMessage("No results returned.");
                }
            }


            return View(model);
        }


        /// <summary>
        /// List Property.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListProperty(ListPropertyViewModel model)
        {
            if(ModelState.IsValid)
            {
                var routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("startfrom", model.StartFromTableType);
                routeValueDictionary.Add("startcode", model.StartFromCode);
                routeValueDictionary.Add("startproperty", model.StartFromProperty);
                routeValueDictionary.Add("listtype", model.ListType);
                routeValueDictionary.Add("exact", model.ExactLookup);
                routeValueDictionary.Add("max", model.MaxRows); 

                return RedirectToAction("ListProperty", routeValueDictionary);
            }

            return View(model);
        }




        /// <summary>
        /// List Deltas.
        /// </summary>
        /// <returns></returns>
        [Menu("ListDeltas", Order = 8, ParentAction = "Index")]
        [HttpGet]
        public ActionResult ListDeltas(string code, string startcode, DateTime? lastupdatedate, int max = 0)
        {
            PageTitle = "List Deltas";

            var model = new ListDeltasViewModel();
            model.Overview = new Infrastructure.ViewModels.ContentViewModel().AddTitle("List Deltas").AddParagraph("List Deltas for a specified table type. This allows testing the various types of Delta web services directly i.e. bypass the use of CommonComponentsOne.");

            if(!string.IsNullOrEmpty(code))
            {
                model.CodeType = code;
                model.StartCode = startcode;
                model.MaxRows = max;
                model.LastUpdateDate = lastupdatedate != null ? lastupdatedate.Value : UserService.DateTime;

                var results = AdwAdminService.GetDeltaList(model.CodeType, model.StartCode, model.LastUpdateDate, maxRows: model.MaxRows);

                model.Results = results.ToPageableDeltaViewModel();

                if(model.Results == null || (model.Results !=null && !model.Results.Any() ))
                {
                    AddInformationMessage("No results returned.");
                }
            }




            return View(model);
        }


        /// <summary>
        /// List Deltas Post action.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListDeltas(ListDeltasViewModel model)
        {
            if (ModelState.IsValid)
            {
                var routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("code", model.CodeType);
                routeValueDictionary.Add("startcode", model.StartCode);
                routeValueDictionary.Add("lastupdatedate", model.LastUpdateDate);
                routeValueDictionary.Add("max", model.MaxRows); 

                return RedirectToAction("ListDeltas", routeValueDictionary);
            }

            return View(model);
        }
    }
}