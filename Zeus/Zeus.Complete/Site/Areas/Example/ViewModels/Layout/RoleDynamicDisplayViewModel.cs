using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Layout
{
    [Group("Role checking", Order = 2)]
    [Button("Submit","true", Order = 1)]
    public class RoleDynamicDisplayViewModel
    {
        public RoleDynamicDisplayViewModel()
        {
            var deptRoles = new[] { "DAD", "DHD", "DES", "DEL", "DEU", "DVO" };
            var resolver = new IsInRoleValueResolver(deptRoles);
            CanEdit = resolver.Resolve(null);
        }

        [Display(Name = "Role checking", Order = 1, GroupName = "Role checking")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("When dealing with properties that shuold be modified based on security roles (e.g. a property only being editable by Deptartmental users), the attributes such as [RequiredIf], [VisibleIf], [EditableIf] are still useful when used in conjunction with ValueResolvers.")
                    .AddParagraph("For this case, simply use a hidden property to hold the value as returned by the Value Resolver")
                    .AddParagraph("In the example below, if you have a department role, the property will be editable")
                    .AddPreformatted(@" 
    [Group(""Role checking"", Order = 2)]
    [Button(""Submit"",""true"", Order = 1)]
    public class RoleDynamicDisplayViewModel
    {
        public RoleDynamicDisplayViewModel()
        {
            var deptRoles = new[] { ""DAD"", ""DHD"", ""DES"", ""DEL"", ""DEU"", ""DVO"" };
            var resolver = new IsInRoleValueResolver(deptRoles);
            CanEdit = resolver.Resolve(null);
        }

        [Display(Name = ""Property 1 (Editable if CanEdit = true)"", Order = 1, GroupName = ""Role checking"")]
        [Bindable]
        [Required]
        [EditableIfTrue(""CanEdit"")]
        public string Property1 { get; set; }

        [Bindable]
        [Hidden]
        [Editable(false, AllowInitialValue = true)]
        public bool CanEdit { get; set; }
    }
")
                ;
            }
        }

        [Display(Name = "Property 1 (Editable if CanEdit = true)", Order = 1, GroupName = "Role checking")]
        [Bindable]
        [EditableIfTrue("CanEdit")]
        public string Property1 { get; set; }

        [Bindable]
        [Hidden]
        [Editable(false, AllowInitialValue = true)]
        public bool CanEdit { get; set; }
    }
}