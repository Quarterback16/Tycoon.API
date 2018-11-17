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

namespace Employment.Web.Mvc.Area.Example.ViewModels.Validation
{
    [Group("Overview")]
    [Group("Properties")]
    [Group("Properties2")]
    [Button("Submit with errors", "Overview", SubmitType="errors")]
    [Button("Submit with success", "Overview", SubmitType="success")]
    public class ValidationMessagesViewModel
    {
        [Display(GroupName = "Overview")]
        public ContentViewModel ContentForOverview
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"Validation messages can be used to draw to the user's attention problems or other information. They are simply added to the page or individual property when desired. You can add Error, Warning, Success, or Information messages. Success messages will only be displayed if there are no errors. Press the submit buttons below to show the messages.")
                    .AddPreformatted(@"        [HttpPost]
        public ActionResult ValidationMessages(ValidationMessagesViewModel model, string submitType)
        {
            AddWarningMessage(""Warning --> you are on the Validation Page."");
            AddInformationMessage(""Info --> you are on the Validation Page."");

            AddWarningMessage(""FieldWithAWarning"", ""An error has occurred on 'Field with a warning'"");
            AddInformationMessage(""FieldWithInformation"", ""Some information about 'Field with information'"");
            AddInformationMessage(""FieldWithMoreInformation"", ""Some more information"");

            if (submitType == ""success"")
            {
                AddSuccessMessage(""Success --> you are on the Validation Page."");
                AddSuccessMessage(""FieldWithASuccess"", ""Success for the 'Field with a success'"");
            }
            else if (submitType == ""errors"")
            {
                AddErrorMessage(""Error --> you are on the Validation Page."");
                AddErrorMessage(""FieldWithAnError"", ""An error has occurred on 'Field with an error'"");
                AddErrorMessage(""FieldWithAnotherError"", ""Another error on that field"");
            }

            return View(model);
        }
")
                ;
                return content;
            }
        }

        [Display(GroupName = "Properties", Name = "Field with no message")]
        [Row("a", Infrastructure.Types.RowType.Half)]
        public string FieldWithNoMessage { get; set; }

        [Display(GroupName = "Properties", Name = "Field with an error")]
        [Row("a", Infrastructure.Types.RowType.Half)]
        public string FieldWithAnError { get; set; }

        [Display(GroupName = "Properties", Name = "Field with a warning")]
        [Row("b", Infrastructure.Types.RowType.Half)]
        public string FieldWithAWarning { get; set; }

        [Display(GroupName = "Properties", Name = "Field with information")]
        [Row("b", Infrastructure.Types.RowType.Half)]
        public string FieldWithInformation { get; set; }

        [Display(GroupName = "Properties", Name = "Field with another error")]
        [Row("d", Infrastructure.Types.RowType.Half)]
        public string FieldWithAnotherError { get; set; }

        [Display(GroupName = "Properties", Name = "Field that is also ok")]
        [Row("d", Infrastructure.Types.RowType.Half)]
        public string FieldThatIsAlsoOK { get; set; }

        [Display(GroupName = "Properties2", Name = "Field with a success")]
        [Row("c", Infrastructure.Types.RowType.Half)]
        public string FieldWithASuccess { get; set; }

        [Display(GroupName = "Properties2", Name = "Field with more information")]
        [Row("c", Infrastructure.Types.RowType.Half)]
        public string FieldWithMoreInformation { get; set; }

    }
}