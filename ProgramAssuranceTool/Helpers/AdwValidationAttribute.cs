using System;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AdwValidationAttribute : ValidationAttribute
    {
        public static AdwRepository Adw = new AdwRepository();

        public AdwValidationAttribute(string tableCode)
        {
            CodeType = tableCode;
        }

        public string CodeType { get; private set; }

        public override bool IsValid(object value)
        {
            return value != null && Adw.IsCodeValid(CodeType, value.ToString());
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The value for {0} is not valid.", name);
        }
    }

}