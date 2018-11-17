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
    [Group("Submit buttons")]
    [Button("No Submit type", "Submit buttons", Primary=true, Order = 10)]
    [Button("Custom submit one", "Submit buttons", SubmitType = "one", Order = 20)]
    [Button("Custom submit two", "Submit buttons", SubmitType = "two", Order = 30)]
    // The custom action types can't be mixed with regular button submits. Not including them for now. The intention with these is that they can be handled by a named action (such as SubmitButtonsActionOne) that has the [ButtonHandler] attribute
    //[Button("Custom action one", "Submit buttons", SubmitType = "SubmitButtonsActionOne", Order = 40)]
    //[Button("Custom action two", "Submit buttons", SubmitType = "SubmitButtonsActionTwo", Order = 50)]
    [Button("Cancel", "Submit buttons", Cancel = true, Order = 60)]
    [Button("Clear", "Submit buttons", Clear = true, Order = 70)]
    public class SubmitButtonsViewModel
    {
        [Display(GroupName = "Submit buttons", Order = 10)]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"Submit buttons are used to post data back to the application. They can be differentiated by giving them different values for the 'SubmitType' parameter. If this is not supplied, then the name of the button is used")
                    //.AddParagraph(@"Additionally, a custom action handler can be defined that matches the SubmitType automatically. This requires using the ""ButtonHandler"" attribute")
                    .AddParagraph(@"The 'Primary' attribute is used to give a primary styling to a button.")
                    .AddParagraph(@"The 'Cancel' attribute is used to make a button that is the same as a regular submit button, but floated to the left.")
                    .AddParagraph(@"The 'Clear' attribute is used to make a button clear the current form.")
                    .AddPreformatted(@"
    public class CustomViewModel
    {
        ......
    }");
            }
        }

        /// <summary>
        /// Content describing which button was selected and which action handled it.
        /// </summary>
        [Display(GroupName = "Submit buttons", Order=30)]
        public ContentViewModel Result { get; set; }


        /// <summary>
        /// Data to work with.
        /// </summary>
        [Display(GroupName = "Submit buttons", Name="Some data", Order=20)]
        [Bindable]
        public string SomeData { get; set; }

    }
}