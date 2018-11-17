using System;

namespace ProgramAssuranceTool.Helpers
{
	public class AdwCodeListAttribute : Attribute
	{
		private readonly bool emptyValue;
		private readonly string adwCode;
		private readonly string adwRelatedCode;
		private readonly string adwSearchCode;
		private readonly bool isRelatedCode;
		private readonly bool goLong;

		public AdwCodeListAttribute( string adwCodeName, bool showEmptyValue, bool bGoLong )
		{
			adwCode = adwCodeName;
			emptyValue = showEmptyValue;
			goLong = bGoLong;
		}

		public AdwCodeListAttribute( string adwRelatedCodeName, string searchCode, bool showEmptyValue )
		{
			isRelatedCode = true;
			adwRelatedCode = adwRelatedCodeName;
			adwSearchCode = searchCode;
			emptyValue = showEmptyValue;
		}

		public string AdwCode
		{
			get { return adwCode; }
		}

		public string AdwRelatedCode
		{
			get { return adwRelatedCode; }
		}

		public string AdwSearchCode
		{
			get { return adwSearchCode; }
		}

		public bool ShowEmptyValue
		{
			get { return emptyValue; }
		}

		public bool IsRelatedCode
		{
			get { return isRelatedCode; }
		}

		public bool GoLong
		{
			get { return goLong; }
		}
	}
}
