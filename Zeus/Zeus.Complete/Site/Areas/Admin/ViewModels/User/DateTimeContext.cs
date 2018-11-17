using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.User
{
    [Button("Submit", "Date")]
    [Group("Date", Description = "User context current date")]
    public class DateTimeContext
    {
        [Display(Name = "Date (dd/mm/yyyy)", Description = "Value of the current user context date for system testing", GroupName = "Date")]
        [Bindable]
        [Required(ErrorMessage = "Date - The field Date must be in the date format dd/mm/yyyy")]
        [DataType(DataType.Date)]
        public DateTime Current { get; set; }
    }
}