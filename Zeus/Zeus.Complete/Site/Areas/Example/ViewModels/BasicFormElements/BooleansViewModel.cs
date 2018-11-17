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
    [Group("Booleans")]
    [Button("Submit", GroupName = "Booleans")]
    public class BooleansViewModel
    {
        #region Basic Boolean
        /// <summary>
        /// Content for describing <see cref="BasicBoolean" />.
        /// </summary>
        [Display(GroupName = "Booleans", Order = 9)]
        public ContentViewModel ContentForBasicBoolean
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A simple boolean select example
        Its value can be accessed with model.BasicBoolean
        [Display(GroupName = ""Booleans"", Order = 10, Name = ""Basic boolean"")]
        [Bindable]
        public bool BasicBoolean { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A simple boolean select example
        /// </summary>
        [Display(GroupName = "Booleans", Order = 10, Name = "Basic boolean")]
        [Bindable]
        public bool BasicBoolean { get; set; }
        #endregion

        #region Custom Text Switcher
        /// <summary>
        /// Content for describing <see cref="CustomTextSwitcherBoolean" />.
        /// </summary>
        [Display(GroupName = "Booleans", Order = 19)]
        public ContentViewModel ContentForCustomTextSwitcherBoolean
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A switcher boolean select with custom text example
        Its value can be accessed with model.CustomTextSwitcherBoolean
        [Display(GroupName = ""Booleans"", Order = 20, Name = ""Custom Text Switcher boolean"")]
        [Switcher(""On"", ""Off"")]
        [Bindable]
        public bool CustomTextSwitcherBoolean { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A boolean select with custom text example
        /// </summary>
        [Display(GroupName = "Booleans", Order = 20, Name = "Custom Text Switcher boolean")]
        [Switcher("On", "Off")]
        [Bindable]
        public bool CustomTextSwitcherBoolean { get; set; }
        #endregion

        #region Checkbox boolean
        /// <summary>
        /// Content for describing <see cref="Checkbox Boolean" />.
        /// </summary>
        [Display(GroupName = "Booleans", Order = 29)]
        public ContentViewModel ContentForCheckboxBoolean
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A boolean select using a checkbox example
        Its value can be accessed with model.CheckboxBoolean
        [Display(GroupName = ""Booleans"", Order = 30, Name = ""Checkbox boolean"")]
        [DataType(CustomDataType.CheckBoxList)]
        [Bindable]
        public bool CheckboxBoolean { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A boolean select using a checkbox example
        /// </summary>
        [Display(GroupName = "Booleans", Order = 30, Name = "Checkbox boolean")]
        [DataType(CustomDataType.CheckBoxList)]
        [Bindable]
        public bool CheckboxBoolean { get; set; }
        #endregion

        #region Nullable boolean
        /// <summary>
        /// Content for describing <see cref="NullableBoolean" />.
        /// </summary>
        [Display(GroupName = "Booleans", Order = 39)]
        public ContentViewModel ContentForNullableBoolean
        {
            get
            {
                var content = new ContentViewModel()
                    .AddPreformatted(@"        /// A simple nullable boolean select example
        Its value can be accessed with model.NullableBoolean
        [Display(GroupName = ""Booleans"", Order = 40, Name = ""Nullable boolean"")]
        [Bindable]
        public bool? NullableBoolean { get; set; }");

                return content;
            }
        }

        /// <summary>
        /// A simple nullable boolean select example
        /// </summary>
        [Display(GroupName = "Booleans", Order = 40, Name = "Nullable boolean")]
        [Bindable]
        public bool? NullableBoolean { get; set; }
        #endregion

    }
}