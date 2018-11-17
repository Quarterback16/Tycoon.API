using System;

namespace ProgramAssuranceTool.Helpers
{
    public class EditLabelAttribute : Attribute
    {
        private readonly string editLabel;

        public EditLabelAttribute(string editLabelValue)
        {
            editLabel = editLabelValue;
        }

        public string GetEditLabel()
        {
            return editLabel;
        }
    }
}