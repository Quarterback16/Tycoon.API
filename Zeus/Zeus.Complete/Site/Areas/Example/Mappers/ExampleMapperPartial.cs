using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.Calendar;
using Employment.Web.Mvc.Area.Example.ViewModels.Grid;
using Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.Mappers
{


    /// <summary>
    /// Represents a mapper that is used to map between View Models and Domain Models.
    /// </summary>
    public static partial class ExampleMapper
    {

        /// <summary>
        /// Extension method that returns an instance of <see cref="AppointmentViewModel"/> from <see cref="CategoryItemViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel">Category Item View Model instance.</param>
        /// <returns>Instance of AppointmentViewModel.</returns>
        public static AppointmentViewModel ToAppointmentViewModel(this CategoryItemViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
            {
                return null;
            }

            return new AppointmentViewModel()
            {
                AllDay = categoryViewModel.AllDay,
                Description = categoryViewModel.Description,
                End = categoryViewModel.End,
                Id = categoryViewModel.Id,
                Start = categoryViewModel.Start,
                Title = categoryViewModel.Title
            };

        }

        /// <summary>
        /// Extension method that returns an instance of <see cref="EventViewModel"/> from <see cref="CategoryItemViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel">Category Item View Model instance.</param>
        /// <returns>Instance of EventViewModel.</returns>
        public static EventViewModel ToEventViewModel(this CategoryItemViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
            {
                return null;
            }

            return new EventViewModel()
            {
                AllDay = categoryViewModel.AllDay,
                Description = categoryViewModel.Description,
                End = categoryViewModel.End,
                Id = categoryViewModel.Id,
                Start = categoryViewModel.Start,
                Title = categoryViewModel.Title
            };

        }


        /// <summary>
        /// Extension method that returns an instance of <see cref="QAViewModel"/> from <see cref="CategoryItemViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel">Category Item View Model instance.</param>
        /// <returns>Instance of QAViewModel.</returns>
        public static QAViewModel ToQAViewModel(this CategoryItemViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
            {
                return null;
            }

            return new QAViewModel()
            {
                AllDay = categoryViewModel.AllDay,
                Description = categoryViewModel.Description,
                End = categoryViewModel.End,
                Id = categoryViewModel.Id,
                Start = categoryViewModel.Start,
                Title = categoryViewModel.Title
            };

        }


        /// <summary>
        /// Extension method that returns an instance of <see cref="CategoryItemViewModel"/> from <see cref="AppointmentViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel">Category Item View Model instance.</param>
        /// <param name="model">Appointment View Model instance.</param>
        /// <returns>Instance of AppointmentViewModel.</returns>
        public static CategoryItemViewModel ToCategoryItemViewModel(this AppointmentViewModel model, CategoryItemViewModel categoryViewModel = null)
        {
            if (categoryViewModel == null)
            {
                categoryViewModel = new CategoryItemViewModel();
            }

            if (model == null)
            {
                return null;
            }


            categoryViewModel.AllDay = model.AllDay;
            categoryViewModel.Description = model.Description;
            categoryViewModel.End = model.End;
            categoryViewModel.Id = model.Id;
            categoryViewModel.Start = model.Start;
            categoryViewModel.Title = model.Title;

            return categoryViewModel;
        }

        /// <summary>
        /// Extension method that returns an instance of <see cref="CategoryItemViewModel"/> from <see cref="EventViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel">Category Item View Model instance.</param>
        /// <param name="model">Event View Model instance.</param>
        /// <returns>Instance of EventViewModel.</returns>
        public static CategoryItemViewModel ToCategoryItemViewModel(this EventViewModel model, CategoryItemViewModel categoryViewModel = null)
        {
            if (categoryViewModel == null)
            {
                categoryViewModel = new CategoryItemViewModel();
            }

            if (model == null)
            {
                return null;
            }

            categoryViewModel.AllDay = model.AllDay;
            categoryViewModel.Description = model.Description;
            categoryViewModel.End = model.End;
            categoryViewModel.Id = model.Id;
            categoryViewModel.Start = model.Start;
            categoryViewModel.Title = model.Title;

            return categoryViewModel;
        }



        /// <summary>
        /// Extension method that returns an instance of <see cref="CategoryItemViewModel"/> from <see cref="QAViewModel"/>.
        /// </summary>
        /// <param name="categoryViewModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static CategoryItemViewModel ToCategoryItemViewModel(this QAViewModel model, CategoryItemViewModel categoryViewModel = null)
        {
            if (categoryViewModel == null)
            {
                categoryViewModel = new CategoryItemViewModel();
            }

            if (model == null)
            {
                return null;
            }

            categoryViewModel.AllDay = model.AllDay;
            categoryViewModel.Description = model.Description;
            categoryViewModel.End = model.End;
            categoryViewModel.Id = model.Id;
            categoryViewModel.Start = model.Start;
            categoryViewModel.Title = model.Title;

            return categoryViewModel;
        }






        public static DateTime ParseToDateTime(string value)
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParse(value, out date);
            return date;
        }

        public static int ParseToInt(string value)
        {
            int number = 0;
            Int32.TryParse(value, out number);
            return number;
        }


        public static long ParseToLong(string value)
        {
            long longNumber;
            Int64.TryParse(value, out longNumber);
            return longNumber;
        }


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
        }



        public static ButtonEditViewModel ToButtonEditViewModel(ClaimWithButtonsViewModel model)
        {
            var dest = new ButtonEditViewModel();
            Map(model, dest);
            return dest;
        }

        public static void ToPageableGridSortingViewModel(IEnumerable<SortModel> source, Pageable<GridSortingViewModel> destination)
        {
            if (destination == null)
            {
                destination = new Pageable<GridSortingViewModel>();
            }

            foreach (var sortModel in source)
            {
                var gridSortingViewModel = new GridSortingViewModel();
                Map(sortModel, gridSortingViewModel);
                destination.Add(gridSortingViewModel);
            }
        }

        public static IEnumerable<ClaimWithButtonsViewModel> ToClaimWithButtonsViewModelList(IEnumerable<Claim> src)
        {
            var dest = new List<ClaimWithButtonsViewModel>();
            if (src != null)
            {
                foreach (var s in src)
                {
                    var claimWithButtonsViewModel = new ClaimWithButtonsViewModel();
                    Map(s, claimWithButtonsViewModel);
                    dest.Add(claimWithButtonsViewModel);
                }
            }
            return dest as IEnumerable<ClaimWithButtonsViewModel>;
        }


        public static IEnumerable<ClaimViewModel> ToClaimViewModelList(IEnumerable<Claim> src)
        {
            var dest = new List<ClaimViewModel>();

            if (src != null)
            {
                foreach (var s in src)
                {
                    dest.Add(MapToClaimViewModel(s));
                }
            }
            return dest as IEnumerable<ClaimViewModel>;
        }



        public static Pageable<DummyViewModel> ToPageableDummyViewModel(IEnumerable<DummyModel> src, Pageable<DummyViewModel> dest)
        {
            if (src != null)
            {
                foreach (var s in src)
                {
                    var dummyViewModel = new DummyViewModel();
                    Map(s, dummyViewModel);
                    dest.Add(dummyViewModel);
                }
            }
            return dest;
        }

    }
}