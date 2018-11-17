using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.BasicFormElements
{
    [Group("Date / Times")]
    [Button("Submit", GroupName = "Date / Times")]
    public class DateTimesViewModel
    {
        #region Date picker
        [Display(GroupName = "Date / Times", Order = 1)]
        public ContentViewModel ContentForDatePicker
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A date picker. Use this for selecting dates that do not need to be attached to times.
        /// Its data can be access via the standard DateTime properties, such as:
        /// model.DatePicker.Day, model.DatePicker.Month, model.DatePicker.Year, model.DatePicker.DayOfWeek, etc.
        /// The DateTime structure can hold time information as well, but that component can be disregarded in this instance.
        [DataType(DataType.Date)]
        [Display(Name = ""Date Picker"", GroupName = ""Date / Times"")]
        [Bindable]
        public DateTime DatePicker { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = ""Nullable Date Picker"", GroupName = ""Date / Times"")]
        [Bindable]
        public DateTime? NullableDatePicker { get; set; }");

                return content;
            }
        }

        [Display(Name = "Date Picker", GroupName = "Date / Times", Order = 2)]
        [DataType(DataType.Date)]
        [Bindable]
        public DateTime DatePicker { get; set; }

        [Display(Name = "Nullable Date Picker", GroupName = "Date / Times", Order = 3)]
        [DataType(DataType.Date)]
        [Bindable]
        public DateTime? NullableDatePicker { get; set; }
        #endregion

        #region Date / time picker
        [Display(GroupName = "Date / Times", Order = 4)]
        public ContentViewModel ContentForDateTimePicker
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A date AND time picker. Use this for selecting a specific time on a specific date.
        /// Its data can be access via the standard DateTime properties, such as:
        /// model.DateTimePicker.DayOfWeek, model.DateTimePicker.Day, model.DateTimePicker.Month, model.DateTimePicker.Hour, model.DateTimePicker.Minute, etc.
        [Display(Name = ""Date Picker"", GroupName = ""Date / Times"")]
        [DataType(DataType.DateTime)]
        [Bindable]
        public DateTime DateTimePicker { get; set; }

        [Display(Name = ""Nullable Date Picker"", GroupName = ""Date / Times"")]
        [DataType(DataType.DateTime)]
        [Bindable]
        public DateTime? NullableDateTimePicker { get; set; }");

                return content;
            }
        }

        [Display(Name = "Date Time Picker", GroupName = "Date / Times", Order = 5)]
        [DataType(DataType.DateTime)]
        [Bindable]
        public DateTime DateTimePicker { get; set; }

        [Display(Name = "Nullable Date Time Picker", GroupName = "Date / Times", Order = 6)]
        [DataType(DataType.DateTime)]
        [Bindable]
        public DateTime? NullableDateTimePicker { get; set; }
        #endregion

        #region Nullable Date / time picker
        [Display(GroupName = "Date / Times", Order = 7)]
        public ContentViewModel ContentForNullableDateTimePicker
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A time picker. Use this for selecting times that do not need to be attached to a date.
        /// Its data can be access via the standard DateTime properties, such as:
        /// model.TimePicker.Hour, model.TimePicker.Minute, model.TimePicker.Second, model.TimePicker.Millisecond, etc.
        /// The DateTime structure can hold date information as well, but that component can be disregarded in this instance.
        [Display(Name = ""Time Picker"", GroupName = ""Date / Times"")]
        [DataType(DataType.Time)]
        [Bindable]
        public DateTime TimePicker { get; set; }

        [Display(Name = ""Nullable Time Picker"", GroupName = ""Date / Times"")]
        [DataType(DataType.Time)]
        [Bindable]
        public DateTime? NullableTimePicker { get; set; }");

                return content;
            }
        }

        [Display(Name = "Time Picker", GroupName = "Date / Times", Order = 8)]
        [DataType(DataType.Time)]
        [Bindable]
        public DateTime TimePicker { get; set; }

        [Display(Name = "Nullable Time Picker", GroupName = "Date / Times", Order = 9)]
        [DataType(DataType.Time)]
        [Bindable]
        public DateTime? NullableTimePicker { get; set; }
        #endregion
    }
}