using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using ReadOnlyAttribute = System.ComponentModel.ReadOnlyAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Workflow
{
    [Serializable]
    [Group("User details", Order = 1)]
    [Group("Contact details", Order = 2)]
    public class Step3ViewModel
    {
        [ReadOnly(true)]
        [Display(GroupName = "User details", Order = 1, Name = "Name")]
        public string Name { get; set; }

        [ReadOnly(true)]
        [Hidden]
        [Display(GroupName = "User details", Name = "Occupation Level 1")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "AZ1")]
        public string OccupationLevel1 { get; set; }

        [ReadOnly(true)]
        [Hidden]
        [Display(GroupName = "User details", Name = "Occupation Level 2")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ12", DependentProperty = "OccupationLevel1", Dominant = true)]
        public string OccupationLevel2 { get; set; }

        [ReadOnly(true)]
        [Display(GroupName = "User details", Order = 2, Name = "Your occupation")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ24", DependentProperty = "OccupationLevel2", Dominant = true)]
        public string OccupationLevel3 { get; set; }


        [ReadOnly(true)]
        [Hidden]
        public bool ShowEmailAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Email address", Order = 3, GroupName = "Contact details")]
        [DataType(DataType.EmailAddress)]
        [VisibleIfTrue("ShowEmailAddress")]
        public string EmailAddress { get; set; }

        [ReadOnly(true)]
        [Hidden]
        public bool ShowPostalAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Postal address", Order = 3, GroupName = "Contact details")]
        [VisibleIfTrue("ShowPostalAddress")]
        public string PostalAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "State", Order = 4, GroupName = "Contact details")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "STT")]
        [Row("Location", RowType.Third)]
        [VisibleIfTrue("ShowPostalAddress")]
        public string State { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Postcode", Order = 5, GroupName = "Contact details")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "PCC")]
        [Row("Location", RowType.Third)]
        [VisibleIfTrue("ShowPostalAddress")]
        public string Postcode { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Suburb", Order = 6, GroupName = "Contact details")]
        [Row("Location", RowType.Third)]
        [VisibleIfTrue("ShowPostalAddress")]
        public string Suburb { get; set; }
    }
}