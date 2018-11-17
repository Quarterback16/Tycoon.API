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
    [Group("Text boxes")]
    [Button("Submit", GroupName="Text boxes")]
    public class TextBoxesViewModel
    {
        #region Simple text box
        [Display(GroupName="Text boxes")]
        public ContentViewModel ContentForSimpleTextBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A simple text box. Note that the data will be preserved when the form is submitted.
        Its data can be accessed via: model.SimpleTextBox
        [Display(Name = ""Simple Text Box"", GroupName = ""Text boxes"")]
        [Bindable]
        public string SimpleTextBox { get; set; }");

                return content;
            }
        }

        [Display(Name = "Simple Text Box", GroupName = "Text boxes")]
        [Bindable]
        public string SimpleTextBox { get; set; }
        #endregion

        #region Multiline text box
        [Display(GroupName="Text boxes")]
        public ContentViewModel ContentForMultilineTextBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A multiline text box
        Its data can be accessed via: model.MultilineTextBox
        [Display(Name = ""Multi-line Text Box"", GroupName = ""Text boxes"")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Bindable]
        public string MultilineTextBox { get; set; }");

                return content;
            }
        }

        [Display(Name = "Multi-line Text Box", GroupName = "Text boxes")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Bindable]
        public string MultilineTextBox { get; set; }
        #endregion

        #region Integer text box
        [Display(GroupName = "Text boxes")]
        public ContentViewModel ContentForIntegerTextBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// An integer input type
        Its data can be accessed via: model.Integer
        [Display(Name = ""Integer Text Box"", GroupName = ""Text boxes"")]
        [Bindable]
        public int Integer { get; set; }");

                return content;
            }
        }

        [Display(Name = "Integer Text Box", GroupName = "Text boxes")]
        [Bindable]
        public int Integer { get; set; }
        #endregion

        #region Decimal text box
        [Display(GroupName = "Text boxes")]
        public ContentViewModel ContentForDecimalTextBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A decimal input type
        Its data can be accessed via: model.Decimal
        [Display(Name = ""Decimal Text Box"", GroupName = ""Text boxes"")]
        [Bindable]
        public int Decimal { get; set; }");

                return content;
            }
        }

        [Display(Name = "Decimal Text Box", GroupName = "Text boxes")]
        [Bindable]
        public decimal Decimal { get; set; }
        #endregion

        #region Hidden
        [Display(GroupName = "Text boxes")]
        public ContentViewModel ContentForHidden
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A hidden element that will be submitted when the form is posted.
        Its data can be accessed via: model.Hidden
        [Display(Name = ""Hidden element"", GroupName = ""Text boxes"")]
        [Hidden]
        [Bindable]
        public string Hidden { get; set; }");

                return content;
            }
        }

        [Display(Name = "Hidden element", GroupName = "Text boxes")]
        [Hidden]
        [Bindable]
        public string Hidden { get; set; }

        [Display(GroupName = "Text boxes")]
        public ContentViewModel HiddenExplanation
        {
            get
            {
                return (new ContentViewModel())
                    .AddParagraph("You can't see the hidden element, because it is, well, .... hidden. Trying viewing the source of this page to see the hidden value that I set.");
            }
        }
        #endregion

        #region Copy attribute
        [Display(GroupName = "Text boxes")]
        public ContentViewModel ContentForCopyBox
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// An input that copies its content from another input. The value will only be copied after a change on Foo has been completed (e.g. if you TAB out of Foo).
        [Display(Name = ""Foo"", GroupName = ""Text boxes"")]
        [Bindable]
        public string Foo { get; set; }

        [Display(Name = ""Copy of Foo"", GroupName = ""Text boxes"")]
        [Bindable]
        [Copy(""Foo"")]
        public string CopyOfFoo { get; set; }
");

                return content;
            }
        }

        [Display(Name = "Foo", GroupName = "Text boxes")]
        [Bindable]
        public string Foo { get; set; }

        [Display(Name = "Copy of Foo", GroupName = "Text boxes")]
        [Bindable]
        [Copy("Foo")]
        public string CopyOfFoo { get; set; }
        #endregion

    }
}