using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.User
{
    [Group("Emulate", Description = "Department Emulate User")]
    [Button("Submit", "Emulate")]
    public class DepartmentEmulateUser
    {
            [Display(Name = "Service Provider User Id", Description = "Service Provider User Id", GroupName = "Emulate")]
            [Bindable]
            [Required(ErrorMessage = "The data you have entered is missing mandatory fields. You need to enter Service Provider ID and, either Job Number or Reason or Both")]    
            [StringLength(10)]
            public string UserId { get; set; }

            [StringLength(20)]
            [RequiredIf("Reason", ComparisonType.EqualTo, "", ErrorMessage = "The data you have entered is missing mandatory fields. You need to enter Service Provider ID and, either Job Number or Reason or Both")]
            [Display(Name = "Job Number", Description = "Job Number", GroupName = "Emulate")]
            [Bindable]
            public string JobNumber { get; set; }

            [StringLength(200)]
            [Display(Name = "Reason", Description = "Reason for emulate function", GroupName = "Emulate")]
            [RequiredIf("JobNumber", ComparisonType.EqualTo, "", ErrorMessage = "The data you have entered is missing mandatory fields. You need to enter Service Provider ID and, either Job Number or Reason or Both")]
            [Bindable]
            public string Reason { get; set; }
        }
}